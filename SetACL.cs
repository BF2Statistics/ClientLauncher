﻿using System;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace BF2statisticsLauncher
{
    class SetACL
    {
        public static void LockHostsFile(Form1 form)
        {
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.UseShellExecute = false;
            Info.CreateNoWindow = true;
            Info.RedirectStandardOutput = true;
            Info.Arguments = String.Format("-on \"{0}\" -ot file -actn ace -ace \"n:S-1-5-32-545;p:read;s:y;m:deny\"", HostsWritter.HostsFile);
            Info.FileName = Path.Combine(Form1.Root, "SetACL.exe");

            Process gsProcess = Process.Start(Info);
            string m = gsProcess.StandardOutput.ReadToEnd();
        }

        public static void UnlockHostsFile()
        {
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.UseShellExecute = false;
            Info.CreateNoWindow = true;
            Info.RedirectStandardOutput = true;
            Info.Arguments = String.Format("-on \"{0}\" -ot file -actn clear -clr dacl", HostsWritter.HostsFile);
            Info.FileName = Path.Combine(Form1.Root, "SetACL.exe");

            Process gsProcess = Process.Start(Info);
            gsProcess.StandardOutput.ReadToEnd();
        }
    }
}
