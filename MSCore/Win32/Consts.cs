using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Win32.Consts
{
    /// <summary>
    ///     Win32 API 常量
    /// </summary>
    /// <summary>
    ///     The XFORM structure specifies a world-space to page-space transformation.
    /// </summary>
 

    public static class CDDS
    {
        public const int CDDS_ITEM = 0x10000;
        public const int CDDS_ITEMPOSTERASE = 0x10004;
        public const int CDDS_ITEMPOSTPAINT = 0x10002;
        public const int CDDS_ITEMPREERASE = 0x10003;
        public const int CDDS_ITEMPREPAINT = 0x10001;
        public const int CDDS_POSTERASE = 4;
        public const int CDDS_POSTPAINT = 2;
        public const int CDDS_PREERASE = 3;
        public const int CDDS_PREPAINT = 1;
    }

    public static class DIBColorTableIdentifier
    {
        public const byte DIB_RGB_COLORS = 0;
        public const byte DIB_PAL_COLORS = 1;
    }

    public static class CompressionType
    {
        public const byte BI_RGB = 0;
        public const byte BI_RLE8 = 1;
        public const byte BI_RLE4 = 2;
        public const byte BI_BITFIELDS = 3;
        public const byte BI_JPEG = 4;
        public const byte BI_PNG = 5;
    }

    public enum ComboBoxButtonState
    {
        STATE_SYSTEM_INVISIBLE = 0x8000,
        STATE_SYSTEM_NONE = 0,
        STATE_SYSTEM_PRESSED = 8
    }


    public static class CDRF
    {
        public const int CDRF_DODEFAULT = 0;
        public const int CDRF_NEWFONT = 2;
        public const int CDRF_NOTIFYITEMDRAW = 0x20;
        public const int CDRF_NOTIFYITEMERASE = 0x80;
        public const int CDRF_NOTIFYPOSTERASE = 0x40;
        public const int CDRF_NOTIFYPOSTPAINT = 0x10;
        public const int CDRF_SKIPDEFAULT = 4;
    }

    public class CombineRgnStyles
    {
        public const int RGN_AND = 1;
        public const int RGN_COPY = 5;
        public const int RGN_DIFF = 4;
        public const int RGN_MAX = 5;
        public const int RGN_MIN = 1;
        public const int RGN_OR = 2;
        public const int RGN_XOR = 3;

        private CombineRgnStyles()
        {
        }
    }

    public class SPI
    {
        public const uint SPI_GETBEEP = 0x0001;
        public const uint SPI_SETBEEP = 0x0002;
        public const uint SPI_GETMOUSE = 0x0003;
        public const uint SPI_SETMOUSE = 0x0004;
        public const uint SPI_GETBORDER = 0x0005;
        public const uint SPI_SETBORDER = 0x0006;
        public const uint SPI_GETKEYBOARDSPEED = 0x000A;
        public const uint SPI_SETKEYBOARDSPEED = 0x000B;
        public const uint SPI_LANGDRIVER = 0x000C;
        public const uint SPI_ICONHORIZONTALSPACING = 0x000D;
        public const uint SPI_GETSCREENSAVETIMEOUT = 0x000E;
        public const uint SPI_SETSCREENSAVETIMEOUT = 0x000F;
        public const uint SPI_GETSCREENSAVEACTIVE = 0x0010;
        public const uint SPI_SETSCREENSAVEACTIVE = 0x0011;
        public const uint SPI_GETGRIDGRANULARITY = 0x0012;
        public const uint SPI_SETGRIDGRANULARITY = 0x0013;
        public const uint SPI_SETDESKWALLPAPER = 0x0014;
        public const uint SPI_SETDESKPATTERN = 0x0015;
        public const uint SPI_GETKEYBOARDDELAY = 0x0016;
        public const uint SPI_SETKEYBOARDDELAY = 0x0017;
        public const uint SPI_ICONVERTICALSPACING = 0x0018;
        public const uint SPI_GETICONTITLEWRAP = 0x0019;
        public const uint SPI_SETICONTITLEWRAP = 0x001A;
        public const uint SPI_GETMENUDROPALIGNMENT = 0x001B;
        public const uint SPI_SETMENUDROPALIGNMENT = 0x001C;
        public const uint SPI_SETDOUBLECLKWIDTH = 0x001D;
        public const uint SPI_SETDOUBLECLKHEIGHT = 0x001E;
        public const uint SPI_GETICONTITLELOGFONT = 0x001F;
        public const uint SPI_SETDOUBLECLICKTIME = 0x0020;
        public const uint SPI_SETMOUSEBUTTONSWAP = 0x0021;
        public const uint SPI_SETICONTITLELOGFONT = 0x0022;
        public const uint SPI_GETFASTTASKSWITCH = 0x0023;
        public const uint SPI_SETFASTTASKSWITCH = 0x0024;
        public const uint SPI_SETDRAGFULLWINDOWS = 0x0025;
        public const uint SPI_GETDRAGFULLWINDOWS = 0x0026;
        public const uint SPI_GETNONCLIENTMETRICS = 0x0029;
        public const uint SPI_SETNONCLIENTMETRICS = 0x002A;
        public const uint SPI_GETMINIMIZEDMETRICS = 0x002B;
        public const uint SPI_SETMINIMIZEDMETRICS = 0x002C;
        public const uint SPI_GETICONMETRICS = 0x002D;
        public const uint SPI_SETICONMETRICS = 0x002E;
        public const uint SPI_SETWORKAREA = 0x002F;
        public const uint SPI_GETWORKAREA = 0x0030;
        public const uint SPI_SETPENWINDOWS = 0x0031;
        public const uint SPI_GETHIGHCONTRAST = 0x0042;
        public const uint SPI_SETHIGHCONTRAST = 0x0043;
        public const uint SPI_GETKEYBOARDPREF = 0x0044;
        public const uint SPI_SETKEYBOARDPREF = 0x0045;
        public const uint SPI_GETSCREENREADER = 0x0046;
        public const uint SPI_SETSCREENREADER = 0x0047;
        public const uint SPI_GETANIMATION = 0x0048;
        public const uint SPI_SETANIMATION = 0x0049;
        public const uint SPI_GETFONTSMOOTHING = 0x004A;
        public const uint SPI_SETFONTSMOOTHING = 0x004B;
        public const uint SPI_SETDRAGWIDTH = 0x004C;
        public const uint SPI_SETDRAGHEIGHT = 0x004D;
        public const uint SPI_SETHANDHELD = 0x004E;
        public const uint SPI_GETLOWPOWERTIMEOUT = 0x004F;
        public const uint SPI_GETPOWEROFFTIMEOUT = 0x0050;
        public const uint SPI_SETLOWPOWERTIMEOUT = 0x0051;
        public const uint SPI_SETPOWEROFFTIMEOUT = 0x0052;
        public const uint SPI_GETLOWPOWERACTIVE = 0x0053;
        public const uint SPI_GETPOWEROFFACTIVE = 0x0054;
        public const uint SPI_SETLOWPOWERACTIVE = 0x0055;
        public const uint SPI_SETPOWEROFFACTIVE = 0x0056;
        public const uint SPI_SETICONS = 0x0058;
        public const uint SPI_GETDEFAULTINPUTLANG = 0x0059;
        public const uint SPI_SETDEFAULTINPUTLANG = 0x005A;
        public const uint SPI_SETLANGTOGGLE = 0x005B;
        public const uint SPI_GETWINDOWSEXTENSION = 0x005C;
        public const uint SPI_SETMOUSETRAILS = 0x005D;
        public const uint SPI_GETMOUSETRAILS = 0x005E;
        public const uint SPI_SCREENSAVERRUNNING = 0x0061;
        public const uint SPI_GETFILTERKEYS = 0x0032;
        public const uint SPI_SETFILTERKEYS = 0x0033;
        public const uint SPI_GETTOGGLEKEYS = 0x0034;
        public const uint SPI_SETTOGGLEKEYS = 0x0035;
        public const uint SPI_GETMOUSEKEYS = 0x0036;
        public const uint SPI_SETMOUSEKEYS = 0x0037;
        public const uint SPI_GETSHOWSOUNDS = 0x0038;
        public const uint SPI_SETSHOWSOUNDS = 0x0039;
        public const uint SPI_GETSTICKYKEYS = 0x003A;
        public const uint SPI_SETSTICKYKEYS = 0x003B;
        public const uint SPI_GETACCESSTIMEOUT = 0x003C;
        public const uint SPI_SETACCESSTIMEOUT = 0x003D;
        public const uint SPI_GETSERIALKEYS = 0x003E;
        public const uint SPI_SETSERIALKEYS = 0x003F;
        public const uint SPI_GETSOUNDSENTRY = 0x0040;
        public const uint SPI_SETSOUNDSENTRY = 0x0041;
        public const uint SPI_GETSNAPTODEFBUTTON = 0x005F;
        public const uint SPI_SETSNAPTODEFBUTTON = 0x0060;
        public const uint SPI_GETMOUSEHOVERWIDTH = 0x0062;
        public const uint SPI_SETMOUSEHOVERWIDTH = 0x0063;
        public const uint SPI_GETMOUSEHOVERHEIGHT = 0x0064;
        public const uint SPI_SETMOUSEHOVERHEIGHT = 0x0065;
        public const uint SPI_GETMOUSEHOVERTIME = 0x0066;
        public const uint SPI_SETMOUSEHOVERTIME = 0x0067;
        public const uint SPI_GETWHEELSCROLLLINES = 0x0068;
        public const uint SPI_SETWHEELSCROLLLINES = 0x0069;
        public const uint SPI_GETMENUSHOWDELAY = 0x006A;
        public const uint SPI_SETMENUSHOWDELAY = 0x006B;
        public const uint SPI_GETSHOWIMEUI = 0x006E;
        public const uint SPI_SETSHOWIMEUI = 0x006F;
        public const uint SPI_GETMOUSESPEED = 0x0070;
        public const uint SPI_SETMOUSESPEED = 0x0071;
        public const uint SPI_GETSCREENSAVERRUNNING = 0x0072;
        public const uint SPI_GETDESKWALLPAPER = 0x0073;
        public const uint SPI_GETACTIVEWINDOWTRACKING = 0x1000;
        public const uint SPI_SETACTIVEWINDOWTRACKING = 0x1001;
        public const uint SPI_GETMENUANIMATION = 0x1002;
        public const uint SPI_SETMENUANIMATION = 0x1003;
        public const uint SPI_GETCOMBOBOXANIMATION = 0x1004;
        public const uint SPI_SETCOMBOBOXANIMATION = 0x1005;
        public const uint SPI_GETLISTBOXSMOOTHSCROLLING = 0x1006;
        public const uint SPI_SETLISTBOXSMOOTHSCROLLING = 0x1007;
        public const uint SPI_GETGRADIENTCAPTIONS = 0x1008;
        public const uint SPI_SETGRADIENTCAPTIONS = 0x1009;
        public const uint SPI_GETKEYBOARDCUES = 0x100A;
        public const uint SPI_SETKEYBOARDCUES = 0x100B;
        public const uint SPI_GETMENUUNDERLINES = SPI_GETKEYBOARDCUES;
        public const uint SPI_SETMENUUNDERLINES = SPI_SETKEYBOARDCUES;
        public const uint SPI_GETACTIVEWNDTRKZORDER = 0x100C;
        public const uint SPI_SETACTIVEWNDTRKZORDER = 0x100D;
        public const uint SPI_GETHOTTRACKING = 0x100E;
        public const uint SPI_SETHOTTRACKING = 0x100F;
        public const uint SPI_GETMENUFADE = 0x1012;
        public const uint SPI_SETMENUFADE = 0x1013;
        public const uint SPI_GETSELECTIONFADE = 0x1014;
        public const uint SPI_SETSELECTIONFADE = 0x1015;
        public const uint SPI_GETTOOLTIPANIMATION = 0x1016;
        public const uint SPI_SETTOOLTIPANIMATION = 0x1017;
        public const uint SPI_GETTOOLTIPFADE = 0x1018;
        public const uint SPI_SETTOOLTIPFADE = 0x1019;
        public const uint SPI_GETCURSORSHADOW = 0x101A;
        public const uint SPI_SETCURSORSHADOW = 0x101B;
        public const uint SPI_GETMOUSESONAR = 0x101C;
        public const uint SPI_SETMOUSESONAR = 0x101D;
        public const uint SPI_GETMOUSECLICKLOCK = 0x101E;
        public const uint SPI_SETMOUSECLICKLOCK = 0x101F;
        public const uint SPI_GETMOUSEVANISH = 0x1020;
        public const uint SPI_SETMOUSEVANISH = 0x1021;
        public const uint SPI_GETFLATMENU = 0x1022;
        public const uint SPI_SETFLATMENU = 0x1023;
        public const uint SPI_GETDROPSHADOW = 0x1024;
        public const uint SPI_SETDROPSHADOW = 0x1025;
        public const uint SPI_GETBLOCKSENDINPUTRESETS = 0x1026;
        public const uint SPI_SETBLOCKSENDINPUTRESETS = 0x1027;
        public const uint SPI_GETUIEFFECTS = 0x103E;
        public const uint SPI_SETUIEFFECTS = 0x103F;
        public const uint SPI_GETFOREGROUNDLOCKTIMEOUT = 0x2000;
        public const uint SPI_SETFOREGROUNDLOCKTIMEOUT = 0x2001;
        public const uint SPI_GETACTIVEWNDTRKTIMEOUT = 0x2002;
        public const uint SPI_SETACTIVEWNDTRKTIMEOUT = 0x2003;
        public const uint SPI_GETFOREGROUNDFLASHCOUNT = 0x2004;
        public const uint SPI_SETFOREGROUNDFLASHCOUNT = 0x2005;
        public const uint SPI_GETCARETWIDTH = 0x2006;
        public const uint SPI_SETCARETWIDTH = 0x2007;
        public const uint SPI_GETMOUSECLICKLOCKTIME = 0x2008;
        public const uint SPI_SETMOUSECLICKLOCKTIME = 0x2009;
        public const uint SPI_GETFONTSMOOTHINGTYPE = 0x200A;
        public const uint SPI_SETFONTSMOOTHINGTYPE = 0x200B;
        public const uint SPI_GETFONTSMOOTHINGCONTRAST = 0x200C;
        public const uint SPI_SETFONTSMOOTHINGCONTRAST = 0x200D;
        public const uint SPI_GETFOCUSBORDERWIDTH = 0x200E;
        public const uint SPI_SETFOCUSBORDERWIDTH = 0x200F;
        public const uint SPI_GETFOCUSBORDERHEIGHT = 0x2010;
        public const uint SPI_SETFOCUSBORDERHEIGHT = 0x2011;
        public const uint SPI_GETFONTSMOOTHINGORIENTATION = 0x2012;
        public const uint SPI_SETFONTSMOOTHINGORIENTATION = 0x2013;
    }

    [Flags]
    public enum SPIF
    {
        None = 0x00,

        /// <summary>Writes the new system-wide parameter setting to the user profile.</summary>
        SPIF_UPDATEINIFILE = 0x01,

        /// <summary>Broadcasts the WM_SETTINGCHANGE message after updating the user profile.</summary>
        SPIF_SENDCHANGE = 0x02,

        /// <summary>Same as SPIF_SENDCHANGE.</summary>
        SPIF_SENDWININICHANGE = 0x02
    }

    public class DCX
    {
        public const int DCX_CACHE = 2;
        public const int DCX_CLIPCHILDREN = 8;
        public const int DCX_CLIPSIBLINGS = 0x10;
        public const int DCX_EXCLUDERGN = 0x40;
        public const int DCX_EXCLUDEUPDATE = 0x100;
        public const int DCX_INTERSECTRGN = 0x80;
        public const int DCX_INTERSECTUPDATE = 0x200;
        public const int DCX_LOCKWINDOWUPDATE = 0x400;
        public const int DCX_NORESETATTRS = 4;
        public const int DCX_PARENTCLIP = 0x20;
        public const int DCX_VALIDATE = 0x200000;
        public const int DCX_WINDOW = 1;
    }

    public enum StretchBltMode
    {
        STRETCH_ANDSCANS = 1,
        STRETCH_ORSCANS = 2,
        STRETCH_DELETESCANS = 3,
        STRETCH_HALFTONE = 4
    }

    public enum DLGC //DialogCodes
    {
        DLGC_WANTARROWS = 0x0001, /* Control wants arrow keys         */
        DLGC_WANTTAB = 0x0002, /* Control wants tab keys           */
        DLGC_WANTALLKEYS = 0x0004, /* Control wants all keys           */
        DLGC_WANTMESSAGE = 0x0004, /* Pass message to control          */
        DLGC_HASSETSEL = 0x0008, /* Understands EM_SETSEL message    */
        DLGC_DEFPUSHBUTTON = 0x0010, /* Default pushbutton               */
        DLGC_UNDEFPUSHBUTTON = 0x0020, /* Non-default pushbutton           */
        DLGC_RADIOBUTTON = 0x0040, /* Radio button                     */
        DLGC_WANTCHARS = 0x0080, /* Want WM_CHAR messages            */
        DLGC_STATIC = 0x0100, /* Static item: don't include       */
        DLGC_BUTTON = 0x2000 /* Button item: can be checked      */
    }

    public static class DI
    {
        public const int DI_COMPAT = 4;
        public const int DI_DEFAULTSIZE = 8;
        public const int DI_IMAGE = 2;
        public const int DI_MASK = 1;
        public const int DI_NOMIRROR = 0x10;
        public const int DI_NORMAL = 3;
    }

    public static class EM
    {
        public const int EM_GETSEL = 0xb0;
        public const int EM_LINEFROMCHAR = 0xc9;
        public const int EM_LINEINDEX = 0xbb;
        public const int EM_POSFROMCHAR = 0xd6;
    }

    public class ESB
    {
        public const int ESB_DISABLE_BOTH = 3;
        public const int ESB_DISABLE_DOWN = 2;
        public const int ESB_DISABLE_LEFT = 1;
        public const int ESB_DISABLE_LTUP = 1;
        public const int ESB_DISABLE_RIGHT = 2;
        public const int ESB_DISABLE_RTDN = 2;
        public const int ESB_DISABLE_UP = 1;
        public const int ESB_ENABLE_BOTH = 0;

        private ESB()
        {
        }
    }

    public class GCL
    {
        public const int GCL_HICON = -14;
        public const int GCL_STYLE = (-26);
    }

    public static class GM
    {
        public const int GM_COMPATIBLE = 0x1;
        public const int GM_ADVANCED = 0x2;
    }

    public static class GWL
    {
        public const int GWL_EXSTYLE = -20;
        public const int GWL_HINSTANCE = -6;
        public const int GWL_HWNDPARENT = -8;
        public const int GWL_ID = -12;
        public const int GWL_STYLE = -16;
        public const int GWL_USERDATA = -21;
        public const int GWL_WNDPROC = -4;
    }

    public sealed class HC
    {
        public const int HC_ACTION = 0;
        public const int HC_GETNEXT = 1;
        public const int HC_NOREM = 3;
        public const int HC_NOREMOVE = 3;
        public const int HC_SKIP = 2;
        public const int HC_SYSMODALOFF = 5;
        public const int HC_SYSMODALON = 4;
    }

    public class HITTEST
    {
        public const int HTBORDER = 0x12;
        public const int HTBOTTOM = 15;
        public const int HTBOTTOMLEFT = 0x10;
        public const int HTBOTTOMRIGHT = 0x11;
        public const int HTCAPTION = 2;
        public const int HTCLIENT = 1;
        public const int HTCLOSE = 20;
        public const int HTERROR = -2;
        public const int HTGROWBOX = 4;
        public const int HTHELP = 0x15;
        public const int HTHSCROLL = 6;
        public const int HTLEFT = 10;
        public const int HTMAXBUTTON = 9;
        public const int HTMENU = 5;
        public const int HTMINBUTTON = 8;
        public const int HTNOWHERE = 0;
        public const int HTOBJECT = 0x13;
        public const int HTRIGHT = 11;
        public const int HTSYSMENU = 3;
        public const int HTTOP = 12;
        public const int HTTOPLEFT = 13;
        public const int HTTOPRIGHT = 14;
        public const int HTTRANSPARENT = -1;
        public const int HTVSCROLL = 7;
    }

    public static class HWND
    {
        public static readonly IntPtr HWND_BOTTOM;
        public static readonly IntPtr HWND_NOTOPMOST;
        public static readonly IntPtr HWND_TOP;
        public static readonly IntPtr HWND_TOPMOST;

        static HWND()
        {
            HWND_TOPMOST = new IntPtr(-1);
            HWND_NOTOPMOST = new IntPtr(-2);
            HWND_TOP = new IntPtr(0);
            HWND_BOTTOM = new IntPtr(1);
        }
    }

    public static class ICC
    {
        public const int ICC_ANIMATE_CLASS = 0x80;
        public const int ICC_BAR_CLASSES = 4;
        public const int ICC_COOL_CLASSES = 0x400;
        public const int ICC_DATE_CLASSES = 0x100;
        public const int ICC_HOTKEY_CLASS = 0x40;
        public const int ICC_LISTVIEW_CLASSES = 1;
        public const int ICC_PROGRESS_CLASS = 0x20;
        public const int ICC_TAB_CLASSES = 8;
        public const int ICC_TREEVIEW_CLASSES = 2;
        public const int ICC_UPDOWN_CLASS = 0x10;
        public const int ICC_USEREX_CLASSES = 0x200;
        public const int ICC_WIN95_CLASSES = 0xff;
    }

    public static class ICON
    {
        public const int ICON_BIG = 1;
        public const int ICON_SMALL = 0;
        public const int ICON_SMALL2 = 2;
    }

    public class LPSTR
    {
        public static readonly IntPtr LPSTR_TEXTCALLBACK;

        static LPSTR()
        {
            LPSTR_TEXTCALLBACK = new IntPtr(-1);
        }
    }

    public sealed class MA
    {
        public const int MA_ACTIVATE = 1;
        public const int MA_ACTIVATEANDEAT = 2;
        public const int MA_NOACTIVATE = 3;
        public const int MA_NOACTIVATEANDEAT = 4;
    }

    public static class MF
    {
        /// <summary>
        ///     Indicates that uIDEnableItem gives the identifier of the menu item. If neither
        ///     the MF_BYCOMMAND nor MF_BYPOSITION flag is specified, the MF_BYCOMMAND flag is
        ///     the default flag.
        /// </summary>
        public const int MF_BYCOMMAND = 0x00000000;

        /// <summary>
        ///     Indicates that the menu item is enabled and restored from a grayed state so that it can be selected.
        /// </summary>
        public const int MF_ENABLED = 0x00000000;

        /// <summary>
        ///     Indicates that the menu item is disabled and grayed so that it cannot be selected.
        /// </summary>
        public const int MF_GRAYED = 0x00000001;

        /// <summary>
        ///     Indicates that the menu item is disabled, but not grayed, so it cannot be selected.
        /// </summary>
        public const int MF_DISABLED = 0x00000002;
    }

    public static class NM
    {
        public const int NM_CLICK = -2;
        public const int NM_DBLCLK = -3;
        public const int NM_FIRST = 0;
        public const int NM_KILLFOCUS = -8;
        public const int NM_OUTOFMEMORY = -1;
        public const int NM_RCLICK = -5;
        public const int NM_RDBLCLK = -6;
        public const int NM_RETURN = -4;
        public const int NM_SETFOCUS = -7;
    }

    public class OBJID
    {
        public const uint OBJID_ALERT = 0xfffffff6;
        public const uint OBJID_CARET = 0xfffffff8;
        public const uint OBJID_CLIENT = 0xfffffffc;
        public const uint OBJID_CURSOR = 0xfffffff7;
        public const uint OBJID_HSCROLL = 0xfffffffa;
        public const uint OBJID_MENU = 0xfffffffd;
        public const uint OBJID_SIZEGRIP = 0xfffffff9;
        public const uint OBJID_SOUND = 0xfffffff5;
        public const uint OBJID_SYSMENU = uint.MaxValue;
        public const uint OBJID_TITLEBAR = 0xfffffffe;
        public const uint OBJID_VSCROLL = 0xfffffffb;
        public const uint OBJID_WINDOW = 0;
    }

    public enum StockObjects
    {
        WHITE_BRUSH = 0,
        LTGRAY_BRUSH = 1,
        GRAY_BRUSH = 2,
        DKGRAY_BRUSH = 3,
        BLACK_BRUSH = 4,
        NULL_BRUSH = 5,
        HOLLOW_BRUSH = NULL_BRUSH,
        WHITE_PEN = 6,
        BLACK_PEN = 7,
        NULL_PEN = 8,
        OEM_FIXED_FONT = 10,
        ANSI_FIXED_FONT = 11,
        ANSI_VAR_FONT = 12,
        SYSTEM_FONT = 13,
        DEVICE_DEFAULT_FONT = 14,
        DEFAULT_PALETTE = 15,
        SYSTEM_FIXED_FONT = 16,
        DEFAULT_GUI_FONT = 17,
        DC_BRUSH = 18,
        DC_PEN = 19
    }

    public static class BlendOp
    {
        public const byte AC_SRC_OVER = 0x00;
        public const byte AC_SRC_ALPHA = 0x01;
    }

    public static class PRF
    {
        public const long PRF_CHECKVISIBLE = 1L;
        public const long PRF_CHILDREN = 0x10L;
        public const long PRF_CLIENT = 4L;
        public const long PRF_ERASEBKGND = 8L;
        public const long PRF_NONCLIENT = 2L;
        public const long PRF_OWNED = 0x20L;
    }

    public class RGN
    {
        public const int RGN_AND = 1;
        public const int RGN_COPY = 5;
        public const int RGN_DIFF = 4;
        public const int RGN_OR = 2;
        public const int RGN_XOR = 3;
    }

    /// <summary>
    ///     滚动条
    /// </summary>
    public class SB
    {
        public const int SB_BOTH = 3;
        public const int SB_BOTTOM = 7;
        public const int SB_ENDSCROLL = 8;
        public const int SB_HORZ = 0;
        public const int SB_LEFT = 6;
        public const int SB_LINEDOWN = 1;
        public const int SB_LINELEFT = 0;
        public const int SB_LINERIGHT = 1;
        public const int SB_LINEUP = 0;
        public const int SB_PAGEDOWN = 3;
        public const int SB_PAGELEFT = 2;
        public const int SB_PAGERIGHT = 3;
        public const int SB_PAGEUP = 2;
        public const int SB_RIGHT = 7;
        public const int SB_THUMBPOSITION = 4;
        public const int SB_THUMBTRACK = 5;
        public const int SB_TOP = 6;
        public const int SB_VERT = 1;
    }

    public class SBM
    {
        public const int SBM_ENABLE_ARROWS = 0xe4;
        public const int SBM_GETPOS = 0xe1;
        public const int SBM_GETRANGE = 0xe3;
        public const int SBM_GETSCROLLBARINFO = 0xeb;
        public const int SBM_GETSCROLLINFO = 0xea;
        public const int SBM_SETPOS = 0xe0;
        public const int SBM_SETRANGE = 0xe2;
        public const int SBM_SETRANGEREDRAW = 230;
        public const int SBM_SETSCROLLINFO = 0xe9;

        private SBM()
        {
        }
    }

    public class SIF
    {
        public const int SIF_ALL = 0x17;
        public const int SIF_DISABLENOSCROLL = 8;
        public const int SIF_PAGE = 2;
        public const int SIF_POS = 4;
        public const int SIF_RANGE = 1;
        public const int SIF_TRACKPOS = 0x10;
    }

    public static class TCHT
    {
        public const int TCHT_NOWHERE = 1;
        public const int TCHT_ONITEM = 6;
        public const int TCHT_ONITEMICON = 2;
        public const int TCHT_ONITEMLABEL = 4;
    }

    public static class TCM
    {
        public const int TCM_HITTEST = 0x130d;
    }

    public sealed class TME
    {
        public const uint TME_CANCEL = 0x80000000;
        public const uint TME_HOVER = 1;
        public const uint TME_LEAVE = 2;
        public const uint TME_NONCLIENT = 0x10;
        public const uint TME_QUERY = 0x40000000;
    }

    public sealed class TPM
    {
        public const int TPM_LEFTALIGN = 0;
        public const int TPM_RETURNCMD = 0x100;
        public const int TPM_TOPALIGN = 0;
    }

    public static class TTDT
    {
        public const int TTDT_AUTOMATIC = 0;
        public const int TTDT_AUTOPOP = 2;
        public const int TTDT_INITIAL = 3;
        public const int TTDT_RESHOW = 1;
    }

    public static class TTF
    {
        public const int TTF_ABSOLUTE = 0x80;
        public const int TTF_CENTERTIP = 2;
        public const int TTF_DI_SETITEM = 0x8000;
        public const int TTF_IDISHWND = 1;
        public const int TTF_PARSELINKS = 0x1000;
        public const int TTF_RTLREADING = 4;
        public const int TTF_SUBCLASS = 0x10;
        public const int TTF_TRACK = 0x20;
        public const int TTF_TRANSPARENT = 0x100;
    }

    public static class TTI
    {
        public const int TTI_ERROR = 3;
        public const int TTI_ERROR_LARGE = 6;
        public const int TTI_INFO = 1;
        public const int TTI_INFO_LARGE = 4;
        public const int TTI_NONE = 0;
        public const int TTI_WARNING = 2;
        public const int TTI_WARNING_LARGE = 5;
    }

    public static class TTM
    {
        public const int TTM_ACTIVATE = 0x401;
        public const int TTM_ADDTOOLA = 0x404;
        public const int TTM_ADDTOOLW = 0x432;
        public const int TTM_ADJUSTRECT = 0x41f;
        public const int TTM_DELTOOLA = 0x405;
        public const int TTM_DELTOOLW = 0x433;
        public const int TTM_ENUMTOOLSA = 0x40e;
        public const int TTM_ENUMTOOLSW = 0x43a;
        public const int TTM_GETCURRENTTOOLA = 0x40f;
        public const int TTM_GETCURRENTTOOLW = 0x43b;
        public const int TTM_GETDELAYTIME = 0x415;
        public const int TTM_GETMARGIN = 0x41b;
        public const int TTM_GETMAXTIPWIDTH = 0x419;
        public const int TTM_GETTEXTA = 0x40b;
        public const int TTM_GETTEXTW = 0x438;
        public const int TTM_GETTIPBKCOLOR = 0x416;
        public const int TTM_GETTIPTEXTCOLOR = 0x417;
        public const int TTM_GETTOOLCOUNT = 0x40d;
        public const int TTM_GETTOOLINFOA = 0x408;
        public const int TTM_GETTOOLINFOW = 0x435;
        public const int TTM_HITTESTA = 0x40a;
        public const int TTM_HITTESTW = 0x437;
        public const int TTM_NEWTOOLRECTA = 0x406;
        public const int TTM_NEWTOOLRECTW = 0x434;
        public const int TTM_POP = 0x41c;
        public const int TTM_POPUP = 0x422;
        public const int TTM_RELAYEVENT = 0x407;
        public const int TTM_SETDELAYTIME = 0x403;
        public const int TTM_SETMARGIN = 0x41a;
        public const int TTM_SETMAXTIPWIDTH = 0x418;
        public const int TTM_SETTIPBKCOLOR = 0x413;
        public const int TTM_SETTIPTEXTCOLOR = 0x414;
        public const int TTM_SETTITLEA = 0x420;
        public const int TTM_SETTITLEW = 0x421;
        public const int TTM_SETTOOLINFOA = 0x409;
        public const int TTM_SETTOOLINFOW = 0x436;
        public const int TTM_TRACKACTIVATE = 0x411;
        public const int TTM_TRACKPOSITION = 0x412;
        public const int TTM_UPDATE = 0x41d;
        public const int TTM_UPDATETIPTEXTA = 0x40c;
        public const int TTM_UPDATETIPTEXTW = 0x439;
        public const int TTM_WINDOWFROMPOINT = 0x410;
        public const int WM_USER = 0x400;
        public static readonly int TTM_ADDTOOL;
        public static readonly int TTM_DELTOOL;
        public static readonly int TTM_ENUMTOOLS;
        public static readonly int TTM_GETCURRENTTOOL;
        public static readonly int TTM_GETTEXT;
        public static readonly int TTM_GETTOOLINFO;
        public static readonly int TTM_HITTEST;
        public static readonly int TTM_NEWTOOLRECT;
        public static readonly int TTM_SETTITLE;
        public static readonly int TTM_SETTOOLINFO;
        public static readonly int TTM_UPDATETIPTEXT;

        static TTM()
        {
            if (Marshal.SystemDefaultCharSize != 1)
            {
                TTM_ADDTOOL = 0x432;
                TTM_DELTOOL = 0x433;
                TTM_NEWTOOLRECT = 0x434;
                TTM_GETTOOLINFO = 0x435;
                TTM_SETTOOLINFO = 0x436;
                TTM_HITTEST = 0x437;
                TTM_GETTEXT = 0x438;
                TTM_UPDATETIPTEXT = 0x439;
                TTM_GETCURRENTTOOL = 0x43b;
                TTM_ENUMTOOLS = 0x43a;
                TTM_GETCURRENTTOOL = 0x43b;
                TTM_SETTITLE = 0x421;
            }
            else
            {
                TTM_ADDTOOL = 0x404;
                TTM_DELTOOL = 0x405;
                TTM_NEWTOOLRECT = 0x406;
                TTM_GETTOOLINFO = 0x408;
                TTM_SETTOOLINFO = 0x409;
                TTM_HITTEST = 0x40a;
                TTM_GETTEXT = 0x40b;
                TTM_UPDATETIPTEXT = 0x40c;
                TTM_GETCURRENTTOOL = 0x40f;
                TTM_ENUMTOOLS = 0x40e;
                TTM_GETCURRENTTOOL = 0x40f;
                TTM_SETTITLE = 0x420;
            }
        }
    }

    public static class TTN
    {
        public const int TTN_FIRST = -520;
        public const int TTN_GETDISPINFOA = -520;
        public const int TTN_GETDISPINFOW = -530;
        public const int TTN_LAST = -549;
        public const int TTN_LINKCLICK = -523;
        public const int TTN_NEEDTEXTA = -520;
        public const int TTN_NEEDTEXTW = -530;
        public const int TTN_POP = -522;
        public const int TTN_SHOW = -521;
        public static readonly int TTN_GETDISPINFO;
        public static readonly int TTN_NEEDTEXT;

        static TTN()
        {
            if (Marshal.SystemDefaultCharSize != 1)
            {
                TTN_GETDISPINFO = -530;
                TTN_NEEDTEXT = -530;
            }
            else
            {
                TTN_GETDISPINFO = -520;
                TTN_NEEDTEXT = -520;
            }
        }
    }

    public static class TTS
    {
        public const int TTS_ALWAYSTIP = 1;
        public const int TTS_BALLOON = 0x40;
        public const int TTS_CLOSE = 0x80;
        public const int TTS_NOANIMATE = 0x10;
        public const int TTS_NOFADE = 0x20;
        public const int TTS_NOPREFIX = 2;
        public const int TTS_USEVISUALSTYLE = 0x100;
    }

    public static class WA
    {
        public const int WA_ACTIVE = 1;
        public const int WA_CLICKACTIVE = 2;
        public const int WA_INACTIVE = 0;
    }

    public sealed class WH
    {
        public const int WH_CALLWNDPROC = 4;
        public const int WH_CALLWNDPROCRET = 12;
        public const int WH_CBT = 5;
        public const int WH_DEBUG = 9;
        public const int WH_FOREGROUNDIDLE = 11;
        public const int WH_GETMESSAGE = 3;
        public const int WH_HARDWARE = 8;
        public const int WH_JOURNALPLAYBACK = 1;
        public const int WH_JOURNALRECORD = 0;
        public const int WH_KEYBOARD = 2;
        public const int WH_KEYBOARD_LL = 13;
        public const int WH_MOUSE = 7;
        public const int WH_MOUSE_LL = 14;
        public const int WH_MSGFILTER = -1;
        public const int WH_SHELL = 10;
        public const int WH_SYSMSGFILTER = 6;
    }

    public static class WM
    {
        public const int WM_ACTIVATE = 6;
        public const int WM_ACTIVATEAPP = 0x1c;
        public const int WM_CAPTURECHANGED = 0x215;
        public const int WM_CHANGEUISTATE = 0x127;
        public const int WM_CHAR = 0x102;
        public const int WM_CLOSE = 0x10;
        public const int WM_COMMAND = 0x111;
        public const int WM_CONTEXTMENU = 0x7b;
        public const int WM_CREATE = 1;
        public const int WM_CTLCOLOREDIT = 0x133;
        public const int WM_CTLCOLORSCROLLBAR = 0x137;
        public const int WM_DESTROY = 2;
        public const int WM_DISPLAYCHANGE = 0x7e;
        public const int WM_ENABLE = 10;
        public const int WM_ENTERIDLE = 0x121;
        public const int WM_ENTERMENULOOP = 0x211;
        public const int WM_ENTERSIZEMOVE = 0x231;
        public const int WM_ERASEBKGND = 20;
        public const int WM_EXITMENULOOP = 530;
        public const int WM_EXITSIZEMOVE = 0x232;
        public const int WM_GETDLGCODE = 0x87;
        public const int WM_GETICON = 0x7f;
        public const int WM_GETMINMAXINFO = 0x24;
        public const int WM_GETTEXT = 13;
        public const int WM_GETTEXTLENGTH = 14;
        public const int WM_HSCROLL = 0x114;
        public const int WM_INITMENU = 0x116;
        public const int WM_INITMENUPOPUP = 0x117;
        public const int WM_KEYDOWN = 0x100;
        public const int WM_KEYUP = 0x101;
        public const int WM_KILLFOCUS = 8;
        public const int WM_LBUTTONDBLCLK = 0x203;
        public const int WM_LBUTTONDOWN = 0x201;
        public const int WM_LBUTTONUP = 0x202;
        public const int WM_MBUTTONDBLCLK = 0x209;
        public const int WM_MBUTTONDOWN = 0x207;
        public const int WM_MBUTTONUP = 520;
        public const int WM_MDIACTIVATE = 0x222;
        public const int WM_MENUCHAR = 0x120;
        public const int WM_MENUCOMMAND = 0x126;
        public const int WM_MENUDRAG = 0x123;
        public const int WM_MENUGETOBJECT = 0x124;
        public const int WM_MENURBUTTONUP = 290;
        public const int WM_MENUSELECT = 0x11f;
        public const int WM_MOUSEACTIVATE = 0x21;
        public const int WM_MOUSEFIRST = 0x200;
        public const int WM_MOUSEHOVER = 0x2a1;
        public const int WM_MOUSELAST = 0x20d;
        public const int WM_MOUSELEAVE = 0x2a3;
        public const int WM_MOUSEMOVE = 0x200;
        public const int WM_MOUSEWHEEL = 0x20a;
        public const int WM_MOVE = 3;
        public const int WM_MOVING = 0x216;
        public const int WM_NCACTIVATE = 0x86;
        public const int WM_NCCALCSIZE = 0x83;
        public const int WM_NCCREATE = 0x81;
        public const int WM_NCDESTROY = 130;
        public const int WM_NCHITTEST = 0x84;
        public const int WM_NCLBUTTONDBLCLK = 0xa3;
        public const int WM_NCLBUTTONDOWN = 0xa1;
        public const int WM_NCLBUTTONUP = 0xa2;
        public const int WM_NCMBUTTONDBLCLK = 0xa9;
        public const int WM_NCMBUTTONDOWN = 0xa7;
        public const int WM_NCMBUTTONUP = 0xa8;
        public const int WM_NCMOUSEHOVER = 0x2a0;
        public const int WM_NCMOUSELEAVE = 0x2a2;
        public const int WM_NCMOUSEMOVE = 160;
        public const int WM_NCPAINT = 0x85;
        public const int WM_NCRBUTTONDBLCLK = 0xa6;
        public const int WM_NCRBUTTONDOWN = 0xa4;
        public const int WM_NCRBUTTONUP = 0xa5;
        public const int WM_NCUAHDRAWCAPTION = 0xae;
        public const int WM_NCUAHDRAWFRAME = 0xaf;
        public const int WM_NEXTMENU = 0x213;
        public const int WM_NOTIFY = 0x4e;
        public const int WM_NULL = 0;
        public const int WM_PAINT = 15;
        public const int WM_PARENTNOTIFY = 0x210;
        public const int WM_PASTE = 770;
        public const int WM_PRINT = 0x317;
        public const int WM_PRINTCLIENT = 0x318;
        public const int WM_QUERYUISTATE = 0x129;
        public const int WM_QUIT = 0x12;
        public const int WM_RBUTTONDBLCLK = 0x206;
        public const int WM_RBUTTONDOWN = 0x204;
        public const int WM_RBUTTONUP = 0x205;
        public const int WM_SETCURSOR = 0x20;
        public const int WM_SETFOCUS = 7;
        public const int WM_SETFONT = 0x30;
        public const int WM_SETICON = 0x80;
        public const int WM_SETREDRAW = 11;
        public const int WM_SETTEXT = 12;
        public const int WM_SHOWWINDOW = 0x18;
        public const int WM_SIZE = 5;
        public const int WM_SIZING = 0x214;
        public const int WM_STYLECHANGED = 0x7d;
        public const int WM_STYLECHANGING = 0x7c;
        public const int WM_SYNCPAINT = 0x88;
        public const int WM_SYSCOLORCHANGE = 0x15;
        public const int WM_SYSCOMMAND = 0x112;
        public const int WM_THEMECHANGED = 0x31a;
        public const int WM_TIMER = 0x113;
        public const int WM_UNINITMENUPOPUP = 0x125;
        public const int WM_UPDATEUISTATE = 0x128;
        public const int WM_VSCROLL = 0x115;
        public const int WM_WINDOWPOSCHANGED = 0x47;
        public const int WM_WINDOWPOSCHANGING = 70;
        public const int WM_DEVICECHANGE = 0x0219;
    }


    public class WVR
    {
        public const int WVR_ALIGNBOTTOM = 0x40;
        public const int WVR_ALIGNLEFT = 0x20;
        public const int WVR_ALIGNRIGHT = 0x80;
        public const int WVR_ALIGNTOP = 0x10;
        public const int WVR_HREDRAW = 0x100;
        public const int WVR_REDRAW = 0x300;
        public const int WVR_VALIDRECTS = 0x400;
        public const int WVR_VREDRAW = 0x200;
    }

    #region mouse_event

    public static class MOUSEEVENT
    {
        /// <summary>
        ///     移动鼠标
        /// </summary>
        public const int MOUSEEVENTF_MOVE = 0x0001;

        /// <summary>
        ///     按下鼠标左键
        /// </summary>
        public const int MOUSEEVENTF_LEFTDOWN = 0x0002;

        /// <summary>
        ///     释放鼠标左键
        /// </summary>
        public const int MOUSEEVENTF_LEFTUP = 0x0004;

        /// <summary>
        ///     按下鼠标右键
        /// </summary>
        public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;

        /// <summary>
        ///     释放鼠标右键
        /// </summary>
        public const int MOUSEEVENTF_RIGHTUP = 0x0010;

        /// <summary>
        ///     按下鼠标中键
        /// </summary>
        public const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;

        /// <summary>
        ///     释放鼠标中键
        /// </summary>
        public const int MOUSEEVENTF_MIDDLEUP = 0x0040;

        /// <summary>
        ///     标示是否采用绝对坐标
        /// </summary>
        public const int MOUSEEVENTF_ABSOLUTE = 0x8000;
    }

    #endregion

    #region keybd_event

    public static class KEYEVENT
    {
        /// <summary>
        ///     抬起按键
        /// </summary>
        public const int KEYEVENTF_KEYUP = 0x0002;

        /// <summary>
        ///     按下按键
        /// </summary>
        public const int KEYEVENTF_KEYDOWN = 0x0000;
    }

    #endregion

    #region OpenProcess

    public static class PROCESS
    {
        /// <summary>
        ///     所有可能的进程对象的访问权限
        /// </summary>
        public const int PROCESS_ALL_ACCESS = 0x1F0FFF;

        /// <summary>
        ///     需要在内存中读取进程应使用ReadProcessMemory
        /// </summary>
        public const int PROCESS_VM_READ = 0x0010;

        /// <summary>
        ///     需要在需要在内存中写入进程应使用WriteProcessMemory
        /// </summary>
        public const int PROCESS_VM_WRITE = 0x0020;
    }

    #endregion

    /// <summary>
    /// 窗体边框阴影效果变量申明
    /// </summary>
    /// 透明
    public delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    /// <summary>
    ///     Define the window class types
    /// </summary>
    public static class WndClassType
    {
        public const int CS_VREDRAW = 0x0001;
        public const int CS_HREDRAW = 0x0002;
        public const int CS_DBLCLKS = 0x0008;
        public const int CS_OWNDC = 0x0020;
        public const int CS_CLASSDC = 0x0040;
        public const int CS_PARENTDC = 0x0080;
        public const int CS_NOCLOSE = 0x0200;
        public const int CS_SAVEBITS = 0x0800;
        public const int CS_BYTEALIGNCLIENT = 0x1000;
        public const int CS_BYTEALIGNWINDOW = 0x2000;
        public const int CS_GLOBALCLASS = 0x4000;
        public const int CS_IME = 0x00010000;
        public const int CS_DROPSHADOW = 0x00020000;
    }

    public static class CS
    {
        public const int CS_BYTEALIGNCLIENT = 0x1000;
        public const int CS_BYTEALIGNWINDOW = 0x2000;
        public const int CS_CLASSDC = 0x40;
        public const int CS_DBLCLKS = 8;
        public const int CS_DROPSHADOW = 0x20000;
        public const int CS_GLOBALCLASS = 0x4000;
        public const int CS_HREDRAW = 2;
        public const int CS_IME = 0x10000;
        public const int CS_NOCLOSE = 0x200;
        public const int CS_OWNDC = 0x20;
        public const int CS_PARENTDC = 0x80;
        public const int CS_SAVEBITS = 0x800;
        public const int CS_VREDRAW = 1;
    }

    /// <summary>
    ///     Windows 消息列表
    /// </summary>
    public enum WindowsMessage:uint
    {
        /// <summary>
        /// </summary>
        WM_NULL = 0x0000,

        /// <summary>
        ///     应用程序创建一个窗口
        /// </summary>
        WM_CREATE = 0x0001,

        /// <summary>
        ///     一个窗口被销毁
        /// </summary>
        WM_DESTROY = 0x0002,

        /// <summary>
        ///     移动一个窗口
        /// </summary>
        WM_MOVE = 0x0003,

        /// <summary>
        ///     改变一个窗口的大小
        /// </summary>
        WM_SIZE = 0x0005,

        /// <summary>
        ///     一个窗口被激活或失去激活状态；
        /// </summary>
        WM_ACTIVATE = 0x0006,

        /// <summary>
        ///     获得焦点后
        /// </summary>
        WM_SETFOCUS = 0x0007,

        /// <summary>
        ///     失去焦点
        /// </summary>
        WM_KILLFOCUS = 0x0008,

        /// <summary>
        ///     改变enable状态
        /// </summary>
        WM_ENABLE = 0x000A,

        /// <summary>
        ///     设置窗口是否能重画
        /// </summary>
        WM_SETREDRAW = 0x000B,

        /// <summary>
        ///     应用程序发送此消息来设置一个窗口的文本
        /// </summary>
        WM_SETTEXT = 0x000C,

        /// <summary>
        ///     应用程序发送此消息来复制对应窗口的文本到缓冲区
        /// </summary>
        WM_GETTEXT = 0x000D,

        /// <summary>
        ///     得到与一个窗口有关的文本的长度（不包含空字符）
        /// </summary>
        WM_GETTEXTLENGTH = 0x000E,

        /// <summary>
        ///     要求一个窗口重画自己
        /// </summary>
        WM_PAINT = 0x000F,

        /// <summary>
        ///     当一个窗口或应用程序要关闭时发送一个信号
        /// </summary>
        WM_CLOSE = 0x0010,

        /// <summary>
        ///     当用户选择结束对话框或程序自己调用ExitWindows函数
        /// </summary>
        WM_QUERYENDSESSION = 0x0011,

        /// <summary>
        ///     用来结束程序运行或当程序调用postquitmessage函数
        /// </summary>
        WM_QUIT = 0x0012,

        /// <summary>
        ///     当用户窗口恢复以前的大小位置时，把此消息发送给某个图标
        /// </summary>
        WM_QUERYOPEN = 0x0013,

        /// <summary>
        ///     当窗口背景必须被擦除时（例在窗口改变大小时）
        /// </summary>
        WM_ERASEBKGND = 0x0014,

        /// <summary>
        ///     当系统颜色改变时，发送此消息给所有顶级窗口
        /// </summary>
        WM_SYSCOLORCHANGE = 0x0015,

        /// <summary>
        ///     当系统进程发出WM_QUERYENDSESSION消息后，此消息发送给应用程序，通知它对话是否结束
        /// </summary>
        WM_ENDSESSION = 0x0016,

        /// <summary>
        /// </summary>
        WM_SYSTEMERROR = 0x0017,

        /// <summary>
        ///     当隐藏或显示窗口是发送此消息给这个窗口
        /// </summary>
        WM_SHOWWINDOW = 0x0018,

        /// <summary>
        ///     发此消息给应用程序哪个窗口是激活的，哪个是非激活的；
        /// </summary>
        WM_ACTIVATEAPP = 0x001C,

        /// <summary>
        ///     当系统的字体资源库变化时发送此消息给所有顶级窗口
        /// </summary>
        WM_FONTCHANGE = 0x001D,

        /// <summary>
        ///     当系统的时间变化时发送此消息给所有顶级窗口
        /// </summary>
        WM_TIMECHANGE = 0x001E,

        /// <summary>
        ///     发送此消息来取消某种正在进行的摸态（操作）
        /// </summary>
        WM_CANCELMODE = 0x001F,

        /// <summary>
        ///     如果鼠标引起光标在某个窗口中移动且鼠标输入没有被捕获时，就发消息给某个窗口
        /// </summary>
        WM_SETCURSOR = 0x0020,

        /// <summary>
        ///     当光标在某个非激活的窗口中而用户正按着鼠标的某个键发送此消息给当前窗口
        /// </summary>
        WM_MOUSEACTIVATE = 0x0021,

        /// <summary>
        ///     发送此消息给MDI子窗口当用户点击此窗口的标题栏，或当窗口被激活，移动，改变大小
        /// </summary>
        WM_CHILDACTIVATE = 0x0022,

        /// <summary>
        ///     此消息由基于计算机的训练程序发送，通过WH_JOURNALPALYBACK的hook程序分离出用户输入消息
        /// </summary>
        WM_QUEUESYNC = 0x0023,

        /// <summary>
        ///     此消息发送给窗口当它将要改变大小或位置；
        /// </summary>
        WM_GETMINMAXINFO = 0x24,

        /// <summary>
        ///     发送给最小化窗口当它图标将要被重画
        /// </summary>
        WM_PAINTICON = 0x0026,

        /// <summary>
        ///     此消息发送给某个最小化窗口，仅当它在画图标前它的背景必须被重画
        /// </summary>
        WM_ICONERASEBKGND = 0x0027,

        /// <summary>
        ///     发送此消息给一个对话框程序去更改焦点位置
        /// </summary>
        WM_NEXTDLGCTL = 0x0028,

        /// <summary>
        ///     每当打印管理列队增加或减少一条作业时发出此消息
        /// </summary>
        WM_SPOOLERSTATUS = 0x002A,

        /// <summary>
        ///     当button，combobox，listbox，menu的可视外观改变时发送此消息给这些空件的所有者
        /// </summary>
        WM_DRAWITEM = 0x002B,

        /// <summary>
        ///     当button, combo box, list box, list view control, or menu item 被创建时
        ///     发送此消息给控件的所有者
        /// </summary>
        WM_MEASUREITEM = 0x002C,

        /// <summary>
        ///     当the list box 或 combo box 被销毁 或 当某些项被删除通过LB_DELETESTRING, LB_RESETCONTENT,
        ///     CB_DELETESTRING, or CB_RESETCONTENT 消息
        /// </summary>
        WM_DELETEITEM = 0x002D,

        /// <summary>
        ///     此消息有一个LBS_WANTKEYBOARDINPUT风格的发出给它的所有者来响应WM_KEYDOWN消息
        /// </summary>
        WM_VKEYTOITEM = 0x002E,

        /// <summary>
        ///     此消息由一个LBS_WANTKEYBOARDINPUT风格的列表框发送给他的所有者来响应WM_CHAR消息
        /// </summary>
        WM_CHARTOITEM = 0x002F,

        /// <summary>
        ///     当绘制文本时程序发送此消息得到控件要用的颜色
        /// </summary>
        WM_SETFONT = 0x0030,

        /// <summary>
        ///     应用程序发送此消息得到当前控件绘制文本的字体
        /// </summary>
        WM_GETFONT = 0x0031,

        /// <summary>
        ///     应用程序发送此消息让一个窗口与一个热键相关连
        /// </summary>
        WM_SETHOTKEY = 0x0032,

        /// <summary>
        ///     应用程序发送此消息来判断热键与某个窗口是否有关联
        /// </summary>
        WM_GETHOTKEY = 0x0033,

        /// <summary>
        ///     此消息发送给最小化窗口，当此窗口将要被拖放而它的类中没有定义图标，应用程序能
        ///     返回一个图标或光标的句柄，当用户拖放图标时系统显示这个图标或光标
        /// </summary>
        WM_QUERYDRAGICON = 0x0037,

        /// <summary>
        ///     发送此消息来判定combobox或listbox新增加的项的相对位置
        /// </summary>
        WM_COMPAREITEM = 0x0039,

        /// <summary>
        /// </summary>
        WM_GETOBJECT = 0x003D,

        /// <summary>
        ///     显示内存已经很少了
        /// </summary>
        WM_COMPACTING = 0x0041,

        /// <summary>
        ///     发送此消息给那个窗口的大小和位置将要被改变时，来调用setwindowpos函数或其它窗口管理函数
        /// </summary>
        WM_WINDOWPOSCHANGING = 0x0046,

        /// <summary>
        ///     发送此消息给那个窗口的大小和位置已经被改变时，来调用setwindowpos函数或其它窗口管理函数
        /// </summary>
        WM_WINDOWPOSCHANGED = 0x0047,

        /// <summary>
        ///     当系统将要进入暂停状态时发送此消息 （适用于16位的windows）
        /// </summary>
        WM_POWER = 0x0048,

        /// <summary>
        ///     当一个应用程序传递数据给另一个应用程序时发送此消息
        /// </summary>
        WM_COPYDATA = 0x004A,

        /// <summary>
        ///     当某个用户取消程序日志激活状态，提交此消息给程序
        /// </summary>
        WM_CANCELJOURNAL = 0x004B,

        /// <summary>
        ///     当某个控件的某个事件已经发生或这个控件需要得到一些信息时，发送此消息给它的父窗口
        /// </summary>
        WM_NOTIFY = 0x004E,

        /// <summary>
        ///     当用户选择某种输入语言，或输入语言的热键改变
        /// </summary>
        WM_INPUTLANGCHANGEREQUEST = 0x0050,

        /// <summary>
        ///     当平台现场已经被改变后发送此消息给受影响的最顶级窗口
        /// </summary>
        WM_INPUTLANGCHANGE = 0x0051,

        /// <summary>
        ///     当程序已经初始化windows帮助例程时发送此消息给应用程序
        /// </summary>
        WM_TCARD = 0x0052,

        /// <summary>
        ///     此消息显示用户按下了F1，如果某个菜单是激活的，就发送此消息个此窗口关联的菜单，否则就
        ///     发送给有焦点的窗口，如果当前都没有焦点，就把此消息发送给当前激活的窗口
        /// </summary>
        WM_HELP = 0x0053,

        /// <summary>
        ///     当用户已经登入或退出后发送此消息给所有的窗口，当用户登入或退出时系统更新用户的具体
        ///     设置信息，在用户更新设置时系统马上发送此消息；
        /// </summary>
        WM_USERCHANGED = 0x0054,

        /// <summary>
        ///     公用控件，自定义控件和他们的父窗口通过此消息来判断控件是使用ANSI还是UNI CODE结构
        ///     在WM_NOTIFY消息，使用此控件能使某个控件与它的父控件之间进行相互通信
        /// </summary>
        WM_NOTIFYformAT = 0x0055,

        /// <summary>
        ///     当用户某个窗口中点击了一下右键就发送此消息给这个窗口
        /// </summary>
        WM_CONTEXTMENU = 0x007B,

        /// <summary>
        ///     当调用SETWINDOWLONG函数将要改变一个或多个 窗口的风格时发送此消息给那个窗口
        /// </summary>
        WM_styleCHANGING = 0x007C,

        /// <summary>
        ///     当调用SETWINDOWLONG函数一个或多个 窗口的风格后发送此消息给那个窗口
        /// </summary>
        WM_STYLECHANGED = 0x007D,

        /// <summary>
        ///     当显示器的分辨率改变后发送此消息给所有的窗口
        /// </summary>
        WM_DISPLAYCHANGE = 0x007E,

        /// <summary>
        ///     此消息发送给某个窗口来返回与某个窗口有关连的大图标或小图标的句柄；
        /// </summary>
        WM_GETICON = 0x007F,

        /// <summary>
        ///     程序发送此消息让一个新的大图标或小图标与某个窗口关联；
        /// </summary>
        WM_SETICON = 0x0080,

        /// <summary>
        ///     当某个窗口第一次被创建时，此消息在WM_CREATE消息发送前发送；
        /// </summary>
        WM_NCCREATE = 0x0081,

        /// <summary>
        ///     此消息通知某个窗口，非客户区正在销毁
        /// </summary>
        WM_NCDESTROY = 0x0082,

        /// <summary>
        ///     当某个窗口的客户区域必须被核算时发送此消息
        /// </summary>
        WM_NCCALCSIZE = 0x0083,

        /// <summary>
        ///     移动鼠标，按住或释放鼠标时发生
        /// </summary>
        WM_NCHITTEST = 0x0084,

        /// <summary>
        ///     程序发送此消息给某个窗口当它（窗口）的框架必须被绘制时；
        /// </summary>
        WM_NCPAINT = 0x0085,

        /// <summary>
        ///     此消息发送给某个窗口 仅当它的非客户区需要被改变来显示是激活还是非激活状态；
        /// </summary>
        WM_NCACTIVATE = 0x0086,

        /// <summary>
        ///     发送此消息给某个与对话框程序关联的控件，widdows控制方位键和TAB键使输入进入此控件
        ///     通过响应WM_GETDLGCODE消息，应用程序可以把他当成一个特殊的输入控件并能处理它
        /// </summary>
        WM_GETDLGCODE = 0x0087,

        /// <summary>
        ///     当光标在一个窗口的非客户区内移动时发送此消息给这个窗口(非客户区 为：窗体的标题栏及窗的边框体)
        /// </summary>
        WM_NCMOUSEMOVE = 0x00A0,

        /// <summary>
        ///     当光标在一个窗口的非客户区同时按下鼠标左键时提交此消息
        /// </summary>
        WM_NCLBUTTONDOWN = 0x00A1,

        /// <summary>
        ///     当用户释放鼠标左键同时光标某个窗口在非客户区十发送此消息；
        /// </summary>
        WM_NCLBUTTONUP = 0x00A2,

        /// <summary>
        ///     当用户双击鼠标左键同时光标某个窗口在非客户区十发送此消息
        /// </summary>
        WM_NCLBUTTONDBLCLK = 0x00A3,

        /// <summary>
        ///     当用户按下鼠标右键同时光标又在窗口的非客户区时发送此消息
        /// </summary>
        WM_NCRBUTTONDOWN = 0x00A4,

        /// <summary>
        ///     当用户释放鼠标右键同时光标又在窗口的非客户区时发送此消息
        /// </summary>
        WM_NCRBUTTONUP = 0x00A5,

        /// <summary>
        ///     当用户双击鼠标右键同时光标某个窗口在非客户区十发送此消息
        /// </summary>
        WM_NCRBUTTONDBLCLK = 0x00A6,

        /// <summary>
        ///     当用户按下鼠标中键同时光标又在窗口的非客户区时发送此消息
        /// </summary>
        WM_NCMBUTTONDOWN = 0x00A7,

        /// <summary>
        ///     当用户释放鼠标中键同时光标又在窗口的非客户区时发送此消息
        /// </summary>
        WM_NCMBUTTONUP = 0x00A8,

        /// <summary>
        ///     当用户双击鼠标中键同时光标又在窗口的非客户区时发送此消息
        /// </summary>
        WM_NCMBUTTONDBLCLK = 0x00A9,

        /// <summary>
        /// </summary>
        WM_KEYFIRST = 0x0100,

        /// <summary>
        ///     按下一个键
        /// </summary>
        WM_KEYDOWN = 0x0100,

        /// <summary>
        ///     释放一个键
        /// </summary>
        WM_KEYUP = 0x0101,

        /// <summary>
        ///     按下某键，并已发出WM_KEYDOWN， WM_KEYUP消息
        /// </summary>
        WM_CHAR = 0x0102,

        /// <summary>
        ///     当用translatemessage函数翻译WM_KEYUP消息时发送此消息给拥有焦点的窗口
        /// </summary>
        WM_DEADCHAR = 0x0103,

        /// <summary>
        ///     当用户按住ALT键同时按下其它键时提交此消息给拥有焦点的窗口；
        /// </summary>
        WM_SYSKEYDOWN = 0x0104,

        /// <summary>
        ///     当用户释放一个键同时ALT 键还按着时提交此消息给拥有焦点的窗口
        /// </summary>
        WM_SYSKEYUP = 0x0105,

        /// <summary>
        ///     当WM_SYSKEYDOWN消息被TRANSLATEMESSAGE函数翻译后提交此消息给拥有焦点的窗口
        /// </summary>
        WM_SYSCHAR = 0x0106,

        /// <summary>
        ///     当WM_SYSKEYDOWN消息被TRANSLATEMESSAGE函数翻译后发送此消息给拥有焦点的窗口
        /// </summary>
        WM_SYSDEADCHAR = 0x0107,

        /// <summary>
        /// </summary>
        WM_KEYLAST = 0x0108,

        /// <summary>
        ///     在一个对话框程序被显示前发送此消息给它，通常用此消息初始化控件和执行其它任务
        /// </summary>
        WM_INITDIALOG = 0x0110,

        /// <summary>
        ///     当用户选择一条菜单命令项或当某个控件发送一条消息给它的父窗口，一个快捷键被翻译
        /// </summary>
        WM_COMMAND = 0x0111,

        /// <summary>
        ///     当用户选择窗口菜单的一条命令或当用户选择最大化或最小化时那个窗口会收到此消息
        /// </summary>
        WM_SYSCOMMAND = 0x0112,

        /// <summary>
        ///     发生了定时器事件
        /// </summary>
        WM_TIMER = 0x0113,

        /// <summary>
        ///     当一个窗口标准水平滚动条产生一个滚动事件时发送此消息给那个窗口，也发送给拥有它的控件
        /// </summary>
        WM_HSCROLL = 0x0114,

        /// <summary>
        ///     当一个窗口标准垂直滚动条产生一个滚动事件时发送此消息给那个窗口，发送给拥有它的控件
        /// </summary>
        WM_VSCROLL = 0x0115,

        /// <summary>
        ///     当一个菜单将要被激活时发送此消息，它发生在用户菜单条中的某项或按下某个菜单键，它允许程序在显示前更改菜单
        /// </summary>
        wm_initmenu = 0x0116,

        /// <summary>
        ///     当一个下拉菜单或子菜单将要被激活时发送此消息，它允许程序在它显示前更改菜单，而不要改变全部
        /// </summary>
        WM_INITMENUPOPUP = 0x0117,

        /// <summary>
        ///     当用户选择一条菜单项时发送此消息给菜单的所有者（一般是窗口）
        /// </summary>
        WM_MENUSELECT = 0x011F,

        /// <summary>
        ///     当菜单已被激活用户按下了某个键（不同于加速键），发送此消息给菜单的所有者；
        /// </summary>
        WM_MENUCHAR = 0x0120,

        /// <summary>
        ///     当一个模态对话框或菜单进入空载状态时发送此消息给它的所有者，一个模态对话框或菜单进入空载
        ///     状态就是在处理完一条或几条先前的消息后没有消息它的列队中等待
        /// </summary>
        WM_ENTERIDLE = 0x0121,

        /// <summary>
        /// </summary>
        WM_MENURBUTTONUP = 0x0122,

        /// <summary>
        /// </summary>
        WM_MENUDRAG = 0x0123,

        /// <summary>
        /// </summary>
        WM_MENUGETOBJECT = 0x0124,

        /// <summary>
        /// </summary>
        WM_UNINITMENUPOPUP = 0x0125,

        /// <summary>
        /// </summary>
        WM_MENUCOMMAND = 0x0126,

        /// <summary>
        /// </summary>
        WM_CHANGEUISTATE = 0x0127,

        /// <summary>
        /// </summary>
        WM_UPDATEUISTATE = 0x0128,

        /// <summary>
        /// </summary>
        WM_QUERYUISTATE = 0x0129,

        /// <summary>
        ///     在windows绘制消息框前发送此消息给消息框的所有者窗口，通过响应这条消息， 所有者窗口可以
        ///     通过使用给定的相关显示设备的句柄来设置消息框的文本和背景颜色
        /// </summary>
        WM_CTLCOLORMSGBOX = 0x0132,

        /// <summary>
        ///     当一个编辑型控件将要被绘制时发送此消息给它的父窗口；通过响应这条消息，所有者窗口可以
        ///     通过使用给定的相关显示设备的句柄来设置编辑框的文本和背景颜色
        /// </summary>
        WM_CTLCOLOREDIT = 0x0133,

        /// <summary>
        ///     当一个列表框控件将要被绘制前发送此消息给它的父窗口；通过响应这条消息，所有者窗口可以
        ///     通过使用给定的相关显示设备的句柄来设置列表框的文本和背景颜色
        /// </summary>
        WM_CTLCOLORLISTBOX = 0x0134,

        /// <summary>
        ///     当一个按钮控件将要被绘制时发送此消息给它的父窗口；通过响应这条消息，所有者窗口可以
        ///     通过使用给定的相关显示设备的句柄来设置按纽的文本和背景颜色
        /// </summary>
        WM_CTLCOLORBTN = 0x0135,

        /// <summary>
        ///     当一个对话框控件将要被绘制前发送此消息给它的父窗口；通过响应这条消息，所有者窗口可以
        ///     通过使用给定的相关显示设备的句柄来设置对话框的文本背景颜色
        /// </summary>
        WM_CTLCOLORDLG = 0x0136,

        /// <summary>
        ///     当一个滚动条控件将要被绘制时发送此消息给它的父窗口；通过响应这条消息，所有者窗口可以
        ///     通过使用给定的相关显示设备的句柄来设置滚动条的背景颜色
        /// </summary>
        WM_CTLCOLORSCROLLBAR = 0x0137,

        /// <summary>
        ///     当一个静态控件将要被绘制时发送此消息给它的父窗口；通过响应这条消息，所有者窗口可以
        ///     通过使用给定的相关显示设备的句柄来设置静态控件的文本和背景颜色
        /// </summary>
        WM_CTLCOLORSTATIC = 0x0138,

        /// <summary>
        /// </summary>
        WM_MOUSEFIRST = 0x0200,

        /// <summary>
        ///     移动鼠标
        /// </summary>
        WM_MOUSEMOVE = 0x0200,

        /// <summary>
        ///     按下鼠标左键
        /// </summary>
        WM_LBUTTONDOWN = 0x0201,

        /// <summary>
        ///     释放鼠标左键
        /// </summary>
        WM_LBUTTONUP = 0x0202,

        /// <summary>
        ///     双击鼠标左键
        /// </summary>
        WM_LBUTTONDBLCLK = 0x0203,

        /// <summary>
        ///     按下鼠标右键
        /// </summary>
        WM_RBUTTONDOWN = 0x0204,

        /// <summary>
        ///     释放鼠标右键
        /// </summary>
        WM_RBUTTONUP = 0x0205,

        /// <summary>
        ///     双击鼠标右键
        /// </summary>
        WM_RBUTTONDBLCLK = 0x0206,

        /// <summary>
        ///     按下鼠标中键
        /// </summary>
        WM_MBUTTONDOWN = 0x0207,

        /// <summary>
        ///     释放鼠标中键
        /// </summary>
        WM_MBUTTONUP = 0x0208,

        /// <summary>
        ///     双击鼠标中键
        /// </summary>
        WM_MBUTTONDBLCLK = 0x0209,

        /// <summary>
        ///     当鼠标轮子转动时发送此消息给当前有焦点的控件
        /// </summary>
        WM_MOUSEWHEEL = 0x020A,

        /// <summary>
        /// </summary>
        WM_MOUSELAST = 0x020A,

        /// <summary>
        ///     当MDI子窗口被创建或被销毁，或用户按了一下鼠标键而光标在子窗口上时发送此消息给它的父窗口
        /// </summary>
        WM_PARENTNOTIFY = 0x0210,

        /// <summary>
        ///     发送此消息通知应用程序的主窗口that已经进入了菜单循环模式
        /// </summary>
        WM_ENTERMENULOOP = 0x0211,

        /// <summary>
        ///     发送此消息通知应用程序的主窗口that已退出了菜单循环模式
        /// </summary>
        WM_EXITMENULOOP = 0x0212,

        /// <summary>
        /// </summary>
        WM_NEXTMENU = 0x0213,

        /// <summary>
        ///     当用户正在调整窗口大小时发送此消息给窗口；通过此消息应用程序可以监视窗口大小和位置也可以修改他们
        /// </summary>
        WM_SIZING = 0x0214,

        /// <summary>
        ///     发送此消息 给窗口当它失去捕获的鼠标时；
        /// </summary>
        WM_CAPTURECHANGED = 0x0215,

        /// <summary>
        ///     当用户在移动窗口时发送此消息，通过此消息应用程序可以监视窗口大小和位置也可以修改他们；
        /// </summary>
        WM_MOVING = 0x0216,

        /// <summary>
        ///     此消息发送给应用程序来通知它有关电源管理事件；
        /// </summary>
        WM_POWERBROADCAST = 0x0218,

        /// <summary>
        ///     当设备的硬件配置改变时发送此消息给应用程序或设备驱动程序
        /// </summary>
        WM_DEVICECHANGE = 0x0219,

        /// <summary>
        /// </summary>
        WM_IME_STARTCOMPOSITION = 0x010D,

        /// <summary>
        /// </summary>
        WM_IME_ENDCOMPOSITION = 0x010E,

        /// <summary>
        /// </summary>
        WM_IME_COMPOSITION = 0x010F,

        /// <summary>
        /// </summary>
        WM_IME_KEYLAST = 0x010F,

        /// <summary>
        /// </summary>
        WM_IME_SETCONTEXT = 0x0281,

        /// <summary>
        /// </summary>
        WM_IME_NOTIFY = 0x0282,

        /// <summary>
        /// </summary>
        WM_IME_CONTROL = 0x0283,

        /// <summary>
        /// </summary>
        WM_IME_COMPOSITIONFULL = 0x0284,

        /// <summary>
        /// </summary>
        WM_IME_SELECT = 0x0285,

        /// <summary>
        /// </summary>
        WM_IME_CHAR = 0x0286,

        /// <summary>
        /// </summary>
        WM_IME_REQUEST = 0x0288,

        /// <summary>
        /// </summary>
        WM_IME_KEYDOWN = 0x0290,

        /// <summary>
        /// </summary>
        WM_IME_KEYUP = 0x0291,

        /// <summary>
        ///     应用程序发送此消息给多文档的客户窗口来创建一个MDI 子窗口
        /// </summary>
        WM_MDICREATE = 0x0220,

        /// <summary>
        ///     应用程序发送此消息给多文档的客户窗口来关闭一个MDI 子窗口
        /// </summary>
        WM_MDIDESTROY = 0x0221,

        /// <summary>
        ///     应用程序发送此消息给多文档的客户窗口通知客户窗口激活另一个MDI子窗口，当客户窗口收到
        ///     此消息后，它发出WM_MDIACTIVE消息给MDI子窗口（未激活）激活它；
        /// </summary>
        WM_MDIACTIVATE = 0x0222,

        /// <summary>
        ///     程序 发送此消息给MDI客户窗口让子窗口从最大最小化恢复到原来大小
        /// </summary>
        WM_MDIRESTORE = 0x0223,

        /// <summary>
        ///     程序 发送此消息给MDI客户窗口激活下一个或前一个窗口
        /// </summary>
        WM_MDINEXT = 0x0224,

        /// <summary>
        ///     程序发送此消息给MDI客户窗口来最大化一个MDI子窗口；
        /// </summary>
        WM_MDIMAXIMIZE = 0x0225,

        /// <summary>
        ///     程序 发送此消息给MDI客户窗口以平铺方式重新排列所有MDI子窗口
        /// </summary>
        WM_MDITILE = 0x0226,

        /// <summary>
        ///     程序 发送此消息给MDI客户窗口以层叠方式重新排列所有MDI子窗口
        /// </summary>
        WM_MDICASCADE = 0x0227,

        /// <summary>
        ///     程序 发送此消息给MDI客户窗口重新排列所有最小化的MDI子窗口
        /// </summary>
        WM_MDIICONARRANGE = 0x0228,

        /// <summary>
        ///     程序 发送此消息给MDI客户窗口来找到激活的子窗口的句柄
        /// </summary>
        WM_MDIGETACTIVE = 0x0229,

        /// <summary>
        ///     程序 发送此消息给MDI客户窗口用MDI菜单代替子窗口的菜单
        /// </summary>
        WM_MDISETMENU = 0x0230,

        /// <summary>
        /// </summary>
        WM_ENTERSIZEMOVE = 0x0231,

        /// <summary>
        /// </summary>
        WM_EXITSIZEMOVE = 0x0232,

        /// <summary>
        /// </summary>
        WM_DROPFILES = 0x0233,

        /// <summary>
        /// </summary>
        WM_MDIREFRESHMENU = 0x0234,

        /// <summary>
        /// </summary>
        WM_MOUSEHOVER = 0x02A1,

        /// <summary>
        /// </summary>
        WM_MOUSELEAVE = 0x02A3,

        /// <summary>
        ///     程序发送此消息给一个编辑框或combobox来删除当前选择的文本
        /// </summary>
        WM_CUT = 0x0300,

        /// <summary>
        ///     程序发送此消息给一个编辑框或combobox来复制当前选择的文本到剪贴板
        /// </summary>
        WM_COPY = 0x0301,

        /// <summary>
        ///     程序发送此消息给editcontrol或combobox从剪贴板中得到数据
        /// </summary>
        WM_PASTE = 0x0302,

        /// <summary>
        ///     程序发送此消息给editcontrol或combobox清除当前选择的内容；
        /// </summary>
        WM_CLEAR = 0x0303,

        /// <summary>
        ///     程序发送此消息给editcontrol或combobox撤消最后一次操作
        /// </summary>
        WM_UNDO = 0x0304,

        /// <summary>
        /// </summary>
        WM_RENDERformAT = 0x0305,

        /// <summary>
        /// </summary>
        WM_RENDERALLformATS = 0x0306,

        /// <summary>
        ///     当调用ENPTYCLIPBOARD函数时 发送此消息给剪贴板的所有者
        /// </summary>
        WM_DESTROYCLIPBOARD = 0x0307,

        /// <summary>
        ///     当剪贴板的内容变化时发送此消息给剪贴板观察链的第一个窗口；它允许用剪贴板观察窗口来显示剪贴板的新内容；
        /// </summary>
        WM_DRAWCLIPBOARD = 0x0308,

        /// <summary>
        ///     当剪贴板包含CF_OWNERDIPLAY格式的数据并且剪贴板观察窗口的客户区需要重画；
        /// </summary>
        WM_PAINTCLIPBOARD = 0x0309,

        /// <summary>
        /// </summary>
        WM_VSCROLLCLIPBOARD = 0x030A,

        /// <summary>
        ///     当剪贴板包含CF_OWNERDIPLAY格式的数据并且剪贴板观察窗口的客户区域的大小已经改变是此消息通过剪
        ///     贴板观察窗口发送给剪贴板的所有者；
        /// </summary>
        WM_SIZECLIPBOARD = 0x030B,

        /// <summary>
        ///     通过剪贴板观察窗口发送此消息给剪贴板的所有者来请求一个CF_OWNERDISPLAY格式的剪贴板的名字
        /// </summary>
        WM_ASKCBformATNAME = 0x030C,

        /// <summary>
        ///     当一个窗口从剪贴板观察链中移去时发送此消息给剪贴板观察链的第一个窗口；
        /// </summary>
        WM_CHANGECBCHAIN = 0x030D,

        /// <summary>
        ///     此消息通过一个剪贴板观察窗口发送给剪贴板的所有者 ；它发生在当剪贴板包含CFOWNERDISPALY格式的数据
        ///     并且有个事件在剪贴板观察窗的水平滚动条上；所有者应滚动剪贴板图象并更新滚动条的值；
        /// </summary>
        WM_HSCROLLCLIPBOARD = 0x030E,

        /// <summary>
        ///     此消息发送给将要收到焦点的窗口，此消息能使窗口在收到焦点时同时有机会实现他的逻辑调色板
        /// </summary>
        WM_QUERYNEWPALETTE = 0x030F,

        /// <summary>
        ///     当一个应用程序正要实现它的逻辑调色板时发此消息通知所有的应用程序
        /// </summary>
        WM_PALETTEISCHANGING = 0x0310,

        /// <summary>
        ///     此消息在一个拥有焦点的窗口实现它的逻辑调色板后发送此消息给所有顶级并重叠的窗口，以此来改变系统调色板
        /// </summary>
        WM_PALETTECHANGED = 0x0311,

        /// <summary>
        ///     当用户按下由REGISTERHOTKEY函数注册的热键时提交此消息
        /// </summary>
        WM_HOTKEY = 0x0312,

        /// <summary>
        ///     应用程序发送此消息仅当WINDOWS或其它应用程序发出一个请求要求绘制一个应用程序的一部分；
        /// </summary>
        WM_PRINT = 0x0317,

        /// <summary>
        /// </summary>
        WM_PRINTCLIENT = 0x0318,

        /// <summary>
        /// </summary>
        WM_HANDHELDFIRST = 0x0358,

        /// <summary>
        /// </summary>
        WM_HANDHELDLAST = 0x035F,

        /// <summary>
        /// </summary>
        WM_PENWINFIRST = 0x0380,

        /// <summary>
        /// </summary>
        WM_PENWINLAST = 0x038F,

        /// <summary>
        /// </summary>
        WM_COALESCE_FIRST = 0x0390,

        /// <summary>
        /// </summary>
        WM_COALESCE_LAST = 0x039F,

        /// <summary>
        /// </summary>
        WM_DDE_FIRST = 0x03E0,

        /// <summary>
        /// </summary>
        WM_THEMECHNAGED = 0x31A,
        //WM_NULL = 0x00,
        //    WM_CREATE = 0x01,
        //    WM_DESTROY = 0x02,
        //    WM_MOVE = 0x03,
        //    WM_SIZE = 0x05,
        //    WM_ACTIVATE = 0x06,
        //    WM_SETFOCUS = 0x07,
        //    WM_KILLFOCUS = 0x08,
        //    WM_ENABLE = 0x0A,
        //    WM_SETREDRAW = 0x0B,
        //    WM_SETTEXT = 0x0C,
        //    WM_GETTEXT = 0x0D,
        //    WM_GETTEXTLENGTH = 0x0E,
        //    WM_PAINT = 0x0F,
        //    WM_CLOSE = 0x10,
        //    WM_QUERYENDSESSION = 0x11,
        //    WM_QUIT = 0x12,
        //    WM_QUERYOPEN = 0x13,
        //    WM_ERASEBKGND = 0x14,
        //    WM_SYSCOLORCHANGE = 0x15,
        //    WM_ENDSESSION = 0x16,
        //    WM_SYSTEMERROR = 0x17,
        //    WM_SHOWWINDOW = 0x18,
        WM_CTLCOLOR = 0x19,
        WM_WININICHANGE = 0x1A,
        WM_SETTINGCHANGE = 0x1A,
        WM_DEVMODECHANGE = 0x1B,
        //WM_ACTIVATEAPP = 0x1C,
        //WM_FONTCHANGE = 0x1D,
        //WM_TIMECHANGE = 0x1E,
        //WM_CANCELMODE = 0x1F,
        //WM_SETCURSOR = 0x20,
        //WM_MOUSEACTIVATE = 0x21,
        //WM_CHILDACTIVATE = 0x22,
        //WM_QUEUESYNC = 0x23,
        //WM_GETMINMAXINFO = 0x24,
        //WM_PAINTICON = 0x26,
        //WM_ICONERASEBKGND = 0x27,
        //WM_NEXTDLGCTL = 0x28,
        //WM_SPOOLERSTATUS = 0x2A,
        //WM_DRAWITEM = 0x2B,
        //WM_MEASUREITEM = 0x2C,
        //WM_DELETEITEM = 0x2D,
        //WM_VKEYTOITEM = 0x2E,
        //WM_CHARTOITEM = 0x2F,

        //WM_SETFONT = 0x30,
        //WM_GETFONT = 0x31,
        //WM_SETHOTKEY = 0x32,
        //WM_GETHOTKEY = 0x33,
        //WM_QUERYDRAGICON = 0x37,
        //WM_COMPAREITEM = 0x39,
        //WM_COMPACTING = 0x41,
        //WM_WINDOWPOSCHANGING = 0x46,
        //WM_WINDOWPOSCHANGED = 0x47,
        //WM_POWER = 0x48,
        //WM_COPYDATA = 0x4A,
        //WM_CANCELJOURNAL = 0x4B,
        //WM_NOTIFY = 0x4E,
        //WM_INPUTLANGCHANGEREQUEST = 0x50,
        //WM_INPUTLANGCHANGE = 0x51,
        //WM_TCARD = 0x52,
        //WM_HELP = 0x53,
        //WM_USERCHANGED = 0x54,
        WM_NOTIFYFORMAT = 0x55,
        //WM_CONTEXTMENU = 0x7B,
        WM_STYLECHANGING = 0x7C,
        //WM_STYLECHANGED = 0x7D,
        //WM_DISPLAYCHANGE = 0x7E,
        //WM_GETICON = 0x7F,
        //WM_SETICON = 0x80,

        //WM_NCCREATE = 0x81,
        //WM_NCDESTROY = 0x82,
        //WM_NCCALCSIZE = 0x83,
        //WM_NCHITTEST = 0x84,
        //WM_NCPAINT = 0x85,
        //WM_NCACTIVATE = 0x86,
        //WM_GETDLGCODE = 0x87,
        //WM_NCMOUSEMOVE = 0xA0,
        //WM_NCLBUTTONDOWN = 0xA1,
        //WM_NCLBUTTONUP = 0xA2,
        //WM_NCLBUTTONDBLCLK = 0xA3,
        //WM_NCRBUTTONDOWN = 0xA4,
        //WM_NCRBUTTONUP = 0xA5,
        //WM_NCRBUTTONDBLCLK = 0xA6,
        //WM_NCMBUTTONDOWN = 0xA7,
        //WM_NCMBUTTONUP = 0xA8,
        //WM_NCMBUTTONDBLCLK = 0xA9,

        //WM_KEYFIRST = 0x100,
        //WM_KEYDOWN = 0x100,
        //WM_KEYUP = 0x101,
        //WM_CHAR = 0x102,
        //WM_DEADCHAR = 0x103,
        //WM_SYSKEYDOWN = 0x104,
        //WM_SYSKEYUP = 0x105,
        //WM_SYSCHAR = 0x106,
        //WM_SYSDEADCHAR = 0x107,
        //WM_KEYLAST = 0x108,

        //WM_IME_STARTCOMPOSITION = 0x10D,
        //WM_IME_ENDCOMPOSITION = 0x10E,
        //WM_IME_COMPOSITION = 0x10F,
        //WM_IME_KEYLAST = 0x10F,

        //WM_INITDIALOG = 0x110,
        //WM_COMMAND = 0x111,
        //WM_SYSCOMMAND = 0x112,
        //WM_TIMER = 0x113,
        //WM_HSCROLL = 0x114,
        //WM_VSCROLL = 0x115,
        WM_INITMENU = 0x116,
        //WM_INITMENUPOPUP = 0x117,
        //WM_MENUSELECT = 0x11F,
        //WM_MENUCHAR = 0x120,
        //WM_ENTERIDLE = 0x121,

        //WM_CTLCOLORMSGBOX = 0x132,
        //WM_CTLCOLOREDIT = 0x133,
        //WM_CTLCOLORLISTBOX = 0x134,
        //WM_CTLCOLORBTN = 0x135,
        //WM_CTLCOLORDLG = 0x136,
        //WM_CTLCOLORSCROLLBAR = 0x137,
        //WM_CTLCOLORSTATIC = 0x138,

        //WM_MOUSEFIRST = 0x200,
        //WM_MOUSEMOVE = 0x200,
        //WM_LBUTTONDOWN = 0x201,
        //WM_LBUTTONUP = 0x202,
        //WM_LBUTTONDBLCLK = 0x203,
        //WM_RBUTTONDOWN = 0x204,
        //WM_RBUTTONUP = 0x205,
        //WM_RBUTTONDBLCLK = 0x206,
        //WM_MBUTTONDOWN = 0x207,
        //WM_MBUTTONUP = 0x208,
        //WM_MBUTTONDBLCLK = 0x209,
        //WM_MOUSEWHEEL = 0x20A,
        WM_MOUSEHWHEEL = 0x20E,

        //WM_PARENTNOTIFY = 0x210,
        //WM_ENTERMENULOOP = 0x211,
        //WM_EXITMENULOOP = 0x212,
        //WM_NEXTMENU = 0x213,
        //WM_SIZING = 0x214,
        //WM_CAPTURECHANGED = 0x215,
        //WM_MOVING = 0x216,
        //WM_POWERBROADCAST = 0x218,
        //WM_DEVICECHANGE = 0x219,

        //WM_MDICREATE = 0x220,
        //WM_MDIDESTROY = 0x221,
        //WM_MDIACTIVATE = 0x222,
        //WM_MDIRESTORE = 0x223,
        //WM_MDINEXT = 0x224,
        //WM_MDIMAXIMIZE = 0x225,
        //WM_MDITILE = 0x226,
        //WM_MDICASCADE = 0x227,
        //WM_MDIICONARRANGE = 0x228,
        //WM_MDIGETACTIVE = 0x229,
        //WM_MDISETMENU = 0x230,
        //WM_ENTERSIZEMOVE = 0x231,
        //WM_EXITSIZEMOVE = 0x232,
        //WM_DROPFILES = 0x233,
        //WM_MDIREFRESHMENU = 0x234,

        //WM_IME_SETCONTEXT = 0x281,
        //WM_IME_NOTIFY = 0x282,
        //WM_IME_CONTROL = 0x283,
        //WM_IME_COMPOSITIONFULL = 0x284,
        //WM_IME_SELECT = 0x285,
        //WM_IME_CHAR = 0x286,
        //WM_IME_KEYDOWN = 0x290,
        //WM_IME_KEYUP = 0x291,

        //WM_MOUSEHOVER = 0x2A1,
        WM_NCMOUSELEAVE = 0x2A2,
        //WM_MOUSELEAVE = 0x2A3,

        //WM_CUT = 0x300,
        //WM_COPY = 0x301,
        //WM_PASTE = 0x302,
        //WM_CLEAR = 0x303,
        //WM_UNDO = 0x304,

        WM_RENDERFORMAT = 0x305,
        WM_RENDERALLFORMATS = 0x306,
        //WM_DESTROYCLIPBOARD = 0x307,
        //WM_DRAWCLIPBOARD = 0x308,
        //WM_PAINTCLIPBOARD = 0x309,
        //WM_VSCROLLCLIPBOARD = 0x30A,
        //WM_SIZECLIPBOARD = 0x30B,
        WM_ASKCBFORMATNAME = 0x30C,
        //WM_CHANGECBCHAIN = 0x30D,
        //WM_HSCROLLCLIPBOARD = 0x30E,
        //WM_QUERYNEWPALETTE = 0x30F,
        //WM_PALETTEISCHANGING = 0x310,
        //WM_PALETTECHANGED = 0x311,

        //WM_HOTKEY = 0x312,
        //WM_PRINT = 0x317,
        //WM_PRINTCLIENT = 0x318,

        //WM_HANDHELDFIRST = 0x358,
        //WM_HANDHELDLAST = 0x35F,
        //WM_PENWINFIRST = 0x380,
        //WM_PENWINLAST = 0x38F,
        //WM_COALESCE_FIRST = 0x390,
        //WM_COALESCE_LAST = 0x39F,
        //WM_DDE_FIRST = 0x3E0,
        WM_DDE_INITIATE = 0x3E0,
        WM_DDE_TERMINATE = 0x3E1,
        WM_DDE_ADVISE = 0x3E2,
        WM_DDE_UNADVISE = 0x3E3,
        WM_DDE_ACK = 0x3E4,
        WM_DDE_DATA = 0x3E5,
        WM_DDE_REQUEST = 0x3E6,
        WM_DDE_POKE = 0x3E7,
        WM_DDE_EXECUTE = 0x3E8,
        WM_DDE_LAST = 0x3E8,

        WM_USER = 0x400,
        WM_APP = 0x8000
    }

    public static class UpdateLayerWindowParameter
    {
        public const int ULW_COLORKEY = 0x00000001;
        public const int ULW_ALPHA = 0x00000002;
        public const int ULW_OPAQUE = 0x00000004;
        public const int ULW_EX_NORESIZE = 0x00000008;
    }


    /// <summary>
    ///     Windows 窗口样式
    /// </summary>
    [Flags]
    public enum WindowStyle
    {
        WS_EX_CLIENTEDGE = 0x00000200,

        /// <summary>
        /// </summary>
        WS_OVERLAPPED = 0x00000000,

        /// <summary>
        /// </summary>
        WS_POPUP = unchecked((int) 0x80000000),

        /// <summary>
        /// </summary>
        WS_CHILD = 0x40000000,

        /// <summary>
        /// </summary>
        WS_MINIMIZE = 0x20000000,

        /// <summary>
        /// </summary>
        WS_VISIBLE = 0x10000000,

        /// <summary>
        /// </summary>
        WS_DISABLED = 0x08000000,

        /// <summary>
        /// </summary>
        WS_CLIPSIBLINGS = 0x04000000,

        /// <summary>
        /// </summary>
        WS_CLIPCHILDREN = 0x02000000,

        /// <summary>
        /// </summary>
        WS_MAXIMIZE = 0x01000000,

        /// <summary>
        /// </summary>
        WS_CAPTION = 0x00C00000,

        /// <summary>
        /// </summary>
        WS_BORDER = 0x00800000,

        /// <summary>
        /// </summary>
        WS_DLGFRAME = 0x00400000,

        /// <summary>
        /// </summary>
        WS_VSCROLL = 0x00200000,

        /// <summary>
        /// </summary>
        WS_HSCROLL = 0x00100000,

        /// <summary>
        /// </summary>
        WS_SYSMENU = 0x00080000,

        /// <summary>
        /// </summary>
        WS_THICKFRAME = 0x00040000,

        /// <summary>
        /// </summary>
        WS_GROUP = 0x00020000,

        /// <summary>
        /// </summary>
        WS_TABSTOP = 0x00010000,

        /// <summary>
        /// </summary>
        WS_MINIMIZEBOX = 0x00020000,

        /// <summary>
        /// </summary>
        WS_MAXIMIZEBOX = 0x00010000,

        /// <summary>
        /// </summary>
        WS_TILED = WS_OVERLAPPED,

        /// <summary>
        /// </summary>
        WS_ICONIC = WS_MINIMIZE,

        /// <summary>
        /// </summary>
        WS_SIZEBOX = WS_THICKFRAME,

        /// <summary>
        /// </summary>
        WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW,

        /// <summary>
        /// </summary>
        WS_OVERLAPPEDWINDOW = (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU |
                               WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX),

        /// <summary>
        /// </summary>
        WS_POPUPWINDOW = (WS_POPUP | WS_BORDER | WS_SYSMENU),

        /// <summary>
        /// </summary>
        WS_CHILDWINDOW = (WS_CHILD)
    }

    public enum WindowShowStyle : uint
    {
        /// <summary>Hides the window and activates another window.</summary>
        /// <remarks>See SW_HIDE</remarks>
        Hide = 0,

        /// <summary>
        ///     Activates and displays a window. If the window is minimized
        ///     or maximized, the system restores it to its original size and
        ///     position. An application should specify this flag when displaying
        ///     the window for the first time.
        /// </summary>
        /// <remarks>See SW_SHOWNORMAL</remarks>
        ShowNormal = 1,

        /// <summary>Activates the window and displays it as a minimized window.</summary>
        /// <remarks>See SW_SHOWMINIMIZED</remarks>
        ShowMinimized = 2,

        /// <summary>Activates the window and displays it as a maximized window.</summary>
        /// <remarks>See SW_SHOWMAXIMIZED</remarks>
        ShowMaximized = 3,

        /// <summary>Maximizes the specified window.</summary>
        /// <remarks>See SW_MAXIMIZE</remarks>
        Maximize = 3,

        /// <summary>
        ///     Displays a window in its most recent size and position.
        ///     This value is similar to "ShowNormal", except the window is not
        ///     actived.
        /// </summary>
        /// <remarks>See SW_SHOWNOACTIVATE</remarks>
        ShowNormalNoActivate = 4,

        /// <summary>
        ///     Activates the window and displays it in its current size
        ///     and position.
        /// </summary>
        /// <remarks>See SW_SHOW</remarks>
        Show = 5,

        /// <summary>
        ///     Minimizes the specified window and activates the next
        ///     top-level window in the Z order.
        /// </summary>
        /// <remarks>See SW_MINIMIZE</remarks>
        Minimize = 6,

        /// <summary>
        ///     Displays the window as a minimized window. This value is
        ///     similar to "ShowMinimized", except the window is not activated.
        /// </summary>
        /// <remarks>See SW_SHOWMINNOACTIVE</remarks>
        ShowMinNoActivate = 7,

        /// <summary>
        ///     Displays the window in its current size and position. This
        ///     value is similar to "Show", except the window is not activated.
        /// </summary>
        /// <remarks>See SW_SHOWNA</remarks>
        ShowNoActivate = 8,

        /// <summary>
        ///     Activates and displays the window. If the window is
        ///     minimized or maximized, the system restores it to its original size
        ///     and position. An application should specify this flag when restoring
        ///     a minimized window.
        /// </summary>
        /// <remarks>See SW_RESTORE</remarks>
        Restore = 9,

        /// <summary>
        ///     Sets the show state based on the SW_ value specified in the
        ///     STARTUPINFO structure passed to the CreateProcess function by the
        ///     program that started the application.
        /// </summary>
        /// <remarks>See SW_SHOWDEFAULT</remarks>
        ShowDefault = 10,

        /// <summary>
        ///     Windows 2000/XP: Minimizes a window, even if the thread
        ///     that owns the window is hung. This flag should only be used when
        ///     minimizing windows from a different thread.
        /// </summary>
        /// <remarks>See SW_FORCEMINIMIZE</remarks>
        ForceMinimized = 11
    }


    /// <summary>
    ///     定义了辅助键的名称（将数字转变为字符以便于记忆，也可去除此枚举使用数值）
    /// </summary>
    [Flags]
    public enum KeyModifiers
    {
        /// <summary>
        /// </summary>
        None = 0,

        /// <summary>
        /// </summary>
        Alt = 1,

        /// <summary>
        /// </summary>
        Ctrl = 2,

        /// <summary>
        /// </summary>
        Shift = 4,

        /// <summary>
        /// </summary>
        WindowsKey = 8
    }

    /// <summary>
    /// </summary>
    [Flags]
    public enum ImageListDrawFlags
    {
        /// <summary>
        /// </summary>
        ILD_NORMAL = 0x00000000,

        /// <summary>
        /// </summary>
        ILD_TRANSPARENT = 0x00000001,

        /// <summary>
        /// </summary>
        ILD_BLEND25 = 0x00000002,

        /// <summary>
        /// </summary>
        ILD_FOCUS = 0x00000002,

        /// <summary>
        /// </summary>
        ILD_BLEND50 = 0x00000004,

        /// <summary>
        /// </summary>
        ILD_SELECTED = 0x00000004,

        /// <summary>
        /// </summary>
        ILD_BLEND = 0x00000004,

        /// <summary>
        /// </summary>
        ILD_MASK = 0x00000010,

        /// <summary>
        /// </summary>
        ILD_IMAGE = 0x00000020,

        /// <summary>
        /// </summary>
        ILD_ROP = 0x00000040,

        /// <summary>
        /// </summary>
        ILD_OVERLAYMASK = 0x00000F00,

        /// <summary>
        /// </summary>
        ILD_PRESERVEALPHA = 0x00001000,

        /// <summary>
        /// </summary>
        ILD_SCALE = 0x00002000,

        /// <summary>
        /// </summary>
        ILD_DPISCALE = 0x00004000,

        /// <summary>
        /// </summary>
        ILD_ASYNC = 0x00008000
    }

    /// <summary>
    /// </summary>
    public enum ImageListColorFlags : uint
    {
        /// <summary>
        /// </summary>
        CLR_NONE = 0xFFFFFFFF,

        /// <summary>
        /// </summary>
        CLR_DEFAULT = 0xFF000000,

        /// <summary>
        /// </summary>
        CLR_HILIGHT = CLR_DEFAULT
    }

    /// <summary>
    ///     挂钩处理过程的类型
    /// </summary>
    public enum HookType
    {
        /// <summary>
        ///     安装一个挂钩处理过程, 以监视由对话框、消息框、菜单条、或滚动条中的输入事件引发的消息
        /// </summary>
        WH_MSGFILTER = -1,

        /// <summary>
        ///     安装一个挂钩处理过程,对寄送至系统消息队列的输入消息进行纪录
        /// </summary>
        WH_JOURNALRECORD = 0,

        /// <summary>
        ///     安装一个挂钩处理过程,对此前由WH_JOURNALRECORD 挂钩处理过程纪录的消息进行寄送
        /// </summary>
        WH_JOURNALPLAYBACK = 1,

        /// <summary>
        ///     安装一个挂钩处理过程对击键消息进行监视
        /// </summary>
        WH_KEYBORARD = 2,

        /// <summary>
        ///     安装一个挂钩处理过程对寄送至消息队列的消息进行监视
        /// </summary>
        WH_GETMESSAGE = 3,

        /// <summary>
        ///     安装一个挂钩处理过程,在系统将消息发送至目标窗口处理过程之前,对该消息进行监视
        /// </summary>
        WH_CALLWNDPROC = 4,

        /// <summary>
        ///     安装一个挂钩处理过程,接受对CBT应用程序有用的消息
        /// </summary>
        WH_CBT = 5,

        /// <summary>
        ///     <para>安装一个挂钩处理过程,以监视由对话框、消息框、菜单条、或滚动条中</para>
        ///     <para>的输入事件引发的消息.这个挂钩处理过程对系统中所有应用程序的这类</para>
        ///     <para>消息都进行监视</para>
        /// </summary>
        WH_SYSMSGFILTER = 6,

        /// <summary>
        ///     安装一个挂钩处理过程,对鼠标消息进行监视
        /// </summary>
        WH_MOUSE = 7,

        /// <summary>
        ///     安装一个挂钩处理过程以便对其他挂钩处理过程进行调试
        /// </summary>
        WH_DEBUG = 9,

        /// <summary>
        ///     安装一个挂钩处理过程以接受对外壳应用程序有用的通知
        /// </summary>
        WH_SHELL = 10,

        /// <summary>
        ///     <para>安装一个挂钩处理过程,该挂钩处理过程当应用程序的前台线程即将</para>
        ///     <para>进入空闲状态时被调用,它有助于在空闲时间内执行低优先级的任务</para>
        /// </summary>
        WH_FOREGROUNDIDLE = 11,

        /// <summary>
        ///     安装一个挂钩处理过程,它对已被目标窗口处理过程处理过了的消息进行监视
        /// </summary>
        WH_CALLWNDPROCRET = 12,

        /// <summary>
        ///     此挂钩只能在Windows NT中被安装,用来对底层的键盘输入事件进行监视
        /// </summary>
        WH_KEYBORARD_LL = 13,

        /// <summary>
        ///     此挂钩只能在Windows NT中被安装,用来对底层的鼠标输入事件进行监视
        /// </summary>
        WH_MOUSE_LL = 14
    }

    /// <summary>
    /// </summary>
    public enum SHGFI
    {
        /// <summary>
        ///     获取图标
        /// </summary>
        SHGFI_ICON = 0x000000100,

        /// <summary>
        ///     获取显示名
        /// </summary>
        SHGFI_DISPLAYNAM = 0x000000200,

        /// <summary>
        ///     获取类型名
        /// </summary>
        SHGFI_TYPENAME = 0x000000400,

        /// <summary>
        ///     获取属性
        /// </summary>
        SHGFI_ATTRIBUTES = 0x000000800,

        /// <summary>
        ///     获取图标位置
        /// </summary>
        SHGFI_ICONLOCATION = 0x000001000,

        /// <summary>
        ///     返回可执行文件的类型
        /// </summary>
        SHGFI_EXETYPE = 0x000002000,

        /// <summary>
        ///     获取系统图标索引
        /// </summary>
        SHGFI_SYSICONINDEX = 0x000004000,

        /// <summary>
        ///     把一个链接覆盖在图标
        /// </summary>
        SHGFI_LINKOVERLAY = 0x000008000,

        /// <summary>
        ///     显示图标在选中时的状态
        /// </summary>
        SHGFI_SELECTED = 0x000010000,

        /// <summary>
        ///     只能指定属性
        /// </summary>
        SHGFI_ATTR_SPECIFIED = 0x000020000,

        /// <summary>
        ///     获取大图标
        /// </summary>
        SHGFI_LARGEICON = 0x000000000,

        /// <summary>
        ///     获取小图标
        /// </summary>
        SHGFI_SMALLICON = 0x000000001,

        /// <summary>
        ///     修改SHGFI_ICON,导致函数来检索文件的打开图标
        /// </summary>
        SHGFI_OPENICON = 0x000000002,

        /// <summary>
        ///     修改SHGFI_ICON,导致函数来检索一个shell大小的图标。如果这个标志没有指定函数大小图标根据系统度量值。
        ///     注意：这个标志不支持Windows手机设备
        /// </summary>
        SHGFI_SHELLICONSIZE = 0x000000004,

        /// <summary>
        /// </summary>
        SHGFI_PIDL = 0x000000008,

        /// <summary>
        ///     通过使用dwFileAttributes
        /// </summary>
        SHGFI_USEFILEATTRIBUTES = 0x000000010,

        /// <summary>
        ///     应用适当的覆盖
        /// </summary>
        SHGFI_ADDOVERLAYS = 0x000000020,

        /// <summary>
        ///     获得该指数的叠加
        /// </summary>
        SHGFI_OVERLAYINDEX = 0x000000040
    }

    /// <summary>
    ///     发送到一个窗口，以确定鼠标在窗口的哪一部分，对应于一个特定的屏幕坐标
    /// </summary>
    public enum WM_NCHITTEST
    {
        /// <summary>
        ///     在屏幕背景或窗口之间的分界线
        /// </summary>
        HTERROR = -2,

        /// <summary>
        ///     在目前一个窗口，其他窗口覆盖在同一个线程
        ///     （该消息将被发送到相关窗口在同一个线程，直到其中一个返回一个代码，是不是HTTRANSPARENT）
        /// </summary>
        HTTRANSPARENT = -1,

        /// <summary>
        ///     在屏幕背景或窗口之间的分界线上
        /// </summary>
        HTNOWHERE = 0,

        /// <summary>
        ///     在客户端区域
        /// </summary>
        HTCLIENT = 1,

        /// <summary>
        ///     在标题栏
        /// </summary>
        HTCAPTION = 2,

        /// <summary>
        ///     在窗口菜单中，或在一个子窗口的关闭按钮
        /// </summary>
        HTSYSMENU = 3,

        /// <summary>
        ///     在大小框（与HTGROWBO相同）
        /// </summary>
        HTSIZE = 4,

        /// <summary>
        ///     在大小框（与HTSIZE相同）
        /// </summary>
        HTGROWBOX = 4,

        /// <summary>
        ///     在一个菜单
        /// </summary>
        HTMENU = 5,

        /// <summary>
        ///     在水平滚动条
        /// </summary>
        HTHSCROLL = 6,

        /// <summary>
        ///     在垂直滚动条
        /// </summary>
        HTVSCROLL = 7,

        /// <summary>
        ///     在最小化按钮
        /// </summary>
        HTREDUCE = 8,

        /// <summary>
        ///     在最小化按钮
        /// </summary>
        HTMINBUTTON = 8,

        /// <summary>
        ///     在最大化按钮
        /// </summary>
        HTMAXBUTTON = 9,

        /// <summary>
        ///     在最大化按钮
        /// </summary>
        HTZOOM = 9,

        /// <summary>
        ///     在左边框可调整大小的窗口
        /// </summary>
        HTLEFT = 10,

        /// <summary>
        ///     在一个可调整大小的窗口的右边框
        /// </summary>
        HTRIGHT = 11,

        /// <summary>
        ///     在窗口的上边框水平线上
        /// </summary>
        HTTOP = 12,

        /// <summary>
        ///     在窗口的左上边框
        /// </summary>
        HTTOPLEFT = 13,

        /// <summary>
        ///     在窗口的右上边框
        /// </summary>
        HTTOPRIGHT = 14,

        /// <summary>
        ///     （用户可以在较低的水平边界可调整大小的窗口单击鼠标，改变窗口的垂直大小）
        /// </summary>
        HTBOTTOM = 15,

        /// <summary>
        ///     在左下角的边框可调整大小的窗口（用户可以通过点击鼠标来调整窗口的大小，对角）
        /// </summary>
        HTBOTTOMLEFT = 16,

        /// <summary>
        ///     在右下角的边框可调整大小的窗口（用户可以通过点击鼠标来调整窗口的大小，对角）
        /// </summary>
        HTBOTTOMRIGHT = 17,

        /// <summary>
        ///     在一个不具有缩放边框的窗口
        /// </summary>
        HTBORDER = 18,

        /// <summary>
        ///     在关闭按钮
        /// </summary>
        HTCLOSE = 20,

        /// <summary>
        ///     在帮助按钮
        /// </summary>
        HTHELP = 21
    }

    /// <summary>
    ///     Windows 使用的256个虚拟键码
    /// </summary>
    public enum KEYS
    {
        /// <summary>
        /// </summary>
        VK_LBUTTON = 0x1,

        /// <summary>
        /// </summary>
        VK_RBUTTON = 0x2,

        /// <summary>
        /// </summary>
        VK_CANCEL = 0x3,

        /// <summary>
        /// </summary>
        VK_MBUTTON = 0x4,

        /// <summary>
        /// </summary>
        VK_BACK = 0x8,

        /// <summary>
        /// </summary>
        VK_TAB = 0x9,

        /// <summary>
        /// </summary>
        VK_CLEAR = 0xC,

        /// <summary>
        /// </summary>
        VK_RETURN = 0xD,

        /// <summary>
        /// </summary>
        VK_SHIFT = 0x10,

        /// <summary>
        /// </summary>
        VK_CONTROL = 0x11,

        /// <summary>
        /// </summary>
        VK_MENU = 0x12,

        /// <summary>
        /// </summary>
        VK_PAUSE = 0x13,

        /// <summary>
        ///     当窗口背景必须被擦除（例如，当一个窗口被调整大小）WM_ERASEBKGND消息被发送。该消息被发送到准备画一个窗口的无效部分。
        /// </summary>
        VK_CAPITAL = 0x14,

        /// <summary>
        /// </summary>
        VK_ESCAPE = 0x1B,

        /// <summary>
        /// </summary>
        VK_SPACE = 0x20,

        /// <summary>
        /// </summary>
        VK_PRIOR = 0x21,

        /// <summary>
        /// </summary>
        VK_NEXT = 0x22,

        /// <summary>
        /// </summary>
        VK_END = 0x23,

        /// <summary>
        /// </summary>
        VK_HOME = 0x24,

        /// <summary>
        /// </summary>
        VK_LEFT = 0x25,

        /// <summary>
        /// </summary>
        VK_UP = 0x26,

        /// <summary>
        /// </summary>
        VK_RIGHT = 0x27,

        /// <summary>
        /// </summary>
        VK_DOWN = 0x28,

        /// <summary>
        /// </summary>
        VK_Select = 0x29,

        /// <summary>
        /// </summary>
        VK_PRINT = 0x2A,

        /// <summary>
        /// </summary>
        VK_EXECUTE = 0x2B,

        /// <summary>
        /// </summary>
        VK_SNAPSHOT = 0x2C,

        /// <summary>
        /// </summary>
        VK_Insert = 0x2D,

        /// <summary>
        /// </summary>
        VK_Delete = 0x2E,

        /// <summary>
        /// </summary>
        VK_HELP = 0x2F,

        /// <summary>
        /// </summary>
        VK_0 = 0x30,

        /// <summary>
        /// </summary>
        VK_1 = 0x31,

        /// <summary>
        /// </summary>
        VK_2 = 0x32,

        /// <summary>
        /// </summary>
        VK_3 = 0x33,

        /// <summary>
        /// </summary>
        VK_4 = 0x34,

        /// <summary>
        /// </summary>
        VK_5 = 0x35,

        /// <summary>
        /// </summary>
        VK_6 = 0x36,

        /// <summary>
        /// </summary>
        VK_7 = 0x37,

        /// <summary>
        /// </summary>
        VK_8 = 0x38,

        /// <summary>
        /// </summary>
        VK_9 = 0x39,

        /// <summary>
        /// </summary>
        VK_A = 0x41,

        /// <summary>
        /// </summary>
        VK_B = 0x42,

        /// <summary>
        /// </summary>
        VK_C = 0x43,

        /// <summary>
        /// </summary>
        VK_D = 0x44,

        /// <summary>
        /// </summary>
        VK_E = 0x45,

        /// <summary>
        /// </summary>
        VK_F = 0x46,

        /// <summary>
        /// </summary>
        VK_G = 0x47,

        /// <summary>
        /// </summary>
        VK_H = 0x48,

        /// <summary>
        /// </summary>
        VK_I = 0x49,

        /// <summary>
        /// </summary>
        VK_J = 0x4A,

        /// <summary>
        /// </summary>
        VK_K = 0x4B,

        /// <summary>
        /// </summary>
        VK_L = 0x4C,

        /// <summary>
        /// </summary>
        VK_M = 0x4D,

        /// <summary>
        /// </summary>
        VK_N = 0x4E,

        /// <summary>
        /// </summary>
        VK_O = 0x4F,

        /// <summary>
        /// </summary>
        VK_P = 0x50,

        /// <summary>
        /// </summary>
        VK_Q = 0x51,

        /// <summary>
        /// </summary>
        VK_R = 0x52,

        /// <summary>
        /// </summary>
        VK_S = 0x53,

        /// <summary>
        /// </summary>
        VK_T = 0x54,

        /// <summary>
        /// </summary>
        VK_U = 0x55,

        /// <summary>
        /// </summary>
        VK_V = 0x56,

        /// <summary>
        /// </summary>
        VK_W = 0x57,

        /// <summary>
        /// </summary>
        VK_X = 0x58,

        /// <summary>
        /// </summary>
        VK_Y = 0x59,

        /// <summary>
        /// </summary>
        VK_Z = 0x5A,

        /// <summary>
        /// </summary>
        VK_STARTKEY = 0x5B,

        /// <summary>
        /// </summary>
        VK_CONTEXTKEY = 0x5D,

        /// <summary>
        /// </summary>
        VK_NUMPAD0 = 0x60,

        /// <summary>
        /// </summary>
        VK_NUMPAD1 = 0x61,

        /// <summary>
        /// </summary>
        VK_NUMPAD2 = 0x62,

        /// <summary>
        /// </summary>
        VK_NUMPAD3 = 0x63,

        /// <summary>
        /// </summary>
        VK_NUMPAD4 = 0x64,

        /// <summary>
        /// </summary>
        VK_NUMPAD5 = 0x65,

        /// <summary>
        /// </summary>
        VK_NUMPAD6 = 0x66,

        /// <summary>
        /// </summary>
        VK_NUMPAD7 = 0x67,

        /// <summary>
        /// </summary>
        VK_NUMPAD8 = 0x68,

        /// <summary>
        /// </summary>
        VK_NUMPAD9 = 0x69,

        /// <summary>
        /// </summary>
        VK_MULTIPLY = 0x6A,

        /// <summary>
        /// </summary>
        VK_ADD = 0x6B,

        /// <summary>
        /// </summary>
        VK_SEPARATOR = 0x6C,

        /// <summary>
        /// </summary>
        VK_SUBTRACT = 0x6D,

        /// <summary>
        /// </summary>
        VK_DECIMAL = 0x6E,

        /// <summary>
        /// </summary>
        VK_DIVIDE = 0x6F,

        /// <summary>
        /// </summary>
        VK_F1 = 0x70,

        /// <summary>
        /// </summary>
        VK_F2 = 0x71,

        /// <summary>
        /// </summary>
        VK_F3 = 0x72,

        /// <summary>
        /// </summary>
        VK_F4 = 0x73,

        /// <summary>
        /// </summary>
        VK_F5 = 0x74,

        /// <summary>
        /// </summary>
        VK_F6 = 0x75,

        /// <summary>
        /// </summary>
        VK_F7 = 0x76,

        /// <summary>
        /// </summary>
        VK_F8 = 0x77,

        /// <summary>
        /// </summary>
        VK_F9 = 0x78,

        /// <summary>
        /// </summary>
        VK_F10 = 0x79,

        /// <summary>
        /// </summary>
        VK_F11 = 0x7A,

        /// <summary>
        /// </summary>
        VK_F12 = 0x7B,

        /// <summary>
        /// </summary>
        VK_F13 = 0x7C,

        /// <summary>
        /// </summary>
        VK_F14 = 0x7D,

        /// <summary>
        /// </summary>
        VK_F15 = 0x7E,

        /// <summary>
        /// </summary>
        VK_F16 = 0x7F,

        /// <summary>
        /// </summary>
        VK_F17 = 0x80,

        /// <summary>
        /// </summary>
        VK_F18 = 0x81,

        /// <summary>
        /// </summary>
        VK_F19 = 0x82,

        /// <summary>
        /// </summary>
        VK_F20 = 0x83,

        /// <summary>
        /// </summary>
        VK_F21 = 0x84,

        /// <summary>
        ///     在WM_NCPAINT消息被发送到时候它的框架必须被画了一个窗口。
        /// </summary>
        VK_F22 = 0x85,

        /// <summary>
        /// </summary>
        VK_F23 = 0x86,

        /// <summary>
        /// </summary>
        VK_F24 = 0x87,

        /// <summary>
        /// </summary>
        VK_NUMLOCK = 0x90,

        /// <summary>
        /// </summary>
        VK_OEM_SCROLL = 0x91,

        /// <summary>
        /// </summary>
        VK_OEM_1 = 0xBA,

        /// <summary>
        /// </summary>
        VK_OEM_PLUS = 0xBB,

        /// <summary>
        /// </summary>
        VK_OEM_COMMA = 0xBC,

        /// <summary>
        /// </summary>
        VK_OEM_MINUS = 0xBD,

        /// <summary>
        /// </summary>
        VK_OEM_PERIOD = 0xBE,

        /// <summary>
        /// </summary>
        VK_OEM_2 = 0xBF,

        /// <summary>
        /// </summary>
        VK_OEM_3 = 0xC0,

        /// <summary>
        /// </summary>
        VK_OEM_4 = 0xDB,

        /// <summary>
        /// </summary>
        VK_OEM_5 = 0xDC,

        /// <summary>
        /// </summary>
        VK_OEM_6 = 0xDD,

        /// <summary>
        /// </summary>
        VK_OEM_7 = 0xDE,

        /// <summary>
        /// </summary>
        VK_OEM_8 = 0xDF,

        /// <summary>
        /// </summary>
        VK_ICO_F17 = 0xE0,

        /// <summary>
        /// </summary>
        VK_ICO_F18 = 0xE1,

        /// <summary>
        /// </summary>
        VK_OEM102 = 0xE2,

        /// <summary>
        /// </summary>
        VK_ICO_HELP = 0xE3,

        /// <summary>
        /// </summary>
        VK_ICO_00 = 0xE4,

        /// <summary>
        /// </summary>
        VK_ICO_CLEAR = 0xE6,

        /// <summary>
        /// </summary>
        VK_OEM_RESET = 0xE9,

        /// <summary>
        /// </summary>
        VK_OEM_JUMP = 0xEA,

        /// <summary>
        /// </summary>
        VK_OEM_PA1 = 0xEB,

        /// <summary>
        /// </summary>
        VK_OEM_PA2 = 0xEC,

        /// <summary>
        /// </summary>
        VK_OEM_PA3 = 0xED,

        /// <summary>
        /// </summary>
        VK_OEM_WSCTRL = 0xEE,

        /// <summary>
        /// </summary>
        VK_OEM_CUSEL = 0xEF,

        /// <summary>
        /// </summary>
        VK_OEM_ATTN = 0xF0,

        /// <summary>
        /// </summary>
        VK_OEM_FINNISH = 0xF1,

        /// <summary>
        /// </summary>
        VK_OEM_COPY = 0xF2,

        /// <summary>
        /// </summary>
        VK_OEM_AUTO = 0xF3,

        /// <summary>
        /// </summary>
        VK_OEM_ENLW = 0xF4,

        /// <summary>
        /// </summary>
        VK_OEM_BACKTAB = 0xF5,

        /// <summary>
        /// </summary>
        VK_ATTN = 0xF6,

        /// <summary>
        /// </summary>
        VK_CRSEL = 0xF7,

        /// <summary>
        /// </summary>
        VK_EXSEL = 0xF8,

        /// <summary>
        /// </summary>
        VK_EREOF = 0xF9,

        /// <summary>
        /// </summary>
        VK_PLAY = 0xFA,

        /// <summary>
        /// </summary>
        VK_ZOOM = 0xFB,

        /// <summary>
        /// </summary>
        VK_NONAME = 0xFC,

        /// <summary>
        /// </summary>
        VK_PA1 = 0xFD,

        /// <summary>
        /// </summary>
        VK_OEM_CLEAR = 0xFE
    }

    /// <summary>
    /// </summary>
    public enum SS
    {
        SS_OWNERDRAW = 0x0000000D,
        SS_ETCHEDHORZ = 0x00000010,
        SS_ETCHEDVERT = 0x00000011
    }

    public static class LWA
    {
        public const int LWA_COLORKEY = 0x1;
        public const int LWA_ALPHA = 0x2;
    }

    public static class WS_EX
    {
        public const int WS_EX_DLGMODALFRAME = 0x00000001;
        public const int WS_EX_NOPARENTNOTIFY = 0x00000004;
        public const int WS_EX_TOPMOST = 0x00000008;
        public const int WS_EX_ACCEPTFILES = 0x00000010;
        public const int WS_EX_TRANSPARENT = 0x00000020;
        public const int WS_EX_MDICHILD = 0x00000040;
        public const int WS_EX_TOOLWINDOW = 0x00000080;
        public const int WS_EX_WINDOWEDGE = 0x00000100;
        public const int WS_EX_CLIENTEDGE = 0x00000200;
        public const int WS_EX_CONTEXTHELP = 0x00000400;
        public const int WS_EX_RIGHT = 0x00001000;
        public const int WS_EX_LEFT = 0x00000000;
        public const int WS_EX_RTLREADING = 0x00002000;
        public const int WS_EX_LTRREADING = 0x00000000;
        public const int WS_EX_LEFTSCROLLBAR = 0x00004000;
        public const int WS_EX_RIGHTSCROLLBAR = 0x00000000;
        public const int WS_EX_CONTROLPARENT = 0x00010000;
        public const int WS_EX_STATICEDGE = 0x00020000;
        public const int WS_EX_APPWINDOW = 0x00040000;
        public const int WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE);
        public const int WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST);
        public const int WS_EX_LAYERED = 0x00080000;
        public const int WS_EX_NOINHERITLAYOUT = 0x00100000; // Disable inheritence of mirroring by children
        public const int WS_EX_LAYOUTRTL = 0x00400000; // Right to left mirroring
        public const int WS_EX_COMPOSITED = 0x02000000;
        public const int WS_EX_NOACTIVATE = 0x08000000;
    }


    public class SW
    {
        public const int SW_HIDE = 0; //隐藏窗口并激活其他窗口
        public const int SW_SHOWNORMAL = 1; //激活并显示一个窗口。如果窗口被最小化或最大化，系统将其恢复到原来的尺寸和大小。应用程序在第一次显示窗口的时候应该指定此标志。
        public const int SW_SHOWMINIMIZED = 2; //激活窗口并将其最小化
        public const int SW_SHOWMAXIMIZED = 3; //激活窗口并将其最大化。
        public const int SW_SHOWNOACTIVATE = 4; //以窗口最近一次的大小和状态显示窗口。激活窗口仍然维持激活状态。
        public const int SW_SHOW = 5; //在窗口原来的位置以原来的尺寸激活和显示窗口。
        public const int SW_MINIMIZE = 6; //最小化指定的窗口并且激活在Z序中的下一个顶层窗口
        public const int SW_SHOWMINNOACTIVE = 7; //窗口最小化，激活窗口仍然维持激活状态。
        public const int SW_SHOWNA = 8; //以窗口原来的状态显示窗口。激活窗口仍然维持激活状态。
        public const int SW_FORCEMINIMIZE = 11; //在WindowNT5.0中最小化窗口，即使拥有窗口的线程被挂起也会最小化。在从其他线程最小化窗口时才使用这个参数。
        public const int SW_RESTORE = 9; //激活并显示窗口。如果窗口最小化或最大化，则系统将窗口恢复到原来的尺寸和位置。在恢复最小化窗口时，应用程序应该指定这个标志。
        public const int SW_SHOWDEFAULT = 10;
        //依据在STARTUPINFO结构中指定的SW_FLAG标志设定显示状态，STARTUPINFO 结构是由启动应用程序的程序传递给CreateProcess函数的。

        private SW()
        {
        } //最大化指定的窗口
    }


    public static class ClassName
    {
        public const string STATIC = "STATIC";
        public const string TOOLTIPS_CLASS = "tooltips_class32";
    }

    public static class Result
    {
        public static readonly IntPtr TRUE = new IntPtr(1);
        public static readonly IntPtr FALSE = new IntPtr(0);
    }


    public static class RDW
    {
        //private RDW() { }

        public const int RDW_INVALIDATE = 0x0001;
        public const int RDW_INTERNALPAINT = 0x0002;
        public const int RDW_ERASE = 0x0004;
        public const int RDW_VALIDATE = 0x0008;
        public const int RDW_NOINTERNALPAINT = 0x0010;
        public const int RDW_NOERASE = 0x0020;
        public const int RDW_NOCHILDREN = 0x0040;
        public const int RDW_ALLCHILDREN = 0x0080;
        public const int RDW_UPDATENOW = 0x0100;
        public const int RDW_ERASENOW = 0x0200;
        public const int RDW_FRAME = 0x0400;
        public const int RDW_NOFRAME = 0x0800;
    }

    public enum TRACKMOUSEEVENT_FLAGS : uint
    {
        TME_HOVER = 1,
        TME_LEAVE = 2,
        TME_QUERY = 0x40000000,
        TME_CANCEL = 0x80000000
    }


    public enum TernaryRasterOperations : uint
    {
        /// <summary>dest = source</summary>
        SRCCOPY = 0x00CC0020,

        /// <summary>dest = source OR dest</summary>
        SRCPAINT = 0x00EE0086,

        /// <summary>dest = source AND dest</summary>
        SRCAND = 0x008800C6,

        /// <summary>dest = source XOR dest</summary>
        SRCINVERT = 0x00660046,

        /// <summary>dest = source AND (NOT dest)</summary>
        SRCERASE = 0x00440328,

        /// <summary>dest = (NOT source)</summary>
        NOTSRCCOPY = 0x00330008,

        /// <summary>dest = (NOT src) AND (NOT dest)</summary>
        NOTSRCERASE = 0x001100A6,

        /// <summary>dest = (source AND pattern)</summary>
        MERGECOPY = 0x00C000CA,

        /// <summary>dest = (NOT source) OR dest</summary>
        MERGEPAINT = 0x00BB0226,

        /// <summary>dest = pattern</summary>
        PATCOPY = 0x00F00021,

        /// <summary>dest = DPSnoo</summary>
        PATPAINT = 0x00FB0A09,

        /// <summary>dest = pattern XOR dest</summary>
        PATINVERT = 0x005A0049,

        /// <summary>dest = (NOT dest)</summary>
        DSTINVERT = 0x00550009,

        /// <summary>dest = BLACK</summary>
        BLACKNESS = 0x00000042,

        /// <summary>dest = WHITE</summary>
        WHITENESS = 0x00FF0062,

        /// <summary>
        ///     Capture window as seen on screen.  This includes layered windows
        ///     such as WPF windows with AllowsTransparency="true"
        /// </summary>
        CAPTUREBLT = 0x40000000
    }

    public static class VK
    {
        public const int VK_LBUTTON = 0x1;
        public const int VK_RBUTTON = 0x2;
    }


    public class Helper
    {
        public static bool LeftKeyPressed()
        {
            if (SystemInformation.MouseButtonsSwapped)
            {
                return (NativeMethods.GetKeyState(VK.VK_RBUTTON) < 0);
            }
            return (NativeMethods.GetKeyState(VK.VK_LBUTTON) < 0);
        }
    }

    public static class WindowsLong
    {
        public const int GWL_WNDPROC = -4;
        public const int GWL_HINSTANCE = -6;
        public const int GWL_HWNDPARENT = -8;
        public const int GWL_STYLE = -16;
        public const int GWL_EXSTYLE = -20;
        public const int GWL_USERDATA = -21;
        public const int GWL_ID = -12;
    }

    public enum MousePositionCodes
    {
        HTERROR = -2,
        HTTRANSPARENT = -1,
        HTNOWHERE = 0,
        HTCLIENT = 1,
        HTCAPTION = 2,
        HTSYSMENU = 3,
        HTGROWBOX = 4,
        HTSIZE = HTGROWBOX,
        HTMENU = 5,
        HTHSCROLL = 6,
        HTVSCROLL = 7,
        HTMINBUTTON = 8,
        HTMAXBUTTON = 9,
        HTLEFT = 10,
        HTRIGHT = 11,
        HTTOP = 12,
        HTTOPLEFT = 13,
        HTTOPRIGHT = 14,
        HTBOTTOM = 15,
        HTBOTTOMLEFT = 16,
        HTBOTTOMRIGHT = 17,
        HTBORDER = 18,
        HTOBJECT = 19,
        HTCLOSE = 20,
        HTHELP = 21
    }

    public static class ULW
    {
        public const int ULW_COLORKEY = 0x00000001;
        public const int ULW_ALPHA = 0x00000002;
        public const int ULW_OPAQUE = 0x00000004;
    }

    public static class AC
    {
        public const byte AC_SRC_OVER = 0x00;
        public const byte AC_SRC_ALPHA = 0x01;
    }


    public static class AW
    {
        /*
        1. AW_SLIDE : 使用滑动类型, 默认为该类型. 当使用 AW_CENTER 效果时, 此效果被忽略
        2. AW_ACTIVE: 激活窗口, 在使用了 AW_HIDE 效果时不可使用此效果
        3. AW_BLEND: 使用淡入效果
        4. AW_HIDE: 隐藏窗口
        5. AW_CENTER: 与 AW_HIDE 效果配合使用则效果为窗口几内重叠,  单独使用窗口向外扩展.
        6. AW_HOR_POSITIVE : 自左向右显示窗口
        7. AW_HOR_NEGATIVE: 自右向左显示窗口
        8. AW_VER_POSITVE: 自顶向下显示窗口
        9. AW_VER_NEGATIVE : 自下向上显示窗口
        */
        //public const Int32 AW_HOR_POSITIVE = 0x00000001;
        //public const Int32 AW_HOR_NEGATIVE = 0x00000002;
        //public const Int32 AW_VER_POSITIVE = 0x00000004;
        //public const Int32 AW_VER_NEGATIVE = 0x00000008;
        //public const Int32 AW_CENTER = 0x00000010;
        //public const Int32 AW_HIDE = 0x00010000;
        //public const Int32 AW_ACTIVATE = 0x00020000;
        //public const Int32 AW_SLIDE = 0x00040000;
        //public const Int32 AW_BLEND = 0x00080000;
        /// <summary>
        ///     使用滑动类型。缺省则为滚动动画类型。当使用AW_CENTER标志时，这个标志就被忽略。
        /// </summary>
        public const int AW_SLIDE = 0x40000;

        /// <summary>
        ///     激活窗口。在使用了AW_HIDE标志后不要使用这个标志。
        /// </summary>
        public const int AW_ACTIVATE = 0x20000;

        /// <summary>
        ///     使用淡出效果。只有当hWnd为顶层窗口的时候才可以使用此标志。
        /// </summary>
        public const int AW_BLEND = 0x80000;

        /// <summary>
        ///     隐藏窗口，缺省则显示窗口。(关闭窗口用)
        /// </summary>
        public const int AW_HIDE = 0x10000;

        /// <summary>
        ///     若使用了AW_HIDE标志，则使窗口向内重叠；若未使用AW_HIDE标志，则使窗口向外扩展。
        /// </summary>
        public const int AW_CENTER = 0x0010;

        /// <summary>
        ///     自左向右显示窗口。该标志可以在滚动动画和滑动动画中使用。当使用AW_CENTER标志时，该标志将被忽略。
        /// </summary>
        public const int AW_HOR_POSITIVE = 0x0001;

        /// <summary>
        ///     自右向左显示窗口。该标志可以在滚动动画和滑动动画中使用。当使用AW_CENTER标志时，该标志将被忽略。
        /// </summary>
        public const int AW_HOR_NEGATIVE = 0x0002;

        /// <summary>
        ///     自顶向下显示窗口。该标志可以在滚动动画和滑动动画中使用。当使用AW_CENTER标志时，该标志将被忽略。
        /// </summary>
        public const int AW_VER_POSITIVE = 0x0004;

        /// <summary>
        ///     自下向上显示窗口。该标志可以在滚动动画和滑动动画中使用。当使用AW_CENTER标志时，该标志将被忽略。
        /// </summary>
        public const int AW_VER_NEGATIVE = 0x0008; //
    }

    /// <summary>
    ///     建立圆角路径的样式。
    /// </summary> 
    public static class SC
    {
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;
        public const int SC_CLOSE = 0xF060;
        public const int SC_MAXIMIZE = 0xF030;
        public const int SC_MINIMIZE = 0xF020;
        public const int SC_RESTORE = 0xF120;
        public const int SC_CONTEXTHELP = 0xF180;
        public const int SC_NOMAL = 0xF120; //窗体还原消息
    }

    public static class TBM
    {
        private const int WM_USER = 0x0400;
        public const int TBM_GETRANGEMIN = (WM_USER + 1);
        public const int TBM_GETRANGEMAX = (WM_USER + 2);
        public const int TBM_GETTIC = (WM_USER + 3);
        public const int TBM_SETTIC = (WM_USER + 4);
        public const int TBM_SETPOS = (WM_USER + 5);
        public const int TBM_SETRANGE = (WM_USER + 6);
        public const int TBM_SETRANGEMIN = (WM_USER + 7);
        public const int TBM_SETRANGEMAX = (WM_USER + 8);
        public const int TBM_CLEARTICS = (WM_USER + 9);
        public const int TBM_SETSEL = (WM_USER + 10);
        public const int TBM_SETSELSTART = (WM_USER + 11);
        public const int TBM_SETSELEND = (WM_USER + 12);
        public const int TBM_GETPTICS = (WM_USER + 14);
        public const int TBM_GETTICPOS = (WM_USER + 15);
        public const int TBM_GETNUMTICS = (WM_USER + 16);
        public const int TBM_GETSELSTART = (WM_USER + 17);
        public const int TBM_GETSELEND = (WM_USER + 18);
        public const int TBM_CLEARSEL = (WM_USER + 19);
        public const int TBM_SETTICFREQ = (WM_USER + 20);
        public const int TBM_SETPAGESIZE = (WM_USER + 21);
        public const int TBM_GETPAGESIZE = (WM_USER + 22);
        public const int TBM_SETLINESIZE = (WM_USER + 23);
        public const int TBM_GETLINESIZE = (WM_USER + 24);
        public const int TBM_GETTHUMBRECT = (WM_USER + 25);
        public const int TBM_GETCHANNELRECT = (WM_USER + 26);
        public const int TBM_SETTHUMBLENGTH = (WM_USER + 27);
        public const int TBM_GETTHUMBLENGTH = (WM_USER + 28);
    }

    //public static class WM_NCHITTEST
    //{
    //    public const int HTLEFT = 10;
    //    public const int HTRIGHT = 11;
    //    public const int HTTOP = 12;
    //    public const int HTTOPLEFT = 13;
    //    public const int HTTOPRIGHT = 14;
    //    public const int HTBOTTOM = 15;
    //    public const int HTBOTTOMLEFT = 0x10;
    //    public const int HTBOTTOMRIGHT = 17;
    //    public const int HTCAPTION = 2;
    //    public const int HTCLIENT = 1;
    //}

    public static class SWP
    {
        public const uint SWP_NOSIZE = 0x0001;
        public const uint SWP_NOMOVE = 0x0002;
        public const uint SWP_NOZORDER = 0x0004;
        public const uint SWP_NOREDRAW = 0x0008;
        public const uint SWP_NOACTIVATE = 0x0010;
        public const uint SWP_FRAMECHANGED = 0x0020; //The frame changed: send WM_NCCALCSIZE
        public const uint SWP_SHOWWINDOW = 0x0040;
        public const uint SWP_HIDEWINDOW = 0x0080;
        public const uint SWP_NOCOPYBITS = 0x0100;
        public const uint SWP_NOOWNERZORDER = 0x0200; //Don't do owner Z ordering
        public const uint SWP_NOSENDCHANGING = 0x0400; //Don't send WM_WINDOWPOSCHANGING

        public const uint SWP_DRAWFRAME = SWP_FRAMECHANGED;
        public const uint SWP_NOREPOSITION = SWP_NOOWNERZORDER;

#if(WINVER0400) //>= 0x0400
        public const uint SWP_DEFERERASE = 0x2000;
        public const uint SWP_ASYNCWINDOWPOS = 0x4000;
#endif
    }

    [Flags]
    public enum DEVICE_NOTIFY : uint
    {
        DEVICE_NOTIFY_WINDOW_HANDLE = 0x00000000,
        DEVICE_NOTIFY_SERVICE_HANDLE = 0x00000001,
        DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 0x00000004
    }

    public enum DBTDEVICE : uint
    {
        DBT_DEVICEARRIVAL = 0x8000, //A device has been inserted and is now available. 
        DBT_DEVICEQUERYREMOVE = 0x8001,
        //Permission to remove a device is requested. Any application can deny this request and cancel the removal.
        DBT_DEVICEQUERYREMOVEFAILED = 0x8002, //Request to remove a device has been canceled.
        DBT_DEVICEREMOVEPENDING = 0x8003, //Device is about to be removed. Cannot be denied.
        DBT_DEVICEREMOVECOMPLETE = 0x8004, //Device has been removed.
        DBT_DEVICETYPESPECIFIC = 0x8005, //Device-specific event.
        DBT_CUSTOMEVENT = 0x8006 //User-defined event
    }

    public enum DBTDEVTYP : uint
    {
        DBT_DEVTYP_OEM = 0x00000000, //OEM-defined device type
        DBT_DEVTYP_DEVNODE = 0x00000001, //Devnode number
        DBT_DEVTYP_VOLUME = 0x00000002, //Logical volume
        DBT_DEVTYP_PORT = 0x00000003, //Serial, parallel
        DBT_DEVTYP_NET = 0x00000004, //Network resource
        DBT_DEVTYP_DEVICEINTERFACE = 0x00000005, //Device interface class
        DBT_DEVTYP_HANDLE = 0x00000006 //File system handle
    }

    /// <summary>
    ///     Access rights for registry key objects.
    /// </summary>
    public enum REGKEYSECURITY : uint
    {
        /// <summary>
        ///     Combines the STANDARD_RIGHTS_REQUIRED, KEY_QUERY_VALUE, KEY_SET_VALUE, KEY_CREATE_SUB_KEY, KEY_ENUMERATE_SUB_KEYS,
        ///     KEY_NOTIFY, and KEY_CREATE_LINK access rights.
        /// </summary>
        KEY_ALL_ACCESS = 0xF003F,

        /// <summary>
        ///     Reserved for system use.
        /// </summary>
        KEY_CREATE_LINK = 0x0020,

        /// <summary>
        ///     Required to create a subkey of a registry key.
        /// </summary>
        KEY_CREATE_SUB_KEY = 0x0004,

        /// <summary>
        ///     Required to enumerate the subkeys of a registry key.
        /// </summary>
        KEY_ENUMERATE_SUB_KEYS = 0x0008,

        /// <summary>
        ///     Equivalent to KEY_READ.
        /// </summary>
        KEY_EXECUTE = 0x20019,

        /// <summary>
        ///     Required to request change notifications for a registry key or for subkeys of a registry key.
        /// </summary>
        KEY_NOTIFY = 0x0010,

        /// <summary>
        ///     Required to query the values of a registry key.
        /// </summary>
        KEY_QUERY_VALUE = 0x0001,

        /// <summary>
        ///     Combines the STANDARD_RIGHTS_READ, KEY_QUERY_VALUE, KEY_ENUMERATE_SUB_KEYS, and KEY_NOTIFY values.
        /// </summary>
        KEY_READ = 0x20019,

        /// <summary>
        ///     Required to create, delete, or set a registry value.
        /// </summary>
        KEY_SET_VALUE = 0x0002,

        /// <summary>
        ///     Indicates that an application on 64-bit Windows should operate on the 32-bit registry view. For more information,
        ///     see Accessing an Alternate Registry View. This flag must be combined using the OR operator with the other flags in
        ///     this table that either query or access registry values. Windows 2000:  This flag is not supported.
        /// </summary>
        KEY_WOW64_32KEY = 0x0200,

        /// <summary>
        ///     Indicates that an application on 64-bit Windows should operate on the 64-bit registry view. For more information,
        ///     see Accessing an Alternate Registry View. This flag must be combined using the OR operator with the other flags in
        ///     this table that either query or access registry values. Windows 2000:  This flag is not supported.
        /// </summary>
        KEY_WOW64_64KEY = 0x0100,

        /// <summary>
        ///     Combines the STANDARD_RIGHTS_WRITE, KEY_SET_VALUE, and KEY_CREATE_SUB_KEY access rights.
        /// </summary>
        KEY_WRITE = 0x20006
    }


    /// <summary>
    ///     Flags controlling what is included in the device information set built by SetupDiGetClassDevs
    /// </summary>
    [Flags]
    public enum DIGCF
    {
        DIGCF_DEFAULT = 0x00000001, // only valid with DIGCF_DEVICEINTERFACE
        DIGCF_PRESENT = 0x00000002,
        DIGCF_ALLCLASSES = 0x00000004,
        DIGCF_PROFILE = 0x00000008,
        DIGCF_DEVICEINTERFACE = 0x00000010
    }

    /// <summary>
    ///     Values specifying the scope of a device property change.
    /// </summary>
    public enum DICS_FLAG : uint
    {
        /// <summary>
        ///     Make change in all hardware profiles
        /// </summary>
        DICS_FLAG_GLOBAL = 0x00000001,

        /// <summary>
        ///     Make change in specified profile only
        /// </summary>
        DICS_FLAG_CONFIGSPECIFIC = 0x00000002,

        /// <summary>
        ///     1 or more hardware profile-specific
        /// </summary>
        DICS_FLAG_CONFIGGENERAL = 0x00000004
    }

    /// <summary>
    ///     KeyType values for SetupDiCreateDevRegKey, SetupDiOpenDevRegKey, and SetupDiDeleteDevRegKey.
    /// </summary>
    public enum DIREG : uint
    {
        /// <summary>
        ///     Open/Create/Delete device key
        /// </summary>
        DIREG_DEV = 0x00000001,

        /// <summary>
        ///     Open/Create/Delete driver key
        /// </summary>
        DIREG_DRV = 0x00000002,

        /// <summary>
        ///     Delete both driver and Device key
        /// </summary>
        DIREG_BOTH = 0x00000004
    }

    public enum WinErrors : long
    {
        ERROR_SUCCESS = 0,
        ERROR_INVALID_FUNCTION = 1,
        ERROR_FILE_NOT_FOUND = 2,
        ERROR_PATH_NOT_FOUND = 3,
        ERROR_TOO_MANY_OPEN_FILES = 4,
        ERROR_ACCESS_DENIED = 5,
        ERROR_INVALID_HANDLE = 6,
        ERROR_ARENA_TRASHED = 7,
        ERROR_NOT_ENOUGH_MEMORY = 8,
        ERROR_INVALID_BLOCK = 9,
        ERROR_BAD_ENVIRONMENT = 10,
        ERROR_BAD_FORMAT = 11,
        ERROR_INVALID_ACCESS = 12,
        ERROR_INVALID_DATA = 13,
        ERROR_OUTOFMEMORY = 14,
        ERROR_INSUFFICIENT_BUFFER = 122,
        ERROR_MORE_DATA = 234,
        ERROR_NO_MORE_ITEMS = 259,
        ERROR_SERVICE_SPECIFIC_ERROR = 1066,
        ERROR_INVALID_USER_BUFFER = 1784
    }

    public enum CRErrorCodes
    {
        CR_SUCCESS = 0,
        CR_DEFAULT,
        CR_OUT_OF_MEMORY,
        CR_INVALID_POINTER,
        CR_INVALID_FLAG,
        CR_INVALID_DEVNODE,
        CR_INVALID_RES_DES,
        CR_INVALID_LOG_CONF,
        CR_INVALID_ARBITRATOR,
        CR_INVALID_NODELIST,
        CR_DEVNODE_HAS_REQS,
        CR_INVALID_RESOURCEID,
        CR_DLVXD_NOT_FOUND, //WIN 95 ONLY
        CR_NO_SUCH_DEVNODE,
        CR_NO_MORE_LOG_CONF,
        CR_NO_MORE_RES_DES,
        CR_ALREADY_SUCH_DEVNODE,
        CR_INVALID_RANGE_LIST,
        CR_INVALID_RANGE,
        CR_FAILURE,
        CR_NO_SUCH_LOGICAL_DEV,
        CR_CREATE_BLOCKED,
        CR_NOT_SYSTEM_VM, //WIN 95 ONLY
        CR_REMOVE_VETOED,
        CR_APM_VETOED,
        CR_INVALID_LOAD_TYPE,
        CR_BUFFER_SMALL,
        CR_NO_ARBITRATOR,
        CR_NO_REGISTRY_HANDLE,
        CR_REGISTRY_ERROR,
        CR_INVALID_DEVICE_ID,
        CR_INVALID_DATA,
        CR_INVALID_API,
        CR_DEVLOADER_NOT_READY,
        CR_NEED_RESTART,
        CR_NO_MORE_HW_PROFILES,
        CR_DEVICE_NOT_THERE,
        CR_NO_SUCH_VALUE,
        CR_WRONG_TYPE,
        CR_INVALID_PRIORITY,
        CR_NOT_DISABLEABLE,
        CR_FREE_RESOURCES,
        CR_QUERY_VETOED,
        CR_CANT_SHARE_IRQ,
        CR_NO_DEPENDENT,
        CR_SAME_RESOURCES,
        CR_NO_SUCH_REGISTRY_KEY,
        CR_INVALID_MACHINENAME, //NT ONLY
        CR_REMOTE_COMM_FAILURE, //NT ONLY
        CR_MACHINE_UNAVAILABLE, //NT ONLY
        CR_NO_CM_SERVICES, //NT ONLY
        CR_ACCESS_DENIED, //NT ONLY
        CR_CALL_NOT_IMPLEMENTED,
        CR_INVALID_PROPERTY,
        CR_DEVICE_INTERFACE_ACTIVE,
        CR_NO_SUCH_DEVICE_INTERFACE,
        CR_INVALID_REFERENCE_STRING,
        CR_INVALID_CONFLICT_LIST,
        CR_INVALID_INDEX,
        CR_INVALID_STRUCTURE_SIZE,
        NUM_CR_RESULTS
    }

    /// <summary>
    ///     Device registry property codes
    /// </summary>
    public enum SPDRP
    {
        /// <summary>
        ///     DeviceDesc (R/W)
        /// </summary>
        SPDRP_DEVICEDESC = 0x00000000,

        /// <summary>
        ///     HardwareID (R/W)
        /// </summary>
        SPDRP_HARDWAREID = 0x00000001,

        /// <summary>
        ///     CompatibleIDs (R/W)
        /// </summary>
        SPDRP_COMPATIBLEIDS = 0x00000002,

        /// <summary>
        ///     unused
        /// </summary>
        SPDRP_UNUSED0 = 0x00000003,

        /// <summary>
        ///     Service (R/W)
        /// </summary>
        SPDRP_SERVICE = 0x00000004,

        /// <summary>
        ///     unused
        /// </summary>
        SPDRP_UNUSED1 = 0x00000005,

        /// <summary>
        ///     unused
        /// </summary>
        SPDRP_UNUSED2 = 0x00000006,

        /// <summary>
        ///     Class (R--tied to ClassGUID)
        /// </summary>
        SPDRP_CLASS = 0x00000007,

        /// <summary>
        ///     ClassGUID (R/W)
        /// </summary>
        SPDRP_CLASSGUID = 0x00000008,

        /// <summary>
        ///     Driver (R/W)
        /// </summary>
        SPDRP_DRIVER = 0x00000009,

        /// <summary>
        ///     ConfigFlags (R/W)
        /// </summary>
        SPDRP_CONFIGFLAGS = 0x0000000A,

        /// <summary>
        ///     Mfg (R/W)
        /// </summary>
        SPDRP_MFG = 0x0000000B,

        /// <summary>
        ///     FriendlyName (R/W)
        /// </summary>
        SPDRP_FRIENDLYNAME = 0x0000000C,

        /// <summary>
        ///     LocationInformation (R/W)
        /// </summary>
        SPDRP_LOCATION_INFORMATION = 0x0000000D,

        /// <summary>
        ///     PhysicalDeviceObjectName (R)
        /// </summary>
        SPDRP_PHYSICAL_DEVICE_OBJECT_NAME = 0x0000000E,

        /// <summary>
        ///     Capabilities (R)
        /// </summary>
        SPDRP_CAPABILITIES = 0x0000000F,

        /// <summary>
        ///     UiNumber (R)
        /// </summary>
        SPDRP_UI_NUMBER = 0x00000010,

        /// <summary>
        ///     UpperFilters (R/W)
        /// </summary>
        SPDRP_UPPERFILTERS = 0x00000011,

        /// <summary>
        ///     LowerFilters (R/W)
        /// </summary>
        SPDRP_LOWERFILTERS = 0x00000012,

        /// <summary>
        ///     BusTypeGUID (R)
        /// </summary>
        SPDRP_BUSTYPEGUID = 0x00000013,

        /// <summary>
        ///     LegacyBusType (R)
        /// </summary>
        SPDRP_LEGACYBUSTYPE = 0x00000014,

        /// <summary>
        ///     BusNumber (R)
        /// </summary>
        SPDRP_BUSNUMBER = 0x00000015,

        /// <summary>
        ///     Enumerator Name (R)
        /// </summary>
        SPDRP_ENUMERATOR_NAME = 0x00000016,

        /// <summary>
        ///     Security (R/W, binary form)
        /// </summary>
        SPDRP_SECURITY = 0x00000017,

        /// <summary>
        ///     Security (W, SDS form)
        /// </summary>
        SPDRP_SECURITY_SDS = 0x00000018,

        /// <summary>
        ///     Device Type (R/W)
        /// </summary>
        SPDRP_DEVTYPE = 0x00000019,

        /// <summary>
        ///     Device is exclusive-access (R/W)
        /// </summary>
        SPDRP_EXCLUSIVE = 0x0000001A,

        /// <summary>
        ///     Device Characteristics (R/W)
        /// </summary>
        SPDRP_CHARACTERISTICS = 0x0000001B,

        /// <summary>
        ///     Device Address (R)
        /// </summary>
        SPDRP_ADDRESS = 0x0000001C,

        /// <summary>
        ///     UiNumberDescFormat (R/W)
        /// </summary>
        SPDRP_UI_NUMBER_DESC_FORMAT = 0X0000001D,

        /// <summary>
        ///     Device Power Data (R)
        /// </summary>
        SPDRP_DEVICE_POWER_DATA = 0x0000001E,

        /// <summary>
        ///     Removal Policy (R)
        /// </summary>
        SPDRP_REMOVAL_POLICY = 0x0000001F,

        /// <summary>
        ///     Hardware Removal Policy (R)
        /// </summary>
        SPDRP_REMOVAL_POLICY_HW_DEFAULT = 0x00000020,

        /// <summary>
        ///     Removal Policy Override (RW)
        /// </summary>
        SPDRP_REMOVAL_POLICY_OVERRIDE = 0x00000021,

        /// <summary>
        ///     Device Install State (R)
        /// </summary>
        SPDRP_INSTALL_STATE = 0x00000022,

        /// <summary>
        ///     Device Location Paths (R)
        /// </summary>
        SPDRP_LOCATION_PATHS = 0x00000023
    }
}