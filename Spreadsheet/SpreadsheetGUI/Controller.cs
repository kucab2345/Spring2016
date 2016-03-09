using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGui;
using System.IO;
using SpreadsheetGUI;
using SS;
using System.Xml;
using System.Diagnostics;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Acts as the middle man between the view and model. Model is not aware of the Controller at all and neither
    /// is the view (SpreadsheetGUI). Contains a spreadsheet and an interface instance variable, interface inherits
    /// from Interface.cs
    public class Controller
    {

        /// <summary>
        /// Declaring an AbstractSpreadSheet Sheet that will eventually be assigned to a
        /// Spreadsheet object
        /// </summary>
        AbstractSpreadsheet sheet;
        /// <summary>
        /// Interface the outlines the event Actions and methods that the view implements.
        /// </summary>
        ISSInterface window;
        /// <summary>
        /// Controller constructor that only takes in the view. Useful for NEW windows and NEW EMPTY spreadsheets.
        /// Sets sheet to a new Spreadsheet
        /// </summary>
        /// <param name="view"></param>
        public Controller(ISSInterface view)//Create a new, blank spreadsheet
        {
            sheet = new Spreadsheet(new System.Text.RegularExpressions.Regex(@"^[A-Z]+[1-9]{1}[0-9]{0,1}$"));
            
            //Registering methods to events
            window = view;
            window.NewWindowEvent += NewWindow;
            window.CloseWindowEvent += CloseWindow;
            window.FileChosenEvent += OpenFile;
            window.SaveFileEvent += SaveFile;
            window.ChangeSelectionEvent += ChangeSelectedCellHandler;
            window.ChangeContentEvent += ChangeContentEventHandler;

            window.fileChanged = sheet.Changed;
        }
        /// <summary>
        /// Controller constructor that only takes in the view. Useful for loading in a spreadsheet file
        /// Sets sheet to a new Spreadsheet and immediately calls FileChosenHandler to load in the fileName's contents
        /// </summary>
        public Controller(ISSInterface view,string fileName)//Create a new, spreadsheet and load in data from a .ss file
        {
            sheet = new Spreadsheet(new System.Text.RegularExpressions.Regex(@"^[A-Z]+[1-9]{1}[0-9]{0,1}$"));

            //Registering methods to events
            window = view;
            window.NewWindowEvent += NewWindow;
            window.CloseWindowEvent += CloseWindow;
            window.FileChosenEvent += OpenFile;
            window.SaveFileEvent += SaveFile;
            window.ChangeSelectionEvent += ChangeSelectedCellHandler;
            window.ChangeContentEvent += ChangeContentEventHandler;

            FileChosenHandler(fileName);
            window.fileChanged = sheet.Changed;
        }

        /// <summary>
        /// Handles the Selection of a new cell. Takes in the col and row
        /// </summary>
        /// <param name="col"></param>
        /// <param name="row"></param>
        public void ChangeSelectedCellHandler(int col, int row)//Handle Changing the selected cell
        {
            char prefixLetter = (char)(97 + col);//Get name of current cell through Unicode operations
            string cellNamePrefix = prefixLetter.ToString().ToUpper();
            string cellNameSuffix = (row + 1).ToString();
            string cellName = cellNamePrefix + cellNameSuffix;

            window.CellNameField = cellName;//Push cellname to cellname box

            window.CellValueField = sheet.GetCellValue(cellName).ToString();//Push cell value to cell value box

            object temp = sheet.GetCellContents(cellName);//Get contents of current cell

            if(!(temp is double) && !(temp is string) && !(temp is FormulaError))//if its not a double, string, or formula error, append = to it since it must be a Formula
            {
                window.CellContentField = "= " + temp.ToString();//and push it to the content window
            }
            else//otherwise, dont touch it and just put it to the content window
            {
                window.CellContentField = temp.ToString();
            }
            window.Update(sheet.GetCellValue(cellName).ToString(), col, row);
            window.fileChanged = sheet.Changed;
        }
        /// <summary>
        /// Handles the changing of a selected cell's contents. Adjusts the contents, finds all affected
        /// cells, and iterates over them, and calls the Update function to update their Values. Takes in the
        /// col, row, and cellContent that needs to be placed into the cell
        /// </summary>
        /// <param name="cellContent"></param>
        /// <param name="col"></param>
        /// <param name="row"></param>
        public void ChangeContentEventHandler(string cellContent, int col, int row)
        {
            char prefixLetter = (char)(97 + col);
            string cellNamePrefix = prefixLetter.ToString().ToUpper();
            string cellNameSuffix = (row + 1).ToString();
            string cellName = cellNamePrefix + cellNameSuffix;
            
            try
            {
                foreach (string i in sheet.SetContentsOfCell(cellName, cellContent.ToString()))
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
            catch (Exception e)
            {
                window.Message = e.Message;
                Debug.WriteLine(e); 
            }
            window.fileChanged = sheet.Changed;
        }
        /// <summary>
        /// Calls the correct SaveFileHandler based on the SaveFileEvent that was fired
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="saveAsBool"></param>
        public void SaveFile(string filename, bool saveAsBool)
        {
            if(sheet.Changed || saveAsBool)
            {
                SaveFileHandler(filename);
            }
        }
        
        /// <summary>
        /// Valls the correct CreateNewWindowHandler based on the CreateNewEvent that was fired. Calls to the view
        /// </summary>
        public void NewWindow()
        {
            window.fileChanged = false;
            window.CreateNewWindowHandler();
        }
        /// <summary>
        /// Calls the close current window handler in the view. Calls to the view
        /// </summary>
        public void CloseWindow()
        {
            bool closingSave = false;
            window.fileChanged = sheet.Changed;
            if (sheet.Changed == true)
            {
                window.CloseCurrentWindowHandler(closingSave = true);
            }
            else
            {
                window.CloseCurrentWindowHandler(closingSave);
            }
        }
        /// <summary>
        /// Calls the correct FileChosenHandler based on the OpenFileEvent that was fired
        /// </summary>
        /// <param name="filename"></param>
        public void OpenFile(string filename)
        {
            FileChosenHandler(filename);
            //SpreadsheetApplicationContext.GetContext().RunNew(filename);
        }
        /// <summary>
        /// Handles the opening of a file based on the filename. Called by the 2nd, overloaded Controller constructor
        /// </summary>
        /// <param name="filename"></param>
        public void FileChosenHandler(String filename)
        {
            try
            {
                using (TextReader inFile = File.OpenText(filename))
                {
                    sheet = new Spreadsheet(inFile);
                }
                window.currentFile = filename;
            }
            catch (Exception ex)
            {
                window.Message = "Unable to open file\n" + ex.Message;
            }
            foreach(string i in sheet.GetNamesOfAllNonemptyCells())//after loading in data, refresh the panel to show all cells
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
            window.fileChanged = sheet.Changed;
        }
        /// <summary>
        /// Handles the saving of a file given the filename.
        /// </summary>
        /// <param name="filename"></param>
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
            window.fileChanged = sheet.Changed;
        }
    }
}
