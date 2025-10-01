namespace Snoop.WpfClient.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class EnvironmentExNew
    {
        public static readonly string? CurrentProcessName;
        public static readonly string? CurrentProcessPath;

        static EnvironmentExNew()
        {
#if NET
        CurrentProcessPath = Environment.ProcessPath;
        CurrentProcessName = OperatingSystem.IsWindows()
            ? Path.GetFileNameWithoutExtension(CurrentProcessPath) : Path.GetFileName(CurrentProcessPath);
#else
            using var currentProcess = Process.GetCurrentProcess();
            CurrentProcessName = currentProcess.ProcessName;
            CurrentProcessPath = GetProcessPath(currentProcess);
#endif
        }

#if !NET
        private static string? GetProcessPath(Process process)
        {
            try
            {
                return process.MainModule?.FileName;
            }
            catch (Exception e)
            {
                LogHelperNew.WriteError(e);
            }

            return string.Empty;
        }
#endif
    }
}
