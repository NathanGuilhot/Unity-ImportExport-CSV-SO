using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimalSO : ScriptableObject, IDataObject
{
    [field: SerializeField]
    public int id { get; set; }
    
    //NOTE(Nighten) Add your object field here

    [field: SerializeField]
    public bool isValid { get; set; } = false;
    
    //NOTE(Nighten) Those Lists are used to serialize the input dictionary
    private List<string> _keys = new List<string>();
    private List<string> _values = new List<string>();

    public void init(Dictionary<string, string> pData)
    {
        //https://docs.unity3d.com/ScriptReference/ISerializationCallbackReceiver.html
        foreach (var kvp in pData)
        {
            _keys.Add(kvp.Key);
            _values.Add(kvp.Value);
        }

        //NOTE(Nighten) Check here if the data is correct or if a mandatory field is filled.
        //              Return early if not

        this.id = int.Parse(pData["id"]);

        //Note(Nighten) Fill the object with the data from the dictionary
        //this.name = pData["name"];

        this.isValid = true;
    }
    public Dictionary<string, string> GetData()
    {
        Dictionary<string, string> SOData = new Dictionary<string, string>();

        for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
            SOData.Add(_keys[i], _values[i]);

        //NOTE(Nighten) Fill the field with data that could have been modified in the editor
        //  Example:
        //SOData["name"] = this.name;

        return SOData;
    }
}
