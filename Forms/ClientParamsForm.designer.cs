namespace BF2statisticsLauncher
{
    partial class ClientParamsForm
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
            this.CustomRes = new System.Windows.Forms.CheckBox();
            this.WindowedMode = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.WidthText = new System.Windows.Forms.TextBox();
            this.HeightText = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.JoinServer = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.AutoLogin = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.AccountPass = new System.Windows.Forms.TextBox();
            this.AccountName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.JoinServerPort = new System.Windows.Forms.NumericUpDown();
            this.JoinServerPass = new System.Windows.Forms.TextBox();
            this.JoinServerIp = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.LowPriority = new System.Windows.Forms.CheckBox();
            this.NoSound = new System.Windows.Forms.CheckBox();
            this.Restart = new System.Windows.Forms.CheckBox();
            this.PlayNow = new System.Windows.Forms.CheckBox();
            this.DisableSwiff = new System.Windows.Forms.CheckBox();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.SaveBtn = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.JoinServerPort)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // CustomRes
            // 
            this.CustomRes.AutoSize = true;
            this.CustomRes.Location = new System.Drawing.Point(46, 57);
            this.CustomRes.Name = "CustomRes";
            this.CustomRes.Size = new System.Drawing.Size(144, 17);
            this.CustomRes.TabIndex = 0;
            this.CustomRes.Text = "Force Custom Resolution";
            this.CustomRes.UseVisualStyleBackColor = true;
            this.CustomRes.CheckedChanged += new System.EventHandler(this.CustomRes_CheckedChanged);
            // 
            // WindowedMode
            // 
            this.WindowedMode.AutoSize = true;
            this.WindowedMode.Location = new System.Drawing.Point(46, 34);
            this.WindowedMode.Name = "WindowedMode";
            this.WindowedMode.Size = new System.Drawing.Size(107, 17);
            this.WindowedMode.TabIndex = 3;
            this.WindowedMode.Text = "Windowed Mode";
            this.WindowedMode.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Width:";
            // 
            // WidthText
            // 
            this.WidthText.Enabled = false;
            this.WidthText.Location = new System.Drawing.Point(57, 82);
            this.WidthText.Name = "WidthText";
            this.WidthText.Size = new System.Drawing.Size(47, 20);
            this.WidthText.TabIndex = 5;
            this.WidthText.Text = "1920";
            // 
            // HeightText
            // 
            this.HeightText.Enabled = false;
            this.HeightText.Location = new System.Drawing.Point(161, 82);
            this.HeightText.Name = "HeightText";
            this.HeightText.Size = new System.Drawing.Size(47, 20);
            this.HeightText.TabIndex = 7;
            this.HeightText.Text = "1080";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(121, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Height: ";
            // 
            // JoinServer
            // 
            this.JoinServer.AutoSize = true;
            this.JoinServer.Location = new System.Drawing.Point(12, 35);
            this.JoinServer.Name = "JoinServer";
            this.JoinServer.Size = new System.Drawing.Size(54, 13);
            this.JoinServer.TabIndex = 8;
            this.JoinServer.Text = "Server IP:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Account: ";
            // 
            // AutoLogin
            // 
            this.AutoLogin.AutoSize = true;
            this.AutoLogin.Location = new System.Drawing.Point(46, 41);
            this.AutoLogin.Name = "AutoLogin";
            this.AutoLogin.Size = new System.Drawing.Size(113, 17);
            this.AutoLogin.TabIndex = 10;
            this.AutoLogin.Text = "Enable Auto Login";
            this.AutoLogin.UseVisualStyleBackColor = true;
            this.AutoLogin.CheckedChanged += new System.EventHandler(this.AutoLogin_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.WindowedMode);
            this.groupBox1.Controls.Add(this.CustomRes);
            this.groupBox1.Controls.Add(this.WidthText);
            this.groupBox1.Controls.Add(this.HeightText);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(24, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(235, 136);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Screen Settings";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.AccountPass);
            this.groupBox2.Controls.Add(this.AccountName);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.AutoLogin);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(24, 175);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(235, 145);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Login Settings";
            // 
            // AccountPass
            // 
            this.AccountPass.Enabled = false;
            this.AccountPass.Location = new System.Drawing.Point(90, 96);
            this.AccountPass.Name = "AccountPass";
            this.AccountPass.Size = new System.Drawing.Size(118, 20);
            this.AccountPass.TabIndex = 13;
            // 
            // AccountName
            // 
            this.AccountName.Enabled = false;
            this.AccountName.Location = new System.Drawing.Point(90, 68);
            this.AccountName.Name = "AccountName";
            this.AccountName.Size = new System.Drawing.Size(118, 20);
            this.AccountName.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Password:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.JoinServerPort);
            this.groupBox3.Controls.Add(this.JoinServerPass);
            this.groupBox3.Controls.Add(this.JoinServerIp);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.JoinServer);
            this.groupBox3.Location = new System.Drawing.Point(276, 24);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(235, 136);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Auto Connect To Server";
            // 
            // JoinServerPort
            // 
            this.JoinServerPort.Location = new System.Drawing.Point(105, 59);
            this.JoinServerPort.Maximum = new decimal(new int[] {
            26555,
            0,
            0,
            0});
            this.JoinServerPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.JoinServerPort.Name = "JoinServerPort";
            this.JoinServerPort.Size = new System.Drawing.Size(63, 20);
            this.JoinServerPort.TabIndex = 15;
            this.JoinServerPort.Value = new decimal(new int[] {
            16567,
            0,
            0,
            0});
            // 
            // JoinServerPass
            // 
            this.JoinServerPass.Location = new System.Drawing.Point(105, 85);
            this.JoinServerPass.Name = "JoinServerPass";
            this.JoinServerPass.Size = new System.Drawing.Size(118, 20);
            this.JoinServerPass.TabIndex = 14;
            // 
            // JoinServerIp
            // 
            this.JoinServerIp.Location = new System.Drawing.Point(105, 32);
            this.JoinServerIp.Name = "JoinServerIp";
            this.JoinServerIp.Size = new System.Drawing.Size(118, 20);
            this.JoinServerIp.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Server Password:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Server Port:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.LowPriority);
            this.groupBox4.Controls.Add(this.NoSound);
            this.groupBox4.Controls.Add(this.Restart);
            this.groupBox4.Controls.Add(this.PlayNow);
            this.groupBox4.Controls.Add(this.DisableSwiff);
            this.groupBox4.Location = new System.Drawing.Point(276, 175);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(235, 145);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Misc";
            // 
            // LowPriority
            // 
            this.LowPriority.AutoSize = true;
            this.LowPriority.Location = new System.Drawing.Point(35, 112);
            this.LowPriority.Name = "LowPriority";
            this.LowPriority.Size = new System.Drawing.Size(117, 17);
            this.LowPriority.TabIndex = 4;
            this.LowPriority.Text = "Run as Low Priority";
            this.LowPriority.UseVisualStyleBackColor = true;
            // 
            // NoSound
            // 
            this.NoSound.AutoSize = true;
            this.NoSound.Location = new System.Drawing.Point(35, 89);
            this.NoSound.Name = "NoSound";
            this.NoSound.Size = new System.Drawing.Size(104, 17);
            this.NoSound.TabIndex = 3;
            this.NoSound.Text = "No Sound Mode";
            this.NoSound.UseVisualStyleBackColor = true;
            // 
            // Restart
            // 
            this.Restart.AutoSize = true;
            this.Restart.Location = new System.Drawing.Point(35, 43);
            this.Restart.Name = "Restart";
            this.Restart.Size = new System.Drawing.Size(60, 17);
            this.Restart.TabIndex = 2;
            this.Restart.Text = "Restart";
            this.Restart.UseVisualStyleBackColor = true;
            // 
            // PlayNow
            // 
            this.PlayNow.AutoSize = true;
            this.PlayNow.Location = new System.Drawing.Point(35, 20);
            this.PlayNow.Name = "PlayNow";
            this.PlayNow.Size = new System.Drawing.Size(96, 17);
            this.PlayNow.TabIndex = 1;
            this.PlayNow.Text = "Start Play Now";
            this.PlayNow.UseVisualStyleBackColor = true;
            // 
            // DisableSwiff
            // 
            this.DisableSwiff.AutoSize = true;
            this.DisableSwiff.Location = new System.Drawing.Point(35, 66);
            this.DisableSwiff.Name = "DisableSwiff";
            this.DisableSwiff.Size = new System.Drawing.Size(119, 17);
            this.DisableSwiff.TabIndex = 0;
            this.DisableSwiff.Text = "Disable Swiff Player";
            this.DisableSwiff.UseVisualStyleBackColor = true;
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(183, 338);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 26);
            this.CancelBtn.TabIndex = 15;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // SaveBtn
            // 
            this.SaveBtn.Location = new System.Drawing.Point(277, 338);
            this.SaveBtn.Name = "SaveBtn";
            this.SaveBtn.Size = new System.Drawing.Size(75, 26);
            this.SaveBtn.TabIndex = 16;
            this.SaveBtn.Text = "Save";
            this.SaveBtn.UseVisualStyleBackColor = true;
            this.SaveBtn.Click += new System.EventHandler(this.SaveBtn_Click);
            // 
            // ClientParamsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(534, 372);
            this.Controls.Add(this.SaveBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ClientParamsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Battlefield 2 Client Params";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.JoinServerPort)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox CustomRes;
        private System.Windows.Forms.CheckBox WindowedMode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox WidthText;
        private System.Windows.Forms.TextBox HeightText;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label JoinServer;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox AutoLogin;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.CheckBox DisableSwiff;
        private System.Windows.Forms.CheckBox PlayNow;
        private System.Windows.Forms.CheckBox Restart;
        private System.Windows.Forms.CheckBox NoSound;
        private System.Windows.Forms.CheckBox LowPriority;
        private System.Windows.Forms.TextBox AccountPass;
        private System.Windows.Forms.TextBox AccountName;
        private System.Windows.Forms.NumericUpDown JoinServerPort;
        private System.Windows.Forms.TextBox JoinServerPass;
        private System.Windows.Forms.TextBox JoinServerIp;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button SaveBtn;
    }
}