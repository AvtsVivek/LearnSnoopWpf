using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WpfWindowTreeViewAnalysisOne.WpfUi
{
    public static class ExtensionMethos
    {
        private const DispatcherPriority DefaultDispatcherPriority = DispatcherPriority.Normal;

        public static T RunInDispatcher<T>(this DispatcherObject? dispatcher, Func<T> action, DispatcherPriority priority = DefaultDispatcherPriority)
        {
            if (dispatcher is null)
            {
                return action();
            }

            return dispatcher.Dispatcher.RunInDispatcher(action, priority);
        }
        public static T RunInDispatcher<T>(this Dispatcher? dispatcher, Func<T> action, DispatcherPriority priority = DefaultDispatcherPriority)
        {
            if (dispatcher is null
                || dispatcher.CheckAccess())
            {
                return action();
            }

            return (T)dispatcher.Invoke(priority, action);
        }
    }
}
