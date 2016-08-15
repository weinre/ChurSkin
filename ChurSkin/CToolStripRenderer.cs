using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    public class CToolStripRenderer : ToolStripRenderer
    {
        //渲染背景 包括menustrip背景 toolstripDropDown背景
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            var toolStrip = e.ToolStrip;
            var g = e.Graphics;
            //g.SmoothingMode = SmoothingMode.HighQuality;//抗锯齿
            Share.GraphicSetup(g);
            var bounds = e.AffectedBounds;
            //DrawHelper.RenderFormBorder(MenuBgImg, 20, g, e.);
            if (toolStrip is MenuStrip)
            {
                // g.FillRectangle(Brushes.Orange, bounds);
                //DrawHelper.RenderFormBorder(MenuBgImg, 20, g, bounds);
            }
            else if (toolStrip is ToolStripDropDown)
            {
                //  g.FillRectangle(Brushes.Orange, bounds);

                //背景圆角
                DrawHelper.SetWindowRegion(toolStrip, 1);
                using (var sb = new SolidBrush(Share.BackColor))
                {
                    g.FillRectangle(sb, bounds);
                    //画边线
                    sb.Color = Share.BorderColor;

                    var pen = new Pen(sb);
                    var gp = DrawHelper.CreateRoundPath2(bounds, 2);
                    e.Graphics.DrawPath(pen, gp);
                    pen.Dispose();
                    gp.Dispose();
                }


                //int Rgn = NativeMethods.CreateRoundRectRgn(0, 0, bounds.Width + 1, bounds.Height + 1, 3, 3);
                //NativeMethods.SetWindowRgn(e.ToolStrip.Handle, Rgn, true);
                //toolStrip.BackColor = Color.White;
                //DrawHelper.RenderFormBorder(MenuBgImg, 27, g, bounds);
            }
            else
            {
                //  g.FillRectangle(Brushes.Orange, bounds);

                // g.FillRectangle(Brushes.Orange, bounds);
                //e.Graphics.Clear(Color.Transparent);
                base.OnRenderToolStripBackground(e);
            }
        }

        //子项背景
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            var toolStrip = e.ToolStrip;
            var item = e.Item;

            if (!item.Enabled)
            {
                return;
            }

            var g = e.Graphics;
            Share.GraphicSetup(g);
            //渲染顶级项
            if (toolStrip is MenuStrip)
            {
                var gp = DrawHelper.CreateRoundPath2(item.ContentRectangle, Share.DefaultRadius);
                // int alpha = (int)Alpha.Normal;
                var sb = new SolidBrush(Share.BackColor);
                var pen = new Pen(Share.BorderColor);
                if (item.Selected)
                {
                    // alpha = (int)Alpha.Normal;
                    sb.Color = pen.Color = Share.FocusBackColor;
                    g.FillPath(sb, gp);
                    g.DrawPath(pen, gp);
                }
                else if (item.Pressed)
                {
                    //   alpha = (int)Alpha.PressOrDown;
                    sb.Color = pen.Color = Share.FocusBackColor;
                    g.FillPath(sb, gp);
                    g.DrawPath(pen, gp);
                }
                else
                {
                    sb.Color = Share.FocusBackColor;
                    pen.Color = Share.BorderColor;
                    base.OnRenderMenuItemBackground(e);
                }
                sb.Dispose();
                pen.Dispose();
                gp.Dispose();
            }
            //渲染下拉项  右键菜单
            else if (toolStrip is ToolStripDropDown)
            {
                if (item.Selected)
                {
                    item.ForeColor = Color.White;
                    var rect = new Rectangle(
                        item.ContentRectangle.X,
                        item.ContentRectangle.Y - 2,
                        item.ContentRectangle.Width + 1,
                        item.ContentRectangle.Height + 3);
                    //Color cl = Color.FromArgb(150, 150, 150);
                    var sb = new SolidBrush(Share.FocusBackColor);
                    g.FillRectangle(sb, rect);
                    sb.Dispose();
                }
                else
                {
                    item.ForeColor = Color.FromArgb(80, 80, 80);
                }
            }
            else
            {
                base.OnRenderMenuItemBackground(e);
            }
        }

        //渲染toolScript 下啦菜单项 一级 单项 
        protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
        {
            var toolStrip = e.ToolStrip;
            var item = e.Item as ToolStripDropDownItem;
            // return;
            if (item != null)
            {
                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.HighQuality;
                Rectangle bounds;
                if (toolStrip.LayoutStyle == ToolStripLayoutStyle.Table)
                    bounds = new Rectangle(Point.Empty, new Size(item.Size.Width - 10, item.Size.Height));
                else
                    bounds = new Rectangle(Point.Empty, item.Size);

                var gp = DrawHelper.CreateRoundPath2(bounds, 5);
                //   int alpha = (int)Alpha.Normal;
                var sb = new SolidBrush(Share.BackColor);
                var pen = new Pen(Share.BorderColor);

                if (item.Pressed && item.HasDropDownItems)
                {
                    //  alpha = (int)Alpha.PressOrDown;
                    sb.Color = Share.BorderColor;
                    g.FillPath(sb, gp);
                    g.DrawPath(pen, gp);
                    sb.Dispose();
                    pen.Dispose();
                    gp.Dispose();
                }
                else if (item.Selected)
                {
                    sb.Color = Share.BorderColor;
                    //   alpha = (int)Alpha.MoveOrUp;
                    g.FillPath(sb, gp);
                    g.DrawPath(pen, gp);
                    sb.Dispose();
                    pen.Dispose();
                    gp.Dispose();
                    item.ForeColor = Color.White;
                }
                else if (toolStrip is ToolStripOverflow)
                {
                }
                else
                {
                    g.FillPath(sb, gp);
                    // g.DrawPath(pen, gp); 
                    item.ForeColor = Color.Black;
                    gp.Dispose();
                    sb.Dispose();
                    //  base.OnRenderDropDownButtonBackground(e);
                }
                g.DrawImage(Properties.Resources.icon_arrw, bounds.Width - 5, bounds.Height - 5);
            }
        }

        //渲染图片区域 下拉菜单左边的图片区域
        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            //屏蔽掉左边图片竖条
            var g = e.Graphics;
            var rect = e.AffectedBounds;
            var cl = Color.FromArgb(180, 180, 180);
            var sb = new SolidBrush(cl);
            rect.X += 1;
            rect.Y += 1;
            rect.Width -= 2;
            rect.Height -= 2;
            g.FillRectangle(sb, rect);
            sb.Dispose();
            // base.OnRenderImageMargin(e);
        }

        //SplitButton美化
        protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
        {
            var toolStrip = e.ToolStrip;
            var item = e.Item as ToolStripSplitButton;

            if (item != null)
            {
                var g = e.Graphics;
                var bounds = new Rectangle(Point.Empty, item.Size);
                var gp = DrawHelper.CreateRoundPath2(item.ContentRectangle, 5);
                var alpha = (int) Alpha.Normal;
                var sb = new SolidBrush(Color.FromArgb(alpha, 0, 0, 0));
                var pen = new Pen(Color.FromArgb(alpha, 0, 0, 0));
                if (item.BackgroundImage != null)
                {
                }
                if (item.Selected)
                {
                    //DrawHelper.RendererBackground(g, bounds, ToolHoverBgImg, true);
                    if (item.ButtonPressed)
                    {
                        // if (item.ButtonPressed)
                        // DrawHelper.RendererBackground(g, item.ButtonBounds, SpitLeftDowmImg, true);
                        //else
                        // DrawHelper.RendererBackground(g, item.ButtonBounds, SpitLeftNorImg, true);
                    }
                    // DrawHelper.RendererBackground(g, item.ButtonBounds, SpitLeftNorImg, true);
                    //else if (item.DropDownButtonSelected)
                    // {
                    //if (item.DropDownButtonPressed)
                    //   DrawHelper.RendererBackground(g, item.DropDownButtonBounds, SpitRightDowmImg, true);
                    // else
                    //        DrawHelper.RendererBackground(g, item.DropDownButtonBounds, SpitRightNorImg, true);
                    //  }
                }

                DrawArrow(
                    new ToolStripArrowRenderEventArgs(
                        g,
                        item,
                        item.DropDownButtonBounds,
                        Color.Black,
                        ArrowDirection.Down));
            }
            //base.OnRenderSplitButtonBackground(e);
        }

        // 渲染分界线
        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            var g = e.Graphics;
            var rect = e.Item.ContentRectangle;
            if (!e.Vertical)
            {
                e.Graphics.DrawLine(new Pen(Brushes.DarkGray), rect.X + 20, 3, rect.Right, 3);
            }
            else
            {
                e.Graphics.DrawImage(Properties.Resources.line, rect);

                //  e.Graphics.DrawLine(pen, rect.Width / 2, 0, rect.Width / 2, rect.Height);
                // e.Graphics.DrawLine(new Pen(Brushes.DarkGray), rect.Width / 2, 0, rect.Width / 2, rect.Height);
                //base.OnRenderSeparator(e);
            }
        }

        // 箭头颜色
        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            return;
            //if (e.Item.Enabled)
            //{
            //    e.ArrowColor = Color.Red;
            //}
            //if ((e.Item.Owner is ToolStripDropDown) && (e.Item is ToolStripMenuItem))
            //{
            //    var arrowRectangle = e.ArrowRectangle;
            //    e.ArrowRectangle = arrowRectangle;
            //}
            //base.OnRenderArrow(e);
        }

        /// <summary>
        ///     主要针对toolbar的绘制
        /// </summary>
        /// <param name="e"></param>
        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            var toolStrip = e.ToolStrip;
            var item = e.Item as ToolStripButton;
            var g = e.Graphics;
            Share.GraphicSetup(g);
            if (item != null)
            {
                var bounds = new Rectangle(Point.Empty, item.Size);
                var rectPath = DrawHelper.CreateRoundPath2(bounds, Share.DefaultRadius);
                //int alpha = (int)Alpha.Normal;
                if (item.BackgroundImage != null)
                {
                    //这里绘制背景
                }
                var sb = new SolidBrush(Share.BackColor);
                var p = new Pen(Share.BorderColor);
                //默认没有选中
                if (item.CheckState == CheckState.Unchecked)
                {
                    if (item.Selected)
                    {
                        item.ForeColor = Color.White;
                        if (item.Pressed)
                            sb.Color = p.Color = Share.BorderColor; //alpha = (int)Alpha.PressOrDown;
                        else
                            sb.Color = p.Color = Share.BorderColor; // alpha = (int)Alpha.MoveOrUp;
                    }
                    else
                    {
                        item.ForeColor = Color.Black;
                        sb.Color = p.Color = Share.BackColor;
                        //alpha = (int)Alpha.None;
                        if (toolStrip is ToolStripOverflow)
                        {
                        }
                    }
                }
                else
                {
                    if (item.Selected)
                    {
                        sb.Color = p.Color = Share.BorderColor; //alpha = (int)Alpha.MoveOrUp;
                        // DrawHelper.RendererBackground(g, bounds, ToolHoverBgImg, true);
                    }
                    if (item.Pressed)
                    {
                        sb.Color = p.Color = Share.FocusBackColor; //alpha = (int)Alpha.PressOrDown;
                        // DrawHelper.RendererBackground(g, bounds, ToolDownBgImg, true);
                    }
                }
                //SolidBrush sb = new SolidBrush(Color.FromArgb(alpha, 0, 0, 0));
                //Pen pen = new Pen(Color.FromArgb(alpha, 0, 0, 0));
                g.FillPath(sb, rectPath);
                g.DrawPath(p, rectPath);
                sb.Dispose();
                p.Dispose();
                rectPath.Dispose();
            }
        }
    }
}