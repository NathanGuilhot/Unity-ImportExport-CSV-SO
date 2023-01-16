using System.Collections;
using System.Collections.Generic;
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

    public bool isValid { get; set; } = false;


    //TODO(Nighten) Switch the name and description to use the Translation system once it's done
    public void init(Dictionary<string, string> pData)
    {
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
}
