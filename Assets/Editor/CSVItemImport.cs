using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class CSVItemImport
{
    [MenuItem("Tools/Import CSV Items")]
    static void ImportAll()
    {
        CSV.Import<ItemSO>("Sheet - Item.csv", "ITEMS");
        CSV.Import<EnemySO>("Sheet - Enemy.csv", "ENEMY");
    }
}
