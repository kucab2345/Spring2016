using System;
using Dependencies;
using SS;
using Formulas;
using System.IO;
using SpreadsheetGUI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpreadsheetGUIStub
{
    [TestClass]
    public class ControllerTester
    {
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
    }
}
