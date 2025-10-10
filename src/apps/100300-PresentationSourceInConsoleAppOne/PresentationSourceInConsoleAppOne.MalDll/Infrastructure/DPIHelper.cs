using PresentationSourceInConsoleAppOne.Core;
using System.Windows;
using System.Windows.Interop;

namespace PresentationSourceInConsoleAppOne.MalDll.Infrastructure
{
    public static class DPIHelper
    {
        public static System.Windows.Point DevicePixelsToLogical(POINT devicePoint, IntPtr hwnd)
        {
            return DevicePixelsToLogical(new System.Windows.Point(devicePoint.X, devicePoint.Y), hwnd);
        }

        public static System.Windows.Point DevicePixelsToLogical(System.Windows.Point devicePoint, IntPtr hwnd)
        {
            var hwndSource = HwndSource.FromHwnd(hwnd);

            if (hwndSource?.CompositionTarget is null)
            {
                return devicePoint;
            }

            return hwndSource.CompositionTarget.TransformFromDevice.Transform(devicePoint);
        }
    }
}
