using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace System.Windows.Forms
{
    public sealed class ControlPaintEx
    {
        public static void DrawScrollBarTrack(Graphics g, Rectangle rect, Color begin, Color end,
            Orientation orientation)
        {
            var mode = (orientation == Orientation.Horizontal)
                ? LinearGradientMode.Vertical
                : LinearGradientMode.Horizontal;
            var blend = new Blend();
            var numArray = new float[3];
            numArray[0] = 1f;
            numArray[1] = 0.5f;
            blend.Factors = numArray;
            var numArray2 = new float[3];
            numArray2[1] = 0.5f;
            numArray2[2] = 1f;
            blend.Positions = numArray2;
            DrawGradientRect(g, rect, begin, end, begin, begin, blend, mode, true, false);
        }

        internal static void DrawGradientRect(Graphics g, Rectangle rect, Color begin, Color end, Color border,
            Color innerBorder, Blend blend, LinearGradientMode mode, bool drawBorder, bool drawInnerBorder)
        {
            using (var brush = new LinearGradientBrush(rect, begin, end, mode))
            {
                brush.Blend = blend;
                g.FillRectangle(brush, rect);
            }
            if (drawBorder)
            {
                ControlPaint.DrawBorder(g, rect, border, ButtonBorderStyle.Solid);
            }
            if (drawInnerBorder)
            {
                rect.Inflate(-1, -1);
                ControlPaint.DrawBorder(g, rect, border, ButtonBorderStyle.Solid);
            }
        }

        public static void DrawScrollBarThumb(Graphics g, Rectangle rect, Color begin, Color end, Color border,
            Color innerBorder, Orientation orientation, bool changeColor)
        {
            bool flag;
            if (changeColor)
            {
                var color = begin;
                begin = end;
                end = color;
            }
            var mode = (flag = orientation == Orientation.Horizontal)
                ? LinearGradientMode.Vertical
                : LinearGradientMode.Horizontal;
            var blend = new Blend();
            var numArray = new float[3];
            numArray[0] = 1f;
            numArray[1] = 0.5f;
            blend.Factors = numArray;
            var numArray2 = new float[3];
            numArray2[1] = 0.5f;
            numArray2[2] = 1f;
            blend.Positions = numArray2;
            if (flag)
            {
                rect.Inflate(0, -1);
            }
            else
            {
                rect.Inflate(-1, 0);
            }
            DrawGradientRoundRect(g, rect, begin, end, border, innerBorder, blend, mode, 4, RoundStyle.All, true, true);
        }

        public static Rectangle CalculateBackgroundImageRectangle(Rectangle bounds, Image backgroundImage,
            ImageLayout imageLayout)
        {
            var rectangle = bounds;
            if (backgroundImage != null)
            {
                switch (imageLayout)
                {
                    case ImageLayout.None:
                        rectangle.Size = backgroundImage.Size;
                        return rectangle;

                    case ImageLayout.Tile:
                        return rectangle;

                    case ImageLayout.Center:
                    {
                        rectangle.Size = backgroundImage.Size;
                        var size2 = bounds.Size;
                        if (size2.Width > rectangle.Width)
                        {
                            rectangle.X = (size2.Width - rectangle.Width)/2;
                        }
                        if (size2.Height > rectangle.Height)
                        {
                            rectangle.Y = (size2.Height - rectangle.Height)/2;
                        }
                        return rectangle;
                    }
                    case ImageLayout.Stretch:
                        rectangle.Size = bounds.Size;
                        return rectangle;

                    case ImageLayout.Zoom:
                    {
                        var size = backgroundImage.Size;
                        var num = bounds.Width/((float) size.Width);
                        var num2 = bounds.Height/((float) size.Height);
                        if (num >= num2)
                        {
                            rectangle.Height = bounds.Height;
                            rectangle.Width = (int) ((size.Width*num2) + 0.5);
                            if (bounds.X >= 0)
                            {
                                rectangle.X = (bounds.Width - rectangle.Width)/2;
                            }
                            return rectangle;
                        }
                        rectangle.Width = bounds.Width;
                        rectangle.Height = (int) ((size.Height*num) + 0.5);
                        if (bounds.Y >= 0)
                        {
                            rectangle.Y = (bounds.Height - rectangle.Height)/2;
                        }
                        return rectangle;
                    }
                }
            }
            return rectangle;
        }

        public static void DrawBackgroundImage(Graphics g, Image backgroundImage, Color backColor,
            ImageLayout backgroundImageLayout, Rectangle bounds, Rectangle clipRect)
        {
            DrawBackgroundImage(g, backgroundImage, backColor, backgroundImageLayout, bounds, clipRect, Point.Empty,
                RightToLeft.No);
        }

        public static void DrawBackgroundImage(Graphics g, Image backgroundImage, Color backColor,
            ImageLayout backgroundImageLayout, Rectangle bounds, Rectangle clipRect, Point scrollOffset)
        {
            DrawBackgroundImage(g, backgroundImage, backColor, backgroundImageLayout, bounds, clipRect, scrollOffset,
                RightToLeft.No);
        }

        public static void DrawBackgroundImage(Graphics g, Image backgroundImage, Color backColor,
            ImageLayout backgroundImageLayout, Rectangle bounds, Rectangle clipRect, Point scrollOffset,
            RightToLeft rightToLeft)
        {
            if (g == null)
            {
                throw new ArgumentNullException("g");
            }
            if (backgroundImageLayout == ImageLayout.Tile)
            {
                using (var brush2 = new TextureBrush(backgroundImage, WrapMode.Tile))
                {
                    if (scrollOffset != Point.Empty)
                    {
                        var transform = brush2.Transform;
                        transform.Translate(scrollOffset.X, scrollOffset.Y);
                        brush2.Transform = transform;
                    }
                    g.FillRectangle(brush2, clipRect);
                    return;
                }
            }
            var rect = CalculateBackgroundImageRectangle(bounds, backgroundImage, backgroundImageLayout);
            if ((rightToLeft == RightToLeft.Yes) && (backgroundImageLayout == ImageLayout.None))
            {
                rect.X += clipRect.Width - rect.Width;
            }
            using (var brush = new SolidBrush(backColor))
            {
                g.FillRectangle(brush, clipRect);
            }
            if (!clipRect.Contains(rect))
            {
                if ((backgroundImageLayout != ImageLayout.Stretch) && (backgroundImageLayout != ImageLayout.Zoom))
                {
                    if (backgroundImageLayout == ImageLayout.None)
                    {
                        rect.Offset(clipRect.Location);
                        var destRect = rect;
                        destRect.Intersect(clipRect);
                        var rectangle3 = new Rectangle(Point.Empty, destRect.Size);
                        g.DrawImage(backgroundImage, destRect, rectangle3.X, rectangle3.Y, rectangle3.Width,
                            rectangle3.Height, GraphicsUnit.Pixel);
                    }
                    else
                    {
                        var rectangle4 = rect;
                        rectangle4.Intersect(clipRect);
                        var rectangle5 = new Rectangle(new Point(rectangle4.X - rect.X, rectangle4.Y - rect.Y),
                            rectangle4.Size);
                        g.DrawImage(backgroundImage, rectangle4, rectangle5.X, rectangle5.Y, rectangle5.Width,
                            rectangle5.Height, GraphicsUnit.Pixel);
                    }
                }
                else
                {
                    rect.Intersect(clipRect);
                    g.DrawImage(backgroundImage, rect);
                }
            }
            else
            {
                var imageAttr = new ImageAttributes();
                imageAttr.SetWrapMode(WrapMode.TileFlipXY);
                g.DrawImage(backgroundImage, rect, 0, 0, backgroundImage.Width, backgroundImage.Height,
                    GraphicsUnit.Pixel, imageAttr);
                imageAttr.Dispose();
            }
        }

        public static void DrawCheckedFlag(Graphics graphics, Rectangle rect, Color color)
        {
            PointF[] points =
            {
                new PointF(rect.X + (rect.Width/4.5f), rect.Y + (rect.Height/2.5f)),
                new PointF(rect.X + (rect.Width/2.5f), rect.Bottom - (rect.Height/3f)),
                new PointF(rect.Right - (rect.Width/4f), rect.Y + (rect.Height/4.5f))
            };
            using (var pen = new Pen(color, 2f))
            {
                graphics.DrawLines(pen, points);
            }
        }

        public static void DrawGlass(Graphics g, RectangleF glassRect, int alphaCenter, int alphaSurround)
        {
            DrawGlass(g, glassRect, Color.White, alphaCenter, alphaSurround);
        }

        public static void DrawGlass(Graphics g, RectangleF glassRect, Color glassColor, int alphaCenter,
            int alphaSurround)
        {
            using (var path = new GraphicsPath())
            {
                path.AddEllipse(glassRect);
                using (var brush = new PathGradientBrush(path))
                {
                    brush.CenterColor = Color.FromArgb(alphaCenter, glassColor);
                    brush.SurroundColors = new[] {Color.FromArgb(alphaSurround, glassColor)};
                    brush.CenterPoint = new PointF(glassRect.X + (glassRect.Width/2f),
                        glassRect.Y + (glassRect.Height/2f));
                    g.FillPath(brush, path);
                }
            }
        }

        public static void DrawScrollBarArraw(Graphics g, Rectangle rect, Color begin, Color end, Color border,
            Color innerBorder, Color fore, Orientation orientation, ArrowDirection arrowDirection, bool changeColor)
        {
            if (changeColor)
            {
                var color = begin;
                begin = end;
                end = color;
            }
            var mode = (orientation == Orientation.Horizontal)
                ? LinearGradientMode.Vertical
                : LinearGradientMode.Horizontal;
            rect.Inflate(-1, -1);
            var blend = new Blend();
            var numArray = new float[3];
            numArray[0] = 1f;
            numArray[1] = 0.5f;
            blend.Factors = numArray;
            var numArray2 = new float[3];
            numArray2[1] = 0.5f;
            numArray2[2] = 1f;
            blend.Positions = numArray2;
            DrawGradientRoundRect(g, rect, begin, end, border, innerBorder, blend, mode, 4, RoundStyle.All, true, true);
            using (var brush = new SolidBrush(fore))
            {
                RenderHelper.RenderArrowInternal(g, rect, arrowDirection, brush);
            }
        }

        internal static void DrawGradientRoundRect(Graphics g, Rectangle rect, Color begin, Color end, Color border,
            Color innerBorder, Blend blend, LinearGradientMode mode, int radios, RoundStyle roundStyle, bool drawBorder,
            bool drawInnderBorder)
        {
            using (var path = DrawHelper.CreateRoundPath(rect, radios, roundStyle, true)) //
            {
                using (var brush = new LinearGradientBrush(rect, begin, end, mode))
                {
                    brush.Blend = blend;
                    g.FillPath(brush, path);
                }
                if (drawBorder)
                {
                    using (var pen = new Pen(border))
                    {
                        g.DrawPath(pen, path);
                    }
                }
            }
            if (drawInnderBorder)
            {
                rect.Inflate(-1, -1);
                using (var path2 = DrawHelper.CreateRoundPath(rect, radios, roundStyle, true)) //
                {
                    using (var pen2 = new Pen(innerBorder))
                    {
                        g.DrawPath(pen2, path2);
                    }
                }
            }
        }
    }
}