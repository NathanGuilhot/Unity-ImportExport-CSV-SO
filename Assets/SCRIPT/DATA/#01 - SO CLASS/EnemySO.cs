using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySO : ScriptableObject, IDataObject
{
    [field: SerializeField]
    public bool isValid { get; set; } = false;

    private List<string> _keys = new List<string>();
    private List<string> _values = new List<string>();

    // Start is called before the first frame update
    public void init(Dictionary<string, string> pData)
    {
        //https://docs.unity3d.com/ScriptReference/ISerializationCallbackReceiver.html
        foreach (var kvp in pData)
        {
            _keys.Add(kvp.Key);
            _values.Add(kvp.Value);
        }
        

        if (pData["name_en"] != "")
        {
            this.isValid = true;
        }
        else
        {
            return;
        }
        isValid = true;
    }

    public Dictionary<string, string> GetData() {
        Dictionary<string, string> _myDictionary = new Dictionary<string, string>();

        for (int i = 0; i != System.Math.Min(_keys.Count, _values.Count); i++)
            _myDictionary.Add(_keys[i], _values[i]);
        return _myDictionary;
    }
}
