using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    public partial class CProgressBar : UserControl
    {
        private Bitmap blockBitmap = Properties.Resources.bar;

        /// Alpha alpha = Alpha.Normal;
        private Timer _timer;

        private int _value;
        private Rectangle blocksRect = Rectangle.Empty;
        private int load;
        private Rectangle loadRect = Rectangle.Empty;
        private int maximum = 100;
        private int pos;
        private ProgressBarStyle style;
        private Color backColor;
        private Color borderColor;
        private bool showBorder = true;

        public CProgressBar()
        {
            BackColor = Color.Transparent;
            InitializeComponent();
            SetStyles();
            //  DrawHelper.SetWindowRegion(this, Share.DefaultRadius / 2);
            blocksRect = new Rectangle(0, 0, (int)(Width * 0.2f), Height);
            //  StyleChanged += (a, b) => { Style = base.Style; };
            btnColor = Share.BackColor;
            borderColor = Share.BorderColor;
            StartProcess();
        }

        //Color color { get { return Color.FromArgb((int)alpha, bgColor.R, bgColor.G, bgColor.B); } }
        //Color bgColor = Color.Black;
        //public Color BgColor { get { return bgColor; } set { bgColor = value; this.Invalidate(); } }
        private Color btnColor;
        [Description("背景颜色")]
        public Color BtnColor { get { return btnColor; } set { btnColor = value; Invalidate(); } }

        [Description("边框颜色")]
        public Color BorderColor { get { return borderColor; } set { borderColor = value; Invalidate(); } }
        [Description("是否显示边框")]
        public bool ShowBorder
        {
            get { return showBorder; }
            set { showBorder = value; Invalidate(); }
        }
        [Description("滚动条图片")]
        public Bitmap BlockBitmap
        {
            get { return blockBitmap; }
            set { blockBitmap = value; Invalidate(); }
        }

        protected override Size DefaultSize
        {
            get { return new Size(150, 10); }
        }

        protected override Size DefaultMaximumSize
        {
            get { return new Size(800, 24); }
        }

        protected override Size DefaultMinimumSize
        {
            get { return new Size(150, 10); }
        }

        private GraphicsPath blocksPath
        {
            get
            {
                return DrawHelper.CreateRoundPath(blocksRect, new Padding(Share.DefaultRadius));
                //return DrawHelper.CreateRoundPath2(blocksRect, Share.DefaultRadius);
            }
        }

        public int Maximum
        {
            get { return maximum; }
            set { maximum = value; }
        }

        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
                load = (Width * Value) / Maximum;
                loadRect = new Rectangle(0, 0, load, Height);
                Refresh();
            }
        }

        public ProgressBarStyle Style
        {
            get { return style; }
            set
            {
                style = value;
                Invalidate();
            }
        }

        internal GraphicsPath rectPath
        {
            get
            {
                var rect = ClientRectangle;
                //  rect.X--; rect.Y--;
                return DrawHelper.CreateRoundPath(rect, new Padding(Share.DefaultRadius));
                //  return DrawHelper.CreateRoundPath2(rect, Share.DefaultRadius);
            }
        }

        internal GraphicsPath loadPath
        {
            get
            {
                //if (loadRect.Width == Width)
                return DrawHelper.CreateRoundPath(loadRect, new Padding(Share.DefaultRadius));
                // return DrawHelper.CreateRoundPath2(loadRect, Share.DefaultRadius);
                //return DrawHelper.CreateRoundPath2(loadRect, 6,);
            }
        }

        private void StartProcess()
        {
            if (!base.DesignMode)
            {
                EventHandler handler = null;
                if (style == ProgressBarStyle.Blocks)
                {
                    if (_timer != null)
                    {
                        _timer.Dispose();
                    }
                    _timer = new Timer();
                    _timer.Interval = 15;
                    if (handler == null)
                    {
                        handler = delegate
                        {
                            if (blocksRect.X >= Width) blocksRect.X = -blocksRect.Width;
                            blocksRect.X += 3;
                            base.Refresh();
                        };
                    }
                    _timer.Tick += handler;
                    _timer.Start();
                }
                else if (style == ProgressBarStyle.Marquee)
                {
                    if (_timer != null)
                    {
                        _timer.Dispose();
                    }
                    _timer = new Timer();
                    _timer.Interval = 30;
                    if (handler == null)
                    {
                        pos = -Width;
                        handler = delegate
                        {
                            if (pos == 0) pos = -Width;
                            pos++;
                            base.Refresh();
                        };
                    }
                    _timer.Tick += handler;

                    _timer.Start();
                }
                else if (_timer != null)
                {
                    _timer.Dispose();
                    _timer = null;
                }
            }
        }

        private void SetStyles()
        {
            base.SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Selectable |
                ControlStyles.DoubleBuffer |
                ControlStyles.SupportsTransparentBackColor |
                //  ControlStyles.EnableNotifyMessage |
                ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.Opaque, false);
            base.UpdateStyles();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            var g = pe.Graphics;
            Share.GraphicSetup(g);
            g.Clip = new Region(rectPath);
            using (var sb = new SolidBrush(btnColor))
            {
                g.FillPath(sb, rectPath); //画背景
                //rectPath.Dispose();
            }
            if (Style == ProgressBarStyle.Continuous) //进度条
            {
                using (var tb = new TextureBrush(blockBitmap))
                {
                    g.FillPath(tb, loadPath);
                }
            }
            else if (Style == ProgressBarStyle.Marquee) //动画
            {
                g.DrawImage(blockBitmap, pos, 0);
            }
            else if (Style == ProgressBarStyle.Blocks) //滚动
            {
                using (var tb = new TextureBrush(blockBitmap))
                {
                    // g.Clip = new Region(blocksPath);
                    g.FillPath(tb, blocksPath);
                }
            }
            if (ShowBorder)
                using (var pen = new Pen(borderColor))
                {
                    g.DrawPath(pen, rectPath); //画边框
                                               // rectPath.Dispose();
                }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            load = (Width * Value) / Maximum;
            loadRect = new Rectangle(0, 0, load, Height);
            blocksRect = new Rectangle(0, 0, (int)(Width * 0.2f), Height);
            // DrawHelper.SetWindowRegion(this, Share.DefaultRadius / 2);
        }
    }
}