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
            sheet = new Spreadsheet();

            window = view;
            window.NewWindowEvent += NewWindow;
            window.CloseWindowEvent += CloseWindow;
            window.FileChosenEvent += OpenFile;
            window.SaveFileEvent += SaveFile;
            window.ChangeSelectionEvent += ChangeSelectedCellHandler;
        }

        public void ChangeSelectedCellHandler(int col, int row)
        {
            char prefixLetter = (char)(97 + col);
            string cellNamePrefix = prefixLetter.ToString().ToUpper();
            string cellNameSuffix = (row + 1).ToString();
            string cellName = cellNamePrefix + cellNameSuffix;

            window.CellNameField = cellName;

            window.CellValueField = sheet.GetCellValue(cellName).ToString();

            

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

        }
        public void SaveFileHandler(String filename)
        {
            //FORCED XML FILETYPE. ADJUST TO MEET SPEC!
            // .ss filetype?
            filename = filename + ".xml";
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
