using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    [DefaultEvent("ValueChanged")]
    public partial class CTrackBar : UserControl
    {
        private readonly int loadingH = 3; //进度高度
        private readonly int sliderbar = 18 - 1; //拖动圈圈大小
        private readonly int thumb = 18; //最小高度

        /// <summary>
        ///     最大取值
        /// </summary>
        private int _Maximum = 10;

        /// <summary>
        ///     最小取值
        /// </summary>
        private int _Minimum;

        /// <summary>
        ///     取值
        /// </summary>
        private int _Value;

        /// <summary>
        ///     拉杆的区域
        /// </summary>
        private Rectangle SliderBar;

        internal int sliderMouse;

        /// <summary>
        ///     拉杆的X坐标
        /// </summary>
        private int x;


        public CTrackBar()
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
        protected override Size DefaultMinimumSize
        {
            get { return new Size(80, thumb); }
        }

        protected override Size DefaultSize
        {
            get { return new Size(80, thumb); }
        }
        [Description("允许最大值")]
        public int Maximum
        {
            get { return _Maximum; }
            set
            {
                _Maximum = value;
                Invalidate();
            }
        }
        [Description("允许最小值")]
        public int Minimum
        {
            get { return _Minimum; }
            set
            {
                _Minimum = value;
                Invalidate();
            }
        }
        [Description("默认值")]
        public int Value
        {
            get { return _Value; }
            set
            {
                if (value > Maximum) value = Maximum;
                if (Maximum == 0) Maximum = 1;
                if (value < Minimum) value = Minimum;
                _Value = value;
                x = ((Width - sliderbar) * (_Value - Minimum)) / (Maximum - Minimum);
                if (x <= 0) x = 0;
                if (x >= (Width - sliderbar)) x = Width - thumb;
                OnValueChanged();
                Invalidate();
            }
        }

        /// <summary>
        ///     背景区域
        /// </summary>
        private Rectangle Slider
        {
            get { return new Rectangle(0, (Height - 2) / 2, Width, loadingH); }
        }

        internal GraphicsPath rectPath
        {
            get { return DrawHelper.CreateRoundPath2(Slider, Share.DefaultRadius / 2); }
        }

        /// <summary>
        ///     拉杆进度区域
        /// </summary>
        private Rectangle Thumb
        {
            get { return new Rectangle(0, (Height - 2) / 2, SliderBar.X + SliderBar.Width / 2, loadingH); }
        }

        private GraphicsPath ThumbPath
        {
            get { return DrawHelper.CreateRoundPath2(Thumb, Share.DefaultRadius / 2); }
        }

        //private int alpha = 160;

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
            SetStyle(ControlStyles.FixedHeight, AutoSize);
            BackColor = Color.Transparent;
            UpdateStyles();
        }

        //public delegate void EventHandler(object sender, EventArgs e);
        public event EventHandler ValueChanged;

        protected virtual void OnValueChanged()
        {
            if (ValueChanged != null)
                ValueChanged(this, EventArgs.Empty);
            //ValueChanged(this, e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            Share.GraphicSetup(g);

            SliderBar = new Rectangle(x, (Height - sliderbar) / 2, sliderbar, sliderbar);
            //画背景条
            var sb = new SolidBrush(bgColor);
            var p = new Pen(borderColor);
            g.FillPath(sb, rectPath);
            g.DrawPath(p, rectPath);
            //画进度
            sb.Color = Share.FocusBackColor;
            p.Color = Share.FocusBackColor;
            g.FillPath(sb, ThumbPath);
            g.DrawPath(p, ThumbPath);
            //画拉杆
            sb.Color = bgColor;
            p.Color = borderColor;

            g.FillEllipse(sb, SliderBar);
            g.DrawEllipse(p, SliderBar);
            sb.Dispose();
            p.Dispose();
            rectPath.Dispose();
            //  using (SolidBrush sb = new SolidBrush(this.BarColor))
            //  {
            // g.FillPath(new SolidBrush(Color.FromArgb(100, Color.Gray)), rectPath);
            // g.DrawPath(new Pen(Color.FromArgb(100, Color.Gray)), rectPath);
            //g.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.Gray)), Thumb);
            //g.DrawRectangle(new Pen(c), Thumb);
            // g.FillEllipse(sb, SliderBar);
            //g.DrawEllipse(new Pen(c), SliderBar);
        }

        //protected override void OnMouseEnter(EventArgs e)
        //{
        //    base.OnMouseEnter(e);
        //   // alpha = 190;
        //    base.Invalidate();
        //}
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            //alpha = 160; 
            sliderMouse = 0;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (sliderMouse == 1)
            {
                Value = (Maximum - Minimum) * (e.Location.X - 5) / (Width - sliderbar) + Minimum;
                //Value = (x * (Maximum + Minimum)) / (Width - 16);//x = e.Location.X;
            }
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            Value = (Maximum - Minimum) * (mevent.Location.X - 5) / (Width - sliderbar) + Minimum;
            //(x * (Maximum + Minimum)) / (Width - 16);//x = e.Location.X;
            if (SliderBar.Contains(mevent.Location))
            {
                sliderMouse = 1;
            }
            Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs mevent)
        {
            base.OnMouseWheel(mevent);
            Value += mevent.Delta / 120;
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            sliderMouse = 0;
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            if (Height > thumb || Height < thumb) Height = thumb;

            base.OnResize(e);
        }
    }
}