namespace UkuleleEditor
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            splitContainer = new SplitContainer();
            panelLeft = new Panel();
            groupBoxLineList = new GroupBox();
            listBoxLines = new ListBox();
            panelLineButtons = new Panel();
            btnMoveLineDown = new Button();
            btnMoveLineUp = new Button();
            btnDeleteLine = new Button();
            btnAddLine = new Button();
            groupBoxItemEditor = new GroupBox();
            dgvItems = new DataGridView();
            colString = new DataGridViewTextBoxColumn();
            colFret = new DataGridViewTextBoxColumn();
            colLyric = new DataGridViewTextBoxColumn();
            panelItemButtons = new Panel();
            btnMoveItemDown = new Button();
            btnMoveItemUp = new Button();
            btnDeleteItem = new Button();
            btnAddItem = new Button();
            panelRight = new Panel();
            pictureBoxPreview = new PictureBox();
            panelPreviewButtons = new Panel();
            btnExport = new Button();
            btnRefresh = new Button();
            menuStrip = new MenuStrip();
            menuFile = new ToolStripMenuItem();
            menuNew = new ToolStripMenuItem();
            menuOpen = new ToolStripMenuItem();
            menuSave = new ToolStripMenuItem();
            menuSaveAs = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            menuExit = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            panelLeft.SuspendLayout();
            groupBoxLineList.SuspendLayout();
            panelLineButtons.SuspendLayout();
            groupBoxItemEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvItems).BeginInit();
            panelItemButtons.SuspendLayout();
            panelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxPreview).BeginInit();
            panelPreviewButtons.SuspendLayout();
            menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(0, 25);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(panelLeft);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(panelRight);
            splitContainer.Size = new Size(1200, 675);
            splitContainer.SplitterDistance = 500;
            splitContainer.TabIndex = 0;
            // 
            // panelLeft
            // 
            panelLeft.Controls.Add(groupBoxItemEditor);
            panelLeft.Controls.Add(groupBoxLineList);
            panelLeft.Dock = DockStyle.Fill;
            panelLeft.Location = new Point(0, 0);
            panelLeft.Name = "panelLeft";
            panelLeft.Padding = new Padding(5);
            panelLeft.Size = new Size(500, 675);
            panelLeft.TabIndex = 0;
            // 
            // groupBoxLineList
            // 
            groupBoxLineList.Controls.Add(listBoxLines);
            groupBoxLineList.Controls.Add(panelLineButtons);
            groupBoxLineList.Dock = DockStyle.Top;
            groupBoxLineList.Location = new Point(5, 5);
            groupBoxLineList.Name = "groupBoxLineList";
            groupBoxLineList.Padding = new Padding(8);
            groupBoxLineList.Size = new Size(490, 200);
            groupBoxLineList.TabIndex = 0;
            groupBoxLineList.TabStop = false;
            groupBoxLineList.Text = "曲谱行列表";
            // 
            // listBoxLines
            // 
            listBoxLines.Dock = DockStyle.Fill;
            listBoxLines.FormattingEnabled = true;
            listBoxLines.ItemHeight = 17;
            listBoxLines.Location = new Point(8, 24);
            listBoxLines.Name = "listBoxLines";
            listBoxLines.Size = new Size(474, 128);
            listBoxLines.TabIndex = 0;
            listBoxLines.SelectedIndexChanged += ListBoxLines_SelectedIndexChanged;
            // 
            // panelLineButtons
            // 
            panelLineButtons.Controls.Add(btnMoveLineDown);
            panelLineButtons.Controls.Add(btnMoveLineUp);
            panelLineButtons.Controls.Add(btnDeleteLine);
            panelLineButtons.Controls.Add(btnAddLine);
            panelLineButtons.Dock = DockStyle.Bottom;
            panelLineButtons.Location = new Point(8, 152);
            panelLineButtons.Name = "panelLineButtons";
            panelLineButtons.Size = new Size(474, 40);
            panelLineButtons.TabIndex = 1;
            // 
            // btnMoveLineDown
            // 
            btnMoveLineDown.Location = new Point(246, 5);
            btnMoveLineDown.Name = "btnMoveLineDown";
            btnMoveLineDown.Size = new Size(75, 30);
            btnMoveLineDown.TabIndex = 3;
            btnMoveLineDown.Text = "下移 ↓";
            btnMoveLineDown.UseVisualStyleBackColor = true;
            btnMoveLineDown.Click += BtnMoveLineDown_Click;
            // 
            // btnMoveLineUp
            // 
            btnMoveLineUp.Location = new Point(165, 5);
            btnMoveLineUp.Name = "btnMoveLineUp";
            btnMoveLineUp.Size = new Size(75, 30);
            btnMoveLineUp.TabIndex = 2;
            btnMoveLineUp.Text = "上移 ↑";
            btnMoveLineUp.UseVisualStyleBackColor = true;
            btnMoveLineUp.Click += BtnMoveLineUp_Click;
            // 
            // btnDeleteLine
            // 
            btnDeleteLine.Location = new Point(84, 5);
            btnDeleteLine.Name = "btnDeleteLine";
            btnDeleteLine.Size = new Size(75, 30);
            btnDeleteLine.TabIndex = 1;
            btnDeleteLine.Text = "删除行";
            btnDeleteLine.UseVisualStyleBackColor = true;
            btnDeleteLine.Click += BtnDeleteLine_Click;
            // 
            // btnAddLine
            // 
            btnAddLine.Location = new Point(3, 5);
            btnAddLine.Name = "btnAddLine";
            btnAddLine.Size = new Size(75, 30);
            btnAddLine.TabIndex = 0;
            btnAddLine.Text = "添加行";
            btnAddLine.UseVisualStyleBackColor = true;
            btnAddLine.Click += BtnAddLine_Click;
            // 
            // groupBoxItemEditor
            // 
            groupBoxItemEditor.Controls.Add(dgvItems);
            groupBoxItemEditor.Controls.Add(panelItemButtons);
            groupBoxItemEditor.Dock = DockStyle.Fill;
            groupBoxItemEditor.Location = new Point(5, 205);
            groupBoxItemEditor.Name = "groupBoxItemEditor";
            groupBoxItemEditor.Padding = new Padding(8);
            groupBoxItemEditor.Size = new Size(490, 465);
            groupBoxItemEditor.TabIndex = 1;
            groupBoxItemEditor.TabStop = false;
            groupBoxItemEditor.Text = "当前行音符编辑";
            // 
            // dgvItems
            // 
            dgvItems.AllowUserToResizeRows = false;
            dgvItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvItems.Columns.AddRange(new DataGridViewColumn[] { colString, colFret, colLyric });
            dgvItems.Dock = DockStyle.Fill;
            dgvItems.Location = new Point(8, 24);
            dgvItems.Name = "dgvItems";
            dgvItems.RowHeadersWidth = 51;
            dgvItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvItems.Size = new Size(474, 393);
            dgvItems.TabIndex = 0;
            dgvItems.CellValueChanged += DgvItems_CellValueChanged;
            // 
            // colString
            // 
            colString.DataPropertyName = "String";
            colString.HeaderText = "弦 (1-4)";
            colString.MinimumWidth = 6;
            colString.Name = "colString";
            colString.Width = 80;
            // 
            // colFret
            // 
            colFret.DataPropertyName = "Fret";
            colFret.HeaderText = "品 (0-12)";
            colFret.MinimumWidth = 6;
            colFret.Name = "colFret";
            colFret.Width = 80;
            // 
            // colLyric
            // 
            colLyric.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colLyric.DataPropertyName = "Lyric";
            colLyric.HeaderText = "歌词";
            colLyric.MinimumWidth = 6;
            colLyric.Name = "colLyric";
            // 
            // panelItemButtons
            // 
            panelItemButtons.Controls.Add(btnMoveItemDown);
            panelItemButtons.Controls.Add(btnMoveItemUp);
            panelItemButtons.Controls.Add(btnDeleteItem);
            panelItemButtons.Controls.Add(btnAddItem);
            panelItemButtons.Dock = DockStyle.Bottom;
            panelItemButtons.Location = new Point(8, 417);
            panelItemButtons.Name = "panelItemButtons";
            panelItemButtons.Size = new Size(474, 40);
            panelItemButtons.TabIndex = 1;
            // 
            // btnMoveItemDown
            // 
            btnMoveItemDown.Location = new Point(246, 5);
            btnMoveItemDown.Name = "btnMoveItemDown";
            btnMoveItemDown.Size = new Size(75, 30);
            btnMoveItemDown.TabIndex = 3;
            btnMoveItemDown.Text = "下移 ↓";
            btnMoveItemDown.UseVisualStyleBackColor = true;
            btnMoveItemDown.Click += BtnMoveItemDown_Click;
            // 
            // btnMoveItemUp
            // 
            btnMoveItemUp.Location = new Point(165, 5);
            btnMoveItemUp.Name = "btnMoveItemUp";
            btnMoveItemUp.Size = new Size(75, 30);
            btnMoveItemUp.TabIndex = 2;
            btnMoveItemUp.Text = "上移 ↑";
            btnMoveItemUp.UseVisualStyleBackColor = true;
            btnMoveItemUp.Click += BtnMoveItemUp_Click;
            // 
            // btnDeleteItem
            // 
            btnDeleteItem.Location = new Point(84, 5);
            btnDeleteItem.Name = "btnDeleteItem";
            btnDeleteItem.Size = new Size(75, 30);
            btnDeleteItem.TabIndex = 1;
            btnDeleteItem.Text = "删除";
            btnDeleteItem.UseVisualStyleBackColor = true;
            btnDeleteItem.Click += BtnDeleteItem_Click;
            // 
            // btnAddItem
            // 
            btnAddItem.Location = new Point(3, 5);
            btnAddItem.Name = "btnAddItem";
            btnAddItem.Size = new Size(75, 30);
            btnAddItem.TabIndex = 0;
            btnAddItem.Text = "添加";
            btnAddItem.UseVisualStyleBackColor = true;
            btnAddItem.Click += BtnAddItem_Click;
            // 
            // panelRight
            // 
            panelRight.Controls.Add(pictureBoxPreview);
            panelRight.Controls.Add(panelPreviewButtons);
            panelRight.Dock = DockStyle.Fill;
            panelRight.Location = new Point(0, 0);
            panelRight.Name = "panelRight";
            panelRight.Padding = new Padding(5);
            panelRight.Size = new Size(696, 675);
            panelRight.TabIndex = 0;
            // 
            // pictureBoxPreview
            // 
            pictureBoxPreview.BackColor = Color.White;
            pictureBoxPreview.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxPreview.Dock = DockStyle.Fill;
            pictureBoxPreview.Location = new Point(5, 5);
            pictureBoxPreview.Name = "pictureBoxPreview";
            pictureBoxPreview.Size = new Size(686, 625);
            pictureBoxPreview.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxPreview.TabIndex = 0;
            pictureBoxPreview.TabStop = false;
            // 
            // panelPreviewButtons
            // 
            panelPreviewButtons.Controls.Add(btnExport);
            panelPreviewButtons.Controls.Add(btnRefresh);
            panelPreviewButtons.Dock = DockStyle.Bottom;
            panelPreviewButtons.Location = new Point(5, 630);
            panelPreviewButtons.Name = "panelPreviewButtons";
            panelPreviewButtons.Size = new Size(686, 40);
            panelPreviewButtons.TabIndex = 1;
            // 
            // btnExport
            // 
            btnExport.Location = new Point(84, 5);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(100, 30);
            btnExport.TabIndex = 1;
            btnExport.Text = "导出图片";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += BtnExport_Click;
            // 
            // btnRefresh
            // 
            btnRefresh.Location = new Point(3, 5);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(75, 30);
            btnRefresh.TabIndex = 0;
            btnRefresh.Text = "刷新";
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += BtnRefresh_Click;
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { menuFile });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(1200, 25);
            menuStrip.TabIndex = 1;
            menuStrip.Text = "menuStrip1";
            // 
            // menuFile
            // 
            menuFile.DropDownItems.AddRange(new ToolStripItem[] { menuNew, menuOpen, menuSave, menuSaveAs, toolStripSeparator1, menuExit });
            menuFile.Name = "menuFile";
            menuFile.Size = new Size(58, 21);
            menuFile.Text = "文件(&F)";
            // 
            // menuNew
            // 
            menuNew.Name = "menuNew";
            menuNew.ShortcutKeys = Keys.Control | Keys.N;
            menuNew.Size = new Size(190, 22);
            menuNew.Text = "新建(&N)";
            menuNew.Click += MenuNew_Click;
            // 
            // menuOpen
            // 
            menuOpen.Name = "menuOpen";
            menuOpen.ShortcutKeys = Keys.Control | Keys.O;
            menuOpen.Size = new Size(190, 22);
            menuOpen.Text = "打开(&O)";
            menuOpen.Click += MenuOpen_Click;
            // 
            // menuSave
            // 
            menuSave.Name = "menuSave";
            menuSave.ShortcutKeys = Keys.Control | Keys.S;
            menuSave.Size = new Size(190, 22);
            menuSave.Text = "保存(&S)";
            menuSave.Click += MenuSave_Click;
            // 
            // menuSaveAs
            // 
            menuSaveAs.Name = "menuSaveAs";
            menuSaveAs.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
            menuSaveAs.Size = new Size(190, 22);
            menuSaveAs.Text = "另存为(&A)";
            menuSaveAs.Click += MenuSaveAs_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(187, 6);
            // 
            // menuExit
            // 
            menuExit.Name = "menuExit";
            menuExit.Size = new Size(190, 22);
            menuExit.Text = "退出(&X)";
            menuExit.Click += MenuExit_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1200, 700);
            Controls.Add(splitContainer);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Name = "MainForm";
            Text = "尤克里里编辑器";
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            panelLeft.ResumeLayout(false);
            groupBoxLineList.ResumeLayout(false);
            panelLineButtons.ResumeLayout(false);
            groupBoxItemEditor.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvItems).EndInit();
            panelItemButtons.ResumeLayout(false);
            panelRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxPreview).EndInit();
            panelPreviewButtons.ResumeLayout(false);
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private SplitContainer splitContainer;
        private Panel panelLeft;
        private GroupBox groupBoxLineList;
        private ListBox listBoxLines;
        private Panel panelLineButtons;
        private Button btnAddLine;
        private Button btnDeleteLine;
        private Button btnMoveLineUp;
        private Button btnMoveLineDown;
        private GroupBox groupBoxItemEditor;
        private DataGridView dgvItems;
        private Panel panelItemButtons;
        private Button btnAddItem;
        private Button btnDeleteItem;
        private Button btnMoveItemUp;
        private Button btnMoveItemDown;
        private Panel panelRight;
        private PictureBox pictureBoxPreview;
        private Panel panelPreviewButtons;
        private Button btnRefresh;
        private Button btnExport;
        private DataGridViewTextBoxColumn colString;
        private DataGridViewTextBoxColumn colFret;
        private DataGridViewTextBoxColumn colLyric;
        private MenuStrip menuStrip;
        private ToolStripMenuItem menuFile;
        private ToolStripMenuItem menuNew;
        private ToolStripMenuItem menuOpen;
        private ToolStripMenuItem menuSave;
        private ToolStripMenuItem menuSaveAs;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem menuExit;
    }
}
