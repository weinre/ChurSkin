using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using Win32.Consts;

namespace System.Windows.Forms
{
    public partial class QQSpitButton : Button
    {
        /// <summary>
        ///     鼠标的当前位置
        /// </summary>
        public enum ButtonMousePosition
        {
            None,
            Button,
            Splitebutton
        }

        /// <summary>
        ///     控件的状态。
        /// </summary>
        public enum ControlState
        {
            /// <summary>
            ///     正常
            /// </summary>
            Normal,

            /// <summary>
            ///     鼠标经过
            /// </summary>
            Hover,

            /// <summary>
            ///     鼠标按下
            /// </summary>
            Pressed
        }

        private bool _alwaysShowBorder;
        private Color _arrowColor = Color.FromArgb(64, 64, 64);
        private Color _baseColor = Color.FromArgb(10, 66, 204, 255);
        private Color _baseColorEnd = Color.FromArgb(200, 66, 204, 255);
        private Color _borderColor = Color.FromArgb(161, 162, 160);
        private bool _contextHandle;
        private int _contextOffset = 5;
        private bool _contextOpened;
        private ControlState _controlState;
        private int _imageHeight = 24;
        private int _imageTextSpace = 2;
        private int _imageWidth = 24;
        private Color _innerBorderColor = Color.FromArgb(200, 255, 255, 255);
        private ButtonMousePosition _mousePosition;
        private bool _pressOffset = true;
        private int _radius = 2;
        private RoundStyle _roundStyle = RoundStyle.All;
        private bool _showSpliteButton;
        private int _spliteButtonWidth = 18;

        /// <summary>
        public QQSpitButton()
        {
            SetStyle(
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, false);

            BackColor = Color.Transparent;
        }

        [DefaultValue(5)]
        [Description("下拉菜单与按钮的距离")]
        public int ContextOffset
        {
            get { return _contextOffset; }
            set { _contextOffset = value; }
        }

        [DefaultValue(false)]
        [Description("是否启用分割按钮")]
        public bool ShowSpliteButton
        {
            get { return _showSpliteButton; }
            set
            {
                if (_showSpliteButton != value)
                {
                    _showSpliteButton = value;
                    Invalidate();
                }
            }
        }

        [DefaultValue(18)]
        [Description("分割按钮的宽度")]
        public int SpliteButtonWidth
        {
            get { return _spliteButtonWidth; }
            set
            {
                if (_spliteButtonWidth != value)
                {
                    _spliteButtonWidth = value;
                    Invalidate();
                }
            }
        }

        [DefaultValue(true)]
        [Description("当鼠标按下时图片和文字是否产生偏移")]
        public bool PressOffset
        {
            get { return _pressOffset; }
            set { _pressOffset = value; }
        }

        [DefaultValue(false)]
        [Description("是否一直显示按钮边框\n设置为false则只在鼠标经过和按下时显示边框")]
        public bool AlwaysShowBorder
        {
            get { return _alwaysShowBorder; }
            set
            {
                if (_alwaysShowBorder != value)
                {
                    _alwaysShowBorder = value;
                    Invalidate();
                }
            }
        }

        [DefaultValue(typeof (Color), "64,64,64")]
        [Description("当显示分割按钮时，分割按钮的箭头颜色")]
        public Color ArrowColor
        {
            get { return _arrowColor; }
            set
            {
                if (_arrowColor != value)
                {
                    _arrowColor = value;
                    Invalidate();
                }
            }
        }

        [DefaultValue(typeof (Color), "161, 162, 160")]
        [Description("按钮的边框颜色")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                if (_borderColor != value)
                {
                    _borderColor = value;
                    Invalidate();
                }
            }
        }

        [DefaultValue(typeof (Color), "200, 255, 255, 255")]
        [Description("按钮内边框颜色")]
        public Color InnerBorderColor
        {
            get { return _innerBorderColor; }
            set
            {
                if (_innerBorderColor != value)
                {
                    _innerBorderColor = value;
                    Invalidate();
                }
            }
        }

        [DefaultValue(typeof (Color), "10 ,66, 204, 160")]
        [Description("鼠标经过和按下时按钮的渐变背景颜色")]
        public Color BaseColor
        {
            get { return _baseColor; }
            set
            {
                if (_baseColor != value)
                {
                    _baseColor = value;
                }
            }
        }

        [DefaultValue(typeof (Color), "200 ,66, 204, 160")]
        [Description("鼠标经过和按下时按钮的渐变背景颜色")]
        public Color BaseColorEnd
        {
            get { return _baseColorEnd; }
            set
            {
                if (_baseColorEnd != value)
                {
                    _baseColorEnd = value;
                }
            }
        }

        [DefaultValue(24)]
        [Description("图片宽度")]
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

        [DefaultValue(24)]
        [Description("图片高度")]
        public int ImageHeight
        {
            get { return _imageHeight; }
            set
            {
                if (value != _imageHeight)
                {
                    _imageHeight = value < 12 ? 12 : value;
                    Invalidate();
                }
            }
        }

        [DefaultValue(typeof (RoundStyle), "1")]
        [Description("按钮圆角样式")]
        public RoundStyle RoundStyle
        {
            get { return _roundStyle; }
            set
            {
                if (_roundStyle != value)
                {
                    _roundStyle = value;
                    Invalidate();
                }
            }
        }

        [DefaultValue(2)]
        [Description("按钮圆角弧度")]
        public int Radius
        {
            get { return _radius; }
            set
            {
                if (_radius != value)
                {
                    _radius = value < 2 ? 2 : value;
                    Invalidate();
                }
            }
        }

        [DefaultValue(2)]
        [Description("图片与文字之间的间距")]
        public int ImageTextSpace
        {
            get { return _imageTextSpace; }
            set
            {
                if (_imageTextSpace != value)
                {
                    _imageTextSpace = value < 0 ? 0 : value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        ///     按钮当前状态
        /// </summary>
        internal ControlState ControlStates
        {
            get { return _controlState; }
            set
            {
                if (_controlState != value)
                {
                    _controlState = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        ///     鼠标当前所在位置
        /// </summary>
        internal ButtonMousePosition CurrentMousePosition
        {
            get { return _mousePosition; }
            set
            {
                if (_mousePosition != value)
                {
                    _mousePosition = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        ///     普通按钮矩形位置
        /// </summary>
        internal Rectangle ButtonRect
        {
            get
            {
                if (ShowSpliteButton)
                {
                    return new Rectangle(0, 0, ClientRectangle.Width - SpliteButtonWidth, ClientRectangle.Height);
                }
                return ClientRectangle;
            }
        }

        /// <summary>
        ///     分割按钮矩形位置
        /// </summary>
        internal Rectangle SpliteButtonRect
        {
            get
            {
                if (ShowSpliteButton)
                {
                    return new Rectangle(ClientRectangle.Width - SpliteButtonWidth, 0, SpliteButtonWidth,
                        ClientRectangle.Height);
                }
                return Rectangle.Empty;
            }
        }

        /// <summary>
        ///     普通按钮按下事件
        /// </summary>
        public event EventHandler OnButtonClick;

        /// <summary>
        ///     分割按钮按下事件
        /// </summary>
        public event EventHandler OnSpliteButtonClick;

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (!_contextOpened)
            {
                ControlStates = ControlState.Normal;
                CurrentMousePosition = ButtonMousePosition.None;
            }
        }

        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            base.OnMouseMove(mevent);
            if (ClientRectangle.Contains(mevent.Location))
            {
                ControlStates = ControlState.Hover;
                if (ShowSpliteButton)
                {
                    CurrentMousePosition = ButtonRect.Contains(mevent.Location)
                        ? ButtonMousePosition.Button
                        : ButtonMousePosition.Splitebutton;
                }
                else
                {
                    CurrentMousePosition = ButtonMousePosition.Button;
                }
            }
            else
            {
                ControlStates = ControlState.Normal;
                CurrentMousePosition = ButtonMousePosition.None;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                ControlStates = ControlState.Pressed;
                if (ShowSpliteButton)
                {
                    CurrentMousePosition = ButtonRect.Contains(e.Location)
                        ? ButtonMousePosition.Button
                        : ButtonMousePosition.Splitebutton;
                }
                else
                {
                    CurrentMousePosition = ButtonMousePosition.Button;
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                if (ClientRectangle.Contains(e.Location))
                {
                    ControlStates = ControlState.Hover;
                    if (ShowSpliteButton)
                    {
                        CurrentMousePosition = ButtonRect.Contains(e.Location)
                            ? ButtonMousePosition.Button
                            : ButtonMousePosition.Splitebutton;
                        if (CurrentMousePosition == ButtonMousePosition.Splitebutton)
                        {
                            if (OnSpliteButtonClick != null)
                            {
                                OnSpliteButtonClick(this, EventArgs.Empty);
                            }
                            if (ContextMenuStrip != null)
                            {
                                if (!_contextHandle)
                                {
                                    _contextHandle = true;
                                    ContextMenuStrip.Opening += ContextMenuStrip_Opening;
                                    ContextMenuStrip.Closed += ContextMenuStrip_Closed;
                                }
                                ContextMenuStrip.Opacity = 1.0;
                                ContextMenuStrip.Show(this, 0, Height + ContextOffset);
                            }
                        }
                        else
                        {
                            if (OnButtonClick != null)
                            {
                                OnButtonClick(this, EventArgs.Empty);
                            }
                        }
                    }
                    else
                    {
                        CurrentMousePosition = ButtonMousePosition.Button;
                    }
                }
                else
                {
                    ControlStates = ControlState.Normal;
                    CurrentMousePosition = ButtonMousePosition.None;
                }
            }
        }

        private void ContextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            _contextOpened = false;
            ControlStates = ControlState.Normal;
            CurrentMousePosition = ButtonMousePosition.None;
            Invalidate();
        }

        private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            _contextOpened = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            OnPaintBackground(e);

            var g = e.Graphics;
            Rectangle imageRect;
            Rectangle textRect;

            CalculateRect(out imageRect, out textRect, g);
            g.SmoothingMode = SmoothingMode.AntiAlias;


            //画边框与背景
            RenderBackGroundInternal(
                g,
                ClientRectangle,
                RoundStyle,
                Radius
                );
            //画图像
            if (Image != null)
            {
                g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                g.DrawImage(
                    Image,
                    imageRect,
                    0,
                    0,
                    Image.Width,
                    Image.Height,
                    GraphicsUnit.Pixel);
            }
            //画文字
            if (Text != "")
            {
                TextRenderer.DrawText(
                    g,
                    Text,
                    Font,
                    textRect,
                    ForeColor,
                    GetTextFormatFlags(TextAlign, RightToLeft == RightToLeft.Yes));
            }
            //画分割按钮
            if (ShowSpliteButton)
            {
                RenderSpliteButton(g, ClientRectangle);
            }
        }

        /// <summary>
        ///     获取图像以及文字的位置
        /// </summary>
        /// <param name="imageRect"></param>
        /// <param name="textRect"></param>
        private void CalculateRect(
            out Rectangle imageRect, out Rectangle textRect, Graphics g)
        {
            imageRect = Rectangle.Empty;
            textRect = Rectangle.Empty;
            var textSize = g.MeasureString(Text, Font);
            if (Image == null)
            {
                switch (TextAlign)
                {
                    case ContentAlignment.BottomCenter:
                        textRect = new Rectangle((ButtonRect.Width - (int) textSize.Width)/2,
                            Height - (int) textSize.Height - 3, (int) textSize.Width, (int) textSize.Height);
                        break;
                    case ContentAlignment.BottomLeft:
                        textRect = new Rectangle(2, Height - (int) textSize.Height - 3, (int) textSize.Width,
                            (int) textSize.Height);
                        break;
                    case ContentAlignment.BottomRight:
                        textRect = new Rectangle(ButtonRect.Width - (int) textSize.Width - 3,
                            Height - (int) textSize.Height - 3, (int) textSize.Width, (int) textSize.Height);
                        break;
                    case ContentAlignment.MiddleCenter:
                        textRect = new Rectangle((ButtonRect.Width - (int) textSize.Width)/2,
                            (Height - (int) textSize.Height)/2, (int) textSize.Width, (int) textSize.Height);
                        break;
                    case ContentAlignment.MiddleLeft:
                        textRect = new Rectangle(2, (Height - (int) textSize.Height)/2, (int) textSize.Width,
                            (int) textSize.Height);
                        break;
                    case ContentAlignment.MiddleRight:
                        textRect = new Rectangle(ButtonRect.Width - (int) textSize.Width - 3,
                            (Height - (int) textSize.Height)/2, (int) textSize.Width, (int) textSize.Height);
                        break;
                    case ContentAlignment.TopCenter:
                        textRect = new Rectangle((ButtonRect.Width - (int) textSize.Width)/2, 2, (int) textSize.Width,
                            (int) textSize.Height);
                        break;
                    case ContentAlignment.TopLeft:
                        textRect = new Rectangle(2, 2, (int) textSize.Width, (int) textSize.Height);
                        break;
                    case ContentAlignment.TopRight:
                        textRect = new Rectangle(ButtonRect.Width - (int) textSize.Width - 3, 2, (int) textSize.Width,
                            (int) textSize.Height);
                        break;
                }
                if (PressOffset && ControlStates == ControlState.Pressed &&
                    CurrentMousePosition == ButtonMousePosition.Button)
                {
                    textRect.X += 1;
                    textRect.Y += 1;
                }
                if (RightToLeft == RightToLeft.Yes)
                {
                    textRect.X = ButtonRect.Width - textRect.Right;
                }
                return;
            }
            if (Text == "")
            {
                switch (ImageAlign)
                {
                    case ContentAlignment.BottomCenter:
                        imageRect = new Rectangle((ButtonRect.Width - ImageWidth)/2, Height - ImageHeight - 3,
                            ImageWidth, ImageHeight);
                        break;
                    case ContentAlignment.BottomLeft:
                        imageRect = new Rectangle(2, Height - ImageHeight - 3, ImageWidth, ImageHeight);
                        break;
                    case ContentAlignment.BottomRight:
                        imageRect = new Rectangle(ButtonRect.Width - ImageWidth - 3, Height - ImageHeight - 3,
                            ImageWidth, ImageHeight);
                        break;
                    case ContentAlignment.MiddleCenter:
                        imageRect = new Rectangle((ButtonRect.Width - ImageWidth)/2, (Height - ImageHeight)/2,
                            ImageWidth, ImageHeight);
                        break;
                    case ContentAlignment.MiddleLeft:
                        imageRect = new Rectangle(2, (Height - ImageHeight)/2, ImageWidth, ImageHeight);
                        break;
                    case ContentAlignment.MiddleRight:
                        imageRect = new Rectangle(ButtonRect.Width - ImageWidth - 3, (Height - ImageHeight)/2,
                            ImageWidth, ImageHeight);
                        break;
                    case ContentAlignment.TopCenter:
                        imageRect = new Rectangle((ButtonRect.Width - ImageWidth)/2, 2, ImageWidth, ImageHeight);
                        break;
                    case ContentAlignment.TopLeft:
                        imageRect = new Rectangle(2, 2, ImageWidth, ImageHeight);
                        break;
                    case ContentAlignment.TopRight:
                        imageRect = new Rectangle(ButtonRect.Width - ImageWidth - 3, 2, ImageWidth, ImageHeight);
                        break;
                }
                if (PressOffset && ControlStates == ControlState.Pressed &&
                    CurrentMousePosition == ButtonMousePosition.Button)
                {
                    imageRect.X += 1;
                    imageRect.Y += 1;
                }
                if (RightToLeft == RightToLeft.Yes)
                {
                    imageRect.X = ButtonRect.Width - imageRect.Right;
                }
                return;
            }
            switch (TextImageRelation)
            {
                case TextImageRelation.Overlay:
                    imageRect = new Rectangle(
                        (ButtonRect.Width - ImageWidth - (int) textSize.Width - ImageTextSpace)/2,
                        (Height - ImageHeight)/2,
                        ImageWidth,
                        ImageHeight);
                    textRect = new Rectangle(
                        imageRect.Right + ImageTextSpace,
                        (Height - (int) textSize.Height)/2,
                        (int) textSize.Width,
                        (int) textSize.Height);
                    break;
                case TextImageRelation.ImageAboveText:
                    imageRect = new Rectangle(
                        (ButtonRect.Width - ImageWidth)/2,
                        (Height - ImageHeight - (int) textSize.Height - ImageTextSpace)/2,
                        ImageWidth,
                        ImageHeight);
                    textRect = new Rectangle(
                        (ButtonRect.Width - (int) textSize.Width)/2,
                        imageRect.Bottom + ImageTextSpace,
                        (int) textSize.Width,
                        (int) textSize.Height);
                    break;
                case TextImageRelation.ImageBeforeText:
                    imageRect = new Rectangle(
                        (ButtonRect.Width - ImageWidth - (int) textSize.Width - ImageTextSpace)/2,
                        (Height - ImageHeight)/2,
                        ImageWidth,
                        ImageHeight);
                    textRect = new Rectangle(
                        imageRect.Right + ImageTextSpace,
                        (Height - (int) textSize.Height)/2,
                        (int) textSize.Width,
                        (int) textSize.Height);
                    break;
                case TextImageRelation.TextAboveImage:
                    textRect = new Rectangle(
                        (ButtonRect.Width - (int) textSize.Width)/2,
                        (Height - (int) textSize.Height - ImageHeight - ImageTextSpace)/2,
                        (int) textSize.Width,
                        (int) textSize.Height);
                    imageRect = new Rectangle(
                        (ButtonRect.Width - ImageWidth)/2,
                        textRect.Bottom + ImageTextSpace,
                        ImageWidth,
                        ImageHeight);
                    break;
                case TextImageRelation.TextBeforeImage:
                    textRect = new Rectangle(
                        (ButtonRect.Width - ImageWidth - (int) textSize.Width - ImageTextSpace)/2,
                        (Height - (int) textSize.Height)/2,
                        (int) textSize.Width,
                        (int) textSize.Height);
                    imageRect = new Rectangle(
                        textRect.Right + ImageTextSpace,
                        (Height - ImageHeight)/2,
                        ImageWidth,
                        ImageHeight);
                    break;
            }
            if (PressOffset && ControlStates == ControlState.Pressed &&
                CurrentMousePosition == ButtonMousePosition.Button)
            {
                imageRect.X += 1;
                imageRect.Y += 1;
                textRect.X += 1;
                textRect.Y += 1;
            }

            if (RightToLeft == RightToLeft.Yes)
            {
                imageRect.X = ButtonRect.Width - imageRect.Right;
                textRect.X = ButtonRect.Width - textRect.Right;
            }
        }

        /// <summary>
        ///     画边框与背景
        /// </summary>
        internal void RenderBackGroundInternal(
            Graphics g,
            Rectangle rect,
            RoundStyle style,
            int roundWidth
            )
        {
            if (ControlStates != ControlState.Normal || AlwaysShowBorder)
            {
                rect.Width--;
                rect.Height--;
                if (style != RoundStyle.None)
                {
                    using (var path = DrawHelper.CreateRoundPath(rect, roundWidth, style, false))
                    {
                        if (ControlStates != ControlState.Normal)
                        {
                            using (
                                var brush = (ControlStates == ControlState.Pressed)
                                    ? new LinearGradientBrush(rect, BaseColorEnd, BaseColor,
                                        LinearGradientMode.ForwardDiagonal)
                                    : new LinearGradientBrush(rect, BaseColor, BaseColorEnd, LinearGradientMode.Vertical)
                                )
                            {
                                if (!ShowSpliteButton)
                                {
                                    g.FillPath(brush, path);
                                }
                                else
                                {
                                    if (CurrentMousePosition == ButtonMousePosition.Button)
                                    {
                                        using (
                                            var buttonpath = DrawHelper.CreateRoundPath(ButtonRect, roundWidth,
                                                RoundStyle.Left, true))
                                        {
                                            g.FillPath(brush, buttonpath);
                                        }
                                    }
                                    else
                                    {
                                        using (
                                            var splitepath = DrawHelper.CreateRoundPath(SpliteButtonRect, roundWidth,
                                                RoundStyle.Right, true))
                                        {
                                            g.FillPath(brush, splitepath);
                                        }
                                    }
                                }
                            }
                        }
                        using (var pen = new Pen(_borderColor))
                        {
                            g.DrawPath(pen, path);
                        }
                    }
                    rect.Inflate(-1, -1);
                    using (var path = DrawHelper.CreateRoundPath(rect, roundWidth, style, false))
                    {
                        using (var pen = new Pen(InnerBorderColor))
                        {
                            g.DrawPath(pen, path);
                        }
                    }
                }
                else
                {
                    if (ControlStates != ControlState.Normal)
                    {
                        using (
                            var brush = (ControlStates == ControlState.Pressed)
                                ? new LinearGradientBrush(rect, BaseColorEnd, BaseColor,
                                    LinearGradientMode.ForwardDiagonal)
                                : new LinearGradientBrush(rect, BaseColor, BaseColorEnd, LinearGradientMode.Vertical))
                        {
                            if (!ShowSpliteButton)
                            {
                                g.FillRectangle(brush, rect);
                            }
                            else
                            {
                                if (CurrentMousePosition == ButtonMousePosition.Button)
                                {
                                    g.FillRectangle(brush, ButtonRect);
                                }
                                else
                                {
                                    g.FillRectangle(brush, SpliteButtonRect);
                                }
                            }
                        }
                    }
                    using (var pen = new Pen(_borderColor))
                    {
                        g.DrawRectangle(pen, rect);
                    }
                    rect.Inflate(-1, -1);
                    using (var pen = new Pen(InnerBorderColor))
                    {
                        g.DrawRectangle(pen, rect);
                    }
                }
            }
        }

        /// <summary>
        ///     画分割按钮
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        internal void RenderSpliteButton(Graphics g, Rectangle rect)
        {
            var points = new Point[3];
            points[0] = new Point(rect.Width - SpliteButtonWidth + 2, (rect.Height - 4)/2);
            points[1] = new Point(rect.Width - SpliteButtonWidth + 2 + 8, (rect.Height - 4)/2);
            points[2] = new Point(rect.Width - SpliteButtonWidth + 2 + 4, (rect.Height - 4)/2 + 4);

            if (PressOffset && ControlStates == ControlState.Pressed &&
                CurrentMousePosition == ButtonMousePosition.Splitebutton)
            {
                points[0].X += 1;
                points[0].Y += 1;
                points[1].X += 1;
                points[1].Y += 1;
                points[2].X += 1;
                points[2].Y += 1;
            }
            using (var brush = new SolidBrush(ArrowColor))
            {
                g.FillPolygon(brush, points);
            }
        }

        internal static TextFormatFlags GetTextFormatFlags(
            ContentAlignment alignment,
            bool rightToleft)
        {
            var flags = TextFormatFlags.WordBreak |
                        TextFormatFlags.SingleLine;
            if (rightToleft)
            {
                flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
            }

            switch (alignment)
            {
                case ContentAlignment.BottomCenter:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.BottomLeft:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Left;
                    break;
                case ContentAlignment.BottomRight:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Right;
                    break;
                case ContentAlignment.MiddleCenter:
                    flags |= TextFormatFlags.HorizontalCenter |
                             TextFormatFlags.VerticalCenter;
                    break;
                case ContentAlignment.MiddleLeft:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                    break;
                case ContentAlignment.MiddleRight:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                    break;
                case ContentAlignment.TopCenter:
                    flags |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.TopLeft:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Left;
                    break;
                case ContentAlignment.TopRight:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Right;
                    break;
            }
            return flags;
        }
    }
}

//public enum RoundStyle
//{
//    /// <summary>
//    /// 四个角都不是圆角。
//    /// </summary>
//    None = 0,
//    /// <summary>
//    /// 四个角都为圆角。
//    /// </summary>
//    All = 1,
//    /// <summary>
//    /// 左边两个角为圆角。
//    /// </summary>
//    Left = 2,
//    /// <summary>
//    /// 右边两个角为圆角。
//    /// </summary>
//    Right = 3,
//    /// <summary>
//    /// 上边两个角为圆角。
//    /// </summary>
//    Top = 4,
//    /// <summary>
//    /// 下边两个角为圆角。
//    /// </summary>
//    Bottom = 5,
//    BottomLeft = 6,
//    BottomRight
//}

//public static class GraphicsPathHelper
//{
//    /// <summary>
//    /// 建立带有圆角样式的矩形路径
//    /// </summary>
//    /// <param name="rect">用来建立路径的矩形。</param>
//    /// <param name="_radius">圆角的大小</param>
//    /// <param name="style">圆角的样式</param>
//    /// <param name="correction">是否把矩形长宽减 1,以便画出边框</param>
//    /// <returns>建立的路径</returns>
//    public static GraphicsPath CreatePath(
//        Rectangle rect, int radius, RoundStyle style, bool correction)
//    {
//        GraphicsPath path = new GraphicsPath();
//        int radiusCorrection = correction ? 1 : 0;
//        switch (style)
//        {
//            case RoundStyle.None:
//                path.AddRectangle(rect);
//                break;
//            case RoundStyle.All:
//                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
//                path.AddArc(
//                    rect.Right - radius - radiusCorrection,
//                    rect.Y,
//                    radius,
//                    radius,
//                    270,
//                    90);
//                path.AddArc(
//                    rect.Right - radius - radiusCorrection,
//                    rect.Bottom - radius - radiusCorrection,
//                    radius,
//                    radius, 0, 90);
//                path.AddArc(
//                    rect.X,
//                    rect.Bottom - radius - radiusCorrection,
//                    radius,
//                    radius,
//                    90,
//                    90);
//                break;
//            case RoundStyle.Left:
//                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
//                path.AddLine(
//                    rect.Right - radiusCorrection, rect.Y,
//                    rect.Right - radiusCorrection, rect.Bottom - radiusCorrection);
//                path.AddArc(
//                    rect.X,
//                    rect.Bottom - radius - radiusCorrection,
//                    radius,
//                    radius,
//                    90,
//                    90);
//                break;
//            case RoundStyle.Right:
//                path.AddArc(
//                    rect.Right - radius - radiusCorrection,
//                    rect.Y,
//                    radius,
//                    radius,
//                    270,
//                    90);
//                path.AddArc(
//                   rect.Right - radius - radiusCorrection,
//                   rect.Bottom - radius - radiusCorrection,
//                   radius,
//                   radius,
//                   0,
//                   90);
//                path.AddLine(rect.X, rect.Bottom - radiusCorrection, rect.X, rect.Y);
//                break;
//            case RoundStyle.Top:
//                path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
//                path.AddArc(
//                    rect.Right - radius - radiusCorrection,
//                    rect.Y,
//                    radius,
//                    radius,
//                    270,
//                    90);
//                path.AddLine(
//                    rect.Right - radiusCorrection, rect.Bottom - radiusCorrection,
//                    rect.X, rect.Bottom - radiusCorrection);
//                break;
//            case RoundStyle.Bottom:
//                path.AddArc(
//                    rect.Right - radius - radiusCorrection,
//                    rect.Bottom - radius - radiusCorrection,
//                    radius,
//                    radius,
//                    0,
//                    90);
//                path.AddArc(
//                    rect.X,
//                    rect.Bottom - radius - radiusCorrection,
//                    radius,
//                    radius,
//                    90,
//                    90);
//                path.AddLine(rect.X, rect.Y, rect.Right - radiusCorrection, rect.Y);
//                break;
//            case RoundStyle.BottomLeft:
//                path.AddArc(
//                    rect.X,
//                    rect.Bottom - radius - radiusCorrection,
//                    radius,
//                    radius,
//                    90,
//                    90);
//                path.AddLine(rect.X, rect.Y, rect.Right - radiusCorrection, rect.Y);
//                path.AddLine(
//                    rect.Right - radiusCorrection,
//                    rect.Y,
//                    rect.Right - radiusCorrection,
//                    rect.Bottom - radiusCorrection);
//                break;
//            case RoundStyle.BottomRight:
//                path.AddArc(
//                    rect.Right - radius - radiusCorrection,
//                    rect.Bottom - radius - radiusCorrection,
//                    radius,
//                    radius,
//                    0,
//                    90);
//                path.AddLine(rect.X, rect.Bottom - radiusCorrection, rect.X, rect.Y);
//                path.AddLine(rect.X, rect.Y, rect.Right - radiusCorrection, rect.Y);
//                break;
//        }
//        path.CloseFigure();

//        return path;
//    }
//}