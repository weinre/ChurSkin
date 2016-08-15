using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using Win32;
using Win32.Consts; 

namespace System.Windows.Forms
{
    internal class SkinForm : Form
    {
        /// <summary>
        ///     Required designer variable.
        /// </summary>
        private readonly IContainer components = null;

        //控件层
        private readonly SkinMain Main;
        //带参构造
        public SkinForm(SkinMain main)
        {
            InitializeComponent();
            //将控制层传值过来
            Main = main;
            //减少闪烁
            SetStyles();
            //初始化
            Init();
        }

        #region 初始化

        private void Init()
        {
            //最顶层
            TopMost = Main.TopMost;
            //是否在任务栏显示
            ShowInTaskbar = Main.SkinShowInTaskbar;
            //无边框模式
            FormBorderStyle = FormBorderStyle.None;
            //自动拉伸背景图以适应窗口
            BackgroundImageLayout = ImageLayout.Stretch;
            //还原任务栏右键菜单
            CommonClass.SetTaskMenu(this);
            //设置绘图层显示位置
            Location = new Point(Main.Location.X - Main.MainPosition.X, Main.Location.Y - Main.MainPosition.Y);
            //设置ICO
            Icon = Main.Icon;
            //是否显示ICO
            ShowIcon = Main.ShowIcon;
            //设置大小
            Size = Main.SkinSize;
            //设置标题名
            Text = Main.Text;
            //设置背景
            var bitmaps = new Bitmap(Main.SkinBack, Size);
            if (Main.SkinTrankColor != Color.Transparent)
            {
                bitmaps.MakeTransparent(Main.SkinTrankColor);
            }
            BackgroundImage = bitmaps;
            //控制层与绘图层合为一体
            Main.TransparencyKey = Main.SkinWhetherTank ? Color.Empty : Main.BackColor;
            Main.Owner = this;
            //绘制层窗体移动
            MouseDown += Frm_MouseDown;
            MouseMove += Frm_MouseMove;
            MouseUp += Frm_MouseUp;
            LocationChanged += Frm_LocationChanged;
            //控制层层窗体移动
            Main.MouseDown += Frm_MouseDown;
            Main.MouseMove += Frm_MouseMove;
            Main.MouseUp += Frm_MouseUp;
            Main.LocationChanged += Frm_LocationChanged;
        }

        #endregion

        #region 减少闪烁

        private void SetStyles()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer, true);
            //强制分配样式重新应用到控件上
            UpdateStyles();
            AutoScaleMode = AutoScaleMode.None;
        }

        #endregion

        #region 不规则无毛边方法

        public void SetBits()
        {
            if (BackgroundImage != null)
            {
                //绘制绘图层背景
                var bitmap = new Bitmap(BackgroundImage, Width, Height);
                if (!Image.IsCanonicalPixelFormat(bitmap.PixelFormat) || !Image.IsAlphaPixelFormat(bitmap.PixelFormat))
                    throw new ApplicationException("图片必须是32位带Alhpa通道的图片。");
                var oldBits = IntPtr.Zero;
                var screenDC = NativeMethods.GetDC(IntPtr.Zero);
                var hBitmap = IntPtr.Zero;
                var memDc = NativeMethods.CreateCompatibleDC(screenDC);

                try
                {
                    var topLoc = new Point(Left, Top);
                    var bitMapSize = new Size(Width, Height);
                    var blendFunc = new Win32.Struct.BLENDFUNCTION();
                    var srcLoc = new Point(0, 0);

                    hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                    oldBits = NativeMethods.SelectObject(memDc, hBitmap);

                    blendFunc.BlendOp = AC.AC_SRC_OVER;
                    blendFunc.SourceConstantAlpha = byte.Parse(Main.SkinOpacity.ToString());
                    blendFunc.AlphaFormat = AC.AC_SRC_ALPHA;
                    blendFunc.BlendFlags = 0;

                    NativeMethods.UpdateLayeredWindow(Handle, screenDC, ref topLoc, ref bitMapSize, memDc, ref srcLoc, 0,
                        ref blendFunc, ULW.ULW_ALPHA);
                }
                finally
                {
                    if (hBitmap != IntPtr.Zero)
                    {
                        NativeMethods.SelectObject(memDc, oldBits);
                        NativeMethods.DeleteObject(hBitmap);
                    }
                    NativeMethods.ReleaseDC(IntPtr.Zero, screenDC);
                    NativeMethods.DeleteDC(memDc);
                }
            }
        }

        #endregion

        /// <summary>
        ///     Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///     Required method for Designer support - do not modify
        ///     the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // SkinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 271);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SkinForm";
            this.Text = "SkinForm";
            this.ResumeLayout(false);
        }

        #endregion

        #region 无标题栏的窗口移动

        private Point mouseOffset; //记录鼠标指针的坐标
        private bool isMouseDown; //记录鼠标按键是否按下
        //窗体按下时
        private void Frm_MouseDown(object sender, MouseEventArgs e)
        {
            if (Main.SkinMobile)
            {
                int xOffset;
                int yOffset;
                //点击窗体时，记录鼠标位置，启动移动
                if (e.Button == MouseButtons.Left)
                {
                    xOffset = -e.X;
                    yOffset = -e.Y;
                    mouseOffset = new Point(xOffset, yOffset);
                    isMouseDown = true;
                }
            }
        }

        //窗体移动时
        private void Frm_MouseMove(object sender, MouseEventArgs e)
        {
            if (Main.SkinMobile)
            {
                //将调用此事件的窗口保存下
                var frm = (Form) sender;
                //确定开启了移动模式后
                if (isMouseDown)
                {
                    //移动的位置计算
                    var mousePos = MousePosition;
                    mousePos.Offset(mouseOffset.X, mouseOffset.Y);
                    //判断是绘图层还是控件层调用了移动事件,并作出相应回馈
                    if (frm == this)
                    {
                        Location = mousePos;
                    }
                    else
                    {
                        Main.Location = mousePos;
                    }
                }
            }
        }

        //窗体按下并释放按钮时
        private void Frm_MouseUp(object sender, MouseEventArgs e)
        {
            if (Main.SkinMobile)
            {
                // 修改鼠标状态isMouseDown的值
                // 确保只有鼠标左键按下并移动时，才移动窗体
                if (e.Button == MouseButtons.Left)
                {
                    //松开鼠标时，停止移动
                    isMouseDown = false;
                    //Top高度小于0的时候，等于0
                    if (Top < 0)
                    {
                        Top = 0;
                        Main.Top = 0 + Main.MainPosition.Y;
                    }
                }
            }
        }

        //窗口移动时
        private void Frm_LocationChanged(object sender, EventArgs e)
        {
            //将调用此事件的窗口保存下
            var frm = (Form) sender;
            if (frm == this)
            {
                Main.Location = new Point(Left + Main.MainPosition.X, Top + Main.MainPosition.Y);
            }
            else
            {
                Location = new Point(Main.Left - Main.MainPosition.X, Main.Top - Main.MainPosition.Y);
            }
        }

        #endregion

        #region 还原任务栏右键菜单

        protected override CreateParams CreateParams
        {
            get
            {
                var cParms = base.CreateParams;
                cParms.ExStyle |= 0x00080000; // WS_EX_LAYERED
                return cParms;
            }
        }

        public class CommonClass
        {
            public const int WS_SYSMENU = 0x00080000;
            public const int WS_MINIMIZEBOX = 0x20000;

            [DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
            private static extern int GetWindowLong(HandleRef hWnd, int nIndex);

            [DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
            private static extern IntPtr SetWindowLong(HandleRef hWnd, int nIndex, int dwNewLong);

            public static void SetTaskMenu(Form form)
            {
                var windowLong = (GetWindowLong(new HandleRef(form, form.Handle), -16));
                SetWindowLong(new HandleRef(form, form.Handle), -16, windowLong | WS_SYSMENU | WS_MINIMIZEBOX);
            }
        }

        #endregion

        #region 重载背景与拉伸更改时事件

        protected override void OnBackgroundImageChanged(EventArgs e)
        {
            base.OnBackgroundImageChanged(e);
            SetBits();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SetBits();
        }

        #endregion
    }
}