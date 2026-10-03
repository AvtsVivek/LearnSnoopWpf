namespace SimpleTreeViewProcInjectorOne.Core
{
    using System;
    using System.IO;

    public static class EnvironmentEx
    {
        public static readonly string? CurrentProcessName;
        public static readonly string? CurrentProcessPath;

        static EnvironmentEx()
        {
            CurrentProcessPath = Environment.ProcessPath;
            CurrentProcessName = OperatingSystem.IsWindows()
            ? Path.GetFileNameWithoutExtension(CurrentProcessPath) : Path.GetFileName(CurrentProcessPath);
        }
    }
}
