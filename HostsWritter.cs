using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace BF2statisticsLauncher
{
    class HostsWritter
    {
        private bool isReverted = true;
        public static string HostsFile { get; protected set; }
        public byte[] OldData;

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
                Log(e.Message.ToString());
                throw e;
            }

            // Backup our current HOSTS content for later replacement
            Backup();
        }

        ~HostsWritter()
        {
            if(!isReverted)
                Revert();
        }


        /// <summary>
        /// Adds lines to the hosts file
        /// </summary>
        /// <param name="lines">An array of [hostname, IP Address]</param>
        public void AppendLines(Dictionary<string, string> lines)
        {
            try
            {
                StreamWriter sw = File.AppendText(HostsFile);

                // Make sure we have a new line first!
                sw.WriteLine();
                foreach (KeyValuePair<String, String> line in lines)
                {
                    sw.WriteLine(String.Format("{0} {1}", line.Value, line.Key));
                }
                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                Log(e.Message.ToString());
            }
        }

        /// <summary>
        /// Grabs all the data in the hosts file for later restoration
        /// </summary>
        private void Backup()
        {
            try
            {
                OldData = Encoding.ASCII.GetBytes(File.ReadAllText(HostsFile).TrimEnd('\r', '\n'));
            }
            catch (Exception e)
            {
                Log(e.Message.ToString());
            }
        }

        /// <summary>
        /// Restores the HOSTS file original contents
        /// </summary>
        public void Revert()
        {
            try
            {
                FileStream stream = File.Open(HostsFile, FileMode.Open);
                stream.SetLength(0);
                stream.Write(OldData, 0, OldData.Length);
                stream.Flush();
                stream.Close();

                isReverted = true;
            }
            catch (Exception e)
            {
                Log(e.Message.ToString());
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
                Log(e.Message.ToString());
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
