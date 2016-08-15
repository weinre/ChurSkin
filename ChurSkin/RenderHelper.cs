using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    public class RenderHelper
    {
        public static void RenderFormBorder(Image borderImg, int cut, Graphics g, Rectangle Rect)
        {
            if (borderImg == null) return;
            try
            {
                var h = Rect.Height;
                var w = Rect.Width;
                var imgh = borderImg.Height;
                var imgw = borderImg.Width;

                //×óÉÏ½Ç
                g.DrawImage(borderImg, new Rectangle(0, 0, cut, cut), new Rectangle(0, 0, cut, cut), GraphicsUnit.Pixel);
                //ÉÏ±ß¿ò
                g.DrawImage(borderImg, new Rectangle(cut, 0, w - cut * 2, cut), new Rectangle(cut, 0, cut, cut),
                    GraphicsUnit.Pixel);
                //ÓÒÉÏ½Ç
                g.DrawImage(borderImg, new Rectangle(w - cut, 0, cut, cut), new Rectangle(imgw - cut, 0, cut, cut),
                    GraphicsUnit.Pixel);
                //ÓÒ±ß¿ò
                g.DrawImage(borderImg, new Rectangle(w - cut, cut, cut, h - cut * 2),
                    new Rectangle(imgw - cut, cut, cut, cut), GraphicsUnit.Pixel);
                //ÓÒÏÂ½Ç
                g.DrawImage(borderImg, new Rectangle(w - cut, h - cut, cut, cut),
                    new Rectangle(imgw - cut, imgh - cut, cut, cut), GraphicsUnit.Pixel);
                //ÏÂ±ß¿ò
                g.DrawImage(borderImg, new Rectangle(cut, h - cut, w - cut * 2, cut),
                    new Rectangle(cut, imgh - cut, cut, cut), GraphicsUnit.Pixel);
                //×óÏÂ½Ç
                g.DrawImage(borderImg, new Rectangle(0, h - cut, cut, cut), new Rectangle(0, imgh - cut, cut, cut),
                    GraphicsUnit.Pixel);
                //×ó±ß¿ò
                g.DrawImage(borderImg, new Rectangle(0, cut, cut, h - cut * 2), new Rectangle(0, cut, cut, cut),
                    GraphicsUnit.Pixel);
                borderImg.Dispose();
            }
            catch
            {
                // MessageBox.Show("¸ãÃ«°¡£¬Ã»ÓÐÕÒµ½ÕâÕÅÍ¼°¡£¬´ó¸ç£¬×ÐÏ¸µã°¡");
            }
        }

        private static Color GetColor(Color colorBase, int a, int r, int g, int b)
        {
            int num = colorBase.A;
            int num2 = colorBase.R;
            int num3 = colorBase.G;
            int num4 = colorBase.B;
            if ((a + num) > 0xff)
            {
                a = 0xff;
            }
            else
            {
                a = Math.Max(0, a + num);
            }
            if ((r + num2) > 0xff)
            {
                r = 0xff;
            }
            else
            {
                r = Math.Max(0, r + num2);
            }
            if ((g + num3) > 0xff)
            {
                g = 0xff;
            }
            else
            {
                g = Math.Max(0, g + num3);
            }
            if ((b + num4) > 0xff)
            {
                b = 0xff;
            }
            else
            {
                b = Math.Max(0, b + num4);
            }
            return Color.FromArgb(a, r, g, b);
        }

        internal static void RenderArrowInternal(Graphics g, Rectangle dropDownRect, ArrowDirection direction,
            Brush brush)
        {
            var point = new Drawing.Point(dropDownRect.Left + (dropDownRect.Width / 2),
                dropDownRect.Top + (dropDownRect.Height / 2));
            Drawing.Point[] points = null;
            switch (direction)
            {
                case ArrowDirection.Left:
                    points = new[]
                    {
                        new Drawing.Point(point.X + 1, point.Y - 4),
                        new Drawing.Point(point.X + 1, point.Y + 4),
                        new Drawing.Point(point.X - 2, point.Y)
                    };
                    break;

                case ArrowDirection.Up:
                    points = new[]
                    {
                        new Drawing.Point(point.X - 4, point.Y + 1),
                        new Drawing.Point(point.X + 4, point.Y + 1),
                        new Drawing.Point(point.X, point.Y - 2)
                    };
                    break;

                case ArrowDirection.Right:
                    points = new[]
                    {
                        new Drawing.Point(point.X - 2, point.Y - 4),
                        new Drawing.Point(point.X - 2, point.Y + 4),
                        new Drawing.Point(point.X + 1, point.Y)
                    };
                    break;

                default:
                    points = new[]
                    {
                        new Drawing.Point(point.X - 4, point.Y - 1),
                        new Drawing.Point(point.X + 4, point.Y - 1),
                        new Drawing.Point(point.X, point.Y + 2)
                    };
                    break;
            }
            g.FillPolygon(brush, points);
        }

        public static void RenderBackgroundInternal(Graphics g, Rectangle rect, Color baseColor, Color borderColor,
            Color innerBorderColor, RoundStyle style, bool drawBorder, bool drawGlass, LinearGradientMode mode)
        {
            RenderBackgroundInternal(g, rect, baseColor, borderColor, innerBorderColor, style, 8, drawBorder, drawGlass,
                mode);
        }

        public static void RenderBackgroundInternal(Graphics g, Rectangle rect, Color baseColor, Color borderColor,
            Color innerBorderColor, RoundStyle style, int roundWidth, bool drawBorder, bool drawGlass,
            LinearGradientMode mode)
        {
            RenderBackgroundInternal(g, rect, baseColor, borderColor, innerBorderColor, style, 8, 0.45f, drawBorder,
                drawGlass, mode);
        }

        public static void RenderBackgroundInternal(
            Graphics g,
            Rectangle rect,
            Color baseColor,
            Color borderColor,
            Color innerBorderColor,
            RoundStyle style,
            int roundWidth,
            float basePosition,
            bool drawBorder,
            bool drawGlass,
            LinearGradientMode mode)
        {
            if (drawBorder)
            {
                rect.Width--;
                rect.Height--;
            }
            if (rect.Width == 0 || rect.Height == 0)
            {
                return;
            }
            using (var brush = new LinearGradientBrush(rect, Color.Transparent, Color.Transparent, mode))
            {
                Color[] colorArray =
                {
                    GetColor(baseColor, 0, 0x23, 0x18, 9), GetColor(baseColor, 0, 13, 8, 3), baseColor,
                    GetColor(baseColor, 0, 0x23, 0x18, 9)
                };
                var blend = new ColorBlend();
                var numArray = new float[4];
                numArray[1] = basePosition;
                numArray[2] = basePosition + 0.05f;
                numArray[3] = 1f;
                blend.Positions = numArray;
                blend.Colors = colorArray;
                brush.InterpolationColors = blend;
                if (style != RoundStyle.None)
                {
                    //using (GraphicsPath path = GraphicsPathHelper.CreatePath(rect, roundWidth, style, false))
                    using (var path = DrawHelper.CreateRoundPath(rect, roundWidth, style, false))
                    {
                        g.FillPath(brush, path);
                    }
                    if (drawGlass)
                    {
                        if (baseColor.A > 80)
                        {
                            var rectangle = rect;
                            if (mode == LinearGradientMode.Vertical)
                            {
                                rectangle.Height = (int)(rectangle.Height * basePosition);
                            }
                            else
                            {
                                rectangle.Width = (int)(rect.Width * basePosition);
                            }
                            using (var path2 = DrawHelper.CreateRoundPath(rectangle, roundWidth, RoundStyle.Top, false))
                                //
                            {
                                using (var brush2 = new SolidBrush(Color.FromArgb(0x80, 0xff, 0xff, 0xff)))
                                {
                                    g.FillPath(brush2, path2);
                                }
                            }
                        }
                        RectangleF glassRect = rect;
                        if (mode == LinearGradientMode.Vertical)
                        {
                            glassRect.Y = rect.Y + (rect.Height * basePosition);
                            glassRect.Height = (rect.Height - (rect.Height * basePosition)) * 2f;
                        }
                        else
                        {
                            glassRect.X = rect.X + (rect.Width * basePosition);
                            glassRect.Width = (rect.Width - (rect.Width * basePosition)) * 2f;
                        }
                        ControlPaintEx.DrawGlass(g, glassRect, 170, 0);
                    }
                    if (!drawBorder)
                    {
                        return;
                    }
                    using (var path3 = DrawHelper.CreateRoundPath(rect, roundWidth, style, false)) //
                    {
                        using (var pen = new Pen(borderColor))
                        {
                            g.DrawPath(pen, path3);
                        }
                    }
                    rect.Inflate(-1, -1);
                    using (var path4 = DrawHelper.CreateRoundPath(rect, roundWidth, style, false)) //
                    {
                        using (var pen2 = new Pen(innerBorderColor))
                        {
                            g.DrawPath(pen2, path4);
                        }
                        return;
                    }
                }
                g.FillRectangle(brush, rect);
                if (drawGlass)
                {
                    if (baseColor.A > 80)
                    {
                        var rectangle2 = rect;
                        if (mode == LinearGradientMode.Vertical)
                        {
                            rectangle2.Height = (int)(rectangle2.Height * basePosition);
                        }
                        else
                        {
                            rectangle2.Width = (int)(rect.Width * basePosition);
                        }
                        using (var brush3 = new SolidBrush(Color.FromArgb(0x80, 0xff, 0xff, 0xff)))
                        {
                            g.FillRectangle(brush3, rectangle2);
                        }
                    }
                    RectangleF ef2 = rect;
                    if (mode == LinearGradientMode.Vertical)
                    {
                        ef2.Y = rect.Y + (rect.Height * basePosition);
                        ef2.Height = (rect.Height - (rect.Height * basePosition)) * 2f;
                    }
                    else
                    {
                        ef2.X = rect.X + (rect.Width * basePosition);
                        ef2.Width = (rect.Width - (rect.Width * basePosition)) * 2f;
                    }
                    ControlPaintEx.DrawGlass(g, ef2, 200, 0);
                }
                if (drawBorder)
                {
                    using (var pen3 = new Pen(borderColor))
                    {
                        g.DrawRectangle(pen3, rect);
                    }
                    rect.Inflate(-1, -1);
                    using (var pen4 = new Pen(innerBorderColor))
                    {
                        g.DrawRectangle(pen4, rect);
                    }
                }
            }
        }
    }
}