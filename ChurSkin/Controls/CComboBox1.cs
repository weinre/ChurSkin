using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace ChurSkins
{
    //[ToolboxBitmap(typeof(TextBox)), Designer(typeof(TextBox))]
    public partial class CComboBox1 : Control
    {
        public CComboBox1()
        {
            SetStyles();
            InitializeComponent();
            this.dropdown.LostFocus += (a, e) =>
            {
                mouseDown = false;
                parent.Controls.Remove(dropdown);
                this.Invalidate();
            };
            this.dropdown.MouseUp += (s, b) =>
            {
                this.Text = this.dropdown.SelectItem.Text;
                mouseDown = false;
                parent.Controls.Remove(dropdown);
                this.Invalidate();
            };
        }
        Control parent;
        internal GraphicsPath gp
        {
            get
            {
                return DrawHelper.CreateRoundPath2(base.ClientRectangle, 3);
            }
        }

        protected override Size DefaultMinimumSize
        {
            get
            {
                return new Size(100, 22);
            }
        }
        protected override Size DefaultMaximumSize { get { return new Size(500, 30); } }
        protected override Size DefaultSize
        {
            get { return new Size(120, 25); }
        }
        //Alpha alpha = Alpha.Normal;
        //Color bgColor = Color.Black;
        //public Color BgColor { get { return bgColor; } set { bgColor = value; this.Invalidate(); } }
        Rectangle rectTxt { get { return new Rectangle(3, 0, Width - 20, Height); } }
        Rectangle rectArrow { get { return new Rectangle(Width - 22, (Height - 22) / 2, 19, 22); } }
        protected override void OnPaint(PaintEventArgs pevent)
        {
            Graphics g = pevent.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            //g.TextRenderingHint = TextRenderingHint.AntiAlias;
            using (SolidBrush sb = new SolidBrush(color)) { g.FillPath(sb, gp); }
            using (Pen pen = new Pen(Color.FromArgb(130, 130, 130), 1)) { g.DrawPath(pen, gp); }
            gp.Dispose();
            TextRenderer.DrawText(g, Text, Font, rectTxt, ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.SingleLine | TextFormatFlags.Left);
            Bitmap arrow;
            if (mouseDown)
                arrow = Properties.Resources.inputbtn_down;
            else
                arrow = Properties.Resources.inputbtn_highlight;
            g.DrawImage(arrow, rectArrow);
            arrow.Dispose();
        }
        private List<ListItem> items;
        [TypeConverter(typeof(System.ComponentModel.CollectionConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public List<ListItem> Items
        {
            get
            {
                if (items == null)
                    items = new List<ListItem>();
                // this.Invalidate();
                return items;
            }
        }
        public int SelectedIndex;
        bool mouseDown = false;
        Alpha alpha = Alpha.Normal;
        Color color { get { return Color.FromArgb((int)alpha, bgColor.R, bgColor.G, bgColor.B); } }
        Color bgColor = Color.Black;
        public Color BgColor { get { return bgColor; } set { bgColor = value; this.Invalidate(); } }
        protected override void OnMouseEnter(EventArgs e)
        {
            this.Focus();
            base.OnMouseEnter(e);
            alpha = Alpha.MoveOrUp;
            base.Invalidate();
        }
        public event EventHandler SelectedIndexChanged;
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (SelectedIndexChanged != null)
                    SelectedIndexChanged(this, EventArgs.Empty);
                SelectedIndex = dropdown.SelectItem.Index;
                base.Text = value;
            }
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e); alpha = Alpha.Normal;
            base.Invalidate();
        }
        private CListBox dropdown = new CListBox();

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            dropdown.Items.Clear();
            if (Items.Count > 0)
            {
                foreach (var res in Items)
                {
                    ListItem a = new ListItem();
                    a.Text = res.Text;
                    dropdown.Items.Add(a);
                }
            }
            //this.Focus();
            base.OnMouseDown(mevent);
            alpha = Alpha.PressOrDown;
            parent = this.Parent;
            this.dropdown.Location = new Point(this.Location.X, this.Location.Y + Height + 1);
            if (parent is GroupBox || parent is CGroupBox)
            {
                parent = parent.Parent;
                this.dropdown.Location = new Point(this.Parent.Left + this.Left, this.Parent.Top + this.Top + this.Height + 1);
            }

            this.dropdown.Width = this.Width;
            this.dropdown.Height = Items.Count * dropdown.ItemHeight + Items.Count + 2;
            if (mouseDown)
            { mouseDown = false; parent.Controls.Remove(dropdown); }
            else
            { mouseDown = true; parent.Controls.Add(dropdown); }
            this.dropdown.BringToFront();
            base.Invalidate();
        }
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent); alpha = Alpha.MoveOrUp;
            base.Invalidate();
        }
        private void SetStyles()
        {
            this.SetStyle(
                   ControlStyles.AllPaintingInWmPaint |
                   ControlStyles.OptimizedDoubleBuffer |
                   ControlStyles.ResizeRedraw |
                   ControlStyles.Selectable |
                   ControlStyles.DoubleBuffer |
                   ControlStyles.SupportsTransparentBackColor |
                   ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.Opaque, false);
            this.BackColor = Color.Transparent;
            this.Font = new Font("微软雅黑", 9f);
            this.UpdateStyles();
        }

    }
}
