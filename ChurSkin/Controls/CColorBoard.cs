using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    public partial class CColorBoard : UserControl
    {
        private readonly Image ColorImage = Properties.Resources.color;
        private readonly int padding = 5;

        private readonly Color[] selectColor =
        {
            Color.Black, Color.Red, Color.Orange, Color.Yellow, Color.Green,
            Color.GreenYellow, Color.Blue, Color.Purple, Color.Gray
        };
        private Padding round = new Padding(Share.DefaultRadius);

        private Color color = Color.Black;
        private MouseState ms;
        private Rectangle r = Rectangle.Empty;
        private int selectColorIndex = -1;

        public CColorBoard()
        {
            InitializeComponent();
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
            r = new Rectangle((Width - 10) / 2, (Height - selectWidth - padding * 2) / 2, 10, 10);
        }

        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                OnValueChanged();
            }
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
        //protected override void OnResize(EventArgs e)
        //{
        //    base.OnResize(e);
        //    GraphicsPath _gp = DrawHelper.CreateRoundPath(Width, Height, 1);
        //    this.Region = new Region(_gp);
        //}
        //protected override void OnPaintBackground(PaintEventArgs e)
        //{
        //    base.OnPaintBackground(e);
        //    Graphics g = e.Graphics;
        //    g.SmoothingMode = SmoothingMode.AntiAlias;
        //    g.InterpolationMode = InterpolationMode.High;
        //    g.DrawImage(img, ClientRectangle);
        //}
        private GraphicsPath rectPath
        {
            get { return DrawHelper.CreateRoundPath(ClientRectangle, round); }
        }

        private int selectWidth
        {
            get { return (int)Math.Round((Width - 10 * padding) / 9d); }
        }

        //Alpha alpha = Alpha.Normal;
        //Color _color { get { return Color.FromArgb((int)alpha, bgColor.R, bgColor.G, bgColor.B); } }
        //Color bgColor = Color.Black;
        //public Color BgColor { get { return bgColor; } set { bgColor = value; this.Invalidate(); } }
        private Rectangle colorImageRect
        {
            get { return new Rectangle(0, selectWidth + padding * 2, Width, Height - selectWidth); }
        }

        private Rectangle[] selectRect
        {
            get
            {
                var rect = new Rectangle[9];
                for (var i = 0; i <= 8; i++)
                {
                    rect[i] = new Rectangle((selectWidth * i) + padding + padding * i, padding, selectWidth, selectWidth);
                }
                return rect;
            }
        }

        public event EventHandler ColorChanged;

        private void OnValueChanged()
        {
            if (ColorChanged != null)
                ColorChanged(this, EventArgs.Empty);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //   base.OnPaint(e);
            var g = e.Graphics;
            Share.GraphicSetup(g);
            var sb = new SolidBrush(Share.BackColor);
            var p = new Pen(Share.BorderColor, 1);
            g.FillPath(sb, rectPath);
            GraphicsPath gPath = null;
            for (var i = 0; i <= 8; i++)
            {
                sb.Color = selectColor[i];
                p.Color = selectColor[i];
                // g.FillRectangle(sb, selectRect[i]);
                // g.DrawRectangle(p, selectRect[i]);
                gPath = DrawHelper.CreateRoundPath2(selectRect[i], Share.DefaultRadius);
                g.FillPath(sb, gPath);
                g.DrawPath(p, gPath);
            }
            g.DrawImage(ColorImage, colorImageRect); //取色图片

            p.Color = Share.BackColor;
            p.Width = 2;
            if (selectColorIndex != -1)
            {
                var rect = selectRect[selectColorIndex];
                rect.Inflate(-1, -1);
                gPath = DrawHelper.CreateRoundPath2(rect, Share.DefaultRadius);
                g.DrawPath(p, gPath);
            }
            gPath.Dispose();

            p.Color = Share.BorderColor;
            sb.Color = Share.BackColor;
            g.DrawPath(p, rectPath); //控件边框

            p.Width = 2;
            p.Color = Color.Black;

            g.DrawEllipse(p, r); //圈圈
            r.Inflate(1, 1);
            p.Color = Color.White;
            g.DrawEllipse(p, r); //圈圈
            r.Inflate(-1, -1);

            sb.Dispose();
            p.Dispose();
        }

        private void GetSelectIndex(Point point)
        {
            for (var i = 0; i <= 8; i++)
            {
                if (selectRect[i].Contains(point))
                {
                    selectColorIndex = i;
                    Color = selectColor[i];
                    return;
                }
            }
        }
         

        private Color GetColor()
        {
            var b = new Bitmap(colorImageRect.Width, colorImageRect.Height);
            var g = Graphics.FromImage(b);
            Share.GraphicSetup(g);

            g.DrawImage(ColorImage, new Rectangle(0, 0, colorImageRect.Width, colorImageRect.Height));
            // g.CopyFromScreen(new Point(MousePosition.X, MousePosition.Y), new Point(0, 0), b.Size);
            //  else
            //    g.CopyFromScreen(new Point(r.X, r.Y), new Point(0, 0), b.Size);
            //  if (r.X - 5 > b.Width) r.X = b.Width;
            // if (r.Y - 5 > b.Height) r.Y = b.Height;
            var x = r.X + 5;
            var y = r.Y - (Height - colorImageRect.Height) + 5;
            if (x >= b.Width) x = b.Width - 1;
            if (y >= b.Height) y = b.Height - 1;
            if (x <= 0) x = 1;
            if (y <= 0) y = 1;
            // y = b.Height - y;
            var color = b.GetPixel(x, y);
            g.Dispose();
            b.Dispose();
            return color;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (colorImageRect.Contains(e.Location))
            {
                r.X = e.X - 5;
                r.Y = e.Y - 5;
                GetSelectIndex(e.Location);
            }
            ms = MouseState.press;
            GetSelectIndex(e.Location);
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (ms == MouseState.press && e.Location.Y >= colorImageRect.Y - 5)
            {
                r.X = e.X - 5;
                r.Y = e.Y - 5;
                if (r.X < -5) r.X = -5;
                if (r.Y < colorImageRect.Y - 5) r.Y = colorImageRect.Y - 5;
                if (r.X > Width - 5) r.X = Width - 5;
                if (r.Y > Height - 5) r.Y = Height - 5;
                Color = GetColor();
                Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            ms = MouseState.leave;
            if (colorImageRect.Contains(e.Location))
                Color = GetColor();
        }
    }
}