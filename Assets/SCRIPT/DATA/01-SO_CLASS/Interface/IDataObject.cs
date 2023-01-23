using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface for object we then import from CSV
//The init method patch the lack of constructor and argument in SO.CreateInstance()
public interface IDataObject
{
    public void init(Dictionary<string, string> pData);
    public bool isValid { get; set; }
    public int id { get; set; }

    //Return the same dictionary as it was inputed in init(),
    //But changing the values that were used and could have been modified in the editor
    public Dictionary<string, string> GetData();
}
