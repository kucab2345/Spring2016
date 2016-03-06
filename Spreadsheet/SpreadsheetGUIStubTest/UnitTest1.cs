using System;
using SpreadsheetGUI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpreadsheetGUIStub
{
    [TestClass]
    public class ControllerTester
    {
        [TestMethod]
        public void GUITestMethod1()
        {
            SpreadsheetStub stub = new SpreadsheetStub();
            Controller controller = new Controller(stub);
            stub.FireCloseWindowEvent();
            Assert.IsTrue(stub.CalledCloseCurrentWindowHandler);
        }
    }
}
