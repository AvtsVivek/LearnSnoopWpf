namespace Snoop.WpfClient.Utilities
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;

    [ComImport]
    [InterfaceType(1)]
    [Guid("17CC9052-3D09-4B9E-B28F-BAD55F3568CD")]
    public interface IGCThreadControlNew
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        void ThreadIsBlockingForSuspension();

        [MethodImpl(MethodImplOptions.InternalCall)]
        void SuspensionStarting();

        [MethodImpl(MethodImplOptions.InternalCall)]
        void SuspensionEnding(uint Generation);
    }
}
