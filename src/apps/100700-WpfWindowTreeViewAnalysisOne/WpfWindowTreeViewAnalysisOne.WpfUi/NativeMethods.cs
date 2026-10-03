using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfWindowTreeViewAnalysisOne.WpfUi
{
    public static class NativeMethods
    {
        private delegate bool EnumWindowsCallBackDelegate(IntPtr hwnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern int EnumWindows(EnumWindowsCallBackDelegate callback, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int processId);

        public static IntPtr[] GetTopLevelWindows()
        {
            var windowList = new List<IntPtr>();
            var handle = GCHandle.Alloc(windowList);
            try
            {
                EnumWindows(EnumWindowsCallback, (IntPtr)handle);
            }
            finally
            {
                handle.Free();
            }

            return windowList.ToArray();
        }

        private static bool EnumWindowsCallback(IntPtr hwnd, IntPtr lParam)
        {
            var target = ((GCHandle)lParam).Target;

            if (target is not List<IntPtr> intPtrs)
            {
                return false;
            }

            intPtrs.Add(hwnd);

            return true;
        }

        public static Dictionary<int, IList<IntPtr>> GetProcessesAndWindows()
        {
            var map = new Dictionary<int, IList<IntPtr>>();
            var rootWindows = GetTopLevelWindows();

            foreach (var rootWindow in rootWindows)
            {
                GetWindowThreadProcessId(rootWindow, out var processId);

                if (map.TryGetValue(processId, out var windows) == false)
                {
                    windows = new List<IntPtr>();
                    map.Add(processId, windows);
                }

                windows.Add(rootWindow);
            }

            return map;
        }
    }
}
