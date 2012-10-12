using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Text;

namespace BF2redirector
{
    class SetACL
    {
        public static readonly string AssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static void LockHostsFile()
        {
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.UseShellExecute = false;
            Info.CreateNoWindow = true;
            Info.RedirectStandardOutput = true;
            Info.Arguments = String.Format("-on \"{0}\" -ot file -actn ace -ace \"n:S-1-5-32-545;p:read;s:y;m:deny\"", HostsWritter.HostsFile);
            Info.FileName = Path.Combine(AssemblyPath, "SetACL.exe");

            Process gsProcess = Process.Start(Info);
            gsProcess.StandardOutput.ReadToEnd();
        }

        public static void UnlockHostsFile()
        {
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.UseShellExecute = false;
            Info.CreateNoWindow = true;
            Info.RedirectStandardOutput = true;
            Info.Arguments = String.Format("-on \"{0}\" -ot file -actn clear -clr dacl", HostsWritter.HostsFile);
            Info.FileName = Path.Combine(AssemblyPath, "SetACL.exe");

            Process gsProcess = Process.Start(Info);
            gsProcess.StandardOutput.ReadToEnd();
        }
    }
}
