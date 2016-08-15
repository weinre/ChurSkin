using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    public partial class CLine : UserControl
    {

        private Color starColor = Color.Gray;
        private Color endColor = Color.White;
        private int angle = 0;
        private int direction = 1;

        public CLine()
        {
            SetStyles();
            InitializeComponent();
        }

        [Description("开始颜色")]
        public Color StarColor
        {
            get { return starColor; }
            set { starColor = value; Invalidate(); }
        }
        [Description("结束颜色")]
        public Color EdColor
        {
            get { return endColor; }
            set { endColor = value; Invalidate(); }
        }
        [Description("角度")]
        public int Angle
        {
            get { return angle; }
            set { angle = value; Invalidate(); }
        }
        [Description("方向，1为水平，0为垂直")]
        public int Direction
        {
            get { return direction; }
            set
            {
                direction = value;
                if (value == 1)
                {
                    Width = Height; Height = 1;
                }
                else
                {
                    Height = Width; Width = 1;
                }
                Invalidate();
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
                ControlStyles.EnableNotifyMessage |
                ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.Opaque, false);
            base.UpdateStyles();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rect = ClientRectangle;
            using (var brush = new LinearGradientBrush(rect, starColor, endColor, angle))
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
                rect.X++;
                rect.Y++;
                using (var sb = new SolidBrush(Color.White))
                {
                    e.Graphics.FillRectangle(sb, rect);
                }
            }
        }
    }
}
