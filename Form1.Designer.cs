namespace BF2redirector
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Bf2webCheckbox = new System.Windows.Forms.CheckBox();
            this.Bf2webAddress = new System.Windows.Forms.TextBox();
            this.BF2webGroupBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.GpcmGroupBox = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.GpcmCheckbox = new System.Windows.Forms.CheckBox();
            this.GpcmAddress = new System.Windows.Forms.TextBox();
            this.iButton = new System.Windows.Forms.Button();
            this.LogBox = new System.Windows.Forms.TextBox();
            this.LogWindow = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.BF2webGroupBox.SuspendLayout();
            this.GpcmGroupBox.SuspendLayout();
            this.LogWindow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Bf2webCheckbox
            // 
            this.Bf2webCheckbox.AutoSize = true;
            this.Bf2webCheckbox.Location = new System.Drawing.Point(10, 19);
            this.Bf2webCheckbox.Name = "Bf2webCheckbox";
            this.Bf2webCheckbox.Size = new System.Drawing.Size(245, 17);
            this.Bf2webCheckbox.TabIndex = 0;
            this.Bf2webCheckbox.Text = "Redirect BF2web.Gamespy.com (Stats Server)";
            this.Bf2webCheckbox.UseVisualStyleBackColor = true;
            // 
            // Bf2webAddress
            // 
            this.Bf2webAddress.Location = new System.Drawing.Point(73, 42);
            this.Bf2webAddress.MaxLength = 30;
            this.Bf2webAddress.Name = "Bf2webAddress";
            this.Bf2webAddress.Size = new System.Drawing.Size(167, 20);
            this.Bf2webAddress.TabIndex = 2;
            this.Bf2webAddress.Text = "127.0.0.1";
            // 
            // BF2webGroupBox
            // 
            this.BF2webGroupBox.Controls.Add(this.label1);
            this.BF2webGroupBox.Controls.Add(this.Bf2webCheckbox);
            this.BF2webGroupBox.Controls.Add(this.Bf2webAddress);
            this.BF2webGroupBox.Location = new System.Drawing.Point(12, 120);
            this.BF2webGroupBox.Name = "BF2webGroupBox";
            this.BF2webGroupBox.Size = new System.Drawing.Size(260, 78);
            this.BF2webGroupBox.TabIndex = 3;
            this.BF2webGroupBox.TabStop = false;
            this.BF2webGroupBox.Text = "BF2 Stats Server";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "IP Address: ";
            // 
            // GpcmGroupBox
            // 
            this.GpcmGroupBox.Controls.Add(this.label2);
            this.GpcmGroupBox.Controls.Add(this.GpcmCheckbox);
            this.GpcmGroupBox.Controls.Add(this.GpcmAddress);
            this.GpcmGroupBox.Location = new System.Drawing.Point(12, 211);
            this.GpcmGroupBox.Name = "GpcmGroupBox";
            this.GpcmGroupBox.Size = new System.Drawing.Size(260, 78);
            this.GpcmGroupBox.TabIndex = 4;
            this.GpcmGroupBox.TabStop = false;
            this.GpcmGroupBox.Text = "Gamespy Login Server";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "IP Address: ";
            // 
            // GpcmCheckbox
            // 
            this.GpcmCheckbox.AutoSize = true;
            this.GpcmCheckbox.Location = new System.Drawing.Point(10, 19);
            this.GpcmCheckbox.Name = "GpcmCheckbox";
            this.GpcmCheckbox.Size = new System.Drawing.Size(134, 17);
            this.GpcmCheckbox.TabIndex = 0;
            this.GpcmCheckbox.Text = "Redirect Login Servers";
            this.GpcmCheckbox.UseVisualStyleBackColor = true;
            // 
            // GpcmAddress
            // 
            this.GpcmAddress.Location = new System.Drawing.Point(73, 42);
            this.GpcmAddress.MaxLength = 30;
            this.GpcmAddress.Name = "GpcmAddress";
            this.GpcmAddress.Size = new System.Drawing.Size(167, 20);
            this.GpcmAddress.TabIndex = 2;
            this.GpcmAddress.Text = "127.0.0.1";
            // 
            // iButton
            // 
            this.iButton.Location = new System.Drawing.Point(12, 299);
            this.iButton.Name = "iButton";
            this.iButton.Size = new System.Drawing.Size(260, 27);
            this.iButton.TabIndex = 5;
            this.iButton.Text = "Begin Redirect";
            this.iButton.UseVisualStyleBackColor = true;
            this.iButton.Click += new System.EventHandler(this.iButton_Click);
            // 
            // LogBox
            // 
            this.LogBox.BackColor = System.Drawing.SystemColors.Control;
            this.LogBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.LogBox.Cursor = System.Windows.Forms.Cursors.Arrow;
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
            this.LogWindow.Location = new System.Drawing.Point(280, 120);
            this.LogWindow.Name = "LogWindow";
            this.LogWindow.Size = new System.Drawing.Size(260, 205);
            this.LogWindow.TabIndex = 9;
            this.LogWindow.TabStop = false;
            this.LogWindow.Text = "Output";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::BF2redirector.Properties.Resources.BF2Redirect;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(555, 110);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(554, 337);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.LogWindow);
            this.Controls.Add(this.iButton);
            this.Controls.Add(this.GpcmGroupBox);
            this.Controls.Add(this.BF2webGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "BF2 Redirector";
            this.BF2webGroupBox.ResumeLayout(false);
            this.BF2webGroupBox.PerformLayout();
            this.GpcmGroupBox.ResumeLayout(false);
            this.GpcmGroupBox.PerformLayout();
            this.LogWindow.ResumeLayout(false);
            this.LogWindow.PerformLayout();
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
        private System.Windows.Forms.Button iButton;
        private System.Windows.Forms.TextBox LogBox;
        private System.Windows.Forms.GroupBox LogWindow;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

