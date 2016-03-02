using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGui;
using System.IO;
using SpreadsheetGUI;
using SS;
using System.Xml;


namespace SpreadsheetGUI
{
    class Controller
    {
        AbstractSpreadsheet sheet;
        ISSInterface window;
        public Controller(ISSInterface view)//Create a new, blank spreadsheet
        {
            sheet = new Spreadsheet(new System.Text.RegularExpressions.Regex(@"^[A-Z]+[1-9]{1}[0-9]{0,1}$"));
            
            window = view;
            window.NewWindowEvent += NewWindow;
            window.CloseWindowEvent += CloseWindow;
            window.FileChosenEvent += OpenFile;
            window.SaveFileEvent += SaveFile;
            window.ChangeSelectionEvent += ChangeSelectedCellHandler;
            window.ChangeContentEvent += ChangeContentEventHandler;
        }

        public void ChangeSelectedCellHandler(int col, int row)
        {
            char prefixLetter = (char)(97 + col);
            string cellNamePrefix = prefixLetter.ToString().ToUpper();
            string cellNameSuffix = (row + 1).ToString();
            string cellName = cellNamePrefix + cellNameSuffix;

            window.CellNameField = cellName;

            window.CellValueField = sheet.GetCellValue(cellName).ToString();

            object temp = sheet.GetCellContents(cellName);

            if(!(temp is double) && !(temp is string) && !(temp is FormulaError))
            {
                window.CellContentField = "= " + temp.ToString();
            }
            else
            {
                window.CellContentField = temp.ToString();
            }
            window.Update(sheet.GetCellValue(cellName).ToString(), col, row);
        }
        public void ChangeContentEventHandler(string cellContent, int col, int row)
        {
            char prefixLetter = (char)(97 + col);
            string cellNamePrefix = prefixLetter.ToString().ToUpper();
            string cellNameSuffix = (row + 1).ToString();
            string cellName = cellNamePrefix + cellNameSuffix;
            
            foreach(string i in sheet.SetContentsOfCell(cellName, cellContent.ToString()))
            {
                char prefix = i[0];
                int colNum = prefix - 65;
                int numRow;

                if (int.TryParse(i.Substring(1), out numRow))
                {
                    window.Update(sheet.GetCellValue(i).ToString(), colNum, numRow - 1);
                }
            }
        }

        public void SaveFile(string filename)
        {
            SaveFileHandler(filename);
        }

        public void NewWindow()
        {
            window.CreateNewWindowHandler();
        }
        public void CloseWindow()
        {
            window.CloseCurrentWindowHandler();
        }
        public void OpenFile(string filename)
        {
            FileChosenHandler(filename);
        }
        public void FileChosenHandler(String filename)
        {
            try
            {
                using (TextReader inFile = File.OpenText(filename))
                {
                    sheet = new Spreadsheet(inFile);
                }
            }
            catch (Exception ex)
            {
                window.Message = "Unable to open file\n" + ex.Message;
            }
            foreach(string i in sheet.GetNamesOfAllNonemptyCells())
            {
                char prefix = i[0];
                int colNum = prefix - 65;
                int numRow;

                if(int.TryParse(i.Substring(1), out numRow))
                {
                    window.Update(sheet.GetCellValue(i).ToString(), colNum, numRow - 1);
                }

            }
            
            window.Title = filename;
        }
        public void SaveFileHandler(String filename)
        {
            try
            {
                using (TextWriter outFile = File.CreateText(filename))
                {
                    sheet.Save(outFile);
                }
            }
            catch (Exception ex)
            {
                window.Message = "Unable to save file\n" + ex.Message;
            }
        }
    }
}
