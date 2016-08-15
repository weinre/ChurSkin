using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    [ToolboxBitmap(typeof(CheckBox))]
    public partial class CCheckBox : CheckBox
    {
        public CCheckBox()
        {
            SetStyles();
            InitializeComponent();
            borderColor = Share.BorderColor;
            bgColor = Share.BackColor;
        }

        private Color borderColor;
        private Color bgColor;

        [Description("边框颜色")]
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; Invalidate(); }
        }
        [Description("背景颜色")]
        public Color BgColor
        {
            get { return bgColor; }
            set { bgColor = value; Invalidate(); }
        }
        internal Rectangle radioRect
        {
            get { return new Rectangle(0, (Height - 14) / 2, 14, 14); }
        }

        internal GraphicsPath CheckPath
        {
            get
            {
                var gp = new GraphicsPath();
                gp.AddLine(radioRect.X + 3, radioRect.Y + 7, radioRect.X + 6, radioRect.Y + 10);
                gp.AddLine(radioRect.X + 6, radioRect.Y + 10, radioRect.X + 11, radioRect.Y + 3);
                return gp;
            }
        }

        //public override string Text
        //{
        //    get
        //    {
        //        return base.Text;
        //    }
        //    set
        //    {
        //        base.Text = value; this.Invalidate();
        //    }
        //}
        internal GraphicsPath radioPath
        {
            get { return DrawHelper.CreateRoundPath2(radioRect, 4); }
        }

        //Alpha alpha = Alpha.Normal;
        //Color bdcolor { get { return Color.FromArgb((int)Alpha.a180, bgColor.R, bgColor.G, bgColor.B); } }
        //Color color { get { return Color.FromArgb((int)alpha, bgColor.R, bgColor.G, bgColor.B); } }
        //Color bgColor = Share.BackColor;
        //public Color BgColor { get { return bgColor; } set { bgColor = value; this.Invalidate(); } }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            if (!DesignMode && !Visible) return;
            var g = pevent.Graphics;
            //g.SmoothingMode = SmoothingMode.AntiAlias;
            //g.TextRenderingHint = TextRenderingHint.AntiAlias;
            Share.GraphicSetup(g);
            if (!Checked)
            {
                using (var sb = new SolidBrush(BgColor))
                {
                    g.FillPath(sb, radioPath);
                }
                using (var p = new Pen(borderColor, 1))
                {
                    g.DrawPath(p, radioPath);
                }
                //g.DrawPath(new Pen(Color.White, 1.8f), gp);
            }
            else
            {
                using (var sb = new SolidBrush(Share.FocusBackColor))
                {
                    g.FillPath(sb, radioPath);
                    var p = new Pen(Share.FocusBackColor, 1);
                    g.DrawPath(p, radioPath);
                    p.Dispose();
                }
                using (var p = new Pen(Share.BackColor, 1.55f))
                {
                    g.DrawPath(p, CheckPath);
                }
            }
            var TextRect = new Rectangle(radioRect.Width + 3, (int)(Height - g.MeasureString(Text, Font).Height) / 2,
                Width - radioRect.Width - 3, Height);
            //TextRenderer.DrawText(g, base.Text, base.Font, TextRect, base.ForeColor, TextFormatFlags.VerticalCenter |
            //                       TextFormatFlags.Left |
            //                       TextFormatFlags.SingleLine);
            if (Text != "")
                DrawHelper.DrawCaptionText(g, TextRect, Text, Font, true, Color.White, 3, ForeColor);

            radioPath.Dispose();
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