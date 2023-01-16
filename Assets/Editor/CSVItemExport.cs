using UnityEngine;
using UnityEditor;
using System.IO;

public static class CSVItemExport
{
    static string CSV_FILEPATH = "/SCRIPT/DATA/CSV/";
    static string DATA_FILEPATH = "Assets/SCRIPT/DATA"; //Were the SO folder will be created

    [MenuItem("Tools/Export CSV Items")]
    static void ImportAll()
    {
        Export<ItemSO>("Sheet - Item.csv", "ITEMS");
        Export<EnemySO>("Sheet - Enemy.csv", "ENEMY");
    }

    static void Export<T>(string pTargetCSV, string pFolderName)
    {
        Debug.Log($"Export {pFolderName}...");
        //Grab all scriptable object from pFolderName

        //Turn them into a array -> How? Should we have a function for that in each IDataObject?
        //                          Would be cool to also have a function to get the headers,
        //                          so that we can reconstruct the csv as human readable as possible
        //                          *(and also not break our import script)*

        //Turn array into commas separated string

        //Put that all into the target file

        //Done!
    }
}
