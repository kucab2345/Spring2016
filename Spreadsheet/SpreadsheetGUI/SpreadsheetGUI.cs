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
            /*
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
            */
            Close();
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
            MessageBox.Show("OPERATION\n"
                + "==============================================\n"
                + "Click on a cell to select it\n"
                + "Type in the Cell Contents box and press ENTER to change its contents\n"
                + "Observe the value of the cell both on the grid and in the Cell Value box\n"
                + "*Cell's whose contents are formulas should begin with a preceeding '=' character. Ex) =A1 + 3\n"
                + "==============================================\n"
                + "File > New : Creates a new, untitled spreadsheet\n"
                + "File > Open : Opens a new spreadsheet (preferred .ss extension) into a new window\n"
                + "File > Save : Saves current spreadsheet. If spreadsheet has not been saves before, will prompt"
                + "user for initial filename. Does not ask overwrite permission if saving ontop of current file.\n"
                + "File > Save As : Saves a copy of the current file under a new, user specified name. Asks permission"
                + "if source name is identical to current name.\n"
                + "File > Close : Closes current spreadsheet. Always prompts user, letting them know that unsaved changes will be lost\n");
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
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            // Confirm user wants to close
            switch (MessageBox.Show(this, "Are you sure you want to close?\nYou will lose any unsaved changes.", "Closing", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    e.Cancel = true;
                    break;
                default:
                    break;
            }
        }
    }
}
