using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System;

public static class CSVItemExport
{
    [MenuItem("Tools/Export CSV Items")]
    static void ImportAll()
    {
        Export<ItemSO>("Sheet - Item.csv", "ITEMS");
        Export<EnemySO>("Sheet - Enemy.csv", "ENEMY");
    }

    public static void Export<T>(string pTargetCSV, string pFolderName) where T : ScriptableObject, IDataObject
    {
        Debug.Log($"Export {pFolderName}...");

        //Grab all scriptable object from pFolderName
        string[] SOFiles = SOFileManagement.ListOfFilesWithClass<T>(pFolderName);
        if (SOFiles.Length == 0)
        {
            Debug.LogError($"No valid Files found in {pFolderName}");
            return;

        }

        List<Dictionary<string, string>> SOData = new List<Dictionary<string, string>>();

        for (int i = 0; i < SOFiles.Length; i++)
        {
            T SOInstance = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(SOFiles[i]));
            SOData.Add(SOInstance.GetData());
        }
        
        //Turn list into commas separated string
        string[] StringData = CSV.ReverseParse(SOData);

        //Put that all into the target file
        File.WriteAllLines(Application.dataPath + CSV.CSV_FILEPATH + pTargetCSV, StringData);

        AssetDatabase.Refresh();

        //Done!
        Debug.Log("Export successful!");
    }

    
}
