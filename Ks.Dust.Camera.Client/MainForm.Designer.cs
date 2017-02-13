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
            this.operateGroupBox = new System.Windows.Forms.GroupBox();
            this.operateTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtSelectedCamera = new System.Windows.Forms.TextBox();
            this.BtnConnect = new System.Windows.Forms.Button();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.operateTabControl = new System.Windows.Forms.TabControl();
            this.playbackTabPage = new System.Windows.Forms.TabPage();
            this.downloadTabPage = new System.Windows.Forms.TabPage();
            this.mainMenuStrip.SuspendLayout();
            this.mainStatusStrip.SuspendLayout();
            this.treeViewPanel.SuspendLayout();
            this.cameraListGroupBox.SuspendLayout();
            this.mainTableLayoutPanel.SuspendLayout();
            this.operateGroupBox.SuspendLayout();
            this.operateTableLayoutPanel.SuspendLayout();
            this.panel2.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.operateTabControl.SuspendLayout();
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "选中的摄像头：";
            // 
            // TxtSelectedCamera
            // 
            this.TxtSelectedCamera.Location = new System.Drawing.Point(98, 6);
            this.TxtSelectedCamera.Name = "TxtSelectedCamera";
            this.TxtSelectedCamera.Size = new System.Drawing.Size(180, 21);
            this.TxtSelectedCamera.TabIndex = 1;
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
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.operateGroupBox);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(303, 3);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(793, 742);
            this.mainPanel.TabIndex = 2;
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
            this.operateGroupBox.ResumeLayout(false);
            this.operateTableLayoutPanel.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.operateTabControl.ResumeLayout(false);
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
    }
}

