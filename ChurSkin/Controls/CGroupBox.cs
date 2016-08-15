using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    [ToolboxBitmap(typeof(GroupBox))]
    public partial class CGroupBox : GroupBox
    {
        public CGroupBox()
        {
            SetStyles();
            InitializeComponent();
            Font = Share.DefaultFont;
            borderColor = Share.BorderColor;
        }

        private Color borderColor;
        [Description("边框颜色")]
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; Invalidate(); }
        }
        //Alpha alpha = Alpha.Normal;
        //Color color { get { return Color.FromArgb((int)alpha, bgColor.R, bgColor.G, bgColor.B); } }
        //Color bgColor = Color.Black;
        //private GraphicsPath gp
        //{
        //    get
        //    {
        //        var rect = new Rectangle(0, 7, Width, Height - 7);
        //        return DrawHelper.CreateRoundPath2(rect, Share.DefaultRadius);
        //    }
        //}

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
        }

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;
        //        return cp;
        //    }
        //}
        protected override void OnPaint(PaintEventArgs e)
        {
            Share.GraphicSetup(e.Graphics);
            // SolidBrush sb = new SolidBrush(color);

            // e.Graphics.FillPath(sb, gp);
            //sb.Dispose(); 
            //sb = null;
            var r = 8;
            var arcRect = new Rectangle(1, 7, r, r);

            var pen = new Pen(borderColor, 1);
            //e.Graphics.DrawString(base.Text, base.Font, Brushes.Black, 10, 1);
            var sz = TextRenderer.MeasureText(Text, Font);
            var textRect = new Rectangle(10, 1, sz.Width, sz.Height);
            if (Text != "")
                DrawHelper.DrawCaptionText(e.Graphics, textRect, Text, Font, true, Color.White, 3, ForeColor);

            // 左上角
            e.Graphics.DrawArc(pen, arcRect, 180, 90);

            //// 右上角
            arcRect.X = Width - r - 1;
            e.Graphics.DrawArc(pen, arcRect, 270, 90);

            //// 右下角
            arcRect.Y = Height - r - 1;
            e.Graphics.DrawArc(pen, arcRect, 0, 90);

            // 左下角
            arcRect.X = 1;
            e.Graphics.DrawArc(pen, arcRect, 90, 90);

            e.Graphics.DrawLine(pen, 4, 7, 8, 7); //左上横
            e.Graphics.DrawLine(pen, e.Graphics.MeasureString(Text, Font).Width + 8, 7, Width - 6, 7); //右上横
            e.Graphics.DrawLine(pen, 1, 11, 1, Height - 6); //左竖
            e.Graphics.DrawLine(pen, 6, Height - 1, Width - 6, Height - 1); //下横
            e.Graphics.DrawLine(pen, Width - 1, 11, Width - 1, Height - 6); //右竖
            pen.Dispose();
            //pen = null;
        }

    }
}