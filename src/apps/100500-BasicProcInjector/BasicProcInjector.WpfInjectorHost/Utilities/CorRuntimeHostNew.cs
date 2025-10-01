namespace BasicProcInjector.WpfInjectorHost.Utilities
{
    using System;
    using System.Runtime.InteropServices;

    [ComImport]
    [Guid("D6D7D95D-B045-4DD1-B139-F2071888A071")]
    [CoClass(typeof(CorRuntimeHostClassNew))]
    public interface CorRuntimeHostNew : ICorRuntimeHostNew
    {
    }
}
