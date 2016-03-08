using System;
using SpreadsheetGUI;

namespace SpreadsheetGUIStub
{
    class SpreadsheetStub : ISSInterface
    // These two properties record whether a method has been called
    {
        public string CellContentField { get; set; }

        public string CellNameField { get; set; }

        public string CellValueField { get; set; }

        public string currentFile { get; set; }

        public bool fileChanged { get; set; }

        public string Message { get; set; }

        public string Title { get; set; }

        public event Action<string, int, int> ChangeContentEvent;
        public event Action<int, int> ChangeSelectionEvent;
        public event Action CloseWindowEvent;
        public event Action<string> FileChosenEvent;
        public event Action NewWindowEvent;
        public event Action<string, bool> SaveFileEvent;


        public void FireNewWindowEvent()
        {
            if (NewWindowEvent != null)
            {
                NewWindowEvent();
            }
        }
        public void FireCloseWindowEvent()
        {
            if (CloseWindowEvent != null)
            {
                CloseWindowEvent();
            }
        }
        public void FireChangeContentEvent(string content, int a, int b)
        {
            if (ChangeContentEvent != null)
            {
                ChangeContentEvent(content, a, b);
            }
        }
        public void FireChangeSelectionEvent(int a, int b)
        {
            if (ChangeSelectionEvent != null)
            {
                ChangeSelectionEvent(a,b);
            }
        }
        public void FireFileChosenEvent(string filename)
        {
            if (FileChosenEvent != null)
            {
                FileChosenEvent(filename);
            }
        }
        public void FireSaveFileEvent(string destination,bool saveAsBool)
        {
            if (SaveFileEvent != null)
            {
                SaveFileEvent(destination,saveAsBool);
            }
        }
        public bool CalledCloseCurrentWindowHandler { get; set; }
        public bool CalledCreateNewWindowHandler { get; set; }

        public bool CalledUpdate { get; set; }


        public void CloseCurrentWindowHandler(bool closingSave)
        {
            CalledCloseCurrentWindowHandler = true;
        }

        public void CreateNewWindowHandler()
        {
            CalledCreateNewWindowHandler = true;
        }

        public void Update(string obj, int col, int row)
        {
            CalledUpdate = true;
        }
    }
}