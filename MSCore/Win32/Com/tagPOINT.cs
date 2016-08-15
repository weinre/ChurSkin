using System.Runtime.InteropServices;

namespace Win32.Com
{
    [StructLayout(LayoutKind.Sequential), ComVisible(true)]
    public struct tagPOINT
    {
        [MarshalAs(UnmanagedType.I4)] public int X;
        [MarshalAs(UnmanagedType.I4)] public int Y;
    }
}