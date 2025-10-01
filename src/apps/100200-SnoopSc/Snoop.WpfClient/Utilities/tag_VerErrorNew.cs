namespace Snoop.WpfClient.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    [ComConversionLoss]
    public struct tag_VerErrorNew
    {
        public uint Flags;

        public uint opcode;

        public uint uOffset;

        public uint Token;

        public uint item1_flags;

        [ComConversionLoss]
        public IntPtr item1_data;

        public uint item2_flags;

        [ComConversionLoss]
        public IntPtr item2_data;
    }
}
