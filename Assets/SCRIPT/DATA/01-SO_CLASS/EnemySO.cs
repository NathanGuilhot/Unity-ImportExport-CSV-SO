using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySO : ScriptableObject, IDataObject
{
    public bool isValid { get; private set; } = false;

    private List<string> _keys = new List<string>();
    private List<string> _values = new List<string>();

    [field: SerializeField]
    public int id { get; private set; }
    [SerializeField] public new string name;
    [SerializeField] public int PV;
    [SerializeField] public int Attack;
    [SerializeField] public ItemSO loot;
    [SerializeField] public GameObject prefab;

    const string ITEMSO_FOLDER = "ITEMS";
    const string PREFAB_PATH = "Assets/PREFAB/ENEMY/";
    public void Init(Dictionary<string, string> pData)
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
        this.PV = int.Parse(pData["PV"]);
        this.Attack = int.Parse(pData["Attack"]);

        Debug.Log(pData["loot"]);
        if (pData["loot"] != string.Empty)
        {
            this.loot = SOFileManagement.GetSOWithId<ItemSO>(int.Parse(pData["loot"]), ITEMSO_FOLDER);
        }

        this.prefab = SOFileManagement.LoadAssetFromFile<GameObject>(PREFAB_PATH, pData["prefab_name"]+".prefab");

        this.isValid = true;
    }

    public Dictionary<string, string> GetData() {
        Dictionary<string, string> SOData = new Dictionary<string, string>();
        for (int i = 0; i != System.Math.Min(_keys.Count, _values.Count); i++)
            SOData.Add(_keys[i], _values[i]);


        SOData["id"] = this.id.ToString();
        SOData["name_en"] = this.name;
        SOData["PV"] = this.PV.ToString();
        SOData["Attack"] = this.Attack.ToString();
        SOData["loot"] = this.loot != null ? this.loot.id.ToString() : "";
        SOData["prefab_name"] = this.prefab != null ? this.prefab.name : "";

        return SOData;
    }
}
