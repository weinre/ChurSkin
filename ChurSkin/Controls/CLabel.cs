using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    public partial class CLabel : Label
    {
        private Padding round = new Padding(Share.DefaultRadius);
        private bool showBorder;

        public CLabel()
        {
            Font = Share.DefaultFont;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
                     ControlStyles.DoubleBuffer | ControlStyles.UserPaint, true);
            BackColor = Color.Transparent;
            UpdateStyles();
            InitializeComponent();
        }

        private GraphicsPath rectPath
        {
            get { return DrawHelper.CreateRoundPath(ClientRectangle, round); }
        }

        [Description("规定四个角的圆角")]
        public Padding Round
        {
            get
            {
                round.Left = round.Left <= 0 ? 1 : round.Left;
                round.Top = round.Top <= 0 ? 1 : round.Top;
                round.Right = round.Right <= 0 ? 1 : round.Right;
                round.Bottom = round.Bottom <= 0 ? 1 : round.Bottom;
                return round;
            }
            set
            {
                round = value;
                Refresh();
            }
        }

        [Description("是否显示边框"), DefaultValue(false)]
        public bool ShowBorder
        {
            get { return showBorder; }
            set
            {
                showBorder = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            if (showBorder)
            {
                Share.GraphicSetup(e.Graphics);
                e.Graphics.SetClip(rectPath);
                var pen = new Pen(Share.BorderColor);
                var sb = new SolidBrush(Share.BackColor);
                e.Graphics.FillPath(sb, rectPath);
                e.Graphics.DrawPath(pen, rectPath);
                pen.Dispose();
                sb.Dispose();
            }
            var sz = TextRenderer.MeasureText(Text, Font);
            //Rectangle textRect = new Rectangle(base.Padding.Left, base.Padding.Top,
            //    base.Padding.Left + base.Padding.Right + sz.Width + base.Margin.Left,
            //   base.Padding.Left + sz.Height);
            //if (base.Text != "")
            DrawHelper.DrawCaptionText(e.Graphics, ClientRectangle, Text, Font, true, Color.White, 3, ForeColor);
        }
    }
}