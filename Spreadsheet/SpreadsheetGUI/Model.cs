using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dependencies;
using Formulas;
using SS;
using System.IO;
using System.Text.RegularExpressions;

namespace SpreadsheetGUI
{
    public class Model
    {
        private AbstractSpreadsheet ss;
        public Model()
        {
            ss = new Spreadsheet();
        }
        public Model(loaded in spreadsheet?)//loading in a spreadsheet constructor
        {
            ss = new Spreadsheet(otherspreadsheet);
        }
        public void ReadSpreadsheet(//address of new spreadsheet. Textreader)
        {
            using (TextReader source = File.OpenText(//address of new spreadsheet))
            {
                ss = new Spreadsheet(source);
            }
        }
    }
}
