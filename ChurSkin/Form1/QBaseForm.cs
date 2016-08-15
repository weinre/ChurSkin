using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using Win32;
using Struct = Win32.Struct;

namespace System.Windows.Forms
{
    public class QBaseForm : Form
    {
        private bool onLoaded;
        private bool isQWindowForm;
        private bool enabledDWM;
        private Struct.DWM_BLURBEHIND dwm_BLURBEHIND;

        public QBaseForm()
        {
            this.dwm_BLURBEHIND = new Struct.DWM_BLURBEHIND();
            this.isQWindowForm = true;
        }

        public void CloseDwm()
        {
            if (NativeMethods.DwmIsCompositionEnabled())
            {
                this.dwm_BLURBEHIND.fEnable = false;
                NativeMethods.DwmEnableBlurBehindWindow(base.Handle.ToInt32(), ref this.dwm_BLURBEHIND);
                this.enabledDWM = false;
            }
        }

        public void EnabledDwm()
        {
            if (NativeMethods.DwmIsCompositionEnabled())
            {
                this.dwm_BLURBEHIND.dwFlags = 3;
                this.dwm_BLURBEHIND.fEnable = true;
                this.dwm_BLURBEHIND.hRegionBlur = IntPtr.Zero;
                NativeMethods.DwmEnableBlurBehindWindow(base.Handle.ToInt32(), ref this.dwm_BLURBEHIND);
                this.enabledDWM = true;
            }
        }

        private void method_0(IntPtr intptr_0, ref Point point_0, ref Size size_0, IntPtr intptr_1, ref Point point_1, ref Struct.BLENDFUNCTION blendfunction_0)
        {
            NativeMethods.UpdateWindow(base.Handle, intptr_0, ref point_0, ref size_0, intptr_1, ref point_1, 0, ref blendfunction_0, 2);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.enabledDWM)
            {
                this.EnabledDwm();
            }
            this.onLoaded = true;
        }

        public virtual void UpdateWindow(Bitmap bitmap)
        {
            this.UpdateWindow(bitmap, 1.0);
        }

        public virtual void UpdateWindow(Bitmap bitmap, double opacity)
        {
            if (bitmap == null) return;
            if (Image.IsCanonicalPixelFormat(bitmap.PixelFormat) && Image.IsAlphaPixelFormat(bitmap.PixelFormat))
            {
                IntPtr zero = IntPtr.Zero;
                IntPtr dC = NativeMethods.GetDC(IntPtr.Zero);
                IntPtr hObj = IntPtr.Zero;
                IntPtr hDC = NativeMethods.CreateCompatibleDC(dC);
                try
                {
                    Point pptDst = new Point(base.Left, base.Top);
                    Size psize = new Size(bitmap.Width, bitmap.Height);
                    Struct.BLENDFUNCTION pblend = new Struct.BLENDFUNCTION();
                    Point pptSrc = new Point(0, 0);
                    hObj = bitmap.GetHbitmap(Color.FromArgb(0));
                    zero = NativeMethods.SelectObject(hDC, hObj);
                    pblend.BlendOp = 0;
                    pblend.SourceConstantAlpha = (byte)((int)(255.0 * opacity));
                    pblend.AlphaFormat = 1;
                    pblend.BlendFlags = 0;
                    NativeMethods.UpdateWindow(base.Handle, dC, ref pptDst, ref psize, hDC, ref pptSrc, 0, ref pblend, 2);
                    return;
                }
                finally
                {
                    if (hObj != IntPtr.Zero)
                    {
                        NativeMethods.SelectObject(hDC, zero);
                        NativeMethods.DeleteObject(hObj);
                    }
                    NativeMethods.ReleaseDC(IntPtr.Zero, dC);
                    NativeMethods.DeleteDC(hDC);
                }
            }
            throw new ApplicationException("图片必须是32位带Alhpa通道的图片。");
        }

        [DefaultValue(false), Description("启用AeroGlass毛玻璃效果，需要系统支持并且需要将背景色设置为半透明色")]
        public bool EnabledDWM
        {
            get
            {
                return this.enabledDWM;
            }
            set
            {
                if (NativeMethods.DwmIsCompositionEnabled() && value)
                {
                    if (this.onLoaded)
                    {
                        this.EnabledDwm();
                    }
                    this.enabledDWM = true;
                }
                else
                {
                    if (this.onLoaded)
                    {
                        this.CloseDwm();
                    }
                    this.enabledDWM = false;
                }
            }
        }

        protected override System.Windows.Forms.CreateParams CreateParams
        {
            get
            {
                System.Windows.Forms.CreateParams createParams = base.CreateParams;
                createParams.Style |= 0x20000;
                if (!base.DesignMode && this.isQWindowForm)
                {
                    createParams.ExStyle |= 0x80000;
                }
                return createParams;
            }
        }

        [Description("是否为QWindow层窗体，为true时，可实现任意透明。")]
        public bool IsQWindowForm
        {
            get
            {
                return this.isQWindowForm;
            }
            set
            {
                this.isQWindowForm = value;
            }
        }

        private delegate void Delegate4(IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pptSrc, ref Struct.BLENDFUNCTION pblend);
    }
    public interface IQ
    {
        event EventHandler<PaintEventArgs> QPaintEvent;

        void DisposeCanvas();

        Bitmap Canvas { get; set; }

        ImageAttributes ImageAttribute { get; set; }
    }
}

