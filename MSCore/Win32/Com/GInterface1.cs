﻿using System;
using System.Runtime.InteropServices;

namespace Win32.Com
{
    [ComImport, Guid("00000115-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown),
     ComVisible(true)]
    public interface GInterface1
    {
        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetWindow([In, Out] ref IntPtr phwnd);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int ContextSensitiveHelp([In, MarshalAs(UnmanagedType.Bool)] bool fEnterMode);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int GetBorder([In, Out, MarshalAs(UnmanagedType.Struct)] ref tagRECT lprectBorder);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int RequestBorderSpace([In, MarshalAs(UnmanagedType.Struct)] ref tagRECT pborderwidths);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int SetBorderSpace([In, MarshalAs(UnmanagedType.Struct)] ref tagRECT pborderwidths);

        [return: MarshalAs(UnmanagedType.I4)]
        [PreserveSig]
        int SetActiveObject([In, MarshalAs(UnmanagedType.Interface)] ref IOleInPlaceActiveObject pActiveObject,
            [In, MarshalAs(UnmanagedType.LPWStr)] string pszObjName);
    }
}