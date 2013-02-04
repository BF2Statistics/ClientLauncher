using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.IO;
using System.Reflection;

namespace BF2statisticsLauncher
{
    /// <summary>
    /// Main application
    /// </summary>
    public partial class Launcher : Form
    {
        /// <summary>
        /// Is the hosts redirect active?
        /// </summary>
        private bool isStarted = false;

        /// <summary>
        /// The HOSTS file object
        /// </summary>
        private HostsFile HostFile;

        /// <summary>
        /// Array of mods found in the "bf2/mods" folder
        /// </summary>
        private string[] Mods;

        /// <summary>
        /// Our BF2 Root Path
        /// </summary>
        public static readonly string Root = Application.StartupPath;

        /// <summary>
        /// Background worker used for pinging the redirects, preventing the GUI from locking up.
        /// </summary>
        private static BackgroundWorker bWorker;

        public Launcher()
        {
            InitializeComponent();
            bool error = false;

            // Make sure we are in the correct directory!
            if (!File.Exists(Path.Combine(Root, "BF2.exe")))
            {
                error = true;
                MessageBox.Show("Program must be executed in the Battlefield 2 install directory!", "BF2 Statistics Launcher Error");
                this.Load += new EventHandler(MyForm_CloseOnStart);
            }

            // Try to access the hosts file
            try {
                HostFile = new HostsFile();
            }
            catch(Exception e)
            {
                error = true;
                MessageBox.Show(e.Message, "BF2 Statistics Launcher Error");
                this.Load += new EventHandler(MyForm_CloseOnStart);
            }

            // Only do hosts check and such if we have no errors
            if (!error)
            {
                // Do hosts file check for existing redirects
                DoHOSTSCheck();

                // Default select mode items
                GetBF2ModsList();
            }
        }

        /// <summary>
        /// Checks the HOSTS file on startup, detecting existing redirects to the bf2web.gamespy
        /// or gpcm/gpsp.gamespy urls
        /// </summary>
        private void DoHOSTSCheck()
        {
            bool MatchFound = false;

            if (HostFile.Lines.ContainsKey("gpcm.gamespy.com"))
            {
                MatchFound = true;
                Bf2webCheckbox.Checked = true;
                Bf2webAddress.Text = HostFile.Lines["gpcm.gamespy.com"];
                BF2webGroupBox.Enabled = false;
            }

            if (HostFile.Lines.ContainsKey("bf2web.gamespy.com"))
            {
                MatchFound = true;
                Bf2webCheckbox.Checked = true;
                Bf2webAddress.Text = HostFile.Lines["bf2web.gamespy.com"];
                BF2webGroupBox.Enabled = false;
            }

            // Did we find any matches?
            if (MatchFound)
            {
                UdpateStatus("- Found old redirect data in HOSTS file.");
                isStarted = true;
                iButton.Text = "Remove HOSTS Redirect";

                UdpateStatus("- Locking HOSTS file");
                HostFile.Lock();
                UdpateStatus("- All Done!");
            }
        }

        /// <summary>
        /// This is the main HOSTS file button event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void iButton_Click(object sender, EventArgs e)
        {
            // Clear the output window
            LogBox.Clear();

            // If we do not have a redirect in the hosts file...
            if (!isStarted)
            {
                // Lines to add to the HOSTS file. [hostname, ipAddress]
                Dictionary<string, string> Lines = new Dictionary<string, string>();

                // Make sure we are going to redirect something...
                if (!Bf2webCheckbox.Checked && !GpcmCheckbox.Checked)
                {
                    MessageBox.Show("Please select at least 1 redirect option", "BF2 Statistics Launcher Error");
                    return;
                }

                // Lock button and groupboxes
                LockGroups();

                // First, lets determine what the user wants to redirect
                if (Bf2webCheckbox.Checked)
                {
                    // Make sure we have a valid IP address in the address box!
                    string text = Bf2webAddress.Text.Trim();
                    if (text.Length < 8)
                    {
                        MessageBox.Show(
                            "You must enter an IP address or Hostname in the Address box!", 
                            "BF2 Statistics Launcher Error"
                        );
                        UnlockGroups();
                        Bf2webAddress.Focus();
                        return;
                    }

                    // Convert Localhost to the Loopback Address
                    if (text.ToLower() == "localhost") 
                        text = IPAddress.Loopback.ToString();

                    // Check if this is an IP address or hostname
                    IPAddress BF2Web;
                    try
                    {
                        BF2Web = GetIpAddress(text);
                    }
                    catch
                    {
                        MessageBox.Show(
                            "Stats server redirect address is invalid, or doesnt exist. Please enter a valid, and existing IPv4/6 or Hostname.", 
                            "BF2 Statistics Launcher Error"
                        );

                        UnlockGroups();
                        return;
                    }

                    Lines.Add("bf2web.gamespy.com", BF2Web.ToString());
                    UdpateStatus("- Adding bf2web.gamespy.com redirect to hosts file");   
                }

                // First, lets determine what the user wants to redirect
                if (GpcmCheckbox.Checked)
                {
                    // Make sure we have a valid IP address in the address box!
                    string text2 = GpcmAddress.Text.Trim(); 
                    if (text2.Length < 8)
                    {
                        MessageBox.Show("You must enter an IP address or Hostname in the Address box!", "BF2 Statistics Launcher Error");
                        UnlockGroups();
                        GpcmAddress.Focus();
                        return;
                    }

                    // Convert Localhost to the Loopback Address
                    if (text2.ToLower() == "localhost") 
                        text2 = IPAddress.Loopback.ToString();

                    // Make sure the IP address is valid!
                    IPAddress GpcmA;
                    try
                    {
                        GpcmA = GetIpAddress(text2);
                    }
                    catch
                    {
                        MessageBox.Show(
                            "Login Server redirect address is invalid, or doesnt exist. Please enter a valid, and existing IPv4/6 or Hostname.",
                            "BF2 Statistics Launcher Error"
                        );
                        UnlockGroups();
                        return;
                    }

                    UdpateStatus("- Adding gpcm.gamespy.com redirect to hosts file");
                    UdpateStatus("- Adding gpsp.gamespy.com redirect to hosts file");

                    Lines.Add("gpcm.gamespy.com", GpcmA.ToString());
                    Lines.Add("gpsp.gamespy.com", GpcmA.ToString());
                }

                // Create new instance of the background worker
                bWorker = new BackgroundWorker();

                // Write the lines to the hosts file
                UpdateStatus("- Writting to hosts file... ", false);
                bool error = false;
                try
                {
                    // Add lines to the hosts file
                    HostFile.AppendLines(Lines);
                    UdpateStatus("Success!");

                    // Flush the DNS!
                    FlushDNS();

                    // Do pings, And lock hosts file. We do this in
                    // a background worker so the User can imediatly start
                    // the BF2 client with the HOSTS redirect finishes
                    bWorker.DoWork += new DoWorkEventHandler(RebuildDNSCache);
                    bWorker.RunWorkerAsync();
                }
                catch
                {
                    UdpateStatus("Failed!");
                    error = true;
                }

                if (!error)
                {
                    // Set form data
                    isStarted = true;
                    iButton.Text = "Remove HOSTS Redirect";
                    iButton.Enabled = true;
                }
                else
                {
                    UnlockGroups();
                    MessageBox.Show(
                        "Unable to WRITE to HOSTS file! Please make sure to replace your HOSTS file with " +
                        "the one provided in the release package, or remove your current permissions from the HOSTS file. " +
                        "It may also help to run this program as an administrator.",
                        "BF2 Statistics Launcher Error"
                    );
                }
            }
            else
            {
                // Lock the button
                iButton.Enabled = false;

                // Tell the writter to restore the HOSTS file to its
                // original state
                UdpateStatus("- Unlocking HOSTS file");
                HostFile.UnLock();

                // Restore the original hosts file contents
                UpdateStatus("- Restoring HOSTS file... ", false);
                try
                {
                    HostFile.Revert();
                    UdpateStatus("Success!");
                }
                catch
                {
                    UdpateStatus("Failed!");
                    MessageBox.Show(
                        "Unable to RESTORE to HOSTS file! Unfortunatly this error can only be fixed by manually removing the HOSTS file,"
                        + " and replacing it with a new one :( . If possible, you may also try changing the permissions yourself.",
                        "BF2 Statistics Launcher Error"
                    );
                }

                // Remove lines
                if (Bf2webCheckbox.Checked)
                    HostFile.Lines.Remove("bf2web.gamespy.com");
                if (GpcmCheckbox.Checked)
                {
                    HostFile.Lines.Remove("gpcm.gamespy.com");
                    HostFile.Lines.Remove("gpsp.gamespy.com");
                }

                // Flush the DNS!
                FlushDNS();

                // Reset form data
                isStarted = false;
                iButton.Text = "Begin HOSTS Redirect";
                UnlockGroups();

                UdpateStatus("- All Done!");
            }
        }

        /// <summary>
        /// Method is used to unlock the input fields
        /// </summary>
        private void UnlockGroups()
        {
            iButton.Enabled = true;
            GpcmGroupBox.Enabled = true;
            BF2webGroupBox.Enabled = true;
        }

        /// <summary>
        /// Method is used to lock the input fields while redirect is active
        /// </summary>
        private void LockGroups()
        {
            iButton.Enabled = false;
            GpcmGroupBox.Enabled = false;
            BF2webGroupBox.Enabled = false;
        }

        /// <summary>
        /// Takes a domain name, or IP address, and returns the Correct IP address.
        /// If multiple IP addresses are found, the first one is returned
        /// </summary>
        /// <param name="text">Domain name or IP Address</param>
        /// <returns></returns>
        private IPAddress GetIpAddress(string text)
        {
            // Make sure the IP address is valid!
            IPAddress Address;
            bool isValid = IPAddress.TryParse(text, out Address);

            if (!isValid)
            {
                // Try to get dns value
                IPAddress[] Addresses;
                try
                {
                    UdpateStatus("- Resolving Hostname: " + text);
                    Addresses = Dns.GetHostAddresses(text);
                }
                catch
                {
                    UdpateStatus("- Failed to Resolve Hostname!");
                    throw new Exception("Invalid Hostname or IP Address");
                }

                if (Addresses.Length == 0)
                {
                    UdpateStatus("- Failed to Resolve Hostname!");
                    throw new Exception("Invalid Hostname or IP Address");
                }

                UdpateStatus("- Found IP: " + Addresses[0]);
                return Addresses[0];
            }

            return Address;
        }

        /// <summary>
        /// Preforms the pings required to fill the dns cache, and locks the HOSTS file.
        /// The reason we ping, is because once the HOSTS file is locked, any request
        /// made to a url (when the DNS cache is empty), will skip the hosts file, because 
        /// it cant be read. If we ping first, then the DNS cache fills up with the IP 
        /// addresses in the hosts file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RebuildDNSCache(object sender, DoWorkEventArgs e)
        {
            UpdateStatus("- Rebuilding DNS Cache... ", false);
            foreach (KeyValuePair<String, String> IP in HostFile.Lines)
            {
                Ping p = new Ping();
                PingReply reply = p.Send(IP.Key);
            }
            UdpateStatus("Done");

            // Lock the hosts file
            UdpateStatus("- Locking HOSTS file");
            HostFile.Lock();
            UdpateStatus("- All Done!");
        }

        /// <summary>
        /// BattleField 2 launcher button event handler. Launches the BF2.exe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LButton_Click(object sender, EventArgs e)
        {
            // Get our current mod
            string mod = ModSelectList.SelectedItem.ToString();

            // Start new BF2 proccess
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.Arguments = String.Format(" +modPath mods/{0} {1}", mod.ToLower(), ParamBox.Text.Trim());
            Info.FileName = Path.Combine(Root, "BF2.exe");
            Process BF2 = Process.Start(Info);
        }

        /// <summary>
        /// Fetches the list of installed BF2 mods.
        /// </summary>
        private void GetBF2ModsList()
        {
            // Get our list of mods
            string path = Path.Combine(Root, "mods");
            int pathLength = path.Length;
            Mods = Directory.GetDirectories(path);

            // Add each mod to the select list
            int i = 0;
            foreach (string D in Mods)
            {
                // Add the mod to the list of items
                string mod = D.Remove(0, pathLength + 1);
                ModSelectList.Items.Add(new Item(mod, i));

                // Set the selected index if the mod is bf2
                if (mod == "bf2")
                    ModSelectList.SelectedIndex = i;
                i++;
            }

            // Do we have any mods?
            if (i == 0)
            {
                MessageBox.Show("No mods detected in the bf2 mods folder. Unable to continue.", "BF2 Statistics Launcher Error");
                this.Load += new EventHandler(MyForm_CloseOnStart);
                return;
            }

            // Make sure we have an item selected
            if (ModSelectList.SelectedIndex == -1)
                ModSelectList.SelectedIndex = 0;
        }

        /// <summary>
        /// Adds a new line to the "status" window on the GUI
        /// </summary>
        /// <param name="message">The message to print</param>
        public void UdpateStatus(string message)
        {
            UpdateStatus(message, true);
        }

        /// <summary>
        /// Adds a new line to the "status" window on the GUI
        /// </summary>
        /// <param name="message">The message to print</param>
        /// <param name="newLine">Add a new line for the next message?</param>
        public void UpdateStatus(string message, bool newLine)
        {
            if (newLine)
                message = message + Environment.NewLine;
            LogBox.Text += message;
            LogBox.Refresh();
        }

        /// <summary>
        /// For external use... Displays a message box with the provided message
        /// </summary>
        /// <param name="message">The message to dispay to the client</param>
        public static void Show(string message) 
        {
            MessageBox.Show(message, "BF2 Statistics Launcher Error");
        }

        /// <summary>
        /// Flushes the Windows DNS cache
        /// </summary>
        public void FlushDNS()
        {
            UdpateStatus("- Flushing DNS Cache");
            DnsFlushResolverCache();
        }

        [DllImport("dnsapi.dll", EntryPoint = "DnsFlushResolverCache")]
        private static extern UInt32 DnsFlushResolverCache();

        /// <summary>
        /// Closes the GUI on startup error
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyForm_CloseOnStart(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
