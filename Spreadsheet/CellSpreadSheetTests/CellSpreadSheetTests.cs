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
            sheet.SetContentsOfCell("a1", "100");

            object test = (double)100;
            object result = sheet.GetCellContents("a1");
            Assert.AreEqual(test, result);
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
            sheet.SetContentsOfCell("a1", "100");

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
            sheet.SetContentsOfCell("A1", "100");

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
            sheet.SetContentsOfCell("A1", "100");

            object test = 100.00;
            object result = sheet.GetCellContents("A1");
            Assert.AreEqual(test, result);
        }
        /// <summary>
        /// Invalid name in argument. Should throw InvalidNameException in reponse
        /// </summary>
        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void SS6()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "100");

            object test = 100.00;
            object result = sheet.GetCellContents("B01");
            Assert.AreEqual(test, result);
        }
        /// <summary>
        /// Invalid cell name. Should throw Invalid name exception. Testing the Regex 
        /// </summary>
        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void SS7()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("11", "100");
        }
        /// <summary>
        /// Invalid cell name. Should throw Invalid name exception. Testing the Regex 
        /// </summary>
        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void SS8()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a0", "100");
        }
        /// <summary>
        /// Invalid cell name. Should throw Invalid name exception. Testing the Regex 
        /// </summary>
        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void SS9()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1a", "100");
        }
        /// <summary>
        /// Invalid cell name. Should throw Invalid name exception. Testing the Regex 
        /// </summary>
        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void SS10()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("0a5", "100");
        }
        /// <summary>
        /// Makes an empty spreadsheet and adds cell with contents of a string.
        /// Asserts it to check that it is correct
        /// </summary>
        [TestMethod]
        public void SS11()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "This is the string");

            object test = "This is the string";
            object result = sheet.GetCellContents("a1");
            Assert.AreEqual(test, result);
        }

        /// <summary>
        /// Makes an empty spreadsheet and adds cells with contents.
        /// The cells contents of the named cell are empty, which should 
        /// yield an empty string.
        /// </summary>
        [TestMethod]
        public void SS15()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula f0 = new Formula("b1 + 3");
            Formula f1 = new Formula("C1 * 5");

            sheet.SetContentsOfCell("a1", "f0");
            sheet.SetContentsOfCell("b1", "f1");

            Formula f2 = new Formula("4+5");
            sheet.SetContentsOfCell("b1", "f2");

            object emptystring = (string)"";

            Assert.AreEqual(sheet.GetCellContents("f2"), emptystring);
        }
    }
}
