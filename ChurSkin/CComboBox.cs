using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Win32;
using Win32.Consts;

namespace ChurSkins
{
    [ToolboxBitmap(typeof(ComboBox))]
    public partial class CComboBox : ComboBox
    {
        public CComboBox()
            : base()
        {
            //InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            //  this.UpdateStyles();
        }
        private bool _bPainting;
        private IntPtr _editHandle;
        private ControlState _buttonState;
        Color bgColor = Color.FromArgb(180, 180, 180);//{ get { return Color.FromArgb((int)alpha, bgColor.R, bgColor.G, bgColor.B); } }
        Color bdColor = Color.FromArgb(130, 130, 130);//{ get { return Color.FromArgb((int)alpha, bgColor.R, bgColor.G, bgColor.B); } }
        internal ControlState ButtonState
        {
            get { return _buttonState; }
            set
            {
                if (_buttonState != value)
                {
                    _buttonState = value;
                    Invalidate(ButtonRect);
                }
            }
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.Region = new Region(GetPath(ClientRectangle));
        }
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            //设置为手动绘制
            this.DrawMode = DrawMode.OwnerDrawFixed;
            //设置固定的DropDownList样式
            this.DropDownStyle = ComboBoxStyle.DropDownList;
            this.ItemHeight = 25;
            //设置为手动绘制
            if (!DesignMode && this.Items.Count != 0)
            {
                this.DropDownHeight = this.Items.Count * 5;
                if (this.DropDownHeight > 150)
                    this.DropDownHeight = 150;
            }
            NativeMethods.ComboBoxInfo cbi = GetComboBoxInfo();
            _editHandle = cbi.hwndEdit;
            dplisthandel = cbi.hwndList;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM.WM_PAINT:
                    WmPaint(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point point = e.Location;
            if (ButtonRect.Contains(point))
            {
                ButtonState = ControlState.Hover;
            }
            else
            {
                ButtonState = ControlState.Normal;
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            Point point = PointToClient(Cursor.Position);
            if (ButtonRect.Contains(point))
            {
                ButtonState = ControlState.Hover;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            ButtonState = ControlState.Normal;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            ButtonState = ControlState.Normal;
        }

        private void WmPaint(ref Message m)
        {
            if (base.DropDownStyle == ComboBoxStyle.Simple)
            {
                base.WndProc(ref m);
                return;
            }

            if (base.DropDownStyle == ComboBoxStyle.DropDown)
            {
                if (!_bPainting)
                {
                    Win32.Struct.PAINTSTRUCT ps = new Win32.Struct.PAINTSTRUCT();

                    _bPainting = true;
                    NativeMethods.BeginPaint(m.HWnd, ref ps);

                    RenderComboBox(ref m);

                    NativeMethods.EndPaint(m.HWnd, ref ps);
                    _bPainting = false;
                    m.Result = Win32.Consts.Result.TRUE;
                }
                else
                {
                    base.WndProc(ref m);
                }
            }
            else
            {
                base.WndProc(ref m);
                RenderComboBox(ref m);
            }
        }
        private void RenderComboBox(ref Message m)
        {
            Rectangle rect = new Rectangle(Point.Empty, Size);
            Rectangle buttonRect = ButtonRect;
            ControlState state = ButtonPressed ? ControlState.Pressed : ButtonState;
            using (Graphics g = Graphics.FromHwnd(m.HWnd))
            {
                RenderComboBoxBackground(g, rect, buttonRect);
                RenderConboBoxDropDownButton(g, ButtonRect, state);
                RenderConboBoxBorder(g, rect);
            }
        }
        GraphicsPath GetPath(Rectangle rect)
        {
            return DrawHelper.CreateRoundPath(rect.Width, rect.Height, 2);
        }
        void RenderComboBoxBackground(Graphics g, Rectangle rect, Rectangle buttonRect)
        {
        //    using (SolidBrush brush = new SolidBrush(bgColor))
        //    {
        //        // buttonRect.Inflate(-1, -1);
        //        //rect.Inflate(-1, -1);
        //        using (Region region = new Region(rect))
        //        {
        //            region.Exclude(buttonRect);
        //            region.Exclude(EditRect);
        //            g.FillRegion(brush, region);
        //        }
        //    }
            //StringFormat sf = new StringFormat();
            //sf.Alignment = StringAlignment.Near;
            //sf.LineAlignment = StringAlignment.Center;
            //g.DrawString(this.Text, Font, Brushes.Black, rect, sf);
            g.FillPath(new SolidBrush(bgColor), GetPath(rect));
        }
        void RenderConboBoxDropDownButton(Graphics g, Rectangle rect, ControlState state)
        {
            rect.Width = 19;
            rect.X = Width - 19;
            rect.Height = 22;
            rect.Y = (Height - 22) / 2;
            if (state == ControlState.Pressed)
            {
                rect.X++;
                rect.Y++;
            }
            g.DrawImage(Properties.Resources.inputbtn_highlight, rect);
        }
        void RenderConboBoxBorder(Graphics g, Rectangle rect)
        {
            rect.Width -= 1;
            rect.Height -= 1;
            g.DrawPath(new Pen(bdColor), GetPath(rect));
        }
        internal Rectangle ButtonRect
        {
            get
            {
                return GetDropDownButtonRect();
            }
        }

        internal bool ButtonPressed
        {
            get
            {
                if (IsHandleCreated)
                {
                    return GetComboBoxButtonPressed();
                }
                return false;
            }
        }
        internal IntPtr EditHandle
        {
            get { return _editHandle; }
        }
        IntPtr dplisthandel;
        internal Rectangle dplistrect
        {
            get
            {
                if (dplisthandel != IntPtr.Zero)
                {
                    Win32.Struct.RECT rcClient = new Win32.Struct.RECT();
                    NativeMethods.GetWindowRect(dplisthandel, ref rcClient);
                    return RectangleToClient(rcClient.Rect);
                } return Rectangle.Empty;
            }
        }

        internal Rectangle EditRect
        {
            get
            {
                if (DropDownStyle == ComboBoxStyle.DropDownList)
                {
                    Rectangle rect = new Rectangle(
                        3, 3, Width - ButtonRect.Width - 6, Height - 6);
                    if (RightToLeft == RightToLeft.Yes)
                    {
                        rect.X += ButtonRect.Right;
                    }
                    return rect;
                }
                if (IsHandleCreated && EditHandle != IntPtr.Zero)
                {
                    Win32.Struct.RECT rcClient = new Win32.Struct.RECT();
                    NativeMethods.GetWindowRect(EditHandle, ref rcClient);
                    return RectangleToClient(rcClient.Rect);
                }
                return Rectangle.Empty;
            }
        }
        private bool GetComboBoxButtonPressed()
        {
            NativeMethods.ComboBoxInfo cbi = GetComboBoxInfo();
            return cbi.stateButton ==
                NativeMethods.ComboBoxButtonState.STATE_SYSTEM_PRESSED;
        }

        private NativeMethods.ComboBoxInfo GetComboBoxInfo()
        {
            NativeMethods.ComboBoxInfo cbi = new NativeMethods.ComboBoxInfo();
            cbi.cbSize = Marshal.SizeOf(cbi);
            NativeMethods.GetComboBoxInfo(base.Handle, ref cbi);
            return cbi;
        }

        private Rectangle GetDropDownButtonRect()
        {
            NativeMethods.ComboBoxInfo cbi = GetComboBoxInfo();
            return cbi.rcButton.Rect;
        }

        protected override void OnDropDown(EventArgs e)
        {
            base.OnDropDown(e);

            this.DropDownWidth = 20;//计算下拉框的总宽度  
        }
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            //绘制区域
            Rectangle r = e.Bounds;
            if (e.Index >= 0)
            {
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) //鼠标划过
                {
                    g.FillRectangle(new SolidBrush(bdColor), r);
                }
                else
                {
                    /*
                     Color backColor;  
            if (e.Index % 2 == 0) //偶数项  
            {  
                backColor = _rowBackColor2;  
            }  
            else //奇数项  
            {  
                backColor = _rowBackColor1;  
            }  
            using (SolidBrush brush = new SolidBrush(backColor))  
            {  
                g.FillRectangle(brush, e.Bounds);  
            } 
                     */
                    g.FillRectangle(new SolidBrush(bgColor), r);

                }
                StringFormat sf = new StringFormat();
                sf.LineAlignment = StringAlignment.Center;
                sf.Alignment = StringAlignment.Near;
                g.DrawString(this.Items[e.Index].ToString(), Font, Brushes.Black, r, sf);
                //Console.WriteLine(e.State);
            }
        }
        /*
        //protected override void WndProc(ref Message m)
        //{
        //    IntPtr hDC = IntPtr.Zero;
        //    Graphics gdc = null;
        //    Console.WriteLine(m.Msg);
        //    if ( m.Msg == 32 || m.Msg == 132)
        //    {
        //       // base.WndProc(ref m);
        //        hDC = NativeMethods.GetWindowDC(m.HWnd);
        //        gdc = Graphics.FromHdc(hDC);
        //        gdc.DrawRectangle(new Pen(Brushes.Blue, 2), new Rectangle(Width - 20, 0, 20, Height));
        //        return;
        //    }
        //    else
        //        base.WndProc(ref m);
        //    //switch (m.Msg)
        //    //{
        //    //    case 133:
        //    //        //hDC = NativeMethods.GetWindowDC(m.HWnd);
        //    //        //gdc = Graphics.FromHdc(hDC);
        //    //        //NativeMethods.SendMessage(this.Handle, WM.WM_ERASEBKGND, hDC.ToInt32(), 0);
        //    //        //SendPrintClientMsg();
        //    //        //NativeMethods.SendMessage(this.Handle, WM.WM_PAINT, 0, 0);
        //    //        //OverrideControlBorder(gdc);
        //    //        //m.Result = (IntPtr)1;    // indicate msg has been processed
        //    //        //NativeMethods.ReleaseDC(m.HWnd, hDC);
        //    //        //gdc.Dispose();
        //    //        break;
        //    //    case WM.WM_PAINT:
        //    //        //base.WndProc(ref m);
        //    //        //  hDC = NativeMethods.GetWindowDC(m.HWnd);
        //    //        //  gdc = Graphics.FromHdc(hDC);

        //    //        // OverrideDropDown(gdc);
        //    //        // OverrideControlBorder(gdc); //画边框
        //    //        // NativeMethods.ReleaseDC(m.HWnd, hDC);
        //    //        // gdc.Dispose();
        //    //        break;
        //    //    default:
        //    //        base.WndProc(ref m);
        //    //        break;
        //    //}
        //}

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            //绘制区域
            Rectangle r = e.Bounds;

            Font fn = null;
            if (e.Index >= 0)
            {
                //Console.WriteLine(e.State);
                if (e.State == DrawItemState.None)
                {
                    Console.WriteLine("1----------");
                    //设置字体、字符串格式、对齐方式
                    fn = e.Font;
                    string s = this.Items[e.Index].ToString();
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Center;
                    //根据不同的状态用不同的颜色表示
                    if (e.State == (DrawItemState.NoAccelerator | DrawItemState.NoFocusRect))
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.Red), r);
                        e.Graphics.DrawString(s, fn, new SolidBrush(Color.Black), r, sf);
                        e.DrawFocusRectangle();
                        Console.WriteLine("3----------");
                    }
                    else
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.White), r);
                        e.Graphics.DrawString(s, fn, new SolidBrush(Shared.FontColor), r, sf);
                        e.DrawFocusRectangle();
                        Console.WriteLine("4----------");
                    }
                }
                else
                {
                    //   Console.WriteLine("2----------");
                    fn = e.Font;
                    StringFormat sf = new StringFormat();
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Center;
                    string s = this.Items[e.Index].ToString();
                    e.Graphics.FillRectangle(new SolidBrush(Shared.ControlBackColor), r);
                    e.Graphics.DrawString(s, fn, new SolidBrush(Shared.FontColor), r, sf);
                }
            }
        }
        */
    }
}
