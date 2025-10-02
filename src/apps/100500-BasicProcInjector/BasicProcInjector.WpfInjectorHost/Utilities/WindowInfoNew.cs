namespace BasicProcInjector.WpfInjectorHost.Utilities
{
    using BasicProcInjector.Core;
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Text.RegularExpressions;

    public class WindowInfoNew
    {
        private static readonly ConcurrentDictionary<IntPtr, WindowInfoNew> windowInfoCache = new();

        //// we have to match "HwndWrapper[{0};{1};{2}]" which is used at https://referencesource.microsoft.com/#WindowsBase/Shared/MS/Win32/HwndWrapper.cs,2a8e13c293bb3f8c
        //private static readonly Regex windowClassNameRegex = new(@"^HwndWrapper\[.*;.*;.*\]$", RegexOptions.Compiled);

        //private ProcessInfoNew? owningProcessInfo;

        //private static readonly int snoopProcessId = Process.GetCurrentProcess().Id;

        private WindowInfoNew(IntPtr hwnd)
        {
            this.HWnd = hwnd;
        }

        private WindowInfoNew(IntPtr hwnd, Process? owningProcess)
            : this(hwnd)
        {
            //if (owningProcess is not null)
            //{
            //    this.owningProcessInfo = new(owningProcess);
            //}
        }

        public static WindowInfoNew GetWindowInfo(IntPtr hwnd, Process? owningProcess = null)
        {
            if (windowInfoCache.TryGetValue(hwnd, out var windowInfo))
            {
                return windowInfo;
            }

            windowInfo = new(hwnd, owningProcess);
            while (windowInfoCache.TryAdd(hwnd, windowInfo) == false)
            {
            }

            return windowInfo;
        }

        public IntPtr HWnd { get; }
    }
}
