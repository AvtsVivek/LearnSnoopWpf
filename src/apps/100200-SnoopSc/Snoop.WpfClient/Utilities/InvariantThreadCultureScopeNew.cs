namespace Snoop.WpfClient.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
