using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        string[] Header = new[] {
            "Username", "Identifier", "First name","Last name"
        };

        List<Dictionary<string, string>> parsedData = CSV.Parse(sampledata, Header);
        List<Dictionary<string, string>> expectedData = new List<Dictionary<string, string>>();
        expectedData.Add(new Dictionary<string, string>()
        {
            {"Username", "booker12"}, {"Identifier", "9012"}, {"First name", "Rachel"}, {"Last name", "Booker"}
        });
        expectedData.Add(new Dictionary<string, string>()
        {
            {"Username", "grey07"}, {"Identifier", "2070"}, {"First name", "Laura"}, {"Last name", "Grey"}
        });


        Assert.AreEqual(parsedData.Count, expectedData.Count); ;

        for (int i = 0; i < parsedData.Count; i++)
        {
            Assert.IsTrue(parsedData[i].SequenceEqual(expectedData[i]));
        }
    }

    [Test]
    public void IsParsingReversible()
    {
        string[] sampledata = {"Username, Identifier, First name,Last name",
                            "booker12,9012,Rachel,Booker",
                            "grey07,2070,Laura,Grey"};
        string[] Header = new[] {
            "Username", "Identifier", "First name","Last name"
        };

        List<Dictionary<string, string>> parsedData = CSV.Parse(sampledata, Header);
        string[] reverseParsedData = CSV.ReverseParse(parsedData);
        List<Dictionary<string, string>> secondParsedData = CSV.Parse(reverseParsedData, Header);

        //Assert.IsTrue(parsedData.SequenceEqual(secondParsedData));

        Assert.AreEqual(parsedData.Count, secondParsedData.Count);

        for (int i = 0; i < parsedData.Count; i++)
        {
            Assert.IsTrue(parsedData[i].SequenceEqual(secondParsedData[i]));
        }

    }

}
