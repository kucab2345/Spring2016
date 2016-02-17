using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulas;
using Dependencies;

namespace SS
{
    /// <summary>
    /// Cell class ADD COMMENTS
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// Name of the cell. Must adhere to specifications, and is always forced to lowercase in every method
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Contents of the current cell, must adhere to specifications
        /// </summary>
        public object contents { get; set; }
        /// <summary>
        /// Value of the current cell, must adhere to specifications
        /// </summary>
        public object value { get; set; }
        /// <summary>
        /// Constructs a cell with a lower-cased name and contents 
        /// </summary>
        /// <param name="cell_name"></param>
        /// <param name="cell_contents"></param>
        public Cell(string cell_name, object cell_contents)//Cell constructor that takes in a string name and generic contents and value
        {
            name = cell_name;
            contents = cell_contents;
        }
    }
    /// <summary>
    /// An AbstractSpreadsheet object represents the state of a simple spreadsheet.  A 
    /// spreadsheet consists of an infinite number of named cells.
    /// 
    /// A string is a cell name if and only if it consists of one or more letters, 
    /// followed by a non-zero digit, followed by zero or more digits.  Cell names
    /// are not case sensitive.
    /// 
    /// For example, "A15", "a15", "XY32", and "BC7" are cell names.  (Note that 
    /// "A15" and "a15" name the same cell.)  On the other hand, "Z", "X07", and 
    /// "hello" are not cell names."
    /// 
    /// A spreadsheet contains a cell corresponding to every possible cell name.  
    /// In addition to a name, each cell has a contents and a value.  The distinction is
    /// important, and it is important that you understand the distinction and use
    /// the right term when writing code, writing comments, and asking questions.
    /// 
    /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    /// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
    /// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
    /// 
    /// In an empty spreadsheet, the contents of every cell is the empty string.
    ///  
    /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
    /// (By analogy, the value of an Excel cell is what is displayed in that cell's position
    /// in the grid.)
    /// 
    /// If a cell's contents is a string, its value is that string.
    /// 
    /// If a cell's contents is a double, its value is that double.
    /// 
    /// If a cell's contents is a Formula, its value is either a double or a FormulaError.
    /// The value of a Formula, of course, can depend on the values of variables.  The value 
    /// of a Formula variable is the value of the spreadsheet cell it names (if that cell's 
    /// value is a double) or is undefined (otherwise).  If a Formula depends on an undefined
    /// variable or on a division by zero, its value is a FormulaError.  Otherwise, its value
    /// is a double, as specified in Formula.Evaluate.
    /// 
    /// Spreadsheets are never allowed to contain a combination of Formulas that establish
    /// a circular dependency.  A circular dependency exists when a cell depends on itself.
    /// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
    /// A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
    /// dependency.
    /// </summary>
    class Spreadsheet : AbstractSpreadsheet
    {
        Dictionary<string, Cell> cellTable = new Dictionary<string, Cell>();
        DependencyGraph dgGraph = new DependencyGraph();
        /// <summary>
        /// Creates a new spreadsheet object, empty.
        /// </summary>
        public Spreadsheet()
        {
            Spreadsheet spreedsheet = new Spreadsheet();
        }
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            name = name.ToLower();
            isValid(name);
            if (cellTable.ContainsKey(name))//if the table contains a cell named as name
            {
                return cellTable[name].contents;//return that cell's contents
            }
            else
            {
                throw new InvalidNameException();
            }
        }
        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            foreach (KeyValuePair<string, Cell> cell in cellTable)//go through all cells
            {
                if (cell.Value.contents != null)//if a cell's value is not null, yield return it's name
                {
                    yield return cell.Value.name;
                }
            }
        }
        /// <summary>
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            HashSet<string> dependents = new HashSet<string>();
            name = name.ToLower();

            isValid(name);

            if (cellTable.ContainsKey(name))//if cellTable contains the named cell
            {
                cellTable[name].contents = formula; //set the named cell's contents to the formula
            }
            else
            {
                cellTable.Add(name, new Cell(name, formula));//otherwise create a new cell, construct it w the name and formula passed to the method
            }
            foreach (string dependee in formula.GetVariables())//get the variables
            {
                if (isValid(dependee) == true)//check that the variable returned is in fact a cell name
                {
                    dgGraph.AddDependency(dependee, name);//add it to the dependencyGraph
                }
            }
            foreach (string i in dgGraph.GetDependents(name))//iterate through the dependents under the current cell named
            {
                dependents.Add(i);//add them to the Hashset
            }
            return dependents;//Return the hashset
        }
        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, string text)
        {
            HashSet<string> resultant = new HashSet<string>();
            name = name.ToLower();
            isValid(name);
            if (text == null)
            {
                throw new ArgumentNullException();
            }
            if (cellTable.ContainsKey(name))
            {
                cellTable[name].contents = text;
            }
            else
            {
                cellTable.Add(name, new Cell(name, text));
            }
            foreach (string i in dgGraph.GetDependents(name))//iterate through the dependents under the current cell named
            {
                resultant.Add(i);//add them to the Hashset
            }
            return resultant;
        }
        /// <summary>
        /// If formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetCellContents(string name, double number)
        {
            HashSet<string> resultant = new HashSet<string>();
            name = name.ToLower();
            isValid(name);
            if (cellTable.ContainsKey(name))
            {
                cellTable[name].contents = number;
            }
            else
            {
                cellTable.Add(name, new Cell(name, number));
            }
            foreach (string i in dgGraph.GetDependents(name))//iterate through the dependents under the current cell named
            {
                resultant.Add(i);//add them to the Hashset
            }
            return resultant;
        }
        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            name = name.ToLower();
            isValid(name);
            foreach (string child in dgGraph.GetDependents(name))
            {
                yield return child;
            }
        }
        /// <summary>
        /// Checks validity of a cell's name. Returns true if cell name is acceptable and false if it isn't.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool isValid(string name)
        {
            name = name.ToLower();
            Regex re = new Regex(@"^[a-z]+[1-9][0-9]*$");

            if (re.IsMatch(name))
            {
                return true;
            }
            else
                return false;
            
            /*
            double test;
            if (!char.IsLetter(name[0]) || (double.TryParse(char.ToString(name[1]), out test) && (test != 0)) && !char.IsLetter(name[1]))//If the first char of the cell name != a letter OR 2nd char == 0, throw an error. 
            {
                throw new InvalidNameException();
            }
            for(int i = 2; i < name.Length; i++)
            {
                if(name[i] < 0 || name[i] > 9)
                {
                    throw new InvalidNameException();
                }
            }
            if (name == null)
            {
                throw new InvalidNameException();
            }
            return true;
            */
        }
        private HashSet<string> GetIndirectDependencies(string name)
        {
            List<string> current = new List<string>();
            HashSet<string> results = new HashSet<string>();
            
            current.Add(name);
            results.Add(name);

            while(current.Count > 0)//while the current list of dependees to check is not empty
            {
                foreach(string dependent in dgGraph.GetDependents(current[0]))//check the dependents of each one
                {
                    if(dependent == name)//if a dependent of the current dependee = the current cell, CircularException
                    {
                        throw new CircularException();
                    }
                    if(dgGraph.HasDependents(dependent))//If the dependent has dependents, those are indirect dependencies to be checked.
                    {
                        current.Add(dependent);// Add that dependent as a new dependee for another round
                    }
                    results.Add(dependent);//Add the current dependent to the set for return
                }
                current.RemoveAt(0);//Remove the current dependent after analyzed through
            }
            return results;
        }
    }
}
