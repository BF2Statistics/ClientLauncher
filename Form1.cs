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
using System.Threading;

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
                Dictionary<string, string> Lines = new Dictionary<string, string>();
                List<string> IPs = new List<string>();

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
                    string text = Bf2webAddress.Text.Trim();
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

                    Lines.Add("BF2web.gamespy.com", BF2Web.ToString());
                    IPs.Add("BF2web.gamespy.com");
                    Output("- Adding bf2web.gamespy.com redirect to hosts file");   
                }

                // First, lets determine what the user wants to redirect
                if (GpcmCheckbox.Checked)
                {
                    // Make sure we have a valid IP address in the address box!
                    string text2 = GpcmAddress.Text.Trim(); 
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

                    Lines.Add("gpcm.gamespy.com", GpcmA.ToString());
                    Lines.Add("gpsp.gamespy.com", GpcmA.ToString());
                    IPs.Add("gpcm.gamespy.com");
                    IPs.Add("gpsp.gamespy.com");
                }

                Output("- Writting to hosts file...");

                // Write the lines to the hosts file
                bool error = false;
                try
                {
                    //SetACL.UnlockHostsFile();
                    Writter.AppendLines(Lines);
                    Output("- Success!");

                    // Flush the DNS!
                    FlushDNS();

                    // Do pings
                    DoPings(IPs);

                    // Lock the hosts file
                    Output("- Locking HOSTS file");
                    SetACL.LockHostsFile(this);
                }
                catch
                {
                    error = true;
                }

                if (!error)
                {
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
                Output("- Unlocking HOSTS file");
                SetACL.UnlockHostsFile();

                // Restore the original hosts file contents
                Output("- Restoring HOSTS file back to original state");
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
            LogBox.ScrollToCaret();
            LogBox.Refresh();
        }

        public void FlushDNS()
        {
            Output("- Flushing DNS Cache");
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.UseShellExecute = false;
            Info.CreateNoWindow = true;
            Info.RedirectStandardOutput = true;
            Info.Arguments = "/C ipconfig /flushdns";
            Info.FileName = Path.Combine(Environment.SystemDirectory, "cmd.exe");

            Process gsProcess = Process.Start(Info);
        }

        public void DoPings(List<string> IPs)
        {
            foreach (string IP in IPs)
            {
                Output("- Pinging " + IP);
                ProcessStartInfo Info = new ProcessStartInfo();
                Info.UseShellExecute = false;
                Info.CreateNoWindow = true;
                Info.RedirectStandardOutput = true;
                Info.Arguments = String.Format("/C ping {0}", IP);
                Info.FileName = Path.Combine(Environment.SystemDirectory, "cmd.exe");

                Process gsProcess = Process.Start(Info);
                Thread.Sleep(1000);
            }
        }
    }
}
