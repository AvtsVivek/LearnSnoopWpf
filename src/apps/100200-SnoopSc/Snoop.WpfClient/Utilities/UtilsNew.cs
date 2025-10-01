namespace Snoop.WpfClient.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public static class UtilsNew
    {
        public static T? IgnoreErrors<T>(Func<T> action, T? defaultValue = default)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                return defaultValue;
            }
        }
    }
}
