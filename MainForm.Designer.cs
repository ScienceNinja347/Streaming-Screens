namespace DesktopSetupV2
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.makeColumnsVisibleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addRowToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.rBNoLog = new System.Windows.Forms.RadioButton();
            this.rBMain = new System.Windows.Forms.RadioButton();
            this.rBAll = new System.Windows.Forms.RadioButton();
            this.groupBoxLL = new System.Windows.Forms.GroupBox();
            this.btnLiveLogging = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.synchronizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.launchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addRowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optimizeColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.makeColumnsVisibleToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.killCommanderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listBoxTemplates = new System.Windows.Forms.ListBox();
            this.buttonCreateFile = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOpenCommandFolder = new System.Windows.Forms.Button();
            this.ButtonRestore = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.ClId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClElapsed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClCommandLine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClArguments = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClWidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClHeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClXLoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClYLoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClMoveAfter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClDefaultSize = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ClAdded = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStrip1.SuspendLayout();
            this.groupBoxLL.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeColumnsToolStripMenuItem,
            this.makeColumnsVisibleToolStripMenuItem,
            this.addRowToolStripMenuItem1,
            this.startToolStripMenuItem1,
            this.stopToolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(192, 114);
            // 
            // removeColumnsToolStripMenuItem
            // 
            this.removeColumnsToolStripMenuItem.Name = "removeColumnsToolStripMenuItem";
            this.removeColumnsToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.removeColumnsToolStripMenuItem.Text = "Remo&ve Columns";
            this.removeColumnsToolStripMenuItem.Click += new System.EventHandler(this.removeColumnsToolStripMenuItem_Click);
            // 
            // makeColumnsVisibleToolStripMenuItem
            // 
            this.makeColumnsVisibleToolStripMenuItem.Name = "makeColumnsVisibleToolStripMenuItem";
            this.makeColumnsVisibleToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.makeColumnsVisibleToolStripMenuItem.Text = "&Make Columns Visible";
            this.makeColumnsVisibleToolStripMenuItem.Click += new System.EventHandler(this.makeColumnsVisibleToolStripMenuItem_Click_1);
            // 
            // addRowToolStripMenuItem1
            // 
            this.addRowToolStripMenuItem1.Name = "addRowToolStripMenuItem1";
            this.addRowToolStripMenuItem1.Size = new System.Drawing.Size(191, 22);
            this.addRowToolStripMenuItem1.Text = "&Add Row";
            this.addRowToolStripMenuItem1.Click += new System.EventHandler(this.addRowToolStripMenuItem1_Click);
            // 
            // startToolStripMenuItem1
            // 
            this.startToolStripMenuItem1.Name = "startToolStripMenuItem1";
            this.startToolStripMenuItem1.Size = new System.Drawing.Size(191, 22);
            this.startToolStripMenuItem1.Text = "S&tart";
            this.startToolStripMenuItem1.Click += new System.EventHandler(this.startToolStripMenuItem1_Click);
            // 
            // stopToolStripMenuItem1
            // 
            this.stopToolStripMenuItem1.Name = "stopToolStripMenuItem1";
            this.stopToolStripMenuItem1.Size = new System.Drawing.Size(191, 22);
            this.stopToolStripMenuItem1.Text = "Sto&p";
            this.stopToolStripMenuItem1.Click += new System.EventHandler(this.stopToolStripMenuItem1_Click);
            // 
            // rBNoLog
            // 
            this.rBNoLog.AutoSize = true;
            this.rBNoLog.Location = new System.Drawing.Point(6, 19);
            this.rBNoLog.Name = "rBNoLog";
            this.rBNoLog.Size = new System.Drawing.Size(80, 17);
            this.rBNoLog.TabIndex = 14;
            this.rBNoLog.Text = "&No Logging";
            this.rBNoLog.UseVisualStyleBackColor = true;
            // 
            // rBMain
            // 
            this.rBMain.AutoSize = true;
            this.rBMain.Location = new System.Drawing.Point(6, 42);
            this.rBMain.Name = "rBMain";
            this.rBMain.Size = new System.Drawing.Size(97, 17);
            this.rBMain.TabIndex = 15;
            this.rBMain.Text = "&Main Functions";
            this.rBMain.UseVisualStyleBackColor = true;
            // 
            // rBAll
            // 
            this.rBAll.AutoSize = true;
            this.rBAll.Checked = true;
            this.rBAll.Location = new System.Drawing.Point(6, 66);
            this.rBAll.Name = "rBAll";
            this.rBAll.Size = new System.Drawing.Size(85, 17);
            this.rBAll.TabIndex = 16;
            this.rBAll.TabStop = true;
            this.rBAll.Text = "&All Functions";
            this.rBAll.UseVisualStyleBackColor = true;
            // 
            // groupBoxLL
            // 
            this.groupBoxLL.Controls.Add(this.rBNoLog);
            this.groupBoxLL.Controls.Add(this.rBAll);
            this.groupBoxLL.Controls.Add(this.rBMain);
            this.groupBoxLL.Controls.Add(this.btnLiveLogging);
            this.groupBoxLL.Location = new System.Drawing.Point(481, 19);
            this.groupBoxLL.Name = "groupBoxLL";
            this.groupBoxLL.Size = new System.Drawing.Size(122, 122);
            this.groupBoxLL.TabIndex = 12;
            this.groupBoxLL.TabStop = false;
            this.groupBoxLL.Text = "Logging Level";
            // 
            // btnLiveLogging
            // 
            this.btnLiveLogging.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnLiveLogging.Location = new System.Drawing.Point(4, 91);
            this.btnLiveLogging.Name = "btnLiveLogging";
            this.btnLiveLogging.Size = new System.Drawing.Size(112, 26);
            this.btnLiveLogging.TabIndex = 11;
            this.btnLiveLogging.Text = "&Open Live Logging";
            this.btnLiveLogging.UseVisualStyleBackColor = true;
            this.btnLiveLogging.Click += new System.EventHandler(this.btnLiveLogging_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.dataGridToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(609, 24);
            this.menuStrip1.TabIndex = 19;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.openToolStripMenuItem.Text = "&Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // dataGridToolStripMenuItem
            // 
            this.dataGridToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.synchronizeToolStripMenuItem,
            this.restoreToolStripMenuItem,
            this.launchToolStripMenuItem,
            this.addRowToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.addRecordToolStripMenuItem,
            this.startToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.optimizeColumnsToolStripMenuItem,
            this.makeColumnsVisibleToolStripMenuItem1,
            this.killCommanderToolStripMenuItem});
            this.dataGridToolStripMenuItem.Enabled = false;
            this.dataGridToolStripMenuItem.Name = "dataGridToolStripMenuItem";
            this.dataGridToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.dataGridToolStripMenuItem.Text = "Data &Grid";
            // 
            // synchronizeToolStripMenuItem
            // 
            this.synchronizeToolStripMenuItem.Name = "synchronizeToolStripMenuItem";
            this.synchronizeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.synchronizeToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.synchronizeToolStripMenuItem.Text = "S&ynchronize";
            this.synchronizeToolStripMenuItem.Click += new System.EventHandler(this.synchronizeToolStripMenuItem_Click);
            // 
            // restoreToolStripMenuItem
            // 
            this.restoreToolStripMenuItem.Name = "restoreToolStripMenuItem";
            this.restoreToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.restoreToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.restoreToolStripMenuItem.Text = "&Restore";
            this.restoreToolStripMenuItem.Click += new System.EventHandler(this.restoreToolStripMenuItem_Click);
            // 
            // launchToolStripMenuItem
            // 
            this.launchToolStripMenuItem.Name = "launchToolStripMenuItem";
            this.launchToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.launchToolStripMenuItem.Text = "&Launch";
            this.launchToolStripMenuItem.Click += new System.EventHandler(this.launchToolStripMenuItem_Click);
            // 
            // addRowToolStripMenuItem
            // 
            this.addRowToolStripMenuItem.Name = "addRowToolStripMenuItem";
            this.addRowToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.addRowToolStripMenuItem.Text = "&Add Row";
            this.addRowToolStripMenuItem.Click += new System.EventHandler(this.addRowToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.deleteToolStripMenuItem.Text = "&Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // addRecordToolStripMenuItem
            // 
            this.addRecordToolStripMenuItem.Name = "addRecordToolStripMenuItem";
            this.addRecordToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.addRecordToolStripMenuItem.Text = "Add Re&cord";
            this.addRecordToolStripMenuItem.Click += new System.EventHandler(this.addRecordToolStripMenuItem_Click);
            // 
            // startToolStripMenuItem
            // 
            this.startToolStripMenuItem.Name = "startToolStripMenuItem";
            this.startToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.startToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.startToolStripMenuItem.Text = "S&tart";
            this.startToolStripMenuItem.Click += new System.EventHandler(this.startToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.stopToolStripMenuItem.Text = "Sto&p";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // optimizeColumnsToolStripMenuItem
            // 
            this.optimizeColumnsToolStripMenuItem.Name = "optimizeColumnsToolStripMenuItem";
            this.optimizeColumnsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.C)));
            this.optimizeColumnsToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.optimizeColumnsToolStripMenuItem.Text = "Remo&ve Columns";
            this.optimizeColumnsToolStripMenuItem.Click += new System.EventHandler(this.optimizeColumnsToolStripMenuItem_Click);
            // 
            // makeColumnsVisibleToolStripMenuItem1
            // 
            this.makeColumnsVisibleToolStripMenuItem1.Name = "makeColumnsVisibleToolStripMenuItem1";
            this.makeColumnsVisibleToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.V)));
            this.makeColumnsVisibleToolStripMenuItem1.Size = new System.Drawing.Size(264, 22);
            this.makeColumnsVisibleToolStripMenuItem1.Text = "&Make Columns Visible";
            this.makeColumnsVisibleToolStripMenuItem1.Click += new System.EventHandler(this.makeColumnsVisibleToolStripMenuItem1_Click);
            // 
            // killCommanderToolStripMenuItem
            // 
            this.killCommanderToolStripMenuItem.Name = "killCommanderToolStripMenuItem";
            this.killCommanderToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.killCommanderToolStripMenuItem.Text = "&Kill Commander";
            this.killCommanderToolStripMenuItem.Click += new System.EventHandler(this.killCommanderToolStripMenuItem_Click);
            // 
            // listBoxTemplates
            // 
            this.listBoxTemplates.FormattingEnabled = true;
            this.listBoxTemplates.Location = new System.Drawing.Point(6, 19);
            this.listBoxTemplates.Name = "listBoxTemplates";
            this.listBoxTemplates.Size = new System.Drawing.Size(386, 121);
            this.listBoxTemplates.TabIndex = 9;
            this.listBoxTemplates.DoubleClick += new System.EventHandler(this.listBoxTemplates_DoubleClick);
            // 
            // buttonCreateFile
            // 
            this.buttonCreateFile.Enabled = false;
            this.buttonCreateFile.Location = new System.Drawing.Point(399, 74);
            this.buttonCreateFile.Name = "buttonCreateFile";
            this.buttonCreateFile.Size = new System.Drawing.Size(75, 30);
            this.buttonCreateFile.TabIndex = 10;
            this.buttonCreateFile.Text = "&Execute";
            this.buttonCreateFile.UseVisualStyleBackColor = true;
            this.buttonCreateFile.Click += new System.EventHandler(this.buttonCreateFile_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.btnOpenCommandFolder);
            this.groupBox1.Controls.Add(this.ButtonRestore);
            this.groupBox1.Controls.Add(this.listBoxTemplates);
            this.groupBox1.Controls.Add(this.buttonCreateFile);
            this.groupBox1.Controls.Add(this.groupBoxLL);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox1.Location = new System.Drawing.Point(0, 59);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(609, 160);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            // 
            // btnOpenCommandFolder
            // 
            this.btnOpenCommandFolder.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOpenCommandFolder.Enabled = false;
            this.btnOpenCommandFolder.Location = new System.Drawing.Point(399, 19);
            this.btnOpenCommandFolder.Name = "btnOpenCommandFolder";
            this.btnOpenCommandFolder.Size = new System.Drawing.Size(75, 49);
            this.btnOpenCommandFolder.TabIndex = 14;
            this.btnOpenCommandFolder.Text = "&Open Command Folder";
            this.btnOpenCommandFolder.UseVisualStyleBackColor = true;
            this.btnOpenCommandFolder.Click += new System.EventHandler(this.btnOpenCommandFolder_Click);
            // 
            // ButtonRestore
            // 
            this.ButtonRestore.Enabled = false;
            this.ButtonRestore.Location = new System.Drawing.Point(399, 110);
            this.ButtonRestore.Name = "ButtonRestore";
            this.ButtonRestore.Size = new System.Drawing.Size(75, 30);
            this.ButtonRestore.TabIndex = 13;
            this.ButtonRestore.Text = "&Restore";
            this.ButtonRestore.UseVisualStyleBackColor = true;
            this.ButtonRestore.Click += new System.EventHandler(this.ButtonRestore_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ClId,
            this.ClDescription,
            this.ClTime,
            this.ClElapsed,
            this.ClCommandLine,
            this.ClArguments,
            this.ClIP,
            this.ClPort,
            this.ClWidth,
            this.ClHeight,
            this.ClXLoc,
            this.ClYLoc,
            this.ClMoveAfter,
            this.ClDefaultSize,
            this.ClAdded});
            this.dataGridView.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 24);
            this.dataGridView.MinimumSize = new System.Drawing.Size(55, 35);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersWidth = 40;
            this.dataGridView.Size = new System.Drawing.Size(609, 35);
            this.dataGridView.TabIndex = 8;
            this.dataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView_KeyDown);
            // 
            // ClId
            // 
            this.ClId.FillWeight = 39.57287F;
            this.ClId.HeaderText = "ID";
            this.ClId.Name = "ClId";
            this.ClId.ReadOnly = true;
            // 
            // ClDescription
            // 
            this.ClDescription.FillWeight = 80F;
            this.ClDescription.HeaderText = "Description";
            this.ClDescription.Name = "ClDescription";
            // 
            // ClTime
            // 
            this.ClTime.FillWeight = 65F;
            this.ClTime.HeaderText = "Last Check";
            this.ClTime.Name = "ClTime";
            this.ClTime.ReadOnly = true;
            // 
            // ClElapsed
            // 
            this.ClElapsed.FillWeight = 75F;
            this.ClElapsed.HeaderText = "Time Elapsed";
            this.ClElapsed.Name = "ClElapsed";
            this.ClElapsed.ReadOnly = true;
            // 
            // ClCommandLine
            // 
            this.ClCommandLine.FillWeight = 261.225F;
            this.ClCommandLine.HeaderText = "Command Line";
            this.ClCommandLine.Name = "ClCommandLine";
            this.ClCommandLine.Visible = false;
            // 
            // ClArguments
            // 
            this.ClArguments.FillWeight = 264.9897F;
            this.ClArguments.HeaderText = "Arguments";
            this.ClArguments.Name = "ClArguments";
            // 
            // ClIP
            // 
            this.ClIP.FillWeight = 80F;
            this.ClIP.HeaderText = "IP Adress";
            this.ClIP.Name = "ClIP";
            // 
            // ClPort
            // 
            this.ClPort.FillWeight = 55F;
            this.ClPort.HeaderText = "Port";
            this.ClPort.Name = "ClPort";
            this.ClPort.Visible = false;
            // 
            // ClWidth
            // 
            this.ClWidth.FillWeight = 47F;
            this.ClWidth.HeaderText = "Width";
            this.ClWidth.Name = "ClWidth";
            this.ClWidth.Visible = false;
            // 
            // ClHeight
            // 
            this.ClHeight.FillWeight = 48.68079F;
            this.ClHeight.HeaderText = "Height";
            this.ClHeight.Name = "ClHeight";
            this.ClHeight.Visible = false;
            // 
            // ClXLoc
            // 
            this.ClXLoc.FillWeight = 64.20244F;
            this.ClXLoc.HeaderText = "X Location";
            this.ClXLoc.Name = "ClXLoc";
            // 
            // ClYLoc
            // 
            this.ClYLoc.FillWeight = 69.76187F;
            this.ClYLoc.HeaderText = "Y Location";
            this.ClYLoc.Name = "ClYLoc";
            // 
            // ClMoveAfter
            // 
            this.ClMoveAfter.FillWeight = 48.67614F;
            this.ClMoveAfter.HeaderText = "Move After";
            this.ClMoveAfter.Name = "ClMoveAfter";
            this.ClMoveAfter.Visible = false;
            // 
            // ClDefaultSize
            // 
            this.ClDefaultSize.FillWeight = 58.63179F;
            this.ClDefaultSize.HeaderText = "Default Size";
            this.ClDefaultSize.Name = "ClDefaultSize";
            this.ClDefaultSize.Visible = false;
            // 
            // ClAdded
            // 
            this.ClAdded.HeaderText = "Added";
            this.ClAdded.Name = "ClAdded";
            this.ClAdded.ReadOnly = true;
            this.ClAdded.Visible = false;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(61, 4);
            // 
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.Name = "contextMenuStrip3";
            this.contextMenuStrip3.Size = new System.Drawing.Size(61, 4);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(609, 219);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(625, 258);
            this.Name = "MainForm";
            this.Text = "Desktop Setup v5.02";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing_1);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView_KeyDown);
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBoxLL.ResumeLayout(false);
            this.groupBoxLL.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RadioButton rBNoLog;
        private System.Windows.Forms.RadioButton rBMain;
        private System.Windows.Forms.RadioButton rBAll;
        private System.Windows.Forms.GroupBox groupBoxLL;
        private System.Windows.Forms.Button btnLiveLogging;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataGridToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem synchronizeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem launchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addRowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addRecordToolStripMenuItem;
        private System.Windows.Forms.ListBox listBoxTemplates;
        private System.Windows.Forms.Button buttonCreateFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optimizeColumnsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem makeColumnsVisibleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addRowToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem startToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem removeColumnsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem makeColumnsVisibleToolStripMenuItem1;
        private System.Windows.Forms.Button ButtonRestore;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClElapsed;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClCommandLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClArguments;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClWidth;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClHeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClXLoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClYLoc;
        private System.Windows.Forms.DataGridViewTextBoxColumn ClMoveAfter;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ClDefaultSize;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ClAdded;
        private System.Windows.Forms.Button btnOpenCommandFolder;
        private System.Windows.Forms.ToolStripMenuItem killCommanderToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip3;
    }
}

