using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CSVParserTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void SimpleSampleData()
    {
        string[] sampledata = {"Username, Identifier, First name,Last name",
                            "booker12,9012,Rachel,Booker",
                            "grey07,2070,Laura,Grey"};

        List<Dictionary<string, string>> parsedData = null; //CSV.Parse(sampledata);
        List<Dictionary<string, string>> expectedData = new List<Dictionary<string, string>>();
        expectedData.Add(new Dictionary<string, string>()
        {
            {"Username", "booker12"}, {"Identifier", "9012"}, {"First name", "Rachel"}, {"Last name", "Booker"}
        });
        expectedData.Add(new Dictionary<string, string>()
        {
            {"Username", "gret07"}, {"Identifier", "2070"}, {"First name", "Laura"}, {"Last name", "Grey"}
        });


        Assert.True(parsedData == expectedData);
    }

}
