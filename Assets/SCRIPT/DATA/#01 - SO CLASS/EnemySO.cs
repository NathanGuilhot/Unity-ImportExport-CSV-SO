using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySO : ScriptableObject, IDataObject
{
    public bool isValid { get; set; } = false;

    // Start is called before the first frame update
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
        isValid = true;
    }
}
