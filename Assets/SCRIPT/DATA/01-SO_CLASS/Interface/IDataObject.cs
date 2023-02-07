using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface for object we then import from CSV
//The init method patch the lack of constructor and argument in SO.CreateInstance()
public interface IDataObject
{
    void Init(Dictionary<string, string> pData);
    bool isValid { get;}
    int id { get; }

    //Return the same dictionary as it was inputed in init(),
    //But changing the values that were used and could have been modified in the editor
    Dictionary<string, string> GetData();
}
