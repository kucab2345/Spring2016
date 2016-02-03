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
        [TestMethod]
        public void GraphTestMethod3()
        {
            string a = "a";
            string b = "b";
            DependencyGraph d = new DependencyGraph();
            d.AddDependency(a,b);
            Debug.Assert(d.Size == 1);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GraphTestMethod4()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency(null, null);
        }
        [TestMethod]
        public void GraphTestMethod5()
        {
            Random rand = new Random();
            DependencyGraph d = new DependencyGraph();
            string[] bigTest = new string[10] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
            for(int i = 0; i < 10000; i++)
            {
                int r = rand.Next(bigTest.Length);
                d.AddDependency(bigTest[r], bigTest[rand.Next(bigTest.Length)]); 
            }
        }
        [TestMethod]
        public void GraphTestMethod6()
        {
            DependencyGraph d = new DependencyGraph();
            string[] bigTest = new string[10] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
            string[] parents = new string[10];
            for (int i = 0; i < bigTest.Length; i++)
            {
               for(int j = 0; j < bigTest.Length; j++)
               {
                    d.AddDependency(bigTest[i],bigTest[j]);
               } 
            }
            foreach(string parent in bigTest)
            {
                
            }
        }
    }
}
