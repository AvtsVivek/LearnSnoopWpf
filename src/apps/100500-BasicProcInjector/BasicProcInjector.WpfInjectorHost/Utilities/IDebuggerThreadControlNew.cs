namespace BasicProcInjector.WpfInjectorHost.Utilities
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport]
    [Guid("899F60E5-5290-47E3-9D67-32677720CEE6")]
    [InterfaceType(1)]
    public interface IDebuggerThreadControlNew
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        void ThreadIsBlockingForDebugger();

        [MethodImpl(MethodImplOptions.InternalCall)]
        void ReleaseAllRuntimeThreads();

        [MethodImpl(MethodImplOptions.InternalCall)]
        void StartBlockingForDebugger(uint dwUnused);
    }
}
