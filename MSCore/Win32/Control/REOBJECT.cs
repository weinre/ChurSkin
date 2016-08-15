using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Win32
{
    [StructLayout(LayoutKind.Sequential)]
    public class REOBJECT
    {
        public int cbStruct;
        public int cp;
        public Guid clsid;
        public IntPtr poleobj;
        public IStorage pstg;
        public IOleClientSite polesite;
        public Size sizel;
        public uint dvAspect;
        public uint dwFlags;
        public uint dwUser;

        public REOBJECT()
        {
            cbStruct = Marshal.SizeOf(typeof (REOBJECT));
        }
    }
}