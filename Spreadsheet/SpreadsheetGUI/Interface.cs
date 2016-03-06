using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    public interface ISSInterface
    {
        /// <summary>
        /// Message box in GUI
        /// </summary>
        string Message { set; }
        /// <summary>
        /// Title of current window/form
        /// </summary>
        string Title { set; }
        /// <summary>
        /// CellNameField box
        /// </summary>
        string CellNameField { set; }
        /// <summary>
        /// CellContentField box
        /// </summary>
        string CellContentField { set; }
        /// <summary>
        /// CellValueField box
        /// </summary>
        string CellValueField { set; }
        /// <summary>
        /// bool that corresponds to the models spreadsheet.Changed
        /// </summary>
        bool fileChanged { get; set; }
        /// <summary>
        /// Registering the same events from the GUI file. See there for precise decriptions of each event
        /// </summary>
        event Action NewWindowEvent;
        event Action CloseWindowEvent;
        event Action<string,bool> SaveFileEvent;
        event Action<string> FileChosenEvent;
        event Action<int, int> ChangeSelectionEvent;
        event Action<string, int, int> ChangeContentEvent;
        /// <summary>
        /// Handler for new windows is implemented in the GUI file itself, since it requires nothing of the model in my implementation
        /// </summary>
        void CreateNewWindowHandler();
        /// <summary>
        /// CloseCurrentWindowHandler is implemented in the GUI since it requries nothing of the model in my implementation
        /// </summary>
        /// <param name="closingSave"></param>
        void CloseCurrentWindowHandler(bool closingSave);
        /// <summary>
        /// Update function that updates a cell defined with the passed in parameters.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="col"></param>
        /// <param name="row"></param>
        void Update(string obj, int col, int row);
        
    }
}
