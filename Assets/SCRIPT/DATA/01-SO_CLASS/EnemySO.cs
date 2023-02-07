using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] public ItemSO[] loot;
    [SerializeField] public ItemSO[] inventory;
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
            string[] loot_array = pData["loot"].Split(",");
            List<ItemSO> loot_list = new List<ItemSO>();
            foreach (string loot_id in loot_array)
            {
                loot_list.Add(SOFileManagement.GetSOWithId<ItemSO>(int.Parse(loot_id), ITEMSO_FOLDER));
            }
            this.loot = loot_list.ToArray();
        }

        if (pData["inventory"] != string.Empty)
        {
            string[] item_array = pData["inventory"].Split(",");
            List<ItemSO> item_list = new List<ItemSO>();
            foreach (string item_id in item_array)
            {
                item_list.Add(SOFileManagement.GetSOWithId<ItemSO>(int.Parse(item_id), ITEMSO_FOLDER));
            }
            this.inventory = item_list.ToArray();
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

        if (this.loot != null)
            SOData["loot"] = String.Join(",", this.loot.Select(item => item.id.ToString()));
        if (this.inventory != null)
            SOData["inventory"] = String.Join(",", this.inventory.Select(item => item.id.ToString()));

        //this.loot != null ? this.loot.id.ToString() : "";
        SOData["prefab_name"] = this.prefab != null ? this.prefab.name : "";

        return SOData;
    }
}
