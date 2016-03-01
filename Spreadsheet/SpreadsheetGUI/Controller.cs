using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SSGui;
using SpreadsheetGUI;


namespace SpreadsheetGUI
{
    class Controller
    {
        ISSInterface window; 
        public Controller(ISSInterface view)
        {
            window = view;
            window.NewWindow += NewWindow;
        }
        public void NewWindow()
        {
            window.CreateNewWindow();
        }
    }
}
