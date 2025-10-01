namespace Snoop.WpfClient.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    [ComImport]
    [InterfaceType(1)]
    [Guid("A7A42B35-A474-4BF6-8282-54DC3AB3EB74")]
    public interface IValidatorNew
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        void Validate([In] [MarshalAs(UnmanagedType.Interface)]
                      IVEHandlerNew veh, [In] [MarshalAs(UnmanagedType.IUnknown)]
                      object pAppDomain, [In] uint ulFlags, [In] uint ulMaxError, [In] uint Token, [In][MarshalAs(UnmanagedType.LPWStr)] string fileName, [In] ref byte pe, [In] uint ulSize);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void FormatEventInfo([In][MarshalAs(UnmanagedType.Error)] int hVECode, [In] tag_VerErrorNew Context, [In] [Out] [MarshalAs(UnmanagedType.LPWStr)]
                             StringBuilder msg, [In] uint ulMaxLength, [In] [MarshalAs(UnmanagedType.SafeArray, SafeArraySubType = VarEnum.VT_VARIANT)]
                             Array psa);
    }
}
