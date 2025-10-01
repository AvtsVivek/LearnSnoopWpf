namespace BasicProcInjector.WpfInjectorHost.Utilities
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport]
    [InterfaceType(1)]
    [Guid("740783FF-F218-4E46-A0D6-282FFBC2EE62")]
    public interface ICorConfigurationNew
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        void SetGCThreadControl([In][MarshalAs(UnmanagedType.Interface)] IGCThreadControlNew pGCThreadControl);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void SetGCHostControl([In][MarshalAs(UnmanagedType.Interface)] IGCHostControlNew pGCHostControl);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void SetDebuggerThreadControl([In][MarshalAs(UnmanagedType.Interface)] IDebuggerThreadControlNew pDebuggerThreadControl);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void AddDebuggerSpecialThread([In] uint dwSpecialThreadId);
    }
}
