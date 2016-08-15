using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    public partial class CListBox : Control
    {
        private readonly Alpha thumbAlpha = Alpha.Normal;
        private Color bgColor = Color.Black;
        private int itemHeight = 25;
        private List<ListItem> list;
        private string[] items;
        //鼠标划过项
        private ListItem mouseoverItem;
        //鼠标按下
        private Point ptMouseDown;
        //拉杆区域
        private Rectangle ScrollThumb;
        private MouseState thumbMouse;
        private int thumbY;
        private Padding round = new Padding(Share.DefaultRadius);
        private CComboBox comboxBox;
        /// 滚动条取值
        /// </summary>
        private int value;

        /// 虚拟的一个高度(控件中内容的高度)
        /// </summary>
        private int virtualHeight;

        public CListBox()
        {
            InitializeComponent();
            list = new List<ListItem>();
            SetStyles();
        }

        public CListBox(CComboBox ctr) : this()
        {
            this.Size = ctr.Size;
            this.comboxBox = ctr;
        }

        public void Clear()
        {
            this.list.Clear();
            resize();
            Invalidate();
        }

        public void Add(object item)
        {
            this.list.Add(new ListItem()
            {
                Text = item.ToString(),
                Index = this.list.Count
            });
            resize();
            Invalidate();
            //items.
        }

        public void AddRange(object[] items)
        {
            foreach (var item in items)
            {
                list.Add(new ListItem()
                {
                    Text = item.ToString(),
                    Index = this.list.Count
                });
            }
            resize();
            Invalidate();
        }

        //[TypeConverter(typeof(CollectionConverter))]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        //public List<ListItem> Items
        //{
        //    get
        //    {
        //        if (list == null)
        //            list = new List<ListItem>();
        //        // this.Invalidate();
        //        return list;
        //    }
        //}
        [Description("菜单项")]
        public virtual string[] Items
        {
            get { return this.items; }
            set
            {
                this.items = value;
                if (this.items != null)
                {
                    this.list.Clear();
                    for (int i = 0; i < this.items.Length; i++)
                    {
                        var em = new ListItem
                        {
                            Text = this.items[i],
                            Index = this.list.Count
                        };
                        this.list.Add(em);
                    }
                    resize();
                    //  base.Height = itemHeight * items.Length + 10;
                    this.Invalidate();
                }
            }
        }

        private void resize()
        {
            if (comboxBox == null) return;
            var h = 0;
            var count = list.Count > 6 ? 6 : list.Count;
            h = (ItemHeight + 1) * count + 2;

            this.Size = new Size(comboxBox.Width, h);
        }
        protected override Size DefaultMinimumSize
        {
            get { return new Size(55, 25); }
        }

        [Description("列表项的高度"), DefaultValue(25)]
        public int ItemHeight
        {
            get { return itemHeight; }
            set
            {
                itemHeight = value;
                Invalidate();
            }
        }

        //选中项
        [Browsable(false)]
        public ListItem SelectItem { get; set; }

        private int selectedIndex;
        [Browsable(false)]
        public int SelectedIndex
        {
            set
            {
                if (selectedIndex != value)
                {
                    selectedIndex = value;
                    if (this.list.Count > 0)
                    {
                        SelectItem = this.list.Find(s => s.Index == value);
                        if (comboxBox != null)
                            comboxBox.Text = SelectItem.Text;
                    }
                }
            }
            get { return SelectItem != null ? SelectItem.Index : 0; }
        }
        private int VirtualHeight
        {
            get { return virtualHeight; }
            set
            {
                if (value <= Height)
                {
                    if (this.value != 0)
                    {
                        this.value = 0;
                        Invalidate();
                    }
                }
                else
                {
                    if (value - this.value < Height)
                    {
                        this.value -= Height - value + this.value;
                        Invalidate();
                    }
                }
                virtualHeight = value;
            }
        }

        private int Value
        {
            get { return value; }
            set
            {
                if (value < 0)
                {
                    if (this.value == 0)
                        return;
                    this.value = 0;
                    Invalidate();
                    return;
                }
                if (value > virtualHeight - Height)
                {
                    if (this.value == virtualHeight - Height)
                        return;
                    this.value = virtualHeight - Height;
                    Invalidate();
                    return;
                }
                this.value = value;
                Invalidate();
            }
        }

        private Color thumbColor
        {
            get { return Color.FromArgb((int)thumbAlpha, bgColor.R, bgColor.G, bgColor.B); }
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
        public Color BgColor
        {
            get { return bgColor; }
            set
            {
                bgColor = value;
                Invalidate();
            }
        }

        public new ContextMenuStrip ContextMenuStrip { get; set; }

        /// <summary>
        ///     列表单击事件
        /// </summary>
        [Description("列表单击事件")]
        public event EventHandler ItemClick;

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
            ScrollThumb = new Rectangle(0, 10, 10, 10);
        }

        private Color GetColor(int alpha)
        {
            return Color.FromArgb(alpha, bgColor.R, bgColor.G, bgColor.B);
        }
        private GraphicsPath rectPath => DrawHelper.CreateRoundPath(ClientRectangle, round);

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!Visible) return;
            var g = e.Graphics;
            Share.GraphicSetup(g);

            using (var sb = new SolidBrush(thumbColor))
            {
                using (var pen = new Pen(thumbColor))
                {
                    g.FillPath(sb, rectPath);
                    g.DrawPath(pen, rectPath);
                    rectPath.Dispose();
                }
            }
            // if (DesignMode)
            //  {
            ScrollThumb.Width = 10;
            ScrollThumb.X = Width - 10;
            ScrollThumb.Height = (int)(((double)Height / virtualHeight) * (Height));
            ScrollThumb.Y = (int)(((double)value / (virtualHeight - Height)) * (Height - ScrollThumb.Height));
            if (ScrollThumb.Height < 20) ScrollThumb.Height = 20;


            g.TranslateTransform(0, -Value);
            var rectItem = new Rectangle(1, 1, Width - 2, ItemHeight);
            if (virtualHeight > Height) //如果超出了.就是存在滚动条
            {
                rectItem.Width -= 10;
            }
            Color fontColor = Color.Black;
            var font = Share.DefaultFont;
            var sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Near;
            sf.Trimming = StringTrimming.EllipsisCharacter; //超出指定矩形区域部分用"..."替代
            if (list != null && list.Count > 0)
            {
                for (var i = 0; i < list.Count; i++)
                {
                    fontColor = (BgColor == Color.Transparent) ? Color.Orange : Color.White;

                    if (list[i].Equals(mouseoverItem)) //鼠标划过
                    {
                        list[i].Alpha = (int)Alpha.Normal;
                    }
                    else if (list[i].Equals(SelectItem)) //选中
                    {
                        list[i].Alpha = (int)Alpha.MoveOrUp;
                    }
                    else
                    {
                        list[i].Alpha = (int)Alpha.None;
                        fontColor = Color.Black;
                    }
                    //var s = g.MeasureString(list[i].Text, font);
                    //var textRect = new RectangleF(list[i].Bounds.Left, i * ItemHeight + (ItemHeight - s.Height) / 2.0f,
                    //    list[i].Bounds.Width, s.Height);
                    list[i].Bounds = new Rectangle(rectItem.Location, rectItem.Size);
                    using (var sb = new SolidBrush(GetColor(list[i].Alpha)))
                    {
                        //画背景
                        g.FillRectangle(sb, rectItem);
                    }
                    using (var sb = new SolidBrush(fontColor))
                    {
                        //画文字
                        // TextRenderer.DrawText(g, list[i].Text, font, list[i].Bounds, fontColor, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
                        g.DrawString(list[i].Text, font, sb, list[i].Bounds, sf);
                    }

                    rectItem.Y = rectItem.Bottom + 1;
                }
            }
            g.ResetTransform();
            VirtualHeight = rectItem.Bottom - rectItem.Size.Height;

            //画滚动条 
            if (virtualHeight > Height)
            {
                var sb = new SolidBrush(thumbColor);
                var gp = DrawHelper.CreateRoundPath2(ScrollThumb, 8);
                var pen = new Pen(thumbColor);
                g.FillPath(sb, gp);
                g.DrawPath(pen, gp);
                sb.Dispose();
                pen.Dispose();
                gp.Dispose();
            }
            //  }
        }

        //绘制背景图
        //protected override void OnPaintBackground(PaintEventArgs pevent)
        //{
        //    base.OnPaintBackground(pevent);
        //    var g = pevent.Graphics;
        //    g.SmoothingMode = SmoothingMode.HighQuality;
        //    using (var gp = DrawHelper.CreateRoundPath2(ClientRectangle, 5))
        //    {
        //        var sb = new SolidBrush(thumbColor);
        //        var pen = new Pen(thumbColor);
        //        g.FillPath(sb, gp);
        //        g.DrawPath(pen, gp);
        //        sb.Dispose();
        //        pen.Dispose();
        //    }
        //}

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (!DesignMode)
            {
                Focus();
                if (ScrollThumb.Contains(e.Location))
                {
                    thumbMouse = MouseState.press;
                    ptMouseDown = e.Location;
                    thumbY = ScrollThumb.Y;
                }
            }
        }

        //protected override void OnMouseEnter(EventArgs e)
        //{
        //    base.OnMouseEnter(e);
        //}

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!DesignMode)
            {
                var mouse = e.Location;
                if (thumbMouse == MouseState.press)
                {
                    ScrollThumb.Y = thumbY + e.Location.Y - ptMouseDown.Y;

                    if (ScrollThumb.Y < 0) ScrollThumb.Y = 0;
                    if (ScrollThumb.Y > Height - ScrollThumb.Height) ScrollThumb.Y = Height - ScrollThumb.Height;
                    value = (int)((double)(ScrollThumb.Y) / (Height - ScrollThumb.Height) * (virtualHeight - Height));
                    Invalidate();
                }
                else
                {
                    if (list != null && list.Count > 0)
                    {
                        foreach (var li in list)
                        {
                            if (new Rectangle(0, li.Bounds.Y - Value, Width, li.Bounds.Height).Contains(mouse))
                            {
                                if (li != mouseoverItem)
                                {
                                    mouseoverItem = li;
                                    Invalidate();
                                }
                            }
                        }
                    }
                }
            }
            base.OnMouseMove(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (!DesignMode)
            {
                var mouse = e.Location;
                if (list != null && list.Count > 0)
                {
                    //foreach (ListItem li in Items)
                    for (var i = 0; i < list.Count; i++)
                    {
                        if (new Rectangle(0, list[i].Bounds.Y - Value, Width, list[i].Bounds.Height).Contains(mouse))
                        {
                            if (list[i] != SelectItem)
                            {
                                SelectItem = list[i];
                                Invalidate();
                                if (ItemClick != null)
                                    ItemClick(this, EventArgs.Empty);
                            }
                        }
                    }
                }
                //右键菜单
                if (ContextMenuStrip != null && e.Button == MouseButtons.Right)
                {
                    ContextMenuStrip.Show(this, mouse);
                }
                thumbMouse = MouseState.up;
            }
            base.OnMouseUp(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (!DesignMode)
            {
                if (list != null && list.Count > 0)
                    foreach (var li in list)
                    {
                        if (!li.Equals(SelectItem))
                            mouseoverItem = null;
                    }
                Invalidate();
            }
            base.OnMouseLeave(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta > 0) Value -= 26;
            if (e.Delta < 0) Value += 26;
            base.OnMouseWheel(e);
        }
    }
}