using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using Win32;
using Win32.Consts;
using Win32.Struct;
using Point = System.Drawing.Point;

namespace System.Windows.Forms
{
    public class SkinTools
    {
        public static Bitmap BothAlpha(Bitmap p_Bitmap, bool p_CentralTransparent, bool p_Crossdirection)
        {
            var image = new Bitmap(p_Bitmap.Width, p_Bitmap.Height);
            var graphics = Graphics.FromImage(image);
            graphics.DrawImage(p_Bitmap, new Rectangle(0, 0, p_Bitmap.Width, p_Bitmap.Height));
            graphics.Dispose();
            var bitmap2 = new Bitmap(image.Width, image.Height);
            var graphics2 = Graphics.FromImage(bitmap2);
            var point = new Point(0, 0);
            var point2 = new Point(bitmap2.Width, 0);
            var point3 = new Point(bitmap2.Width, bitmap2.Height / 2);
            var point4 = new Point(0, bitmap2.Height / 2);
            if (p_Crossdirection)
            {
                point = new Point(0, 0);
                point2 = new Point(bitmap2.Width / 2, 0);
                point3 = new Point(bitmap2.Width / 2, bitmap2.Height);
                point4 = new Point(0, bitmap2.Height);
            }
            Point[] points = { point, point2, point3, point4 };
            var brush = new PathGradientBrush(points, WrapMode.TileFlipY);
            brush.CenterPoint = new PointF(0f, 0f);
            brush.FocusScales = new PointF(bitmap2.Width / 2, 0f);
            brush.CenterColor = Color.FromArgb(0, 0xff, 0xff, 0xff);
            brush.SurroundColors = new[] { Color.FromArgb(0xff, 0xff, 0xff, 0xff) };
            if (p_Crossdirection)
            {
                brush.FocusScales = new PointF(0f, bitmap2.Height);
                brush.WrapMode = WrapMode.TileFlipX;
            }
            if (p_CentralTransparent)
            {
                brush.CenterColor = Color.FromArgb(0xff, 0xff, 0xff, 0xff);
                brush.SurroundColors = new[] { Color.FromArgb(0, 0xff, 0xff, 0xff) };
            }
            graphics2.FillRectangle(brush, new Rectangle(0, 0, bitmap2.Width, bitmap2.Height));
            graphics2.Dispose();
            var bitmapdata = bitmap2.LockBits(new Rectangle(0, 0, bitmap2.Width, bitmap2.Height), ImageLockMode.ReadOnly,
                bitmap2.PixelFormat);
            var destination = new byte[bitmapdata.Stride * bitmapdata.Height];
            Marshal.Copy(bitmapdata.Scan0, destination, 0, destination.Length);
            bitmap2.UnlockBits(bitmapdata);
            var data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite,
                image.PixelFormat);
            var buffer = new byte[data.Stride * data.Height];
            Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);
            var index = 0;
            for (var i = 0; i != data.Height; i++)
            {
                index = (i * data.Stride) + 3;
                for (var j = 0; j != data.Width; j++)
                {
                    buffer[index] = destination[index];
                    index += 4;
                }
            }
            Marshal.Copy(buffer, 0, data.Scan0, buffer.Length);
            image.UnlockBits(data);
            return image;
        }

        public static GraphicsPath CalculateControlGraphicsPath(Bitmap bitmap, int Alpha)
        {
            var path = new GraphicsPath();
            var x = 0;
            for (var i = 0; i < bitmap.Height; i++)
            {
                x = 0;
                for (var j = 0; j < bitmap.Width; j++)
                {
                    if (bitmap.GetPixel(j, i).A < Alpha)
                    {
                        continue;
                    }
                    x = j;
                    var num3 = j;
                    num3 = x;
                    while (num3 < bitmap.Width)
                    {
                        if (bitmap.GetPixel(num3, i).A < Alpha)
                        {
                            break;
                        }
                        num3++;
                    }
                    path.AddRectangle(new Rectangle(x, i, num3 - x, 1));
                    j = num3;
                }
            }
            return path;
        }

        public static bool ColorSlantsDarkOrBright(Color c)
        {
            var hsl = ColorConverterEx.smethod_0(c);
            return ((hsl.Luminance < 0.15) || ((hsl.Luminance < 0.35) || ((hsl.Luminance < 0.85) && false)));
        }

        public static void CreateControlRegion(Control control, Bitmap bitmap, int Alpha)
        {
            if ((control != null) && (bitmap != null))
            {
                control.Width = bitmap.Width;
                control.Height = bitmap.Height;
                if (control is Form)
                {
                    var form = (Form)control;
                    form.Width = control.Width;
                    form.Height = control.Height;
                    form.FormBorderStyle = FormBorderStyle.None;
                    form.BackgroundImage = bitmap;
                    var path = CalculateControlGraphicsPath(bitmap, Alpha);
                    form.Region = new Region(path);
                }
                else if (control is CButton)
                {
                    var button = (CButton)control;
                    var path3 = CalculateControlGraphicsPath(bitmap, Alpha);
                    button.Region = new Region(path3);
                }
                else if (control is CProgressBar)
                {
                    var bar = (CProgressBar)control;
                    var path2 = CalculateControlGraphicsPath(bitmap, Alpha);
                    bar.Region = new Region(path2);
                }
            }
        }

        public static void CreateRegion(Control control, Rectangle bounds)
        {
            CreateRegion(control, bounds, 8, RoundStyle.All);
        }

        public static void CreateRegion(Control ctrl, int RgnRadius)
        {
            var hRgn = NativeMethods.CreateRoundRectRgn(0, 0, ctrl.ClientRectangle.Width + 1,
                ctrl.ClientRectangle.Height + 1, RgnRadius, RgnRadius);
            NativeMethods.SetWindowRgn(ctrl.Handle, hRgn, true);
        }

        public static void CreateRegion(IntPtr hWnd, int radius, RoundStyle roundStyle, bool redraw)
        {
            var lpRect = new RECT();
            NativeMethods.GetWindowRect(hWnd, ref lpRect);
            var rect = new Rectangle(Point.Empty, lpRect.Size);
            if (roundStyle != RoundStyle.None)
            {
                using (var path = DrawHelper.CreateRoundPath(rect, radius, roundStyle, true)) //
                {
                    using (var region = new Region(path))
                    {
                        path.Widen(Pens.White);
                        region.Union(path);
                        var windowDC = NativeMethods.GetWindowDC(hWnd);
                        try
                        {
                            using (var graphics = Graphics.FromHdc(windowDC))
                            {
                                NativeMethods.SetWindowRgn(hWnd, region.GetHrgn(graphics), redraw);
                            }
                        }
                        finally
                        {
                            NativeMethods.ReleaseDC(hWnd, windowDC);
                        }
                    }
                    return;
                }
            }
            var hRgn = NativeMethods.CreateRectRgn(0, 0, rect.Width, rect.Height);
            NativeMethods.SetWindowRgn(hWnd, hRgn, redraw);
        }

        public static void CreateRegion(Control control, Rectangle bounds, int radius, RoundStyle roundStyle)
        {
            using (var path = DrawHelper.CreateRoundPath(bounds, radius, roundStyle, true)) //
            {
                var region = new Region(path);
                path.Widen(Pens.White);
                region.Union(path);
                control.Region = region;
            }
        }
        public static void CreateRegion(Control control, Rectangle bounds, int radius)
        {
            using (var path = DrawHelper.CreateRoundPath(bounds.Width - 1, bounds.Height - 1, radius)) //
            {
                var region = new Region(path);
                path.Widen(Pens.White);
                region.Union(path);
                control.Region = region;
            }
        }

        public static void CursorClick(int x, int y)
        {
            NativeMethods.mouse_event(2, (x * 0x10000) / 0x400, (y * 0x10000) / 0x300, 0, 0);
            NativeMethods.mouse_event(4, (x * 0x10000) / 0x400, (y * 0x10000) / 0x300, 0, 0);
        }

        public static Bitmap GaryImg(Bitmap b)
        {
            var bitmap = b.Clone(new Rectangle(0, 0, b.Width, b.Height), PixelFormat.Format24bppRgb);
            b.Dispose();
            var bitmapdata = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite,
                bitmap.PixelFormat);
            var destination = new byte[bitmap.Height * bitmapdata.Stride];
            Marshal.Copy(bitmapdata.Scan0, destination, 0, destination.Length);
            var num = 0;
            var width = bitmap.Width;
            while (num < width)
            {
                var num3 = 0;
                var height = bitmap.Height;
                while (num3 < height)
                {
                    byte num4;
                    destination[((num3 * bitmapdata.Stride) + (num * 3)) + 2] =
                        num4 =
                            GetAvg(destination[(num3 * bitmapdata.Stride) + (num * 3)],
                                destination[((num3 * bitmapdata.Stride) + (num * 3)) + 1],
                                destination[((num3 * bitmapdata.Stride) + (num * 3)) + 2]);
                    destination[(num3 * bitmapdata.Stride) + (num * 3)] =
                        destination[((num3 * bitmapdata.Stride) + (num * 3)) + 1] = num4;
                    num3++;
                }
                num++;
            }
            Marshal.Copy(destination, 0, bitmapdata.Scan0, destination.Length);
            bitmap.UnlockBits(bitmapdata);
            return bitmap;
        }

        private static byte GetAvg(byte b, byte g, byte r)
        {
            return (byte)(((r + g) + b) / 3);
        }

        public static Color GetImageAverageColor(Bitmap back)
        {
            return BitmapHelper.GetImageAverageColor(back);
        }

        public static Image ImageLightEffect(string Str, Font F, Color ColorFore, Color ColorBack, int BlurConsideration)
        {
            Bitmap image = null;
            using (var graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                var ef = graphics.MeasureString(Str, F);
                using (var bitmap2 = new Bitmap((int)ef.Width, (int)ef.Height))
                {
                    using (var graphics2 = Graphics.FromImage(bitmap2))
                    {
                        using (var brush = new SolidBrush(Color.FromArgb(0x10, ColorBack.R, ColorBack.G, ColorBack.B)))
                        {
                            using (var brush2 = new SolidBrush(ColorFore))
                            {
                                graphics2.SmoothingMode = SmoothingMode.HighQuality;
                                graphics2.InterpolationMode = InterpolationMode.HighQualityBilinear;
                                graphics2.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                                graphics2.DrawString(Str, F, brush, 0f, 0f);
                                image = new Bitmap(bitmap2.Width + BlurConsideration, bitmap2.Height + BlurConsideration);
                                using (var graphics3 = Graphics.FromImage(image))
                                {
                                    graphics3.SmoothingMode = SmoothingMode.HighQuality;
                                    graphics3.InterpolationMode = InterpolationMode.HighQualityBilinear;
                                    graphics3.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                                    for (var i = 0; i <= BlurConsideration; i++)
                                    {
                                        for (var j = 0; j <= BlurConsideration; j++)
                                        {
                                            graphics3.DrawImageUnscaled(bitmap2, i, j);
                                        }
                                    }
                                    graphics3.DrawString(Str, F, brush2, BlurConsideration / 2, BlurConsideration / 2);
                                }
                                return image;
                            }
                        }
                    }
                }
            }
        }

        public static Image ImageLightEffect(string Str, Font F, Color ColorFore, Color ColorBack, int BlurConsideration,
            Rectangle rc, bool auto)
        {
            Bitmap image = null;
            var format = new StringFormat(StringFormatFlags.NoWrap);
            format.Trimming = auto ? StringTrimming.EllipsisWord : StringTrimming.None;
            using (var graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                if (Str == "") Str = " ";
                var ef = graphics.MeasureString(Str, F);
                using (var bitmap2 = new Bitmap((int)ef.Width, (int)ef.Height))
                {
                    using (var graphics2 = Graphics.FromImage(bitmap2))
                    {
                        using (var brush = new SolidBrush(Color.FromArgb(0x10, ColorBack.R, ColorBack.G, ColorBack.B)))
                        {
                            using (var brush2 = new SolidBrush(ColorFore))
                            {
                                graphics2.SmoothingMode = SmoothingMode.HighQuality;
                                graphics2.InterpolationMode = InterpolationMode.HighQualityBilinear;
                                graphics2.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                                graphics2.DrawString(Str, F, brush, rc, format);
                                image = new Bitmap(bitmap2.Width + BlurConsideration, bitmap2.Height + BlurConsideration);
                                using (var graphics3 = Graphics.FromImage(image))
                                {
                                    if (ColorBack != Color.Transparent)
                                    {
                                        graphics3.SmoothingMode = SmoothingMode.HighQuality;
                                        graphics3.InterpolationMode = InterpolationMode.HighQualityBilinear;
                                        graphics3.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                                        for (var i = 0; i <= BlurConsideration; i++)
                                        {
                                            for (var j = 0; j <= BlurConsideration; j++)
                                            {
                                                graphics3.DrawImageUnscaled(bitmap2, i, j);
                                            }
                                        }
                                    }
                                    graphics3.DrawString(Str, F, brush2,
                                        new Rectangle(
                                            new Point(Convert.ToInt32(BlurConsideration / 2),
                                                Convert.ToInt32(BlurConsideration / 2)), rc.Size), format);
                                }
                                return image;
                            }
                        }
                    }
                }
            }
        }

        public static Bitmap ResizeBitmap(Bitmap b, int dstWidth, int dstHeight)
        {
            var image = new Bitmap(dstWidth, dstHeight);
            var graphics = Graphics.FromImage(image);
            graphics.InterpolationMode = InterpolationMode.Bilinear;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(b, new Rectangle(0, 0, image.Width, image.Height), new Rectangle(0, 0, b.Width, b.Height),
                GraphicsUnit.Pixel);
            graphics.Save();
            graphics.Dispose();
            return image;
        }
    }
}