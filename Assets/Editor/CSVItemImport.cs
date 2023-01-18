using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public static class CSVItemImport
{
    [MenuItem("Tools/Import CSV Items")]
    static void ImportAll()
    {
        Import<ItemSO>("Sheet - Item.csv", "ITEMS");
        Import<EnemySO>("Sheet - Enemy.csv", "ENEMY");
    }

    public static void Import<T>(string pCSVName, string pTargetFolderName) where T : ScriptableObject, IDataObject
    {
        if (!AssetDatabase.IsValidFolder($"{CSV.DATA_FILEPATH}/{pTargetFolderName}"))
        {
            AssetDatabase.CreateFolder($"{CSV.DATA_FILEPATH}", pTargetFolderName);
        }
        string[] CSVLines = File.ReadAllLines(Application.dataPath + CSV.CSV_FILEPATH + pCSVName);
        Debug.Log(string.Join(" - ", CSVLines));
        string[] CSVHeader = CSV.ParseHeader(CSVLines[0]);
        List<Dictionary<string, string>> Data = CSV.Parse(CSVLines, CSVHeader);

        foreach (Dictionary<string, string> ItemData in Data)
        {
            T item = ScriptableObject.CreateInstance<T>();

            //NOTE(Nighten) It is not possible to call a constructor with SO.CreateInstance()
            //Therefore, we need to create an init function to initialize our data
            //The object is responsible for how it will handle the Dictionary
            item.init(ItemData, CSVHeader);

            //NOTE(Nighten) Each object is also responsible for the way it treat wrong data
            //For example, we might want to check if the row is valid in the init() function
            //by simply checking if a mandatory variable ("name" for example) is not empty
            //  If so, the isValid flag is raised
            if (item.isValid)
                AssetDatabase.CreateAsset(item, $"{CSV.DATA_FILEPATH}/{pTargetFolderName}/{ItemData["name_en"]}.asset");

        }

        AssetDatabase.SaveAssets();
    }
}
