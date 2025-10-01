namespace BasicProcInjector.WpfInjectorHost.Utilities
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport]
    [Guid("ECB2A741-61E0-4AED-9858-7A782C2B2033")]
    [InterfaceType(1)]
    public interface IGCHostControlNew
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        void RequestVirtualMemLimit([In][ComAliasName("mscoree.ULONG_PTR")] uint sztMaxVirtualMemMB, [In][Out][ComAliasName("mscoree.ULONG_PTR")] ref uint psztNewMaxVirtualMemMB);
    }
}
