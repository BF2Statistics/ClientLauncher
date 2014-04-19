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
using System.Xml;
using System.Security.Principal;
using BF2statisticsLauncher.Properties;

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
        private bool RedirectsEnabled = false;

        /// <summary>
        /// Array of mods found in the "bf2/mods" folder
        /// </summary>
        private string[] Mods;

        /// <summary>
        /// Our BF2 Root Path
        /// </summary>
        public static readonly string Root = Application.StartupPath;

        /// <summary>
        /// The User Config object
        /// </summary>
        public static Settings Config = Settings.Default;

        /// <summary>
        /// Background worker used for pinging the redirects, preventing the GUI from locking up.
        /// </summary>
        private static BackgroundWorker HostsWorker;

        /// <summary>
        /// Returns whether the app is running in administrator mode.
        /// </summary>
        public static bool IsAdministrator
        {
            get
            {
                WindowsPrincipal wp = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                return wp.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        public Launcher()
        {
            InitializeComponent();

            // Make sure we are in the correct directory!
            if (!File.Exists(Path.Combine(Root, "BF2.exe")))
            {
                MessageBox.Show(
                    "Program must be executed in the Battlefield 2 install directory!", 
                    "Launch Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                this.Load += new EventHandler(MyForm_CloseOnStart);
                return;
            }

            // Do hosts file check for existing redirects
            DoHOSTSCheck();

            // Default select mode items
            GetBF2ModsList();

            // Add administrator title to program title bar
            if (IsAdministrator)
                this.Text += " (Administrator)";
        }

        #region Startup Methods

        /// <summary>
        /// Checks the HOSTS file on startup, detecting existing redirects to the bf2web.gamespy
        /// or gpcm/gpsp.gamespy urls
        /// </summary>
        private void DoHOSTSCheck()
        {
            // Make sure we can read and write
            if (!HostsFile.CanRead)
            {
                UpdateStatus("- Cannot open HOSTS file for reading!");
            }
            else if (!HostsFile.CanWrite)
            {
                UpdateStatus("- Cannot open HOSTS file for writing!");
            }
            else
            {
                bool MatchFound = false;

                // Login server redirect
                if (HostsFile.HasEntry("gpcm.gamespy.com"))
                {
                    MatchFound = true;
                    GpcmCheckbox.Checked = true;
                    GpcmAddress.Text = HostsFile.Get("gpcm.gamespy.com");
                }

                // Stat server redirect
                if (HostsFile.HasEntry("bf2web.gamespy.com"))
                {
                    MatchFound = true;
                    Bf2webCheckbox.Checked = true;
                    Bf2webAddress.Text = HostsFile.Get("bf2web.gamespy.com");
                }

                // Did we find any matches?
                if (MatchFound)
                {
                    UpdateStatus("- Found old redirect data in HOSTS file.");
                    RedirectsEnabled = true;
                    LockGroups();

                    RedirectButton.Enabled = true;
                    RedirectButton.Text = "Remove HOSTS Redirect";

                    UpdateStatus("- Locking HOSTS file");
                    HostsFile.Lock();
                    UpdateStatus("- All Done!");
                }
            }
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
            XmlDocument Desc = new XmlDocument();

            // Proccess each installed mod
            int i = 0;
            foreach (string D in Mods)
            {
                // Get just the mod folder name
                string ModName = D.Remove(0, path.Length + 1);

                // Make sure we have a mod description file
                string DescFile = Path.Combine(D, "mod.desc");
                if (!File.Exists(DescFile))
                    continue;

                // Get the actual name of the mod
                try
                {
                    Desc.Load(DescFile);
                    XmlNodeList Node = Desc.GetElementsByTagName("title");
                    string Name = Node[0].InnerText.Trim();
                    if (Name == "MODDESC_BF2_TITLE")
                    {
                        ModSelectList.Items.Add(new KeyValueItem(ModName, "Battlefield 2"));
                        ModSelectList.SelectedIndex = ModSelectList.Items.Count - 1;
                        continue;
                    }
                    else if (Name == "MODDESC_XP_TITLE")
                        Name = "Battlefield 2: Special Forces";

                    ModSelectList.Items.Add(new KeyValueItem(ModName, Name));
                    i++;
                }
                catch { }
            }

            // Do we have any mods?
            if (i == 0)
            {
                MessageBox.Show(
                    "No mods detected in the bf2 mods folder. Unable to continue.", 
                    "Launch Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                this.Load += new EventHandler(MyForm_CloseOnStart);
                return;
            }

            // Make sure we have an item selected
            if (ModSelectList.SelectedIndex == -1)
                ModSelectList.SelectedIndex = 0;
        }

        #endregion

        /// <summary>
        /// This is the main HOSTS file button event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RedirectButton_Click(object sender, EventArgs e)
        {
            // Clear the output window
            LogBox.Clear();

            // Show exception message on button push if we cant read or write
            if (!HostsFile.CanRead)
            {
                string message = "Unable to READ the HOST file! Please make sure this program is being ran as an administrator, or "
                    + "modify your HOSTS file permissions, allowing this program to read/modify it.";
                MessageBox.Show(message, "Hosts file Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (!HostsFile.CanWrite)
            {
                string message = "HOSTS file is not WRITABLE! Please make sure this program is being ran as an administrator, or "
                    + "modify your HOSTS file permissions, allowing this program to read/modify it.";
                MessageBox.Show(message, "Hosts file Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


            // If we do not have a redirect in the hosts file...
            else if (!RedirectsEnabled)
            {
                // Make sure we are going to redirect something...
                if (!Bf2webCheckbox.Checked && !GpcmCheckbox.Checked)
                {
                    MessageBox.Show(
                        "Please select at least 1 redirect option",
                        "Select an Option", MessageBoxButtons.OK, MessageBoxIcon.Information
                    );
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
                            "Invalid Address", MessageBoxButtons.OK, MessageBoxIcon.Warning
                        );
                        UnlockGroups();
                        Bf2webAddress.Focus();
                        return;
                    }

                    // Convert Localhost to the Loopback Address
                    if (text.ToLower().Trim() == "localhost")
                        text = IPAddress.Loopback.ToString();

                    // Check if this is an IP address or hostname
                    IPAddress BF2Web;
                    try
                    {
                        UpdateStatus("- Resolving Hostname: " + text);
                        BF2Web = GetIpAddress(text);
                        UpdateStatus("- Found IP: " + BF2Web);
                    }
                    catch
                    {
                        MessageBox.Show(
                            "Stats server redirect address is invalid, or doesnt exist. Please enter a valid, and existing IPv4/6 or Hostname.",
                            "Invalid Address", MessageBoxButtons.OK, MessageBoxIcon.Warning
                        );

                        UpdateStatus("- Failed to Resolve Hostname!");
                        UnlockGroups();
                        return;
                    }

                    // Append line, and update status
                    HostsFile.Set("bf2web.gamespy.com", BF2Web.ToString());
                    Config.LastStatsServerAddress = Bf2webAddress.Text.Trim();
                    UpdateStatus("- Adding bf2web.gamespy.com redirect to hosts file");
                }

                // First, lets determine what the user wants to redirect
                if (GpcmCheckbox.Checked)
                {
                    // Make sure we have a valid IP address in the address box!
                    string text2 = GpcmAddress.Text.Trim();
                    if (text2.Length < 8)
                    {
                        MessageBox.Show(
                            "You must enter an IP address or Hostname in the Address box!",
                            "Invalid Address", MessageBoxButtons.OK, MessageBoxIcon.Warning
                        );
                        UnlockGroups();
                        GpcmAddress.Focus();
                        return;
                    }

                    // Convert Localhost to the Loopback Address
                    if (text2.ToLower().Trim() == "localhost")
                        text2 = IPAddress.Loopback.ToString();

                    // Make sure the IP address is valid!
                    IPAddress GpcmA;
                    try
                    {
                        UpdateStatus("- Resolving Hostname: " + text2);
                        GpcmA = GetIpAddress(text2);
                        UpdateStatus("- Found IP: " + GpcmA);
                    }
                    catch
                    {
                        MessageBox.Show(
                            "Login Server redirect address is invalid, or doesnt exist. Please enter a valid, and existing IPv4/6 or Hostname.",
                            "Invalid Address", MessageBoxButtons.OK, MessageBoxIcon.Warning
                        );

                        UpdateStatus("- Failed to Resolve Hostname!");
                        UnlockGroups();
                        return;
                    }

                    // Update status
                    UpdateStatus("- Adding gpcm.gamespy.com redirect to hosts file");
                    UpdateStatus("- Adding gpsp.gamespy.com redirect to hosts file");

                    // Append lines to hosts file
                    HostsFile.Set("gpcm.gamespy.com", GpcmA.ToString());
                    HostsFile.Set("gpsp.gamespy.com", GpcmA.ToString());
                    Config.LastLoginServerAddress = GpcmAddress.Text.Trim();
                }

                // Save last used addresses
                Config.Save();

                // Create new instance of the background worker
                HostsWorker = new BackgroundWorker();
                HostsWorker.WorkerSupportsCancellation = true;

                // Write the lines to the hosts file
                UdpateStatus("- Writting to hosts file... ", false);
                try
                {
                    // Save lines to hosts file
                    HostsFile.Save();
                    UpdateStatus("Success!");

                    // Flush DNS Cache
                    FlushDNS();

                    // Do pings, And lock hosts file. We do this in
                    // a background worker so the User can imediatly start
                    // the BF2 client while the HOSTS redirect finishes
                    HostsWorker.DoWork += new DoWorkEventHandler(RebuildDNSCache);
                    HostsWorker.RunWorkerAsync();

                    // Set form data
                    RedirectsEnabled = true;
                    RedirectButton.Text = "Remove HOSTS Redirect";
                    RedirectButton.Enabled = true;
                }
                catch
                {
                    UpdateStatus("Failed!");
                    UnlockGroups();
                    MessageBox.Show(
                        "Unable to WRITE to HOSTS file! Please make sure to replace your HOSTS file with " +
                        "the one provided in the release package, or remove your current permissions from the HOSTS file. " +
                        "It may also help to run this program as an administrator.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning
                    );
                }
            }
            else
            {
                // Lock the button
                RedirectButton.Enabled = false;

                // Create new instance of the background worker
                if (HostsWorker == null)
                {
                    HostsWorker = new BackgroundWorker();
                    HostsWorker.WorkerSupportsCancellation = true;
                }

                // Stop worker if its busy
                if (HostsWorker.IsBusy)
                {
                    LoadingForm.ShowScreen(this);
                    this.Enabled = false;
                    HostsWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(HostsWorker_RunWorkerCompleted);
                    HostsWorker.CancelAsync();
                    return;
                }

                UndoRedirects();
            }
        }

        /// <summary>
        /// Removes HOSTS file redirects.
        /// </summary>
        private void UndoRedirects()
        {
            // Tell the writter to restore the HOSTS file to its
            // original state
            UpdateStatus("- Unlocking HOSTS file");
            HostsFile.UnLock();

            // Restore the original hosts file contents
            UdpateStatus("- Restoring HOSTS file... ", false);
            try
            {
                HostsFile.Remove("bf2web.gamespy.com");
                HostsFile.Remove("gpcm.gamespy.com");
                HostsFile.Remove("gpsp.gamespy.com");
                HostsFile.Save();
                UpdateStatus("Success!");
            }
            catch
            {
                UpdateStatus("Failed!");
                MessageBox.Show(
                    "Unable to RESTORE to HOSTS file! Unfortunatly this error can only be fixed by manually removing the HOSTS file,"
                    + " and replacing it with a new one :( . If possible, you may also try changing the permissions yourself.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error
                );
            }

            // Flush the DNS!
            FlushDNS();

            // Update status
            UpdateStatus("- All Done!");

            // Reset form data
            RedirectsEnabled = false;
            RedirectButton.Text = "Begin HOSTS Redirect";
            UnlockGroups();
        }

        /// <summary>
        /// If the user cancels the redirects while the worker is currently building
        /// the DNS cache, this event will be registered. On completion, the redirects
        /// are reverted.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HostsWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Unregister
            HostsWorker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(HostsWorker_RunWorkerCompleted);
            UndoRedirects();
            LoadingForm.CloseForm();
            this.Enabled = true;
        }

        /// <summary>
        /// Method is used to unlock the input fields
        /// </summary>
        private void UnlockGroups()
        {
            RedirectButton.Enabled = true;
            GpcmGroupBox.Enabled = true;
            BF2webGroupBox.Enabled = true;
        }

        /// <summary>
        /// Method is used to lock the input fields while redirect is active
        /// </summary>
        private void LockGroups()
        {
            RedirectButton.Enabled = false;
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
                    UpdateStatus("- Resolving Hostname: " + text);
                    Addresses = Dns.GetHostAddresses(text);
                }
                catch
                {
                    UpdateStatus("- Failed to Resolve Hostname!");
                    throw new Exception("Invalid Hostname or IP Address");
                }

                if (Addresses.Length == 0)
                {
                    UpdateStatus("- Failed to Resolve Hostname!");
                    throw new Exception("Invalid Hostname or IP Address");
                }

                UpdateStatus("- Found IP: " + Addresses[0]);
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
        private void RebuildDNSCache(object sender, DoWorkEventArgs e)
        {
            // Update status window
            UdpateStatus("- Rebuilding DNS Cache... ", false);
            foreach (KeyValuePair<String, String> IP in HostsFile.GetLines())
            {
                // Cancel if we have a cancelation request
                if (HostsWorker.CancellationPending)
                {
                    UpdateStatus("Cancelled!");
                    e.Cancel = true;
                    return;
                }

                Ping p = new Ping();
                PingReply reply = p.Send(IP.Key);
            }
            UpdateStatus("Done");

            // Lock the hosts file
            UpdateStatus("- Locking HOSTS file");
            HostsFile.Lock();
            UpdateStatus("- All Done!");
        }

        /// <summary>
        /// BattleField 2 launcher button event handler. Launches the BF2.exe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LButton_Click(object sender, EventArgs e)
        {
            // Get our current mod
            string mod = ((KeyValueItem)ModSelectList.SelectedItem).Key;

            // Start new BF2 proccess
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.Arguments = String.Format(" +modPath mods/{0} {1}", mod.ToLower(), ParamBox.Text.Trim());
            Info.FileName = Path.Combine(Root, "BF2.exe");
            Process BF2 = Process.Start(Info);
        }

        /// <summary>
        /// Event fired when the Params Button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClientParamsBtn_Click(object sender, EventArgs e)
        {
            ClientParamsForm F = new ClientParamsForm(ParamBox.Text);
            if (F.ShowDialog() == DialogResult.OK)
                ParamBox.Text = ClientParamsForm.ParamString;
        }

        /// <summary>
        /// Adds a new line to the "status" window on the GUI
        /// </summary>
        /// <param name="message">The message to print</param>
        public void UpdateStatus(string message)
        {
            UdpateStatus(message, true);
        }

        /// <summary>
        /// Adds a new line to the "status" window on the GUI
        /// </summary>
        /// <param name="message">The message to print</param>
        /// <param name="newLine">Add a new line for the next message?</param>
        public void UdpateStatus(string message, bool newLine)
        {
            // Add new line
            if (newLine) message = message + Environment.NewLine;

            if (InvokeRequired)
            {
                // Invoke the logbox update
                Invoke((MethodInvoker)delegate
                {
                    LogBox.Text += message;
                    LogBox.Refresh();
                });
            }
            else
            {
                LogBox.Text += message;
                LogBox.Refresh();
            }
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
            UpdateStatus("- Flushing DNS Cache");
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

        /// <summary>
        /// Event fired when the UI is closing, Unlocks the HostsFile
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Launcher_FormClosing(object sender, FormClosingEventArgs e)
        {
            HostsFile.UnLock();
        }
    }
}
