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
    [Guid("ECB2A741-61E0-4AED-9858-7A782C2B2033")]
    [InterfaceType(1)]
    public interface IGCHostControlNew
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        void RequestVirtualMemLimit([In][ComAliasName("mscoree.ULONG_PTR")] uint sztMaxVirtualMemMB, [In][Out][ComAliasName("mscoree.ULONG_PTR")] ref uint psztNewMaxVirtualMemMB);
    }
}
