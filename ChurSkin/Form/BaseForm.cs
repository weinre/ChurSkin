using System.Drawing;
using System.Drawing.Drawing2D;
using Win32; 

namespace System.Windows.Forms
{
    public partial class BaseForm : Form
    {
        private readonly CForm Main;
        public Color[] CornerColors;
        public Color[] ShadowColors;

        public BaseForm(CForm main)
        {
            InitializeComponent();
            ShadowColors = new[] {Color.FromArgb(60, Color.Black), Color.Transparent};
            CornerColors = new[] {Color.FromArgb(180, Color.Black), Color.Transparent};
            Main = main;
            InitializeComponent();
            SetStyles();
            Init();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var createParams = base.CreateParams;
                createParams.ExStyle |= 0x80000;
                return createParams;
            }
        }

        //public BaseForm()
        //{
        //    InitializeComponent();
        //  //  this.Main = this;
        //    this.SetStyles();
        //    this.SetBits();
        //    this.CanPenetrate();
        //}
        private void Init()
        {
            TopMost = Main.TopMost;
            Main.BringToFront();
            ShowInTaskbar = false;
            ////base.FormBorderStyle = FormBorderStyle.None;
            Location = new Point(Main.Location.X - Main.ShadowWidth, Main.Location.Y - Main.ShadowWidth);
            Icon = Main.Icon;
            ShowIcon = Main.ShowIcon;
            Width = Main.Width + (Main.ShadowWidth*2);
            Height = Main.Height + (Main.ShadowWidth*2);
            Text = Main.Text;
            Main.LocationChanged += Main_LocationChanged;
            Main.SizeChanged += Main_SizeChanged;
            Main.VisibleChanged += Main_VisibleChanged;
            SetBits();
            CanPenetrate();
        }

        //#region 磨砂
        //[StructLayout(LayoutKind.Sequential)]
        //public struct MARGINS
        //{
        //    public int Left;
        //    public int Right;
        //    public int Top;
        //    public int Bottom;
        //}

        //[DllImport("dwmapi.dll", PreserveSig = false)]
        //static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

        //[DllImport("dwmapi.dll", PreserveSig = false)]
        //static extern bool DwmIsCompositionEnabled(); //Dll 导入 DwmApi
        //protected override void OnLoad(EventArgs e)
        //{
        //    //如果启用Aero
        //    if (!DesignMode && DwmIsCompositionEnabled())
        //    {
        //        MARGINS m = new MARGINS();
        //        m.Right = -1; //设为负数,则全窗体透明
        //        DwmExtendFrameIntoClientArea(this.Handle, ref m); //开启全窗体透明效果
        //    }
        //    base.OnLoad(e); 
        //}
        //protected override void OnPaintBackground(PaintEventArgs e)
        //{
        //    base.OnPaintBackground(e);
        //    if (!DesignMode && DwmIsCompositionEnabled())
        //    {
        //        e.Graphics.Clear(Color.Black); //将窗体用黑色填充（Dwm 会把黑色视为透明区域）
        //    }
        //}
        //#endregion
        private void SetStyles()
        {
            SetStyle(
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        private void Main_LocationChanged(object sender, EventArgs e)
        {
            Location = new Point(Main.Left - Main.ShadowWidth, Main.Top - Main.ShadowWidth);
        }

        private void Main_SizeChanged(object sender, EventArgs e)
        {
            Width = Main.Width + (Main.ShadowWidth*2);
            Height = Main.Height + (Main.ShadowWidth*2);
            SetBits();
        }

        private void Main_VisibleChanged(object sender, EventArgs e)
        {
            Visible = Main.Visible;
        }

        private void DrawCorners(Graphics g, Size corSize)
        {
            Action<int> action = delegate(int n)
            {
                using (var path = new GraphicsPath())
                {
                    Point point;
                    float num2;
                    PointF tf;
                    Point point2;
                    Point point3;
                    var size = new Size(corSize.Width*2, corSize.Height*2);
                    var size2 = new Size(this.Main.Radius*2, this.Main.Radius*2);
                    switch (n)
                    {
                        case 1:
                            point = new Point(0, 0);
                            num2 = 180f;
                            tf = new PointF(size.Width - (size2.Width*0.5f), size.Height - (size2.Height*0.5f));
                            point2 = new Point(corSize.Width, this.Main.ShadowWidth);
                            point3 = new Point(this.Main.ShadowWidth, corSize.Height);
                            break;

                        case 3:
                            point = new Point(this.Width - size.Width, 0);
                            num2 = 270f;
                            tf = new PointF(point.X + (size2.Width*0.5f), size.Height - (size2.Height*0.5f));
                            point2 = new Point(this.Width - this.Main.ShadowWidth, corSize.Height);
                            point3 = new Point(this.Width - corSize.Width, this.Main.ShadowWidth);
                            break;

                        case 7:
                            point = new Point(0, this.Height - size.Height);
                            num2 = 90f;
                            tf = new PointF(size.Width - (size2.Width*0.5f), point.Y + (size2.Height*0.5f));
                            point2 = new Point(this.Main.ShadowWidth, this.Height - corSize.Height);
                            point3 = new Point(corSize.Width, this.Height - this.Main.ShadowWidth);
                            break;

                        default:
                            point = new Point(this.Width - size.Width, this.Height - size.Height);
                            num2 = 0f;
                            tf = new PointF(point.X + (size2.Width*0.5f), point.Y + (size2.Height*0.5f));
                            point2 = new Point(this.Width - corSize.Width, this.Height - this.Main.ShadowWidth);
                            point3 = new Point(this.Width - this.Main.ShadowWidth, this.Height - corSize.Height);
                            break;
                    }
                    var rect = new Rectangle(point, size);
                    var location = new Point(point.X + ((size.Width - size2.Width)/2),
                        point.Y + ((size.Height - size2.Height)/2));
                    var rectangle2 = new Rectangle(location, size2);
                    path.AddArc(rect, num2, 91f);
                    if (this.Main.Radius > 3)
                    {
                        path.AddArc(rectangle2, num2 + 90f, -91f);
                    }
                    else
                    {
                        path.AddLine(point2, point3);
                    }
                    using (var brush = new PathGradientBrush(path))
                    {
                        var colorArray = new Color[2];
                        var numArray = new float[2];
                        var blend = new ColorBlend();
                        colorArray[0] = this.CornerColors[1];
                        colorArray[1] = this.CornerColors[0];
                        numArray[0] = 0f;
                        numArray[1] = 1f;
                        blend.Colors = colorArray;
                        blend.Positions = numArray;
                        brush.InterpolationColors = blend;
                        brush.CenterPoint = tf;
                        g.FillPath(brush, path);
                    }
                }
            };
            action(1);
            action(3);
            action(7);
            action(9);
        }

        private void DrawLines(Graphics g, Size corSize, Size gradientSize_LR, Size gradientSize_TB)
        {
            var rect = new Rectangle(new Point(corSize.Width, 0), gradientSize_TB);
            var rectangle2 = new Rectangle(new Point(0, corSize.Width), gradientSize_LR);
            var rectangle3 = new Rectangle(new Point(Size.Width - Main.ShadowWidth, corSize.Width), gradientSize_LR);
            var rectangle4 = new Rectangle(new Point(corSize.Width, Size.Height - Main.ShadowWidth), gradientSize_TB);
            using (
                var brush = new LinearGradientBrush(rect, ShadowColors[1], ShadowColors[0], LinearGradientMode.Vertical)
                )
            {
                using (
                    var brush2 = new LinearGradientBrush(rectangle2, ShadowColors[1], ShadowColors[0],
                        LinearGradientMode.Horizontal))
                {
                    using (
                        var brush3 = new LinearGradientBrush(rectangle3, ShadowColors[0], ShadowColors[1],
                            LinearGradientMode.Horizontal))
                    {
                        using (
                            var brush4 = new LinearGradientBrush(rectangle4, ShadowColors[0], ShadowColors[1],
                                LinearGradientMode.Vertical))
                        {
                            g.FillRectangle(brush, rect);
                            g.FillRectangle(brush2, rectangle2);
                            g.FillRectangle(brush3, rectangle3);
                            g.FillRectangle(brush4, rectangle4);
                        }
                    }
                }
            }
        }

        private void DrawShadow(Graphics g)
        {
            ShadowColors[0] = Color.FromArgb(60, Main.ShadowColor);
            CornerColors[0] = Color.FromArgb(180, Main.ShadowColor);
            var corSize = new Size(Main.ShadowWidth + Main.Radius, Main.ShadowWidth + Main.Radius);
            var size2 = new Size(Main.ShadowWidth, Size.Height - (corSize.Height*2));
            var size4 = new Size(Size.Width - (corSize.Width*2), Main.ShadowWidth);
            DrawLines(g, corSize, size2, size4);
            DrawCorners(g, corSize);
        }

        public void SetBits()
        {
            var image = new Bitmap(Main.Width + (Main.ShadowWidth*2), Main.Height + (Main.ShadowWidth*2));
            var g = Graphics.FromImage(image);
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            DrawShadow(g);
            if (Image.IsCanonicalPixelFormat(image.PixelFormat) && Image.IsAlphaPixelFormat(image.PixelFormat))
            {
                var zero = IntPtr.Zero;
                var dC = NativeMethods.GetDC(IntPtr.Zero);
                var hgdiobj = IntPtr.Zero;
                var hdc = NativeMethods.CreateCompatibleDC(dC);
                try
                {
                    var pptDst = new Win32.Struct.POINT(Left, Top);
                    var psize = new Win32.Struct.SIZE(Width, Height);
                    var pblend = new Win32.Struct.BLENDFUNCTION();
                    var pprSrc = new Win32.Struct.POINT(0, 0);
                    hgdiobj = image.GetHbitmap(Color.FromArgb(0));
                    zero = NativeMethods.SelectObject(hdc, hgdiobj);
                    pblend.BlendOp = 0;
                    pblend.SourceConstantAlpha = byte.Parse("255");
                    pblend.AlphaFormat = 1;
                    pblend.BlendFlags = 0;
                    NativeMethods.UpdateLayeredWindow(Handle, dC, ref pptDst, ref psize, hdc, ref pprSrc, 0, ref pblend,
                        2);
                    return;
                }
                finally
                {
                    if (hgdiobj != IntPtr.Zero)
                    {
                        NativeMethods.SelectObject(hdc, zero);
                        NativeMethods.DeleteObject(hgdiobj);
                    }
                    NativeMethods.ReleaseDC(IntPtr.Zero, dC);
                    NativeMethods.DeleteDC(hdc);
                }
            }
            throw new ApplicationException("图片必须是32位带Alhpa通道的图片。");
        }

        private void CanPenetrate()
        {
            NativeMethods.GetWindowLong(Handle, -20);
            NativeMethods.SetWindowLong(Handle, -20, 0x80020);
        }
    }
}