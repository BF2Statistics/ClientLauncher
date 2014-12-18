namespace BF2statisticsLauncher
{
    partial class Launcher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Launcher));
            this.Bf2webCheckbox = new System.Windows.Forms.CheckBox();
            this.Bf2webAddress = new System.Windows.Forms.TextBox();
            this.BF2webGroupBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.GpcmGroupBox = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.GpcmCheckbox = new System.Windows.Forms.CheckBox();
            this.GpcmAddress = new System.Windows.Forms.TextBox();
            this.RedirectButton = new System.Windows.Forms.Button();
            this.LogBox = new System.Windows.Forms.TextBox();
            this.LogWindow = new System.Windows.Forms.GroupBox();
            this.LaunchWindow = new System.Windows.Forms.GroupBox();
            this.ClientParamsBtn = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.ParamBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ModSelectList = new System.Windows.Forms.ComboBox();
            this.LButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.BF2webGroupBox.SuspendLayout();
            this.GpcmGroupBox.SuspendLayout();
            this.LogWindow.SuspendLayout();
            this.LaunchWindow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Bf2webCheckbox
            // 
            this.Bf2webCheckbox.AutoSize = true;
            this.Bf2webCheckbox.Location = new System.Drawing.Point(10, 23);
            this.Bf2webCheckbox.Name = "Bf2webCheckbox";
            this.Bf2webCheckbox.Size = new System.Drawing.Size(127, 17);
            this.Bf2webCheckbox.TabIndex = 0;
            this.Bf2webCheckbox.Text = "Redirect Stats Server";
            this.Bf2webCheckbox.UseVisualStyleBackColor = true;
            // 
            // Bf2webAddress
            // 
            this.Bf2webAddress.Location = new System.Drawing.Point(97, 46);
            this.Bf2webAddress.MaxLength = 100;
            this.Bf2webAddress.Name = "Bf2webAddress";
            this.Bf2webAddress.Size = new System.Drawing.Size(145, 20);
            this.Bf2webAddress.TabIndex = 2;
            this.Bf2webAddress.Text = "localhost";
            // 
            // BF2webGroupBox
            // 
            this.BF2webGroupBox.Controls.Add(this.label1);
            this.BF2webGroupBox.Controls.Add(this.Bf2webCheckbox);
            this.BF2webGroupBox.Controls.Add(this.Bf2webAddress);
            this.BF2webGroupBox.Location = new System.Drawing.Point(12, 85);
            this.BF2webGroupBox.Name = "BF2webGroupBox";
            this.BF2webGroupBox.Size = new System.Drawing.Size(260, 78);
            this.BF2webGroupBox.TabIndex = 3;
            this.BF2webGroupBox.TabStop = false;
            this.BF2webGroupBox.Text = "Battlefield 2 Stats Server";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Hostname or IP: ";
            // 
            // GpcmGroupBox
            // 
            this.GpcmGroupBox.Controls.Add(this.label2);
            this.GpcmGroupBox.Controls.Add(this.GpcmCheckbox);
            this.GpcmGroupBox.Controls.Add(this.GpcmAddress);
            this.GpcmGroupBox.Location = new System.Drawing.Point(12, 176);
            this.GpcmGroupBox.Name = "GpcmGroupBox";
            this.GpcmGroupBox.Size = new System.Drawing.Size(260, 78);
            this.GpcmGroupBox.TabIndex = 4;
            this.GpcmGroupBox.TabStop = false;
            this.GpcmGroupBox.Text = "Gamespy Login Server";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Hostname or IP: ";
            // 
            // GpcmCheckbox
            // 
            this.GpcmCheckbox.AutoSize = true;
            this.GpcmCheckbox.Location = new System.Drawing.Point(10, 23);
            this.GpcmCheckbox.Name = "GpcmCheckbox";
            this.GpcmCheckbox.Size = new System.Drawing.Size(134, 17);
            this.GpcmCheckbox.TabIndex = 0;
            this.GpcmCheckbox.Text = "Redirect Login Servers";
            this.GpcmCheckbox.UseVisualStyleBackColor = true;
            // 
            // GpcmAddress
            // 
            this.GpcmAddress.Location = new System.Drawing.Point(97, 46);
            this.GpcmAddress.MaxLength = 100;
            this.GpcmAddress.Name = "GpcmAddress";
            this.GpcmAddress.Size = new System.Drawing.Size(145, 20);
            this.GpcmAddress.TabIndex = 2;
            this.GpcmAddress.Text = "localhost";
            // 
            // RedirectButton
            // 
            this.RedirectButton.Location = new System.Drawing.Point(12, 264);
            this.RedirectButton.Name = "RedirectButton";
            this.RedirectButton.Size = new System.Drawing.Size(260, 27);
            this.RedirectButton.TabIndex = 5;
            this.RedirectButton.Text = "Begin HOSTS Redirect";
            this.RedirectButton.UseVisualStyleBackColor = true;
            this.RedirectButton.Click += new System.EventHandler(this.RedirectButton_Click);
            // 
            // LogBox
            // 
            this.LogBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.LogBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LogBox.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.LogBox.Enabled = false;
            this.LogBox.Location = new System.Drawing.Point(5, 15);
            this.LogBox.Multiline = true;
            this.LogBox.Name = "LogBox";
            this.LogBox.ReadOnly = true;
            this.LogBox.Size = new System.Drawing.Size(250, 185);
            this.LogBox.TabIndex = 8;
            // 
            // LogWindow
            // 
            this.LogWindow.Controls.Add(this.LogBox);
            this.LogWindow.Location = new System.Drawing.Point(280, 85);
            this.LogWindow.Name = "LogWindow";
            this.LogWindow.Size = new System.Drawing.Size(260, 205);
            this.LogWindow.TabIndex = 9;
            this.LogWindow.TabStop = false;
            this.LogWindow.Text = "Status";
            // 
            // LaunchWindow
            // 
            this.LaunchWindow.Controls.Add(this.ClientParamsBtn);
            this.LaunchWindow.Controls.Add(this.label4);
            this.LaunchWindow.Controls.Add(this.ParamBox);
            this.LaunchWindow.Controls.Add(this.label3);
            this.LaunchWindow.Controls.Add(this.ModSelectList);
            this.LaunchWindow.Controls.Add(this.LButton);
            this.LaunchWindow.Location = new System.Drawing.Point(14, 302);
            this.LaunchWindow.Name = "LaunchWindow";
            this.LaunchWindow.Size = new System.Drawing.Size(525, 76);
            this.LaunchWindow.TabIndex = 11;
            this.LaunchWindow.TabStop = false;
            this.LaunchWindow.Text = "BF2 Launcher";
            // 
            // ClientParamsBtn
            // 
            this.ClientParamsBtn.Image = global::BF2statisticsLauncher.Properties.Resources.Settings;
            this.ClientParamsBtn.Location = new System.Drawing.Point(492, 43);
            this.ClientParamsBtn.Name = "ClientParamsBtn";
            this.ClientParamsBtn.Size = new System.Drawing.Size(24, 24);
            this.ClientParamsBtn.TabIndex = 5;
            this.ClientParamsBtn.UseVisualStyleBackColor = true;
            this.ClientParamsBtn.Click += new System.EventHandler(this.ClientParamsBtn_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(175, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Extra Parameters:";
            // 
            // ParamBox
            // 
            this.ParamBox.Location = new System.Drawing.Point(268, 46);
            this.ParamBox.Name = "ParamBox";
            this.ParamBox.Size = new System.Drawing.Size(220, 20);
            this.ParamBox.TabIndex = 3;
            this.ParamBox.Text = "+fullscreen 1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(189, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Selected Mod:";
            // 
            // ModSelectList
            // 
            this.ModSelectList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ModSelectList.FormattingEnabled = true;
            this.ModSelectList.Location = new System.Drawing.Point(268, 19);
            this.ModSelectList.Name = "ModSelectList";
            this.ModSelectList.Size = new System.Drawing.Size(220, 21);
            this.ModSelectList.TabIndex = 1;
            // 
            // LButton
            // 
            this.LButton.Location = new System.Drawing.Point(10, 19);
            this.LButton.Name = "LButton";
            this.LButton.Size = new System.Drawing.Size(157, 47);
            this.LButton.TabIndex = 0;
            this.LButton.Text = "Launch Battlefield 2";
            this.LButton.UseVisualStyleBackColor = true;
            this.LButton.Click += new System.EventHandler(this.LButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = global::BF2statisticsLauncher.Properties.Resources.BF2Stats_Logo;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(554, 75);
            this.pictureBox1.TabIndex = 12;
            this.pictureBox1.TabStop = false;
            // 
            // Launcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(554, 387);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.LaunchWindow);
            this.Controls.Add(this.LogWindow);
            this.Controls.Add(this.RedirectButton);
            this.Controls.Add(this.GpcmGroupBox);
            this.Controls.Add(this.BF2webGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Launcher";
            this.Text = "Battlefield 2 Statistics Client Launcher v1.8";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Launcher_FormClosing);
            this.BF2webGroupBox.ResumeLayout(false);
            this.BF2webGroupBox.PerformLayout();
            this.GpcmGroupBox.ResumeLayout(false);
            this.GpcmGroupBox.PerformLayout();
            this.LogWindow.ResumeLayout(false);
            this.LogWindow.PerformLayout();
            this.LaunchWindow.ResumeLayout(false);
            this.LaunchWindow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox Bf2webCheckbox;
        private System.Windows.Forms.TextBox Bf2webAddress;
        private System.Windows.Forms.GroupBox BF2webGroupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox GpcmGroupBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox GpcmCheckbox;
        private System.Windows.Forms.TextBox GpcmAddress;
        private System.Windows.Forms.Button RedirectButton;
        private System.Windows.Forms.TextBox LogBox;
        private System.Windows.Forms.GroupBox LogWindow;
        private System.Windows.Forms.GroupBox LaunchWindow;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox ParamBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ModSelectList;
        private System.Windows.Forms.Button LButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button ClientParamsBtn;
    }
}

