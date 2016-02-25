using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Formulas;
using Dependencies;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace SS
{
    /// <summary>
    /// Cell class holds three variables, name, contents, and value. The name field preserves the case sensitivity of the user-inputted cell name. 
    /// Standard constructor allows the cell to be name with a cell_name, and a cell_contents.
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
            value = cell_contents;
        }
        /// <summary>
        /// Empty cell constructor. Name requires, but contents are not
        /// </summary>
        public Cell(string cell_name)
        {
            name = cell_name;
            contents = "";
            value = "";
        }
        /// <summary>
        /// Empty cell with no name. Empty string;
        /// </summary>
        public Cell()
        {
            name = "";
            contents = "";
            value = "";
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
    public class Spreadsheet : AbstractSpreadsheet
    {
        /// <summary>
        /// Dictionary that maps strings of names to the cells
        /// </summary>
        Dictionary<string, Cell> cellTable = new Dictionary<string, Cell>();
        /// <summary>
        /// DependencyGraph that maps the dependencies of the formulas encapsulated in each cell
        /// </summary>
        DependencyGraph dgGraph = new DependencyGraph();
        /// <summary>
        /// Changed's method's boolean value. False by default.
        /// </summary>
        private bool ChangeBool = false;

        private Regex IsValid;
        /// <summary>
        /// Creates a new spreadsheet object, empty.
        /// </summary>
        /// // ADDED FOR PS6
        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed
        {
            get
            {
                return this.ChangeBool;
            }

            protected set
            {
                this.ChangeBool = value;
            }
        }
        /// <summary>
        /// Constructor with an empty regex IsValid parameter
        /// </summary>
        public Spreadsheet()
        {
            IsValid = new Regex(".*?");
        }
        /// <summary>
        /// Creates a new spreadsheet object that adhears to the naming principles of the Regex constraints
        /// </summary>
        /// <param name="isValid"></param>
        public Spreadsheet(Regex isValid)
        {
            IsValid = isValid;
        }
        /// <summary>
        /// Takes in a source file and allows user to load in a previously saved XML file
        /// </summary>
        /// <param name="source"></param>
        public Spreadsheet(TextReader source)
        {
            XmlSchemaSet sc = new XmlSchemaSet();

            sc.Add(null, "Spreadsheet.xsd");

            // Configure validation.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = sc;
            settings.ValidationEventHandler += ValidationCallback;

            try
            {
                using (XmlReader reader = XmlReader.Create(source, settings))
                {
                    while (reader.Read())
                    {
                        switch (reader.Name)
                        {
                            case "spreadsheet":
                                if (reader.NodeType != XmlNodeType.EndElement)
                                {
                                    IsValid = new Regex(reader["IsValid"]);//create regex 
                                }
                                break;

                            case "cell":
                                if(cellTable.ContainsKey(reader["name"]))//checks for duplicate cell names, throws SpreadsheetReadException
                                {
                                    throw new SpreadsheetReadException("Duplicate cell names detected");
                                }
                                SetContentsOfCell(reader["name"], reader["contents"]);
                                break;
                        }
                    }
                }
            }
            catch (Exception e)//if an exception was thrown, break it down
            {
                if(e is InvalidNameException)//if a cell name was invalid
                {
                    throw new SpreadsheetReadException("Invalid cell name in source");
                }
                if(e is SpreadsheetReadException)//is a duplicate cell name was caught
                {
                    throw new SpreadsheetReadException("Duplicated cell names detected");
                }
                throw new IOException();//Other generic IO error, like invalid Source/Destination
            }
           
            foreach(string i in GetNamesOfAllNonemptyCells())
            {
                if (cellTable[i].value is FormulaError)
                {
                    throw new SpreadsheetReadException("Formula Error Stored in Cell");
                }
            }
        }
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            isValidName(name);
            name = name.ToUpper();
            if (!cellTable.ContainsKey(name))
            {
                object emptyStringCell = "";
                return emptyStringCell;
            }
            if (cellTable.ContainsKey(name))//if the table contains a cell named as name
            {
                return cellTable[name].contents;//return that cell's contents
            }
            else
            {
                throw new InvalidNameException();
            }
        }
        // ADDED FOR PS6
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {
            isValidName(name);
            string originalname = name;
            name = name.ToUpper();

            //check to see if named cell exists
            if (cellTable.ContainsKey(name))
            {
                if (cellTable[name].contents is string)
                {
                    cellTable[name].value = (string)cellTable[name].contents;//set value as contents if it is a string
                }
                else if (cellTable[name].contents is double)
                {
                    cellTable[name].value = (double)cellTable[name].contents;//set value as contents if it is a double
                }
                return cellTable[name].value;//return the value
            }
            else
            {
                SetCellContents(name, "");
                return "";
            }
        }
        private void SetCellValue(string name)
        {
            if (cellTable.ContainsKey(name) && (cellTable[name].contents is string || cellTable[name].contents is double))//if the content that needs to be valued is a double or string, just copy it to value
            {
                cellTable[name].value = cellTable[name].contents;
            }
            else if (cellTable[name].contents is Formula)//if the contents is of a formula type
            {
                try
                {
                    Formula f = (Formula)cellTable[name].contents;//cast it as a formula object
                    cellTable[name].value = f.Evaluate(s => lookupHelper(s));//evaluate it and set that cell's value to the evaluated double
                }
                catch (Exception e)//catch and handle any exceptions.
                {
                    if (e is FormulaFormatException)//catch expected errors and throw appropriate exceptions. Save them to the cell's value
                    {
                        cellTable[name].value = new FormulaError("Formula Format Exception");
                    }
                    else if (e is CircularException)
                    {
                        cellTable[name].value = new FormulaError("Circular Exception");
                    }
                    else if (e is FormulaEvaluationException)
                    {
                        cellTable[name].value = new FormulaError("Formula Evaluation Exception");
                    }
                }
            }
        }
        private double lookupHelper(string name)
        {
            if (!(cellTable[name].contents is double) && !(cellTable[name].contents is Formula))//catches invalid lookups and undefined variable exceptions
            {
                throw new UndefinedVariableException(name);
            }
            return (double)cellTable[name].value;
        }
        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            foreach (KeyValuePair<string, Cell> cell in cellTable)//go through all cells
            {
                if (!(cell.Value.contents is string) || (string)cell.Value.contents != "")//if a cell's value is not null, yield return it's name
                {
                    yield return cell.Value.name;
                }
            }
        }
        // ADDED FOR PS6
        /// <summary>
        /// Writes the contents of this spreadsheet to dest using an XML format.
        /// The XML elements should be structured as follows:
        ///
        /// <spreadsheet IsValid="IsValid regex goes here">
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        /// </spreadsheet>
        ///
        /// The value of the isvalid attribute should be IsValid.ToString()
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.
        /// If the cell contains a string, the string (without surrounding double quotes) should be written as the contents.
        /// If the cell contains a double d, d.ToString() should be written as the contents.
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        ///
        /// If there are any problems writing to dest, the method should throw an IOException.
        /// </summary>
        public override void Save(TextWriter dest)
        {
            using (XmlWriter writer = XmlWriter.Create(dest))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("IsValid", IsValid.ToString());

                List<string> AllCells = new List<string>();
                foreach (string i in GetNamesOfAllNonemptyCells())//Get names of all non empty cells and store them in the list
                {
                    AllCells.Add(i);
                }

                foreach (string i in AllCells)
                {
                    writer.WriteStartElement("cell");
                    writer.WriteAttributeString("name", i);
                    if (cellTable[i].contents is string)//write string attribute of contents if is a string
                    {
                        writer.WriteAttributeString("contents", (string)cellTable[i].contents);
                    }
                    else if (cellTable[i].contents is double)//write string attribute of contents if it is a string
                    {
                        writer.WriteAttributeString("contents", cellTable[i].contents.ToString());
                    }
                    else if (cellTable[i].contents is Formula)//What if it is a formula error?
                    {
                        writer.WriteAttributeString("contents", "=" + cellTable[i].contents.ToString());
                    }
                    else if (cellTable[i].contents is FormulaError)//Handling formula error
                    {
                        writer.WriteAttributeString("contents", "=" + cellTable[i].contents.ToString());
                    }
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            Changed = false;
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
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            ISet<string> dependents = new HashSet<string>();
            isValidName(name);
            string originalname = name;
            name = name.ToUpper();

            if (cellTable.ContainsKey(name) && cellTable[name].contents is Formula)//if cellTable contains the named cell
            {
                Formula original = (Formula)cellTable[name].contents;
                foreach (string token in original.GetVariables())//get the variables
                {
                    if (isValidName(token) == true)//check that the variable returned is in fact a cell name
                    {
                        dgGraph.RemoveDependency(token, name);//remove the dependency to old cells
                    }
                }
                foreach (string token in formula.GetVariables())
                {
                    if (isValidName(token) == true)
                    {
                        dgGraph.AddDependency(token, name);//create the new dependencies
                    }
                }
            }
            else
            {
                if (!cellTable.ContainsKey(name))
                {
                    cellTable.Add(name, new Cell(originalname));//otherwise create a new cell, construct it w the name and formula passed to the method
                }
                foreach (string i in formula.GetVariables())
                {
                    if (isValidName(i))
                    {
                        dgGraph.AddDependency(i, name);//Add new dependencies for each referenced cell
                    }
                }
            }

            foreach (string i in GetCellsToRecalculate(name))//Get names of all cells that depend on the change in question
            {
                dependents.Add(i);
            }

            cellTable[name].contents = formula; //set the named cell's contents to the formula

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
        protected override ISet<string> SetCellContents(string name, string text)
        {
            isValidName(name);
            ISet<string> resultant = new HashSet<string>();
            string originalname = name;
            name = name.ToUpper();
            if (text == null)
            {
                throw new ArgumentNullException();
            }
            if (cellTable.ContainsKey(name))//Check if cell in question exists. If it does, rewrite its contents
            {
                cellTable[name].contents = text;
            }
            else
            {
                cellTable.Add(name, new Cell(originalname, text));//Otherwise, create a new cell
            }
            foreach (string i in GetCellsToRecalculate(name))//Get ISet of cells who depend on the change
            {
                resultant.Add(i);
            }
            return resultant;
        }
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, double number)
        {
            isValidName(name);
            ISet<string> resultant = new HashSet<string>();
            string originalname = name;
            name = name.ToUpper();
            if (cellTable.ContainsKey(name))//Check that cellTable contains the cell in question
            {
                cellTable[name].contents = number;//if it does, rewrite its contents
            }
            else
            {
                cellTable.Add(name, new Cell(originalname, number));//otherwise make a new cell and assign the value
            }
            foreach (string i in GetCellsToRecalculate(name))//Recalculate all cells that depend on the change
            {
                resultant.Add(i);
            }
            return resultant;
        }
        // ADDED FOR PS6
        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        ///
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        ///
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor with s => s.ToUpper() as the normalizer and a validator that
        /// checks that s is a valid cell name as defined in the AbstractSpreadsheet
        /// class comment.  There are then three possibilities:
        ///
        ///   (1) If the remainder of content cannot be parsed into a Formula, a
        ///       Formulas.FormulaFormatException is thrown.
        ///
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///
        ///   (3) Otherwise, the contents of the named cell becomes f.
        ///
        /// Otherwise, the contents of the named cell becomes content.
        ///
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        ///
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            if (content == null)
            {
                throw new ArgumentNullException();
            }

            isValidName(name);
            string originalname = name;
            name = name.ToUpper();

            ISet<string> result;

            double test = 0;
            if (content == "")
            {
                result = SetCellContents(name, "");
            }
            else if (double.TryParse(content, out test))
            {
                result = SetCellContents(name, test);
            }
            else if (content[0] == '=')
            {
                string remainder = content;
                remainder = remainder.Remove(0, 1);

                result = SetCellContents(name, new Formula(remainder, s => s.ToUpper(), s => isValidName(s)));
            }
            else
            {
                result = SetCellContents(name, content);
            }
            foreach (string i in result)
            {
                SetCellValue(i);
            }
            Changed = true;
            return result;
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
            isValidName(name);
            name = name.ToUpper();
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
        private bool isValidName(string name)
        {
            Regex re = new Regex(@"^[A-Z]+[1-9][0-9]*$");
            if (name == null)
            {
                throw new InvalidNameException();
            }
            name = name.ToUpper();
            if (re.IsMatch(name))
            {
                return true;
            }
            else
            {
                throw new InvalidNameException();
            }
        }
        private static void ValidationCallback(object sender, ValidationEventArgs e)
        {
            throw new SpreadsheetReadException("Validation of Spreadsheet Failed");
        }
    }
}
