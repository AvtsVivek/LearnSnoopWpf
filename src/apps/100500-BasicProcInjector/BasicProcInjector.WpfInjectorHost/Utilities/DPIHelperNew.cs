namespace BasicProcInjector.WpfInjectorHost.Utilities
{
    using BasicProcInjector.Core;
    using System;
    using System.Windows;
    using System.Windows.Interop;

    public static class DPIHelperNew
    {
        public static Point DevicePixelsToLogical(POINT devicePoint, IntPtr hwnd)
        {
            return DevicePixelsToLogical(new Point(devicePoint.X, devicePoint.Y), hwnd);
        }

        public static Point DevicePixelsToLogical(Point devicePoint, IntPtr hwnd)
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
