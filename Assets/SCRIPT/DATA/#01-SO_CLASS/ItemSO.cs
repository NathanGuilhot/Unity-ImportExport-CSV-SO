using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSO : ScriptableObject, IDataObject
{
    [field:SerializeField]
    public int id { get; set; }
    public new string name;
    public string description;
    public int cost;
    public bool canEquip;
    public int damage;
    public UnityEngine.Object prefab;

    public bool isValid { get; set; } = false;

    private List<string> _keys = new List<string>();
    private List<string> _values = new List<string>();

    const string PREFAB_PATH = "Assets/PREFAB/ITEMS/";
    
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
        this.description = pData["description_en"];
        this.cost = (pData["damage"] != "") ? int.Parse(pData["cost"]):0;
        this.canEquip = bool.Parse(pData["canEquip"]);
        this.damage = (pData["damage"]!="")?int.Parse(pData["damage"]):0;
        
        this.prefab = SOFileManagement.LoadAssetFromFile<UnityEngine.Object>(PREFAB_PATH, pData["prefab_name"] + ".prefab");

        this.isValid = true;

    }
    public Dictionary<string, string> GetData()
    {
        Dictionary<string, string> SOData = new Dictionary<string, string>();

        for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
            SOData.Add(_keys[i], _values[i]);

        SOData["id"] = this.id.ToString();
        SOData["name_en"] = this.name;
        SOData["description_en"] = this.description;
        SOData["damage"] = this.damage.ToString();
        SOData["canEquip"] = this.canEquip.ToString();
        SOData["damage"] = this.damage.ToString();
        SOData["prefab_name"] = this.prefab != null ? this.prefab.name : "";

        return SOData;
    }
}
