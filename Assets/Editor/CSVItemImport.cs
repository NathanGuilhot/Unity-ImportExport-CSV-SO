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
        Import<ItemSO>("Sheet - Item.csv", "ITEMS", "name_en");
        Import<EnemySO>("Sheet - Enemy.csv", "ENEMY", "name_en");
    }

    public static void Import<T>(string pCSVName, string pTargetFolderName, string pNameField) where T : ScriptableObject, IDataObject
    {
        Debug.Log($"Import {pTargetFolderName}...");
        if (!AssetDatabase.IsValidFolder($"{CSV.DATA_FILEPATH}/{pTargetFolderName}"))
        {
            AssetDatabase.CreateFolder($"{CSV.DATA_FILEPATH}", pTargetFolderName);
        }
        string[] CSVLines = File.ReadAllLines(Application.dataPath + CSV.CSV_FILEPATH + pCSVName);
        string[] CSVHeader = CSV.ParseHeader(CSVLines[0]);
        List<Dictionary<string, string>> Data = CSV.Parse(CSVLines, CSVHeader);

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
            {
                if (!ItemData.ContainsKey(pNameField))
                {
                    Debug.LogError($"Can't find the field {pNameField} in the data to use for filename");
                    return;
                }

                string filename = ItemData[pNameField];
                AssetDatabase.CreateAsset(item, $"{CSV.DATA_FILEPATH}/{pTargetFolderName}/{filename}.asset");
                
            }

        }

        AssetDatabase.SaveAssets();
        Debug.Log($"Import successful!");
    }
}
