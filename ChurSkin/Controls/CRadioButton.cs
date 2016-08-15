using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    public partial class CRadioButton : RadioButton
    {
        //Alpha alpha = Alpha.Normal;
        //Color bdcolor { get { return Color.FromArgb((int)Alpha.a180, bgColor.R, bgColor.G, bgColor.B); } }
        //Color color { get { return Color.FromArgb((int)alpha, bgColor.R, bgColor.G, bgColor.B); } }
        //Color bgColor = Color.Black;
        //public Color BgColor { get { return bgColor; } set { bgColor = value; this.Invalidate(); } }
        private readonly float wh = 14f; //圆圈的宽高
        private readonly float wh1 = 5f; // 圆圈里面的宽高

        public CRadioButton()
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
        internal RectangleF TextRect //文本
        {
            get { return new RectangleF(radioRect.Width + 3f, 2f, Width - wh - 3f, Height); }
        }

        internal RectangleF radioRect //选中框
        {
            get { return new RectangleF(0f, (Height - wh) / 2f, wh, wh); }
        }

        internal RectangleF innerRadioRect //圆点
        {
            get { return new RectangleF((radioRect.Right - radioRect.Left - wh1) / 2f, (Height - wh1) / 2f, wh1, wh1); }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;
            Share.GraphicSetup(g);

            var sb = new SolidBrush(Share.FocusBackColor);
            var p = new Pen(Share.FocusBackColor);
            if (Checked)
            {
                g.FillEllipse(sb, radioRect);
                g.DrawEllipse(p, radioRect);
                sb.Color = Share.BackColor;
                g.FillEllipse(sb, innerRadioRect);
            }
            else
            {
                sb.Color = bgColor;
                g.FillEllipse(sb, radioRect);
                p.Color = borderColor;
                g.DrawEllipse(p, radioRect);
            }
            sb.Dispose();
            p.Dispose();
            //TextRenderer.DrawText(g, base.Text, base.Font, TextRect, base.ForeColor, TextFormatFlags.VerticalCenter |
            //                       TextFormatFlags.Left |
            //                       TextFormatFlags.SingleLine);
            if (Text != "")
            {
                var TextRect = new RectangleF(radioRect.Width + 3f,
                    ((float)Height - g.MeasureString(Text, Font).Height) / 2f + .5f,
                    Width - radioRect.Width - 3f, Height);
                DrawHelper.DrawCaptionText(g, TextRect, Text, Font, true, Color.White, 3, ForeColor);
            }
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
            UpdateStyles();
            Font = Share.DefaultFont;
        }
    }
}