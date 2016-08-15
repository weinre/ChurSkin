using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    public partial class CMacCheckBox : CheckBox
    {
        private Timer _timer;
        private EventHandler handler;
        private Rectangle radioRect = Rectangle.Empty;

        public CMacCheckBox()
        {
            SetStyles();
            InitializeComponent();
            Resize += (a, b) =>
            {
                if (!base.Checked)
                {
                    radioRect = new Rectangle(1, 1, Height - 2, Height - 2);
                }
                else
                {
                    radioRect = new Rectangle(Width - radioRect.Width - 1, 1, Height - 2, Height - 2);
                }
            };
            CheckedChanged += (c, d) => { Checked = base.Checked; };
        }

        internal GraphicsPath CheckPath
        {
            get { return DrawHelper.CreateRoundPath(radioRect, new Padding(radioRect.Height/2)); }
        }

        private Rectangle textRect
        {
            get
            {
                var sz = TextRenderer.MeasureText(Text, Font);
                if (!Checked)
                    return new Rectangle(radioRect.Right + 2, (Height - sz.Height)/2, sz.Width, sz.Height);
                return new Rectangle(-(Width - radioRect.Width - 5) + radioRect.X, (Height - sz.Height)/2,
                    Width - radioRect.Width - 3, sz.Height);
            }
        }

        internal GraphicsPath rectPath
        {
            get { return DrawHelper.CreateRoundPath(ClientRectangle, new Padding(Height/2)); }
        }

        [Description("是否选中状态"), DefaultValue(false)]
        public new bool Checked
        {
            get { return base.Checked; }
            set
            {
                base.Checked = value;
                if (!DesignMode)
                {
                    if (_timer != null)
                    {
                        _timer.Dispose();
                        handler = null;
                    }
                    _timer = new Timer();
                    _timer.Interval = 5;
                    if (handler == null)
                    {
                        handler = delegate
                        {
                            if (value)
                            {
                                if (radioRect.X >= Width - radioRect.Width - 3)
                                {
                                    radioRect.X = Width - radioRect.Width - 3;
                                    this._timer.Stop();
                                    _timer.Dispose();
                                }
                                radioRect.X += 3;
                            }
                            else
                            {
                                radioRect.X -= 3;
                                if (radioRect.X <= 1)
                                {
                                    radioRect.X = 1;
                                    this._timer.Stop();
                                }
                            }
                            base.Refresh();
                        };
                    }
                    _timer.Tick += handler;
                    _timer.Start();
                }
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            if (!DesignMode && !Visible) return;
            var g = pevent.Graphics;
            //g.SmoothingMode = SmoothingMode.AntiAlias;
            //g.TextRenderingHint = TextRenderingHint.AntiAlias;
            Share.GraphicSetup(g);
            var pen = new Pen(Share.BorderColor);
            var sb = new SolidBrush(Share.FocusBackColor);
            if (Enabled)
            {
                if (!base.Checked)
                {
                    g.DrawPath(pen, rectPath);
                    g.DrawPath(pen, CheckPath);
                    pen.Dispose();
                    TextRenderer.DrawText(g, Text, Font, textRect, Color.Black,
                        TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
                    //g.DrawString(base.Text, base.Font, Brushes.Black, new PointF(0, 0),StringFormatFlags.DirectionVertical);
                }
                else
                {
                    g.FillPath(sb, rectPath);
                    pen.Color = Share.FocusBackColor;
                    g.DrawPath(pen, rectPath);
                    sb.Color = Share.BackColor;
                    g.FillPath(sb, CheckPath);
                    sb.Dispose();
                    TextRenderer.DrawText(g, Text, Font, textRect, Color.White,
                        TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
                }
            }
            else
            {
                sb.Color = Share.DisabelBackColor;
                pen.Color = Share.DisableBorderColor;
                g.FillPath(sb, rectPath);
                g.DrawPath(pen, rectPath);
                g.FillPath(sb, CheckPath);
                g.DrawPath(pen, CheckPath);
                TextRenderer.DrawText(g, Text, Font, textRect, Share.DisabelFontColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
                pen.Dispose();
                sb.Dispose();
            }
            rectPath.Dispose();
            CheckPath.Dispose();
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
            Font = Share.DefaultFont;
            UpdateStyles();
        }
    }
}