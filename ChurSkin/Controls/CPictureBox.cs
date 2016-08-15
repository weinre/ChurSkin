using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using Win32.Consts;

namespace System.Windows.Forms
{
    public partial class CPictureBox : PictureBox
    {
        private Bitmap icon;
        private Padding round = new Padding(Share.DefaultRadius);
        private bool showBorder;
        private Color bgColor;
        public CPictureBox()
        {
            ShowIndex = 0;
            ZIndex = 0;
            InitializeComponent();
          //  this.BackColor = Share.BackColor;
            Init();
        }

        public Color BgColor
        {
            get { return bgColor; }
            set { bgColor = value;Invalidate(); }
        }
        //Color color = Color.FromArgb(180, 180, 180);
        [Description("辅助显示顺序"), DefaultValue(0)]
        public int ZIndex { get; set; }

        [Description("辅助变量储存其他数据"), DefaultValue(0)]
        public long ShowIndex { get; set; }

        [Description("规定四个角的圆角")]
        public Padding Round
        {
            get { return round; }
            set
            {
                round.Left = round.Left <= 0 ? 1 : round.Left;
                round.Top = round.Top <= 0 ? 1 : round.Top;
                round.Right = round.Right <= 0 ? 1 : round.Right;
                round.Bottom = round.Bottom <= 0 ? 1 : round.Bottom;
                round = value;
                Refresh();
            }
        }
        [Description("是否显示边框"), DefaultValue(true)]
        public bool ShowBorder
        {
            get { return showBorder; }
            set { showBorder = value;Invalidate(); }
        }
        private GraphicsPath rectPath
        {
            get { return DrawHelper.CreateRoundPath(ClientRectangle, round); }
        }

        [Description("右上角小图标"), DefaultValue(null)]
        public Bitmap Icon
        {
            get { return icon; }
            set
            {
                icon = value;
                Invalidate();
            }
        }

        public void Init()
        {
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            BackColor = Color.Transparent;
            BorderStyle = BorderStyle.None;
            UpdateStyles();
        }

        private Image thisImage;
        [Description("为达到透明和异性的效果，尽量不采用BackgroundImage 和Image")]
        public Image ThisImage
        {
            get { return thisImage; }
            set { thisImage = value; }
        }
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            Share.GraphicSetup(pe.Graphics);

            pe.Graphics.SetClip(rectPath);
            using (var sb = new SolidBrush(bgColor))
            {
                pe.Graphics.FillPath(sb,rectPath);
            }
            if (thisImage != null)
                DrawBackGroundImage(pe.Graphics);
            if (showBorder)
            {
                using (Pen p = new Pen(Share.BorderColor))
                {
                    pe.Graphics.DrawPath(p, rectPath);
                    rectPath.Dispose();
                }
            }

            if (Icon != null)
            {
                pe.Graphics.DrawImage(Icon, Width - icon.Width, 0);
            }
        }
        private void DrawBackGroundImage(Graphics g)
        {
            g.SetClip(rectPath);
            var rect = Rectangle.Empty;
            switch (BackgroundImageLayout)
            {
                case ImageLayout.None:
                    rect = new Rectangle(0, 0, thisImage.Width, thisImage.Height);
                    break;
                case ImageLayout.Center:
                    rect = new Rectangle((Width - thisImage.Width) / 2,
                        (Height - thisImage.Height) / 2,
                        thisImage.Width,
                        thisImage.Height);
                    break;
                case ImageLayout.Stretch:
                    rect = ClientRectangle;
                    break;
                case ImageLayout.Tile:
                    if (thisImage.Width > Width && Height > thisImage.Height)
                        g.DrawImage(thisImage, ClientRectangle);
                    else
                    {
                        var fillX = (int)Math.Ceiling(Width / (double)thisImage.Width);
                        var fillY = (int)Math.Ceiling(Height / (double)thisImage.Height);
                        for (var x = 0; x <= fillX; x++) //画X轴
                        {
                            for (var y = 0; y <= fillY; y++) //画Y轴
                            {
                                var rectXY = new Rectangle(thisImage.Width * x, thisImage.Height * y,
                                    thisImage.Width, thisImage.Height);
                                g.DrawImage(thisImage, rectXY);
                            }
                        }
                    }
                    return;
                case ImageLayout.Zoom:
                    if (Width > thisImage.Width)
                    {
                        rect.X = (Width - thisImage.Width) / 2;
                        rect.Width = thisImage.Width;
                    }
                    else
                    {
                        rect.X = 0;
                        rect.Width = Width;
                    }
                    if (Height > thisImage.Height)
                    {
                        rect.Y = (Height - thisImage.Height) / 2;
                        rect.Height = thisImage.Height;
                    }
                    else
                    {
                        rect.Y = 0;
                        rect.Height = Height;
                    }
                    break;
            }
            // var xy = (cs == ControlState.Pressed) ? 1 : 0;
            //rect.X += xy;
            //rect.Y += xy;
            g.DrawImage(thisImage, rect);
        }
        //private void SetReion()
        //{
        //    if (Region != null)
        //    {
        //        Region.Dispose();
        //    }
        //    SkinTools.CreateRegion(this, ClientRectangle, Round.All, RoundStyle.All);
        //}

        //public new Image Image
        //{
        //    get { return base.Image; }
        //    set
        //    {
        //        base.Image = value;
        //        SetReion();
        //    }
        //}
        //protected override void OnSizeChanged(EventArgs e)
        //{
        //    base.OnSizeChanged(e);
        //    SetReion();
        //}
        //protected override void OnSizeModeChanged(EventArgs e)
        //{
        //    base.OnSizeModeChanged(e);
        //    SetReion();
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
        //protected override void OnPaintBackground(PaintEventArgs pevent)
        //{
        //    base.OnPaintBackground(pevent);
        //    using (SolidBrush sb = new SolidBrush(Share.BackColor))
        //    {
        //        pevent.Graphics.FillPath(sb, rectPath);
        //    }
        //}
    }
}