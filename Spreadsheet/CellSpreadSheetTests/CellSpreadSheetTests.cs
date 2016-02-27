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

            object test = (double)100;
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
        /// <summary>
        /// Invalid name in argument. Should throw InvalidNameException in reponse
        /// </summary>
        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void SS6()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("A1", 100);

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
            sheet.SetCellContents("11", 100);
        }
        /// <summary>
        /// Invalid cell name. Should throw Invalid name exception. Testing the Regex 
        /// </summary>
        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void SS8()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("a0", 100);
        }
        /// <summary>
        /// Invalid cell name. Should throw Invalid name exception. Testing the Regex 
        /// </summary>
        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void SS9()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("a1a", 100);
        }
        /// <summary>
        /// Invalid cell name. Should throw Invalid name exception. Testing the Regex 
        /// </summary>
        [ExpectedException(typeof(InvalidNameException))]
        [TestMethod]
        public void SS10()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("0a5", 100);
        }
        /// <summary>
        /// Makes an empty spreadsheet and adds cell with contents of a string.
        /// Asserts it to check that it is correct
        /// </summary>
        [TestMethod]
        public void SS11()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("a1", "This is the string");

            object test = "This is the string";
            object result = sheet.GetCellContents("a1");
            Assert.AreEqual(test, result);
        }
        /// <summary>
        /// Makes an empty spreadsheet and adds cell with contents of a string.
        /// Asserts it to check that it is correct.
        /// Makes a cell with a formula, and asserts that it is correct. 
        /// Then returns a list of nonemptycells and checks that the names match. 
        /// </summary>
        [TestMethod]
        public void SS12()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula f = new Formula("2 + 3");
            sheet.SetCellContents("F32", f);
            sheet.SetCellContents("a1", "This is the string");

            object stringtest = (string)"This is the string";
            object formula = (string)f.ToString();

            Assert.AreEqual(stringtest, sheet.GetCellContents("a1"));
            Assert.AreEqual(formula, "2+3");

            List<string> cells = new List<string>();
            List<string> results = new List<string>();
            cells.Add("F32");
            cells.Add("a1");

            foreach(string i in sheet.GetNamesOfAllNonemptyCells())
            {
                results.Add(i);
            }

            for(int i = 0; i < cells.Count; i++)
            {
                Assert.AreEqual(results[i],cells[i]);
            }
        }
        /// <summary>
        /// Makes an empty spreadsheet and adds cells with contents.
        /// Returns names of all cells that are not empty and compares them to actual names.
        /// </summary>
        [TestMethod]
        public void SS13()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetCellContents("a1", "This is the string");
            sheet.SetCellContents("a1", "Rewriting the string");
            sheet.SetCellContents("b3", "This is the string1");
            sheet.SetCellContents("V2", "This is the string2");
            sheet.SetCellContents("N19", "This is the string3");

            List<string> actualnames = new List<string>();
            List<string> returnednames = new List<string>();

            actualnames.Add("a1");
            actualnames.Add("b3");
            actualnames.Add("V2");
            actualnames.Add("N19");

            foreach (string i in sheet.GetNamesOfAllNonemptyCells())
            {
                returnednames.Add(i);
            }
            for(int i = 0; i < actualnames.Count; i++)
            {
                Assert.AreEqual(actualnames[i], returnednames[i]);
            }
        }
        /// <summary>
        /// Makes an empty spreadsheet and adds cells with contents.
        /// </summary>
        [TestMethod]
        public void SS14()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula f0 = new Formula("b1 + 3");
            Formula f1 = new Formula("c1 * 5");
            Formula f2 = new Formula("b1 * a1");

            sheet.SetCellContents("a1", f0);
            sheet.SetCellContents("b1", f1);
            sheet.SetCellContents("c1", f2);
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
            Formula f1 = new Formula("c1 * 5");

            sheet.SetCellContents("a1", f0);
            sheet.SetCellContents("b1", f1);

            Formula f2 = new Formula("4+5");
            sheet.SetCellContents("b1", f2);

            object emptystring = (string)"";

            Assert.AreEqual(sheet.GetCellContents("f2"), emptystring);
        }
        /// <summary>
        /// Creates a series of dependencies, both direct and indirect, and checks that all are found and returned as an ISet
        /// </summary>
        [TestMethod]
        public void SS16()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Formula f0 = new Formula("b1 + 5");
            Formula f1 = new Formula("b2 + 3");
            Formula f2 = new Formula("c1 + 2");

            List<string> dependencies = new List<string>();
            HashSet<string> actual = new HashSet<string>();

            sheet.SetCellContents("a1", f0);
            sheet.SetCellContents("b1", f1);
            sheet.SetCellContents("b2", f2);
            actual = (HashSet<string>)sheet.SetCellContents("c1", "End of dependencies");

            dependencies.Add("C1");
            dependencies.Add("B2");
            dependencies.Add("B1");
            dependencies.Add("A1");

            int counter = 0;
            foreach (string i in actual)
            {
                Assert.AreEqual(dependencies[counter++], i);
            }
        }
       
        /// <summary>
        /// Creates a dependency and then replaces it.
        /// </summary>
        [TestMethod]
        public void SS18()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();

            Formula f0 = new Formula("a2 + 1");
            Formula f1 = new Formula("f1 + 2");

            HashSet<string> dependents = new HashSet<string>();

            List<string> firstCheck = new List<string>();
            List<string> secondCheck = new List<string>();

            firstCheck.Add("A1");
            firstCheck.Add("A2");
            secondCheck.Add("A1");
            secondCheck.Add("F1");

            sheet.SetCellContents("a2", "hi");
            dependents = (HashSet<string>)sheet.SetCellContents("A1", f0);

            int count = 0;
            foreach(string i in dependents)
            {
                Assert.AreEqual(firstCheck[count++], i);
            }
            

            dependents.Clear();

            dependents = (HashSet<string>)sheet.SetCellContents("A1", f1);

            count = 0;
            foreach (string i in dependents)
            {
                Assert.AreEqual(secondCheck[count++], i);
            }
        }
    }
}
