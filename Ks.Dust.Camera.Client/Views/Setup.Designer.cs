namespace Ks.Dust.Camera.Client.Views
{
    partial class Setup
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
            this.setupTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.setupButtonflowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.setupParamsPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.txtServerAddress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtServerPort = new System.Windows.Forms.TextBox();
            this.txtIpServerPort = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtIpServerAddress = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.setupTableLayout.SuspendLayout();
            this.setupButtonflowLayoutPanel.SuspendLayout();
            this.setupParamsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // setupTableLayout
            // 
            this.setupTableLayout.ColumnCount = 1;
            this.setupTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.setupTableLayout.Controls.Add(this.setupButtonflowLayoutPanel, 0, 1);
            this.setupTableLayout.Controls.Add(this.setupParamsPanel, 0, 0);
            this.setupTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.setupTableLayout.Location = new System.Drawing.Point(0, 0);
            this.setupTableLayout.Name = "setupTableLayout";
            this.setupTableLayout.RowCount = 2;
            this.setupTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.setupTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.setupTableLayout.Size = new System.Drawing.Size(484, 261);
            this.setupTableLayout.TabIndex = 0;
            // 
            // setupButtonflowLayoutPanel
            // 
            this.setupButtonflowLayoutPanel.Controls.Add(this.btnCancel);
            this.setupButtonflowLayoutPanel.Controls.Add(this.btnApply);
            this.setupButtonflowLayoutPanel.Controls.Add(this.btnConfirm);
            this.setupButtonflowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.setupButtonflowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.setupButtonflowLayoutPanel.Location = new System.Drawing.Point(3, 229);
            this.setupButtonflowLayoutPanel.Name = "setupButtonflowLayoutPanel";
            this.setupButtonflowLayoutPanel.Size = new System.Drawing.Size(478, 29);
            this.setupButtonflowLayoutPanel.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(400, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.Close);
            // 
            // btnApply
            // 
            this.btnApply.Enabled = false;
            this.btnApply.Location = new System.Drawing.Point(319, 3);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "应用";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.Apply);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(238, 3);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnConfirm.TabIndex = 2;
            this.btnConfirm.Text = "确认";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.Confirm);
            // 
            // setupParamsPanel
            // 
            this.setupParamsPanel.Controls.Add(this.txtIpServerPort);
            this.setupParamsPanel.Controls.Add(this.label3);
            this.setupParamsPanel.Controls.Add(this.txtIpServerAddress);
            this.setupParamsPanel.Controls.Add(this.label4);
            this.setupParamsPanel.Controls.Add(this.txtServerPort);
            this.setupParamsPanel.Controls.Add(this.label2);
            this.setupParamsPanel.Controls.Add(this.txtServerAddress);
            this.setupParamsPanel.Controls.Add(this.label1);
            this.setupParamsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.setupParamsPanel.Location = new System.Drawing.Point(3, 3);
            this.setupParamsPanel.Name = "setupParamsPanel";
            this.setupParamsPanel.Size = new System.Drawing.Size(478, 220);
            this.setupParamsPanel.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "更新服务器地址：";
            // 
            // txtServerAddress
            // 
            this.txtServerAddress.Location = new System.Drawing.Point(139, 4);
            this.txtServerAddress.Name = "txtServerAddress";
            this.txtServerAddress.Size = new System.Drawing.Size(200, 21);
            this.txtServerAddress.TabIndex = 1;
            this.txtServerAddress.Tag = "ServerAddress";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(350, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "端口号：";
            // 
            // txtServerPort
            // 
            this.txtServerPort.Location = new System.Drawing.Point(409, 4);
            this.txtServerPort.Name = "txtServerPort";
            this.txtServerPort.Size = new System.Drawing.Size(60, 21);
            this.txtServerPort.TabIndex = 3;
            this.txtServerPort.Tag = "ServerPort";
            // 
            // txtIpServerPort
            // 
            this.txtIpServerPort.Location = new System.Drawing.Point(409, 34);
            this.txtIpServerPort.Name = "txtIpServerPort";
            this.txtIpServerPort.Size = new System.Drawing.Size(60, 21);
            this.txtIpServerPort.TabIndex = 7;
            this.txtIpServerPort.Tag = "IpServerPort";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(350, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "端口号：";
            // 
            // txtIpServerAddress
            // 
            this.txtIpServerAddress.Location = new System.Drawing.Point(139, 34);
            this.txtIpServerAddress.Name = "txtIpServerAddress";
            this.txtIpServerAddress.Size = new System.Drawing.Size(200, 21);
            this.txtIpServerAddress.TabIndex = 5;
            this.txtIpServerAddress.Tag = "IpServerAddress";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "DDNS服务器地址：";
            // 
            // Setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 261);
            this.Controls.Add(this.setupTableLayout);
            this.MaximumSize = new System.Drawing.Size(500, 300);
            this.MinimumSize = new System.Drawing.Size(500, 300);
            this.Name = "Setup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "设置";
            this.Load += new System.EventHandler(this.OnLoaded);
            this.setupTableLayout.ResumeLayout(false);
            this.setupButtonflowLayoutPanel.ResumeLayout(false);
            this.setupParamsPanel.ResumeLayout(false);
            this.setupParamsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel setupTableLayout;
        private System.Windows.Forms.FlowLayoutPanel setupButtonflowLayoutPanel;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Panel setupParamsPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtIpServerPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtIpServerAddress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtServerPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtServerAddress;
    }
}