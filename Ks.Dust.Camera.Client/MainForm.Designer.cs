namespace Ks.Dust.Camera.Client
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
            this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.operateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refershToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.mainInfoToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.treeViewPanel = new System.Windows.Forms.Panel();
            this.cameraListGroupBox = new System.Windows.Forms.GroupBox();
            this.cameraNodesTreeView = new System.Windows.Forms.TreeView();
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.operateGroupBox = new System.Windows.Forms.GroupBox();
            this.operateTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.BtnConnect = new System.Windows.Forms.Button();
            this.TxtSelectedCamera = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.operateTabControl = new System.Windows.Forms.TabControl();
            this.playbackTabPage = new System.Windows.Forms.TabPage();
            this.downloadTabPage = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.startDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.endDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.btnSearchHistory = new System.Windows.Forms.Button();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listView1 = new System.Windows.Forms.ListView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pbPreviewer = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnStartPlayback = new System.Windows.Forms.Button();
            this.btnStopPlayback = new System.Windows.Forms.Button();
            this.btnDownloadPlayback = new System.Windows.Forms.Button();
            this.mainMenuStrip.SuspendLayout();
            this.mainStatusStrip.SuspendLayout();
            this.treeViewPanel.SuspendLayout();
            this.cameraListGroupBox.SuspendLayout();
            this.mainTableLayoutPanel.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.operateGroupBox.SuspendLayout();
            this.operateTableLayoutPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.operateTabControl.SuspendLayout();
            this.playbackTabPage.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreviewer)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainMenuStrip
            // 
            this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.operateToolStripMenuItem});
            this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.mainMenuStrip.Name = "mainMenuStrip";
            this.mainMenuStrip.Size = new System.Drawing.Size(1099, 25);
            this.mainMenuStrip.TabIndex = 0;
            this.mainMenuStrip.Text = "mainMenuStrip";
            // 
            // operateToolStripMenuItem
            // 
            this.operateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refershToolStripMenuItem,
            this.setupToolStripMenuItem});
            this.operateToolStripMenuItem.Name = "operateToolStripMenuItem";
            this.operateToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.operateToolStripMenuItem.Text = "操作";
            // 
            // refershToolStripMenuItem
            // 
            this.refershToolStripMenuItem.Name = "refershToolStripMenuItem";
            this.refershToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.refershToolStripMenuItem.Text = "刷新";
            // 
            // setupToolStripMenuItem
            // 
            this.setupToolStripMenuItem.Name = "setupToolStripMenuItem";
            this.setupToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.setupToolStripMenuItem.Text = "设置";
            this.setupToolStripMenuItem.Click += new System.EventHandler(this.OpenSetup);
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mainInfoToolStripStatusLabel});
            this.mainStatusStrip.Location = new System.Drawing.Point(0, 773);
            this.mainStatusStrip.Name = "mainStatusStrip";
            this.mainStatusStrip.Size = new System.Drawing.Size(1099, 22);
            this.mainStatusStrip.TabIndex = 1;
            this.mainStatusStrip.Text = "mainStatusStrip";
            // 
            // mainInfoToolStripStatusLabel
            // 
            this.mainInfoToolStripStatusLabel.Name = "mainInfoToolStripStatusLabel";
            this.mainInfoToolStripStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // treeViewPanel
            // 
            this.treeViewPanel.Controls.Add(this.cameraListGroupBox);
            this.treeViewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewPanel.Location = new System.Drawing.Point(3, 3);
            this.treeViewPanel.Name = "treeViewPanel";
            this.treeViewPanel.Size = new System.Drawing.Size(294, 742);
            this.treeViewPanel.TabIndex = 0;
            // 
            // cameraListGroupBox
            // 
            this.cameraListGroupBox.Controls.Add(this.cameraNodesTreeView);
            this.cameraListGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraListGroupBox.Location = new System.Drawing.Point(0, 0);
            this.cameraListGroupBox.Name = "cameraListGroupBox";
            this.cameraListGroupBox.Size = new System.Drawing.Size(294, 742);
            this.cameraListGroupBox.TabIndex = 0;
            this.cameraListGroupBox.TabStop = false;
            this.cameraListGroupBox.Text = "在建工地列表";
            // 
            // cameraNodesTreeView
            // 
            this.cameraNodesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cameraNodesTreeView.Location = new System.Drawing.Point(3, 17);
            this.cameraNodesTreeView.Name = "cameraNodesTreeView";
            this.cameraNodesTreeView.Size = new System.Drawing.Size(288, 722);
            this.cameraNodesTreeView.TabIndex = 0;
            this.cameraNodesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnCameraSelected);
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.ColumnCount = 2;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.Controls.Add(this.treeViewPanel, 0, 0);
            this.mainTableLayoutPanel.Controls.Add(this.mainPanel, 1, 0);
            this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 25);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            this.mainTableLayoutPanel.RowCount = 1;
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(1099, 748);
            this.mainTableLayoutPanel.TabIndex = 3;
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.operateGroupBox);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(303, 3);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(793, 742);
            this.mainPanel.TabIndex = 2;
            // 
            // operateGroupBox
            // 
            this.operateGroupBox.Controls.Add(this.operateTableLayoutPanel);
            this.operateGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.operateGroupBox.Location = new System.Drawing.Point(0, 0);
            this.operateGroupBox.Name = "operateGroupBox";
            this.operateGroupBox.Size = new System.Drawing.Size(793, 742);
            this.operateGroupBox.TabIndex = 0;
            this.operateGroupBox.TabStop = false;
            this.operateGroupBox.Text = "视频操作";
            // 
            // operateTableLayoutPanel
            // 
            this.operateTableLayoutPanel.ColumnCount = 1;
            this.operateTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.operateTableLayoutPanel.Controls.Add(this.panel2, 0, 0);
            this.operateTableLayoutPanel.Controls.Add(this.panel1, 0, 1);
            this.operateTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.operateTableLayoutPanel.Location = new System.Drawing.Point(3, 17);
            this.operateTableLayoutPanel.Name = "operateTableLayoutPanel";
            this.operateTableLayoutPanel.RowCount = 2;
            this.operateTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.operateTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.operateTableLayoutPanel.Size = new System.Drawing.Size(787, 722);
            this.operateTableLayoutPanel.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.BtnConnect);
            this.panel2.Controls.Add(this.TxtSelectedCamera);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(781, 69);
            this.panel2.TabIndex = 0;
            // 
            // BtnConnect
            // 
            this.BtnConnect.Enabled = false;
            this.BtnConnect.Location = new System.Drawing.Point(284, 5);
            this.BtnConnect.Name = "BtnConnect";
            this.BtnConnect.Size = new System.Drawing.Size(75, 23);
            this.BtnConnect.TabIndex = 2;
            this.BtnConnect.Text = "连接摄像头";
            this.BtnConnect.UseVisualStyleBackColor = true;
            // 
            // TxtSelectedCamera
            // 
            this.TxtSelectedCamera.Location = new System.Drawing.Point(98, 6);
            this.TxtSelectedCamera.Name = "TxtSelectedCamera";
            this.TxtSelectedCamera.Size = new System.Drawing.Size(180, 21);
            this.TxtSelectedCamera.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "选中的摄像头：";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.operateTabControl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 78);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(781, 641);
            this.panel1.TabIndex = 1;
            // 
            // operateTabControl
            // 
            this.operateTabControl.Controls.Add(this.playbackTabPage);
            this.operateTabControl.Controls.Add(this.downloadTabPage);
            this.operateTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.operateTabControl.Location = new System.Drawing.Point(0, 0);
            this.operateTabControl.Name = "operateTabControl";
            this.operateTabControl.SelectedIndex = 0;
            this.operateTabControl.Size = new System.Drawing.Size(781, 641);
            this.operateTabControl.TabIndex = 0;
            // 
            // playbackTabPage
            // 
            this.playbackTabPage.Controls.Add(this.tableLayoutPanel1);
            this.playbackTabPage.Location = new System.Drawing.Point(4, 22);
            this.playbackTabPage.Name = "playbackTabPage";
            this.playbackTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.playbackTabPage.Size = new System.Drawing.Size(773, 615);
            this.playbackTabPage.TabIndex = 0;
            this.playbackTabPage.Text = "视频回放";
            this.playbackTabPage.UseVisualStyleBackColor = true;
            // 
            // downloadTabPage
            // 
            this.downloadTabPage.Location = new System.Drawing.Point(4, 22);
            this.downloadTabPage.Name = "downloadTabPage";
            this.downloadTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.downloadTabPage.Size = new System.Drawing.Size(773, 615);
            this.downloadTabPage.TabIndex = 1;
            this.downloadTabPage.Text = "已下载视频";
            this.downloadTabPage.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(767, 609);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnSearchHistory);
            this.panel3.Controls.Add(this.endDateTimePicker);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.startDateTimePicker);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(761, 29);
            this.panel3.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "视频开始时间:";
            // 
            // startDateTimePicker
            // 
            this.startDateTimePicker.CustomFormat = "yyyy-MM-dd hh:mm:ss";
            this.startDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startDateTimePicker.Location = new System.Drawing.Point(94, 4);
            this.startDateTimePicker.Name = "startDateTimePicker";
            this.startDateTimePicker.Size = new System.Drawing.Size(200, 21);
            this.startDateTimePicker.TabIndex = 1;
            // 
            // endDateTimePicker
            // 
            this.endDateTimePicker.CustomFormat = "yyyy-MM-dd hh:mm:ss";
            this.endDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endDateTimePicker.Location = new System.Drawing.Point(388, 4);
            this.endDateTimePicker.Name = "endDateTimePicker";
            this.endDateTimePicker.Size = new System.Drawing.Size(200, 21);
            this.endDateTimePicker.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(300, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "视频结束时间:";
            // 
            // btnSearchHistory
            // 
            this.btnSearchHistory.Location = new System.Drawing.Point(595, 3);
            this.btnSearchHistory.Name = "btnSearchHistory";
            this.btnSearchHistory.Size = new System.Drawing.Size(75, 23);
            this.btnSearchHistory.TabIndex = 4;
            this.btnSearchHistory.Text = "搜索视频";
            this.btnSearchHistory.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 360F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.groupBox2, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 38);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(761, 568);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(354, 562);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listView1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(348, 521);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "历史视频列表";
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(3, 17);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(342, 501);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pbPreviewer);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(363, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(395, 562);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "视频预览";
            // 
            // pbPreviewer
            // 
            this.pbPreviewer.BackColor = System.Drawing.Color.Black;
            this.pbPreviewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbPreviewer.Location = new System.Drawing.Point(3, 17);
            this.pbPreviewer.Name = "pbPreviewer";
            this.pbPreviewer.Size = new System.Drawing.Size(389, 542);
            this.pbPreviewer.TabIndex = 0;
            this.pbPreviewer.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnStartPlayback);
            this.flowLayoutPanel1.Controls.Add(this.btnStopPlayback);
            this.flowLayoutPanel1.Controls.Add(this.btnDownloadPlayback);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 530);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(348, 29);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // btnStartPlayback
            // 
            this.btnStartPlayback.Location = new System.Drawing.Point(3, 3);
            this.btnStartPlayback.Name = "btnStartPlayback";
            this.btnStartPlayback.Size = new System.Drawing.Size(75, 23);
            this.btnStartPlayback.TabIndex = 0;
            this.btnStartPlayback.Text = "开始回放";
            this.btnStartPlayback.UseVisualStyleBackColor = true;
            // 
            // btnStopPlayback
            // 
            this.btnStopPlayback.Location = new System.Drawing.Point(84, 3);
            this.btnStopPlayback.Name = "btnStopPlayback";
            this.btnStopPlayback.Size = new System.Drawing.Size(75, 23);
            this.btnStopPlayback.TabIndex = 1;
            this.btnStopPlayback.Text = "停止回放";
            this.btnStopPlayback.UseVisualStyleBackColor = true;
            // 
            // btnDownloadPlayback
            // 
            this.btnDownloadPlayback.Location = new System.Drawing.Point(165, 3);
            this.btnDownloadPlayback.Name = "btnDownloadPlayback";
            this.btnDownloadPlayback.Size = new System.Drawing.Size(75, 23);
            this.btnDownloadPlayback.TabIndex = 2;
            this.btnDownloadPlayback.Text = "下载视频";
            this.btnDownloadPlayback.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1099, 795);
            this.Controls.Add(this.mainTableLayoutPanel);
            this.Controls.Add(this.mainStatusStrip);
            this.Controls.Add(this.mainMenuStrip);
            this.MainMenuStrip = this.mainMenuStrip;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "昆山扬尘监控视频管理平台";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.OnLoad);
            this.mainMenuStrip.ResumeLayout(false);
            this.mainMenuStrip.PerformLayout();
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            this.treeViewPanel.ResumeLayout(false);
            this.cameraListGroupBox.ResumeLayout(false);
            this.mainTableLayoutPanel.ResumeLayout(false);
            this.mainPanel.ResumeLayout(false);
            this.operateGroupBox.ResumeLayout(false);
            this.operateTableLayoutPanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.operateTabControl.ResumeLayout(false);
            this.playbackTabPage.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbPreviewer)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip mainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem operateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refershToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setupToolStripMenuItem;
        private System.Windows.Forms.StatusStrip mainStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel mainInfoToolStripStatusLabel;
        private System.Windows.Forms.Panel treeViewPanel;
        private System.Windows.Forms.GroupBox cameraListGroupBox;
        private System.Windows.Forms.TreeView cameraNodesTreeView;
        private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.GroupBox operateGroupBox;
        private System.Windows.Forms.TableLayoutPanel operateTableLayoutPanel;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button BtnConnect;
        private System.Windows.Forms.TextBox TxtSelectedCamera;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl operateTabControl;
        private System.Windows.Forms.TabPage playbackTabPage;
        private System.Windows.Forms.TabPage downloadTabPage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DateTimePicker startDateTimePicker;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSearchHistory;
        private System.Windows.Forms.DateTimePicker endDateTimePicker;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.PictureBox pbPreviewer;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnStartPlayback;
        private System.Windows.Forms.Button btnStopPlayback;
        private System.Windows.Forms.Button btnDownloadPlayback;
    }
}

