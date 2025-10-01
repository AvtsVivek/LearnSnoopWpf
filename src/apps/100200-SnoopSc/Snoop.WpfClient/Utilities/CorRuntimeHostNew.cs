namespace Snoop.WpfClient.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;

    [ComImport]
    [Guid("D6D7D95D-B045-4DD1-B139-F2071888A071")]
    [CoClass(typeof(CorRuntimeHostClassNew))]
    public interface CorRuntimeHostNew : ICorRuntimeHostNew
    {
    }
}
