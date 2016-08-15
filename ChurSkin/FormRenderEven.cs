using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Security.Permissions;
using Win32.Consts;

namespace System.Windows.Forms
{
    public class SkinFormBorderRenderEventArgs : PaintEventArgs
    {
        public SkinFormBorderRenderEventArgs(CForm skinForm, Graphics g, Rectangle clipRect, bool active)
            : base(g, clipRect)
        {
            SkinForm = skinForm;
            Active = active;
        }

        public bool Active { get; private set; }
        public CForm SkinForm { get; private set; }
    }

    public delegate void SkinFormBorderRenderEventHandler(object sender, SkinFormBorderRenderEventArgs e);

    public class SkinFormCaptionRenderEventArgs : PaintEventArgs
    {
        public SkinFormCaptionRenderEventArgs(CForm skinForm, Graphics g, Rectangle clipRect, bool active)
            : base(g, clipRect)
        {
            SkinForm = skinForm;
            Active = active;
        }

        public bool Active { get; private set; }
        public CForm SkinForm { get; private set; }
    }

    public delegate void SkinFormCaptionRenderEventHandler(object sender, SkinFormCaptionRenderEventArgs e);

    public class SkinFormControlBoxRenderEventArgs : PaintEventArgs
    {
        public SkinFormControlBoxRenderEventArgs(CForm form, Graphics graphics, Rectangle clipRect, bool active,
            ControlBoxStyle controlBoxStyle, ControlBoxState controlBoxState)
            : base(graphics, clipRect)
        {
            Form = form;
            Active = active;
            ControlBoxtate = controlBoxState;
            ControlBoxStyle = controlBoxStyle;
        }

        public bool Active { get; private set; }
        public ControlBoxStyle ControlBoxStyle { get; private set; }
        public ControlBoxState ControlBoxtate { get; private set; }
        public CForm Form { get; private set; }
    }

    public delegate void SkinFormControlBoxRenderEventHandler(object sender, SkinFormControlBoxRenderEventArgs e);

    public class SkinFormProfessionalRenderer : SkinFormRenderer
    {
        private SkinFormColorTable _colorTable;

        public SkinFormProfessionalRenderer()
        {
        }

        public SkinFormProfessionalRenderer(SkinFormColorTable colortable)
        {
            _colorTable = colortable;
        }

        public SkinFormColorTable ColorTable
        {
            get
            {
                if (_colorTable == null)
                {
                    _colorTable = new SkinFormColorTable();
                }
                return _colorTable;
            }
        }

        private GraphicsPath CreateCloseFlagPath(Rectangle rect)
        {
            var tf = new PointF(rect.X + (rect.Width / 2f), rect.Y + (rect.Height / 2f));
            var path = new GraphicsPath();
            path.AddLine(tf.X, tf.Y - 2f, tf.X - 2f, tf.Y - 4f);
            path.AddLine(tf.X - 2f, tf.Y - 4f, tf.X - 6f, tf.Y - 4f);
            path.AddLine(tf.X - 6f, tf.Y - 4f, tf.X - 2f, tf.Y);
            path.AddLine(tf.X - 2f, tf.Y, tf.X - 6f, tf.Y + 4f);
            path.AddLine(tf.X - 6f, tf.Y + 4f, tf.X - 2f, tf.Y + 4f);
            path.AddLine(tf.X - 2f, tf.Y + 4f, tf.X, tf.Y + 2f);
            path.AddLine(tf.X, tf.Y + 2f, tf.X + 2f, tf.Y + 4f);
            path.AddLine(tf.X + 2f, tf.Y + 4f, tf.X + 6f, tf.Y + 4f);
            path.AddLine(tf.X + 6f, tf.Y + 4f, tf.X + 2f, tf.Y);
            path.AddLine(tf.X + 2f, tf.Y, tf.X + 6f, tf.Y - 4f);
            path.AddLine(tf.X + 6f, tf.Y - 4f, tf.X + 2f, tf.Y - 4f);
            path.CloseFigure();
            return path;
        }

        private GraphicsPath CreateMaximizeFlafPath(Rectangle rect, bool maximize)
        {
            var tf = new PointF(rect.X + (rect.Width / 2f), rect.Y + (rect.Height / 2f));
            var path = new GraphicsPath();
            if (maximize)
            {
                path.AddLine(tf.X - 3f, tf.Y - 3f, tf.X - 6f, tf.Y - 3f);
                path.AddLine(tf.X - 6f, tf.Y - 3f, tf.X - 6f, tf.Y + 5f);
                path.AddLine(tf.X - 6f, tf.Y + 5f, tf.X + 3f, tf.Y + 5f);
                path.AddLine(tf.X + 3f, tf.Y + 5f, tf.X + 3f, tf.Y + 1f);
                path.AddLine(tf.X + 3f, tf.Y + 1f, tf.X + 6f, tf.Y + 1f);
                path.AddLine(tf.X + 6f, tf.Y + 1f, tf.X + 6f, tf.Y - 6f);
                path.AddLine(tf.X + 6f, tf.Y - 6f, tf.X - 3f, tf.Y - 6f);
                path.CloseFigure();
                path.AddRectangle(new RectangleF(tf.X - 4f, tf.Y, 5f, 3f));
                path.AddLine(tf.X - 1f, tf.Y - 4f, tf.X + 4f, tf.Y - 4f);
                path.AddLine(tf.X + 4f, tf.Y - 4f, tf.X + 4f, tf.Y - 1f);
                path.AddLine(tf.X + 4f, tf.Y - 1f, tf.X + 3f, tf.Y - 1f);
                path.AddLine(tf.X + 3f, tf.Y - 1f, tf.X + 3f, tf.Y - 3f);
                path.AddLine(tf.X + 3f, tf.Y - 3f, tf.X - 1f, tf.Y - 3f);
                path.CloseFigure();
                return path;
            }
            path.AddRectangle(new RectangleF(tf.X - 6f, tf.Y - 4f, 12f, 8f));
            path.AddRectangle(new RectangleF(tf.X - 3f, tf.Y - 1f, 6f, 3f));
            return path;
        }

        private GraphicsPath CreateMinimizeFlagPath(Rectangle rect)
        {
            var tf = new PointF(rect.X + (rect.Width / 2f), rect.Y + (rect.Height / 2f));
            var path = new GraphicsPath();
            path.AddRectangle(new RectangleF(tf.X - 6f, tf.Y + 1f, 12f, 3f));
            return path;
        }

        public override Region CreateRegion(CForm form)
        {
            var rect = new Rectangle(Point.Empty, form.Size);
            using (var path = DrawHelper.CreateRoundPath(rect, form.Radius, form.RoundStyle, false)) //
            {
                return new Region(path);
            }
        }

        private void DrawBorder(Graphics g, Rectangle rect, RoundStyle roundStyle, int radius, CForm frm)
        {
            g.SmoothingMode = SmoothingMode.HighQuality;
            rect.Width--;
            rect.Height--;
            using (var path = DrawHelper.CreateRoundPath(rect, radius, roundStyle, false)) //
            {
                using (var pen = new Pen(ColorTable.Border))
                {
                    g.DrawPath(pen, path);
                }
            }
            rect.Inflate(-1, -1);
            using (var path2 = DrawHelper.CreateRoundPath(rect, radius, roundStyle, false)) //
            {
                using (var pen2 = new Pen(ColorTable.InnerBorder))
                {
                    g.DrawPath(pen2, path2);
                }
            }
        }

        private void DrawCaptionBackground(Graphics g, Rectangle captionRect, bool active)
        {
            var baseColor = active ? ColorTable.CaptionActive : ColorTable.CaptionDeactive;
            RenderHelper.RenderBackgroundInternal(g, captionRect, baseColor, ColorTable.Border, ColorTable.InnerBorder,
                RoundStyle.None, 0, 0.25f, false, false, LinearGradientMode.Vertical);
        }

        private void DrawCaptionText(Graphics g, Rectangle textRect, string text, Font font, bool Effect,
            Color EffetBack, int EffectWidth, Color FrmColor)
        {
            if (Effect)
            {
                var size = TextRenderer.MeasureText(text, font);
                var image = SkinTools.ImageLightEffect(text, font, FrmColor, EffetBack, EffectWidth,
                    new Rectangle(0, 0, textRect.Width, size.Height), true);
                g.DrawImage(image, textRect.X - (EffectWidth / 2), textRect.Y - (EffectWidth / 2));
            }
        }

        private void DrawIcon(Graphics g, Rectangle iconRect, Icon icon)
        {
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawIcon(icon, iconRect);
        }

        public override void InitSkinForm(CForm form)
        {
            form.BackColor = ColorTable.Back;
        }

        protected override void OnRenderSkinFormBorder(SkinFormBorderRenderEventArgs e)
        {
            var graphics = e.Graphics;
            using (new AntiAliasGraphics(graphics))
            {
                DrawBorder(graphics, e.ClipRectangle, e.SkinForm.RoundStyle, e.SkinForm.Radius, e.SkinForm);
            }
        }

        protected override void OnRenderSkinFormCaption(SkinFormCaptionRenderEventArgs e)
        {
            var graphics = e.Graphics;
            var clipRectangle = e.ClipRectangle;
            var skinForm = e.SkinForm;
            var iconRect = skinForm.IconRect;
            var empty = Rectangle.Empty;
            var controlBox = skinForm.ControlBox;
            var flag2 = skinForm.ControlBox && skinForm.MinimizeBox;
            var flag3 = skinForm.ControlBox && skinForm.MaximizeBox;
            var flag4 = skinForm.ControlBox && skinForm.SysBottomVisibale;
            var num = 0;
            if (controlBox)
            {
                num += skinForm.CloseBoxSize.Width + skinForm.ControlBoxOffset.X;
            }
            if (flag3)
            {
                num += skinForm.MaxSize.Width + skinForm.ControlBoxSpace;
            }
            if (flag2)
            {
                num += skinForm.MiniSize.Width + skinForm.ControlBoxSpace;
            }
            if (flag4)
            {
                num += skinForm.SysBottomSize.Width + skinForm.ControlBoxSpace;
            }
            // empty = new Rectangle(iconRect.Right + 3, skinForm.BorderPadding.Left + 3, ((clipRectangle.Width - iconRect.Right) - num) - 6, clipRectangle.Height - skinForm.BorderPadding.Left);//old
            empty = new Rectangle(iconRect.Right + 3, iconRect.Top, ((clipRectangle.Width - iconRect.Right) - num) - 6,
                clipRectangle.Height - skinForm.BorderPadding.Left);
            using (new AntiAliasGraphics(graphics))
            {
                DrawCaptionBackground(graphics, clipRectangle, e.Active);
                if (skinForm.FormShowIcon && (skinForm.Icon != null))
                {
                    DrawIcon(graphics, iconRect, skinForm.Icon);
                }
                if (!string.IsNullOrEmpty(skinForm.Text))
                {
                    var effectBack = skinForm.EffectBack;
                    var titleColor = skinForm.TitleColor;
                    if (skinForm.TitleSuitColor)
                    {
                        if (SkinTools.ColorSlantsDarkOrBright(skinForm.BackColor))
                        {
                            titleColor = Color.White;
                            effectBack = Color.Black;
                        }
                        else
                        {
                            titleColor = Color.Black;
                            effectBack = Color.White;
                        }
                    }
                    DrawCaptionText(graphics, empty, skinForm.Text, skinForm.CaptionFont, skinForm.EffectCaption,
                        effectBack, skinForm.EffectWidth, titleColor);
                }
            }
        }

        protected override void OnRenderSkinFormControlBox(SkinFormControlBoxRenderEventArgs e)
        {
            var form = e.Form;
            var g = e.Graphics;
            var clipRectangle = e.ClipRectangle;
            var controlBoxtate = e.ControlBoxtate;
            var active = e.Active;
            var minimizeBox = form.ControlBox && form.MinimizeBox;
            var maximizeBox = form.ControlBox && form.MaximizeBox;
            switch (e.ControlBoxStyle)
            {
                case ControlBoxStyle.Minimize:
                    RenderSkinFormMinimizeBoxInternal(g, clipRectangle, controlBoxtate, active, form);
                    return;

                case ControlBoxStyle.Maximize:
                    RenderSkinFormMaximizeBoxInternal(g, clipRectangle, controlBoxtate, active, minimizeBox,
                        form.WindowState == FormWindowState.Maximized, form);
                    return;

                case ControlBoxStyle.Close:
                    RenderSkinFormCloseBoxInternal(g, clipRectangle, controlBoxtate, active, minimizeBox, maximizeBox,
                        form);
                    return;

                case ControlBoxStyle.SysBottom:
                    RenderSkinFormSysBottomInternal(g, clipRectangle, controlBoxtate, active, form);
                    return;
            }
        }

        private void RenderSkinFormCloseBoxInternal(Graphics g, Rectangle rect, ControlBoxState state, bool active,
            bool minimizeBox, bool maximizeBox, CForm form)
        {
            Bitmap image = (Bitmap)form.btnCloseImage;
            var controlBoxActive = ColorTable.ControlBoxActive;
            int i = 0;
            Rectangle srcRect;
            if (state == ControlBoxState.Pressed)
            {
                i = 2; controlBoxActive = ColorTable.ControlCloseBoxPressed;
            }
            else if (state == ControlBoxState.Hover)
            {
                i = 1; controlBoxActive = ColorTable.ControlCloseBoxHover;
            }
            else
            {
                i = 0; controlBoxActive = active ? ColorTable.ControlBoxActive : ColorTable.ControlBoxDeactive;
            }
            if (image != null)
            {
                srcRect = new Rectangle(i * image.Width / 3, 0, image.Width / 3, image.Height);
                g.DrawImage(image, rect, srcRect, GraphicsUnit.Pixel);
            }
            else
            {
                var style = (minimizeBox || maximizeBox) ? RoundStyle.BottomRight : RoundStyle.Bottom;
                using (new AntiAliasGraphics(g))
                {
                    RenderHelper.RenderBackgroundInternal(g, rect, controlBoxActive, controlBoxActive,
                        ColorTable.ControlBoxInnerBorder, style, 6, 0.38f, true, false, LinearGradientMode.Vertical);
                    using (var pen = new Pen(ColorTable.Border))
                    {
                        g.DrawLine(pen, rect.X, rect.Y, rect.Right, rect.Y);
                    }
                    using (var path = CreateCloseFlagPath(rect))
                    {
                        g.FillPath(Brushes.White, path);
                        using (var pen2 = new Pen(controlBoxActive))
                        {
                            g.DrawPath(pen2, path);
                        }
                    }
                }
            }
        }

        private void RenderSkinFormMaximizeBoxInternal(Graphics g, Rectangle rect, ControlBoxState state, bool active,
            bool minimizeBox, bool maximize, CForm form)
        {
            Bitmap image = maximize ? ((Bitmap)form.btnRestoreImage) : ((Bitmap)form.btnMaxImage);
            var controlBoxActive = ColorTable.ControlBoxActive;
            int i = 0;
            Rectangle srcRect;
            if (state == ControlBoxState.Pressed)
            {
                i = 2; controlBoxActive = ColorTable.ControlBoxPressed;
            }
            else if (state == ControlBoxState.Hover)
            {
                i = 1; controlBoxActive = ColorTable.ControlBoxHover;
            }
            else
            {
                i = 0; controlBoxActive = active ? ColorTable.ControlBoxActive : ColorTable.ControlBoxDeactive;
            }
            if (image != null)
            {
                srcRect = new Rectangle(i * image.Width / 3, 0, image.Width / 3, image.Height);
                g.DrawImage(image, rect, srcRect, GraphicsUnit.Pixel);
            }
            else
            {
                var style = minimizeBox ? RoundStyle.None : RoundStyle.BottomLeft;
                using (new AntiAliasGraphics(g))
                {
                    RenderHelper.RenderBackgroundInternal(g, rect, controlBoxActive, controlBoxActive,
                        ColorTable.ControlBoxInnerBorder, style, 6, 0.38f, true, false, LinearGradientMode.Vertical);
                    using (var pen = new Pen(ColorTable.Border))
                    {
                        g.DrawLine(pen, rect.X, rect.Y, rect.Right, rect.Y);
                    }
                    using (var path = CreateMaximizeFlafPath(rect, maximize))
                    {
                        g.FillPath(Brushes.White, path);
                        using (var pen2 = new Pen(controlBoxActive))
                        {
                            g.DrawPath(pen2, path);
                        }
                    }
                }
            }
        }

        private void RenderSkinFormMinimizeBoxInternal(Graphics g, Rectangle rect, ControlBoxState state, bool active,
            CForm form)
        {
            Bitmap image = (Bitmap)form.btnMinImage; ;
            var controlBoxActive = ColorTable.ControlBoxActive;
            int i = 0;
            Rectangle srcRect;
            if (state == ControlBoxState.Pressed)
            {
                i = 2; controlBoxActive = ColorTable.ControlBoxPressed;
            }
            else if (state == ControlBoxState.Hover)
            {
                i = 1; controlBoxActive = ColorTable.ControlBoxHover;
            }
            else
            {
                i = 0; controlBoxActive = active ? ColorTable.ControlBoxActive : ColorTable.ControlBoxDeactive;
            }
            if (image != null)
            {
                srcRect = new Rectangle(i * image.Width / 3, 0, image.Width / 3, image.Height);
                g.DrawImage(image, rect, srcRect, GraphicsUnit.Pixel);
            }
            else
            {
                var bottomLeft = RoundStyle.BottomLeft;
                using (new AntiAliasGraphics(g))
                {
                    RenderHelper.RenderBackgroundInternal(g, rect, controlBoxActive, controlBoxActive,
                        ColorTable.ControlBoxInnerBorder, bottomLeft, 6, 0.38f, true, false, LinearGradientMode.Vertical);
                    using (var pen = new Pen(ColorTable.Border))
                    {
                        g.DrawLine(pen, rect.X, rect.Y, rect.Right, rect.Y);
                    }
                    using (var path = CreateMinimizeFlagPath(rect))
                    {
                        g.FillPath(Brushes.White, path);
                        using (var pen2 = new Pen(controlBoxActive))
                        {
                            g.DrawPath(pen2, path);
                        }
                    }
                }
            }
        }

        private void RenderSkinFormSysBottomInternal(Graphics g, Rectangle rect, ControlBoxState state, bool active,
            CForm form)
        {
            Bitmap image = null;
            var controlBoxActive = ColorTable.ControlBoxActive;
            if (state == ControlBoxState.Pressed)
            {
                image = (Bitmap)form.SysBottomDown;
                controlBoxActive = ColorTable.ControlBoxPressed;
            }
            else if (state == ControlBoxState.Hover)
            {
                image = (Bitmap)form.SysBottomMouse;
                controlBoxActive = ColorTable.ControlBoxHover;
            }
            else
            {
                image = (Bitmap)form.SysBottomNorml;
                controlBoxActive = active ? ColorTable.ControlBoxActive : ColorTable.ControlBoxDeactive;
            }
            if (image != null)
            {
                g.DrawImage(image, rect);
            }
            else
            {
                var bottomLeft = RoundStyle.BottomLeft;
                using (new AntiAliasGraphics(g))
                {
                    RenderHelper.RenderBackgroundInternal(g, rect, controlBoxActive, controlBoxActive,
                        ColorTable.ControlBoxInnerBorder, bottomLeft, 6, 0.38f, true, false, LinearGradientMode.Vertical);
                    using (var pen = new Pen(ColorTable.Border))
                    {
                        g.DrawLine(pen, rect.X, rect.Y, rect.Right, rect.Y);
                    }
                }
            }
        }

        public ImageAttributes Trank(Bitmap btm, int Alphas)
        {
            var image = (Bitmap)btm.Clone();
            Graphics.FromImage(image);
            var num = Alphas / 100f;
            var numArray = new float[5][];
            var numArray2 = new float[5];
            numArray2[0] = 1f;
            numArray[0] = numArray2;
            var numArray3 = new float[5];
            numArray3[1] = 1f;
            numArray[1] = numArray3;
            var numArray4 = new float[5];
            numArray4[2] = 1f;
            numArray[2] = numArray4;
            var numArray5 = new float[5];
            numArray5[3] = num;
            numArray[3] = numArray5;
            var numArray6 = new float[5];
            numArray6[4] = 1f;
            numArray[4] = numArray6;
            var newColorMatrix = numArray;
            var matrix = new ColorMatrix(newColorMatrix);
            var attributes = new ImageAttributes();
            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            return attributes;
        }
    }

    public abstract class SkinFormRenderer
    {
        private static readonly object EventRenderSkinFormBorder;
        private static readonly object EventRenderSkinFormCaption;
        private static readonly object EventRenderSkinFormControlBox;
        private EventHandlerList _events;

        static SkinFormRenderer()
        {
            EventRenderSkinFormCaption = new object();
            EventRenderSkinFormBorder = new object();
            EventRenderSkinFormControlBox = new object();
        }

        protected EventHandlerList Events
        {
            get
            {
                if (_events == null)
                {
                    _events = new EventHandlerList();
                }
                return _events;
            }
        }

        public event SkinFormBorderRenderEventHandler RenderSkinFormBorder
        {
            add { AddHandler(EventRenderSkinFormBorder, value); }
            remove { RemoveHandler(EventRenderSkinFormBorder, value); }
        }

        public event SkinFormCaptionRenderEventHandler RenderSkinFormCaption
        {
            add { AddHandler(EventRenderSkinFormCaption, value); }
            remove { RemoveHandler(EventRenderSkinFormCaption, value); }
        }

        public event SkinFormControlBoxRenderEventHandler RenderSkinFormControlBox
        {
            add { AddHandler(EventRenderSkinFormControlBox, value); }
            remove { RemoveHandler(EventRenderSkinFormControlBox, value); }
        }

        [UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
        protected void AddHandler(object key, Delegate value)
        {
            Events.AddHandler(key, value);
        }

        public abstract Region CreateRegion(CForm form);

        public void DrawSkinFormBorder(SkinFormBorderRenderEventArgs e)
        {
            OnRenderSkinFormBorder(e);
            var handler = Events[EventRenderSkinFormBorder] as SkinFormBorderRenderEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void DrawSkinFormCaption(SkinFormCaptionRenderEventArgs e)
        {
            OnRenderSkinFormCaption(e);
            var handler = Events[EventRenderSkinFormCaption] as SkinFormCaptionRenderEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void DrawSkinFormControlBox(SkinFormControlBoxRenderEventArgs e)
        {
            OnRenderSkinFormControlBox(e);
            var handler = Events[EventRenderSkinFormControlBox] as SkinFormControlBoxRenderEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public abstract void InitSkinForm(CForm form);
        protected abstract void OnRenderSkinFormBorder(SkinFormBorderRenderEventArgs e);
        protected abstract void OnRenderSkinFormCaption(SkinFormCaptionRenderEventArgs e);
        protected abstract void OnRenderSkinFormControlBox(SkinFormControlBoxRenderEventArgs e);

        [UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
        protected void RemoveHandler(object key, Delegate value)
        {
            Events.RemoveHandler(key, value);
        }
    }
}