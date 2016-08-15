using System.Runtime.InteropServices;

namespace Win32
{
    [StructLayout(LayoutKind.Sequential), ComVisible(false)]
    public sealed class STATDATA
    {
        [MarshalAs(UnmanagedType.U4)] public int advf;
        [MarshalAs(UnmanagedType.U4)] public int dwConnection;
    }
}