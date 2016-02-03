using System;
using System.Collections.Generic;
using Dependencies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace DependencyGraphTester
{
    [TestClass]
    public class DependencyGraphTesting
    {
        /// <summary>
        /// Basic constructor test
        /// </summary>
        [TestMethod]
        public void GraphTestMethod1()
        {
            DependencyGraph d = new DependencyGraph();
        }
        /// <summary>
        /// More constructor testing
        /// </summary>
        [TestMethod]
        public void GraphTestMethod1a()
        {
            DependencyGraph d = new DependencyGraph();
            foreach (string y in d.GetDependents("a"))
            {
                Debug.WriteLine(y);
            }
        }
        /// <summary>
        /// Checking size report of constructor
        /// </summary>
        [TestMethod]
        public void GraphTestMethod2()
        {
            DependencyGraph d = new DependencyGraph();
            Debug.Assert(d.Size == 0);
        }
        /// <summary>
        /// Tests add, remove functions and asserts with size after each addition/removal
        /// </summary>
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
        /// <summary>
        /// null exception expectation
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GraphTestMethod4()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency(null, null);
        }
        /// <summary>
        /// Adds in up to 10000 random dependencies 
        /// </summary>
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
        /// <summary>
        /// Adds in dependencies in a pattern and checks them via assert and GetDependees
        /// </summary>
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
        /// <summary>
        /// Adds in dependencies in the pattern a,a   b,b   c,c etc and then asserts them as being correct. Asserts them w/ GetDependents
        /// </summary>
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
        }/// <summary>
        /// Throwing null arguments to AddDependency function
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GraphTestMethod7()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency("s", null);
        }
        /// <summary>
        /// Throwing null arguments to AddDependency function
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GraphTestMethod8()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency(null, "t");
        }
        /// <summary>
        /// Throwing null arguments to AddDependency function
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GraphTestMethod9()
        {
            DependencyGraph d = new DependencyGraph();
            d.AddDependency(null, null);
        }
        /// <summary>
        /// Adds in a bunch of dependencies and then throws null as the argument to GetDependents.
        /// </summary>
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
        /// <summary>
        /// Basic Replace dependents test
        /// </summary>
        [TestMethod]
        public void GraphTestMethod11()
        {
            DependencyGraph d = new DependencyGraph();
            string a = "a";
            string b = "b";
            d.AddDependency(a, b);
            d.ReplaceDependents(a, new string[]{"c"});
        }
        /// <summary>
        /// Throwing null as ienumerable argument to ReplaceDependents
        /// </summary>
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
        /// <summary>
        /// Comprehensive test of replacedependents. Creates dependency a,b then replaces dependency with a,c and tests additive function 
        /// of ReplaceDependents with argument b,e. Counts up Dependents under b and asserts them to what the sum should be. Finally,
        /// throws a double null argument to ReplaceDependents to invoke Exception
        /// </summary>
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
            d.AddDependency(b, "f");
            foreach(string bdependents in d.GetDependents(b))
            {
                dependentcount++;
            }
            Debug.Assert(2 == dependentcount);
            d.ReplaceDependents(null,null);
        }
        /// <summary>
        /// Comprehensive test or ReplaceDependees. Creates dependency a,b and the does a replace b,d. Removes b from a, adds d,b dependency. 
        /// Then, replaces b,d which should remove b from under d, and place it under c. Total of 3 keys, a,c,d with b under c. 1 dependency total.
        /// </summary>
        [TestMethod]
        public void GraphTestMethod13a()
        {
            DependencyGraph d = new DependencyGraph();
            string a = "a";
            string b = "b";
            int dependeecount = 0;
            d.AddDependency(a, b);
            d.ReplaceDependees(b, new string[] { "d" });
            d.ReplaceDependees(b, new string[] { "c" });
            foreach(string dependee in d.GetDependees("b"))
            {
                dependeecount++;
            }
            Debug.Assert(dependeecount == 1);
        }/// <summary>
        /// Test to ensure that hasdependees and hasdependents returns a copy, not a reference.
        /// Credit to Ryan Steele, he helped me write this test.
        /// </summary>
        [TestMethod]
        public void GraphDataMethodTest()
        {
            try
            {
                DependencyGraph d = new DependencyGraph();
                d.AddDependency("1", "a");
                d.AddDependency("2", "b");
                ICollection<string> test = (ICollection<string>)d.GetDependents("a");
                test.Add("d");

                Debug.Assert((new HashSet<string> { "a", "2", "b" }.SetEquals(test)) == true);
                Debug.Assert((new HashSet<string> { "a", "2" }.SetEquals(d.GetDependents("1")) == true));
            }
            catch(Exception fail)
            {
                if (!(fail is NotSupportedException || fail is InvalidCastException))
                    Assert.Fail();
            }
        }
    }
}
