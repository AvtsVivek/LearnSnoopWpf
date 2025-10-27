using ProcInjectorNoSettingsFile.Core;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Interop;

namespace ProcInjectorNoSettingsFile.MalDll.Infrastructure
{
    public static class SnoopWindowUtils
    {
        public static Window? FindOwnerWindow(Window ownedWindow)
        {
            var ownerWindow = TransientSettingsData.Current is not null
                ? WindowHelperNew.GetVisibleWindow(TransientSettingsData.Current.TargetWindowHandle, ownedWindow.Dispatcher)
                : null;

            if (System.Windows.Application.Current.MainWindow is not null
                && System.Windows.Application.Current.MainWindow.CheckAccess()
                && System.Windows.Application.Current.MainWindow.Visibility == Visibility.Visible)
            {
                // first: set the owner window as the current application's main window, if visible.
                ownerWindow = System.Windows.Application.Current.MainWindow;
            }
            else
            {
                // second: try and find a visible window in the list of the current application's windows
                foreach (Window? window in System.Windows.Application.Current.Windows)
                {
                    if (window is null)
                    {
                        continue;
                    }

                    if (window.CheckAccess()
                        && window.Visibility == Visibility.Visible)
                    {
                        ownerWindow = window;
                        break;
                    }
                }
            }

            if (ReferenceEquals(ownerWindow, ownedWindow))
            {
                return null;
            }

            if (ownerWindow is not null
                && ownerWindow.Dispatcher != ownedWindow.Dispatcher)
            {
                return null;
            }

            return ownerWindow;
        }
    }
}
