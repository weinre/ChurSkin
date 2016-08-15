using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using Win32;

namespace System.Windows.Forms
{
    public partial class CPanel : Panel
    {
        private Padding round = new Padding(Share.DefaultRadius);
        private Color bottomColor = Share.BorderColor;
        private bool macStyle;
        private bool canMoveSelf;
        private bool canMoveParent;
        private bool showBorder = true;
        //Color color { get { return Color.FromArgb((int)alpha, bgColor.R, bgColor.G, bgColor.B); } }
        //Alpha alpha = Alpha.Normal;
        //public Color BgColor { get { return bgColor; } set { bgColor = value; this.Invalidate(); } }
        private Color topColor = Share.BackColor;
        private Color bgColor;

        public CPanel()
        {
            InitializeComponent();
            SetStyles();
        }

        [Description("是否渲染渐变"), DefaultValue(false)]
        public bool MacStyle
        {
            get { return macStyle; }
            set
            {
                macStyle = value;
                Refresh();
            }
        }
        [Description("是否允许拖动控件"), DefaultValue(false)]
        public bool CanMoveSeft
        {
            get { return canMoveSelf; }
            set { canMoveSelf = value; }
        }
        [Description("是否允许拖动窗体"), DefaultValue(false)]
        public bool CanMoveParent
        {
            get { return canMoveParent; }
            set { canMoveParent = value; }
        }

        [Description("是否显示边框"), DefaultValue(true)]
        public bool ShowBorder
        {
            get { return showBorder; }
            set
            {
                showBorder = value;
                Invalidate();
            }
        }

        [Description("边框圆角")]
        public Padding Round
        {
            get { return round; }
            set
            {
                round = value;
                Invalidate();
            }
        }
        [Description("背景颜色")]
        public Color BgColor
        {
            get { return bgColor; }
            set { bgColor = value; Invalidate(); }
        }
        private GraphicsPath rectPath
        {
            get { return DrawHelper.CreateRoundPath(ClientRectangle, round); }
        }
        [Description("渐变顶部的颜色")]
        public Color TopColor
        {
            get { return topColor; }
            set { topColor = value; Invalidate(); }
        }

        [Description("渐变低部的颜色")]
        public Color BottomColor
        {
            get { return bottomColor; }
            set { bottomColor = value; Invalidate(); }
        }

        private void SetStyles()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Selectable |
                ControlStyles.DoubleBuffer |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.Opaque, false);
            BackColor = Color.Transparent;
            UpdateStyles();
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            Rectangle rect = ClientRectangle;
            rect.Width--;
            rect.Height--;
            var g = pevent.Graphics;
            Share.GraphicSetup(g);
            if (!macStyle)
            {
                using (SolidBrush sb = new SolidBrush(bgColor))
                {
                    g.FillPath(sb, rectPath);
                }
            }
            else
            {
                using (LinearGradientBrush lgb = new LinearGradientBrush(rect, topColor, bottomColor, 90))
                {
                    g.FillRectangle(lgb, rect);
                }
            }
            if (showBorder)
            {
                using (Pen p = new Pen(Share.BorderColor))
                {
                    g.DrawPath(p, rectPath);
                }
            }
            rectPath.Dispose();
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (!base.DesignMode)
            {
                if (canMoveParent || canMoveSelf)
                {
                    IntPtr handle = canMoveParent ? Tools.GetParent(this).Handle : (canMoveSelf ? this.Handle : IntPtr.Zero);
                    //释放鼠标焦点捕获
                    NativeMethods.ReleaseCapture();
                    //向当前窗体发送拖动消息
                    NativeMethods.SendMessage(handle, 0x0112, 0xF011, 0);
                }
            }
        }
        //protected override void WndProc(ref Message m)
        //{
        //    var p = new Point(m.LParam.ToInt32());
        //    p = PointToClient(p);
        //    switch (m.Msg)
        //    {
        //        case 0x0203:
        //        case 0x00A3:

        //            break; //干掉鼠标左键双击
        //        case 0x00A5: //鼠标右键

        //            break;
        //        case 0x84: //拖动
        //            if (TabStop && canMove)
        //                m.Result = new IntPtr(2);
        //            else
        //                base.WndProc(ref m);
        //            break;
        //        default:
        //            base.WndProc(ref m);
        //            break;
        //    }
        //}

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;
        //        return cp;
        //    }
        //}

        //protected override void OnLocationChanged(EventArgs e)
        //{
        //    if (Created && !macStyle)
        //    {
        //        DrawHelper.SetWindowRegion(this, Share.DefaultRadius);
        //    }
        //    base.OnLocationChanged(e);
        //}

        //protected override void OnResize(EventArgs e)
        //{
        //    //if (!macStyle)
        //    //{
        //    DrawHelper.SetWindowRegion(this, Share.DefaultRadius / 2);
        //    //}
        //    base.OnResize(e);
        //}
    }
}