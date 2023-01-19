using System.Collections;
using System.Collections.Generic;
using System.Linq; //Used for the List<>.Last() function
using UnityEngine;

//Used to import, parse and export CSV and Scriptable Object
public static class CSV
{
    public const string CSV_FILEPATH = "/SCRIPT/DATA/CSV/";
    public const string DATA_FILEPATH = "Assets/SCRIPT/DATA"; //Were the SO folder will be created

    public static string[] ParseHeader(string pData)
    {
        Debug.Log(pData);
        string[] HeaderRow = pData.Split(",");
        Debug.Log(HeaderRow);
        //Remove the white spaces from the headers
        for (int i = 0; i < HeaderRow.Length; i++)
        {
            //string HeaderTitle = HeaderRow[i];
            HeaderRow[i] = HeaderRow[i].Trim();
        }
        return HeaderRow;

    }

    //NOTE(Nighten) We use a list of Dictionary so that if we add a column anywhere in the sheet,
    //              the SO constructor doesn't break
    public static List<Dictionary<string, string>> Parse(string[] pData, string[] pHeader)
    {
        List<Dictionary<string, string>> Result = new List<Dictionary<string, string>>();

        List<string[]> DataRowsList = new List<string[]>();

        Debug.Log(string.Join(" - ", pHeader));

        for (int i = 1; i < pData.Length; i++)
        {
            string[] DataRow = pData[i].Split(",");

            //Don't add entries that are empty or missing column (like the last line)
            if (DataRow.Length != pHeader.Length)
            {
                continue;
            }

            Result.Add(new Dictionary<string, string>());
            for (int j = 0; j < pHeader.Length; j++)
            {
                //We Trim the data just so we avoid unexpected results:
                //  white spaces are rarely a disired feature and can be introduced by the spreadsheet program when exporting
                Result.Last().Add(pHeader[j], DataRow[j].Trim()); 
            }

        }

        return Result;
    }
 
    public static string[] ReverseParse(List<Dictionary<string, string>> pData, string[] pHeaders)
    {
        pData = OrderById(pData);
        List<string> DataList = new List<string>();
        string HeaderRow = "";
        Debug.Log(pData[0]["id"]);
        foreach (string header in pHeaders)
        {
            HeaderRow = $"{HeaderRow}{header},";
        }
        DataList.Add(HeaderRow.Remove(HeaderRow.Length-1, 1)); //Remove the last comma

        for (int i = 0; i < pData.Count; i++)
        {
            string row = "";
            foreach (string header in pHeaders)
            {
                row = $"{row}{pData[i][header]},";
            }
            DataList.Add(row.Remove(row.Length - 1, 1));
        }
        return DataList.ToArray();
    }

    public static List<Dictionary<string, string>> OrderById(List<Dictionary<string, string>> pDict)
    {
        return pDict.OrderBy(t => t["id"]).ToList<Dictionary<string, string>>();
    }
}
