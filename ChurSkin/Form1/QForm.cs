// using Controls;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Animations;
using Win32;
namespace System.Windows.Forms
{

    public class QForm : QBaseForm, IQ
    {
        private Animations.Animation animation;
        private AnimationTypes animationTypes;
        private Bitmap bitmap;
        private Bitmap canvas;
        private bool drawIcon;
        private bool bool_4;
        private bool bool_5;
        private bool enableAnimation;
        private bool bool_7;
        private Color color;
        private Color captionColor;
        private double opacity;
        private Font font;
        private Graphics graphics;
        private Graphics graphics_1;
        private GraphicsPath graphicsPath;
        private IContainer icontainer;
        private Image image;
        private ImageAttributes imageAttributes;
        private int captionHeight;
        private int haloSize;
        private int radius;
        private int int_3;
        private int int_4;
        private int int_5;
        private QFormBackgroundRender layeredFormBackgroundRender;
        private MoveModes moveModes;
        private Point point;
        private Rectangle rectangle;
        private Rectangle rectangle_1;
        private Size size;
        private TextRenderingHint textRenderingHint;
        private TextShowModes textShowModes;
        private System.Windows.Forms.Timer timer;
        private Color haloColor;

        [Description("控件重绘时候发生")]
        public event EventHandler<PaintEventArgs> QPaintEvent;

        public QForm()
        {
            this.color = Color.White;
            this.moveModes = MoveModes.Whole;
            this.rectangle = new Rectangle(5, 5, 0x10, 0x10);
            this.captionHeight = 20;
            this.font = new Font("宋体", 10f);
            this.captionColor = Color.Black;
            this.point = new Point();
            this.haloColor = Color.White;
            this.haloSize = 5;
            this.textShowModes = TextShowModes.Halo;
            this.drawIcon = true;
            this.opacity = 1.0;
            this.graphicsPath = new GraphicsPath();
            this.animation = new Animation();
            this.enableAnimation = true;
            this.animationTypes = AnimationTypes.FadeinFadeoutEffect;
            this.timer = new System.Windows.Forms.Timer();
            base.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            if (base.DesignMode)
            {
                base.SetStyle(ControlStyles.ResizeRedraw, true);
            }
            base.UpdateStyles();
            this.InitializeComponent();
        }

        private void animation_AnimationStart(object sender, EventArgs e)
        {
            this.animation.Original = this.Canvas;
            this.bool_4 = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.icontainer != null))
            {
                this.icontainer.Dispose();
            }
            if (this.graphicsPath != null)
            {
                this.graphicsPath.Dispose();
            }
            this.timer.Dispose();
            if(animation!=null)
            this.Animation.Dispose();
            this.DisposeCanvas();
            base.Dispose(disposing);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void DisposeCanvas()
        {
            if (this.canvas != null)
            {
                this.canvas.Dispose();
                this.canvas = null;
            }
            if (this.graphics != null)
            {
                this.graphics.Dispose();
                this.graphics = null;
            }
            if (this.bitmap != null)
            {
                this.bitmap.Dispose();
                this.bitmap = null;
            }
            if (this.graphics_1 != null)
            {
                this.graphics_1.Dispose();
                this.graphics_1 = null;
            }
            if (this.imageAttributes != null)
            {
                this.imageAttributes.Dispose();
                this.imageAttributes = null;
            }
            if (this.image != null)
            {
                this.image.Dispose();
                this.image = null;
            }
        }

        protected virtual void DrawCaption(Graphics g, Rectangle invalidateRect)
        {
            Rectangle rect = new Rectangle(0, 0, base.Width, this.CaptionHeight);
            if (!invalidateRect.IsEmpty && ((invalidateRect.Contains(rect) || invalidateRect.IntersectsWith(rect)) || ((invalidateRect == rect) || rect.Contains(invalidateRect))))
            {
                if (this.drawIcon)
                {
                    g.DrawIcon(base.Icon, this.IconRectangle);
                }
                if (this.textShowModes != TextShowModes.None)
                {
                    if (this.image == null)
                    {
                        this.rZmPiSaoEe();
                    }
                    g.DrawImage(this.image, 0, 0);
                }
            }
        }

        private void InitializeComponent()
        {
            base.SuspendLayout();
            base.AutoScaleMode = AutoScaleMode.None;
            base.ClientSize = new Size(300, 300);
            base.ControlBox = false;
            this.DoubleBuffered = true;
            base.FormBorderStyle = FormBorderStyle.None;
            base.Name = "QForm";
            this.Text = "QForm";
            base.ResumeLayout(false);
        }

        public new void Invalidate()
        {
            this.Invalidate(new Rectangle(0, 0, base.Width, base.Height));
        }

        public new void Invalidate(Rectangle rect)
        {
            Rectangle rectangle = new Rectangle(new Point(), base.Size);
            if (((rectangle.Contains(rect) || rect.IntersectsWith(rect)) || ((rectangle == rect) || rect.Contains(rectangle))) && !rect.IsEmpty)
            {
                if (!rect.Contains(this.rectangle_1) && (rect != this.rectangle_1))
                {
                    if (!this.rectangle_1.Contains(rect))
                    {
                        if (this.rectangle_1.IsEmpty)
                        {
                            this.rectangle_1 = rect;
                        }
                        else
                        {
                            int x = (this.rectangle_1.X < rect.X) ? this.rectangle_1.X : rect.X;
                            int y = (this.rectangle_1.Y < rect.Y) ? this.rectangle_1.Y : rect.Y;
                            int width = (((this.rectangle_1.Width + this.rectangle_1.X) - x) > ((rect.Width + rect.X) - x)) ? ((this.rectangle_1.Width + this.rectangle_1.X) - x) : ((rect.Width + rect.X) - x);
                            int height = (((this.rectangle_1.Height + this.rectangle_1.Y) - y) > ((rect.Height + rect.Y) - y)) ? ((this.rectangle_1.Height + this.rectangle_1.Y) - y) : ((rect.Height + rect.Y) - y);
                            Rectangle rectangle2 = new Rectangle(x, y, width, height);
                            this.rectangle_1 = rectangle2;
                        }
                    }
                }
                else
                {
                    this.rectangle_1 = rect;
                }
                this.rectangle_1.Intersect(rectangle);
            }
            if (base.DesignMode || !base.IsQWindowForm)
            {
                base.Invalidate(rect);
            }
        }

        public void QPaint()
        {
            this.QPaint(new Rectangle(0, 0, base.Width, base.Height));
        }

        public virtual void QPaint(Rectangle invalidateRect)
        {
            if ((base.Visible && (base.WindowState != FormWindowState.Minimized)) && !invalidateRect.IsEmpty)
            {
                if (this.canvas != null)
                {
                    Graphics graphics;
                    if (this.ImageAttribute != null)
                    {
                        graphics = this.graphics_1;
                    }
                    else
                    {
                        graphics = this.graphics;
                    }
                    graphics.SmoothingMode = SmoothingMode.HighSpeed;
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.SetClip(invalidateRect);
                    if (this.BackgroundRender == null)
                    {
                        graphics.Clear(this.color);
                        if (this.BackgroundImage != null)
                        {
                          //  ControlRender.DrawBackgroundImage(graphics, this.BackgroundImageLayout, this.BackgroundImage, new Rectangle(0, 0, base.Width, base.Height));
                        }
                    }
                    else
                    {
                        graphics.Clear(Color.Transparent);
                        this.BackgroundRender.RenderBackground(graphics, this, invalidateRect);
                    }
                    if (!string.IsNullOrEmpty(this.Text))
                    {
                        this.DrawCaption(graphics, invalidateRect);
                    }
                    this.PrePaint(graphics, invalidateRect);
                    if (base.IsQWindowForm && !base.DesignMode)
                    {
                        this.method_10(graphics, invalidateRect);
                    }
                    if (this.ImageAttribute != null)
                    {
                        this.graphics.SmoothingMode = SmoothingMode.HighSpeed;
                        this.graphics.CompositingQuality = CompositingQuality.HighSpeed;
                        this.graphics.SetClip(invalidateRect);
                        this.graphics.Clear(Color.Transparent);
                        Rectangle destRect = new Rectangle(new Point(0, 0), new Size(base.Width, base.Height));
                        this.graphics.DrawImage(this.bitmap, destRect, 0, 0, base.Width, base.Height, GraphicsUnit.Pixel, this.imageAttributes);
                    }
                    this.OnQPaint(new PaintEventArgs(this.graphics, invalidateRect));
                    this.rectangle_1 = new Rectangle();
                    if ((base.IsQWindowForm && !this.bool_4) && !base.DesignMode)
                    {
                        this.UpdateWindow(this.canvas, this.opacity);
                    }
                }
                else
                {
                    this.method_9();
                    this.QPaint();
                }
            }
        }

        private GraphicsPath getRoundPath(Rectangle rectangle_2, int int_6)
        {
            int width = int_6;
            Rectangle rect = new Rectangle(rectangle_2.Location, new Size(width, width));
            this.graphicsPath.Reset();
            this.graphicsPath.AddArc(rect, 180f, 90f);
            rect.X = rectangle_2.Right - width;
            this.graphicsPath.AddArc(rect, 270f, 90f);
            rect.Y = rectangle_2.Bottom - width;
            this.graphicsPath.AddArc(rect, 360f, 90f);
            rect.X = rectangle_2.Left;
            this.graphicsPath.AddArc(rect, 90f, 90f);
            this.graphicsPath.CloseFigure();
            return this.graphicsPath;
        }

        private void method_10(Graphics graphics_2, Rectangle rectangle_2)
        {
            Rectangle rect = new Rectangle(0, 0, base.Width, base.Height);
            for (int i = base.Controls.Count - 1; i >= 0; i--)
            {
                Control control = base.Controls[i];
                if (control.Visible && ((rectangle_2.IntersectsWith(control.Bounds) || rectangle_2.Contains(control.Bounds)) || ((rectangle_2 == control.Bounds) || control.Bounds.Contains(rectangle_2))))
                {
                    if (control is IQ)
                    {
                        IQ layered = control as IQ;
                        graphics_2.DrawImage(layered.Canvas, control.Bounds, 0, 0, control.Width, control.Height, GraphicsUnit.Pixel, layered.ImageAttribute);
                        continue;
                    }
                    using (Bitmap bitmap = new Bitmap(control.Width, control.Height))
                    {
                        control.DrawToBitmap(bitmap, new Rectangle(0, 0, control.Width, control.Height));
                        graphics_2.DrawImage(bitmap, control.Left, control.Top);
                        continue;
                    }
                }
                if ((control is IQ) && (!control.Visible || ((rect.IntersectsWith(control.Bounds) && rect.Contains(control.Bounds)) && ((rect != control.Bounds) && control.Bounds.Contains(rect)))))
                {
                    (control as IQ).DisposeCanvas();
                }
            }
        }

        private void method_11()
        {
            if (this.image != null)
            {
                this.image.Dispose();
                this.image = null;
            }
            this.Invalidate(new Rectangle(0, 0, base.Width, this.captionHeight));
        }

        private void method_12(ref Message message)
        {
            if (message.WParam.ToInt32() == 1)
            {
                this.bool_7 = true;
            }
            else
            {
                this.bool_7 = false;
            }
            message.Result = (IntPtr)1;
            if (base.DesignMode || !base.IsQWindowForm)
            {
                base.Invalidate();
            }
        }

        private void setRegion()
        {
            if (this.radius > 0)
            {
                // Rectangle rectangle = new Rectangle(0, 0, base.Width, base.Height);
                //  this.graphicsPath = this.getRoundPath(rectangle, this.radius);
                //  base.Region = new Region(this.graphicsPath);

                SkinTools.CreateRegion(this, rectangle, this.radius, RoundStyle.All);

            }
            else
            {
                base.Region = null;
            }
        }

        private void method_3(object sender, AnimationEventArgs e)
        {
            this.int_3 = (e.CurrentFrame.Width - base.Width) / 2;
            this.int_4 = (e.CurrentFrame.Height - base.Height) / 2;
            this.size = base.Size;
            base.Size = e.CurrentFrame.Size;
            base.Location = new Point(base.Left - this.int_3, base.Top - this.int_4);
        }

        private void method_4(object sender, EventArgs e)
        {
            if (!this.bool_5)
            {
                base.Size = this.size;
                this.bool_4 = false;
                this.QPaint();
                base.Location = new Point(base.Left + this.int_3, base.Top + this.int_4);
                GC.Collect();
            }
            else
            {
                base.Hide();
                base.Close();
            }
        }

        private void method_5(object sender, AnimationEventArgs e)
        {
            if (!e.IsFinal && base.IsQWindowForm)
            {
                this.UpdateWindow(e.CurrentFrame, this.opacity);
            }
        }

        private void method_6(object sender, EventArgs e)
        {
            if (base.IsQWindowForm && !base.DesignMode)
            {
                Control control = sender as Control;
                this.Invalidate(control.Bounds);
            }
        }

        private void method_7(object sender, InvalidateEventArgs e)
        {
            if (base.IsQWindowForm && !base.DesignMode)
            {
                Control control = sender as Control;
                Rectangle invalidRect = e.InvalidRect;
                invalidRect.Offset(control.Location);
                this.Invalidate(invalidRect);
            }
        }

        private void method_8(object sender, EventArgs e)
        {
            if (base.IsQWindowForm && !base.DesignMode)
            {
                this.Invalidate();
            }
        }

        private void method_9()
        {
            this.canvas = new Bitmap(base.Width, base.Height);
            if (this.imageAttributes != null)
            {
                this.bitmap = new Bitmap(base.Width, base.Height);
                this.graphics_1 = Graphics.FromImage(this.bitmap);
            }
            this.graphics = Graphics.FromImage(this.canvas);
        }

        protected override void OnBackgroundImageChanged(EventArgs e)
        {
            base.OnBackgroundImageChanged(e);
            this.Invalidate();
        }

        protected override void OnBackgroundImageLayoutChanged(EventArgs e)
        {
            this.Invalidate();
            base.OnBackgroundImageLayoutChanged(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if ((!this.bool_5 && this.enableAnimation) && (base.IsQWindowForm && (this.animation.Effect != null)))
            {
                this.bool_4 = true;
                this.animation.Original = this.canvas;
                this.animation.Asc = !this.animation.Asc;
                this.animation.Start();
                e.Cancel = true;
                this.bool_5 = true;
            }
            base.OnClosing(e);
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            if (base.IsQWindowForm && !base.DesignMode)
            {
                //if (e.Control is QPanel)
                //{
                //    ((QPanel)e.Control).QInvalidated += new EventHandler<InvalidateEventArgs>(this.method_7);
                //}
                //else if (e.Control is QTabControl)
                //{
                //    ((QTabControl)e.Control).QInvalidated += new EventHandler<InvalidateEventArgs>(this.method_7);
                //}
                //else
                //{
                e.Control.Invalidated += new InvalidateEventHandler(this.method_7);
                // }
                if (!(e.Control is IQ))
                {
                    e.Control.Move += new EventHandler(this.method_8);
                    e.Control.VisibleChanged += new EventHandler(this.method_6);
                }
            }
        }

        protected virtual void OnQPaint(PaintEventArgs e)
        {
            if (this.QPaintEvent != null)
            {
                this.QPaintEvent(this, e);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!base.DesignMode && base.IsQWindowForm && animation != null)
            {
                if (!this.enableAnimation)
                {
                    this.bool_4 = false;
                }
                else
                {
                    this.bool_4 = true;
                }
                switch (this.animationTypes)
                {
                    case AnimationTypes.ZoomEffect:
                        this.animation.Effect = new ZoomEffect();
                        break;

                    case AnimationTypes.GradualCurtainEffect:
                        this.animation.Effect = new GradualCurtainEffect();
                        break;

                    case AnimationTypes.FadeinFadeoutEffect:
                        this.animation.Effect = new FadeinFadeoutEffect();
                        break;

                    case AnimationTypes.RotateZoomEffect:
                        this.animation.Effect = new RotateZoomEffect();
                        break;

                    case AnimationTypes.ThreeDTurn:
                        this.animation.Effect = new ThreeDTurn();
                        break;
                }
                this.animation.FrameChanged += new EventHandler<AnimationEventArgs>(this.method_5);
                this.animation.AnimationEnded += new EventHandler(this.method_4);
                this.animation.AnimationStart += new EventHandler(this.animation_AnimationStart);
                this.animation.AnimationStarted += new EventHandler<AnimationEventArgs>(this.method_3);
            }
            this.QPaint();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!base.DesignMode)
            {
                if (this.moveModes == MoveModes.Whole)
                {
                    NativeMethods.MouseToMoveControl(base.Handle);
                }
                else if ((this.moveModes == MoveModes.Title) && (e.Y <= this.captionHeight))
                {
                    NativeMethods.MouseToMoveControl(base.Handle);
                }
            }
            base.OnMouseDown(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (base.DesignMode || !base.IsQWindowForm)
            {
                Graphics graphics = e.Graphics;
                graphics.SmoothingMode = SmoothingMode.HighSpeed;
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.SetClip(e.ClipRectangle);
                graphics.DrawImage(this.Canvas, 0, 0);
            }
            if (base.DesignMode && (this.color == Color.Transparent))
            {
                using (Pen pen = new Pen(this.ForeColor))
                {
                    pen.DashStyle = DashStyle.Dot;
                    e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, base.Width - 1, base.Height - 1));
                }
            }
            base.OnPaint(e);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (base.DesignMode && base.IsQWindowForm)
            {
                e.Graphics.SetClip(e.ClipRectangle);
                if (base.Parent != null)
                {
                    e.Graphics.Clear(base.Parent.BackColor);
                }
            }
            else
            {
                base.OnPaintBackground(e);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            if (!base.DesignMode && base.IsQWindowForm && animation != null)
            {
                if ((this.animation.Effect != null) && this.enableAnimation)
                {
                    this.animation.Start();
                }
                else
                {
                    this.bool_4 = false;
                    this.QPaint();
                }
                this.timer.Interval = 15;
                this.timer.Tick += new EventHandler(this.timer_Tick);
                this.timer.Start();
            }
            base.OnShown(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (!this.bool_4)
            {
                if (base.Visible && (base.WindowState != FormWindowState.Minimized))
                {
                    this.DisposeCanvas();
                    this.QPaint();
                }
                this.setRegion();
            }
            this.size = base.Size;
            GC.Collect();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (base.Visible)
            {
                this.Invalidate();
            }
            base.OnVisibleChanged(e);
        }

        protected virtual void PrePaint(Graphics g, Rectangle invalidateRect)
        {
        }

        private void rZmPiSaoEe()
        {
            if (this.image != null)
            {
                this.image.Dispose();
                this.image = null;
            }
            int blurConsideration = (this.textShowModes == TextShowModes.Halo) ? this.haloSize : 0;
            int num2 = this.drawIcon ? 20 : 5;
            this.image = ImageEffects.ImageLightEffect(this.Text, this.font, this.captionColor, this.haloColor, blurConsideration, new Rectangle(this.point.X + num2, this.point.Y + 5, base.Width - this.point.X, this.captionHeight), StringFormat.GenericDefault, this.textRenderingHint);
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (this.int_5 != 0)
            {
                try
                {
                    System.Type type = typeof(Form);
                    FieldInfo field = type.GetField("FormStateExWindowBoundsWidthIsClientSize", BindingFlags.NonPublic | BindingFlags.Static);
                    FieldInfo info2 = type.GetField("formStateEx", BindingFlags.NonPublic | BindingFlags.Instance);
                    FieldInfo info3 = type.GetField("restoredWindowBounds", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (((field != null) && (info2 != null)) && (info3 != null))
                    {
                        Rectangle rectangle = (Rectangle)info3.GetValue(this);
                        BitVector32.Section section = (BitVector32.Section)field.GetValue(this);
                        BitVector32 vector = (BitVector32)info2.GetValue(this);
                        if (vector[section] == 1)
                        {
                            width = rectangle.Width;
                            height = rectangle.Height;
                        }
                    }
                }
                catch
                {
                }
            }
            base.SetBoundsCore(x, y, width, height, specified);
        }

        protected override void SetClientSizeCore(int x, int y)
        {
            System.Type type = typeof(Control);
            System.Type type2 = typeof(Form);
            FieldInfo field = type.GetField("clientWidth", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo info2 = type.GetField("clientHeight", BindingFlags.NonPublic | BindingFlags.Instance);
            FieldInfo info3 = type2.GetField("FormStateSetClientSize", BindingFlags.NonPublic | BindingFlags.Static);
            FieldInfo info4 = type2.GetField("formState", BindingFlags.NonPublic | BindingFlags.Instance);
            if (((field != null) && (info2 != null)) && ((info4 != null) && (info3 != null)))
            {
                base.Size = new Size(x, y);
                field.SetValue(this, x);
                info2.SetValue(this, y);
                BitVector32.Section section = (BitVector32.Section)info3.GetValue(this);
                BitVector32 vector = (BitVector32)info4.GetValue(this);
                vector[section] = 1;
                info4.SetValue(this, vector);
                this.OnClientSizeChanged(EventArgs.Empty);
                vector[section] = 0;
                info4.SetValue(this, vector);
            }
            else
            {
                base.SetClientSizeCore(x, y);
            }
        }

        protected override Size SizeFromClientSize(Size clientSize)
        {
            return clientSize;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if ((!this.bool_4 && base.IsQWindowForm) && !this.rectangle_1.IsEmpty)
            {
                this.QPaint(this.rectangle_1);
            }
        }

        protected virtual void WmNcCalcSize(ref Message m)
        {
            if (base.Opacity != 1.0)
            {
                this.Invalidate();
            }
        }

        protected virtual void WmWindowPosChanged(ref Message m)
        {
            this.int_5++;
            base.WndProc(ref m);
            this.int_5--;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x83:
                    this.WmNcCalcSize(ref m);
                    return;

                case 0x85:
                    return;

                case 0x86:
                    this.method_12(ref m);
                    return;

                case 0x47:
                    this.WmWindowPosChanged(ref m);
                    return;

                case 0xae:
                case 0xaf:
                    m.Result = (IntPtr)1;
                    return;
            }
            base.WndProc(ref m);
        }

        [Browsable(false)]
        public Animations.Animation Animation
        {
            get
            {
                return this.animation;
            }
        }

        [DefaultValue(2), Description("窗体启动和关闭特效，在层窗体模式下才有效，如果自定义的话，在Load事件里写this.Animation.Effect = new Animations.特效名Effect();")]
        public AnimationTypes AnimationType
        {
            get
            {
                return
              this.animationTypes;
            }
            set
            {
                this.animationTypes = value;
            }
        }

        [Description("窗体背景色，层窗体模式下支持透明色，普通窗体模式下不要设置透明色，如果有半透明该属性不能获取真正的背景色，需要通过RealBackColor属性获取")]
        public override Color BackColor
        {
            get
            {
                if (base.DesignMode)
                {
                    return this.color;
                }
                return base.BackColor;
            }
            set
            {
                this.color = value;
                if (value.A < 0xff)
                {
                    if (value == Color.Transparent)
                    {
                        base.BackColor = Color.White;
                    }
                    else
                    {
                        base.BackColor = Color.FromArgb(0xff, value.R, value.G, value.B);
                    }
                }
                else
                {
                    base.BackColor = value;
                }
                this.Invalidate();
            }
        }

        [Browsable(false)]
        public QFormBackgroundRender BackgroundRender
        {
            get
            {
                return this.layeredFormBackgroundRender;
            }
            set
            {
                if (this.layeredFormBackgroundRender != value)
                {
                    this.layeredFormBackgroundRender = value;
                    this.Invalidate();
                }
            }
        }

        [Browsable(false), ReadOnly(true)]
        public Bitmap Canvas
        {
            get
            {
                this.QPaint(this.rectangle_1);
                return this.canvas;
            }
            set
            {
                this.canvas = value;
            }
        }

        [Description("标题颜色"), Category("标题栏")]
        public Color CaptionColor
        {
            get
            {
                return
              this.captionColor;
            }
            set
            {
                if (this.captionColor != value)
                {
                    this.captionColor = value;
                    this.method_11();
                }
            }
        }

        [Description("标题字体"), Category("标题栏")]
        public Font CaptionFont
        {
            get
            {
                return
              this.font;
            }
            set
            {
                if (this.font != value)
                {
                    this.font = value;
                    this.method_11();
                }
            }
        }

        [Description("标题栏高度"), Category("标题栏")]
        public int CaptionHeight
        {
            get
            {
                return
              this.captionHeight;
            }
            set
            {
                if (this.captionHeight != value)
                {
                    this.captionHeight = value;
                    this.method_11();
                }
            }
        }

        [Category("标题栏"), Description("标题位置偏移")]
        public Point CaptionOffset
        {
            get
            {
                return
              this.point;
            }
            set
            {
                if (this.point != value)
                {
                    this.point = value;
                    this.method_11();
                }
            }
        }

        [Description("标题显示模式"), Category("标题栏")]
        public TextShowModes CaptionShowMode
        {
            get
            {
                return
              this.textShowModes;
            }
            set
            {
                if (this.textShowModes != value)
                {
                    this.textShowModes = value;
                    this.method_11();
                }
            }
        }

        [Category("标题栏"), Description("标题文本渲染模式")]
        public TextRenderingHint CaptionTextRender
        {
            get
            {
                return
              this.textRenderingHint;
            }
            set
            {
                if (this.textRenderingHint != value)
                {
                    this.textRenderingHint = value;
                    this.method_11();
                }
            }
        }

        [Description("是否在标题栏绘制图标"), Category("标题栏")]
        public bool DrawIcon
        {
            get
            {
                return
              this.drawIcon;
            }
            set
            {
                if (this.drawIcon != value)
                {
                    this.drawIcon = value;
                    this.method_11();
                }
            }
        }

        [Description("是否启用窗体动画")]
        public bool EnableAnimation
        {
            get
            {
                return
              this.enableAnimation;
            }
            set
            {
                this.enableAnimation = value;
            }
        }

        [Category("标题栏"), Description("标题文字光圈颜色")]
        public Color HaloColor
        {
            get
            {
                return
              this.haloColor;
            }
            set
            {
                if (this.haloColor != value)
                {
                    this.haloColor = value;
                    this.method_11();
                }
            }
        }

        [Description("标题文字光圈大小"), Category("标题栏")]
        public int HaloSize
        {
            get
            {
                return
              this.haloSize;
            }
            set
            {
                if (this.haloSize != value)
                {
                    this.haloSize = value;
                    this.method_11();
                }
            }
        }

        [Description("Icon绘制区域"), Category("标题栏")]
        public Rectangle IconRectangle
        {
            get
            {
                return
              this.rectangle;
            }
            set
            {
                if (this.rectangle != value)
                {
                    this.rectangle = value;
                    this.method_11();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ImageAttributes ImageAttribute
        {
            get
            {
                return
              this.imageAttributes;
            }
            set
            {
                if (this.imageAttributes != value)
                {
                    if (this.imageAttributes != null)
                    {
                        this.imageAttributes.Dispose();
                        this.imageAttributes = null;
                    }
                    this.imageAttributes = value;
                }
            }
        }

        [Description("窗体移动模式，鼠标如何移动窗体")]
        public MoveModes MoveMode
        {
            get
            {
                return
              this.moveModes;
            }
            set
            {
                this.moveModes = value;
            }
        }

        [DefaultValue(1), Description("设置窗体的不透明度,0-1")]
        public new double Opacity
        {
            get
            {
                return
              this.opacity;
            }
            set
            {
                if (!base.IsQWindowForm)
                {
                    base.Opacity = value;
                }
                if (this.opacity != value)
                {
                    this.opacity = value;
                    if (value >= 1.0)
                    {
                        this.opacity = 1.0;
                    }
                    if (this.opacity <= 0.0)
                    {
                        this.opacity = 0.0;
                    }
                    this.UpdateWindow(this.Canvas, this.opacity);
                }
            }
        }

        [Description("窗体圆角半径")]
        public int Radius
        {
            get
            {
                return
              this.radius;
            }
            set
            {
                if (this.radius != value)
                {
                    this.radius = value;
                    this.setRegion();
                }
            }
        }

        [Browsable(false)]
        public Color RealBackColor
        {
            get
            {
                return this.color;
            }
        }

        public override string Text
        {
            get
            {
                return
              base.Text;
            }
            set
            {
                if (base.Text != value)
                {
                    base.Text = value;
                    this.method_11();
                }
            }
        }
    }
}

