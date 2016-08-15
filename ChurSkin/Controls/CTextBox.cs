using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;

namespace System.Windows.Forms
{
    //[ToolboxBitmap(typeof(TextBox)), Designer(typeof(TextBox))]
    public partial class CTextBox : Control
    {
        private readonly string pattern = @"^[0-9]*$";
        private char isPasswordChat;
        private bool isSystemPasswordChar;
        private int maxLength;
        private string param = string.Empty;
        private bool readOnly;
        private string text;
        private string waterText;
        private Color bgColor;
        private Color borderColor;
        public CTextBox()
        {
            SetStyles();
            InitializeComponent();
            bgColor = Share.BackColor;
            borderColor = Share.BorderColor;
            txtEx.TextChanged += TxtChange;
            UpdateUI();
        }
        [Description("背景颜色")]
        public Color BgColor
        {
            get { return bgColor; }
            set
            {
                bgColor = value;
                this.txtEx.BackColor = value; Invalidate();
            }
        }
        [Description("背景颜色")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value; Invalidate();
            }
        }
        [Description("水印文字"), Category("自定义属性"), DefaultValue("")]
        public virtual string WaterText
        {
            get { return waterText; }
            set
            {
                waterText = txtEx.WaterText = value;
                Invalidate();
            }
        }

        internal GraphicsPath rectPath
        {
            get { return DrawHelper.CreateRoundPath2(ClientRectangle, Share.DefaultRadius); }
        }

        [Description("是否允许值输入数字"), DefaultValue(false)]
        public bool IsNum { get; set; }

        public virtual bool ReadOnly
        {
            get { return readOnly; }
            set
            {
                readOnly = txtEx.ReadOnly = value;
                Invalidate();
            }
        }

        [Description("允许输入最大长度"), DefaultValue(32767)]
        public virtual int MaxLength
        {
            get { return maxLength; }
            set { maxLength = txtEx.MaxLength = value; }
        }

        [Description("正则表达"), DefaultValue("\0")]
        public char IsPasswordChat
        {
            get { return isPasswordChat; }
            set { isPasswordChat = txtEx.PasswordChar = value; }
        }

        [Description("是否为密码输入框"), DefaultValue(false)]
        public virtual bool IsSystemPasswordChar
        {
            get { return isSystemPasswordChar; }
            set { isSystemPasswordChar = txtEx.UseSystemPasswordChar = value; }
        }

        [Description("文本框文字"), DefaultValue("")]
        public override string Text
        {
            get { return text; }
            set { text = txtEx.Text = value; }
        }

        protected override Size DefaultSize
        {
            get { return new Size(120, 26); }
        }

        public new event EventHandler TextChanged;

        protected virtual void OnTextChanged()
        {
            if (TextChanged != null)
                TextChanged(this, EventArgs.Empty);
            //ValueChanged(this, e);
        }
        //public override Color BackColor { get { return Color.Transparent; } }
        private void TxtChange(object sender, EventArgs e)
        {
            if (IsNum)
            {
                var m = Regex.Match(txtEx.Text, pattern);
                if (!m.Success)
                {
                    txtEx.Text = param;
                    txtEx.SelectionStart = txtEx.Text.Length;
                }
                else
                {
                    param = txtEx.Text;
                }
            }
            Text = txtEx.Text; OnTextChanged();
        }

        //Alpha alpha = Alpha.Normal;
        //Color color { get { return Color.FromArgb(170, 170, 170); } }
        ////public 
        //private Color bgColor;

        //[Description("控件的背景颜色")]
        //public Color BgColor
        //{
        //    get { return bgColor; }
        //    set
        //    {
        //        bgColor = value;
        //        txtEx.BackColor = bgColor;
        //        this.Invalidate();
        //    }
        //}
        //protected override void OnCreateControl()
        //{
        //    base.OnCreateControl();
        //    this.BgColor = Color.FromArgb(170, 170, 170);
        //    txtEx.BackColor = BgColor;
        //}
        protected override void OnPaint(PaintEventArgs pevent)
        {
            var g = pevent.Graphics;

            Share.GraphicSetup(g);
            var sb = new SolidBrush(bgColor);
            var p = new Pen(borderColor);
            if (!Enabled)
            {
                sb.Color = txtEx.BackColor = Share.DisabelBackColor;
                p.Color = txtEx.ForeColor = Share.DisableBorderColor;
            }
            g.FillPath(sb, rectPath);
            g.DrawPath(p, rectPath);
            sb.Dispose();
            p.Dispose();
        }

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
            // BackColor = Color.Transparent;
            UpdateStyles();
        }

        private void UpdateUI()
        {
            if (Height > 50)
            {
                txtEx.Multiline = true;
                txtEx.Height = Height - 6;
            }
            else
            {
                txtEx.Multiline = false;
            }
            txtEx.Width = Width - 15;
            txtEx.Location = new Point((Width - txtEx.Width) / 2, (Height - txtEx.Height) / 2);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateUI();
            //if (this.Parent != null && this.Parent.GetType() == typeof(CGroupBox))
            //{
            //    if (this.Height > 30) { txtEx.Multiline = true; txtEx.Height = this.Height - 6; }
            //    txtEx.Width = this.Width - 3;
            //    txtEx.Location = new Point((this.Width - txtEx.Width) / 2, (this.Height - txtEx.Height) / 2);
            //}
            //else
            //{
        }
    }
}