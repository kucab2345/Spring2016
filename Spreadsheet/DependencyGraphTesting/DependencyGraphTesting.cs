using System;
using Dependencies;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

namespace DependencyGraphTestCases
{
    /// <summary>
    /// Series of tests for the DependencyGraph class. Tests range from basic constructor tests
    /// to comprehensive collection tests that test multiple functions at once. There are also
    /// a series of tests that test null arguments and expect exceptions to be thrown.
    /// </summary>
    [TestClass]
    public class DependencyGraphTestCases
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
            d.AddDependency(a, b);
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
            for (int i = 0; i < 10000; i++)
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
                int r = rand.Next(0, 10);
                d.AddDependency(bigTest[i], bigTest[i]);
            }
            for (int i = 0; i < bigTest.Length; i++)
            {
                foreach (string dependee in d.GetDependees(bigTest[i]))
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
                for (int j = 0; j < bigTest.Length; j++)
                {
                    d.AddDependency(bigTest[i], bigTest[j]);
                }
            }
            for (int i = 0; i < bigTest.Length; i++)
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
            d.ReplaceDependents(a, new string[] { "c" });
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
            foreach (string bdependents in d.GetDependents(b))
            {
                dependentcount++;
            }
            Debug.Assert(2 == dependentcount);
            d.ReplaceDependents(null, null);
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
            foreach (string dependee in d.GetDependees("b"))
            {
                dependeecount++;
            }
            Debug.Assert(dependeecount == 1);
        }
        /// <summary>
        /// Same as GraphTestMethod13 but with a copy constructor at the end, and an assertion 
        /// checking the copy's validity
        /// </summary>
        public void GraphTestMethod13b()
        {
            DependencyGraph d = new DependencyGraph();
            string a = "a";
            string b = "b";
            int dependeecount = 0;
            d.AddDependency(a, b);
            d.ReplaceDependees(b, new string[] { "d" });
            d.ReplaceDependees(b, new string[] { "c" });
            foreach (string dependee in d.GetDependees("b"))
            {
                dependeecount++;
            }
            Debug.Assert(dependeecount == 1);
            DependencyGraph copy = new DependencyGraph(d);
            Debug.Assert(copy == d);
        }
        /// <summary>
        /// Following Tests are just testing null argument to HasDependees.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GraphNullTestMethod14()
        {
            DependencyGraph d = new DependencyGraph();
            d.HasDependees(null);
        }
        /// <summary>
        /// Following Tests are just testing null arguments to HasDependents.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GraphNullTestMethod15()
        {
            DependencyGraph d = new DependencyGraph();
            d.HasDependents(null);
        }
        /// <summary>
        /// Following Tests are just testing null arguments to GetDependees.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GraphNullTestMethod16()
        {
            DependencyGraph d = new DependencyGraph();
            foreach (string a in d.GetDependees(null))
            {

            }
        }
        /// <summary>
        /// Following Tests are just testing null arguments to RemoveDependency.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GraphNullTestMethod17()
        {
            DependencyGraph d = new DependencyGraph();
            d.RemoveDependency(null, null);
        }
        /// <summary>
        /// Following Tests are just testing null arguments to ReplaceDependees
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GraphNullTestMethod18()
        {
            DependencyGraph d = new DependencyGraph();
            d.ReplaceDependees(null, null);
        }
        /// <summary>
        /// Test to ensure that hasdependees and hasdependents returns a copy, not a reference.
        /// Credit to Ryan Steele, he helped me write this test.
        /// </summary>
        [TestMethod]
        public void GraphDataMethodTest()
        {
            try
            {
                DependencyGraph d = new DependencyGraph();
                d.AddDependency("a", "b");
                d.AddDependency("a", "c");
                ICollection<string> test = (ICollection<string>)d.GetDependents("a");
                test.Add("d");

                Debug.Assert((new HashSet<string> { "b", "c", "d" }.SetEquals(test)) == true);
                Debug.Assert((new HashSet<string> { "b", "c" }.SetEquals(d.GetDependents("a")) == true));
            }
            catch (Exception fail)
            {
                if (!(fail is NotSupportedException || fail is InvalidCastException))
                    Assert.Fail();
            }
        }
        /// <summary>
        ///Using lots of data with replacement AND COPY CONSTRUCTOR TEST
        ///</summary>
        [TestMethod()]
        public void StressTest8()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 400;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 2; j < SIZE; j += 3)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Replace a bunch of dependents
            for (int i = 0; i < SIZE; i += 2)
            {
                HashSet<string> newDents = new HashSet<String>();
                for (int j = 0; j < SIZE; j += 5)
                {
                    newDents.Add(letters[j]);
                }
                t.ReplaceDependents(letters[i], newDents);

                foreach (string s in dents[i])
                {
                    dees[s[0] - 'a'].Remove(letters[i]);
                }

                foreach (string s in newDents)
                {
                    dees[s[0] - 'a'].Add(letters[i]);
                }

                dents[i] = newDents;
            }
            
            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }
            //Check that the copy is correct too.
            DependencyGraph copy = new DependencyGraph(t);
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(copy.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(copy.GetDependees(letters[i]))));
            }
        }
    }
}
