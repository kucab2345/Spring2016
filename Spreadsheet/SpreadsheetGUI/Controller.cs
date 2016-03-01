using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGui;
using SpreadsheetGUI;
using SS;


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
        }
        public void NewWindow()
        {
            window.CreateNewWindowHandler();
        }
        public void CloseWindow()
        {
            window.CloseCurrentWindowHandler();
        }
        public string OpenFile()
        {
            
        }
        public void FileChosenHandler(String filename)
        {
            try
            {
                model.ReadFile(filename);
                window.CharCount = model.CountChars();
                window.WordCount = model.CountWords();
                window.LineCount = model.CountLines();
                window.SubstringCount = 0;
                window.SearchString = "";
                window.Title = filename;
            }
            catch (Exception ex)
            {
                window.Message = "Unable to open file\n" + ex.Message;
            }
        }
    }
}
