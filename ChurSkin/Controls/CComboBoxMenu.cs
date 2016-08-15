using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.CompilerServices;

namespace System.Windows.Forms
{
    //  [ToolboxItem(false)]
    public partial class CComboBoxMenu : Control

    {
        public CComboBoxMenu()
        {
            InitializeComponent();
        }

        private Color fontColor;
        private Color fontHoverColor;
        private Color bgColor;
        private CComboBox dmcomboBox;
        private int itemHeight;
        private int mRadius;
        public List<CMenuItem> list;
        private bool showBorder;

        public CComboBoxMenu(CComboBox dmcomboBox)
        {
            this.itemHeight = 35;
            this.fontColor = Color.White;
            this.mRadius = 0;
            this.Font = Share.DefaultFont;
            this.Init();
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.Selectable | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.Opaque, false);
            base.BackColor = Color.Transparent;
            base.UpdateStyles();
            this.dmcomboBox = dmcomboBox;
        }

        private void DMComboBoxMenu_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < this.list.Count; i++)
            {
                if (this.list[i].Bounds.Contains(base.PointToClient(Control.MousePosition)))
                {
                 //   this.dmcomboBox.SelectItem = this.list[i];
                //    this.dmcomboBox.method_6();
                    base.Visible = false;
                    return;
                }
            }
        }

        private void DMComboBoxMenu_MouseMove(object sender, MouseEventArgs e)
        {
            for (var i = 0; i < this.list.Count; i++)
            {
                this.list[i].Move = this.list[i].Bounds.Contains(e.Location) ? true : false;
            }
            base.Invalidate();
        }

        private void Init()
        {
            base.SuspendLayout();
            base.Size = new Size(0xb1, 0x68);
            base.Click += new EventHandler(this.DMComboBoxMenu_Click);
            base.MouseMove += new MouseEventHandler(this.DMComboBoxMenu_MouseMove);
            base.ResumeLayout(false);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var dc = e.Graphics;
            dc.SmoothingMode = SmoothingMode.AntiAlias;
            dc.TextRenderingHint = TextRenderingHint.AntiAlias;
            //背景
            using (var sb = new SolidBrush(bgColor))
            {
                dc.FillRectangle(sb, 0, 0, base.Width, base.Height);
            }
             
            var y = showBorder ? 1 : 0;

            if (list != null && list.Count >= 1)
            {
                base.Height = (this.itemHeight * this.list.Count) + y * 2; //这里加2 就是为了防止边框盖住子项
                for (var i = 0; i < this.list.Count; i++)
                {
                    this.list[i].Bounds = new Rectangle(2, y, base.Width - 4, this.itemHeight);
                    if (this.list[i].Move)
                    {
                        fontHoverColor = fontHoverColor == Color.Empty ? Color.FromArgb(100, bgColor) : fontHoverColor;
                        using (var sb1 = new SolidBrush(fontHoverColor))
                        {
                            dc.FillRectangle(sb1, this.list[i].Bounds);
                        }
                    }
                    TextRenderer.DrawText(e.Graphics, this.list[i].Text, Font, this.list[i].Bounds, ForeColor,
                        TextFormatFlags.VerticalCenter);

                    //  TextRenderer.DrawText(dc, this.list[i].Text, this.Font, new Point(5, y), this.FontColor,TextFormatFlags.VerticalCenter);
                    y += this.itemHeight;
                }
            }
            //边框
            if (showBorder)
            {
                using (var path = DrawHelper.CreateRoundPath(ClientRectangle, mRadius))
                {
                    using (var pen = new Pen(Color.FromArgb(100, bgColor)))
                    {
                        dc.DrawPath(pen, path);
                    }
                }
            }
        }


        [Description("是否显示边框")]
        public virtual bool ShowBorder
        {
            get { return showBorder; }
            set { showBorder = value; }
        }

        [Description("鼠标平时的图片")]
        public virtual Color BgColor
        {
            get { return this.bgColor; }
            set
            {
                this.bgColor = value;
                base.Invalidate();
            }
        }

        [Description("圆角按钮边缘大小")]
        public virtual int MRadius
        {
            get
            {
                if (this.mRadius == 0)
                {
                    this.mRadius = 5;
                }
                return this.mRadius;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                this.mRadius = value;
                base.Invalidate();
            }
        }
        [Description("子项的高度")]
        public int ItemHeight
        {
            get { return this.itemHeight; }
            set
            {
                this.itemHeight = value;
                base.Invalidate();
            }
        }
        [Description("字体颜色")]
        public Color FontColor
        {
            get { return this.fontColor; }
            set
            {
                this.fontColor = value;
                base.Invalidate();
            }
        }

        public Color FontHoverColor
        {
            get { return this.fontHoverColor; }
            set
            {
                this.fontHoverColor = value;
                base.Invalidate();
            }
        }
    }


    public class CMenuItem
    {
        [CompilerGenerated]
        private bool move;
        [CompilerGenerated]
        private int id;
        [CompilerGenerated]
        private Rectangle bounds;
        [CompilerGenerated]
        private string text;

        public CMenuItem()
        {
        }

        public Rectangle Bounds
        {
            [CompilerGenerated]
            get { return this.bounds; }
            [CompilerGenerated]
            set
            {
                this.bounds = value;
            }
        }

        public int ID
        {
            [CompilerGenerated]
            get { return this.id; }
            [CompilerGenerated]
            set
            {
                this.id = value;
            }
        }

        public bool Move
        {
            [CompilerGenerated]
            get { return this.move; }
            [CompilerGenerated]
            set
            {
                this.move = value;
            }
        }

        public string Text
        {
            [CompilerGenerated]
            get { return this.text; }
            [CompilerGenerated]
            set
            {
                this.text = value;
            }
        }
    }
}
