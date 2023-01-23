using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SOFileManagement
{

    public static T GetSOWithId<T>(int pID, string pSOFolder) where T : ScriptableObject, IDataObject
    {
        string[] SOFiles = ListOfFilesWithClass<T>(pSOFolder);

        for (int i = 0; i < SOFiles.Length; i++)
        {
            T SELECTED_ITEMS = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(SOFiles[i]));
            if (SELECTED_ITEMS.id == pID)
            {
                return SELECTED_ITEMS;
            }
        }
        return null;
    }

    public static string[] ListOfFilesWithClass<T>(string pFolderName) where T : ScriptableObject, IDataObject
    {
        return AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { $"{CSV.DATA_FILEPATH}/{pFolderName}" });
    }

    public static T LoadAssetFromFile<T>(string pPath, string pFilename) where T : UnityEngine.Object
    {
        Debug.Log($"Trying to Load asset from file {pPath}{pFilename} ...");
        T loadedObject = AssetDatabase.LoadAssetAtPath<T>($"{pPath}{pFilename}");

        if (loadedObject == null)
        {
            Debug.LogError("ERROR ...no file found.");
        }

        return loadedObject;
    }
}
