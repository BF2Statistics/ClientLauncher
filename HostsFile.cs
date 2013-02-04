using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.AccessControl;

namespace BF2statisticsLauncher
{
    class HostsFile
    {
        public static readonly string FilePath = Path.Combine(Environment.SystemDirectory, "drivers", "etc", "hosts");
        protected FileSecurity Fs;
        public List<string> OrigContents;
        public Dictionary<string, string> Lines;

        public HostsFile()
        {
            // Get the hosts file access control
            Fs = File.GetAccessControl(FilePath);

            // Make sure we can read the file amd write to it!
            try {
                UnLock(); // Unlock
                OrigContents = new List<string>(File.ReadAllLines(FilePath));
            }
            catch (Exception e) {
                Log(e.Message);
                string message = "Unable to READ the HOST file!" + Environment.NewLine + Environment.NewLine
                    + "Exception Message: " + e.Message;
                throw new Exception(message);
            }

            // Check that we can write to the hosts file
            FileStream Stream;
            try
            {
                Stream = File.Open(FilePath, FileMode.Open, FileAccess.Write);
                if (!Stream.CanWrite)
                {
                    Stream.Close();
                    throw new Exception("Hosts file cannot be written to!");
                }
            }
            catch
            {
                string message =
                    "HOSTS file is not WRITABLE! Please make sure to replace your HOSTS file with " +
                    "the one provided in the release package, or remove your current permissions from the HOSTS file. " +
                    "It may also help to run this program as an administrator.";
                throw new Exception(message);
            }

            Stream.Close();

            // Backup our current HOSTS content for later replacement
            Backup();
        }

        public void UnLock()
        {
            // Allow ReadData
            Fs.RemoveAccessRule(new FileSystemAccessRule("users", FileSystemRights.ReadData, AccessControlType.Deny));
            Fs.AddAccessRule(new FileSystemAccessRule("users", FileSystemRights.ReadData, AccessControlType.Allow));
            File.SetAccessControl(FilePath, Fs);
        }

        public void Lock()
        {
            Fs.RemoveAccessRule(new FileSystemAccessRule("users", FileSystemRights.ReadData, AccessControlType.Allow));
            Fs.AddAccessRule(new FileSystemAccessRule("users", FileSystemRights.ReadData, AccessControlType.Deny));
            File.SetAccessControl(FilePath, Fs);
        }


        /// <summary>
        /// Adds lines to the hosts file
        /// </summary>
        /// <param name="lines">An array of [hostname, IP Address]</param>
        public void AppendLines(Dictionary<string, string> add)
        {
            try
            {
                // First, add the lines
                foreach (KeyValuePair<String, String> line in add)
                {
                    if (Lines.ContainsKey(line.Key))
                    {
                        Lines[line.Key] = line.Value;
                        continue;
                    }

                    Lines.Add(line.Key, line.Value);
                }

                // Convert the dictionary of lines to a list of lines
                List<string> lns = new List<string>();
                foreach (KeyValuePair<String, String> line in Lines)
                {
                    lns.Add( String.Format("{0}\t{1}", line.Value, line.Key) );
                }

                File.WriteAllLines(FilePath, lns);
            }
            catch (Exception e)
            {
                Log("Error writing to hosts file! Reason: " + e.Message);
            }
        }

        /// <summary>
        /// Grabs all the data in the hosts file for later restoration
        /// </summary>
        private void Backup()
        {
            Lines = new Dictionary<string, string>();
            foreach (string line in OrigContents)
            {
                // Dont add empty lines or comments
                string cLine = line.Trim();
                if (String.IsNullOrWhiteSpace(cLine) || cLine.StartsWith("#"))
                    continue;

                // Add line if we have a valid address and hostname
                Match M = Regex.Match(cLine,
                    @"^([\s|\t]+)?(?<address>[a-z0-9\.:]+)[\s|\t]+(?<hostname>[a-z0-9\.\-_\s]+)$", 
                    RegexOptions.IgnoreCase);
                if (M.Success)
                    Lines.Add(M.Groups["hostname"].Value.ToLower().Trim(), M.Groups["address"].Value.Trim());
            }

            // Remove old dirty redirects from the Backup
            for (int i = 0; i < OrigContents.Count; i++)
            {
                if (OrigContents[i].Contains("bf2web.gamespy.com"))
                    OrigContents.RemoveAt(i);
                else if (OrigContents[i].Contains("gpcm.gamespy.com"))
                    OrigContents.RemoveAt(i);
                else if (OrigContents[i].Contains("gpsp.gamespy.com"))
                    OrigContents.RemoveAt(i);
            }

            // Make sure we have a localhost loopback!
            if (!Lines.ContainsKey("localhost"))
                Lines.Add("localhost", System.Net.IPAddress.Loopback.ToString());
        }

        /// <summary>
        /// Restores the HOSTS file original contents, also removing the redirects
        /// </summary>
        public void Revert()
        {
            try
            {
                File.WriteAllLines(FilePath, OrigContents);
            }
            catch (Exception e)
            {
                Log("Error writing to hosts file! Reason: " + e.Message);
                throw e;
            }
        }

        /// <summary>
        /// This method is used to store a message in the console.log file
        /// </summary>
        /// <param name="message">The message to be written to the log file</param>
        public static void Log(string message)
        {
            DateTime datet = DateTime.Now;
            String logFile = Path.Combine(Launcher.Root, "Bf2StatisticsLauncher.error.log");
            if (!File.Exists(logFile))
            {
                FileStream files = File.Create(logFile);
                files.Close();
            }
            try
            {
                StreamWriter sw = File.AppendText(logFile);
                sw.WriteLine(datet.ToString("MM/dd hh:mm") + "> " + message);
                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                Log(e.Message);
            }
        }

        /// <summary>
        /// This method is used to store a message in the console.log file
        /// </summary>
        /// <param name="message">The message to be written to the log file</param>
        public static void Log(string message, params object[] items)
        {
            Log(String.Format(message, items));
        }
    }
}
