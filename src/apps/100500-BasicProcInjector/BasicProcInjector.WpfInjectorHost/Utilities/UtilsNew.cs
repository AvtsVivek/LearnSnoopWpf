namespace BasicProcInjector.WpfInjectorHost.Utilities
{
    using System;
    using System.Diagnostics;

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
