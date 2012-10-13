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
using System.Reflection;

namespace BF2statisticsLauncher
{
    public partial class Form1 : Form
    {
        private bool isStarted;
        private HostsWritter Writter;
        private string[] Mods;
        public static readonly string Root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static BackgroundWorker bWorker = new BackgroundWorker();

        public Form1()
        {
            this.isStarted = false;
            InitializeComponent();
            Writter = new HostsWritter(this);

            // Make sure we are in the correct directory!
            if (!File.Exists(Path.Combine(Root, "BF2.exe")))
            {
                MessageBox.Show("Program must be executed in the Battlefield 2 install directory!", "BF2 Statistics Lanucher Error");
                this.Load += new EventHandler(MyForm_CloseOnStart);
            }
            else
            {
                // Default select mode items
                GetBF2ModsList();
            }
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
                    MessageBox.Show("Please select at least 1 redirect option", "BF2 Statistics Launcher Error");
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
                        MessageBox.Show("You must enter an IP address in the Address box!", "BF2 Statistics Launcher Error");
                        return;
                    }

                    // Make sure the IP address is valid!
                    IPAddress BF2Web;
                    bool BF2WebValid = IPAddress.TryParse(text, out BF2Web);

                    if (!BF2WebValid)
                    {
                        MessageBox.Show("Stats redirect address is invalid.", "BF2 Statistics Launcher Error");
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
                        MessageBox.Show("You must enter an IP address in the Address box!", "BF2 Statistics Launcher Error");
                        return;
                    }

                    // Make sure the IP address is valid!
                    IPAddress GpcmA;
                    bool GpcmValid = IPAddress.TryParse(text2, out GpcmA);

                    if (!GpcmValid)
                    {
                        MessageBox.Show("Login Server redirect address is invalid.", "BF2 Statistics Launcher Error");
                        return;
                    }

                    // Lock groupboxes and button
                    BF2webGroupBox.Enabled = false;
                    GpcmGroupBox.Enabled = false;
                    iButton.Enabled = false;

                    Output("- Adding gpcm.gamespy.com redirect to hosts file");
                    Output("- Adding gpsp.gamespy.com redirect to hosts file");

                    Lines.Add("gpcm.gamespy.com", GpcmA.ToString());
                    Lines.Add("gpsp.gamespy.com", GpcmA.ToString());
                    IPs.Add("gpcm.gamespy.com");
                    IPs.Add("gpsp.gamespy.com");
                }

                // Write the lines to the hosts file
                bool error = false;
                try
                {
                    //SetACL.UnlockHostsFile();
                    Writter.AppendLines(Lines);
                    Output("- Writting to hosts file... Success!");

                    // Flush the DNS!
                    FlushDNS();

                    // Do pings, And lock hosts file. We do this in
                    // a background worker so the User can imediatly start
                    // the BF2 client with the HOSTS redirect finishes
                    bWorker.DoWork += new DoWorkEventHandler(DoPingsAndFinish);
                    bWorker.RunWorkerAsync(IPs);
                }
                catch
                {
                    Output("- Writting to hosts file... Failed!");
                    error = true;
                }

                if (!error)
                {
                    // Set form data
                    isStarted = true;
                    iButton.Text = "Remove HOSTS Redirect";
                    iButton.Enabled = true;
                }
            }
            else
            {
                // Lock the button
                iButton.Enabled = false;

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
                iButton.Text = "Begin HOSTS Redirect";
                BF2webGroupBox.Enabled = true;
                GpcmGroupBox.Enabled = true;
                iButton.Enabled = true;

                Output("- All Done!");
            }
        }

        void DoPingsAndFinish(object sender, DoWorkEventArgs e)
        {
            List<string> IPs= new List<string>((List<string>) e.Argument);

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
                gsProcess.StandardOutput.ReadToEnd();
                gsProcess.Close();
            }

            // Lock the hosts file
            Output("- Locking HOSTS file");
            SetACL.LockHostsFile(this);
            Output("- All Done!");
        }

        private void LButton_Click(object sender, EventArgs e)
        {
            StartBF2();
        }

        private void StartBF2()
        {
            // Get our current mod
            object i = ModSelectList.SelectedItem;
            string item = "";
            try
            {
                item = i.ToString();
            }
            catch
            {
                item = "bf2";
            }

            // Start new BF2 proccess
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.Arguments = String.Format(" +modPath mods/{0} {1}", item.ToLower(), ParamBox.Text.Trim());
            Info.FileName = Path.Combine(Root, "BF2.exe");
            Process BF2 = Process.Start(Info);
        }

        private void GetBF2ModsList()
        {
            int i = 0;
            string path = Path.Combine(Root, "mods");
            Mods = Directory.GetDirectories(path);
            foreach (string D in Mods)
            {
                ModSelectList.Items.Add(new Item(D.Remove(0, path.Length + 1), i));
                i++;
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
            gsProcess.StandardOutput.ReadToEnd();
            gsProcess.Close();
        }

        private void MyForm_CloseOnStart(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
