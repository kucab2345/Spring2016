﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    public interface ISSInterface
    {
        event Action NewWindow;
        void CreateNewWindow();
    }
}
