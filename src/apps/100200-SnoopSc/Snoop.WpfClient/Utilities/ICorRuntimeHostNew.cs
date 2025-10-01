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
    [Guid("D6D7D95D-B045-4DD1-B139-F2071888A071")]
    [InterfaceType(1)]
    [ComConversionLoss]
    public interface ICorRuntimeHostNew
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        void CreateLogicalThreadState();

        [MethodImpl(MethodImplOptions.InternalCall)]
        void DeleteLogicalThreadState();

        [MethodImpl(MethodImplOptions.InternalCall)]
        void SwitchInLogicalThreadState([In] ref uint pFiberCookie);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void SwitchOutLogicalThreadState([Out] IntPtr pFiberCookie);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void LocksHeldByLogicalThread(out uint pCount);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void MapFile([In] IntPtr hFile, out IntPtr hMapAddress);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void GetConfiguration([MarshalAs(UnmanagedType.Interface)] out ICorConfigurationNew pConfiguration);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void Start();

        [MethodImpl(MethodImplOptions.InternalCall)]
        void Stop();

        [MethodImpl(MethodImplOptions.InternalCall)]
        void CreateDomain([In][MarshalAs(UnmanagedType.LPWStr)] string pwzFriendlyName, [In] [MarshalAs(UnmanagedType.IUnknown)]
                          object pIdentityArray, [MarshalAs(UnmanagedType.IUnknown)] out object pAppDomain);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void GetDefaultDomain([MarshalAs(UnmanagedType.IUnknown)] out object pAppDomain);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void EnumDomains(out IntPtr hEnum);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void NextDomain([In] IntPtr hEnum, [MarshalAs(UnmanagedType.IUnknown)] out object pAppDomain);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void CloseEnum([In] IntPtr hEnum);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void CreateDomainEx([In][MarshalAs(UnmanagedType.LPWStr)] string pwzFriendlyName, [In] [MarshalAs(UnmanagedType.IUnknown)]
                            object pSetup, [In] [MarshalAs(UnmanagedType.IUnknown)]
                            object pEvidence, [MarshalAs(UnmanagedType.IUnknown)] out object pAppDomain);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void CreateDomainSetup([MarshalAs(UnmanagedType.IUnknown)] out object pAppDomainSetup);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void CreateEvidence([MarshalAs(UnmanagedType.IUnknown)] out object pEvidence);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void UnloadDomain([In] [MarshalAs(UnmanagedType.IUnknown)]
                          object pAppDomain);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void CurrentDomain([MarshalAs(UnmanagedType.IUnknown)] out object pAppDomain);
    }
}
