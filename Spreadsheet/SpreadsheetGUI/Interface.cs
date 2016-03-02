using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    public interface ISSInterface
    {
        string Message { set; }
        string Title { set; }
        string CellNameField { set; }
        string CellContentField { set; }
        string CellValueField { set; }
        event Action NewWindowEvent;
        event Action CloseWindowEvent;
        event Action<string> SaveFileEvent;
        event Action<string> FileChosenEvent;
        event Action<int, int> ChangeSelectionEvent;
        event Action<string, int, int> ChangeContentEvent;
        void CreateNewWindowHandler();
        void CloseCurrentWindowHandler();
        void Update(string obj, int col, int row);
        
    }
}
