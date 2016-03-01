using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    public interface Interface
    {
        event Action<string> FileChosenEvent;

        event Action<string> CountEvent;

        event Action CloseEvent;

        event Action NewEvent;

        string Title { set; }

        string Message { set; }

        void DoClose();

        void OpenNew();

        void ReadSpreadsheet();??

    }
}
