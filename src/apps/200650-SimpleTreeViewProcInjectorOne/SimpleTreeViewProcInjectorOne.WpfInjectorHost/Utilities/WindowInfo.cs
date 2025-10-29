namespace SimpleTreeViewProcInjectorOne.WpfInjectorHost.Utilities
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;

    public class WindowInfo
    {
        private static readonly ConcurrentDictionary<IntPtr, WindowInfo> windowInfoCache = new();

        private WindowInfo(IntPtr hwnd)
        {
            this.HWnd = hwnd;
        }

        private WindowInfo(IntPtr hwnd, Process? owningProcess)
            : this(hwnd)
        {
        }

        public static WindowInfo GetWindowInfo(IntPtr hwnd, Process? owningProcess = null)
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
