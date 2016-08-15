using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design ")]
    public partial class CImageButton : Button
    {
        public Image buttonDownImage;
        public Image buttonHoverImage;
        private Image buttonImage;
        public Image buttonNormalImage;

        public CImageButton()
        {
            InitializeComponent();
            SetStyles();
            BackColor = Color.Transparent;
            //  buttonImage = buttonNormalImage;
        }

        public Image ButtonNormalImage
        {
            get { return buttonNormalImage; }
            set
            {
                buttonImage = buttonNormalImage = value;
                Refresh();
            }
        }

        public Image ButtonHoverImage
        {
            get { return buttonHoverImage; }
            set { buttonHoverImage = value; }
        }

        public Image ButtonDownImage
        {
            get { return buttonDownImage; }
            set { buttonDownImage = value; }
        }

        private void DrawBackGroundImage(Graphics g)
        {
            var rect = Rectangle.Empty;
            switch (BackgroundImageLayout)
            {
                case ImageLayout.None:
                    rect = new Rectangle(0, 0, buttonImage.Width, buttonImage.Height);
                    break;
                case ImageLayout.Center:
                    rect = new Rectangle((Width - buttonImage.Width)/2,
                        (Height - buttonImage.Height)/2,
                        buttonImage.Width,
                        buttonImage.Height);
                    break;
                case ImageLayout.Stretch:
                    rect = ClientRectangle;
                    break;
                case ImageLayout.Tile:
                    if (buttonImage.Width > Width && Height > buttonImage.Height)
                        g.DrawImage(buttonImage, ClientRectangle);
                    else
                    {
                        var fillX = (int) Math.Ceiling(Width/(double) buttonImage.Width);
                        var fillY = (int) Math.Ceiling(Height/(double) buttonImage.Height);
                        for (var x = 0; x <= fillX; x++) //画X轴
                        {
                            for (var y = 0; y <= fillY; y++) //画Y轴
                            {
                                var rectXY = new Rectangle(buttonImage.Width*x, buttonImage.Height*y, buttonImage.Width,
                                    buttonImage.Height);
                                g.DrawImage(buttonImage, rectXY);
                            }
                        }
                    }
                    return;
                case ImageLayout.Zoom:
                    if (Width > buttonImage.Width)
                    {
                        rect.X = (Width - buttonImage.Width)/2;
                        rect.Width = buttonImage.Width;
                    }
                    else
                    {
                        rect.X = 0;
                        rect.Width = Width;
                    }
                    if (Height > buttonImage.Height)
                    {
                        rect.Y = (Height - buttonImage.Height)/2;
                        rect.Height = buttonImage.Height;
                    }
                    else
                    {
                        rect.Y = 0;
                        rect.Height = Height;
                    }
                    break;
            }
            g.DrawImage(buttonImage, rect);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            // if (!DesignMode && !Visible) return;
            var g = pevent.Graphics;
            Share.GraphicSetup(g);
            if (buttonImage != null) DrawBackGroundImage(g);
        }

        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            base.OnMouseMove(mevent);
            buttonImage = buttonHoverImage;
            Refresh();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            buttonImage = buttonNormalImage;
            Refresh();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            //this.Focus();
            base.OnMouseDown(mevent);
            buttonImage = buttonDownImage;
            Refresh();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            buttonImage = buttonHoverImage;
            Refresh();
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
                ControlStyles.EnableNotifyMessage |
                ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.Opaque, false);
            UpdateStyles();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0014) // 禁掉清除背景消息

                return;

            base.WndProc(ref m);
        }
    }
}