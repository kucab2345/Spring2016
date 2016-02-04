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
        public void GraphTestMethod1a()
        {
            DependencyGraph d = new DependencyGraph();
            foreach (string y in d.GetDependents("a"))
            {
                Debug.WriteLine(y);
            }
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
            d.RemoveDependency(a, b);
            Debug.Assert(d.Size == 0);
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
        public void GraphTestMethod5a()
        {
            Random rand = new Random();
            DependencyGraph d = new DependencyGraph();
            string[] bigTest = new string[10] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
            for (int i = 0; i < bigTest.Length; i++)
            {
                int r = rand.Next(0,10);
                d.AddDependency(bigTest[i], bigTest[i]);
            }
            for(int i = 0; i < bigTest.Length; i++)
            {
                foreach(string dependee in d.GetDependees(bigTest[i]))
                {
                    Debug.Assert(dependee == bigTest[i]);
                }
            }
        }
        [TestMethod]
        public void GraphTestMethod6()
        {
            DependencyGraph d = new DependencyGraph();
            string[] bigTest = new string[10] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
            string[] children = new string[10] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
            for (int i = 0; i < bigTest.Length; i++)
            {
               for(int j = 0; j < bigTest.Length; j++)
               {
                    d.AddDependency(bigTest[i],bigTest[j]);
               } 
            }
            for(int i = 0; i < bigTest.Length; i++)
            {
                int child = 0;
                foreach (string j in d.GetDependents(bigTest[i]))
                {
                    Debug.Assert(j == children[child]);
                    child++;
                }
            }
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GraphTestMethod7()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("s", null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GraphTestMethod8()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency(null, "t");
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GraphTestMethod9()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency(null, null);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GraphTestMethod10()
        {
            DependencyGraph d = new DependencyGraph();
            string[] bigTest = new string[10] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
            string[] children = new string[10] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
            for (int i = 0; i < bigTest.Length; i++)
            {
                for (int j = 0; j < bigTest.Length; j++)
                {
                    d.AddDependency(bigTest[i], bigTest[j]);
                }
            }
            for (int i = 0; i < bigTest.Length; i++)
            {
                int child = 0;
                foreach (string j in d.GetDependents(null))
                {
                }
            }
        }
        [TestMethod]
        public void GraphTestMethod11()
        {
            DependencyGraph d = new DependencyGraph();
            string a = "a";
            string b = "b";
            d.AddDependency(a, b);
            d.ReplaceDependents(a, new string[]{"c"});
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GraphTestMethod12()
        {
            DependencyGraph d = new DependencyGraph();
            string a = "a";
            string b = "b";
            d.AddDependency(a, b);
            d.ReplaceDependents(a, new string[] { null });
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GraphTestMethod13()
        {
            int dependentcount = 0;
            DependencyGraph d = new DependencyGraph();
            string a = "a";
            string b = "b";
            d.AddDependency(a, b);
            d.ReplaceDependents(a, new string[] { "c" });
            d.ReplaceDependents(b, new string[] { "e" });
            foreach(string bdependents in d.GetDependents(b))
            {
                dependentcount++;
            }
            foreach (string bdependents in d.GetDependents(b))
            {
                dependentcount++;
            }
            Debug.Assert(2 == dependentcount);
            d.ReplaceDependents(null,null);
        }
    }
}
