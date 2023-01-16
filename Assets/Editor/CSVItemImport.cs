using System.Collections;
using System.Collections.Generic;
using System.Linq; //Used for the List<>.Last() function
using UnityEngine;
using UnityEditor;
using System.IO;

public static class CSVItemImport
{
    static string CSV_FILEPATH = "/SCRIPT/DATA/CSV/";
    static string DATA_FILEPATH = "Assets/SCRIPT/DATA"; //Were the SO folder will be created

    [MenuItem("Tools/Import CSV Items")]
    static void ImportAll()
    {
        Import<ItemSO>("Sheet - Item.csv", "ITEMS");
        Import<EnemySO>("Sheet - Enemy.csv", "ENEMY");
    }



    static void Import<T>(string pCSVName, string pTargetFolderName) where T : ScriptableObject, IDataObject
    {
        if (!AssetDatabase.IsValidFolder($"{DATA_FILEPATH}/{pTargetFolderName}")){
            AssetDatabase.CreateFolder($"{DATA_FILEPATH}", pTargetFolderName);
        }

        List<Dictionary<string, string>> Data = CSVParser(File.ReadAllLines(Application.dataPath + CSV_FILEPATH + pCSVName));

        foreach (Dictionary<string, string> ItemData in Data)
        {
            T item = ScriptableObject.CreateInstance<T>();

            //NOTE(Nighten) It is not possible to call a constructor with SO.CreateInstance()
            //Therefore, we need to create an init function to initialize our data
            //The object is responsible for how it will handle the Dictionary
            item.init(ItemData);

            //NOTE(Nighten) Each object is also responsible for the way it treat wrong data
            //For example, we might want to check if the row is valid in the init() function
            //by simply checking if a mandatory variable ("name" for example) is not empty
            //  If so, the isValid flag is raised
            if (item.isValid)
                AssetDatabase.CreateAsset(item, $"{DATA_FILEPATH}/{pTargetFolderName}/{ItemData["name_en"]}.asset");

        }

        AssetDatabase.SaveAssets();
    }

    //NOTE(Nighten) We use a list of Dictionary so that if we add a column anywhere in the sheet,
    //              the SO constructor doesn't break
    static List<Dictionary<string, string>> CSVParser(string[] pData)
    {
        List<Dictionary<string, string>> Result = new List<Dictionary<string, string>>();
        
        string[] HeaderRow = pData[0].Split(",");
        List<string[]> DataRowsList = new List<string[]>(); 

        for (int i = 1; i < pData.Length; i++)
        {
            string[] row = pData[i].Split(",");

            //Don't add entries that are empty or missing column (like the last line)
            if (row.Length != HeaderRow.Length)
            {
                continue;
            }

            Result.Add(new Dictionary<string, string>());
            for (int i2 = 0; i2 < HeaderRow.Length; i2++)
            {
                Result.Last().Add(HeaderRow[i2], row[i2]);
            }

        }

        return Result;
    }
}
