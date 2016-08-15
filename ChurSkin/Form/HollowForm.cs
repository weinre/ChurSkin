using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using Win32;
using Win32.Consts;

namespace System.Windows.Forms
{
    public partial class HollowForm : Form
    {
        /// <summary>
        ///     声明委托类
        /// </summary>
        /// <param name="MsgStr"></param>
        public delegate void FormCt();

        /// <summary>
        ///     定义委托
        /// </summary>
        public static FormCt Ct;

        private readonly Bitmap downImg;// = Properties.Resources.btn_close_down;
        private readonly Bitmap hoverImg;//= Properties.Resources.btn_close_highlight;
        private readonly Bitmap normalImg;// = Properties.Resources.btn_close_normal;
        private readonly int xy = 10;
        private Padding _borderPadding;
        private int _captionHeight;
        private Bitmap btnImg;// = Properties.Resources.btn_close_normal;
        private bool canMove = true;
        private bool canResize = true;

        public HollowForm()
        {
            InitializeComponent();
            //这个地方 研究很多穿透的方法。 貌似只有对绿色 才有穿透效果。红色和蓝色 没有穿透效果。是微软的bug 吗 ？还是故意为之 ？
            BackColor = TransparencyKey = Color.Green;
            _captionHeight = 30;
            SetStyles();
            MinimumSize = new Size(320, 240);
            //this.Load += (a, b) =>
            //{
            //   // CanPenetrate();
            //};
        }

        [Description("是否可以调整窗体大小"), DefaultValue(true)]
        public bool CanResize
        {
            get { return canResize; }
            set { canResize = value; }
        }

        [Description("是否可以移动窗体"), DefaultValue(true)]
        public bool CanMove
        {
            get { return canMove; }
            set { canMove = value; }
        }

        public Padding BorderPadding
        {
            get { return _borderPadding; }
            set
            {
                _borderPadding = value;
                Refresh();
            }
        }

        [Category("Skin"), DefaultValue(0x18), Description("设置或获取窗体标题栏的高度")]
        public int CaptionHeight
        {
            get { return _captionHeight; }
            set
            {
                if (_captionHeight != value)
                {
                    _captionHeight = (value < BorderPadding.Left) ? BorderPadding.Left : value;
                    Invalidate();
                }
            }
        }

        public Rectangle CaptionRect
        {
            get { return new Rectangle(3, 3, Width, CaptionHeight); }
        }

        private Rectangle closeRect
        {
            get { return new Rectangle(Width - 39 - 3, 2, 39, 20); }
        }

        private Rectangle textRect
        {
            get
            {
                return new Rectangle(IconRect.Right + 3, IconRect.Top, ((Width - IconRect.Right)) - 6, CaptionHeight);
            }
        }

        public Rectangle IconRect
        {
            get
            {
                var width = SystemInformation.SmallIconSize.Width;
                if (!ShowIcon || (Icon == null))
                {
                    return new Rectangle(3, (CaptionHeight - width) / 2, 1, 1);
                }
                var x = BorderPadding.Left < 6 ? 6 : BorderPadding.Left;
                if (((CaptionHeight - x) - 6) < width)
                {
                    width = (CaptionHeight - x) - 6;
                }
                x += 3;
                //return new Rectangle(x, x + (((this.CaptionHeight - x) - width) / 2), width, width); old
                return new Rectangle(x, (CaptionHeight - width + 6) / 2, width, width);
            }
        }

        private void SetStyles()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.DoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            UpdateStyles();
            AutoScaleMode = AutoScaleMode.None;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            // Share.GraphicSetup(graphics);
            //  base.OnPaint(e);
            //绘制质感的标题栏
            if (true)
            {
                var lgb = new LinearGradientBrush(CaptionRect, Share.LineTop, Share.LineBottom, 90f);
                graphics.FillRectangle(lgb, CaptionRect);
                lgb.Dispose();
                var p = new Pen(Share.Line, 1);
                graphics.DrawLine(p, new Point(0, CaptionRect.Height + 3),
                    new Point(CaptionRect.Width, CaptionRect.Height + 3));
                //p.Color = Color.Blue;
                //  p.Width = 5;
                //  graphics.DrawLine(p, new Point(0, this.CaptionRect.Height + 1), new Point(this.CaptionRect.Width, this.CaptionRect.Height+1));
                p.Dispose();
            }
            //标题文字

            var size = TextRenderer.MeasureText(Text, Font);
            var image = SkinTools.ImageLightEffect(Text, Font, ForeColor, Color.White, 4,
                new Rectangle(0, 0, Width, size.Height), true);
            graphics.DrawImage(image, textRect.X - (4 / 2), textRect.Y - (4 / 2));
            //画图标
            if (Icon != null)
                graphics.DrawIcon(Icon, IconRect);
            //画关闭按钮
            graphics.DrawImage(btnImg, closeRect);
            //画边框
            var rect = ClientRectangle;
            rect.X += 3;
            rect.Y += 3;
            rect.Width -= 7;
            rect.Height -= 7;
            using (var pen = new Pen(Color.DarkGray))
            {
                var path = DrawHelper.CreateRoundPathFPoint(rect, 2);
                graphics.DrawPath(pen, path);
                path.Dispose();
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            var rect = ClientRectangle;
            rect.Inflate(-3, -3);
            var shape = DrawHelper.CreateRoundPathFPoint(rect, 2);
            Region = new Region(shape);
            // DrawHelper.SetWindowRegion(this, 2);
            Invalidate();
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x84:
                    WmNcHitTest(ref m);
                    return;
                case 0x0203:
                case 0x00A3:
                    return;
                default:
                    base.WndProc(ref m);
                    return;
            }
            // base.WndProc(ref m);
        }

        private void WmNcHitTest(ref Message m)
        {
            var p = new Point(m.LParam.ToInt32());
            p = PointToClient(p);
            //if (this.IconRect.Contains(p) && this.ShowSystemMenu)
            //{
            //    m.Result = new IntPtr(3);
            //}
            //else
            //{
            if (CanResize)
            {
                if ((p.X < xy) && (p.Y < xy))
                {
                    m.Result = new IntPtr(13);
                    return;
                }
                if ((p.X > (Width - xy)) && (p.Y < xy))
                {
                    m.Result = new IntPtr(14);
                    return;
                }
                if ((p.X < xy) && (p.Y > (Height - xy)))
                {
                    m.Result = new IntPtr(0x10);
                    return;
                }
                if ((p.X > (Width - xy)) && (p.Y > (Height - xy)))
                {
                    m.Result = new IntPtr(0x11);
                    return;
                }
                if (p.Y < xy)
                {
                    m.Result = new IntPtr(12);
                    return;
                }
                if (p.Y > (Height - xy))
                {
                    m.Result = new IntPtr(15);
                    return;
                }
                if (p.X < xy)
                {
                    m.Result = new IntPtr(10);
                    return;
                }
                if (p.X > (Width - xy))
                {
                    m.Result = new IntPtr(11);
                    return;
                }
            }


            if (closeRect.Contains(p))
            {
                btnImg = hoverImg;
                Invalidate();
            }
            else
            {
                btnImg = normalImg;
                Invalidate();
            }
            if (p.Y < CaptionHeight && p.X < closeRect.X && CanMove)
            {
                m.Result = new IntPtr(2); //转到caption;
                return;
            }
            //  int oldGWLEx = NativeMethods.SetWindowLong(this.Handle, (int)Win32.Consts.GWL.GWL_EXSTYLE, (int)Win32.Consts.WS_EX.WS_EX_LAYERED | (int)Win32.Consts.WS_EX.WS_EX_TRANSPARENT);

            m.Result = new IntPtr(1); //转到client;

            //if (this.Mobile == MobileStyle.Mobile)
            //{
            //    m.Result = new IntPtr(2);
            //    return;
            //}
            //   m.Result=
            //  m.Result = new IntPtr(1);
            //  this.isMouseDown = false;
            // }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            btnImg = normalImg;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (closeRect.Contains(e.Location))
            {
                btnImg = downImg;
                Invalidate(closeRect);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (closeRect.Contains(e.Location))
            {
                Close();
            }
        }

        public void CanPenetrate()
        {
            if (InvokeRequired)
            {
                Invoke(Ct);
            }
            else
            {
                TopMost = true;
                NativeMethods.GetWindowLong(Handle, GWL.GWL_EXSTYLE);
                NativeMethods.SetWindowLong(Handle, GWL.GWL_EXSTYLE, WS_EX.WS_EX_TRANSPARENT | WS_EX.WS_EX_LAYERED);
                //  NativeMethods.SetLayeredWindowAttributes(this.Handle, 0, 100, Win32.Consts.LWA.LWA_ALPHA);
            }
            // int intExTemp = NativeMethods.GetWindowLong(this.Handle, (int)Win32.Consts.GWL.GWL_EXSTYLE);

            // int oldGWLEx = NativeMethods.SetWindowLong(this.Handle, (int)Win32.Consts.GWL.GWL_EXSTYLE, (int)Win32.Consts.WS_EX.WS_EX_LAYERED | (int)Win32.Consts.WS_EX.WS_EX_TRANSPARENT);

            //实则是 封装 TransparencyKey
            // NativeMethods.SetLayeredWindowAttributes(this.Handle, 0x0000ffff, 255, Win32.Consts.LWA.LWA_COLORKEY);
        }
    }
}