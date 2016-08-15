using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Win32.Consts;

namespace Win32.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int Left, Top, Right, Bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ANIMATIONINFO
        {
            /// <summary>
            ///     Creates an AMINMATIONINFO structure.
            /// </summary>
            /// <param name="iMinAnimate">If non-zero and SPI_SETANIMATION is specified, enables minimize/restore animation.</param>
            public ANIMATIONINFO(int iMinAnimate)
            {
                cbSize = (uint) Marshal.SizeOf(typeof (ANIMATIONINFO));
                this.iMinAnimate = iMinAnimate;
            }

            /// <summary>
            ///     Always must be set to (System.UInt32)Marshal.SizeOf(typeof(ANIMATIONINFO)).
            /// </summary>
            public uint cbSize;

            /// <summary>
            ///     If non-zero, minimize/restore animation is enabled, otherwise disabled.
            /// </summary>
            public int iMinAnimate;
        }

        public RECT(Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom)
        {
        }

        public int X
        {
            get { return Left; }
            set
            {
                Right -= (Left - value);
                Left = value;
            }
        }

        public int Y
        {
            get { return Top; }
            set
            {
                Bottom -= (Top - value);
                Top = value;
            }
        }

        public int Height
        {
            get { return Bottom - Top; }
            set { Bottom = value + Top; }
        }

        public int Width
        {
            get { return Right - Left; }
            set { Right = value + Left; }
        }

        public System.Drawing.Point Location
        {
            get { return new System.Drawing.Point(Left, Top); }
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        public System.Drawing.Size Size
        {
            get { return new System.Drawing.Size(Width, Height); }
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        public static implicit operator Rectangle(RECT r)
        {
            return new Rectangle(r.Left, r.Top, r.Width, r.Height);
        }

        public static implicit operator RECT(Rectangle r)
        {
            return new RECT(r);
        }

        public static bool operator ==(RECT r1, RECT r2)
        {
            return r1.Equals(r2);
        }

        public static bool operator !=(RECT r1, RECT r2)
        {
            return !r1.Equals(r2);
        }

        public bool Equals(RECT r)
        {
            return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
        }

        public override bool Equals(object obj)
        {
            if (obj is RECT)
                return Equals((RECT) obj);
            if (obj is Rectangle)
                return Equals(new RECT((Rectangle) obj));
            return false;
        }

        public override int GetHashCode()
        {
            return ((Rectangle) this).GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top,
                Right, Bottom);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NMCUSTOMDRAW
    {
        public NMHDR hdr;
        public uint dwDrawStage;
        public IntPtr hdc;
        public RECT rc;
        public IntPtr dwItemSpec;
        public uint uItemState;
        public IntPtr lItemlParam;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NCCALCSIZE_PARAMS
    {
        public RECT rgrc0;
        public RECT rgrc1;
        public RECT rgrc2;
        public IntPtr lppos;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class MOUSEHOOKSTRUCTEX
    {
        public MOUSEHOOKSTRUCT Mouse;
        public int mouseData;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MOUSEHOOKSTRUCT
    {
        public Point Pt;
        public IntPtr hwnd;
        public uint wHitTestCode;
        public IntPtr dwExtraInfo;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MINMAXINFO
    {
        public Point reserved;
        public Size maxSize;
        public System.Drawing.Point maxPosition;
        public System.Drawing.Size minTrackSize;
        public System.Drawing.Size maxTrackSize;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct INITCOMMONCONTROLSEX
    {
        public int dwSize;
        public int dwICC;

        public INITCOMMONCONTROLSEX(int flags)
        {
            dwSize = Marshal.SizeOf(typeof (INITCOMMONCONTROLSEX));
            dwICC = flags;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public sealed class CWPSTRUCT
    {
        public IntPtr lParam;
        public IntPtr wParam;
        public int message;
        public IntPtr hwnd;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CWPRETSTRUCT
    {
        public IntPtr lResult;
        public IntPtr lParam;
        public IntPtr wParam;
        public int message;
        public IntPtr hwnd;
    }

    //[StructLayout(LayoutKind.Sequential)]
    //public struct NMHDR
    //{
    //    public IntPtr hwndFrom;
    //    public int idFrom;
    //    public int code;
    //    public NMHDR(int flag)
    //    {

    //        this.hwndFrom = IntPtr.Zero;
    //        this.idFrom = 0;
    //        this.code = 0;
    //    }
    //}
    [StructLayout(LayoutKind.Sequential)]
    public struct NMTTCUSTOMDRAW
    {
        public NMCUSTOMDRAW nmcd;
        public uint uDrawFlags;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NMHDR
    {
        public IntPtr hwndFrom;
        public int idFrom;
        public int code;

        public NMHDR(int flag)
        {
            hwndFrom = IntPtr.Zero;
            idFrom = 0;
            code = 0;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct NMTTDISPINFO
    {
        public NMHDR hdr;
        public IntPtr lpszText;
        public IntPtr szText;
        public IntPtr hinst;
        public int uFlags;
        public IntPtr lParam;

        public NMTTDISPINFO(int flags)
        {
            hdr = new NMHDR(0);
            lpszText = IntPtr.Zero;
            szText = IntPtr.Zero;
            hinst = IntPtr.Zero;
            uFlags = 0;
            lParam = IntPtr.Zero;
        }
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct SCROLLINFO
    {
        public uint cbSize;
        public uint fMask;
        public int nMin;
        public int nMax;
        public uint nPage;
        public int nPos;
        public int nTrackPos;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct STYLESTRUCT
    {
        public int styleOld;
        public int styleNew;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TCHITTESTINFO
    {
        public System.Drawing.Point Point;
        public int Flags;

        public TCHITTESTINFO(System.Drawing.Point location)
        {
            Point = location;
            Flags = 6;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TT_HITTESTINFO
    {
        public IntPtr hwnd;
        public Point pt;
        public TOOLINFO ti;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct API_MSG
    {
        public IntPtr Hwnd;
        public int Msg;
        public IntPtr WParam;
        public IntPtr LParam;
        public int Time;
        public Point Pt;

        public Message ToMessage()
        {
            var message = new Message();
            message.HWnd = Hwnd;
            message.Msg = Msg;
            message.WParam = WParam;
            message.LParam = LParam;
            return message;
        }

        public void FromMessage(ref Message msg)
        {
            Hwnd = msg.HWnd;
            Msg = msg.Msg;
            WParam = msg.WParam;
            LParam = msg.LParam;
        }
    }

    /// <summary>
    ///     表示在二维平面中定义点的、整数 X 和 Y 坐标的有序对。
    /// </summary>
    //[StructLayout(LayoutKind.Sequential)]
    //public struct POINT
    //{
    //    /// <summary>
    //    ///     X 坐标
    //    /// </summary>
    //    public int X;

    //    /// <summary>
    //    ///     Y 坐标
    //    /// </summary>
    //    public int Y;

    //    /// <summary>
    //    ///     初始化 Win32.POINT 结构的新实例。
    //    /// </summary>
    //    /// <param name="x">x 水平坐标</param>
    //    /// <param name="y">y 垂直坐标</param>
    //    public POINT(int x, int y)
    //    {
    //        X = x;
    //        Y = y;
    //    }

    //    public POINT(System.Drawing.Point pt) : this(pt.X, pt.Y)
    //    {
    //    }

    //    public static implicit operator System.Drawing.Point(POINT p)
    //    {
    //        return new System.Drawing.Point(p.X, p.Y);
    //    }

    //    public static implicit operator POINT(System.Drawing.Point p)
    //    {
    //        return new POINT(p.X, p.Y);
    //    }
    //}

    ///// <summary>
    ///// 存储一组整数，共四个，表示一个矩形的位置和大小
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential)]
    //public struct RECT
    //{
    //    #region 变量
    //    /// <summary>
    //    /// 获取此 RECT 结构左边缘的 x 坐标。
    //    /// </summary>
    //    public int Left;
    //    /// <summary>
    //    /// 获取此 RECT 结构上边缘的 y 坐标。
    //    /// </summary>
    //    public int Top;
    //    /// <summary>
    //    /// 获取 x 坐标，该坐标是此 RECT 结构的 X 与 Width 属性值之和。
    //    /// </summary>
    //    public int Right;
    //    /// <summary>
    //    /// 获取 y 坐标，该坐标是此 RECT 结构的 Y 与 Height 属性值之和。
    //    /// </summary>
    //    public int Bottom;

    //    #endregion

    //    #region 构造函数

    //    /// <summary>
    //    /// 初始化 Win32.RECT 结构的新实例。
    //    /// </summary>
    //    /// <param name="left">此 RECT 结构左边缘的 x 坐标。</param>
    //    /// <param name="top">此 RECT 结构上边缘的 y 坐标。</param>
    //    /// <param name="right">x 坐标，该坐标是此 RECT 结构的 X 与 Width 属性值之和。</param>
    //    /// <param name="bottom">y 坐标，该坐标是此 RECT 结构的 Y 与 Height 属性值之和。</param>
    //    public RECT(int left, int top, int right, int bottom)
    //    {
    //        this.Left = left;
    //        this.Top = top;
    //        this.Right = right;
    //        this.Bottom = bottom;
    //    }

    //    /// <summary>
    //    /// 初始化 Win32.RECT 结构的新实例。
    //    /// </summary>
    //    /// <param name="rect">System.Drawing.Rectangle 对象</param>
    //    public RECT(Rectangle rect)
    //    {
    //        this.Left = rect.Left;
    //        this.Top = rect.Top;
    //        this.Right = rect.Right;
    //        this.Bottom = rect.Bottom;
    //    }

    //    #endregion

    //    #region 属性

    //    /// <summary>
    //    /// 获取或设置此 System.Drawing.Rectangle 的区域。
    //    /// </summary>
    //    public Rectangle Rect
    //    {
    //        get { return new Rectangle(this.Left, this.Top, this.Right - this.Left, this.Bottom - this.Top); }
    //    }

    //    /// <summary>
    //    /// 获取或设置此 RECT 的大小。
    //    /// </summary>
    //    public Size Size
    //    {
    //        get { return new Size(this.Right - this.Left, this.Bottom - this.Top); }
    //    }

    //    #endregion
    //}

    /// <summary>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyBoardHookStruct
    {
        /// <summary>
        /// </summary>
        public int vkCode;

        /// <summary>
        /// </summary>
        public int scanCode;

        /// <summary>
        /// </summary>
        public int flags;

        /// <summary>
        /// </summary>
        public int time;

        /// <summary>
        /// </summary>
        public int dwExtraInfo;
    }

    /// <summary>
    ///     鼠标钩子的相关信息
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseHookStruct
    {
        /// <summary>
        ///     鼠标的屏幕坐标
        /// </summary>
        public Point Point;

        /// <summary>
        ///     鼠标所按下的键
        /// </summary>
        public int MouseData;

        /// <summary>
        ///     指定事件注入标志
        /// </summary>
        public int Flags;

        /// <summary>
        ///     消息的时间戳
        /// </summary>
        public int Time;

        /// <summary>
        ///     与消息相关联的额外信息
        /// </summary>
        public int ExtraInfo;
    }

    /// <summary>
    ///     存储一个有序整数对，通常为矩形的宽度和高度。
    /// </summary>
    //[StructLayout(LayoutKind.Sequential)]
    //public struct SIZE
    //{
    //    #region 变量

    //    /// <summary>
    //    ///     获取或设置此 SIZE 的水平分量。
    //    /// </summary>
    //    public int Width;

    //    /// <summary>
    //    ///     获取或设置此 SIZE 的垂直分量。
    //    /// </summary>
    //    public int Height;

    //    #endregion

    //    #region 构造函数

    //    /// <summary>
    //    ///     初始化 Win32.SIZE 结构的新实例。
    //    /// </summary>
    //    /// <param name="width">此 SIZE 的水平分量</param>
    //    /// <param name="height">此 SIZE 的垂直分量。</param>
    //    public SIZE(int width, int height)
    //    {
    //        Width = width;
    //        Height = height;
    //    }

    //    #endregion
    //}

    /// <summary>
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
        /// <summary>
        ///     文件的图标句柄
        /// </summary>
        public IntPtr hIcon;

        /// <summary>
        ///     图标的系统索引号
        /// </summary>
        public int iIcon;

        /// <summary>
        ///     文件的属性值
        /// </summary>
        public int dwAttributes;

        /// <summary>
        ///     文件的显示名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)] public string szDisplayName;

        /// <summary>
        ///     文件的类型名
        /// </summary>
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)] public string szTypeName;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct BITMAPINFOHEADER
    {
        public uint biSize;
        public int biWidth;
        public int biHeight;
        public ushort biPlanes;
        public ushort biBitCount;
        public uint biCompression;
        public uint biSizeImage;
        public int biXPelsPerMeter;
        public int biYPelsPerMeter;
        public uint biClrUsed;
        public uint biClrImportant;

        public void Init()
        {
            biSize = (uint) Marshal.SizeOf(this);
        }
    }

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct GUITHREADINFO
    {
        public uint cbSize;
        public uint flags;
        public IntPtr hwndActive;
        public IntPtr hwndFocus;
        public IntPtr hwndCapture;
        public IntPtr hwndMenuOwner;
        public IntPtr hwndMoveSize;
        public IntPtr hwndCaret;
        public RECT rcCaret;

        public void Init()
        {
            cbSize = (uint) Marshal.SizeOf(this);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ICONINFO
    {
        public bool fIcon; // Specifies whether this structure defines an icon or a cursor. A value of TRUE specifies 
        // an icon; FALSE specifies a cursor. 
        public int xHotspot;
            // Specifies the x-coordinate of a cursor's hot spot. If this structure defines an icon, the hot 

        // spot is always in the center of the icon, and this member is ignored.
        public int yHotspot;
            // Specifies the y-coordinate of the cursor's hot spot. If this structure defines an icon, the hot 

        // spot is always in the center of the icon, and this member is ignored. 
        public IntPtr hbmMask;
            // (HBITMAP) Specifies the icon bitmask bitmap. If this structure defines a black and white icon, 

        // this bitmask is formatted so that the upper half is the icon AND bitmask and the lower half is 
        // the icon XOR bitmask. Under this condition, the height should be an even multiple of two. If 
        // this structure defines a color icon, this mask only defines the AND bitmask of the icon. 
        public IntPtr hbmColor; // (HBITMAP) Handle to the icon color bitmap. This member can be optional if this 
        // structure defines a black and white icon. The AND bitmask of hbmMask is applied with the SRCAND 
        // flag to the destination; subsequently, the color bitmap is applied (using XOR) to the 
        // destination by using the SRCINVERT flag. 
    }

    ///// <summary>
    ///// 
    ///// </summary>
    //[StructLayout(LayoutKind.Sequential)]
    //public struct BLENDFUNCTION
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public byte BlendOp;
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public byte BlendFlags;
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public byte SourceConstantAlpha;
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public byte AlphaFormat;
    //}
    [StructLayout(LayoutKind.Sequential)]
    public struct XFORM
    {
        public float eM11;
        public float eM12;
        public float eM21;
        public float eM22;
        public float eDx;
        public float eDy;

        public XFORM(float eM11, float eM12, float eM21, float eM22, float eDx, float eDy)
        {
            this.eM11 = eM11;
            this.eM12 = eM12;
            this.eM21 = eM21;
            this.eM22 = eM22;
            this.eDx = eDx;
            this.eDy = eDy;
        }

        /// <summary>
        ///     Allows implicit converstion to a managed transformation matrix.
        /// </summary>
        public static implicit operator Matrix(XFORM xf)
        {
            return new Matrix(xf.eM11, xf.eM12, xf.eM21, xf.eM22, xf.eDx, xf.eDy);
        }

        /// <summary>
        ///     Allows implicit converstion from a managed transformation matrix.
        /// </summary>
        public static implicit operator XFORM(Matrix m)
        {
            var elems = m.Elements;
            return new XFORM(elems[0], elems[1], elems[2], elems[3], elems[4], elems[5]);
        }
    }

    /// <summary>
    ///     BLENDFUNCTION
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PCURSORINFO
    {
        public int cbSize;
        public int flag;
        public IntPtr hCursor;
        public Point ptScreenPos;
    }

    //[StructLayout(LayoutKind.Sequential)]
    //public struct POINT
    //{
    //    public int x;
    //    public int y;

    //    public POINT(int x, int y)
    //    {
    //        this.x = x;
    //        this.y = y;
    //    }
    //}

    //[StructLayout(LayoutKind.Sequential)]
    //public struct SIZE
    //{
    //    public int cx;
    //    public int cy;

    //    public SIZE(int x, int y)
    //    {
    //        cx = x;
    //        cy = y;
    //    }
    //}
    [StructLayout(LayoutKind.Sequential)]
    public struct DWM_BLURBEHIND
    {
        public uint dwFlags;
        [MarshalAs(UnmanagedType.Bool)]
        public bool fEnable;
        public IntPtr hRegionBlur;
        [MarshalAs(UnmanagedType.Bool)]
        public bool fTransitionOnMaximized;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct MARGINS
    {
        public int Left;
        public int Right;
        public int Top;
        public int Bottom;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct BLENDFUNCTION
    {
        public byte BlendOp;
        public byte BlendFlags;
        public byte SourceConstantAlpha;
        public byte AlphaFormat;

        public BLENDFUNCTION(byte op, byte flags, byte alpha, byte format)
        {
            BlendOp = op;
            BlendFlags = flags;
            SourceConstantAlpha = alpha;
            AlphaFormat = format;
        }
    }

    /// <summary>
    ///     ARGB 通道
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ARGB
    {
        /// <summary>
        ///     蓝色值
        /// </summary>
        public byte Blue;

        /// <summary>
        ///     绿色值
        /// </summary>
        public byte Green;

        /// <summary>
        ///     红色值
        /// </summary>
        public byte Red;

        /// <summary>
        ///     透明度
        /// </summary>
        public byte Alpha;
    }

    /// <summary>
    ///     CPU的信息结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct CPU_INFO
    {
        /// <summary>
        /// </summary>
        public uint dwOemId;

        /// <summary>
        /// </summary>
        public uint dwPageSize;

        /// <summary>
        /// </summary>
        public uint lpMinimumApplicationAddress;

        /// <summary>
        /// </summary>
        public uint lpMaximumApplicationAddress;

        /// <summary>
        /// </summary>
        public uint dwActiveProcessorMask;

        /// <summary>
        /// </summary>
        public uint dwNumberOfProcessors;

        /// <summary>
        /// </summary>
        public uint dwProcessorType;

        /// <summary>
        /// </summary>
        public uint dwAllocationGranularity;

        /// <summary>
        /// </summary>
        public uint dwProcessorLevel;

        /// <summary>
        /// </summary>
        public uint dwProcessorRevision;
    }

    /// <summary>
    ///     内存的信息结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MEMORY_INFO
    {
        /// <summary>
        /// </summary>
        public uint dwLength;

        /// <summary>
        /// </summary>
        public uint dwMemoryLoad;

        /// <summary>
        /// </summary>
        public uint dwTotalPhys;

        /// <summary>
        /// </summary>
        public uint dwAvailPhys;

        /// <summary>
        /// </summary>
        public uint dwTotalPageFile;

        /// <summary>
        /// </summary>
        public uint dwAvailPageFile;

        /// <summary>
        /// </summary>
        public uint dwTotalVirtual;

        /// <summary>
        /// </summary>
        public uint dwAvailVirtual;
    }

    /// <summary>
    ///     系统时间的信息结构
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEMTIME_INFO
    {
        /// <summary>
        /// </summary>
        public ushort wYear;

        /// <summary>
        /// </summary>
        public ushort wMonth;

        /// <summary>
        /// </summary>
        public ushort wDayOfWeek;

        /// <summary>
        /// </summary>
        public ushort wDay;

        /// <summary>
        /// </summary>
        public ushort wHour;

        /// <summary>
        /// </summary>
        public ushort wMinute;

        /// <summary>
        /// </summary>
        public ushort wSecond;

        /// <summary>
        /// </summary>
        public ushort wMilliseconds;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SCROLLBARINFO
    {
        public int cbSize;
        public RECT rcScrollBar;
        public int dxyLineButton;
        public int xyThumbTop;
        public int xyThumbBottom;
        public int reserved;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)] internal int[] rgstate;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PAINTSTRUCT
    {
        public IntPtr hdc;
        public int fErase;
        public RECT rcPaint;
        public int fRestore;
        public int fIncUpdate;
        public int Reserved1;
        public int Reserved2;
        public int Reserved3;
        public int Reserved4;
        public int Reserved5;
        public int Reserved6;
        public int Reserved7;
        public int Reserved8;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TRACKMOUSEEVENT
    {
        internal uint cbSize;
        internal TRACKMOUSEEVENT_FLAGS dwFlags;
        internal IntPtr hwndTrack;
        internal uint dwHoverTime;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TOOLINFO
    {
        internal TOOLINFO(int flags)
        {
            cbSize = Marshal.SizeOf(typeof (TOOLINFO));
            uFlags = flags;
            hwnd = IntPtr.Zero;
            uId = IntPtr.Zero;
            rect = new RECT(0, 0, 0, 0);
            hinst = IntPtr.Zero;
            lpszText = IntPtr.Zero;
            lParam = IntPtr.Zero;
        }

        public int cbSize;
        public int uFlags;
        public IntPtr hwnd;
        public IntPtr uId;
        public RECT rect;
        public IntPtr hinst;
        public IntPtr lpszText;
        public IntPtr lParam;
    }


    //[StructLayout(LayoutKind.Sequential)]
    //public struct NMHDR
    //{
    //    internal NMHDR(int flag)
    //    {
    //        this.hwndFrom = IntPtr.Zero;
    //        this.idFrom = 0;
    //        this.code = 0;
    //    }

    //    internal IntPtr hwndFrom;
    //    internal int idFrom;
    //    internal int code;
    //}
    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWINFO
    {
        public uint cbSize;
        public RECT rcWindow;
        public RECT rcClient;
        public uint dwStyle;
        public uint dwExStyle;
        public uint dwWindowStatus;
        public uint cxWindowBorders;
        public uint cyWindowBorders;
        public IntPtr atomWindowType;
        public ushort wCreatorVersion;
    }

    public struct WINDOWPLACEMENT
    {
        public int flags;
        public int length;
        public Point ptMaxPosition;
        public Point ptMinPosition;
        public RECT rcNormalPosition;
        public int showCmd;

        public static WINDOWPLACEMENT Default
        {
            get
            {
                var structure = new WINDOWPLACEMENT();
                structure.length = Marshal.SizeOf(structure);
                return structure;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPOS
    {
        public IntPtr hWnd;
        public IntPtr hWndInsertAfter;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public int flags;
    }


    /// <summary>
    ///     WNDCLASSEX
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct WNDCLASSEX
    {
        [MarshalAs(UnmanagedType.U4)] public uint cbSize;
        [MarshalAs(UnmanagedType.U4)] public uint style;
        public WndProcDelegate lpfnWndProc;
        public int cbClsExtra;
        public int cbWndExtra;
        public IntPtr hInstance;
        public IntPtr hIcon;
        public IntPtr hCursor;
        public IntPtr hbrBackground;
        [MarshalAs(UnmanagedType.LPWStr)] public string lpszMenuName;
        [MarshalAs(UnmanagedType.LPWStr)] public string lpszClassName;
        public IntPtr hIconSm;

        public void Init()
        {
            cbSize = (uint) Marshal.SizeOf(this);
        }
    }

    #region ComboBoxInfo Struct

    public enum ComboBoxButtonState
    {
        STATE_SYSTEM_NONE = 0,
        STATE_SYSTEM_INVISIBLE = 0x00008000,
        STATE_SYSTEM_PRESSED = 0x00000008
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ComboBoxInfo
    {
        public int cbSize;
        public RECT rcItem;
        public RECT rcButton;
        public ComboBoxButtonState stateButton;
        public IntPtr hwndCombo;
        public IntPtr hwndEdit;
        public IntPtr hwndList;
    }

    #endregion

    #region USB

    //pack=8 for 64 bit.
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SP_DEVINFO_DATA
    {
        public uint cbSize;
        public Guid ClassGuid;
        public uint DevInst;
        public IntPtr Reserved;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SP_DEVICE_INTERFACE_DATA
    {
        public uint cbSize;
        public Guid interfaceClassGuid;
        public uint flags;
        private readonly IntPtr reserved;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SP_DEVICE_INTERFACE_DETAIL_DATA
    {
        public uint cbSize;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)] public string devicePath;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct DevBroadcastDeviceInterfaceBuffer
    {
        public DevBroadcastDeviceInterfaceBuffer(int deviceType)
        {
            dbch_size = Marshal.SizeOf(typeof (DevBroadcastDeviceInterfaceBuffer));
            dbch_devicetype = deviceType;
            dbch_reserved = 0;
        }

        [FieldOffset(0)] public int dbch_size;
        [FieldOffset(4)] public int dbch_devicetype;
        [FieldOffset(8)] public int dbch_reserved;
    }

    //Structure with information for RegisterDeviceNotification.
    //DEV_BROADCAST_HDR Structure
    /*typedef struct _DEV_BROADCAST_HDR {
      DWORD dbch_size;
      DWORD dbch_devicetype;
      DWORD dbch_reserved;
    }DEV_BROADCAST_HDR, *PDEV_BROADCAST_HDR;*/

    [StructLayout(LayoutKind.Sequential)]
    public struct DEV_BROADCAST_HDR
    {
        public int dbcc_size;
        public int dbcc_devicetype;
        public int dbcc_reserved;
    }

    //DEV_BROADCAST_HANDLE Structure
    /*typedef struct _DEV_BROADCAST_HANDLE {
      DWORD      dbch_size;
      DWORD      dbch_devicetype;
      DWORD      dbch_reserved;
      HANDLE     dbch_handle;
      HDEVNOTIFY dbch_hdevnotify;
      GUID       dbch_eventguid;
      LONG       dbch_nameoffset;
      BYTE       dbch_data[1];
    }DEV_BROADCAST_HANDLE *PDEV_BROADCAST_HANDLE;*/

    [StructLayout(LayoutKind.Sequential)]
    public struct DEV_BROADCAST_HANDLE
    {
        public int dbch_size;
        public int dbch_devicetype;
        public int dbch_reserved;
        public IntPtr dbch_handle;
        public IntPtr dbch_hdevnotify;
        public Guid dbch_eventguid;
        public long dbch_nameoffset;
        public byte dbch_data;
        public byte dbch_data1;
    }

    //DEV_BROADCAST_DEVICEINTERFACE Structure
    /*typedef struct _DEV_BROADCAST_DEVICEINTERFACE {
      DWORD dbcc_size;
      DWORD dbcc_devicetype;
      DWORD dbcc_reserved;
      GUID  dbcc_classguid;
      TCHAR dbcc_name[1];
    }DEV_BROADCAST_DEVICEINTERFACE *PDEV_BROADCAST_DEVICEINTERFACE;*/

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct DEV_BROADCAST_DEVICEINTERFACE
    {
        public int dbcc_size;
        public int dbcc_devicetype;
        public int dbcc_reserved;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 16)] public byte[]
            dbcc_classguid;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)] public char[] dbcc_name;
    }

    //DEV_BROADCAST_VOLUME Structure
    /*typedef struct _DEV_BROADCAST_VOLUME {
      DWORD dbcv_size;
      DWORD dbcv_devicetype;
      DWORD dbcv_reserved;
      DWORD dbcv_unitmask;
      WORD  dbcv_flags;
    }DEV_BROADCAST_VOLUME, *PDEV_BROADCAST_VOLUME;*/

    [StructLayout(LayoutKind.Sequential)]
    public struct DEV_BROADCAST_VOLUME
    {
        public int dbcv_size;
        public int dbcv_devicetype;
        public int dbcv_reserved;
        public int dbcv_unitmask;
        public short dbcv_flags;
    }

    //DEV_BROADCAST_PORT Structure
    /*typedef struct _DEV_BROADCAST_PORT {
      DWORD dbcp_size;
      DWORD dbcp_devicetype;
      DWORD dbcp_reserved;
      TCHAR dbcp_name[1];
    }DEV_BROADCAST_PORT *PDEV_BROADCAST_PORT;*/

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct DEV_BROADCAST_PORT
    {
        public int dbcp_size;
        public int dbcp_devicetype;
        public int dbcp_reserved;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)] public char[] dbcp_name;
    }

    //DEV_BROADCAST_OEM Structure
    /*typedef struct _DEV_BROADCAST_OEM {
      DWORD dbco_size;
      DWORD dbco_devicetype;
      DWORD dbco_reserved;
      DWORD dbco_identifier;
      DWORD dbco_suppfunc;
    }DEV_BROADCAST_OEM, *PDEV_BROADCAST_OEM;*/

    [StructLayout(LayoutKind.Sequential)]
    public struct DEV_BROADCAST_OEM
    {
        public int dbco_size;
        public int dbco_devicetype;
        public int dbco_reserved;
        public int dbco_identifier;
        public int dbco_suppfunc;
    }

    /// <summary>
    ///     Device Interface GUIDs.
    /// </summary>
    public struct GUID_DEVINTERFACE
    {
        public const string DISK = "53f56307-b6bf-11d0-94f2-00a0c91efb8b";
        public const string HUBCONTROLLER = "3abf6f2d-71c4-462a-8a92-1e6861e6af27";
        public const string MODEM = "2C7089AA-2E0E-11D1-B114-00C04FC2AAE4";
        public const string SERENUM_BUS_ENUMERATOR = "4D36E978-E325-11CE-BFC1-08002BE10318";
        public const string COMPORT = "86E0D1E0-8089-11D0-9CE4-08003E301F73";
        public const string PARALLEL = "97F76EF0-F883-11D0-AF1F-0000F800845C";
    }

    #endregion
}