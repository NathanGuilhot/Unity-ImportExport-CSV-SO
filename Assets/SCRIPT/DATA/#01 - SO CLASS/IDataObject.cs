using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interface for object we then import from CSV
//The init method patch the lack of constructor and argument in SO.CreateInstance()
public interface IDataObject
{
    public void init(Dictionary<string, string> pData, string[] pHeader);
    public bool isValid { get; set; }

    //public string GetHeader();
    public Dictionary<string, string> GetData();
    public string[] GetHeader();
}
