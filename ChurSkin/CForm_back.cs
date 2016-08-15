using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ChurSkins
{
    public partial class CForm : Form
    {
        public CForm()
            : base()
        {
            //System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo("en");
            //ApplyResource();

            InitializeComponent();
            this.SetStyle(
                             ControlStyles.AllPaintingInWmPaint |
                             ControlStyles.OptimizedDoubleBuffer |
                             ControlStyles.ResizeRedraw |
                // ControlStyles.Selectable |
                // ControlStyles.ContainerControl |
                             ControlStyles.UserPaint, true);
            // this.SetStyle(ControlStyles.Opaque, false);
            this.FormBorderStyle = FormBorderStyle.None;
            this.UpdateStyles();
        }
        //private void ApplyResource()
        //{
        //    System.ComponentModel.ComponentResourceManager res =
        //        new System.ComponentModel.ComponentResourceManager(this.GetType());
        //    foreach (Control ctl in Controls)
        //    {
        //        res.ApplyResources(ctl, ctl.Name);
        //    }
        //    res.ApplyResources(this, "$this");
        //}

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (this.WindowState == FormWindowState.Maximized)
            { maxBmp = Properties.Resources.btn_restore_normal; }
            else if (this.WindowState == FormWindowState.Normal)
            { maxBmp = Properties.Resources.btn_max_normal; }
            // GetRegion();
            DrawHelper.SetWindowRegion(this, 3);
            //DrawHelper.SetFormRoundRectRgn(this, 6);
        }
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value; this.Invalidate();
            }
        }
        protected virtual Rectangle closeRect
        {
            get { return new Rectangle(this.ClientRectangle.Right - 39, -1, 39, 20); }
        }
        protected virtual Rectangle maxRect
        {
            get
            {
                return new Rectangle(this.Width - this.closeRect.Width - 28, -1, 28, 20);
            }
        }
        protected virtual Rectangle minRect
        {
            get
            {
                int x = 0;
                {
                    if (MaximizeBox)
                        x = this.Width - this.closeRect.Width - this.maxRect.Width - 28;
                    else
                        x = this.Width - this.closeRect.Width - 28;
                    return new Rectangle(x, -1, 28, 20);
                }
            }
        }
        private Bitmap minBmp = Properties.Resources.btn_mini_normal;
        private Bitmap maxBmp = Properties.Resources.btn_max_normal;
        private Bitmap closeBmp = Properties.Resources.btn_close_normal;

        bool minMousedown = false;
        bool maxMousedown = false;
        bool closeMousedown = false;


        public new bool MaximizeBox { get { return base.MaximizeBox; } set { base.MaximizeBox = value; this.Invalidate(); } }
        public new bool MinimizeBox { get { return base.MinimizeBox; } set { base.MinimizeBox = value; this.Invalidate(); } }
        protected override void OnPaint(PaintEventArgs e)
        {
            //画最小按钮
            if (MinimizeBox)
            {
                e.Graphics.DrawImage(minBmp, minRect);
            }
            if (MaximizeBox)
            {
                e.Graphics.DrawImage(maxBmp, maxRect);
            }
            e.Graphics.DrawImage(closeBmp, closeRect);
            //minBmp.Dispose();
            //maxBmp.Dispose();
            //closeBmp.Dispose();
            //画标题
            // if (ShowTitle)
            //{
            Font font = new System.Drawing.Font("微软雅黑", 10, FontStyle.Bold);
            e.Graphics.DrawString(base.Text, font, Brushes.Black, 10, 5);
            // }


            font.Dispose();
            //画边框
            Bitmap bd = Properties.Resources.borders;
            DrawHelper.RenderFormBorder(bd, 8, e.Graphics, this.ClientRectangle);
            bd.Dispose();
            // base.OnPaint(e);

        }



        protected GraphicsPath gp;

        protected override void OnMouseDown(MouseEventArgs e)
        {

            base.OnMouseDown(e);
            if (MinimizeBox)
            {
                if (minRect.Contains(e.Location))
                {
                    minMousedown = true;
                    minBmp = Properties.Resources.btn_mini_down;
                }
                this.Invalidate(minRect);
            }
            if (MaximizeBox)
            {
                if (maxRect.Contains(e.Location))
                {
                    maxMousedown = true;
                    if (this.WindowState == FormWindowState.Normal)
                        maxBmp = Properties.Resources.btn_max_down;
                    else
                        maxBmp = Properties.Resources.btn_restore_down;
                }

                this.Invalidate(maxRect);
            }
            if (closeRect.Contains(e.Location))
            {
                closeMousedown = true;
                closeBmp = Properties.Resources.btn_close_down;
                this.Invalidate(closeRect);
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (MinimizeBox)
            {
                if (minMousedown)
                    minBmp = Properties.Resources.btn_mini_down;
                else
                {
                    if (minRect.Contains(e.Location))
                        minBmp = Properties.Resources.btn_mini_highlight;
                    else
                        minBmp = Properties.Resources.btn_mini_normal;
                }
                this.Invalidate(minRect);
            }
            if (MaximizeBox)
            {
                if (maxRect.Contains(e.Location))
                {
                    if (maxMousedown)
                        if (this.WindowState == FormWindowState.Normal)
                            maxBmp = Properties.Resources.btn_max_down;
                        else
                            maxBmp = Properties.Resources.btn_restore_down;
                    else
                        if (this.WindowState == FormWindowState.Normal)
                            maxBmp = Properties.Resources.btn_max_highlight;
                        else
                            maxBmp = Properties.Resources.btn_restore_highlight;
                }
                else
                {
                    if (maxMousedown)
                        if (this.WindowState == FormWindowState.Normal)
                            maxBmp = Properties.Resources.btn_max_down;
                        else
                            maxBmp = Properties.Resources.btn_restore_down;
                    else
                        if (this.WindowState == FormWindowState.Normal)
                            maxBmp = Properties.Resources.btn_max_normal;
                        else
                            maxBmp = Properties.Resources.btn_restore_normal;
                }

                this.Invalidate(maxRect);
            }
            if (closeMousedown)
                closeBmp = Properties.Resources.btn_close_down;
            else
                if (closeRect.Contains(e.Location))
                    closeBmp = Properties.Resources.btn_close_highlight;
                else
                    closeBmp = Properties.Resources.btn_close_normal;
            this.Invalidate(closeRect);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            minBmp = Properties.Resources.btn_mini_normal;
            closeBmp = Properties.Resources.btn_close_normal;
            if (WindowState == FormWindowState.Normal)
                maxBmp = Properties.Resources.btn_max_normal;
            if (WindowState == FormWindowState.Maximized)
                maxBmp = Properties.Resources.btn_restore_normal;
            this.Invalidate(minRect);
            this.Invalidate(closeRect);
            this.Invalidate(maxRect);
        }
        /// <summary>
        /// 有效解决窗体最大化最小化闪烁
        /// </summary>
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        if (!DesignMode)
        //        {
        //            cp.ExStyle |= 0x02000000;
        //        }
        //        return cp;

        //    }
        //}
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                if (!DesignMode)
                {
                    cp.Style |= 0x00040000;// (int)NativeMethods.WindowStyle.WS_THICKFRAME;

                    if (ControlBox)
                    {
                        cp.Style |= 0x00080000;// (int)NativeMethods.WindowStyle.WS_SYSMENU;
                    }

                    if (MinimizeBox)
                    {
                        cp.Style |= 0x00020000;// (int)NativeMethods.WindowStyle.WS_MINIMIZEBOX;
                    }

                    if (!MaximizeBox)
                    {
                        cp.Style &= ~0x00010000;// (int)NativeMethods.WindowStyle.WS_MAXIMIZEBOX;
                    }

                    if (_inPosChanged)
                    {
                        //cp.Style &= ~((int)NativeMethods.WindowStyle.WS_THICKFRAME |
                        //    (int)NativeMethods.WindowStyle.WS_SYSMENU);
                        //cp.ExStyle &= ~((int)NativeMethods.WindowStyleEx.WS_EX_DLGMODALFRAME |
                        //    (int)NativeMethods.WindowStyleEx.WS_EX_WINDOWEDGE);
                        cp.Style &= ~(0x00040000 | 0x00080000);
                        cp.ExStyle &= ~(0x00000001 | 0x00000100);
                    }
                }

                return cp;
            }
        }
        private bool _inPosChanged;
        protected override void OnMouseUp(MouseEventArgs e)
        {
            minMousedown = maxMousedown = closeMousedown = false;
            base.OnMouseUp(e);
            if (closeRect.Contains(e.Location)) { this.Close(); }
            else
            {
                closeBmp = Properties.Resources.btn_close_normal;
                this.Invalidate(closeRect);
            }
            if (MinimizeBox)
            {
                if (minRect.Contains(e.Location))
                {
                    this.SuspendLayout();
                    this.WindowState = FormWindowState.Minimized;
                    this.ResumeLayout(false);
                }
                minBmp = Properties.Resources.btn_mini_normal;
                this.Invalidate(minRect);
            }



            if (MaximizeBox)
            {
                if (maxRect.Contains(e.Location))
                {
                    if (this.WindowState == FormWindowState.Maximized)
                    {
                        this.SuspendLayout();
                        this.WindowState = FormWindowState.Normal;
                        this.ResumeLayout(false);
                        maxBmp = Properties.Resources.btn_max_normal;
                    }
                    else if (this.WindowState == FormWindowState.Normal)
                    {
                        this.SuspendLayout();
                        this.WindowState = FormWindowState.Maximized;
                        this.ResumeLayout(false);
                        maxBmp = Properties.Resources.btn_restore_normal;
                    }
                }
                else
                {
                    if (this.WindowState == FormWindowState.Maximized)
                    {
                        maxBmp = Properties.Resources.btn_restore_normal;
                    }
                    else if (this.WindowState == FormWindowState.Normal)
                    {
                        maxBmp = Properties.Resources.btn_max_normal;
                    }
                }
                this.Invalidate(maxRect);
            }
        }
        //[DllImport("user32.dll")]
        //private static extern IntPtr GetWindowDC(IntPtr hWnd);
        //[DllImport("user32.dll")]
        //private static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        //private const int WM_NCPAINT = 0x0085;
        //private const int WM_NCACTIVATE = 0x0086;
        //private const int WM_NCLBUTTONDOWN = 0x00A1;

        Rectangle controlBox { get { return new Rectangle(this.ClientRectangle.Right - 95, 0, 95, 30); } }
        protected override void WndProc(ref Message m)
        {
            //base.WndProc(ref m);
            //Rectangle vRectangle = new Rectangle((Width - 75) / 2, 3, 75, 25);
            //switch (m.Msg)
            //{
            //    case WM_NCPAINT: break;
            //    case WM_NCACTIVATE:
            //        break;
            //        //IntPtr vHandle = GetWindowDC(m.HWnd);
            //        //Graphics vGraphics = Graphics.FromHdc(vHandle);
            //        ////vGraphics.FillRectangle(new LinearGradientBrush(vRectangle,
            //        ////    Color.Pink, Color.Purple, LinearGradientMode.BackwardDiagonal),
            //        ////    vRectangle);

            //        //StringFormat vStringFormat = new StringFormat();
            //        //vStringFormat.Alignment = StringAlignment.Center;
            //        //vStringFormat.LineAlignment = StringAlignment.Center;
            //        ////vGraphics.DrawString("About", Font, Brushes.BlanchedAlmond,
            //        ////    vRectangle, vStringFormat);

            //        //vGraphics.Dispose();
            //        //ReleaseDC(m.HWnd, vHandle);
            //        //break;
            //    case WM_NCLBUTTONDOWN:
            //        Point vPoint = new Point((int)m.LParam);
            //        vPoint.Offset(-Left, -Top);
            //        if (vRectangle.Contains(vPoint))
            //            MessageBox.Show(vPoint.ToString());
            //        break;
            //}
            //
            switch (m.Msg)
            {
                //case 0x24: //要改变大小或位置
                //    if (WindowState == FormWindowState.Minimized) Height = 900;
                //    //WmGetMinMaxInfo(ref m);
                //    break;
                //case 0x0086://是否激活
                // case 0x0014: //当窗口背景必须被擦除时->WM_ERASEBKGND  //用来减少窗体闪烁
                //   m.Result = IntPtr.Zero;
                //break;
                case 0x0047://(int)NativeMethods.WindowMessages.WM_WINDOWPOSCHANGED:
                    _inPosChanged = true;
                    base.WndProc(ref m);
                    _inPosChanged = false;
                    break;
                case 0x0086: //WM_NCACTIVATE->此消息发送给某个窗口 仅当它的非客户区需要被改变来显示是激活还是非激活状态
                    WmNcActive(ref m);
                    break;
                case 0x0085: //画边框->WM_NCPAINT 
                case 0x0083://客户区->WM_NCCALCSIZE 
                    break;
                case 0x0024: //此消息发送给窗口当它将要改变大小或位置->(int)WM_GETMINMAXINFO:
                    WmGetMinMaxInfo(ref m);
                    break;
                case 0x0084: //移动鼠标，按住或释放鼠标时发生 ->拖动->WM_NCHITTEST
                    WmNcHitTest(ref m);
                    break;
                case 0x00A3:  //双击->WM_NCLBUTTONDBLCLK ->当用户双击鼠标左键同时光标某个窗口在非客户区十发送此消息
                    if (MaximizeBox)
                        base.WndProc(ref m);
                    else return;
                    break;
                default: base.WndProc(ref m);
                    break;
            }
        }
        private void WmNcActive(ref Message m)
        {
            //if (m.WParam.ToInt32() == 1)
            //{
            //    _active = true;
            //}
            //else
            //{
            //    _active = false;
            //}
            m.Result = new IntPtr(1);
            base.Invalidate();
        }
        public struct MINMAXINFO
        {
            public Point reserved;
            public Size maxSize;
            public Point maxPosition;
            public Size minTrackSize;
            public Size maxTrackSize;
        }
        private void WmGetMinMaxInfo(ref Message m)
        {
            MINMAXINFO minmax = (MINMAXINFO)Marshal.PtrToStructure(m.LParam, typeof(MINMAXINFO));

            if (MaximumSize != Size.Empty)
            {
                minmax.maxTrackSize = MaximumSize;
            }
            else
            {
                Rectangle rect = Screen.GetWorkingArea(this);
                minmax.maxPosition = new Point(
                    Math.Abs(rect.X),
                    Math.Abs(rect.Y));
                minmax.maxTrackSize = new Size(
                    rect.Width,
                    rect.Height);
            }

            if (MinimumSize != Size.Empty)
            {
                minmax.minTrackSize = MinimumSize;
            }
            else
            {
                minmax.minTrackSize = new Size(150, 39);
            }

            Marshal.StructureToPtr(minmax, m.LParam, false);
        }

        private void WmNcHitTest(ref Message m)
        {
            int x = (short)(m.LParam.ToInt32() & 0x0000FFFF);
            int y = (short)((m.LParam.ToInt32() & 0xFFFF0000) >> 16);
            Point point = new Point(x, y);
            point = PointToClient(point);
            #region 拉伸窗体
            if (MaximizeBox && WindowState != FormWindowState.Maximized)
            {
                if (point.X < 5 && point.Y < 5)
                {
                    m.Result = new IntPtr(13);//(int)NativeMethods.NCHITTEST.HTTOPLEFT);
                    return;
                }

                if (point.X > Width - 5 && point.Y < 5)
                {
                    m.Result = new IntPtr(14);//(int)NativeMethods.NCHITTEST.HTTOPRIGHT);
                    return;
                }

                if (point.X < 5 && point.Y > Height - 5)
                {
                    m.Result = new IntPtr(16);//(int)NativeMethods.NCHITTEST.HTBOTTOMLEFT);
                    return;
                }

                if (point.X > Width - 5 && point.Y > Height - 5)
                {
                    m.Result = new IntPtr(17);//(int)NativeMethods.NCHITTEST.HTBOTTOMRIGHT);
                    return;
                }

                if (point.Y < 3)
                {
                    m.Result = new IntPtr(12);//(int)NativeMethods.NCHITTEST.HTTOP);
                    return;
                }

                if (point.Y > Height - 3)
                {
                    m.Result = new IntPtr(15);//(int)NativeMethods.NCHITTEST.HTBOTTOM);
                    return;
                }

                if (point.X < 3)
                {
                    m.Result = new IntPtr(10);//(int)NativeMethods.NCHITTEST.HTLEFT);
                    return;
                }

                if (point.X > Width - 3)
                {
                    m.Result = new IntPtr(11);//(int)NativeMethods.NCHITTEST.HTRIGHT);
                    return;
                }
            }
            #endregion

            if (!controlBox.Contains(point))
            {
                m.Result = (IntPtr)2; new IntPtr(2);//(int)NativeMethods.NCHITTEST.HTCAPTION);
                return;
            }
            else
                m.Result = (IntPtr)1;//(int)NativeMethods.NCHITTEST.HTCLIENT);
        }
    }
}
