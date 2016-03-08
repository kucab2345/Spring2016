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
        /// <summary>
        /// Message dialogue box property
        /// </summary>
        public string Message
        {
            set
            {
                MessageBox.Show(value);
            }
        }
        /// <summary>
        /// Field input for CellNameBox
        /// </summary>
        public string CellNameField
        {
            set
            {
                CellNameBox.Text = value;
            }
        }
        /// <summary>
        /// Field input for CellContentField
        /// </summary>
        public string CellContentField
        {
            set
            {
                CellContentBox.Text = value;
            }
        }
        /// <summary>
        /// Field input for CellValueField
        /// </summary>
        public string CellValueField
        {
            set
            {
                CellValueBox.Text = value;
            }
        }
        /// <summary>
        /// Form title property
        /// </summary>
        public string Title
        {
            set
            {
                Text = value;
            }
        }
        /// <summary>
        /// Property for currently opened file
        /// </summary>
        public string currentFile { get; set; }
        /// <summary>
        /// Bool that gets passed ot SaveHandler in controller to distinguish between save and saveas
        /// </summary>
        public bool saveAsBool { get; set; }
        /// <summary>
        /// corresponds to the models spreadsheet.Changed variable
        /// </summary>
        public bool fileChanged { get; set; }

        /// <summary>
        /// Constructor for SpreadsheetGUI. Creates form, displays panels, and sets title to untitled.
        /// </summary>
        public SpreadsheetGUI()
        {
            InitializeComponent();
            spreadsheetPanel2.SelectionChanged += displaySelection;
            Title = "untitled";
        }
        /// <summary>
        /// Receives event if user clicks on a cell in the panel. Derived from professors demo
        /// </summary>
        /// <param name="sender"></param>
        private void displaySelection(SpreadsheetPanel sender)
        {
            int row, col;
            sender.GetSelection(out col, out row);

            if(ChangeSelectionEvent != null)
            {
                ChangeSelectionEvent(col,row);
            }
        }
        /// <summary>
        /// NewWindowEvent declaration. Called when loading a new file or hitting File>New
        /// </summary>
        public event Action NewWindowEvent;
        /// <summary>
        /// CloseWindowEvent. Called when closing with red X or File>Close
        /// </summary>
        public event Action CloseWindowEvent;
        /// <summary>
        /// ChangeSelectionEvent. Called when user selects cell on panel
        /// </summary>
        public event Action<int,int> ChangeSelectionEvent;
        /// <summary>
        /// FileChosenEvent. Called when FileDialogue successfully loads receives a valid file address
        /// </summary>
        public event Action<string> FileChosenEvent;
        /// <summary>
        /// SaveFileEvent. Called whenever user hits File>Save or File>SaveAs
        /// </summary>
        public event Action<string,bool> SaveFileEvent;
        /// <summary>
        /// ChangeContentEvent. Called whenever user changes the contents of currently selected cell
        /// </summary>
        public event Action<string, int, int> ChangeContentEvent;
        /// <summary>
        /// Fires New Window event on click of File>New
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)//FILE>NEW
        {
            if (NewWindowEvent != null)
            {
                NewWindowEvent();
            }
        }
        /// <summary>
        /// Handles creation of new Window. Communicates with ApplicationContext to ensure new window is added
        /// </summary>
        public void CreateNewWindowHandler()
        {
            SpreadsheetApplicationContext.GetContext().RunNew();
        }
        /// <summary>
        /// Fires CloseWindowEvent on when user hits File>Close
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)//FILE>CLOSE
        {
            if(CloseWindowEvent != null)
            {
                CloseWindowEvent();
            }
        }
        /// <summary>
        /// Handles closing of window
        /// </summary>
        /// <param name="closingSave"></param>
        public void CloseCurrentWindowHandler(bool closingSave)
        {
            Close();
        }
        /// <summary>
        /// Creates a new window through SpreadsheetApplicationContext's overloaded RunNew. Passes filename
        /// through RunNew to an overloaded Controller constructor that creates a new, blank spreadsheet, and then
        /// loads in the data with the carried over filename
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)//File > Open
        {
            DialogResult result = FileDialogueBox.ShowDialog();
            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                //if (FileChosenEvent != null)
                //{
                    //FileChosenEvent(FileDialogueBox.FileName);
                    SpreadsheetApplicationContext.GetContext().RunNew(FileDialogueBox.FileName);
                //}
                if(ChangeSelectionEvent != null)
                {
                    ChangeSelectionEvent(0, 0);
                }
            }
        }
        /// <summary>
        /// Fires ChangeContentEvent. Passes the column, row, and new contents of the cell in question
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Fires SaveFileEvent and determines if the file being written to is the same as the current file.
        /// Handles whether or not we should treat the current save as a "save as" or just a regular save.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (currentFile != null)//if current file is the same as the destination file, overwrite w/o permission
            {
                if (SaveFileEvent != null)
                {
                    SaveFileEvent(currentFile, saveAsBool = false);
                    //SaveFileEvent(SaveDialogueBox.FileName,saveAsBool = false);
                }
            }
            else//otherwise, treat this as a save as
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
        /// <summary>
        /// Selected A1 in the panel on opening of a new spreadsheet. Fulfills requirement
        /// that a cell be selected at all times
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void SpreadsheetGUI_Load(object sender, EventArgs e)
        {
            if(ChangeSelectionEvent != null)
            {
                ChangeSelectionEvent(0, 0);
            }
        }
        /// <summary>
        /// Updates the panel. Controller methods use this to update all cells in question
        /// after a parent cell changes
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="col"></param>
        /// <param name="row"></param>
        public void Update(string obj, int col, int row)
        {
            spreadsheetPanel2.SetValue(col, row, obj);
        }
        /// <summary>
        /// Help menu dialogue
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Standard save as treatment. Does not care whether user has saved before or not.
        /// Prompts user for destination name and asks for overwrite permission if destination already
        /// exists.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// Prompts user, letting them know that any unsaved changes will be lost on closing. Fired
        /// whenever user attempts to close a form, via File > Close, the red X in the corner, Alt-F4, etc
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            if(fileChanged == true)
            {
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
        /// <summary>
        /// File dialogue method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileDialogueBox_FileOk(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Save Dialogue method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveDialogueBox_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}
