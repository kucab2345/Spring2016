using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    static class Launch
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //ApplicationContext version of launcher which allows for multiwindow setup
            var context = SpreadsheetApplicationContext.GetContext();
            SpreadsheetApplicationContext.GetContext().RunNew();
            Application.Run(context);
        }
    }
}
