using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq; //Used for the List<>.Last() function

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
        string[] SOFiles = AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { $"{CSV.DATA_FILEPATH}/{pFolderName}" });
        if (SOFiles.Length == 0)
        {
            Debug.LogError($"No valid Files found in {pFolderName}");
            return;

        }
        Debug.Log("Files: "+string.Join(" ", SOFiles));

        List<Dictionary<string, string>> SOData = new List<Dictionary<string, string>>();

        T tmpObject = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(SOFiles[0]));

        string[] SOHeader = tmpObject.GetHeader();
        Debug.Log("SOHeader :" + string.Join(", ", SOHeader));
        for (int i = 0; i < SOFiles.Length; i++)
        {
            T SOInstance = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(SOFiles[i]));
            SOData.Add(SOInstance.GetData());
        }

        Debug.Log("Before reverse parser: "+string.Join(" ", tmpObject.GetData().Values.ToArray()));
        Debug.Log("SOFiles count: " + SOFiles.Length);
        Debug.Log("SOData count: "+ SOData.Count);
        //Turn list into commas separated string
        string[] StringData = CSV.ReverseParse(SOData, SOHeader);

        //Put that all into the target file
        File.WriteAllLines(Application.dataPath + CSV.CSV_FILEPATH + pTargetCSV, StringData);

        AssetDatabase.Refresh();

        //Done!
        Debug.Log("Export successful!");
    }
}
