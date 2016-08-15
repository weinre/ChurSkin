using System.Runtime.InteropServices;

namespace Win32.Com
{
    [StructLayout(LayoutKind.Sequential), ComVisible(true)]
    public struct tagOLECMD
    {
        [MarshalAs(UnmanagedType.U4)] public uint cmdID;
        [MarshalAs(UnmanagedType.U4)] public uint cmdf;
    }
}