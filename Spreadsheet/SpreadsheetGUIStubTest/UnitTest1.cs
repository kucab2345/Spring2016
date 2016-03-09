using System;
using Dependencies;
using SS;
using Formulas;
using System.IO;
using SpreadsheetGUI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

//Finished testing

namespace SpreadsheetGUIStub
{
    /// <summary>
    /// Test stub that runs as much of the controller as possible.
    /// Most exceptions are handled by the controller/view so no need to catch them
    /// </summary>
    [TestClass]
    public class ControllerTester
    {
        /// <summary>
        /// Creates a blank spreadsheet, then closes
        /// </summary>
        [TestMethod]
        public void GUITestMethod0()
        {
            SpreadsheetStub stub = new SpreadsheetStub();
            Controller controller = new Controller(stub);
            stub.FireNewWindowEvent();
            stub.FireCloseWindowEvent();
            stub.FireCloseWindowEvent();
            Assert.IsTrue(stub.CalledCloseCurrentWindowHandler);
        }
        /// <summary>
        /// Creates a blank spreadsheet, then closes
        /// </summary>
        [TestMethod]
        public void GUITestMethod1()
        {
            SpreadsheetStub stub = new SpreadsheetStub();
            Controller controller = new Controller(stub);
            stub.FireCloseWindowEvent();
            Assert.IsTrue(stub.CalledCloseCurrentWindowHandler);
        }
        /// <summary>
        /// Open a new window, create a dependency and formula.
        /// Save the file, close both windows
        /// </summary>
        [TestMethod]
        public void GUITestMethod2()
        {
            SpreadsheetStub stub = new SpreadsheetStub();
            Controller controller = new Controller(stub);
            stub.FireCloseWindowEvent();
            Assert.IsTrue(stub.CalledCloseCurrentWindowHandler);

            stub.FireChangeSelectionEvent(1, 1);
            stub.FireChangeContentEvent("10", 1, 1);

            stub.FireChangeSelectionEvent(1, 2);
            stub.FireChangeContentEvent("=b2 + 10", 1, 2);

            stub.FireSaveFileEvent("../../test.ss", false);
            
            stub.FireCloseWindowEvent();
            stub.FireCloseWindowEvent();

        }
        /// <summary>
        /// Open the file saved in the above test. Assert the file is correct by
        /// checking the value in a cell
        /// Close both windows.
        /// </summary>
        [TestMethod]
        public void GUITestMethod3()
        {
            SpreadsheetStub stub = new SpreadsheetStub();
            Controller controller = new Controller(stub);
            
            Spreadsheet test;

            stub.FireFileChosenEvent("../../test.ss");

            using (TextReader inFile = File.OpenText("../../test.ss"))
            {
                test = new Spreadsheet(inFile);
            }
            
            Assert.AreEqual((double)test.GetCellValue("b3"), 20);
        }
        /// <summary>
        /// Open a new window, try to select an out of bounds cell
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GUITestMethod4()
        {
            SpreadsheetStub stub = new SpreadsheetStub();
            Controller controller = new Controller(stub);

            stub.FireChangeSelectionEvent(200, 500);
        }
        /// <summary>
        /// Tries opening an invalid file location. 
        /// No exception expected
        /// </summary>
        [TestMethod]
        public void GUITestMethod5()
        {
            SpreadsheetStub stub = new SpreadsheetStub();
            Controller controller = new Controller(stub);

            stub.FireFileChosenEvent("../../../../test.ss");
        }
        /// <summary>
        /// Testing loading the overriden constructor for loading in files
        /// </summary>
        [TestMethod]
        public void GUITestMethod6()
        {
            SpreadsheetStub stub = new SpreadsheetStub();
            Controller controller = new Controller(stub, "../../test.ss");

            Spreadsheet test;

            using (TextReader inFile = File.OpenText("../../test.ss"))
            {
                test = new Spreadsheet(inFile);
            }

            Assert.AreEqual((double)test.GetCellValue("b3"), 20);
        }
        /// <summary>
        /// Testing loading the overriden constructor for loading in files. Loads
        /// in a file, asserts that it loaded in correctly. Modifies a cell/dependency
        /// within the spreadsheet, saves it. Loads it, and asserts the change was saved correctly
        /// </summary>
        [TestMethod]
        public void GUITestMethod7()
        {
            SpreadsheetStub stub = new SpreadsheetStub();
            Controller controller = new Controller(stub, "../../test.ss");

            Spreadsheet test,test2;

            using (TextReader inFile = File.OpenText("../../test.ss"))
            {
                test = new Spreadsheet(inFile);
            }
            Assert.AreEqual((double)test.GetCellValue("b3"), 20);

            stub.FireChangeSelectionEvent(1, 2);
            stub.FireChangeContentEvent("=b2 + 20", 1, 2);
            stub.FireSaveFileEvent("../../test2.ss",false);

            using (TextReader inFile = File.OpenText("../../test2.ss"))
            {
                test2 = new Spreadsheet(inFile);
            }

            Assert.AreEqual((double)test2.GetCellValue("b3"), 30);
        }
        /// <summary>
        /// Loads in a file through overloaded constructor, makes some changes,
        /// then attempts to close without saving
        /// </summary>
        [TestMethod]
        public void GUITestMethod8()
        {
            SpreadsheetStub stub = new SpreadsheetStub();
            Controller controller = new Controller(stub, "../../test.ss");

            Spreadsheet test;

            using (TextReader inFile = File.OpenText("../../test.ss"))
            {
                test = new Spreadsheet(inFile);
            }
            Assert.AreEqual((double)test.GetCellValue("b3"), 20);

            stub.FireChangeSelectionEvent(1, 2);
            stub.FireChangeContentEvent("=b2 + 20", 1, 2);
            stub.FireCloseWindowEvent();
        }
    }
}
