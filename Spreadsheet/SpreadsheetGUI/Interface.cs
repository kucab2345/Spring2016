using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    public interface ISSInterface
    {
        event Action NewWindowEvent;
        event Action CloseWindowEvent;
        event Action<string> FileChosenEvent;
        void CreateNewWindowHandler();
        void CloseCurrentWindowHandler();
    }
}
