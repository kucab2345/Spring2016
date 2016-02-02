using System;
using Dependencies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace DependencyGraphTester
{
    [TestClass]
    public class DependencyGraphTesting
    {
        [TestMethod]
        public void GraphTestMethod1()
        {
            DependencyGraph d = new DependencyGraph();
        }
        [TestMethod]
        public void GraphTestMethod2()
        {
            DependencyGraph d = new DependencyGraph();
            Debug.Assert(d.Size == 0);
        }
    }
}
