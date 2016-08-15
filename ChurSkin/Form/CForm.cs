using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Win32;
using Win32.Consts;
using Win32.Struct;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;

namespace System.Windows.Forms
{
    public partial class CForm : Form
    {
        public CForm()
        {
            InitializeComponent();

            _roundStyle = RoundStyle.All;
            //  _radius = Share.FormRadius;
            _captionHeight = 30; //0x18;
            _captionFont = Share.DefaultFont; //SystemFonts.CaptionFont;
                                              //  _miniSize = new Size(28, 20);
            sysBottomSize = new Size(0x1c, 20);
            //  _maxBoxSize = new Size(28, 20);
            //  _closeBoxSize = new Size(39, 20);
            _controlBoxOffset = new Point(0, -1);
            CanResize = true;
            _mobile = MobileStyle.Mobile;
            _borderPadding = new Padding(4);
            skinOpacity = 1.0;
            backLayout = true;
            showborder = true;
            special = true;
            shadow = Share.FormShadown;
            shadowColor = Share.FormShadownColor;
            shadowWidth = Share.FormShadownWidth;
            backrectangle = new Rectangle(10, 10, 10, 10);
            borderrectangle = new Rectangle(10, 10, 10, 10);
            backtocolor = false;
            backshade = true;
            effectcaption = true;
            effectback = Color.White;
            titleColor = Color.Black;
            effectWidth = 6;
            dropback = true;
            OneVisibles = true;
            BackColor = Share.FormBackColor;
            SetStyles();
            Init();
        }

        //protected override void OnSizeChanged(EventArgs e)
        //{
        //    base.OnSizeChanged(e);
        //    if (WindowState == FormWindowState.Maximized)
        //    {
        //        SuspendLayout();
        //        WindowState = FormWindowState.Maximized;
        //        ResumeLayout(false);
        //    }
        //    else if (WindowState == FormWindowState.Minimized)
        //    {
        //        SuspendLayout();
        //        WindowState = FormWindowState.Minimized;
        //        ResumeLayout(false);
        //    }
        //    else if (WindowState == FormWindowState.Normal)
        //    {
        //        SuspendLayout();
        //        WindowState = FormWindowState.Normal;
        //        ResumeLayout(false);
        //    }
        //}
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                if (!DesignMode)
                    cp.ExStyle |= 0x02000000;   //不是设计模式才启用，不然设计器会显示不正常
                return cp;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (msg.Msg == 0x100 || msg.Msg == 0x104) // WM_KEYDOWN, WM_SYSKEYDOWN  
            {
                if (keyData == Keys.Escape)
                {
                    Close(); // Esc关闭窗体  
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Init()
        {
            ToolTip = new ToolTip();
            base.FormBorderStyle = FormBorderStyle.Sizable;
            BackgroundImageLayout = ImageLayout.None;
            Renderer.InitSkinForm(this);
            base.Padding = DefaultPadding;
        }

        #region 私有变量

        private bool _active;
        private Padding _borderPadding;
        private Font _captionFont = Share.DefaultFont;
        private int _captionHeight;
        private bool _clientSizeSet;
        private SkinFormColorTable _colorTable;
        private ControlBoxManager _controlBoxManager;
        private Point _controlBoxOffset;
        private int _controlBoxSpace;
        private Rectangle _deltaRect;
        private int _inWmWindowPosChanged;
        private MobileStyle _mobile;
        private Padding _padding;
        private int _radius;
        private SkinFormRenderer _renderer;
        private RoundStyle _roundStyle;
        public AnchorStyles Aanhor;
        private Image back;
        private bool backLayout;
        private Image backpalace;
        private Rectangle backrectangle;
        private bool backshade;
        private bool backtocolor;
        private Image borderpalace;
        private Rectangle borderrectangle;
        private bool dropback;
        private Color effectback;
        private bool effectcaption;
        private int effectWidth;
        private static readonly object EventRendererChanged = null;
        public bool isMouseDown;
        private bool OneVisibles;
        private bool shadow;
        private Color shadowColor;
        private int shadowWidth;
        private bool showborder;
       //ublic BaseForm skin;
        private double skinOpacity;
        private bool special;
        private Image sysBottomDown;
        private Image sysBottomMouse;
        private Image sysBottomNorml;
        private Size sysBottomSize;
        private string sysBottomToolTip;
        private bool sysBottomVisibale;
        private Color titleColor;
        private bool titleSuitColor;
        private bool backStruk = true;
        #endregion

        #region 共有变量

        [Description("背景是否拉伸"), DefaultValue(true)]
        public bool BackStruk
        {
            get { return backStruk; }
            set { backStruk = value; }
        }

        [Description("背景"), Category("Skin")]
        public Image Back
        {
            get { return back; }
            set
            {
                if (back != value)
                {
                    back = value;
                    if (BackToColor && (back != null))
                    {
                        BackColor = BitmapHelper.GetImageAverageColor((Bitmap)back);
                    }
                    Invalidate();
                    OnBackChanged(new BackEventArgs(back, value));
                }
            }
        }

        [Category("Skin"), Description("是否从左绘制背景")]
        public bool BackLayout
        {
            get { return backLayout; }
            set
            {
                if (backLayout != value)
                {
                    backLayout = value;
                    Invalidate();
                }
            }
        }

        [Category("Skin"), Description("质感层背景")]
        public Image BackPalace
        {
            get { return backpalace; }
            set
            {
                if (backpalace != value)
                {
                    backpalace = value;
                    Invalidate();
                }
            }
        }

        [Category("Skin"), Description("质感层九宫绘画区域"), DefaultValue(typeof(Rectangle), "10,10,10,10")]
        public Rectangle BackRectangle
        {
            get { return backrectangle; }
            set
            {
                if (backrectangle != value)
                {
                    backrectangle = value;
                    Invalidate();
                }
            }
        }

        [Description("是否加入背景渐变效果"), DefaultValue(true), Category("Skin")]
        public bool BackShade
        {
            get { return backshade; }
            set
            {
                if (backshade != value)
                {
                    backshade = value;
                    Invalidate();
                }
            }
        }

        [DefaultValue(true), Description("是否根据背景图决定背景色"), Category("Skin")]
        public bool BackToColor
        {
            get { return backtocolor; }
            set
            {
                if (backtocolor != value)
                {
                    backtocolor = value;
                    Invalidate();
                }
            }
        }

        // protected internal System.Windows.Forms.Padding BorderPadding
        public Padding BorderPadding
        {
            get { return _borderPadding; }
            set
            {
                _borderPadding = value;
                Refresh();
            }
        }

        [Description("边框层背景"), Category("Skin")]
        public Image BorderPalace
        {
            get
            {
                // this.borderpalace = null;
                if (borderpalace == null)
                    borderpalace = Properties.Resources.BackPalace;
                return borderpalace;
            }
            set
            {
                if (borderpalace != value)
                {
                    borderpalace = value;
                    Invalidate();
                }
            }
        }

        [Description("边框质感层九宫绘画区域"), DefaultValue(typeof(Rectangle), "10,10,10,10"), Category("Skin")]
        public Rectangle BorderRectangle
        {
            get { return borderrectangle; }
            set
            {
                if (borderrectangle != value)
                {
                    borderrectangle = value;
                    Invalidate();
                }
            }
        }

        [Category("Skin"), Description("设置或获取窗体是否可以改变大小"), DefaultValue(true)]
        public bool CanResize { get; set; }

        [Category("Caption"), Description("设置或获取窗体标题的字体"), DefaultValue(typeof(Font), "CaptionFont")]
        public Font CaptionFont
        {
            get { return _captionFont; }
            set
            {
                if (value == null)
                {
                    _captionFont = SystemFonts.CaptionFont;
                }
                else
                {
                    _captionFont = value;
                }
                Invalidate(CaptionRect);
            }
        }

        [Category("Skin"), DefaultValue(0x18), Description("设置或获取窗体标题栏的高度"), Browsable(false)]
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
        [Browsable(false)]
        public Rectangle CaptionRect
        {
            get { return new Rectangle(0, 0, Width, CaptionHeight); }
        }
        [Browsable(false)]
        public SkinFormColorTable Colortable
        {
            get
            {
                if (_colorTable == null)
                {
                    _colorTable = new SkinFormColorTable();
                }
                return _colorTable;
            }
            set { _colorTable = value; }
        }

        [Category("Skin"), Description("系统按钮激活时色调颜色"), DefaultValue(typeof(Color), "51, 153, 204")]
        public Color ControlBoxActive
        {
            get { return Colortable.ControlBoxActive; }
            set
            {
                Colortable.ControlBoxActive = value;
                Renderer = new SkinFormProfessionalRenderer(Colortable);
            }
        }

        [DefaultValue(typeof(Color), "88, 172, 218"), Category("Skin"), Browsable(false), Description("系统按钮停用时色调颜色")]
        public Color ControlBoxDeactive
        {
            get { return Colortable.ControlBoxDeactive; }
            set
            {
                Colortable.ControlBoxDeactive = value;
                Renderer = new SkinFormProfessionalRenderer(Colortable);
            }
        }
        [Browsable(false)]
        public ControlBoxManager ControlBoxManager
        {
            get
            {
                if (_controlBoxManager == null)
                {
                    _controlBoxManager = new ControlBoxManager(this);
                }
                return _controlBoxManager;
            }
        }

        [Category("Skin"), Description("设置或获取控制按钮的偏移"), DefaultValue(typeof(Point), "6, 0")]
        public Point ControlBoxOffset
        {
            get { return _controlBoxOffset; }
            set
            {
                if (_controlBoxOffset != value)
                {
                    _controlBoxOffset = value;
                    Invalidate();
                }
            }
        }

        [DefaultValue(0), Description("设置或获取控制按钮的间距"), Category("Skin")]
        public int ControlBoxSpace
        {
            get { return _controlBoxSpace; }
            set
            {
                if (_controlBoxSpace != value)
                {
                    _controlBoxSpace = value;
                    Invalidate();
                }
            }
        }

        protected override Padding DefaultPadding
        {
            get { return new Padding(BorderPadding.Left, CaptionHeight, BorderPadding.Left, BorderPadding.Left); }
        }

        public override Rectangle DisplayRectangle
        {
            get
            {
                var realClientRect = RealClientRect;
                realClientRect.X += _borderPadding.Left + Padding.Left;
                realClientRect.Y += (_borderPadding.Top + _captionHeight) + Padding.Top;
                realClientRect.Width -= _borderPadding.Horizontal + Padding.Horizontal;
                realClientRect.Height -= (_borderPadding.Vertical + _captionHeight) + Padding.Vertical;
                return realClientRect;
            }
        }

        [DefaultValue(true), Description("指示控件是否可以将用户拖动到背景上的图片作为背景(注意:开启前请设置AllowDrop为true,否则无效)"), Category("Skin")]
        public bool DropBack
        {
            get { return dropback; }
            set
            {
                if (dropback != value)
                {
                    dropback = value;
                }
            }
        }

        [Category("Caption"), DefaultValue(typeof(Color), "White"), Description("发光字体背景色")]
        public Color EffectBack
        {
            get { return effectback; }
            set
            {
                if (effectback != value)
                {
                    effectback = value;
                    Invalidate();
                }
            }
        }

        [Category("Caption"), Description("是否绘制发光标题"), DefaultValue(true)]
        public bool EffectCaption
        {
            get { return effectcaption; }
            set
            {
                if (effectcaption != value)
                {
                    effectcaption = value;
                    Invalidate();
                }
            }
        }

        [DefaultValue(typeof(int), "6"), Description("光圈大小"), Category("Caption")]
        public int EffectWidth
        {
            get { return effectWidth; }
            set
            {
                if (effectWidth != value)
                {
                    effectWidth = value;
                    Invalidate();
                }
            }
        }
        private bool formShowIcon = true;
        [Description("是否绘制窗体图标")]//, DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        public bool FormShowIcon { get { return formShowIcon; } set { formShowIcon = value; Invalidate(); } }
        public new FormBorderStyle FormBorderStyle
        {
            get { return base.FormBorderStyle; }
            set { base.FormBorderStyle = value; }
        }
        [Browsable(false)]
        public Rectangle IconRect
        {
            get
            {
                var width = SystemInformation.SmallIconSize.Width;
                if (!FormShowIcon || (Icon == null))
                {
                    return new Rectangle(3, (CaptionHeight - width) / 2, 1, 1);
                }
                var x = BorderPadding.Left < 6 ? 6 : BorderPadding.Left;
                if (((CaptionHeight - x) - 6) < width)
                {
                    width = (CaptionHeight - x) - 6;
                }
                //return new Rectangle(x, x + (((this.CaptionHeight - x) - width) / 2), width, width); old
                return new Rectangle(x, (CaptionHeight - width) / 2, width, width);
            }
        }

        [Category("Skin"), Description("是否继承所属窗体的背景")]
        public bool InheritBack { get; set; }





        [Category("Skin"), DefaultValue(typeof(MobileStyle), "2"), Description("移动窗体的条件")]
        public MobileStyle Mobile
        {
            get { return _mobile; }
            set
            {
                if (_mobile != value)
                {
                    _mobile = value;
                }
            }
        }

        [DefaultValue(typeof(Padding), "0")]
        public new Padding Padding
        {
            get { return _padding; }
            set
            {
                _padding = value;
                base.Padding = new Padding(BorderPadding.Left + _padding.Left, CaptionHeight + _padding.Top,
                    BorderPadding.Left + _padding.Right, BorderPadding.Left + _padding.Bottom);
            }
        }

        [Description("设置或获取窗体的圆角的大小"), DefaultValue(1), Category("Skin")]
        public int Radius
        {
            get { return _radius; }
            set
            {
                if (_radius <= 0) _radius = 0;
                if (_radius != value)
                {
                    _radius = value;
                    SetReion();
                    Invalidate();
                }
            }
        }

        protected Rectangle RealClientRect
        {
            get
            {
                if (WindowState == FormWindowState.Maximized)
                {
                    return new Rectangle(_deltaRect.X, _deltaRect.Y, Width - _deltaRect.Width,
                        Height - _deltaRect.Height);
                }
                return new Rectangle(Point.Empty, Size);
            }
        }

        [Category("Skin"), DefaultValue(typeof(RoundStyle), "1"), Description("设置或获取窗体的圆角样式")]
        public RoundStyle RoundStyle
        {
            get { return _roundStyle; }
            set
            {
                if (_roundStyle != value)
                {
                    _roundStyle = value;
                    //   SetReion();
                    Invalidate();
                }
            }
        }

        [DefaultValue(true), Description("是否启用窗体阴影"), Category("Shadow")]
        public bool Shadow
        {
            get { return shadow; }
            set
            {
                if (shadow != value)
                {
                    shadow = value;
                }
            }
        }

        [DefaultValue(typeof(Color), "Black"), Description("窗体阴影颜色"), Category("Shadow")]
        public Color ShadowColor
        {
            get { return shadowColor; }
            set
            {
                if (shadowColor != value)
                {
                    shadowColor = value;
                    //if (skin != null)
                    //{
                    //    skin.SetBits();
                    //}
                }
            }
        }

        [Description("窗体阴影宽度"), Category("Shadow"), DefaultValue(typeof(int), "4")]
        public int ShadowWidth
        {
            get { return shadowWidth; }
            set
            {
                if (shadowWidth != value)
                {
                    shadowWidth = (value < 1) ? 1 : value;
                    //if (skin != null)
                    //{
                    //    skin.SetBits();
                    //}
                }
            }
        }

        [DefaultValue(true), Description("是否在窗体上绘画边框"), Category("Skin")]
        public bool ShowBorder
        {
            get { return showborder; }
            set
            {
                if (showborder != value)
                {
                    showborder = value;
                    Invalidate();
                }
            }
        }

        //[DefaultValue(true), Category("窗口样式"), Description("是否在窗体上绘画ICO图标")]
        //public bool ShowDrawIcon
        //{
        //    get
        //    {
        //        return this.showdrawicon;
        //    }
        //    set
        //    {
        //        if (this.showdrawicon != value)
        //        {
        //            this.showdrawicon = value;
        //            base.Invalidate();
        //        }
        //    }
        //}

        [Category("Skin"), Description("获取或设置窗体是否显示系统菜单"), DefaultValue(false)]
        public bool ShowSystemMenu { get; set; }

        [Description("窗体渐变后透明度"), Category("Skin")]
        public double SkinOpacity
        {
            get
            {
                if (skinOpacity == 0) skinOpacity = 0.9d;
                return skinOpacity;
            }
            set
            {
                if (skinOpacity != value)
                {
                    skinOpacity = value;
                }
            }
        }

        [DefaultValue(true), Description("是否启用窗口淡入淡出"), Category("Skin")]
        public bool Special
        {
            get { return special; }
            set
            {
                if (special != value)
                {
                    special = value;
                }
            }
        }

        [Category("SysBottom"), Description("自定义系统按钮点击时")]
        public Image SysBottomDown
        {
            get { return sysBottomDown; }
            set
            {
                if (sysBottomDown != value)
                {
                    sysBottomDown = value;
                    Invalidate();
                }
            }
        }

        [Category("SysBottom"), Description("自定义系统按钮悬浮时")]
        public Image SysBottomMouse
        {
            get { return sysBottomMouse; }
            set
            {
                if (sysBottomMouse != value)
                {
                    sysBottomMouse = value;
                    Invalidate();
                }
            }
        }

        [Category("SysBottom"), Description("自定义系统按钮初始时")]
        public Image SysBottomNorml
        {
            get { return sysBottomNorml; }
            set
            {
                if (sysBottomNorml != value)
                {
                    sysBottomNorml = value;
                    Invalidate();
                }
            }
        }

        [DefaultValue(typeof(Size), "28, 20"), Description("设置或获取自定义系统按钮的大小"), Category("SysBottom")]
        public Size SysBottomSize
        {
            get { return sysBottomSize; }
            set
            {
                if (sysBottomSize != value)
                {
                    sysBottomSize = value;
                    Invalidate();
                }
            }
        }

        [Category("SysBottom"), Description("自定义系统按钮悬浮提示")]
        public string SysBottomToolTip
        {
            get { return sysBottomToolTip; }
            set
            {
                if (sysBottomToolTip != value)
                {
                    sysBottomToolTip = value;
                    Invalidate();
                }
            }
        }

        [Description("自定义系统按钮是否显示"), Category("SysBottom")]
        public bool SysBottomVisibale
        {
            get { return sysBottomVisibale; }
            set
            {
                if (sysBottomVisibale != value)
                {
                    sysBottomVisibale = value;
                    Invalidate();
                }
            }
        }

        [Category("Caption")]
        public override string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = value;
                Invalidate(new Rectangle(0, 0, Width, CaptionHeight + 1));
            }
        }

        [DefaultValue(typeof(Color), "Black"), Category("Caption"), Description("标题颜色")]
        public Color TitleColor
        {
            get { return titleColor; }
            set
            {
                if (titleColor != value)
                {
                    titleColor = value;
                    Invalidate();
                }
            }
        }

        [DefaultValue(false), Description("是否根据背景色自动适应标题颜色。\n(背景色为暗色时标题显示白色，背景为亮色时标题显示黑色。)"), Category("Caption")]
        public bool TitleSuitColor
        {
            get { return titleSuitColor; }
            set
            {
                if (titleSuitColor != value)
                {
                    titleSuitColor = value;
                    Invalidate();
                }
            }
        }
        private Image btnclose;
        private Image btnrestore;
        private Image btnmin;
        private Image btnmax;
        private Image btncustom;



        [Description("关闭按钮图片")]
        public Image btnCloseImage
        {
            get { if (btnclose == null) btnclose = Properties.Resources.btnClose1; return btnclose; }
            set { btnclose = value; }
        }
        [Description("最小化按钮图片")]
        public Image btnMinImage
        {
            get { if (btnmin == null) btnmin = Properties.Resources.btnMin1; return btnmin; }
            set { btnmin = value; Invalidate(); }
        }
        [Description("最大化按钮图片")]
        public Image btnMaxImage
        {
            get { if (btnmax == null) btnmax = Properties.Resources.btnMax1; return btnmax; }
            set { btnmax = value; Invalidate(); }
        }
        [Description("系统自定义按钮")]
        public Image btnCustomImage
        {
            get { return btncustom; }
            set { btncustom = value; Invalidate(); }
        }
        [Description("还原按钮图片")]
        public Image btnRestoreImage
        {
            get { if (btnrestore == null) btnrestore = Properties.Resources.btnRestore1; return btnrestore; }
            set { btnrestore = value; Invalidate(); }
        }
        [Description("设置或获取最大化（还原）按钮的大小"), Category("MaximizeBox"), Browsable(false)]
        public Size MaxSize { get { return new Size(btnMaxImage.Width / 3, btnMaxImage.Height); } }
        [Category("MinimizeBox"), Description("设置或获取最小化按钮的大小"), Browsable(false)]
        public Size MiniSize { get { return new Size(btnMinImage.Width / 3, btnMinImage.Height); } }
        [Browsable(false), Category("CloseBox"), Description("设置或获取关闭按钮的大小")]
        public Size CloseBoxSize { get { return new Size(btnCloseImage.Width / 3, btnCloseImage.Height); } }

        [Description("设置或获取自定义系统按钮的大小"), Category("CustomSize"), Browsable(false)]
        public Size CustomSize { get { return btnCustomImage == null ? new Size(0, 0) : new Size(btnCustomImage.Width / 3, btnCustomImage.Height); } }
        public ToolTip ToolTip { get; private set; }

        public delegate void BackEventHandler(object sender, BackEventArgs e);

        public delegate void SysBottomEventHandler(object sender);

        #endregion

        #region  事件

        [Description("Back属性值更改时引发的事件"), Category("Skin")]
        public event BackEventHandler BackChanged;

        [Category("Skin"), Description("自定义按钮被点击时引发的事件")]
        public event SysBottomEventHandler SysBottomClick;

        #endregion

        #region  重载

        private void main_BackChanged(object sender, BackEventArgs e)
        {
            if (InheritBack)
            {
                var main = (CForm)sender;
                BackToColor = true;
                Back = main.Back;
                BackLayout = main.BackLayout;
            }
        }

        private void main_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (InheritBack)
            {
                var form = (Form)sender;
                Back = form.BackgroundImage;
                BackLayout = true;
                form.BackColor = Share.FormBackColor;
                BackColor = (form.BackgroundImage == null)
                    ? form.BackColor
                    : SkinTools.GetImageAverageColor((Bitmap)form.BackgroundImage);
            }
        }

        protected Size MaximumSizeFromMaximinClientSize()
        {
            var empty = Size.Empty;
            if (MaximumSize != Size.Empty)
            {
                empty.Width = MaximumSize.Width + _borderPadding.Horizontal;
                empty.Height = (MaximumSize.Height + _borderPadding.Vertical) + _captionHeight;
            }
            return empty;
        }

        protected Size MinimumSizeFromMiniminClientSize()
        {
            var defaultMinTrackSize = GetDefaultMinTrackSize();
            if (MinimumSize != Size.Empty)
            {
                defaultMinTrackSize.Width = MinimumSize.Width + _borderPadding.Horizontal;
                defaultMinTrackSize.Height = (MinimumSize.Height + _borderPadding.Vertical) + _captionHeight;
            }
            return defaultMinTrackSize;
        }

        private void mStopAnthor()
        {
            if (Left <= 0)
            {
                Aanhor = AnchorStyles.Left;
            }
            else if (Left >= (Screen.PrimaryScreen.Bounds.Width - Width))
            {
                Aanhor = AnchorStyles.Right;
            }
            else if (Top <= 0)
            {
                Aanhor = AnchorStyles.Top;
            }
            else
            {
                Aanhor = AnchorStyles.None;
            }
        }

        protected virtual void OnBackChanged(BackEventArgs e)
        {
            if (BackChanged != null)
            {
                BackChanged(this, e);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            //if (skin != null)
            //{
            //    skin.Close();
            //}
            if (Special && !DesignMode)
            {
                NativeMethods.AnimateWindow(Handle, 150, 0x90000);
                Update();
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetReion();
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            //    SetReion();
            if ((Owner is CForm) && InheritBack)
            {
                var owner = (CForm)Owner;
                BackToColor = true;
                Back = owner.Back;
                BackLayout = owner.BackLayout;
                owner.BackChanged += main_BackChanged;
            }
            else if ((Owner != null) && InheritBack)
            {
                var form = Owner;
                Back = form.BackgroundImage;
                BackLayout = true;
                form.BackColor = Share.FormBackColor;
                BackColor = (form.BackgroundImage == null)
                    ? form.BackColor
                    : SkinTools.GetImageAverageColor((Bitmap)form.BackgroundImage);
                form.BackgroundImageChanged += main_BackgroundImageChanged;
            }
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            if (DropBack)
            {
                var data = (string[])drgevent.Data.GetData(DataFormats.FileDrop);
                var info = new FileInfo(data[0]);
                if (data != null)
                {
                    var str = info.Extension.Substring(1);
                    //string[] strArray3 = new string[] { "png", "bmp", "jpg", "jpeg", "gif" };
                    var strArray3 = "png,bmp,jpg,jpeg,gif";
                    if (strArray3.Contains(str.ToLower()))
                    {
                        Back = Image.FromFile(data[0]);
                    }
                }
            }
            base.OnDragDrop(drgevent);
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            if (DropBack)
            {
                drgevent.Effect = DragDropEffects.Link;
            }
            base.OnDragEnter(drgevent);
        }

        protected override void OnLoad(EventArgs e)
        {
            //如果启用Aero
            //if (!DesignMode && DwmIsCompositionEnabled())
            //{
            //    MARGINS m = new MARGINS();
            //    m.Right = -1; //设为负数,则全窗体透明
            //    DwmExtendFrameIntoClientArea(this.Handle, ref m); //开启全窗体透明效果
            //}
            base.OnLoad(e);
            ResizeCore();
            // Invalidate();
            //if (false)//!DesignMode && Areo.IsAreoEnabled())
            //{
            //    Areo.AreoParams parameter = new Areo.AreoParams();
            //    parameter.Flags = Areo.AreoParams.ENABLE;
            //    parameter.Enable = true;
            //    parameter.AreoRegion = IntPtr.Zero;
            //    Areo.AreoWindow(this.Handle, parameter);                //将整个窗体背景设置成Areo效果
            //}
        }

        //protected override void OnPaintBackground(PaintEventArgs e)
        //{
        //    base.OnPaintBackground(e);
        //    if (!DesignMode && DwmIsCompositionEnabled())
        //    {
        //        e.Graphics.Clear(Color.Black); //将窗体用黑色填充（Dwm 会把黑色视为透明区域）
        //    }
        //}

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            mStopAnthor();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            var location = e.Location;
            if (e.Button == MouseButtons.Left)
            {
                if (((!ControlBoxManager.CloseBoxRect.Contains(location) &&
                      !ControlBoxManager.MaximizeBoxRect.Contains(location)) &&
                     (!ControlBoxManager.MinimizeBoxRect.Contains(location) &&
                      !ControlBoxManager.SysBottomRect.Contains(location))) && (Mobile != MobileStyle.None))
                {
                    isMouseDown = true;
                }
                else
                {
                    ControlBoxManager.ProcessMouseOperate(e.Location, MouseOperate.Down);
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            ControlBoxManager.ProcessMouseOperate(PointToClient(MousePosition), MouseOperate.Hover);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ControlBoxManager.ProcessMouseOperate(Point.Empty, MouseOperate.Leave);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            ControlBoxManager.ProcessMouseOperate(e.Location, MouseOperate.Move);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            isMouseDown = false;
            base.OnMouseUp(e);
            ControlBoxManager.ProcessMouseOperate(e.Location, MouseOperate.Up);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            this.SuspendLayout();
            if (base.IsMdiContainer)
            {
                base.OnPaint(e);
            }
            // if (DesignMode) return;
            var graphics = e.Graphics;
            //if (false)//System.Environment.OSVersion.Version.Major == 6 && Areo.IsAreoEnabled())
            //{
            //    //graphics.DrawImage(Properties.Resources.bg, this.ClientRectangle);
            //    graphics.Clear(Color.FromArgb(150, Color.White));  //设置背景透明色
            //    //LinearGradientBrush lgb = new LinearGradientBrush(this.ClientRectangle, Color.FromArgb(150, base.BackColor),
            //    //   Color.FromArgb(150, base.BackColor), 90);
            //    //graphics.FillRectangle(lgb, this.ClientRectangle);
            //}
            //else
            //{
            //  graphics.SmoothingMode = SmoothingMode.HighQuality;
            //  graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            Share.GraphicSetup(graphics);
            if (Back != null)
            {
                if (BackLayout)
                {
                    int w = backStruk ? base.Width : back.Width;
                    graphics.DrawImage(Back, 0, 0, w, Back.Height);
                }
                else
                {
                    graphics.DrawImage(Back, -(Back.Width - Width), 0, Back.Width, Back.Height);
                }
                if (BackShade)
                {
                    if (BackLayout)
                    {
                        using (var brush3 = new LinearGradientBrush(new Rectangle(Back.Width - 50, 0, 50, Back.Height), BackColor, Color.Transparent, 180f))
                        {
                            graphics.FillRectangle(brush3, (Back.Width - brush3.Rectangle.Width) + 1f, 0f, brush3.Rectangle.Width, brush3.Rectangle.Height);
                        }
                        using (var brush4 = new LinearGradientBrush(new Rectangle(0, Back.Height - 50, Back.Width, 50), BackColor, Color.Transparent, 270f))
                        {
                            graphics.FillRectangle(brush4, 0f, (Back.Height - brush4.Rectangle.Height) + 1f, brush4.Rectangle.Width, brush4.Rectangle.Height);
                        }
                    }
                    else
                    {
                        using (var brush = new LinearGradientBrush(new Rectangle(-(Back.Width - Width), 0, 50, Back.Height), BackColor, Color.Transparent, 360f))
                        {
                            graphics.FillRectangle(brush, -(Back.Width - Width), 0f, brush.Rectangle.Width, brush.Rectangle.Height);
                        }
                        using (var brush2 = new LinearGradientBrush(new Rectangle(-(Back.Width - Width), Back.Height - 50, Back.Width, 50), BackColor, Color.Transparent, 270f))
                        {
                            graphics.FillRectangle(brush2, -(Back.Width - Width), Back.Height - 50, brush2.Rectangle.Width, brush2.Rectangle.Height);
                        }
                    }
                }
            }
            //}

            if (!base.IsMdiContainer)
            {
                base.OnPaint(e);
            }
            //绘制质感的标题栏
            if (showCaptionPlace)
            {
                var lgb = new LinearGradientBrush(CaptionRect, Share.LineTop, Share.LineBottom, 90f);
                graphics.FillRectangle(lgb, CaptionRect);
                lgb.Dispose();
                var p = new Pen(Share.Line, 1);
                graphics.DrawLine(p, new Point(0, CaptionRect.Height), new Point(CaptionRect.Width, CaptionRect.Height));
                p.Color = Color.White;
                //  graphics.DrawLine(p, new Point(0, this.CaptionRect.Top), new Point(this.CaptionRect.Width, this.CaptionRect.Top));
                p.Dispose();
            }

            //绘制渐变图片
            //if (BorderPalace != null) // && !Areo.IsAreoEnabled())
            //{
            //    ImageDrawRect.DrawRect(graphics, (Bitmap)BorderPalace,
            //        new Rectangle(ClientRectangle.X - 5, ClientRectangle.Y - 5,
            //            ClientRectangle.Width + 10, ClientRectangle.Height + 10),
            //        Rectangle.FromLTRB(BorderRectangle.X, BorderRectangle.Y,
            //            BorderRectangle.Width, BorderRectangle.Height), 1, 1);
            //}
            var clientRectangle = ClientRectangle;
            var renderer = Renderer;
            if (ControlBoxManager.CloseBoxVisibale)
            {
                renderer.DrawSkinFormControlBox(new SkinFormControlBoxRenderEventArgs(this, graphics, ControlBoxManager.CloseBoxRect, _active, ControlBoxStyle.Close, ControlBoxManager.CloseBoxState));
            }
            if (ControlBoxManager.MaximizeBoxVisibale)
            {
                renderer.DrawSkinFormControlBox(new SkinFormControlBoxRenderEventArgs(this, graphics, ControlBoxManager.MaximizeBoxRect, _active, ControlBoxStyle.Maximize, ControlBoxManager.MaximizeBoxState));
            }
            if (ControlBoxManager.MinimizeBoxVisibale)
            {
                renderer.DrawSkinFormControlBox(new SkinFormControlBoxRenderEventArgs(this, graphics, ControlBoxManager.MinimizeBoxRect, _active, ControlBoxStyle.Minimize, ControlBoxManager.MinimizeBoxState));
            }
            if (ControlBoxManager.SysBottomVisibale)
            {
                renderer.DrawSkinFormControlBox(new SkinFormControlBoxRenderEventArgs(this, graphics, ControlBoxManager.SysBottomRect, _active, ControlBoxStyle.SysBottom, ControlBoxManager.SysBottomState));
            }
            if (ShowBorder)
            {
                renderer.DrawSkinFormBorder(new SkinFormBorderRenderEventArgs(this, graphics, clientRectangle, _active));
            }
            //绘制边框
            ////if (BackPalace != null)
            ////{
            ////    ImageDrawRect.DrawRect(graphics, (Bitmap)borderpalace,
            ////        new Rectangle(ClientRectangle.X - 5, ClientRectangle.Y - 5, ClientRectangle.Width + 10, ClientRectangle.Height + 10),
            ////        Rectangle.FromLTRB(BackRectangle.X, BackRectangle.Y, BackRectangle.Width, BackRectangle.Height), 1, 1);
            ////}

            renderer.DrawSkinFormCaption(new SkinFormCaptionRenderEventArgs(this, graphics, CaptionRect, _active));
            this.ResumeLayout(false);
        }

        private bool showCaptionPlace;

        [Description("是否显示标题栏背景"), DefaultValue(false)]
        public bool ShowCaptionPlace
        {
            get { return showCaptionPlace; }
            set
            {
                showCaptionPlace = value;
                Invalidate();
            }
        }

        public event EventHandler RendererChangled
        {
            add
            {
                base.Events.AddHandler(EventRendererChanged, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventRendererChanged, value);
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("设置或获取窗体的绘制方法"), Browsable(false)]
        public SkinFormRenderer Renderer
        {
            get
            {
                if (_renderer == null)
                {
                    _renderer = new SkinFormProfessionalRenderer(Colortable);
                }
                return _renderer;
            }
            set
            {
                _renderer = value;
                OnRendererChanged(EventArgs.Empty);
            }
        }


        protected virtual void OnRendererChanged(EventArgs e)
        {
            Renderer.InitSkinForm(this);
            var handler = Events[EventRendererChanged] as EventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
            base.Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            //this.SuspendLayout();
            //AvoidControlFlicker a = new AvoidControlFlicker();
            //Console.WriteLine("前",this.Controls.Count,this.WindowState);
            //a.FreezePainting(this, true);
            ResizeCore();
            base.OnResize(e);
            //a.FreezePainting(this, false);
            //Console.WriteLine("后",this.Controls.Count, this.WindowState);
            //this.ResumeLayout(false);
        }

        protected override void OnStyleChanged(EventArgs e)
        {
            if (_clientSizeSet)
            {
                ClientSize = ClientSize;
                _clientSizeSet = false;
            }
            base.OnStyleChanged(e);
        }

        protected virtual void OnSysBottomClick(object e)
        {
            if (SysBottomClick != null)
            {
                SysBottomClick(this);
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
            {
                if (Special && !base.DesignMode)
                {
                    var dwtime = (!OneVisibles || !Shadow) ? 150 : 300;
                    NativeMethods.AnimateWindow(Handle, dwtime, 0xa0000);
                    //  base.Opacity = this.SkinOpacity;
                    //  Update();
                }
                //if ((!DesignMode && (skin == null)) && Shadow)
                //{
                //    //skin = new BaseForm(this);
                //    //skin.Show(this);
                //}
                OneVisibles = false;
                base.OnVisibleChanged(e);
            }
            else
            {
                base.OnVisibleChanged(e);
                if (Special && !base.DesignMode)
                {
                    Opacity = 1.0;
                    NativeMethods.AnimateWindow(Handle, 150, 0x90000);
                    // Update();
                }
            }
        }

        protected void CalcDeltaRect()
        {
            if (WindowState == FormWindowState.Maximized)
            {
                var bounds = Bounds;
                Rectangle workingArea = Screen.GetWorkingArea(this);
                //var workingArea = Screen.PrimaryScreen.WorkingArea;
                workingArea.X -= _borderPadding.Left;
                workingArea.Y -= _borderPadding.Top;
                workingArea.Width += _borderPadding.Horizontal;
                workingArea.Height += _borderPadding.Vertical;
                var x = 0;
                var y = 0;
                var width = 0;
                var height = 0;
                if (bounds.Left < workingArea.Left)
                {
                    x = workingArea.Left - bounds.Left;
                }
                if (bounds.Top < workingArea.Top)
                {
                    y = workingArea.Top - bounds.Top;
                }
                if (bounds.Width > workingArea.Width)
                {
                    width = bounds.Width - workingArea.Width;
                }
                if (bounds.Height > workingArea.Height)
                {
                    height = bounds.Height - workingArea.Height;
                }
                _deltaRect = new Rectangle(x, y, width, height);
            }
            else
            {
                _deltaRect = Rectangle.Empty;
            }
        }

        protected virtual void ResizeCore()
        {
            CalcDeltaRect();
            //  SetReion();
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            var minimumSize = MinimumSize;
            var maximumSize = MaximumSize;
            var size3 = SizeFromClientSize(Size.Empty);
            base.ScaleControl(factor, specified);
            if (minimumSize != Size.Empty)
            {
                minimumSize -= size3;
                minimumSize =
                    new Size((int)Math.Round(minimumSize.Width * factor.Width),
                        (int)Math.Round(minimumSize.Height * factor.Height)) + size3;
            }
            if (maximumSize != Size.Empty)
            {
                maximumSize -= size3;
                maximumSize =
                    new Size((int)Math.Round(maximumSize.Width * factor.Width),
                        (int)Math.Round(maximumSize.Height * factor.Height)) + size3;
            }
            MinimumSize = minimumSize;
            MaximumSize = maximumSize;
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (_inWmWindowPosChanged != 0)
            {
                try
                {
                    var type = typeof(Form);
                    var field = type.GetField("FormStateExWindowBoundsWidthIsClientSize",
                        BindingFlags.NonPublic | BindingFlags.Static);
                    var info2 = type.GetField("formStateEx", BindingFlags.NonPublic | BindingFlags.Instance);
                    var info3 = type.GetField("restoredWindowBounds", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (((field != null) && (info2 != null)) && (info3 != null))
                    {
                        var rectangle = (Rectangle)info3.GetValue(this);
                        var section = (BitVector32.Section)field.GetValue(this);
                        var vector = (BitVector32)info2.GetValue(this);
                        if (vector[section] == 1)
                        {
                            width = rectangle.Width;
                            height = rectangle.Height;
                        }
                    }
                }
                catch
                {
                }
            }
            base.SetBoundsCore(x, y, width, height, specified);
        }

        protected override void SetClientSizeCore(int x, int y)
        {
            _clientSizeSet = false;
            var type = typeof(Control);
            var type2 = typeof(Form);
            var field = type.GetField("clientWidth", BindingFlags.NonPublic | BindingFlags.Instance);
            var info2 = type.GetField("clientHeight", BindingFlags.NonPublic | BindingFlags.Instance);
            var info3 = type2.GetField("FormStateSetClientSize", BindingFlags.NonPublic | BindingFlags.Static);
            var info4 = type2.GetField("formState", BindingFlags.NonPublic | BindingFlags.Instance);
            if (((field != null) && (info2 != null)) && ((info4 != null) && (info3 != null)))
            {
                _clientSizeSet = true;
                Size = new Size(x, y);
                field.SetValue(this, x);
                info2.SetValue(this, y);
                var section = (BitVector32.Section)info3.GetValue(this);
                var vector = (BitVector32)info4.GetValue(this);
                vector[section] = 1;
                info4.SetValue(this, vector);
                OnClientSizeChanged(EventArgs.Empty);
                vector[section] = 0;
                info4.SetValue(this, vector);
            }
            else
            {
                base.SetClientSizeCore(x, y);
            }
        }

        private void SetReion()
        {
            if (_radius > 0)
            {
                SkinTools.CreateRegion(this, RealClientRect, Radius);
            }
        }

        private void SetStyles()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.DoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.ResizeRedraw |
                     //  ControlStyles.EnableNotifyMessage|
                     ControlStyles.UserPaint, true);
            UpdateStyles();
            AutoScaleMode = AutoScaleMode.None;
        }
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;
        //        //cp.ExStyle |= 0x00080000;
        //        return cp;
        //    }
        //}
        //protected override void OnNotifyMessage(Message m)
        //{
        //  //  base.OnNotifyMessage(m);
        //}
        ////protected override void onNotifyMessage(Message m)
        //{
        //    // 此处书写过滤消息代码   
        //}
        protected override Size SizeFromClientSize(Size clientSize)
        {
            return clientSize;
        }

        public void SysbottomAv(object e)
        {
            OnSysBottomClick(e);
        }

        protected void TrackPopupSysMenu(ref Message m)
        {
            if (m.WParam.ToInt32() == 2)
            {
                TrackPopupSysMenu(m.HWnd, new Point(m.LParam.ToInt32()));
            }
        }

        protected void TrackPopupSysMenu(IntPtr hWnd, Point point)
        {
            if (ShowSystemMenu && (point.Y <= (((Top + _borderPadding.Top) + _deltaRect.Y) + _captionHeight)))
            {
                var wParam = NativeMethods.TrackPopupMenu(NativeMethods.GetSystemMenu(hWnd, false), 0x100, point.X,
                    point.Y, 0, hWnd, IntPtr.Zero);
                NativeMethods.PostMessage(hWnd, 0x112, wParam, IntPtr.Zero);
            }
        }

        protected virtual Size GetDefaultMinTrackSize()
        {
            return
                new Size(
                    (((((CloseBoxSize.Width + MinimumSize.Width) + MaxSize.Width) + SysBottomSize.Width) +
                      _borderPadding.Horizontal) + SystemInformation.SmallIconSize.Width) + 20,
                    (CaptionHeight + _borderPadding.Vertical) + 2);
        }

        private void WmGetMinMaxInfo(ref Message m)
        {
            var structure = (MINMAXINFO)Marshal.PtrToStructure(m.LParam, typeof(MINMAXINFO));
            if (MaximumSize != Size.Empty)
            {
                structure.maxTrackSize = MaximumSize;
            }
            else
            {
                //Rectangle workingArea = Screen.GetWorkingArea(this);
                var workingArea = Screen.PrimaryScreen.WorkingArea;
                structure.maxPosition = new Point(workingArea.X, workingArea.Y);
                structure.maxTrackSize = new Size(workingArea.Width, workingArea.Height - 1);
            }
            if (MinimumSize != Size.Empty)
            {
                structure.minTrackSize = MinimumSize;
            }
            else
            {
                GetDefaultMinTrackSize();
                structure.minTrackSize =
                    new Size(
                        (((((((CloseBoxSize.Width + MiniSize.Width) + MaxSize.Width) + SysBottomSize.Width) +
                            ControlBoxOffset.X) + (ControlBoxSpace * 2)) + SystemInformation.SmallIconSize.Width) +
                         (BorderPadding.Left * 2)) + 3, CaptionHeight);
            }
            Marshal.StructureToPtr(structure, m.LParam, false);
        }

        private void WmNcActive(ref Message m)
        {
            if (m.WParam.ToInt32() == 1)
            {
                _active = true;
            }
            else
            {
                _active = false;
            }
            m.Result = Result.TRUE;
            base.Invalidate();
        }

        protected virtual void WmNcCalcSize(ref Message m)
        {
            if (Opacity != 1.0)
            {
                base.Invalidate();
            }
        }

        private void WmNcHitTest(ref Message m)
        {
            var p = new Point(m.LParam.ToInt32());
            p = PointToClient(p);
            if (IconRect.Contains(p) && ShowSystemMenu)
            {
                m.Result = new IntPtr(3); //菜单
            }
            else
            {
                if (CanResize)
                {
                    if ((p.X < 5) && (p.Y < 5))
                    {
                        m.Result = new IntPtr(13);
                        return;
                    }
                    if ((p.X > (Width - 5)) && (p.Y < 5))
                    {
                        m.Result = new IntPtr(14);
                        return;
                    }
                    if ((p.X < 5) && (p.Y > (Height - 5)))
                    {
                        m.Result = new IntPtr(0x10);
                        return;
                    }
                    if ((p.X > (Width - 5)) && (p.Y > (Height - 5)))
                    {
                        m.Result = new IntPtr(0x11);
                        return;
                    }
                    if (p.Y < 3)
                    {
                        m.Result = new IntPtr(12);
                        return;
                    }
                    if (p.Y > (Height - 3))
                    {
                        m.Result = new IntPtr(15);
                        return;
                    }
                    if (p.X < 3)
                    {
                        m.Result = new IntPtr(10);
                        return;
                    }
                    if (p.X > (Width - 3))
                    {
                        m.Result = new IntPtr(11);
                        return;
                    }
                }
                if (((!ControlBoxManager.CloseBoxRect.Contains(p) && !ControlBoxManager.MaximizeBoxRect.Contains(p)) &&
                     (!ControlBoxManager.MinimizeBoxRect.Contains(p) && !ControlBoxManager.SysBottomRect.Contains(p))) &&
                    (Mobile != MobileStyle.None))
                {
                    if ((Mobile == MobileStyle.TitleMobile) && (p.Y < CaptionHeight))
                    {
                        m.Result = new IntPtr(2);
                        return;
                    }
                    if (Mobile == MobileStyle.Mobile)
                    {
                        m.Result = new IntPtr(2);
                        return;
                    }
                }
                m.Result = new IntPtr(1);
                isMouseDown = false;
            }
        }

        protected virtual void WmNcRButtonUp(ref Message m)
        {
            TrackPopupSysMenu(ref m);
            base.WndProc(ref m);
        }

        protected virtual void WmWindowPosChanged(ref Message m)
        {
            _inWmWindowPosChanged++;
            base.WndProc(ref m);
            _inWmWindowPosChanged--;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x24:
                    WmGetMinMaxInfo(ref m);
                    return;
                //case 0x13:
                case 0x14:
                    break;
                case 0x47:
                    WmWindowPosChanged(ref m);
                    return;

                case 0x83:
                    WmNcCalcSize(ref m);
                    return;

                case 0x84:
                    WmNcHitTest(ref m);
                    return;

                case 0x85:
                    break;

                case 0x86:
                    WmNcActive(ref m);
                    return;

                case 0xa5:
                    WmNcRButtonUp(ref m);
                    return;

                case 0xae:
                case 0xaf:
                    m.Result = Result.TRUE;
                    return;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #endregion

    }
}