using System.Runtime.InteropServices;

namespace Win32.Com
{
    [StructLayout(LayoutKind.Sequential), ComVisible(true)]
    public struct tagRECT
    {
        [MarshalAs(UnmanagedType.I4)] public int Left;
        [MarshalAs(UnmanagedType.I4)] public int Top;
        [MarshalAs(UnmanagedType.I4)] public int Right;
        [MarshalAs(UnmanagedType.I4)] public int Bottom;

        public tagRECT(int left_, int top_, int right_, int bottom_)
        {
            Left = left_;
            Top = top_;
            Right = right_;
            Bottom = bottom_;
        }
    }
}