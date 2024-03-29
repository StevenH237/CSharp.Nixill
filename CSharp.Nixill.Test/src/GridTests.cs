using System.IO;
using System;
using NUnit.Framework;
using Nixill.Collections.Grid;
using Nixill.Collections.Grid.CSV;
using System.Collections.Generic;
using System.Linq;

namespace Nixill.Test
{
  public class GridTests
  {
    [Test]
    public void GridManipTest()
    {
      Grid<string> testingGrid = new Grid<string>();

      testingGrid.AddColumn();
      testingGrid.AddColumn();
      testingGrid.AddColumn();

      Assert.AreEqual(0, testingGrid.Size);
      Assert.AreEqual(0, testingGrid.Height);
      Assert.AreEqual(3, testingGrid.Width);

      testingGrid.AddRow();
      testingGrid.AddRow();

      Assert.AreEqual(6, testingGrid.Size);
      Assert.AreEqual(2, testingGrid.Height);
      Assert.AreEqual(3, testingGrid.Width);

      // please don't actually do this it makes me cry ;-;
      testingGrid["A1"] = "hello";
      testingGrid["r2c1"] = "world";
      testingGrid[new GridReference(1, 0)] = "there";
      testingGrid["c1"] = "beautiful";
      testingGrid[1, 1] = "lovely";
      testingGrid[(GridReference)new Tuple<int, int>(2, 1)] = "day";

      Assert.AreEqual(testingGrid[new GridReference("R2C1")], "world");

      string toCSV = CSVParser.GridToString(testingGrid);

      Assert.AreEqual("hello,there,beautiful\nworld,lovely,day", toCSV);

      Grid<string> toGrid = CSVParser.StringToGrid(toCSV);
      string toCSVAgain = CSVParser.GridToString(toGrid);

      Assert.AreEqual(toCSV, toCSVAgain);

      testingGrid.AddRow(new List<string> { "add", "some", "values" });
      testingGrid.InsertColumn(2, new List<string> { "commas,", "\"quotes\"", "new\nlines" });

      toCSV = CSVParser.GridToString(testingGrid);

      Assert.AreEqual("hello,there,\"commas,\",beautiful\nworld,lovely,\"\"\"quotes\"\"\",day\nadd,some,\"new\nlines\",values", toCSV);

      toGrid = CSVParser.StringToGrid(toCSV);
      toCSVAgain = CSVParser.GridToString(toGrid);

      Assert.AreEqual(toCSV, toCSVAgain);

      // Now test files
      string tmp = Path.GetTempPath();
      string file = tmp + "test.csv";

      CSVParser.GridToFile(testingGrid, file);
      toGrid = CSVParser.FileToGrid(file);
      toCSVAgain = CSVParser.GridToString(toGrid);

      Assert.AreEqual(toCSV, toCSVAgain);
    }

    [Test]
    public void GridColumnsTest()
    {
      // Let's test the Columns thing! :D
      decimal[][] divisionArray = Enumerable.Range(1, 10).Select(x => (decimal)x).Select(
        num => Enumerable.Range(1, 5).Select(x => (decimal)x).Select(den => num / den).ToArray()
      ).ToArray();

      Grid<decimal> divisionTable = new(divisionArray);
      Grid<decimal> divisionTableTransposed = new(divisionTable.Columns);

      foreach (int r in Enumerable.Range(0, 10))
      {
        foreach (int c in Enumerable.Range(0, 5))
        {
          Assert.AreEqual(divisionTable[r, c], divisionTableTransposed[c, r]);
        }
      }
    }
  }
}