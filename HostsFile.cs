using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Net;

namespace BF2statisticsLauncher
{
    class HostsFile
    {
        /// <summary>
        /// Direct Filepath to the hosts file
        /// </summary>
        public static readonly string FilePath = Path.Combine(Environment.SystemDirectory, "drivers", "etc", "hosts");

        /// <summary>
        /// Hosts file security object
        /// </summary>
        protected static FileSecurity Security;

        /// <summary>
        /// The fileinfo object for the HostsFile
        /// </summary>
        protected static FileInfo HostFile;

        /// <summary>
        /// The windows permission that represents everyone
        /// </summary>
        protected static SecurityIdentifier WorldSid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);

        /// <summary>
        /// Each line of the hosts file stored in a list. ALl redirects are removed
        /// from this list before being stored here.
        /// </summary>
        public static List<string> OrigContents = new List<string>();

        /// <summary>
        /// A list of "hostname" => "IPAddress" in the hosts file.
        /// </summary>
        protected static Dictionary<string, string> Entries = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Returns whether the HOSTS file can be read from
        /// </summary>
        public static readonly bool CanRead = false;

        /// <summary>
        /// Returns whether the HOSTS file can be written to
        /// </summary>
        public static readonly bool CanWrite = false;

        /// <summary>
        /// If CanRead or CanWrite are false, the exception that was thrown
        /// will be stored here
        /// </summary>
        public static Exception LastException { get; protected set; }

        /// <summary>
        /// Constructor
        /// </summary>
        static HostsFile()
        {
            try
            {
                // Get the Hosts file object
                HostFile = new FileInfo(FilePath);

                // If HOSTS file is readonly, remove that attribute!
                if (HostFile.IsReadOnly)
                {
                    try
                    {
                        HostFile.IsReadOnly = false;
                    }
                    catch (Exception e)
                    {
                        Log("HOSTS file is READONLY, Attribute cannot be removed: " + e.Message);
                        LastException = e;
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                Log("Program cannot access HOSTS file in any way: " + e.Message);
                LastException = e;
                return;
            }

            // Try to get the Access Control for the hosts file
            try
            {
                Security = HostFile.GetAccessControl();
            }
            catch (Exception e)
            {
                Log("Unable to get HOSTS file Access Control: " + e.Message);
                LastException = e;
                return;
            }

            // Make sure we can read the file amd write to it!
            try
            {
                // Unlock hosts file
                if (!UnLock())
                    return;

                // Get the hosts file contents
                using (StreamReader Rdr = new StreamReader(HostFile.OpenRead()))
                {
                    while (!Rdr.EndOfStream)
                        OrigContents.Add(Rdr.ReadLine());
                }

                CanRead = true;
            }
            catch (Exception e)
            {
                Log("Unable to READ the HOSTS file: " + e.Message);
                LastException = e;
                return;
            }

            // Check that we can write to the hosts file
            try
            {
                using (FileStream Stream = HostFile.OpenWrite())
                    CanWrite = true;
            }
            catch (Exception e)
            {
                Log("Unable to WRITE to the HOSTS file: " + e.Message);
                LastException = e;
                return;
            }

            // Parse hosts file lines
            foreach (string line in OrigContents)
            {
                // Dont add empty lines or comments
                string cLine = line.Trim();
                if (String.IsNullOrWhiteSpace(cLine) || cLine.StartsWith("#"))
                    continue;

                // Add line if we have a valid address and hostname
                Match M = Regex.Match(
                    cLine,
                    @"^([\s|\t]+)?(?<address>[a-z0-9\.:]+)[\s|\t]+(?<hostname>[a-z0-9\.\-_\s]+)$",
                    RegexOptions.IgnoreCase
                );

                // Add line
                if (M.Success)
                    Entries.Add(M.Groups["hostname"].Value.ToLower().Trim(), M.Groups["address"].Value.Trim());
            }

            // Make sure we have a localhost loopback! Save aswell, so its available for future
            if (!Entries.ContainsKey("localhost"))
            {
                OrigContents.Add("127.0.0.1\tlocalhost");
                Entries.Add("localhost", IPAddress.Loopback.ToString());
                File.WriteAllLines(FilePath, OrigContents);
            }
        }

        /// <summary>
        /// Removes the "Deny" read permissions, and adds the "Allow" read permission
        /// on the HOSTS file. If the Hosts file cannot be unlocked, the exception error
        /// will be logged in the App error log
        /// </summary>
        public static bool UnLock()
        {
            // Make sure we have a security object
            if (Security == null)
                return false;

            // Allow ReadData
            Security.RemoveAccessRule(new FileSystemAccessRule(WorldSid, FileSystemRights.ReadData, AccessControlType.Deny));
            Security.AddAccessRule(new FileSystemAccessRule(WorldSid, FileSystemRights.ReadData, AccessControlType.Allow));

            // Try and set the new access control
            try
            {
                HostFile.SetAccessControl(Security);
                return true;
            }
            catch (Exception e)
            {
                Log("Unable to REMOVE the Readonly rule on Hosts File: " + e.Message);
                LastException = e;
                return false;
            }
        }

        /// <summary>
        /// Removes the "Allow" read permissions, and adds the "Deny" read permission
        /// on the HOSTS file. If the Hosts file cannot be locked, the exception error
        /// will be logged in the App error log
        /// </summary>
        public static bool Lock()
        {
            // Make sure we have a security object
            if (Security == null)
                return false;

            // Donot allow Read for the Everyone Sid. This prevents the BF2 client from reading the hosts file
            Security.RemoveAccessRule(new FileSystemAccessRule(WorldSid, FileSystemRights.ReadData, AccessControlType.Allow));
            Security.AddAccessRule(new FileSystemAccessRule(WorldSid, FileSystemRights.ReadData, AccessControlType.Deny));

            // Try and set the new access control
            try
            {
                HostFile.SetAccessControl(Security);
                return true;
            }
            catch (Exception e)
            {
                Log("Unable to REMOVE the Readonly rule on Hosts File: " + e.Message);
                LastException = e;
                return false;
            }
        }

        /// <summary>
        /// Sets a domain name with an IP in the hosts file
        /// </summary>
        /// <param name="Domain">The domain name</param>
        /// <param name="Ip">The IP address</param>
        public static void Set(string Domain, string Ip)
        {
            // Throw exception if there was one!
            if (LastException != null) throw LastException;

            if (Entries.ContainsKey(Domain))
                Entries[Domain] = Ip;
            else
                Entries.Add(Domain, Ip);
        }

        /// <summary>
        /// Removes a domain name from the hosts file
        /// </summary>
        /// <param name="Domain">The domain name</param>
        public static void Remove(string Domain)
        {
            // Throw exception if there was one!
            if (LastException != null) throw LastException;

            if (Entries.ContainsKey(Domain))
                Entries.Remove(Domain);
        }

        /// <summary>
        /// Returns whether the hostsfile contains a domain name
        /// </summary>
        /// <param name="Domain">The domain name</param>
        /// <returns></returns>
        public static bool HasEntry(string Domain)
        {
            // Throw exception if there was one!
            if (LastException != null) throw LastException;
            return Entries.ContainsKey(Domain);
        }

        /// <summary>
        /// Returns the IP address for the provided domain name
        /// </summary>
        /// <param name="Domain">The domain name</param>
        /// <returns></returns>
        public static string Get(string Domain)
        {
            // Throw exception if there was one!
            if (LastException != null) throw LastException;
            return Entries[Domain];
        }

        /// <summary>
        /// Saves all currently set domains and IPs to the hosts file
        /// </summary>
        public static void Save()
        {
            // Throw exception if there was one!
            if (LastException != null) throw LastException;

            // Convert the dictionary of lines to a list of lines
            List<string> lns = new List<string>();
            foreach (KeyValuePair<String, String> line in Entries)
                lns.Add(String.Format("{0}\t{1}", line.Value, line.Key));

            File.WriteAllLines(FilePath, lns);
        }

        /// <summary>
        /// Returns a list of all current hosts file lines
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetLines()
        {
            // Throw exception if there was one!
            if (LastException != null) throw LastException;
            return Entries;
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
                File.Create(logFile).Close();

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
