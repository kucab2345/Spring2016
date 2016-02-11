// Written by Joe Zachary for CS 3500, January 2016.
// Repaired error in Evaluate5.  Added TestMethod Attribute
//    for Evaluate4 and Evaluate5 - JLZ January 25, 2016
// Corrected comment for Evaluate3 - JLZ January 29, 2016

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formulas;
using System.Collections.Generic;
using System.Diagnostics;

namespace FormulaTestCases
{
    /// <summary>
    /// These test cases are in no sense comprehensive!  They are intended to show you how
    /// client code can make use of the Formula class, and to show you how to create your
    /// own (which we strongly recommend).  To run them, pull down the Test menu and do
    /// Run > All Tests.
    /// </summary>
    [TestClass]
    public class UnitTests
    {
        /// <summary>
        /// This tests that a syntactically incorrect parameter to Formula results
        /// in a FormulaFormatException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct1()
        {
            Formula f = new Formula("_");
        }

        /// <summary>
        /// This is another syntax error
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct2()
        {
            Formula f = new Formula("2++3");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct3()
        {
            Formula f = new Formula("2 3");
        }

        /// <summary>
        /// Makes sure that "2+3" evaluates to 5.  Since the Formula
        /// contains no variables, the delegate passed in as the
        /// parameter doesn't matter.  We are passing in one that
        /// maps all variables to zero.
        /// </summary>
        [TestMethod]
        public void Evaluate1()
        {
            Formula f = new Formula("2+3");
            Assert.AreEqual(f.Evaluate(v => 0), 5.0, 1e-6);
        }

        /// <summary>
        /// The Formula consists of a single variable (x5).  The value of
        /// the Formula depends on the value of x5, which is determined by
        /// the delegate passed to Evaluate.  Since this delegate maps all
        /// variables to 22.5, the return value should be 22.5.
        /// </summary>
        [TestMethod]
        public void Evaluate2()
        {
            Formula f = new Formula("x5");
            Assert.AreEqual(f.Evaluate(v => 22.5), 22.5, 1e-6);
        }

        /// <summary>
        /// Here, the delegate passed to Evaluate always throws a
        /// UndefinedVariableException (meaning that no variables have
        /// values).  The test case checks that the result of
        /// evaluating the Formula is a FormulaEvaluationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate3()
        {
            Formula f = new Formula("x + y");
            f.Evaluate(v => { throw new UndefinedVariableException(v); });
        }

        /// <summary>
        /// The delegate passed to Evaluate is defined below.  We check
        /// that evaluating the formula returns in 10.
        /// </summary>
        [TestMethod]
        public void Evaluate4()
        {
            Formula f = new Formula("x + y");
            Assert.AreEqual(f.Evaluate(Lookup4), 10.0, 1e-6);
        }

        /// <summary>
        /// This uses one of each kind of token.
        /// </summary>
        [TestMethod]
        public void Evaluate5()
        {
            Formula f = new Formula("(x + y) * (z / x) * 1.0");
            Assert.AreEqual(f.Evaluate(Lookup4), 20.0, 1e-6);
        }
        /// <summary>
        /// Tests Parameterless Constructor 
        /// </summary>
        [TestMethod]
        public void Evaluate5a()
        {
            Formula f = new Formula();
            Assert.AreEqual(f.Evaluate(Lookup4),0);
        }
        /// <summary>
        /// This test has a missing operator between the second set of parenthesis and 3. Should return as a FormulaFormatException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Evaluate6()
        {
            Formula f = new Formula("(x + y) * (z / x) 3 * 1.0");
        }
        /// <summary>
        /// Here we pass a 3 in front of the second set of parenthesis which would require an operator in between. Expects a FormulaFormatException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Evaluate7()
        {
            Formula f = new Formula("(x + y) * 3 (z / x) * 1.0");
        }
        /// <summary>
        /// Here we pass a nested set of parenthesis which should be illegal and throw a FormulaFormatException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Evaluate8()
        {
            Formula f = new Formula("(1+2))",Normalizer4, validator => true);
        }
        /// <summary>
        /// Tests Normalizer4 on the input, which should return a Missing Definition Error, since
        /// it should compare capital letter variables to lower case equivalents
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate8a()
        {
            Formula f = new Formula("x + y", Normalizer4, validator => true);
            Assert.AreEqual(f.Evaluate(Lookup4), 10.0, 1e-6);
        }
        /// <summary>
        /// Formula here has inconsistent number of opening and closing parenthesis. Throws FormulaFormatException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Evaluate9()
        {
            Formula f = new Formula("1+(1+(1+2)");
        }
        /// <summary>
        /// Testing the ToString method on the formula class
        /// </summary>
        [TestMethod]
        public void Evaluate9a()
        {
            string test = "";
            Formula f = new Formula("1+3+4", Normalizer4, validator => true);
            test = f.ToString();
            Debug.Assert("1+3+4" == test);
        }
        /// <summary>
        /// Check zero argument constructor and the ToString function. 
        /// Should create formula to be 0, also checks the zero argument case
        /// in ToString
        /// </summary>
        [TestMethod]
        public void Evaluate10()
        {
            Formula f = new Formula();
            Debug.Assert("0" == f.ToString());
            List<string> vars = new List<string>();
            ISet<string> variables = f.GetVariables();
            foreach (string b in variables)
            {
                vars.Add(b);
            }
            Debug.Assert(vars.Count == 0);
        }
        /// <summary>
        /// Tests GetVariables. Makes simple formula and returns the 2 variables, x and y
        /// and asserts that they are correct against literals
        /// </summary>
        [TestMethod]
        public void Evaluate10a()
        {
            List<string> vars = new List<string>();
            Formula f = new Formula("x + y");
            ISet<string> variables = f.GetVariables();
            foreach(string b in variables)
            {
                vars.Add(b);
            }
            Debug.Assert(vars[0] == "x" && vars[1] == "y");
        }
        /// <summary>
        /// A Lookup method that maps x to 4.0, y to 6.0, and z to 8.0.
        /// All other variables result in an UndefinedVariableException.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double Lookup4(String v)
        {
            switch (v)
            {
                case "x": return 4.0;
                case "y": return 6.0;
                case "z": return 8.0;
                default: throw new UndefinedVariableException(v);
            }
        }
        /// <summary>
        /// Normalizer4 takes all variables and ensures they are upper case. If input is 
        /// not a variable, it simply returns back the input unchanged
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public string Normalizer4(String v)
        {
            return v.ToUpper();
        }
    }
}
