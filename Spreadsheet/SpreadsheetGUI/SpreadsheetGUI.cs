using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SSGui;
using SpreadsheetGUI;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    public partial class SpreadsheetGUI : Form , ISSInterface
    {
        public SpreadsheetGUI()
        {
            InitializeComponent();
        }

        public event Action NewWindow;

        private void spreadsheetPanel_Load(object sender, EventArgs e)
        {
            
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (NewWindow != null)
            {
                NewWindow();
            }
        }
        public void CreateNewWindow()
        {
            SpreadsheetApplicationContext.GetContext().RunNew();
        }
    }
}
