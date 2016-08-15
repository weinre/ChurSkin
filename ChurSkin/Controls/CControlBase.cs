using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using Win32;


namespace System.Windows.Forms
{
    public partial class CControlBase : Control
    {
        private Image _image;
        private bool _checked;
        private Padding roundPadding = new Padding(Share.DefaultRadius);
        private Color borderColor = Share.BorderColor;
        private Color backColor = Share.BorderColor;
        private string toolTipText;
        private ToolTip ToolTip;
        private Point imageoffset;
        private bool showBorder;
        private ControlBaseCollection items;
        private Image bgImage;
        private Bitmap btnImage;
        private Color btnColor;
        private bool canMoveSelf;
        private bool canMoveParent;

        public event EventHandler<ControlEventArgs> ControlAdded;

        public event EventHandler<ControlEventArgs> ControlRemoved;
        public event EventHandler CheckedChanged;
        public CControlBase()
        {
            SetStyles();
            InitializeComponent();
            base.BackColor = Color.Transparent;
            Font = Share.DefaultFont;
            ToolTip = new ToolTip();
        }
        [Browsable(false)]
        public GraphicsPath rectPath => DrawHelper.CreateRoundPath(ClientRectangle, roundPadding);

        [Description("背景图片")]
        public Image BgImage
        {
            get { return bgImage; }
            set { bgImage = value; }
        }
        [Description("九宫格图片")]
        public Bitmap BtnImage { get { return btnImage; } set { btnImage = value; Invalidate(); } }

        [Description("背景颜色")]
        public Color BtnColor { get { return btnColor; } set { btnColor = value; Invalidate(); } }
        [Description("图标")]
        public Image Image
        {
            get { return _image; }
            set { _image = value; Invalidate(); }
        }
        [Description("选中状态")]
        public bool Checked
        {
            get { return _checked; }
            set
            {
                _checked = value;
                if (CheckedChanged != null)
                    CheckedChanged(this, EventArgs.Empty);
                Invalidate();
            }
        }

        [Description("边框颜色")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value; Invalidate();
            }
        }
        [Description("边框颜色")]
        public Color BackColor
        {
            get { return backColor; }
            set
            {
                backColor = value;
                Invalidate();
            }
        }
        [Description("规定四个角的圆角")]
        public Padding RoundPadding
        {
            get { return roundPadding; }
            set
            {
                roundPadding.Left = roundPadding.Left <= 0 ? 1 : roundPadding.Left;
                roundPadding.Top = roundPadding.Top <= 0 ? 1 : roundPadding.Top;
                roundPadding.Right = roundPadding.Right <= 0 ? 1 : roundPadding.Right;
                roundPadding.Bottom = roundPadding.Bottom <= 0 ? 1 : roundPadding.Bottom;
                roundPadding = value;
                Refresh();
            }
        }

        [Description("图标Image坐标偏移")]
        public Point ImageOffSet
        {
            get { return imageoffset; }
            set
            {
                imageoffset = value;
                Invalidate();
            }
        }
        [Description("是否显示边框")]
        public bool ShowBorder
        {
            get { return showBorder; }
            set { showBorder = value; Invalidate(); }
        }
        //[Description("是否显示边框")]
        //public ControlBaseCollection Items
        //{
        //    get { return Items; }
        //    set { items = value; Invalidate(); }
        //}
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Editor(typeof(CControlCollectionEditor), typeof(UITypeEditor))]
        //public virtual ControlBaseCollection Items
        //{
        //    get
        //    {
        //        if (this.items == null)
        //        {
        //            this.items = new ControlBaseCollection(this);
        //        }
        //        return this.items;
        //    }
        //    set
        //    {
        //        this.items = value;
        //    }
        //}
        [Description("提示文字")]
        public string ToopTipText { get { return toolTipText; } set { toolTipText = value; ToolTip.SetToolTip(this, value); } }
        [Description("是否允许拖动控件"), DefaultValue(false)]
        public bool CanMoveSeft
        {
            get { return canMoveSelf; }
            set { canMoveSelf = value; }
        }
        [Description("是否允许拖动窗体"), DefaultValue(false)]
        public bool CanMoveParent
        {
            get { return canMoveParent; }
            set { canMoveParent = value; }
        }
        [Browsable(false)]
        public DialogResult DialogResult { get; set; }




        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (!base.DesignMode)
            {
                if (canMoveParent || canMoveSelf)
                {
                    IntPtr handle = canMoveParent ? Tools.GetParent(this).Handle : (canMoveSelf ? this.Handle : IntPtr.Zero);
                    //释放鼠标焦点捕获
                    NativeMethods.ReleaseCapture();
                    //向当前窗体发送拖动消息
                    NativeMethods.SendMessage(handle, 0x0112, 0xF011, 0);
                }
            }
        }
        internal virtual void OnControlAdded(ControlEventArgs e)
        {
            e.Control.Parent = this;
            if (this.ControlAdded != null)
            {
                this.ControlAdded(this, e);
            }
           // this.Invalidate(e.Control.ClientRectangle);
        }

        internal virtual void OnControlRemoved(ControlEventArgs e)
        {
            if (this.ControlRemoved != null)
            {
                this.ControlRemoved(this, e);
            }
          //  this.Invalidate(e.Control.ClientRectangle);
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
                //  ControlStyles.EnableNotifyMessage |
                ControlStyles.UserPaint, true);
            base.SetStyle(ControlStyles.Opaque, false);
            base.UpdateStyles();
        }
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion
    }
 
    
}

