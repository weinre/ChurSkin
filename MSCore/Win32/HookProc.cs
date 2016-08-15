using System;

namespace Win32.Callback
{
    public delegate int HookProc(int ncode, IntPtr wParam, IntPtr lParam);
}