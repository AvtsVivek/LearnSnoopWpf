namespace Snoop.WpfClient.Utilities
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport]
    [Guid("55B6CDE7-121D-4A37-AEF9-F7BF0DA7D2DC")]
    [InterfaceType(1)]
    public interface IVEHandlerNew
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        void VEHandler([In][MarshalAs(UnmanagedType.Error)] int VECode, [In] tag_VerErrorNew Context, [In] [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_VARIANT)]
                       Array psa);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void SetReporterFtn([In] long lFnPtr);
    }
}
