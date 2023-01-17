using UnityEngine;
using UnityEditor;

public static class CSVItemExport
{
    [MenuItem("Tools/Export CSV Items")]
    static void ImportAll()
    {
        CSV.Export<ItemSO>("Sheet - Item.csv", "ITEMS");
        CSV.Export<EnemySO>("Sheet - Enemy.csv", "ENEMY");
    }

    
}
