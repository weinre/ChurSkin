using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    /// <summary>
    /// </summary>
    [ToolboxBitmap(typeof (TabControl))]
    public class CTabControl : TabControl
    {
        private readonly Container components = null;
        private Rectangle CloseRect;
        private MouseState closeState = MouseState.leave;
        // protected override tabadd
        //Alpha alpha = Alpha.Normal;
        //Color color { get { return Color.FromArgb((int)alpha, bgColor.R, bgColor.G, bgColor.B); } }
        //Color bgColor = Color.White;
        //public Color BgColor
        //{
        //    get { return bgColor; }
        //    set
        //    {
        //        bgColor = value;
        //        if (this.TabPages.Count > 0)
        //        {
        //            alpha = Alpha.MoveOrUp;
        //            for (int i = 0; i < this.TabPages.Count; i++)
        //            {
        //                this.TabPages[i].BackColor = color;
        //            }
        //        }
        //        this.Refresh();
        //    }
        //}
        public CTabControl()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.DoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
            SizeMode = TabSizeMode.Fixed;
            ItemSize = new Size(120, 25);
            Font = Share.DefaultFont;
            AllowDrop = true;
            UpdateStyles();
        }

        //解决自绘之后切换后会闪烁
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        #region 属性

        [DefaultValue(typeof (Color), "234, 247, 254")]
        [Category("自定义属性")]
        public override Color BackColor
        {
            get { return Color.Transparent; } //让控件透明,系统影藏属性
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private GraphicsPath GetPath(Rectangle rect, int raduio)
        {
            return DrawHelper.CreateRoundPath2(rect, raduio);
        }

        private Rectangle GetCloseRect(Rectangle rect)
        {
            rect.Height += 8;
            var r = 16; //叉叉的宽高
            var p = 3; //右边的距离
            CloseRect = new Rectangle(rect.Right - r - p, (rect.Height - r)/2, r, r);
            return CloseRect;
            //GraphicsPath gp = new GraphicsPath();
            //gp.AddLine(rec.X, rec.Top, rec.Right, rec.Bottom);
            //gp.CloseFigure();
            //gp.AddLine(rec.Left, rec.Bottom, rec.Right, rec.Top);
            ////gp.
            //return gp;
        }

        /// <summary>
        ///     关闭Tab触发事件
        /// </summary>
        [Description("关闭Tab触发事件")]
        public event EventHandler TabPageClose;

        /// <summary>
        ///     关闭Tab之前触发事件
        /// </summary>
        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (!Visible) return;
            //画选点击选项卡, 
            if (TabCount > 0)
            {
                var g = e.Graphics;
                Share.GraphicSetup(g);
                //  g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                // g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                // g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                // alpha = Alpha.Normal;
                var sb = new SolidBrush(Share.BackColor);

                var pen = new Pen(Share.BorderColor);
                //   g.FillPath(sb, GetPath(ClientRectangle, 5)); //画整体边框
                //   g.DrawPath(pen, GetPath(ClientRectangle, 5)); //画整体背景
                var tabRect = Rectangle.Empty;
                var selecttabRect = Rectangle.Empty;
                // Point cursorPoint = this.PointToClient(MousePosition);
                TabPage page = null;
                TabPage selectPage = null;
                GraphicsPath gp = null;
                GraphicsPath selectgp = null;

                for (var i = 0; i < TabCount; i++)
                {
                    page = TabPages[i];
                    tabRect = GetTabRect(i);
                    tabRect.X += 2;
                    tabRect.Y += 2;
                    gp = DrawHelper.GetGoogleTabPath(tabRect, 8);
                    if (SelectedIndex != i)
                    {
                        //  alpha = Alpha.MoveOrUp;
                        page.ForeColor = Color.CornflowerBlue;
                        //   sb = new SolidBrush(color);
                        //   pen = new Pen(color);
                        sb.Color = Share.DisabelBackColor;

                        g.FillPath(sb, gp); //画边框
                        g.DrawPath(pen, gp); //画背景
                        gp.Dispose();
                        //  sb.Dispose();
                        //渲染选项卡文字
                        tabRect.X += 10; //让文字往右偏移10个像素.因为谷歌的tab 是个梯形的
                        tabRect.Width -= 20;
                        TextRenderer.DrawText(g, page.Text, page.Font, tabRect,
                            page.ForeColor,
                            TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
                            TextFormatFlags.HorizontalCenter);
                    }
                    else
                    {
                        selectPage = page;
                        selectgp = gp;
                        selecttabRect = tabRect;
                    }
                    if (i == TabCount - 1)
                    {
                        selectPage.ForeColor = Color.Orange;
                        // alpha = Alpha.a200;
                        //  sb = new SolidBrush(color);
                        //  pen = new Pen(color);
                        sb.Color = Share.BackColor;
                        g.FillPath(sb, selectgp); //画背景 
                        g.DrawPath(pen, selectgp); //画边框
                        selectgp.Dispose();
                        // sb.Dispose();
                        //画叉叉 ,叉叉是在最上层的, 所以要最后画
                        Bitmap clsBitmap;
                        if (closeState == MouseState.press)
                            clsBitmap = Properties.Resources.icon_tbclose_press;
                        else if (closeState == MouseState.move)
                            clsBitmap = Properties.Resources.icon_tbclose_hover;
                        else
                            clsBitmap = Properties.Resources.icon_tbclose_normal;
                        g.DrawImage(clsBitmap, GetCloseRect(selecttabRect));
                        clsBitmap.Dispose();
                        //渲染选项卡文字
                        selecttabRect.X += 10; //让文字往右偏移10个像素.因为谷歌的tab 是个梯形的
                        selecttabRect.Width -= 20;
                        TextRenderer.DrawText(g, selectPage.Text, selectPage.Font, selecttabRect,
                            selectPage.ForeColor,
                            TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis |
                            TextFormatFlags.HorizontalCenter);

                        var rect = new Rectangle(GetTabRect(0).X + 2,
                            GetTabRect(0).Bottom + 2,
                            TabPages[0].DisplayRectangle.Width + 6,
                            TabPages[0].DisplayRectangle.Height + 6);
                        var path = GetPath(rect, 5);
                        pen.Color = Share.BorderColor;
                        g.DrawPath(pen, path); //画页面边框
                        path.Dispose();
                    }
                }
                sb.Dispose();
                pen.Dispose();
            }
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            // if (TabPages.Count > 0)
            for (var i = 0; i < TabPages.Count; i++)
            {
                TabPages[SelectedIndex].BackColor = Color.Transparent;
            }
            Refresh();
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            //  if (TabPages.Count > 0)
            for (var i = 0; i < TabPages.Count; i++)
            {
                TabPages[i].BackColor = Color.Transparent;
            }
            Refresh();
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            for (var i = 0; i < TabPages.Count; i++)
            {
                TabPages[i].BackColor = Color.Transparent;
            }
            Refresh();
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            closeState = MouseState.leave;
            Refresh();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (CloseRect.Contains(e.Location) && e.Button == MouseButtons.Left)
            {
                closeState = MouseState.press;
                Refresh();
            }
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            base.OnDragOver(drgevent);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (CloseRect.Contains(e.Location))
                closeState = MouseState.move;
            else
                closeState = MouseState.leave;
            Refresh();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            //鼠标中间关闭选中
            if (e.Button == MouseButtons.Middle && TabPages.Count > 1)
            {
                for (var i = 0; i < TabPages.Count; i++)
                {
                    if (GetTabRect(i).Contains(e.Location))
                    {
                        if (TabPageClose != null)
                            TabPageClose(this, EventArgs.Empty);
                        TabPages.RemoveAt(i);
                        return;
                    }
                }
            }
            //单击叉叉关闭
            if (CloseRect.Contains(e.Location) && closeState == MouseState.press && e.Button == MouseButtons.Left)
            {
                if (TabPageClose != null)
                    TabPageClose(this, EventArgs.Empty);
                if (TabPages.Count > 1)
                    TabPages.RemoveAt(SelectedIndex);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            Refresh();
        }
    }
}