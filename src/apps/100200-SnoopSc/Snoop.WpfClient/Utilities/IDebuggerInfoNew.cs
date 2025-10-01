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
    [Guid("BFE5F992-A84F-4112-8F62-BCE49D21F0F6")]
    [InterfaceType(1)]
    public interface IDebuggerInfoNew
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        void IsDebuggerAttached(out int pbAttached);
    }
}
