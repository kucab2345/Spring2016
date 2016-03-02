namespace SpreadsheetGUI
{
    partial class SpreadsheetGUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.MenuStrip = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.NewButton = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveButton = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSource3 = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSource4 = new System.Windows.Forms.BindingSource(this.components);
            this.spreadsheetPanel2 = new SSGui.SpreadsheetPanel();
            this.FileDialogueBox = new System.Windows.Forms.OpenFileDialog();
            this.CellContentsLabel = new System.Windows.Forms.Label();
            this.CellValueLabel = new System.Windows.Forms.Label();
            this.CellValueBox = new System.Windows.Forms.TextBox();
            this.CellNameLabel = new System.Windows.Forms.Label();
            this.CellNameBox = new System.Windows.Forms.TextBox();
            this.SaveDialogueBox = new System.Windows.Forms.SaveFileDialog();
            this.CellContentBox = new System.Windows.Forms.TextBox();
            this.MenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource4)).BeginInit();
            this.SuspendLayout();
            // 
            // MenuStrip
            // 
            this.MenuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.MenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip.Name = "MenuStrip";
            this.MenuStrip.Size = new System.Drawing.Size(45, 24);
            this.MenuStrip.TabIndex = 1;
            this.MenuStrip.Text = "MenuStrip";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.NewButton,
            this.SaveButton,
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
            this.toolStripMenuItem1.Text = "File";
            // 
            // NewButton
            // 
            this.NewButton.Name = "NewButton";
            this.NewButton.Size = new System.Drawing.Size(112, 22);
            this.NewButton.Text = "New...";
            this.NewButton.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(112, 22);
            this.SaveButton.Text = "Save...";
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.openToolStripMenuItem.Text = "Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.closeToolStripMenuItem.Text = "Close...";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // spreadsheetPanel2
            // 
            this.spreadsheetPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.spreadsheetPanel2.Location = new System.Drawing.Point(0, 27);
            this.spreadsheetPanel2.Name = "spreadsheetPanel2";
            this.spreadsheetPanel2.Size = new System.Drawing.Size(845, 416);
            this.spreadsheetPanel2.TabIndex = 2;
            // 
            // FileDialogueBox
            // 
            this.FileDialogueBox.DefaultExt = "ss";
            this.FileDialogueBox.FileName = "FileDialogueBox";
            this.FileDialogueBox.Filter = "ss files|*.ss|All files|*.*";
            this.FileDialogueBox.FileOk += new System.ComponentModel.CancelEventHandler(this.FileDialogueBox_FileOk);
            // 
            // CellContentsLabel
            // 
            this.CellContentsLabel.AutoSize = true;
            this.CellContentsLabel.Location = new System.Drawing.Point(511, 9);
            this.CellContentsLabel.Name = "CellContentsLabel";
            this.CellContentsLabel.Size = new System.Drawing.Size(69, 13);
            this.CellContentsLabel.TabIndex = 4;
            this.CellContentsLabel.Text = "Cell Contents";
            // 
            // CellValueLabel
            // 
            this.CellValueLabel.AutoSize = true;
            this.CellValueLabel.Location = new System.Drawing.Point(326, 9);
            this.CellValueLabel.Name = "CellValueLabel";
            this.CellValueLabel.Size = new System.Drawing.Size(54, 13);
            this.CellValueLabel.TabIndex = 5;
            this.CellValueLabel.Text = "Cell Value";
            // 
            // CellValueBox
            // 
            this.CellValueBox.Location = new System.Drawing.Point(386, 2);
            this.CellValueBox.Name = "CellValueBox";
            this.CellValueBox.ReadOnly = true;
            this.CellValueBox.Size = new System.Drawing.Size(100, 20);
            this.CellValueBox.TabIndex = 6;
            // 
            // CellNameLabel
            // 
            this.CellNameLabel.AutoSize = true;
            this.CellNameLabel.Location = new System.Drawing.Point(142, 9);
            this.CellNameLabel.Name = "CellNameLabel";
            this.CellNameLabel.Size = new System.Drawing.Size(55, 13);
            this.CellNameLabel.TabIndex = 7;
            this.CellNameLabel.Text = "Cell Name";
            // 
            // CellNameBox
            // 
            this.CellNameBox.Location = new System.Drawing.Point(203, 2);
            this.CellNameBox.Name = "CellNameBox";
            this.CellNameBox.ReadOnly = true;
            this.CellNameBox.Size = new System.Drawing.Size(100, 20);
            this.CellNameBox.TabIndex = 8;
            // 
            // SaveDialogueBox
            // 
            this.SaveDialogueBox.DefaultExt = "ss";
            this.SaveDialogueBox.Filter = "ss files|*.ss|All files|*.*";
            this.SaveDialogueBox.FileOk += new System.ComponentModel.CancelEventHandler(this.SaveDialogueBox_FileOk);
            // 
            // CellContentBox
            // 
            this.CellContentBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CellContentBox.Location = new System.Drawing.Point(597, 3);
            this.CellContentBox.Name = "CellContentBox";
            this.CellContentBox.Size = new System.Drawing.Size(100, 20);
            this.CellContentBox.TabIndex = 9;
            this.CellContentBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CellContentBox_TextChanged);
            // 
            // SpreadsheetGUI
            // 
            this.ClientSize = new System.Drawing.Size(845, 446);
            this.Controls.Add(this.CellContentBox);
            this.Controls.Add(this.CellNameBox);
            this.Controls.Add(this.CellNameLabel);
            this.Controls.Add(this.CellValueBox);
            this.Controls.Add(this.CellValueLabel);
            this.Controls.Add(this.CellContentsLabel);
            this.Controls.Add(this.spreadsheetPanel2);
            this.Controls.Add(this.MenuStrip);
            this.MainMenuStrip = this.MenuStrip;
            this.Name = "SpreadsheetGUI";
            this.Load += new System.EventHandler(this.SpreadsheetGUI_Load);
            this.MenuStrip.ResumeLayout(false);
            this.MenuStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SSGui.SpreadsheetPanel spreadsheetPanel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem OpenButton;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private SSGui.SpreadsheetPanel spreadsheetPanel;
        private System.Windows.Forms.MenuStrip MenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem SaveButton;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.BindingSource bindingSource2;
        private System.Windows.Forms.BindingSource bindingSource3;
        private System.Windows.Forms.BindingSource bindingSource4;
        private SSGui.SpreadsheetPanel spreadsheetPanel2;
        private System.Windows.Forms.ToolStripMenuItem NewButton;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog FileDialogueBox;
        private System.Windows.Forms.Label CellContentsLabel;
        private System.Windows.Forms.Label CellValueLabel;
        private System.Windows.Forms.TextBox CellValueBox;
        private System.Windows.Forms.Label CellNameLabel;
        private System.Windows.Forms.TextBox CellNameBox;
        private System.Windows.Forms.SaveFileDialog SaveDialogueBox;
        private System.Windows.Forms.TextBox CellContentBox;
    }
}

