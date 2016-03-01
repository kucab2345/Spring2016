using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGui;
using System.IO;
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
        public void OpenFile(string filename)
        {
            FileChosenHandler(filename);
        }
        public void FileChosenHandler(String filename)
        {
            try//Go back to PS6 tests to implement file reading system;
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
    }
}
