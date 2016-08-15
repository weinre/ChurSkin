using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    public partial class CSliderGroupButton : UserControl
    {
        private Timer _timer;
        private EventHandler handler;
        private List<ItemString> items;
        private Padding round = new Padding(Share.DefaultRadius);
        private int x;

        public CSliderGroupButton()
        {
            SelectIndex = 0;
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
            UpdateStyles();
        }

        protected override Size DefaultMinimumSize
        {
            get { return new Size(90, 25); }
        }

        protected override Size DefaultSize
        {
            get { return new Size(120, 30); }
        }

        [Description("按钮的圆角大小")]
        public Padding Round
        {
            get { return round; }
            set
            {
                round = value;
                Invalidate();
            }
        }

        [TypeConverter(typeof (CollectionConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<ItemString> Items
        {
            get
            {
                if (items == null)
                    items = new List<ItemString>();
                // this.Invalidate();
                return items;
            }
        }

        [Description("选中序号"), DefaultValue(0)]
        public int SelectIndex { get; set; }

        private GraphicsPath rectPath
        {
            get { return DrawHelper.CreateRoundPath(ClientRectangle, round); }
        }

        private int width
        {
            get { return ClientRectangle.Width/items.Count; }
        }

        private Rectangle selectRect
        {
            get { return new Rectangle(x, 0, width, Height); }
        }

        private GraphicsPath selectPath
        {
            get { return DrawHelper.CreateRoundPath(selectRect, round); }
        }

        [Description("列表单击事件")]
        public event EventHandler ItemClick;

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            Share.GraphicSetup(g);

            var pen = new Pen(Share.BorderColor);
            var sb = new SolidBrush(Share.BackColor);
            var textRect = Rectangle.Empty;

            if (Enabled)
            {
                g.FillPath(sb, rectPath);
                g.DrawPath(pen, rectPath);
                if (items != null && items.Count > 0)
                    for (var i = 0; i < items.Count; i++)
                    {
                        textRect = new Rectangle(i*width, 0, width, Height);
                        if (SelectIndex == i)
                        {
                            pen.Color = Share.FocusBorderColor;
                            sb.Color = Share.FocusBackColor;
                            g.DrawPath(pen, selectPath);
                            g.FillPath(sb, selectPath);
                            TextRenderer.DrawText(g, items[i].Text, Font, textRect, Color.White,
                                TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
                        }
                        else
                            TextRenderer.DrawText(g, items[i].Text, Font, textRect, Color.Black,
                                TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
                    }
            }
            else
            {
                pen.Color = Share.DisableBorderColor;
                sb.Color = Share.DisabelBackColor;
                g.FillPath(sb, rectPath);
                g.DrawPath(pen, rectPath);
                if (items != null && items.Count > 0)
                    for (var i = 0; i < items.Count; i++)
                    {
                        textRect = new Rectangle(i*width, 0, width, Height);
                        if (SelectIndex == i)
                        {
                            g.DrawPath(pen, selectPath);
                            g.FillPath(sb, selectPath);
                        }
                        TextRenderer.DrawText(g, items[i].Text, Font, textRect, Share.DisabelFontColor,
                            TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
                    }
            }
            pen.Dispose();
            sb.Dispose();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            var rect = Rectangle.Empty;
            if (!DesignMode)
            {
                if (items != null && items.Count > 0)
                {
                    for (var i = 0; i < items.Count; i++)
                    {
                        rect = new Rectangle(i*width, 0, width, Height);
                        if (rect.Contains(e.Location))
                        {
                            SelectIndex = i;
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
                                    if (x < width*i)
                                    {
                                        x += 5;
                                        if (x > width*i)
                                            x = width*i;
                                    }
                                    else if (x > width*i)
                                    {
                                        x -= 5;
                                        if (x < width*i)
                                            x = width*i;
                                    }
                                    else
                                    {
                                        _timer.Stop();
                                        _timer.Dispose();
                                        if (ItemClick != null)
                                            ItemClick(this, EventArgs.Empty);
                                    }
                                    base.Refresh();
                                };
                            }
                            _timer.Tick += handler;
                            _timer.Start();
                            return;
                        }
                    }
                }
            }
        }
    }
}