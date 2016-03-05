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
        public string Message
        {
            set
            {
                MessageBox.Show(value);
            }
        }

        public string CellNameField
        {
            set
            {
                CellNameBox.Text = value;
            }
        }

        public string CellContentField
        {
            set
            {
                CellContentBox.Text = value;
            }
        }

        public string CellValueField
        {
            set
            {
                CellValueBox.Text = value;
            }
        }

        public string Title
        {
            set
            {
                Text = value;
            }
        }
        public string currentFile { get; set; }
        public bool saveAsBool { get; set; }

        public SpreadsheetGUI()
        {
            InitializeComponent();
            spreadsheetPanel2.SelectionChanged += displaySelection;
            Title = "untitled";
        }

        private void displaySelection(SpreadsheetPanel sender)
        {
            int row, col;
            sender.GetSelection(out col, out row);

            if(ChangeSelectionEvent != null)
            {
                ChangeSelectionEvent(col,row);
            }
        }

        public event Action NewWindowEvent;
        public event Action CloseWindowEvent;
        public event Action<int,int> ChangeSelectionEvent;
        public event Action<string> FileChosenEvent;
        public event Action<string,bool> SaveFileEvent;
        public event Action<string, int, int> ChangeContentEvent;

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
        public void CloseCurrentWindowHandler(bool closingSave)
        {
            if(closingSave == true)
            {
                DialogResult dialogResult = MessageBox.Show("You are attempting to close the file without saving your work.\nYes: Close out and lose unsaved changes\nNo: Cancel closing and return to program", "Exiting without saving", MessageBoxButtons.YesNo);
                if(dialogResult == DialogResult.Yes)
                {
                    Close();
                }
            }
            else
            {
                Close();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)//File > Open
        {
            DialogResult result = FileDialogueBox.ShowDialog();
            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                if (FileChosenEvent != null)
                {
                    SpreadsheetApplicationContext.GetContext().RunNew(FileDialogueBox.FileName);
                }
                if(ChangeSelectionEvent != null)
                {
                    ChangeSelectionEvent(0, 0);
                }
            }
        }
        private void CellContentBox_TextChanged(object sender, KeyEventArgs e)
        {
            int col, row;

            if(e.KeyCode == Keys.Enter)
            {
                if(ChangeContentEvent != null)
                {
                    spreadsheetPanel2.GetSelection(out col, out row);
                    ChangeContentEvent(CellContentBox.Text, col, row);
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (currentFile != null)
            {
                if (SaveFileEvent != null)
                {
                    SaveFileEvent(SaveDialogueBox.FileName,saveAsBool = false);
                }
            }
            else
            {
                try
                {
                    DialogResult result = SaveDialogueBox.ShowDialog();
                    currentFile = SaveDialogueBox.FileName;
                    if (result == DialogResult.Yes || result == DialogResult.OK)
                    {
                        if (SaveFileEvent != null)
                        {
                            SaveFileEvent(SaveDialogueBox.FileName,saveAsBool = true);
                        }
                    }
                }
                catch (Exception error)
                {
                    Message = error.Message;
                }
            }
            Title = currentFile;
        }
        private void FileDialogueBox_FileOk(object sender, EventArgs e)
        {

        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void SaveDialogueBox_FileOk(object sender, CancelEventArgs e)
        {
            
        }

        private void SpreadsheetGUI_Load(object sender, EventArgs e)
        {
            if(ChangeSelectionEvent != null)
            {
                ChangeSelectionEvent(0, 0);
            }
        }
        public void Update(string obj, int col, int row)
        {
            spreadsheetPanel2.SetValue(col, row, obj);
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = SaveDialogueBox.ShowDialog();
                currentFile = SaveDialogueBox.FileName;
                if (result == DialogResult.Yes || result == DialogResult.OK)
                {
                    if (SaveFileEvent != null)
                    {
                        SaveFileEvent(SaveDialogueBox.FileName, saveAsBool = true);
                    }
                }
            }
            catch (Exception error)
            {
                Message = error.Message;
            }
        }
    }
}
