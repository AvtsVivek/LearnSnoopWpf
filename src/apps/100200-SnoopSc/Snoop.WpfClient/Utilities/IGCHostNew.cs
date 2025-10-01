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
    [Guid("9142A031-EB5C-4CAE-BC71-87A02440A7AD")]
    [InterfaceType(1)]
    public interface IGCHostNew
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        void SetGCStartupLimits([In] uint SegmentSize, [In] uint MaxGen0Size);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void Collect([In] int Generation);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void GetStats([In][Out] ref _COR_GC_STATS pStats);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void GetThreadStats([In] ref uint pFiberCookie, [In][Out] ref _COR_GC_THREAD_STATS_NEW pStats);

        [MethodImpl(MethodImplOptions.InternalCall)]
        void SetVirtualMemLimit([In][ComAliasName("mscoree.ULONG_PTR")] uint sztMaxVirtualMemMB);
    }
}
