namespace BasicProcInjector.WpfInjectorHost.Utilities
{
    using System;
    using System.Globalization;

    public class InvariantThreadCultureScopeNew : IDisposable
    {
        private readonly CultureInfo fallbackCulture;

        public InvariantThreadCultureScopeNew()
        {
            this.fallbackCulture = Thread.CurrentThread.CurrentCulture;

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        }

        public void Dispose()
        {
            Thread.CurrentThread.CurrentCulture = this.fallbackCulture;
        }
    }
}
