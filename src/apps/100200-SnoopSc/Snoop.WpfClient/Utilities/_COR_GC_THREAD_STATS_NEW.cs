namespace Snoop.WpfClient.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public struct _COR_GC_THREAD_STATS_NEW
    {
        public ulong PerThreadAllocation;

        public uint Flags;
    }
}
