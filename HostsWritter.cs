using System;
using System.IO;
using System.Collections.Generic;

namespace BF2redirector
{
    class HostsWritter
    {
        private bool isReverted = true;
        public static string HostsFile { get; protected set; }
        private byte[] OldData;
        private Form1 instance;

        public HostsWritter(Form1 form)
        {
            this.instance = form;
            HostsFile = Path.Combine(Environment.SystemDirectory, "drivers", "etc", "HOSTS");

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

            Backup();
        }

        ~HostsWritter()
        {
            if(!isReverted)
                Revert();
        }

        public void AppendLines(List<string> lines)
        {
            try
            {
                StreamWriter sw = File.AppendText(HostsFile);
                foreach (string line in lines)
                {
                    sw.WriteLine(line);
                }
                sw.Flush();
                sw.Close();
            }
            catch (Exception e)
            {
                Log(e.Message.ToString());
            }
        }

        private void Backup()
        {
            try
            {
                OldData = File.ReadAllBytes(HostsFile);
            }
            catch (Exception e)
            {
                Log(e.Message.ToString());
            }
        }

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
            // Log the message to the output window as well
            instance.Output("- [ERROR] " + message);

            DateTime datet = DateTime.Now;
            String logFile = Path.Combine(SetACL.AssemblyPath, "error.log");
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
