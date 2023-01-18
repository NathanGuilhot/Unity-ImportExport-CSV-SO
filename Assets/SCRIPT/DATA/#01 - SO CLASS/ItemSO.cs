using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemSO : ScriptableObject, IDataObject
{
    public int id;
    public new string name;
    public string description;
    public int cost;
    public bool canEquip;
    public int damage;
    public GameObject prefab;

    [field: SerializeField]
    public bool isValid { get; set; } = false;

    [field: SerializeField]
    private List<string> _keys = new List<string>();
    [field: SerializeField]
    private List<string> _values = new List<string>();

    //TODO(Nighten) Switch the name and description to use the Translation system once it's done
    public void init(Dictionary<string, string> pData, string[] pHeader)
    {
        //https://docs.unity3d.com/ScriptReference/ISerializationCallbackReceiver.html
        foreach (var kvp in pData)
        {
            _keys.Add(kvp.Key);
            _values.Add(kvp.Value);
        }
        //this.Header = pHeader;

        if (pData["name_en"] != "")
        {
            this.isValid = true;
        }
        else
        {
            return;
        }
        this.id = int.Parse(pData["id"]);
        this.name = pData["name_en"];
        this.description = pData["description_en"];
        this.cost = (pData["damage"] != "") ? int.Parse(pData["cost"]):0;
        this.canEquip = bool.Parse(pData["canEquip"]);
        this.damage = (pData["damage"]!="")?int.Parse(pData["damage"]):0;
        //pData["prefab_name"];

    }
    public Dictionary<string, string> GetData()
    {
        Dictionary<string, string> _myDictionary = new Dictionary<string, string>();

        for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
            _myDictionary.Add(_keys[i], _values[i]);

        _myDictionary["id"] = this.id.ToString();
        _myDictionary["name_en"] = this.name;
        _myDictionary["description_en"] = this.description;
        _myDictionary["damage"] = this.damage.ToString();
        _myDictionary["canEquip"] = this.canEquip.ToString();
        _myDictionary["damage"] = this.damage.ToString();

        return _myDictionary;
    }
    public string[] GetHeader()
    {
        return _keys.ToArray();
    }
}
