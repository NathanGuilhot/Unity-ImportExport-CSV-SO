using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemSO : ScriptableObject, IDataObject
{
    [field:SerializeField]
    public int id { get; private set; }
    public new string name;
    public string description;
    public int price;
    public bool canEquip;
    public bool canUse;
    public int damage;

    public bool isPotion;
    public bool targetPlayer;
    public PotionType potionType;
    public int PotionValue;

    public GameObject prefab;
    public GameObject effectParticle;
    public Sprite sprite;

    public bool isValid { get; private set; } = false;

    private List<string> _keys = new List<string>();
    private List<string> _values = new List<string>();

    const string PREFAB_PATH = "Assets/PREFAB/ITEMS/";
    const string PARTICLE_PATH = "Assets/PREFAB/FX/Particles/";

    public enum PotionType
    {
        heal,
        love,
        enrage,
        pacifier
    }

    static Dictionary<string, PotionType> _potionNameMap =
        new Dictionary<string, PotionType>
    {
        {"heal", PotionType.heal },
        {"love", PotionType.love },
        {"enrage", PotionType.enrage },
        {"pacifier", PotionType.pacifier },
    };
    static Dictionary<PotionType, string> _potionTypeMap =
        _potionNameMap.ToDictionary((i) => i.Value, (i) => i.Key);


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
        this.description = pData["description_en"];
        this.canEquip = bool.Parse(pData["canEquip"]);
        this.canUse = bool.Parse(pData["canUse"]);
        this.damage = (pData["damage"]!="")?int.Parse(pData["damage"]):0;

        this.price = (pData["price"] != "") ? int.Parse(pData["price"]) : 0;

        this.isPotion = (pData["potionType"] != "");
        if (this.isPotion)
        {
            this.targetPlayer = bool.Parse(pData["targetPlayer"]);
            this.potionType = _potionNameMap[pData["potionType"]];
            this.PotionValue = (pData["potionValue"] != "") ? int.Parse(pData["potionValue"]) : 0;
        }


        this.prefab = SOFileManagement.LoadAssetFromFile<GameObject>(PREFAB_PATH, pData["prefab_name"] + ".prefab");
        this.sprite = this.prefab.GetComponent<SpriteRenderer>().sprite;
        if (pData["effectParticle"] != "")
            this.effectParticle = SOFileManagement.LoadAssetFromFile<GameObject>(PARTICLE_PATH, pData["effectParticle"] + ".prefab");

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
        SOData["canUse"] = this.canUse.ToString();

        SOData["price"] = this.price.ToString();

        SOData["targetPlayer"] = "";
        SOData["potionType"] = "";
        SOData["potionValue"] = "";
        SOData["effectParticle"] = "";

        if (this.isPotion)
        {
            SOData["targetPlayer"] = this.targetPlayer.ToString();
            SOData["potionType"] = _potionTypeMap[this.potionType];
            SOData["potionValue"] = this.PotionValue.ToString() ;
            SOData["effectParticle"] = this.effectParticle != null ? this.effectParticle.name : "";
        }

        SOData["prefab_name"] = this.prefab != null ? this.prefab.name : "";

        return SOData;
    }

    
}
