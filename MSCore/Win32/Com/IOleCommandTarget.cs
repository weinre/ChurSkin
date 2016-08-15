using System;
using System.Runtime.InteropServices;

namespace Win32.Com
{
    [ComImport, ComVisible(true), InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
     Guid("B722BCCB-4E68-101B-A2BC-00AA00404770")]
    public interface IOleCommandTarget
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int QueryStatus([In] IntPtr pguidCmdGroup, [In, MarshalAs(UnmanagedType.U4)] uint cCmds,
            [In, Out, MarshalAs(UnmanagedType.Struct)] ref tagOLECMD prgCmds, [In, Out] IntPtr pCmdText);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int Exec([In] IntPtr pguidCmdGroup, [In, MarshalAs(UnmanagedType.U4)] uint nCmdID,
            [In, MarshalAs(UnmanagedType.U4)] uint nCmdexecopt, [In] IntPtr pvaIn, [In, Out] IntPtr pvaOut);
    }
}