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
        /// The windows permission that represents everyone
        /// </summary>
        protected static SecurityIdentifier WorldSid;

        /// <summary>
        /// Each line of the hosts file stored in a list. ALl redirects are removed
        /// from this list before being stored here.
        /// </summary>
        public static List<string> OrigContents;

        /// <summary>
        /// A list of "hostname" => "IPAddress" in the hosts file.
        /// </summary>
        protected static Dictionary<string, string> Lines = new Dictionary<string,string>();

        /// <summary>
        /// Returns whether the HOSTS file can be read from
        /// </summary>
        public static readonly bool CanRead;

        /// <summary>
        /// Returns whether the HOSTS file can be written to
        /// </summary>
        public static readonly bool CanWrite;

        /// <summary>
        /// If CanRead or CanWrite are false, the exception that was thrown
        /// will be stored here
        /// </summary>
        public static readonly Exception Exception;

        /// <summary>
        /// Constructor
        /// </summary>
        static HostsFile()
        {
            // Get the hosts file access control
            Security = File.GetAccessControl(FilePath);

            // Get our "Everyone" user permission ID
            WorldSid = new SecurityIdentifier(WellKnownSidType.WorldSid, null);

            // Make sure we can read the file amd write to it!
            try 
            {
                UnLock(); // Unlock
                OrigContents = new List<string>(File.ReadAllLines(FilePath));
                CanRead = true;
            }
            catch (Exception e) 
            {
                CanRead = false;
                Log("Unable to READ the HOST file! " + e.Message);
                Exception = e;
                return;
            }

            // Check that we can write to the hosts file
            try
            {
                using (FileStream Stream = File.Open(FilePath, FileMode.Open, FileAccess.Write))
                {
                    if (!Stream.CanWrite)
                        throw new Exception("Hosts file cannot be written to!");
                    else
                        CanWrite = true;
                }
            }
            catch(Exception e)
            {
                CanWrite = false;
                Exception = e;
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
                    Lines.Add(M.Groups["hostname"].Value.ToLower().Trim(), M.Groups["address"].Value.Trim());
            }

            // Make sure we have a localhost loopback! Save aswell, so its available for future
            if (!Lines.ContainsKey("localhost"))
            {
                OrigContents.Add("127.0.0.1\tlocalhost");
                Lines.Add("localhost", IPAddress.Loopback.ToString());
                File.WriteAllLines(FilePath, OrigContents);
            }
        }

        /// <summary>
        /// Removes the "Deny" read permissions, and adds the "Allow" read permission
        /// on the HOSTS file
        /// </summary>
        public static void UnLock()
        {
            // Throw exception if there was one!
            if (Exception != null) throw Exception;

            // Allow ReadData
            Security.RemoveAccessRule(new FileSystemAccessRule(WorldSid, FileSystemRights.ReadData, AccessControlType.Deny));
            Security.AddAccessRule(new FileSystemAccessRule(WorldSid, FileSystemRights.ReadData, AccessControlType.Allow));
            File.SetAccessControl(FilePath, Security);
        }

        /// <summary>
        /// Removes the "Allow" read permissions, and adds the "Deny" read permission
        /// on the HOSTS file
        /// </summary>
        public static void Lock()
        {
            // Throw exception if there was one!
            if (Exception != null) throw Exception;

            Security.RemoveAccessRule(new FileSystemAccessRule(WorldSid, FileSystemRights.ReadData, AccessControlType.Allow));
            Security.AddAccessRule(new FileSystemAccessRule(WorldSid, FileSystemRights.ReadData, AccessControlType.Deny));
            File.SetAccessControl(FilePath, Security);
        }

        /// <summary>
        /// Sets a domain name with an IP in the hosts file
        /// </summary>
        /// <param name="Domain">The domain name</param>
        /// <param name="Ip">The IP address</param>
        public static void Set(string Domain, string Ip)
        {
            // Throw exception if there was one!
            if (Exception != null) throw Exception;

            if (Lines.ContainsKey(Domain))
                Lines[Domain] = Ip;
            else
                Lines.Add(Domain, Ip);
        }

        /// <summary>
        /// Removes a domain name from the hosts file
        /// </summary>
        /// <param name="Domain">The domain name</param>
        public static void Remove(string Domain)
        {
            // Throw exception if there was one!
            if (Exception != null) throw Exception;

            if (Lines.ContainsKey(Domain))
                Lines.Remove(Domain);
        }

        /// <summary>
        /// Returns whether the hostsfile contains a domain name
        /// </summary>
        /// <param name="Domain">The domain name</param>
        /// <returns></returns>
        public static bool Contains(string Domain)
        {
            // Throw exception if there was one!
            if (Exception != null) throw Exception;
            return Lines.ContainsKey(Domain);
        }

        /// <summary>
        /// Returns the IP address for the provided domain name
        /// </summary>
        /// <param name="Domain">The domain name</param>
        /// <returns></returns>
        public static string Get(string Domain)
        {
            // Throw exception if there was one!
            if (Exception != null) throw Exception;
            return Lines[Domain];
        }

        /// <summary>
        /// Saves all currently set domains and IPs to the hosts file
        /// </summary>
        public static void Save()
        {
            // Throw exception if there was one!
            if (Exception != null) throw Exception;

            // Convert the dictionary of lines to a list of lines
            List<string> lns = new List<string>();
            foreach (KeyValuePair<String, String> line in Lines)
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
            if (Exception != null) throw Exception;
            return Lines;
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
