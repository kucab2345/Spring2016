// Skeleton written by Joe Zachary for CS 3500, January 2015
// Revised by Joe Zachary, January 2016
// JLZ Repaired pair of mistakes, January 23, 2016
// Additional code written by Henry Kucab 1/26/16 PS2 Commit
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Formulas
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  Provides a means to evaluate Formulas.  Formulas can be composed of
    /// non-negative floating-point numbers, variables, left and right parentheses, and
    /// the four binary operator symbols +, -, *, and /.  (The unary operators + and -
    /// are not allowed.)
    /// </summary>
    public class Formula
    {
        List<string> rawFormula = new List<string>();
        /// <summary>
        /// Creates a Formula from a string that consists of a standard infix expression composed
        /// from non-negative floating-point numbers (using C#-like syntax for double/int literals), 
        /// variable symbols (a letter followed by zero or more letters and/or digits), left and right
        /// parentheses, and the four binary operator symbols +, -, *, and /.  White space is
        /// permitted between tokens, but is not required.
        /// 
        /// Examples of a valid parameter to this constructor are:
        ///     "2.5e9 + x5 / 17"
        ///     "(5 * 2) + 8"
        ///     "x*y-2+35/9"
        ///     
        /// Examples of invalid parameters are:
        ///     "_"
        ///     "-5.3"
        ///     "2 5 + 3"
        /// 
        /// If the formula is syntacticaly invalid, throws a FormulaFormatException with an 
        /// explanatory Message.
        /// </summary>
        public Formula(String formula)
        {
            int openParenthesis = 0, closeParenthesis = 0;
            char[] charformula = formula.ToCharArray();

            if (formula.Length < 1)//ensures formula not empty
            {
                throw new Exception("Formula length too short!");
            }
            if (char.IsLetterOrDigit(formula[0]) == false && formula[0] != '(')//checks first character validity
            {
                throw new FormulaFormatException("Starting character in formula invalid");
            }
            if (char.IsLetterOrDigit(formula[formula.Length-1]) == false && formula[formula.Length - 1] != ')')//checks last character validity
            {
                throw new FormulaFormatException("Ending character in formula invalid");
            }
            foreach (char i in formula)
            {
                double test;
                if(i == '(')
                {
                    openParenthesis++;
                }
                if(i == ')')
                {
                    closeParenthesis++;
                }
                double.TryParse(i.ToString(), out test);
                if(test < 0)
                {
                    throw new FormulaFormatException("Cannot have negative numbers");
                }
            }
            if(openParenthesis != closeParenthesis)
            {
                throw new FormulaFormatException("Number of '(' and ')' not equal");
            }
            foreach (string b in GetTokens(formula))
            {
                rawFormula.Add(b);
            }
            for (int i = 0; i < rawFormula.Count() - 1; i++)
            {
                double test;
                bool isoperand = false;
                bool isnumber = false;
                if (rawFormula[i] == "+" || rawFormula[i] == "-" || rawFormula[i] == "*" || rawFormula[i] == "/")
                {
                    isoperand = true;
                    if ((rawFormula[i + 1] == "+" || rawFormula[i + 1] == "-" || rawFormula[i + 1] == "*" || rawFormula[i + 1] == "/") && isoperand == true)
                    {
                        throw new FormulaFormatException("Consecutive operands illegal");
                    }
                }
                if (double.TryParse(rawFormula[i], out test) == true)
                {
                    isnumber = true;
                    if (double.TryParse(rawFormula[i + 1], out test) == true && isnumber == true)
                    {
                        throw new FormulaFormatException("Missing operands");
                    }
                }
            }
        }
        /// <summary>
        /// Evaluates this Formula, using the Lookup delegate to determine the values of variables.  (The
        /// delegate takes a variable name as a parameter and returns its value (if it has one) or throws
        /// an UndefinedVariableException (otherwise).  Uses the standard precedence rules when doing the evaluation.
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, its value is returned.  Otherwise, throws a FormulaEvaluationException  
        /// with an explanatory Message.
        /// </summary>
        public double Evaluate(Lookup lookup)
        {
            Stack<double> valueStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();
            double test;

            foreach (string i in rawFormula)
            {
                if (double.TryParse(i, out test) == true)//if t is double
                {
                    if (operatorStack.Count() != 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                    {
                        if (operatorStack.Peek() == "*")
                        {
                            double pop = valueStack.Pop();
                            operatorStack.Pop();
                            valueStack.Push(test * pop);
                        }
                        else if (operatorStack.Peek() == "/")
                        {
                            double pop = valueStack.Pop();
                            operatorStack.Pop();
                            if(test != 0)
                            {
                                valueStack.Push(pop / test);
                            }
                            else
                            {
                                throw new FormulaEvaluationException("Cannot divide by zero");
                            }
                        }
                    }
                    else
                    {
                        valueStack.Push(test);
                    }//end * and /
                }//end i as double
                else if(i == "+" || i == "-")
                {
                    if (operatorStack.Count() != 0 && (operatorStack.Peek() == "+" || operatorStack.Peek() == "-"))
                    {
                        double var1, var2, resultant;
                        var1 = valueStack.Pop();
                        var2 = valueStack.Pop();
                        if(operatorStack.Peek() == "+")
                        {
                            resultant = var1 + var2;
                        }
                        else
                        {
                            resultant = var1 - var2;
                        }
                        operatorStack.Pop();
                        valueStack.Push(resultant);
                    }
                    operatorStack.Push(i);
                }//end i as + or -
                else if(i == "*" || i == "/" || i == "(")
                {
                    operatorStack.Push(i);
                }
                else if(i == ")")
                {
                    if(operatorStack.Count != 0 && (operatorStack.Peek() == "+" || operatorStack.Peek() == "-"))
                    {
                        double var1, var2, resultant;
                        var1 = valueStack.Pop();
                        var2 = valueStack.Pop();
                        if (operatorStack.Peek() == "+")
                        {
                            resultant = var1 + var2;
                        }
                        else
                        {
                            resultant = var1 - var2;
                        }
                        operatorStack.Pop();
                        valueStack.Push(resultant);
                    }
                    operatorStack.Pop();
                    if(operatorStack.Count != 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                    {
                        double var1, var2, resultant;
                        var1 = valueStack.Pop();
                        var2 = valueStack.Pop();
                        if (operatorStack.Peek() == "*")
                        {
                            resultant = var1 * var2;
                        }
                        else
                        {
                            if(var2 != 0)
                            {
                                resultant = var1 / var2;
                            }
                            else
                            {
                                throw new FormulaEvaluationException("Cannot divide by zero");
                            }
                        }
                        operatorStack.Pop();
                        valueStack.Push(resultant);
                    }
                }//end t as )
                else
                {
                    if (operatorStack.Count != 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                    {
                        if (operatorStack.Peek() == "*")
                        {
                            double pop = valueStack.Pop();
                            operatorStack.Pop();
                            try
                            {
                                valueStack.Push(lookup(i) * pop);
                            }
                            catch(UndefinedVariableException)
                            {
                                throw new FormulaEvaluationException(i + " : Missing Definition");
                            }
                        }
                        else if (operatorStack.Peek() == "/")
                        {
                            double pop = valueStack.Pop();
                            operatorStack.Pop();
                            if(lookup(i) != 0)
                            {
                                try
                                {
                                    valueStack.Push(pop / lookup(i));
                                }
                                catch(UndefinedVariableException)
                                {
                                    throw new FormulaEvaluationException(i + " : Missing Definition");
                                }
                            }
                            else
                            {
                                throw new FormulaEvaluationException("Cannot divide by zero");
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            valueStack.Push(lookup(i));
                        }
                        catch(UndefinedVariableException)
                        {
                            throw new FormulaEvaluationException(i + " : Missing Definition");
                        }
                    }//end * and /
                }
            }
            if(operatorStack.Count == 0)
            {
                return valueStack.Pop();
            }
            else
            {
                double var1, var2;
                var1 = valueStack.Pop();
                var2 = valueStack.Pop();
                if (operatorStack.Peek() == "+")
                {
                    return var1 + var2;
                }
                else
                {
                    return var1 - var2;
                }
            }
            return 0;
        }

        /// <summary>
        /// Given a formula, enumerates the tokens that compose it.  Tokens are left paren,
        /// right paren, one of the four operator symbols, a string consisting of a letter followed by
        /// zero or more digits and/or letters, a double literal, and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }
    }

    /// <summary>
    /// A Lookup method is one that maps some strings to double values.  Given a string,
    /// such a function can either return a double (meaning that the string maps to the
    /// double) or throw an UndefinedVariableException (meaning that the string is unmapped 
    /// to a value. Exactly how a Lookup method decides which strings map to doubles and which
    /// don't is up to the implementation of the method.
    /// </summary>
    public delegate double Lookup(string s);

    /// <summary>
    /// Used to report that a Lookup delegate is unable to determine the value
    /// of a variable.
    /// </summary>
    public class UndefinedVariableException : Exception
    {
        /// <summary>
        /// Constructs an UndefinedVariableException containing whose message is the
        /// undefined variable.
        /// </summary>
        /// <param name="variable"></param>
        public UndefinedVariableException(String variable)
            : base(variable)
        {
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the parameter to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message) : base(message)
        {
        }
    }

    /// <summary>
    /// Used to report errors that occur when evaluating a Formula.
    /// </summary>
    public class FormulaEvaluationException : Exception
    {
        /// <summary>
        /// Constructs a FormulaEvaluationException containing the explanatory message.
        /// </summary>
        public FormulaEvaluationException(String message) : base(message)
        {
        }
    }
}
