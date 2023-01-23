using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySO : ScriptableObject, IDataObject
{
    public bool isValid { get; set; } = false;

    private List<string> _keys = new List<string>();
    private List<string> _values = new List<string>();

    [field: SerializeField]
    public int id { get; set; }
    [SerializeField] new string name;
    [SerializeField] int PV;
    [SerializeField] int Attack;
    [SerializeField] ItemSO loot;
    [SerializeField] UnityEngine.Object prefab;

    const string ITEMSO_FOLDER = "ITEMS";
    const string PREFAB_PATH = "Assets/PREFAB/ENEMY/";
    public void init(Dictionary<string, string> pData)
    {
        //https://docs.unity3d.com/ScriptReference/ISerializationCallbackReceiver.html
        foreach (var kvp in pData)
        {
            _keys.Add(kvp.Key);
            _values.Add(kvp.Value);
        }
        

        if (pData["name_en"] == "")
        {
            return;
        }

        this.id = int.Parse(pData["id"]);
        this.name = pData["name_en"];

        if (pData["loot"] != "")
        {
            this.loot = SOFileManagement.GetSOWithId<ItemSO>(int.Parse(pData["loot"]), ITEMSO_FOLDER);
        }

        this.prefab = SOFileManagement.LoadAssetFromFile<UnityEngine.Object>(PREFAB_PATH, pData["prefab_name"]+".prefab");

        this.isValid = true;
    }

    public Dictionary<string, string> GetData() {
        Dictionary<string, string> SOData = new Dictionary<string, string>();
        for (int i = 0; i != System.Math.Min(_keys.Count, _values.Count); i++)
            SOData.Add(_keys[i], _values[i]);


        SOData["id"] = this.id.ToString();
        SOData["name_en"] = this.name;
        SOData["loot"] = this.loot != null ? this.loot.id.ToString() : "";
        SOData["prefab_name"] = this.prefab != null ? this.prefab.name : "";

        return SOData;
    }
}
