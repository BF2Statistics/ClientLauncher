using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BF2statisticsLauncher
{
    class HostsWritter
    {
        public static string HostsFile { get; protected set; }
        public List<string> OrigContents;
        public Dictionary<string, string> Lines;

        public HostsWritter()
        {
            HostsFile = Path.Combine(Environment.SystemDirectory, "drivers", "etc", "HOSTS");
            SetACL.UnlockHostsFile();

            // Try to open the hosts file
            try
            {
                FileStream stream = File.Open(HostsFile, FileMode.Open);
                stream.Close();
            }
            catch (IOException e)
            {
                Log(e.Message);
                throw e;
            }

            // Backup our current HOSTS content for later replacement
            Backup();
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
                    lns.Add( String.Format("{0} {1}", line.Value, line.Key) );
                }

                File.WriteAllLines(HostsFile, lns);
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
            try
            {
                OrigContents = new List<string>(File.ReadAllLines(HostsFile));
                Lines = new Dictionary<string, string>();
                foreach (string line in OrigContents)
                {
                    // Dont add empty lines
                    if (String.IsNullOrWhiteSpace(line))
                        continue;

                    // Add line if we have a valid address and hostname
                    Match M = Regex.Match(line.Trim(), @"^([\s|\t])?(?<address>[a-z0-9\.:]+)[\s|\t]+(?<hostname>[a-z0-9\.\-_\s]+)$", RegexOptions.IgnoreCase);
                    if (M.Success)
                    {
                        Lines.Add(M.Groups["hostname"].Value.ToLower().Trim(), M.Groups["address"].Value.Trim());
                    }
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
            }
            catch (Exception e)
            {
                Log("Error backing up hosts file! Reason: " + e.Message);
                throw e;
            }
        }

        /// <summary>
        /// Restores the HOSTS file original contents, also removing the redirects
        /// </summary>
        public void Revert()
        {
            try
            {
                File.WriteAllLines(HostsFile, OrigContents);
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
        public void Log(string message)
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
        public void Log(string message, params object[] items)
        {
            Log(String.Format(message, items));
        }
    }
}
