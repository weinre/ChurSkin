using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    [ToolboxBitmap(typeof(Button))]
    public partial class CButton : Button
    {
        private int _imageWidth = 18;
        private TextFormatFlags _textAlign = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
        private Color fontColor = Color.Black;
        private Color backColor;
        private Color borderColor;
        private ControlState cs = ControlState.Normal;
        //  private Point mouselocation = Point.Empty;
        private Padding round;
        private bool _checked;
        private string toolTipText;
        private ToolTip ToolTip;
        private Point imageoffset;
        private bool showBorder;
        public CButton() : base()
        {
            SetStyles();
            InitializeComponent();
            btnColor = Share.BackColor;
            borderColor = Share.BorderColor;
            Font = Share.DefaultFont;
            round = new Padding(Share.DefaultRadius);
            ToolTip = new ToolTip();
            //if (Focused)
            //{
            //    cs = ControlState.Pressed;
            //    backColor = borderColor = Share.FocusBackColor;
            //    ForeColor = Color.White;
            //}
            BackColor = Color.Transparent;
        }
        //public Point LocationToScreen
        //{
        //    get
        //    {
        //        Point locationToScreen = new Point();
        //        if (this.Parent is Control)
        //        {
        //            return ((Control)this.Parent).PointToScreen(this.Location);
        //        }
        //        //if (this.Parent is DuiBaseControl)
        //        //{
        //        //    locationToScreen = ((DuiBaseControl)this.Parent).LocationToScreen;
        //        //    locationToScreen.Offset(this.Location);
        //        //}
        //        return locationToScreen;
        //    }
        //}

        private Image bgImage;

        public Image BgImage
        {
            get { return bgImage; }
            set { bgImage = value; }
        }

        public  Color FontColor
        {
            get { return fontColor; }
            set { fontColor = value;Invalidate(); }
        }
        [Description("图标Image坐标偏移")]
        public Point ImageOffSet
        {
            get { return imageoffset; }
            set
            {
                imageoffset = value;
                Invalidate();
            }
        }
        [Description("是否显示边框")]
        public bool ShowBorder
        {
            get { return showBorder; }
            set { showBorder = value; Invalidate(); }
        }

        private Bitmap btnImage;
        [Description("图片按钮")]
        public Bitmap BtnImage { get { return btnImage; } set { btnImage = value; Invalidate(); } }

        private Color btnColor;
        [Description("按钮颜色")]
        public Color BtnColor { get { return btnColor; } set { btnColor = value; Invalidate(); } }

        [Description("按钮边框颜色")]
        public Color BorderColor { get { return borderColor; } set { borderColor = value; Invalidate(); } }

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        var parms = base.CreateParams;
        //        parms.Style &= ~0x02000000;  // Turn off WS_CLIPCHILDREN
        //        parms.Style &= ~0x00080000;  // Turn off WS_CLIPCHILDREN
        //        return parms;
        //    }
        //}
        //  private int radius = Share.DefaultRadius;
        //[Description("按钮的圆角")]
        //public int Radius
        //{
        //    get
        //    {
        //        radius = radius > Width ? Width : radius;
        //        radius = radius > Height ? Height : radius;
        //        //  radius = radius == 0 ? 1 : radius;
        //        return radius;
        //    }
        //    set { radius = value; this.Invalidate(); }
        //}
        //public override Color BackColor
        //{
        //    get { return Color.Transparent; }
        //}
        public string ToopTipText { get { return toolTipText; } set { toolTipText = value; ToolTip.SetToolTip(this, value); } }
        private GraphicsPath rectPath => DrawHelper.CreateRoundPath(ClientRectangle, round);

        //  return DrawHelper.CreateRoundPath2(ClientRectangle, radius); //old


        [Description("按钮是否被选中"), DefaultValue(false)]
        public bool Checked
        {
            get { return _checked; }
            set
            {
                BringToFront();
                _checked = value;
                Invalidate();
            }
        }


        protected override void OnResize(EventArgs e)
        {
            this.Visible = false;
            base.OnResize(e);
            this.Visible = true;
            //SkinTools.CreateRegion(this, ClientRectangle, Round.Left *2, RoundStyle.All);
        }
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

        //public new bool Enabled
        //{
        //    get { return base.Enabled; }
        //    set
        //    {
        //        base.Enabled = value;
        //        Invalidate();
        //    }
        //}

        public int ImageWidth
        {
            get { return _imageWidth; }
            set
            {
                if (value != _imageWidth)
                {
                    _imageWidth = value < 12 ? 12 : value;
                    Invalidate();
                }
            }
        }

        [Description("按钮上文字的对齐方式")]
        public override ContentAlignment TextAlign
        {
            get { return base.TextAlign; }
            set
            {
                base.TextAlign = value;
                switch (base.TextAlign)
                {
                    case ContentAlignment.BottomCenter:
                        _textAlign = TextFormatFlags.Bottom |
                                     TextFormatFlags.HorizontalCenter |
                                     TextFormatFlags.SingleLine;
                        break;
                    case ContentAlignment.BottomLeft:
                        _textAlign = TextFormatFlags.Bottom |
                                     TextFormatFlags.Left |
                                     TextFormatFlags.SingleLine;
                        break;
                    case ContentAlignment.BottomRight:
                        _textAlign = TextFormatFlags.Bottom |
                                     TextFormatFlags.Right |
                                     TextFormatFlags.SingleLine;
                        break;
                    case ContentAlignment.MiddleCenter:
                        _textAlign = TextFormatFlags.SingleLine |
                                     TextFormatFlags.HorizontalCenter |
                                     TextFormatFlags.VerticalCenter;
                        break;
                    case ContentAlignment.MiddleLeft:
                        _textAlign = TextFormatFlags.Left |
                                     TextFormatFlags.VerticalCenter |
                                     TextFormatFlags.SingleLine;
                        break;
                    case ContentAlignment.MiddleRight:
                        _textAlign = TextFormatFlags.Right |
                                     TextFormatFlags.VerticalCenter |
                                     TextFormatFlags.SingleLine;
                        break;
                    case ContentAlignment.TopCenter:
                        _textAlign = TextFormatFlags.Top |
                                     TextFormatFlags.HorizontalCenter |
                                     TextFormatFlags.SingleLine;
                        break;
                    case ContentAlignment.TopLeft:
                        _textAlign = TextFormatFlags.Top |
                                     TextFormatFlags.Left |
                                     TextFormatFlags.SingleLine;
                        break;
                    case ContentAlignment.TopRight:
                        _textAlign = TextFormatFlags.Top |
                                     TextFormatFlags.Right |
                                     TextFormatFlags.SingleLine;
                        break;
                }
                Invalidate();
            }
        }

        private void DrawBackGroundImage(Graphics g)
        {
            g.SetClip(rectPath);
            var rect = Rectangle.Empty;
            switch (BackgroundImageLayout)
            {
                case ImageLayout.None:
                    rect = new Rectangle(0, 0, BgImage.Width, BgImage.Height);
                    break;
                case ImageLayout.Center:
                    rect = new Rectangle((Width - BgImage.Width) / 2,
                        (Height - BgImage.Height) / 2,
                        BgImage.Width,
                        BgImage.Height);
                    break;
                case ImageLayout.Stretch:
                    rect = ClientRectangle;
                    break;
                case ImageLayout.Tile:
                    if (BgImage.Width > Width && Height > BgImage.Height)
                        g.DrawImage(BgImage, ClientRectangle);
                    else
                    {
                        var fillX = (int)Math.Ceiling(Width / (double)BgImage.Width);
                        var fillY = (int)Math.Ceiling(Height / (double)BgImage.Height);
                        for (var x = 0; x <= fillX; x++) //画X轴
                        {
                            for (var y = 0; y <= fillY; y++) //画Y轴
                            {
                                var rectXY = new Rectangle(BgImage.Width * x, BgImage.Height * y,
                                    BgImage.Width, BgImage.Height);
                                g.DrawImage(BgImage, rectXY);
                            }
                        }
                    }
                    return;
                case ImageLayout.Zoom:
                    if (Width > BgImage.Width)
                    {
                        rect.X = (Width - BgImage.Width) / 2;
                        rect.Width = BgImage.Width;
                    }
                    else
                    {
                        rect.X = 0;
                        rect.Width = Width;
                    }
                    if (Height > BgImage.Height)
                    {
                        rect.Y = (Height - BgImage.Height) / 2;
                        rect.Height = BgImage.Height;
                    }
                    else
                    {
                        rect.Y = 0;
                        rect.Height = Height;
                    }
                    break;
            }
            var xy = (cs == ControlState.Pressed) ? 1 : 0;
            rect.X += xy;
            rect.Y += xy;
            g.DrawImage(BgImage, rect);
        }

        void drawBtn(Graphics gs)
        {
            using (var bitmap = new Bitmap(btnImage.Width / 3, btnImage.Height))
            {
                using (var g = Graphics.FromImage(bitmap))
                {
                    var i = 0;
                    i = (cs == ControlState.Hover) ? i = 1 : ((cs == ControlState.Pressed || _checked) ? 2 : 0);
                    g.DrawImage(btnImage, new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        new Rectangle(i * btnImage.Width / 3, 0, btnImage.Width / 3, btnImage.Height), GraphicsUnit.Pixel);
                    RenderHelper.RenderFormBorder(bitmap, 5, gs, ClientRectangle);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle imageRect;
            Rectangle textRect;
            CalculateRect(out imageRect, out textRect);
            //按钮背景
            if (BgImage != null) DrawBackGroundImage(e.Graphics);

            if (btnImage != null)
            {
                //e.Graphics.Clip = new Region(rectPath);
                drawBtn(e.Graphics);
            }
            else
            {
                SetColor();
                using (var sb = new SolidBrush(backColor))
                {
                    using (var p = new Pen(borderColor))
                    {
                        Share.GraphicSetup(e.Graphics);
                        e.Graphics.FillPath(sb, rectPath); //画背景
                        if (showBorder) e.Graphics.DrawPath(p, rectPath);  //画边框
                        rectPath.Dispose();
                    }
                }
            }

            //按钮图标
            if (Image != null)
            {
                imageRect.X += imageoffset.X;
                imageRect.Y += imageoffset.Y;
                e.Graphics.DrawImage(Image, imageRect);
            }
            //按钮文字
            if (Text != "")
                TextRenderer.DrawText(e.Graphics, Text, Font, textRect, fontColor, _textAlign);

            //   DrawHelper.DrawCaptionText(g, textRect, base.Text, base.Font, true, Color.White, 1, base.ForeColor);

            //}
            //g1.DrawImage(drawimg, 0, 0);
            // drawimg.Dispose();
            #region old

            //using (SolidBrush sb = new SolidBrush(color)) { g.FillPath(sb, gp); }//画背景

            //Rectangle clientRectangle = base.ClientRectangle;

            //Image image = null;
            //Size empty = Size.Empty;
            //if (base.Image != null)
            //{
            //    if (string.IsNullOrEmpty(this.Text))
            //    {
            //        image = base.Image;
            //        empty = new Size(image.Width, image.Height);
            //        clientRectangle.Inflate(-4, -4);
            //        if ((empty.Width * empty.Height) != 0)
            //        {
            //            Rectangle withinThis = clientRectangle;
            //            withinThis = ImageDrawRect.HAlignWithin(empty, withinThis, base.ImageAlign);
            //            withinThis = ImageDrawRect.VAlignWithin(empty, withinThis, base.ImageAlign);
            //            if (!base.Enabled)
            //            {
            //                ControlPaint.DrawImageDisabled(g, image, withinThis.Left, withinThis.Top, this.BackColor);
            //            }
            //            else
            //            {
            //                g.DrawImage(image, withinThis.Left + num, withinThis.Top + num, image.Width, image.Height);
            //            }
            //        }
            //    }
            //    else
            //    {
            //        g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            //        g.DrawImage(base.Image, imageRect, -num, -num, base.Image.Width, base.Image.Height, GraphicsUnit.Pixel);
            //    }
            //}
            //else if ((base.ImageList != null) && (base.ImageIndex != -1))
            //{
            //    image = base.ImageList.Images[base.ImageIndex];
            //}
            ////画图片
            ////if (Image != null)
            ////{
            ////    g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            ////    g.DrawImage(
            ////        Image,
            ////        imageRect,
            ////        0,
            ////        0,
            ////        Image.Width,
            ////        Image.Height,
            ////        GraphicsUnit.Pixel);
            ////}
            ////g.TextRenderingHint = TextRenderingHint.AntiAlias;
            //if (!base.Enabled)
            //{
            //    alpha = Alpha.DisEnable;
            //    base.ForeColor = Color.DarkGray;
            //}
            //else
            //    base.ForeColor = Color.Black;
            //using (Pen pen = new Pen(bdcolor))
            //{
            //    g.DrawPath(pen, gp);
            //    if (this.Focused)
            //    {
            //        pen.DashStyle = DashStyle.Dash;
            //        g.DrawPath(pen, gp1);
            //        gp1.Dispose();
            //    }
            //}
            //if (mouseDown && base.Enabled) { textRect.X++; textRect.Y++; }
            //// _textAlign.HotkeyPrefix = this.ShowKeyboardCues ? HotkeyPrefix.Show : HotkeyPrefix.Hide;
            //Size sz = TextRenderer.MeasureText(base.Text, base.Font);
            //if (Image == null)
            //{
            //    textRect.Y = (this.Height - sz.Height) / 2;
            //}
            //textRect.X = (Width - sz.Width) / 2;
            //  //TextRenderer.DrawText(g, base.Text, new Font("微软雅黑", base.Font.Size), textRect, base.ForeColor, this._textAlign);
            //gp.Dispose();

            #endregion
        }

        private void CalculateRect(out Rectangle imageRect, out Rectangle textRect)
        {
            imageRect = Rectangle.Empty;
            textRect = Rectangle.Empty;
            if (Image == null)
            {
                textRect = new Rectangle(
                    2,
                    0,
                    Width - 4,
                    Height);
                return;
            }
            switch (TextImageRelation)
            {
                case TextImageRelation.Overlay:
                    imageRect = new Rectangle(
                        2,
                        (Height - ImageWidth) / 2,
                        ImageWidth,
                        ImageWidth);
                    textRect = new Rectangle(
                        2,
                        0,
                        Width - 4,
                        Height);
                    break;
                case TextImageRelation.ImageAboveText:
                    imageRect = new Rectangle(
                        (Width - ImageWidth) / 2,
                        2 + Padding.Top,
                        ImageWidth,
                        ImageWidth);
                    textRect = new Rectangle(
                        2,
                        imageRect.Bottom,
                        Width,
                        Height - imageRect.Bottom - 2);
                    break;
                case TextImageRelation.ImageBeforeText:
                    imageRect = new Rectangle(
                        2,
                        (Height - ImageWidth) / 2,
                        ImageWidth,
                        ImageWidth);
                    textRect = new Rectangle(
                        imageRect.Right + 2,
                        0,
                        Width - imageRect.Right - 4,
                        Height);
                    break;
                case TextImageRelation.TextAboveImage:
                    imageRect = new Rectangle(
                        (Width - ImageWidth) / 2,
                        Height - ImageWidth - 2,
                        ImageWidth,
                        ImageWidth);
                    textRect = new Rectangle(
                        0,
                        2,
                        Width,
                        Height - imageRect.Y - 2);
                    break;
                case TextImageRelation.TextBeforeImage:
                    imageRect = new Rectangle(
                        Width - ImageWidth - 2,
                        (Height - ImageWidth) / 2,
                        ImageWidth,
                        ImageWidth);
                    textRect = new Rectangle(
                        2,
                        0,
                        imageRect.X - 2,
                        Height);
                    break;
            }

            if (RightToLeft == RightToLeft.Yes)
            {
                imageRect.X = Width - imageRect.Right;
                textRect.X = Width - textRect.Right;
            }
        }

        void SetColor()
        {
            if (!base.Enabled)
            {
                btnColor = Share.DisabelBackColor;
                ForeColor = Share.DisabelFontColor;
                //  borderColor = Share.DisableBorderColor;
                return;
            }
            if (_checked)
            {
                btnColor = Share.FocusBackColor;
                // ForeColor = Share.FocusBorderColor;
                //  borderColor = Color.White;
                return;
            }
            if (cs == ControlState.Normal)
            {

                backColor = btnColor;
                //  borderColor = Share.BorderColor;
                ForeColor = Color.Black;
            }
            else if (cs == ControlState.Pressed)
            {
                backColor = Share.FocusBackColor;
                // borderColor = Share.FocusBorderColor;
                ForeColor = Color.White;
            }
            else
            {
                backColor = Color.FromArgb(150, Share.BackColor);
                // borderColor = Color.FromArgb(150, Share.BorderColor);
                ForeColor = Color.Black;
            }
        }

        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            base.OnMouseMove(mevent);

            cs = (cs == ControlState.Pressed) ? ControlState.Pressed : cs = ControlState.Hover;

            base.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            cs = ControlState.Normal;
            Invalidate();
        }
        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            //this.Focus();
            base.OnMouseDown(mevent);
            cs = ControlState.Pressed;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            cs = (ClientRectangle.Contains(mevent.Location)) ? ControlState.Hover : ControlState.Normal;
            Invalidate();
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
        //protected override void WndProc(ref Message m)
        //{
        //    if (m.Msg == 0x0014) // 禁掉清除背景消息
        //        return;
        //    base.WndProc(ref m);
        //}
    }
}