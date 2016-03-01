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

        public event Action NewWindowEvent;
        public event Action CloseWindowEvent;
        public event Action<string> FileChosenEvent;

        private void newToolStripMenuItem_Click(object sender, EventArgs e)//FILE>NEW
        {
            if (NewWindowEvent != null)
            {
                NewWindowEvent();
            }
        }
        public void CreateNewWindowHandler()
        {
            SpreadsheetApplicationContext.GetContext().RunNew();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)//FILE>CLOSE
        {
            if(CloseWindowEvent != null)
            {
                CloseWindowEvent();
            }
        }
        public void CloseCurrentWindowHandler()
        {
            //Added in a check to make sure user has saved
            Close();
        }
        private void FileDialogueBox_FileOk(object sender, EventArgs e)
        {

        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)//File > Open
        {
            DialogResult result = FileDialogueBox.ShowDialog();
            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                if (FileChosenEvent != null)
                {
                    FileChosenEvent(FileDialogueBox.FileName);
                }
            }
        }
    }
}
