using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace BF2redirector
{
    public partial class Form1 : Form
    {
        private bool isStarted;
        private HostsWritter Writter;

        public Form1()
        {
            this.isStarted = false;
            InitializeComponent();
            Writter = new HostsWritter(this);
        }

        private void iButton_Click(object sender, EventArgs e)
        {
            if (!isStarted)
            {
                List<string> Lines = new List<string>();

                // Clear the output window
                LogBox.Text = "";

                // Make sure we are going to redirect something...
                if (!Bf2webCheckbox.Checked && !GpcmCheckbox.Checked)
                {
                    MessageBox.Show("Please select at least 1 redirect option", "BF2 Redirector Error");
                    return;
                }

                // First, lets determine what the user wants to redirect
                if (Bf2webCheckbox.Checked)
                {
                    // Make sure we have a valid IP address in the address box!
                    string text = Bf2webAddress.Text;
                    if (text == "localhost") text = "127.0.0.1";

                    if (text.Length < 8)
                    {
                        MessageBox.Show("You must enter an IP address in the Address box!", "BF2 Redirector Error");
                        return;
                    }

                    // Make sure the IP address is valid!
                    IPAddress BF2Web;
                    bool BF2WebValid = IPAddress.TryParse(text, out BF2Web);

                    if (!BF2WebValid)
                    {
                        MessageBox.Show("Stats redirect address is invalid.", "BF2 Redirector Error");
                        return;
                    }

                    Lines.Add(String.Format("{0}      bf2web.gamespy.com", BF2Web));
                    Output("- Adding bf2web.gamespy.com redirect to hosts file");
                        
                }

                // First, lets determine what the user wants to redirect
                if (GpcmCheckbox.Checked)
                {
                    // Make sure we have a valid IP address in the address box!
                    string text2 = GpcmAddress.Text; 
                    if (text2 == "localhost") text2 = "127.0.0.1";

                    if (text2.Length < 8)
                    {
                        MessageBox.Show("You must enter an IP address in the Address box!", "BF2 Redirector Error");
                        return;
                    }

                    // Make sure the IP address is valid!
                    IPAddress GpcmA;
                    bool GpcmValid = IPAddress.TryParse(text2, out GpcmA);

                    if (!GpcmValid)
                    {
                        MessageBox.Show("Login Server redirect address is invalid.", "BF2 Redirector Error");
                        return;
                    }

                    Output("- Adding gpcm.gamespy.com redirect to hosts file");
                    Output("- Adding gpsp.gamespy.com redirect to hosts file");

                    Lines.Add(String.Format("{0}      gpcm.gamespy.com", GpcmA));
                    Lines.Add(String.Format("{0}      gpsp.gamespy.com", GpcmA));
                }

                Output("- Writting to hosts file...");

                // Write the lines to the hosts file
                bool error = false;
                try
                {
                    SetACL.UnlockHostsFile();
                    Writter.AppendLines(Lines);
                    SetACL.LockHostsFile();
                    Output("- Success!");
                }
                catch
                {
                    error = true;
                }

                if (!error)
                {
                    // Flush the DNS!
                    FlushDNS();

                    // Set form data
                    isStarted = true;
                    iButton.Text = "Stop Redirect";
                    BF2webGroupBox.Enabled = false;
                    GpcmGroupBox.Enabled = false;
                }
            }
            else
            {
                // Tell the writter to restore the HOSTS file to its
                // original state
                Output("- Reverting HOSTS file back to original state");
                SetACL.UnlockHostsFile();
                Writter.Revert();

                // Flush the DNS!
                FlushDNS();

                // Reset form data
                isStarted = false;
                iButton.Text = "Begin Redirect";
                BF2webGroupBox.Enabled = true;
                GpcmGroupBox.Enabled = true;
            }
        }

        public void Output(string message)
        {
            LogBox.Text += message + Environment.NewLine;
        }

        public void FlushDNS()
        {
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.UseShellExecute = false;
            Info.CreateNoWindow = true;
            Info.RedirectStandardOutput = true;
            Info.Arguments = "/C ipconfig /flushdns";
            Info.FileName = Path.Combine(Environment.SystemDirectory, "cmd.exe");

            Process gsProcess = Process.Start(Info);
            gsProcess.StandardOutput.ReadToEnd();
        }
    }
}
