namespace System.Windows.Forms
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.Drawing.Text;
    using System.Runtime.InteropServices;

    public class ImageEffects
    {
        public ImageEffects()
        {
        }

        public static unsafe Bitmap Blocks(Image srcImage)
        {
            int height = srcImage.Height;
            int width = srcImage.Width;
            Bitmap bitmap = new Bitmap(srcImage);
            BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte* numPtr = (byte*) bitmapdata.Scan0;
            int num3 = bitmapdata.Stride - (width * 3);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    byte num5 = (byte) (((numPtr[0] + numPtr[1]) + numPtr[2]) / 3);
                    if (num5 > 0x80)
                    {
                        num5 = 0xff;
                    }
                    else
                    {
                        num5 = 0;
                    }
                    numPtr[0] = num5;
                    numPtr[1] = num5;
                    numPtr[2] = num5;
                    numPtr += 3;
                }
                numPtr += num3;
            }
            bitmap.UnlockBits(bitmapdata);
            return bitmap;
        }

        public static Bitmap BothAlpha(Bitmap p_Bitmap, bool p_CentralTransparent, bool p_Crossdirection)
        {
            Bitmap image = new Bitmap(p_Bitmap.Width, p_Bitmap.Height);
            Graphics graphics = Graphics.FromImage(image);
            graphics.DrawImage(p_Bitmap, new Rectangle(0, 0, p_Bitmap.Width, p_Bitmap.Height));
            graphics.Dispose();
            Bitmap bitmap2 = new Bitmap(image.Width, image.Height);
            Graphics graphics2 = Graphics.FromImage(bitmap2);
            Point point = new Point(0, 0);
            Point point2 = new Point(bitmap2.Width, 0);
            Point point3 = new Point(bitmap2.Width, bitmap2.Height / 2);
            Point point4 = new Point(0, bitmap2.Height / 2);
            if (p_Crossdirection)
            {
                point = new Point(0, 0);
                point2 = new Point(bitmap2.Width / 2, 0);
                point3 = new Point(bitmap2.Width / 2, bitmap2.Height);
                point4 = new Point(0, bitmap2.Height);
            }
            Point[] points = new Point[] { point, point2, point3, point4 };
            PathGradientBrush brush = new PathGradientBrush(points, WrapMode.TileFlipY) {
                CenterPoint = new PointF(0f, 0f),
                FocusScales = new PointF((float) (bitmap2.Width / 2), 0f),
                CenterColor = Color.FromArgb(0, 0xff, 0xff, 0xff)
            };
            brush.SurroundColors = new Color[] { Color.FromArgb(0xff, 0xff, 0xff, 0xff) };
            if (p_Crossdirection)
            {
                brush.FocusScales = new PointF(0f, (float) bitmap2.Height);
                brush.WrapMode = WrapMode.TileFlipX;
            }
            if (p_CentralTransparent)
            {
                brush.CenterColor = Color.FromArgb(0xff, 0xff, 0xff, 0xff);
                brush.SurroundColors = new Color[] { Color.FromArgb(0, 0xff, 0xff, 0xff) };
            }
            graphics2.FillRectangle(brush, new Rectangle(0, 0, bitmap2.Width, bitmap2.Height));
            graphics2.Dispose();
            BitmapData bitmapdata = bitmap2.LockBits(new Rectangle(0, 0, bitmap2.Width, bitmap2.Height), ImageLockMode.ReadOnly, bitmap2.PixelFormat);
            byte[] destination = new byte[bitmapdata.Stride * bitmapdata.Height];
            Marshal.Copy(bitmapdata.Scan0, destination, 0, destination.Length);
            bitmap2.UnlockBits(bitmapdata);
            BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadWrite, image.PixelFormat);
            byte[] buffer = new byte[data.Stride * data.Height];
            Marshal.Copy(data.Scan0, buffer, 0, buffer.Length);
            int index = 0;
            for (int i = 0; i != data.Height; i++)
            {
                index = (i * data.Stride) + 3;
                for (int j = 0; j != data.Width; j++)
                {
                    buffer[index] = destination[index];
                    index += 4;
                }
            }
            Marshal.Copy(buffer, 0, data.Scan0, buffer.Length);
            image.UnlockBits(data);
            return image;
        }

        public static Image BrightnessChange(int percent, Image srcImage)
        {
            float num = 0.006f * percent;
            float[][] numArray = new float[5][];
            float[] numArray2 = new float[5];
            numArray2[0] = 1f;
            numArray[0] = numArray2;
            float[] numArray3 = new float[5];
            numArray3[1] = 1f;
            numArray[1] = numArray3;
            float[] numArray4 = new float[5];
            numArray4[2] = 1f;
            numArray[2] = numArray4;
            float[] numArray5 = new float[5];
            numArray5[3] = 1f;
            numArray[3] = numArray5;
            float[] numArray6 = new float[5];
            numArray6[0] = num;
            numArray6[1] = num;
            numArray6[2] = num;
            numArray6[4] = 1f;
            numArray[4] = numArray6;
            float[][] newColorMatrix = numArray;
            ColorMatrix matrix = new ColorMatrix(newColorMatrix);
            using (ImageAttributes attributes = new ImageAttributes())
            {
                attributes.SetColorMatrix(matrix);
                Bitmap image = new Bitmap(srcImage.Width, srcImage.Height);
                using (Graphics graphics = Graphics.FromImage(image))
                {
                    Rectangle destRect = new Rectangle(0, 0, image.Width, image.Height);
                    graphics.DrawImage(srcImage, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                }
                return image;
            }
        }

        public static ImageAttributes ChangeOpacity(float opacity)
        {
            float[][] numArray = new float[5][];
            float[] numArray2 = new float[5];
            numArray2[0] = 1f;
            numArray[0] = numArray2;
            float[] numArray3 = new float[5];
            numArray3[1] = 1f;
            numArray[1] = numArray3;
            float[] numArray4 = new float[5];
            numArray4[2] = 1f;
            numArray[2] = numArray4;
            float[] numArray5 = new float[5];
            numArray5[3] = opacity;
            numArray[3] = numArray5;
            float[] numArray6 = new float[5];
            numArray6[4] = 1f;
            numArray[4] = numArray6;
            float[][] newColorMatrix = numArray;
            ColorMatrix matrix = new ColorMatrix(newColorMatrix);
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            return attributes;
        }

        public static Bitmap ChangeOpacity(Bitmap srcImage, float opacity)
        {
            if (opacity == 1f)
            {
                return new Bitmap(srcImage);
            }
            if (opacity == 0f)
            {
                return new Bitmap(srcImage.Width, srcImage.Height);
            }
            float[][] numArray = new float[5][];
            float[] numArray2 = new float[5];
            numArray2[0] = 1f;
            numArray[0] = numArray2;
            float[] numArray3 = new float[5];
            numArray3[1] = 1f;
            numArray[1] = numArray3;
            float[] numArray4 = new float[5];
            numArray4[2] = 1f;
            numArray[2] = numArray4;
            float[] numArray5 = new float[5];
            numArray5[3] = opacity;
            numArray[3] = numArray5;
            float[] numArray6 = new float[5];
            numArray6[4] = 1f;
            numArray[4] = numArray6;
            float[][] newColorMatrix = numArray;
            ColorMatrix matrix = new ColorMatrix(newColorMatrix);
            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            Bitmap image = new Bitmap(srcImage.Width, srcImage.Height);
            Graphics.FromImage(image).DrawImage(srcImage, new Rectangle(0, 0, srcImage.Width, srcImage.Height), 0, 0, srcImage.Width, srcImage.Height, GraphicsUnit.Pixel, imageAttr);
            return image;
        }

        public static void ChangeOpacity(float opacity, ImageAttributes imageAttributes)
        {
            float[][] numArray = new float[5][];
            float[] numArray2 = new float[5];
            numArray2[0] = 1f;
            numArray[0] = numArray2;
            float[] numArray3 = new float[5];
            numArray3[1] = 1f;
            numArray[1] = numArray3;
            float[] numArray4 = new float[5];
            numArray4[2] = 1f;
            numArray[2] = numArray4;
            float[] numArray5 = new float[5];
            numArray5[3] = opacity;
            numArray[3] = numArray5;
            float[] numArray6 = new float[5];
            numArray6[4] = 1f;
            numArray[4] = numArray6;
            float[][] newColorMatrix = numArray;
            ColorMatrix matrix = new ColorMatrix(newColorMatrix);
            imageAttributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
        }

        public static unsafe void ChangeOpacity(Bitmap bitmap, double opacity, Rectangle rect)
        {
            BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* numPtr = (byte*) bitmapdata.Scan0;
            int num = bitmapdata.Stride - (rect.Width * 4);
            for (int i = 0; i < rect.Height; i++)
            {
                for (int j = 0; j < rect.Width; j++)
                {
                    numPtr[3] = (byte) (numPtr[3] * opacity);
                    numPtr += 4;
                }
                numPtr += num;
            }
            bitmap.UnlockBits(bitmapdata);
        }

        public static void DrawLightString(string text, Graphics g, Font font, Color light, Color color, Rectangle rect, StringFormat sf, int lightWidth)
        {
            if (!string.IsNullOrEmpty(text))
            {
                GraphicsUnit pageUnit = g.PageUnit;
                SmoothingMode smoothingMode = g.SmoothingMode;
                InterpolationMode interpolationMode = g.InterpolationMode;
                using (GraphicsPath path = new GraphicsPath())
                {
                    g.PageUnit = GraphicsUnit.Point;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    path.AddString(text, font.FontFamily, (int) font.Style, font.SizeInPoints, rect, sf);
                    if (lightWidth > 0)
                    {
                        using (Bitmap bitmap = new Bitmap(rect.Width / lightWidth, rect.Height / lightWidth))
                        {
                            using (Graphics graphics = Graphics.FromImage(bitmap))
                            {
                                Matrix matrix = new Matrix(1f / ((float) lightWidth), 0f, 0f, 1f / ((float) lightWidth), -1f / ((float) lightWidth), -1f / ((float) lightWidth));
                                graphics.SmoothingMode = SmoothingMode.HighQuality;
                                graphics.Transform = matrix;
                                using (Pen pen = new Pen(light, (float) lightWidth))
                                {
                                    graphics.DrawPath(pen, path);
                                }
                            }
                            g.DrawImage(bitmap, new Rectangle(1, 0, rect.Width, rect.Height), 0, 0, bitmap.Width, bitmap.Height, GraphicsUnit.Pixel);
                        }
                    }
                    using (SolidBrush brush = new SolidBrush(color))
                    {
                        if (lightWidth > 0)
                        {
                            g.FillPath(brush, path);
                        }
                        else
                        {
                            g.DrawString(text, font, brush, rect, sf);
                        }
                    }
                    g.PageUnit = pageUnit;
                    g.SmoothingMode = smoothingMode;
                    g.InterpolationMode = interpolationMode;
                }
            }
        }

        public static unsafe Image GradualAlpha(Image srcImage, Rectangle rect, int direction)
        {
            Bitmap bitmap = new Bitmap(srcImage);
            double num = 1.0;
            BitmapData bitmapdata = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* numPtr = (byte*) bitmapdata.Scan0;
            int num2 = bitmapdata.Stride - (rect.Width * 4);
            for (int i = 0; i < rect.Height; i++)
            {
                if (direction == 0)
                {
                    num = (1.0 * i) / ((double) (rect.Height - 1));
                }
                else if (direction == 1)
                {
                    num = 1.0 - ((1.0 * i) / ((double) (rect.Height - 1)));
                }
                for (int j = 0; j < rect.Width; j++)
                {
                    if (direction == 2)
                    {
                        num = (1.0 * j) / ((double) (rect.Width - 1));
                    }
                    else if (direction == 3)
                    {
                        num = 1.0 - ((1.0 * j) / ((double) (rect.Width - 1)));
                    }
                    numPtr[3] = (byte) (numPtr[3] * num);
                    numPtr += 4;
                }
                numPtr += num2;
            }
            bitmap.UnlockBits(bitmapdata);
            return bitmap;
        }

        public static unsafe void GradualAlpha(Rectangle rect, int direction, Bitmap srcImage)
        {
            double num = 1.0;
            BitmapData bitmapdata = srcImage.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* numPtr = (byte*) bitmapdata.Scan0;
            int num2 = bitmapdata.Stride - (rect.Width * 4);
            for (int i = 0; i < rect.Height; i++)
            {
                if (direction == 0)
                {
                    num = (1.0 * i) / ((double) (rect.Height - 1));
                }
                else if (direction == 1)
                {
                    num = 1.0 - ((1.0 * i) / ((double) (rect.Height - 1)));
                }
                for (int j = 0; j < rect.Width; j++)
                {
                    if (direction == 2)
                    {
                        num = (1.0 * j) / ((double) (rect.Width - 1));
                    }
                    else if (direction == 3)
                    {
                        num = 1.0 - ((1.0 * j) / ((double) (rect.Width - 1)));
                    }
                    numPtr[3] = (byte) (numPtr[3] * num);
                    numPtr += 4;
                }
                numPtr += num2;
            }
            srcImage.UnlockBits(bitmapdata);
        }

        public static Image ImageLightEffect(string Str, Font F, Color ColorFore, Color ColorBack, int BlurConsideration, Rectangle rc, bool auto)
        {
            Bitmap image = null;
            StringFormat format = new StringFormat {
                Trimming = auto ? StringTrimming.EllipsisWord : StringTrimming.None,
                LineAlignment = StringAlignment.Center
            };
            using (Graphics.FromHwnd(IntPtr.Zero))
            {
                using (Bitmap bitmap2 = new Bitmap(rc.Width, rc.Height))
                {
                    using (Graphics graphics2 = Graphics.FromImage(bitmap2))
                    {
                        using (SolidBrush brush = new SolidBrush(Color.FromArgb(0x10, ColorBack.R, ColorBack.G, ColorBack.B)))
                        {
                            using (SolidBrush brush2 = new SolidBrush(ColorFore))
                            {
                                graphics2.SmoothingMode = SmoothingMode.HighQuality;
                                graphics2.InterpolationMode = InterpolationMode.HighQualityBilinear;
                                graphics2.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                                graphics2.DrawString(Str, F, brush, rc, format);
                                image = new Bitmap(bitmap2.Width, bitmap2.Height);
                                using (Graphics graphics3 = Graphics.FromImage(image))
                                {
                                    if (ColorBack != Color.Transparent)
                                    {
                                        graphics3.SmoothingMode = SmoothingMode.HighQuality;
                                        graphics3.InterpolationMode = InterpolationMode.HighQualityBilinear;
                                        graphics3.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                                        for (int i = 0; i <= BlurConsideration; i++)
                                        {
                                            for (int j = 0; j <= BlurConsideration; j++)
                                            {
                                                graphics3.DrawImageUnscaled(bitmap2, i, j);
                                            }
                                        }
                                    }
                                    graphics3.DrawString(Str, F, brush2, new Rectangle(new Point(Convert.ToInt32((int) (BlurConsideration / 2)), Convert.ToInt32((int) (BlurConsideration / 2))), rc.Size), format);
                                }
                                return image;
                            }
                        }
                    }
                }
            }
        }

        public static Image ImageLightEffect(string Str, Font F, Color ColorFore, Color ColorBack, int BlurConsideration, Rectangle rc, StringFormat sf, TextRenderingHint textRender)
        {
            Bitmap image = null;
            using (Graphics.FromHwnd(IntPtr.Zero))
            {
                using (Bitmap bitmap2 = new Bitmap(rc.Width, rc.Height))
                {
                    using (Graphics graphics2 = Graphics.FromImage(bitmap2))
                    {
                        using (SolidBrush brush = new SolidBrush(Color.FromArgb(0x10, ColorBack.R, ColorBack.G, ColorBack.B)))
                        {
                            using (SolidBrush brush2 = new SolidBrush(ColorFore))
                            {
                                graphics2.SmoothingMode = SmoothingMode.HighQuality;
                                graphics2.TextRenderingHint = textRender;
                                graphics2.DrawString(Str, F, brush, rc, sf);
                                image = new Bitmap(bitmap2.Width, bitmap2.Height);
                                using (Graphics graphics3 = Graphics.FromImage(image))
                                {
                                    if (ColorBack != Color.Transparent)
                                    {
                                        graphics3.SmoothingMode = SmoothingMode.HighQuality;
                                        graphics3.TextRenderingHint = textRender;
                                        for (int i = 0; i <= BlurConsideration; i++)
                                        {
                                            for (int j = 0; j <= BlurConsideration; j++)
                                            {
                                                graphics3.DrawImageUnscaled(bitmap2, i, j);
                                            }
                                        }
                                    }
                                    graphics3.DrawString(Str, F, brush2, new Rectangle(new Point(Convert.ToInt32((int) (BlurConsideration / 2)) + rc.X, Convert.ToInt32((int) (BlurConsideration / 2)) + rc.Y), rc.Size), sf);
                                }
                                return image;
                            }
                        }
                    }
                }
            }
        }

        public static unsafe Bitmap KiContrast(Image srcImage, int degree)
        {
            int height = srcImage.Height;
            int width = srcImage.Width;
            Bitmap bitmap = new Bitmap(srcImage);
            if (degree < -100)
            {
                degree = -100;
            }
            if (degree > 100)
            {
                degree = 100;
            }
            try
            {
                double num3 = 0.0;
                double num4 = (100.0 + degree) / 100.0;
                num4 *= num4;
                BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                byte* numPtr = (byte*) bitmapdata.Scan0;
                int num5 = bitmapdata.Stride - (width * 3);
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        for (int k = 0; k < 3; k++)
                        {
                            num3 = ((((((double) numPtr[k]) / 255.0) - 0.5) * num4) + 0.5) * 255.0;
                            if (num3 < 0.0)
                            {
                                num3 = 0.0;
                            }
                            if (num3 > 255.0)
                            {
                                num3 = 255.0;
                            }
                            numPtr[k] = (byte) num3;
                        }
                        numPtr += 3;
                    }
                    numPtr += num5;
                }
                bitmap.UnlockBits(bitmapdata);
                return bitmap;
            }
            catch
            {
                return null;
            }
        }

        public static unsafe Image MaSaiKe(Image m_PreImage, int val)
        {
            Bitmap bitmap = new Bitmap(m_PreImage);
            int width = bitmap.Width;
            int height = bitmap.Height;
            int num3 = 0;
            int num4 = 0;
            int num5 = 0;
            BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte* numPtr = (byte*) bitmapdata.Scan0.ToPointer();
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if ((i % val) == 0)
                    {
                        if ((j % val) == 0)
                        {
                            num3 = numPtr[2];
                            num4 = numPtr[1];
                            num5 = numPtr[0];
                        }
                        else
                        {
                            numPtr[0] = (byte) num5;
                            numPtr[1] = (byte) num4;
                            numPtr[2] = (byte) num3;
                        }
                    }
                    else
                    {
                        byte* numPtr2 = numPtr - bitmapdata.Stride;
                        numPtr[0] = numPtr2[0];
                        numPtr[1] = numPtr2[1];
                        numPtr[2] = numPtr2[2];
                    }
                    numPtr += 3;
                }
                numPtr += bitmapdata.Stride - (width * 3);
            }
            bitmap.UnlockBits(bitmapdata);
            return bitmap;
        }

        public static unsafe Image NegativeImage(Image srcImage)
        {
            int height = srcImage.Height;
            int width = srcImage.Width;
            Bitmap bitmap = new Bitmap(srcImage);
            BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte* numPtr = (byte*) bitmapdata.Scan0;
            int num3 = bitmapdata.Stride - (width * 3);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        numPtr[k] = (byte) (0xff - numPtr[k]);
                    }
                    numPtr += 3;
                }
                numPtr += num3;
            }
            bitmap.UnlockBits(bitmapdata);
            return bitmap;
        }

        public static unsafe Bitmap Relief(Image srcImage)
        {
            int height = srcImage.Height;
            int width = srcImage.Width;
            Bitmap bitmap = new Bitmap(srcImage);
            BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte* numPtr = (byte*) bitmapdata.Scan0;
            int num3 = bitmapdata.Stride - (width * 3);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    int num6 = 0;
                    int num5 = 0;
                    int num7 = 0;
                    if ((j < (width - 1)) && (i < (height - 1)))
                    {
                        num6 = Math.Abs((int) ((numPtr[0] - numPtr[3 * (width + 1)]) + 0x80));
                        num5 = Math.Abs((int) ((numPtr[1] - numPtr[(3 * (width + 1)) + 1]) + 0x80));
                        num7 = Math.Abs((int) ((numPtr[2] - numPtr[(3 * (width + 1)) + 2]) + 0x80));
                    }
                    else
                    {
                        num6 = 0x80;
                        num5 = 0x80;
                        num7 = 0x80;
                    }
                    if (num6 > 0xff)
                    {
                        num6 = 0xff;
                    }
                    if (num6 < 0)
                    {
                        num6 = 0;
                    }
                    if (num5 > 0xff)
                    {
                        num5 = 0xff;
                    }
                    if (num5 < 0)
                    {
                        num5 = 0;
                    }
                    if (num7 > 0xff)
                    {
                        num7 = 0xff;
                    }
                    if (num7 < 0)
                    {
                        num7 = 0;
                    }
                    numPtr[0] = (byte) num6;
                    numPtr[1] = (byte) num5;
                    numPtr[2] = (byte) num7;
                    numPtr += 3;
                }
                numPtr += num3;
            }
            bitmap.UnlockBits(bitmapdata);
            return bitmap;
        }

        public static Image RotateImage(Image srcImage, float angle, Point center)
        {
            Bitmap bitmap;
            Bitmap bitmap2;
            if (srcImage.Width > srcImage.Height)
            {
                bitmap = new Bitmap(srcImage.Width, srcImage.Width);
                bitmap2 = new Bitmap(srcImage.Width, srcImage.Width);
            }
            else
            {
                bitmap = new Bitmap(srcImage.Height, srcImage.Height);
                bitmap2 = new Bitmap(srcImage.Height, srcImage.Height);
            }
            using (Graphics graphics = Graphics.FromImage(bitmap2))
            {
                graphics.DrawImage(srcImage, (int) ((bitmap.Width - srcImage.Width) / 2), (int) ((bitmap.Height - srcImage.Height) / 2));
            }
            using (Graphics graphics2 = Graphics.FromImage(bitmap))
            {
                graphics2.TranslateTransform((float) center.X, (float) center.Y);
                graphics2.RotateTransform(angle);
                graphics2.TranslateTransform((float) -center.X, (float) -center.Y);
                graphics2.DrawImage(bitmap2, 0, 0);
                graphics2.ResetTransform();
                graphics2.Save();
            }
            bitmap2.Dispose();
            return bitmap;
        }

        public static unsafe void SingleColor(Bitmap bmp)
        {
            int height = bmp.Height;
            int width = bmp.Width;
            int num3 = 0;
            BitmapData bitmapdata = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* numPtr = (byte*) bitmapdata.Scan0;
            int num4 = bitmapdata.Stride - (width * 4);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    num3 = (((int) (0.7 * numPtr[0])) + ((int) (0.2 * numPtr[1]))) + ((int) (0.1 * numPtr[2]));
                    num3 = Math.Min(0xff, num3);
                    numPtr[0] = (byte) num3;
                    numPtr[1] = (byte) num3;
                    numPtr[2] = (byte) num3;
                    numPtr += 4;
                }
                numPtr += num4;
            }
            bmp.UnlockBits(bitmapdata);
        }

        public static unsafe Bitmap SingleColor(Image srcImage)
        {
            int height = srcImage.Height;
            int width = srcImage.Width;
            Bitmap bitmap = new Bitmap(srcImage);
            int num3 = 0;
            BitmapData bitmapdata = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            byte* numPtr = (byte*) bitmapdata.Scan0;
            int num4 = bitmapdata.Stride - (width * 4);
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    num3 = (((int) (0.7 * numPtr[0])) + ((int) (0.2 * numPtr[1]))) + ((int) (0.1 * numPtr[2]));
                    num3 = Math.Min(0xff, num3);
                    numPtr[0] = (byte) num3;
                    numPtr[1] = (byte) num3;
                    numPtr[2] = (byte) num3;
                    numPtr += 4;
                }
                numPtr += num4;
            }
            bitmap.UnlockBits(bitmapdata);
            return bitmap;
        }

        public static void SudokuDrawImage(Graphics g, Image img, Rectangle rect, int width)
        {
            g.DrawImage(img, new Rectangle(rect.X, rect.Y, width, width), new Rectangle(0, 0, width, width), GraphicsUnit.Pixel);
            g.DrawImage(img, new Rectangle(rect.Right - width, rect.Y, width, width), new Rectangle(img.Width - width, 0, width, width), GraphicsUnit.Pixel);
            g.DrawImage(img, new Rectangle(rect.X, rect.Bottom - width, width, width), new Rectangle(0, img.Height - width, width, width), GraphicsUnit.Pixel);
            g.DrawImage(img, new Rectangle(rect.Right - width, rect.Bottom - width, width, width), new Rectangle(img.Width - width, img.Height - width, width, width), GraphicsUnit.Pixel);
            g.DrawImage(img, new Rectangle(rect.X, rect.Y + width, width, rect.Height - (width * 2)), new Rectangle(0, width, width, img.Height - (width * 2)), GraphicsUnit.Pixel);
            g.DrawImage(img, new Rectangle(rect.X + width, rect.Y, rect.Width - (width * 2), width), new Rectangle(width, 0, img.Width - (width * 2), width), GraphicsUnit.Pixel);
            g.DrawImage(img, new Rectangle(rect.Right - width, rect.Y + width, width, rect.Height - (width * 2)), new Rectangle(img.Width - width, width, width, img.Height - (width * 2)), GraphicsUnit.Pixel);
            g.DrawImage(img, new Rectangle(rect.X + width, rect.Bottom - width, rect.Width - (width * 2), width), new Rectangle(width, img.Height - width, img.Width - (width * 2), width), GraphicsUnit.Pixel);
            g.DrawImage(img, new Rectangle(rect.X + width, rect.Y + width, rect.Width - (width * 2), rect.Height - (width * 2)), new Rectangle(width, width, img.Width - (width * 2), img.Height - (width * 2)), GraphicsUnit.Pixel);
        }

        public static unsafe Bitmap TrapezoidTransformation(Bitmap src, double compressH, double compressW, bool isLeft, bool isCenter)
        {
            Rectangle rect = new Rectangle(0, 0, src.Width, src.Height);
            using (Bitmap bitmap = new Bitmap(rect.Width, rect.Height))
            {
                Bitmap bitmap2 = new Bitmap(rect.Width, rect.Height);
                BitmapData bitmapdata = src.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                BitmapData data2 = bitmap.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                BitmapData data3 = bitmap2.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
                int num = 0;
                byte* numPtr = (byte*) bitmapdata.Scan0;
                byte* numPtr2 = (byte*) data2.Scan0;
                byte* numPtr3 = (byte*) data3.Scan0;
                int stride = bitmapdata.Stride;
                double num3 = (((1.0 - compressH) * rect.Height) / 2.0) / ((double) rect.Width);
                for (int i = 0; i < rect.Height; i++)
                {
                    for (int k = 0; k < rect.Width; k++)
                    {
                        double num9 = 0.0;
                        double num10 = 0.0;
                        if (isLeft && (i >= (num3 * (rect.Width - k))))
                        {
                            num9 = rect.Height - ((2.0 * num3) * (rect.Width - k));
                            num10 = i - (num3 * (rect.Width - k));
                        }
                        else if (!isLeft && (i >= (num3 * k)))
                        {
                            num9 = rect.Height - ((2.0 * num3) * k);
                            num10 = i - (num3 * k);
                        }
                        double num12 = (1.0 * num10) / num9;
                        int num7 = (int) (rect.Height * num12);
                        if ((num7 < rect.Height) && (num7 > -1))
                        {
                            byte* numPtr6 = (numPtr + (num7 * stride)) + (k * 4);
                            numPtr2[0] = numPtr6[0];
                            numPtr2[1] = numPtr6[1];
                            numPtr2[2] = numPtr6[2];
                            numPtr2[3] = numPtr6[3];
                        }
                        numPtr2 += 4;
                    }
                }
                numPtr2 = (byte*) data2.Scan0;
                if (isCenter)
                {
                    num = (int) ((rect.Width - (compressW * rect.Width)) / 2.0);
                }
                for (int j = 0; j < rect.Height; j++)
                {
                    for (int m = 0; m < rect.Width; m++)
                    {
                        int num6 = (int) ((1.0 * m) / compressW);
                        if ((num6 > -1) && (num6 < rect.Width))
                        {
                            byte* numPtr4 = (numPtr2 + (num6 * 4)) + (stride * j);
                            byte* numPtr5 = numPtr3 + (num * 4);
                            numPtr5[0] = numPtr4[0];
                            numPtr5[1] = numPtr4[1];
                            numPtr5[2] = numPtr4[2];
                            numPtr5[3] = numPtr4[3];
                        }
                        numPtr3 += 4;
                    }
                }
                src.UnlockBits(bitmapdata);
                bitmap.UnlockBits(data2);
                bitmap2.UnlockBits(data3);
                return bitmap2;
            }
        }
    }
}

