using System;
using SS;
using Formulas;
using Dependencies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Collections.Generic;

namespace CellSpreadSheetTests
{
    [TestClass]
    public class CellSpreadSheetTests
    {
        /// <summary>
        /// Makes an empty spreadsheet
        /// </summary>
        [TestMethod]
        public void SS1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
        }
        /// <summary>
        /// Makes an empty spreadsheet and adds cell with contents of a double
        /// </summary>
        [TestMethod]
        public void SS2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("a1", 100);

            object test = 100.00;
            object result = sheet.GetCellContents("a1");
            Assert.AreEqual(test,result);
        }
        /// <summary>
        /// Makes an empty spreadsheet and adds cell with contents of a double.
        /// This time however, I am trying to access the cell "a1" via "A1".
        /// Checking case sensitivity.
        /// </summary>
        [TestMethod]
        public void SS3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("a1", 100);

            object test = 100.00;
            object result = sheet.GetCellContents("A1");
            Assert.AreEqual(test, result);
        }
        /// <summary>
        /// Makes an empty spreadsheet and adds cell with contents of a double.
        /// This time however, I am trying to access the cell "A1" via "a1".
        /// Checking case sensitivity.
        /// </summary>
        [TestMethod]
        public void SS4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 100);

            object test = 100.00;
            object result = sheet.GetCellContents("a1");
            Assert.AreEqual(test, result);
        }
        /// <summary>
        /// Makes an empty spreadsheet and adds cell with contents of a double.
        /// This time however, I am trying to access the cell "A1" via "A1".
        /// Checking case sensitivity.
        /// </summary>
        [TestMethod]
        public void SS5()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 100);

            object test = 100.00;
            object result = sheet.GetCellContents("A1");
            Assert.AreEqual(test, result);
        }
    }
}
