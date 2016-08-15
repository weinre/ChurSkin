using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Win32;
using Win32.Consts; 
namespace System.Windows.Forms
{
    public static class Tools
    {
        public static Control GetParent(Control ctr)
        {
            if (ctr.Parent == null)
            {
                return ctr;
            }
            else
            {
                return Tools.GetParent(ctr.Parent);
            }
        }

    }
    public static class DrawHelper
    {
        public static void SetBits(Form form, Bitmap bitmap)
        {
            //  if (!haveHandle) return;

            if (!Image.IsCanonicalPixelFormat(bitmap.PixelFormat) || !Image.IsAlphaPixelFormat(bitmap.PixelFormat))
                throw new ApplicationException("图片必须是32位带Alhpa通道的图片。");

            var oldBits = IntPtr.Zero;
            var screenDC = NativeMethods.GetDC(IntPtr.Zero);
            var hBitmap = IntPtr.Zero;
            var memDc = NativeMethods.CreateCompatibleDC(screenDC);

            try
            {
                var topLoc = new Point(form.Left, form.Top);
                var bitMapSize = new Size(bitmap.Width, bitmap.Height);
                var blendFunc = new Win32.Struct.BLENDFUNCTION();
                var srcLoc = new Point(0, 0);

                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                oldBits = NativeMethods.SelectObject(memDc, hBitmap);

                blendFunc.BlendOp = AC.AC_SRC_OVER;
                blendFunc.SourceConstantAlpha = 255;
                blendFunc.AlphaFormat = AC.AC_SRC_ALPHA;
                blendFunc.BlendFlags = 0;

                NativeMethods.UpdateLayeredWindow(form.Handle, screenDC, ref topLoc, ref bitMapSize, memDc, ref srcLoc,
                    0, ref blendFunc, UpdateLayerWindowParameter.ULW_ALPHA);
            }
            finally
            {
                if (hBitmap != IntPtr.Zero)
                {
                    NativeMethods.SelectObject(memDc, oldBits);
                    NativeMethods.DeleteObject(hBitmap);
                }
                NativeMethods.ReleaseDC(IntPtr.Zero, screenDC);
                NativeMethods.DeleteDC(memDc);
            }
        }

        public static void SetBits(Form form, Bitmap bitmap, byte opcity)
        {
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)

                throw new ApplicationException("The bitmap must be 32ppp with alpha-channel.");

            // The ideia of this is very simple,
            // 1. Create a compatible DC with screen;
            // 2. Select the bitmap with 32bpp with alpha-channel in the compatible DC;
            // 3. Call the UpdateLayeredWindow.
            var screenDc = NativeMethods.GetDC(IntPtr.Zero);
            var memDc = NativeMethods.CreateCompatibleDC(screenDc);
            var hBitmap = IntPtr.Zero;
            var oldBitmap = IntPtr.Zero;
            try
            {
                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0)); // grab a GDI handle from this GDI+ bitmap
                oldBitmap = NativeMethods.SelectObject(memDc, hBitmap);
                var size = new Size(bitmap.Width, bitmap.Height);
                var pointSource = new Point(0, 0);
                var topPos = new Point(form.Left, form.Top);
                var blend = new Win32.Struct.BLENDFUNCTION();
                blend.BlendOp = AC.AC_SRC_OVER;
                blend.BlendFlags = 0;
                blend.SourceConstantAlpha = opcity;
                blend.AlphaFormat = AC.AC_SRC_ALPHA;
                NativeMethods.UpdateLayeredWindow(form.Handle, screenDc, ref topPos, ref size, memDc, ref pointSource, 0,
                    ref blend, UpdateLayerWindowParameter.ULW_ALPHA);
            }
            finally
            {
                NativeMethods.ReleaseDC(IntPtr.Zero, screenDc);
                if (hBitmap != IntPtr.Zero)
                {
                    NativeMethods.SelectObject(memDc, oldBitmap);
                    //Windows.DeleteObject(hBitmap); // The documentation says that we have to use the Windows.DeleteObject... but since there is no such method I use the normal DeleteObject from NativeMethods.GDI and it's working fine without any resource leak.
                    NativeMethods.DeleteObject(hBitmap);
                }
                NativeMethods.DeleteDC(memDc);
            }
            bitmap.Dispose();
        }

        public static void DrawCaptionText(Graphics g, Rectangle textRect, string text, Font font, bool Effect,
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

        public static void DrawCaptionText(Graphics g, RectangleF textRect, string text, Font font, bool Effect,
            Color EffetBack, int EffectWidth, Color FrmColor)
        {
            if (Effect)
            {
                var size = TextRenderer.MeasureText(text, font);
                var image = SkinTools.ImageLightEffect(text, font, FrmColor, EffetBack, EffectWidth,
                    new Rectangle(0, 0, (int)textRect.Width, size.Height), true);
                g.DrawImage(image, (int)textRect.X - (EffectWidth / 2), (int)textRect.Y - (EffectWidth / 2));
            }
        }

        #region CreateRoundPath 构建圆角路径

        /// <summary>
        ///     通过弧度构建圆角路径
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="cornerRadius"></param>
        /// <returns></returns>
        public static GraphicsPath CreateRoundPath(Rectangle rect, int cornerRadius)
        {
            var roundedRect = new GraphicsPath();
            roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2,
                cornerRadius * 2, cornerRadius * 2, 0, 90);
            roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
            roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
            roundedRect.CloseFigure();
            return roundedRect;
        }

        /// <summary>
        ///     通过贝塞尔曲线构建圆角路径
        /// </summary>
        /// <param name="r">区域</param>
        /// <param name="r1">左上</param>
        /// <param name="r2">右上</param>
        /// <param name="r3">右下</param>
        /// <param name="r4">左下</param>
        /// <returns></returns>
        public static GraphicsPath CreateRoundPath(Rectangle r, float r1, float r2, float r3, float r4)
        {
            float x = r.X;
            float y = r.Y;
            float width = r.Width - 1;
            float height = r.Height - 1;
            var path = new GraphicsPath();
            path.AddBezier(x, y + r1, x, y, x + r1, y, x + r1, y);
            path.AddLine(x + r1, y, (x + width) - r2, y);
            path.AddBezier((x + width) - r2, y, x + width, y, x + width, y + r2, x + width, y + r2);
            path.AddLine(x + width, y + r2, x + width, (y + height) - r3);
            path.AddBezier(x + width, (y + height) - r3, x + width, y + height, (x + width) - r3, y + height,
                (x + width) - r3, y + height);
            path.AddLine((x + width) - r3, y + height, x + r4, y + height);
            path.AddBezier(x + r4, y + height, x, y + height, x, (y + height) - r4, x, (y + height) - r4);
            path.AddLine(x, (y + height) - r4, x, y + r1);
            return path;
        }

        /// <summary>
        ///     构建圆角路径
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="p">左上右下</param>
        /// <returns></returns>
        public static GraphicsPath CreateRoundPath(Rectangle rect, Padding p)
        {
            var path = new GraphicsPath();
            var arcRect = new Rectangle();
            if (p == new Padding(0))
            {
                path.AddRectangle(rect);
                path.CloseFigure(); //闭合曲线
                return path;
            }
            rect.Width--;
            rect.Height--;
            // 左上角
            arcRect = new Rectangle(rect.Location, new Size(p.Left * 2, p.Left * 2));
            path.AddArc(arcRect, 180, 90);

            // 右上角
            arcRect = new Rectangle(rect.Location, new Size(p.Top * 2, p.Top * 2));
            arcRect.X = rect.Right - p.Top * 2;
            path.AddArc(arcRect, 270, 90);

            // 右下角
            arcRect = new Rectangle(rect.Location, new Size(p.Right * 2, p.Right * 2));
            arcRect.X = rect.Right - p.Right * 2;
            arcRect.Y = rect.Bottom - p.Right * 2;
            path.AddArc(arcRect, 0, 90);

            // 左下角
            arcRect = new Rectangle(rect.Location, new Size(p.Bottom * 2, p.Bottom * 2));
            arcRect.X = rect.Left;
            arcRect.Y = rect.Bottom - p.Bottom * 2;
            path.AddArc(arcRect, 90, 90);
            path.CloseFigure(); //闭合曲线
            return path;
        }

        /// <summary>
        ///     通过弧度构建圆角路径
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static GraphicsPath CreateRoundPath2(RectangleF rect, float radius)
        {
            var arcRect = new RectangleF(rect.Location, new SizeF(radius, radius));
            var path = new GraphicsPath();
            // 左上角
            path.AddArc(arcRect, 180, 90);
            // 右上角
            arcRect.X = rect.Right - radius - 1;
            path.AddArc(arcRect, 270, 90);
            // 右下角
            arcRect.Y = rect.Bottom - radius - 1;
            path.AddArc(arcRect, 0, 90);
            // 左下角
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);
            path.CloseFigure(); //闭合曲线
            return path;
        }

        /// <summary>
        ///     通过弧度构建圆角路径
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static GraphicsPath CreateRoundPath2(Rectangle rect, int diameter)
        {
            var path = new GraphicsPath();
            var arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
            // 左上角
            path.AddArc(arcRect, 180, 90);

            // 右上角
            arcRect.X = rect.Right - diameter - 1;
            path.AddArc(arcRect, 270, 90);

            // 右下角
            arcRect.Y = rect.Bottom - diameter - 1;
            path.AddArc(arcRect, 0, 90);

            // 左下角
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);
            path.CloseFigure(); //闭合曲线
            return path;
        }

        /// <summary>
        ///     更具结构体返回谷歌的浏览器Tab选项的路径
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="diameter"></param>
        /// <returns></returns>
        public static GraphicsPath GetGoogleTabPath(Rectangle rect, int diameter)
        {
            rect.Width += 10; //原来的宽度要加上12 用来画 叉叉按钮
            var arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
            var path = new GraphicsPath();
            // 左上角
            arcRect.X = rect.X + 10;
            path.AddArc(arcRect, 180, 90);

            // 右上角
            arcRect.X = rect.Right - 8 - diameter;
            path.AddArc(arcRect, 270, 90);


            path.AddLine(rect.Right, rect.Bottom, rect.X, rect.Bottom);
            path.CloseFigure(); //闭合曲线
            return path;
        }

        /// <summary>
        ///     建立圆角路径的样式。
        /// </summary>
        /// <summary>
        ///     建立带有圆角样式的路径。
        /// </summary>
        /// <param name="rect">用来建立路径的矩形。</param>
        /// <param name="_radius">圆角的大小。</param>
        /// <param name="style">圆角的样式。</param>
        /// <param name="correction">是否把矩形长宽减 1,以便画出边框。</param>
        /// <returns>建立的路径。</returns>
        public static GraphicsPath CreateRoundPath(Rectangle rect, int radius, RoundStyle style, bool correction)
        {
            var path = new GraphicsPath();
            var radiusCorrection = correction ? 1 : 0;
            if (radius <= 0)
            {
                rect.Width = rect.Width - radiusCorrection;
                rect.Height = rect.Height - radiusCorrection;
                path.AddRectangle(rect);
                path.CloseFigure();

                return path;
            }
            switch (style)
            {
                case RoundStyle.None:
                    rect.Width = rect.Width - radiusCorrection;
                    rect.Height = rect.Height - radiusCorrection;
                    path.AddRectangle(rect);
                    break;
                case RoundStyle.All:
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    path.AddArc(
                        rect.Right - radius - radiusCorrection,
                        rect.Y,
                        radius,
                        radius,
                        270,
                        90);
                    path.AddArc(
                        rect.Right - radius - radiusCorrection,
                        rect.Bottom - radius - radiusCorrection,
                        radius,
                        radius, 0, 90);
                    path.AddArc(
                        rect.X,
                        rect.Bottom - radius - radiusCorrection,
                        radius,
                        radius,
                        90,
                        90);
                    break;
                case RoundStyle.Left:
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    path.AddLine(
                        rect.Right - radiusCorrection, rect.Y,
                        rect.Right - radiusCorrection, rect.Bottom - radiusCorrection);
                    path.AddArc(
                        rect.X,
                        rect.Bottom - radius - radiusCorrection,
                        radius,
                        radius,
                        90,
                        90);
                    break;
                case RoundStyle.Right:
                    path.AddArc(
                        rect.Right - radius - radiusCorrection,
                        rect.Y,
                        radius,
                        radius,
                        270,
                        90);
                    path.AddArc(
                        rect.Right - radius - radiusCorrection,
                        rect.Bottom - radius - radiusCorrection,
                        radius,
                        radius,
                        0,
                        90);
                    path.AddLine(rect.X, rect.Bottom - radiusCorrection, rect.X, rect.Y);
                    break;
                case RoundStyle.Top:
                    path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
                    path.AddArc(
                        rect.Right - radius - radiusCorrection,
                        rect.Y,
                        radius,
                        radius,
                        270,
                        90);
                    path.AddLine(
                        rect.Right - radiusCorrection, rect.Bottom - radiusCorrection,
                        rect.X, rect.Bottom - radiusCorrection);
                    break;
                case RoundStyle.Bottom:
                    path.AddArc(
                        rect.Right - radius - radiusCorrection,
                        rect.Bottom - radius - radiusCorrection,
                        radius,
                        radius,
                        0,
                        90);
                    path.AddArc(
                        rect.X,
                        rect.Bottom - radius - radiusCorrection,
                        radius,
                        radius,
                        90,
                        90);
                    path.AddLine(rect.X, rect.Y, rect.Right - radiusCorrection, rect.Y);
                    break;
            }
            path.CloseFigure();

            return path;
        }

        /// <summary>
        ///     8个点组成的框框
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static GraphicsPath CreateRoundPath(int w, int h, int radian)
        {
            //int radian = r; //圆弧角的比率，可以自己改变这个值看具体的效果
            //对于矩形的窗体，要在一个角上画个弧度至少需要2个点，所以4个角需要至少8个点
            var p1 = new Drawing.Point(radian, 0);
            var p2 = new Drawing.Point(w - radian, 0);
            var p3 = new Drawing.Point(w, radian);
            var p4 = new Drawing.Point(w, h - radian - 1);
            var p5 = new Drawing.Point(w - radian - 1, h);
            var p6 = new Drawing.Point(radian, h);
            var p7 = new Drawing.Point(0, h - radian - 1);
            var p8 = new Drawing.Point(0, radian);

            var shape = new GraphicsPath();

            Drawing.Point[] p = { p1, p2, p3, p4, p5, p6, p7, p8 };
            shape.AddPolygon(p);
            return shape;
        }

        /// <summary>
        ///     8个点组成的框框,活动坐标
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static GraphicsPath CreateRoundPathFPoint(Rectangle rect, int radian)
        {
            //int radian = r; //圆弧角的比率，可以自己改变这个值看具体的效果
            //对于矩形的窗体，要在一个角上画个弧度至少需要2个点，所以4个角需要至少8个点
            var p1 = new Drawing.Point(rect.Left + radian, rect.Top);
            var p2 = new Drawing.Point(rect.Right - radian, rect.Top);
            var p3 = new Drawing.Point(rect.Right, rect.Top + radian);
            var p4 = new Drawing.Point(rect.Right, rect.Bottom - radian - 1);
            var p5 = new Drawing.Point(rect.Right - radian - 1, rect.Bottom);
            var p6 = new Drawing.Point(rect.Left + radian, rect.Bottom);
            var p7 = new Drawing.Point(rect.Left, rect.Bottom - radian - 1);
            var p8 = new Drawing.Point(rect.Left, rect.Top + radian);

            var shape = new GraphicsPath();

            Drawing.Point[] p = { p1, p2, p3, p4, p5, p6, p7, p8 };
            shape.AddPolygon(p);
            return shape;
        }

        /// <summary>
        ///     通过8个点来切角
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="r"></param>
        public static void SetWindowRegion(Control handle, int r)
        {
            var shape = CreateRoundPath(handle.ClientRectangle.Width, handle.ClientRectangle.Height, r);
            //GraphicsPath shape = CreateRoundPath2(handle.ClientRectangle, 7);
            //将窗体的显示区域设为GraphicsPath的实例
            handle.Region = new Region(shape);
        }

        /// <summary>
        ///     通过弧度来切角
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="r"></param>
        public static void SetWindowRoundRegion(Control handle, int r)
        {
            var shape = CreateRoundPath2(handle.ClientRectangle, r);
            //GraphicsPath shape = CreateRoundPath2(handle.ClientRectangle, 7);
            //将窗体的显示区域设为GraphicsPath的实例
            handle.Region = new Region(shape);
        }

        /// <summary>
        ///     8个点来切角，用于ToolStrip
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="r"></param>
        public static void SetWindowRegion(ToolStrip handle, int r)
        {
            var w = handle.Width; //窗体宽
            var h = handle.Height; //窗体高 
            var shape = CreateRoundPath(w, h, r);
            //将窗体的显示区域设为GraphicsPath的实例
            handle.Region = new Region(shape);
        }

        /// <summary>
        ///     通过API函数来给窗体切角
        /// </summary>
        /// <param name="form"></param>
        /// <param name="rgnRadius"></param>
        public static void SetFormRoundRectRgn(Form form, int rgnRadius)
        {
            var hRgn = 0;
            hRgn = CreateRoundRectRgn(0, 0, form.Width + 1, form.Height + 1, rgnRadius, rgnRadius);
            SetWindowRgn(form.Handle, hRgn, true);
            DeleteObject(hRgn);
        }

        //此处需要把所需要的API函数引用到类NativeMethods中，引用的时候注意添加System.Runtime.InteropServices 命名空间：        
        [DllImport("gdi32.dll")]
        public static extern int CreateRoundRectRgn(int x1, int y1, int x2, int y2, int x3, int y3);

        [DllImport("user32.dll")]
        public static extern int SetWindowRgn(IntPtr hwnd, int hRgn, bool bRedraw);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject", CharSet = CharSet.Ansi)]
        public static extern int DeleteObject(int hObject);

        public static Color GetColor(Color colorBase, int a, int r, int g, int b)
        {
            int a0 = colorBase.A;
            int r0 = colorBase.R;
            int g0 = colorBase.G;
            int b0 = colorBase.B;

            if (a + a0 > 255)
            {
                a = 255;
            }
            else
            {
                a = Math.Max(0, a + a0);
            }
            if (r + r0 > 255)
            {
                r = 255;
            }
            else
            {
                r = Math.Max(0, r + r0);
            }
            if (g + g0 > 255)
            {
                g = 255;
            }
            else
            {
                g = Math.Max(0, g + g0);
            }
            if (b + b0 > 255)
            {
                b = 255;
            }
            else
            {
                b = Math.Max(0, b + b0);
            }

            return Color.FromArgb(a, r, g, b);
        }

        #endregion
    }

    public class RenderHelper
    {
        public static void RendererBackground(Graphics g, Rectangle rect, Bitmap backgroundImage, bool method)
        {
            if (!method)
            {
                g.DrawImage(backgroundImage, new Rectangle(rect.X, rect.Y, 5, rect.Height), 0, 0, 5, backgroundImage.Height, GraphicsUnit.Pixel);
                g.DrawImage(backgroundImage, new Rectangle(rect.X + 5, rect.Y, rect.Width - 10, rect.Height), 5, 0, backgroundImage.Width - 10, backgroundImage.Height, GraphicsUnit.Pixel);
                g.DrawImage(backgroundImage, new Rectangle((rect.X + rect.Width) - 5, rect.Y, 5, rect.Height), backgroundImage.Width - 5, 0, 5, backgroundImage.Height, GraphicsUnit.Pixel);
            }
            else
            {
                RenderFormBorder(backgroundImage, 5, g, rect);
            }
        }

        /// <summary>
        /// 九宫格绘图
        /// </summary>
        /// <param name="borderImg"></param>
        /// <param name="cut"></param>
        /// <param name="g"></param>
        /// <param name="Rect"></param>
        public static void RenderFormBorder(Bitmap borderImg, int cut, Graphics g, Rectangle Rect)
        {
            if (borderImg == null || Rect == Rectangle.Empty) return;
            try
            {
                cut = cut <= 0 ? 1 : cut;
                var h = Rect.Height;
                var w = Rect.Width;
                var imgh = borderImg.Height;
                var imgw = borderImg.Width;

                //左上角
                g.DrawImage(borderImg, new Rectangle(0, 0, cut, cut), new Rectangle(0, 0, cut, cut), GraphicsUnit.Pixel);
                //上边框
                g.DrawImage(borderImg, new Rectangle(cut, 0, w - cut * 2, cut), new Rectangle(cut, 0, imgw - cut * 2, cut),GraphicsUnit.Pixel);
                //右上角
                g.DrawImage(borderImg, new Rectangle(w - cut, 0, cut, cut), new Rectangle(imgw - cut, 0, cut, cut),GraphicsUnit.Pixel);
                //右边框
                g.DrawImage(borderImg, new Rectangle(w - cut, cut, cut, h - cut * 2),new Rectangle(imgw - cut, cut, cut, imgh - cut * 2), GraphicsUnit.Pixel);
                //右下角
                g.DrawImage(borderImg, new Rectangle(w - cut, h - cut, cut, cut),new Rectangle(imgw - cut, imgh - cut, cut, cut), GraphicsUnit.Pixel);
                //下边框
                g.DrawImage(borderImg, new Rectangle(cut, h - cut, w - cut * 2, cut),new Rectangle(cut, imgh - cut, imgw - cut * 2, cut), GraphicsUnit.Pixel);
                //左下角
                g.DrawImage(borderImg, new Rectangle(0, h - cut, cut, cut), new Rectangle(0, imgh - cut, cut, cut),GraphicsUnit.Pixel);
                //左边框
                g.DrawImage(borderImg, new Rectangle(0, cut, cut, h - cut * 2), new Rectangle(0, cut, cut, imgh - cut * 2),GraphicsUnit.Pixel);
                //中间
                g.DrawImage(borderImg, new Rectangle(cut, cut, w - cut * 2, h - cut * 2), new Rectangle(cut, cut, imgw - cut * 2, imgh - cut * 2), GraphicsUnit.Pixel);
                borderImg.Dispose();
            }
            catch
            {
                // MessageBox.Show("搞毛啊，没有找到这张图啊，大哥，仔细点啊");
            }
        }

        private static Color GetColor(Color colorBase, int a, int r, int g, int b)
        {
            int num = colorBase.A;
            int num2 = colorBase.R;
            int num3 = colorBase.G;
            int num4 = colorBase.B;
            a = (a + num) > 0xff ? 0xff : Math.Max(0, a + num);
            r = (r + num2) > 0xff ? 0xff : Math.Max(0, r + num2);
            g = (g + num3) > 0xff ? 0xff : Math.Max(0, g + num3);
            b = (b + num4) > 0xff ? 0xff : Math.Max(0, b + num4);
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

    public static class BitmapHelper
    {
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static unsafe Color GetImageAverageColor(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                throw new ArgumentNullException("bitmap");
            }
            var width = bitmap.Width;
            var height = bitmap.Height;
            var rect = new Rectangle(0, 0, width, height);
            try
            {
                var bitmapdata = bitmap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                var numPtr = (byte*)bitmapdata.Scan0;
                var num3 = bitmapdata.Stride - (bitmapdata.Width * 4);
                var num4 = width * height;
                var num5 = 0;
                var red = 0;
                var green = 0;
                var blue = 0;
                for (var i = 0; i < height; i++)
                {
                    for (var j = 0; j < width; j++)
                    {
                        numPtr++;
                        blue += numPtr[0];
                        numPtr++;
                        green += numPtr[0];
                        numPtr++;
                        red += numPtr[0];
                        numPtr++;
                        num5 += numPtr[0];
                    }
                    numPtr += num3;
                }
                bitmap.UnlockBits(bitmapdata);
                num5 /= num4;
                red /= num4;
                green /= num4;
                blue /= num4;
                return Color.FromArgb(0xff, red, green, blue);
            }
            catch
            {
                return Color.FromArgb(0x7f, 0x7f, 0x7f);
            }
        }
    }

    public class ImageDrawRect
    {
        public static ContentAlignment anyBottom;
        public static ContentAlignment anyCenter;
        public static ContentAlignment anyMiddle;
        public static ContentAlignment anyRight;
        public static ContentAlignment anyTop;

        static ImageDrawRect()
        {
            anyRight = ContentAlignment.BottomRight | ContentAlignment.MiddleRight | ContentAlignment.TopRight;
            anyTop = ContentAlignment.TopRight | ContentAlignment.TopCenter | ContentAlignment.TopLeft;
            anyBottom = ContentAlignment.BottomRight | ContentAlignment.BottomCenter | ContentAlignment.BottomLeft;
            anyCenter = ContentAlignment.BottomCenter | ContentAlignment.MiddleCenter | ContentAlignment.TopCenter;
            anyMiddle = ContentAlignment.MiddleRight | ContentAlignment.MiddleCenter | ContentAlignment.MiddleLeft;
        }

        public static void DrawRect(Graphics g, Bitmap img, Rectangle r, int index, int Totalindex)
        {
            if (img != null)
            {
                var width = img.Width / Totalindex;
                var height = img.Height;
                var x = (index - 1) * width;
                var left = r.Left;
                var top = r.Top;
                var srcRect = new Rectangle(x, 0, width, height);
                var destRect = new Rectangle(left, top, r.Width, r.Height);
                g.DrawImage(img, destRect, srcRect, GraphicsUnit.Pixel);
            }
        }

        public static void DrawRect(Graphics g, Bitmap img, Rectangle r, Rectangle lr, int index, int Totalindex)
        {
            if (img != null)
            {
                Rectangle rectangle;
                Rectangle rectangle2;
                var x = ((index - 1) * img.Width) / Totalindex;
                var y = 0;
                var left = r.Left;
                var top = r.Top;
                if ((r.Height > img.Height) && (r.Width <= (img.Width / Totalindex)))
                {
                    rectangle = new Rectangle(x, y, img.Width / Totalindex, lr.Top);
                    rectangle2 = new Rectangle(left, top, r.Width, lr.Top);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                    rectangle = new Rectangle(x, y + lr.Top, img.Width / Totalindex, (img.Height - lr.Top) - lr.Bottom);
                    rectangle2 = new Rectangle(left, top + lr.Top, r.Width, (r.Height - lr.Top) - lr.Bottom);
                    if ((lr.Top + lr.Bottom) == 0)
                    {
                        rectangle.Height--;
                    }
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                    rectangle = new Rectangle(x, (y + img.Height) - lr.Bottom, img.Width / Totalindex, lr.Bottom);
                    rectangle2 = new Rectangle(left, (top + r.Height) - lr.Bottom, r.Width, lr.Bottom);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                }
                else if ((r.Height <= img.Height) && (r.Width > (img.Width / Totalindex)))
                {
                    rectangle = new Rectangle(x, y, lr.Left, img.Height);
                    rectangle2 = new Rectangle(left, top, lr.Left, r.Height);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                    rectangle = new Rectangle(x + lr.Left, y, ((img.Width / Totalindex) - lr.Left) - lr.Right, img.Height);
                    rectangle2 = new Rectangle(left + lr.Left, top, (r.Width - lr.Left) - lr.Right, r.Height);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                    rectangle = new Rectangle((x + (img.Width / Totalindex)) - lr.Right, y, lr.Right, img.Height);
                    rectangle2 = new Rectangle((left + r.Width) - lr.Right, top, lr.Right, r.Height);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                }
                else if ((r.Height <= img.Height) && (r.Width <= (img.Width / Totalindex)))
                {
                    rectangle = new Rectangle(((index - 1) * img.Width) / Totalindex, 0, img.Width / Totalindex,
                        img.Height - 1);
                    g.DrawImage(img, new Rectangle(left, top, r.Width, r.Height), rectangle, GraphicsUnit.Pixel);
                }
                else if ((r.Height > img.Height) && (r.Width > (img.Width / Totalindex)))
                {
                    rectangle = new Rectangle(x, y, lr.Left, lr.Top);
                    rectangle2 = new Rectangle(left, top, lr.Left, lr.Top);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                    rectangle = new Rectangle(x, (y + img.Height) - lr.Bottom, lr.Left, lr.Bottom);
                    rectangle2 = new Rectangle(left, (top + r.Height) - lr.Bottom, lr.Left, lr.Bottom);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                    rectangle = new Rectangle(x, y + lr.Top, lr.Left, (img.Height - lr.Top) - lr.Bottom);
                    rectangle2 = new Rectangle(left, top + lr.Top, lr.Left, (r.Height - lr.Top) - lr.Bottom);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                    rectangle = new Rectangle(x + lr.Left, y, ((img.Width / Totalindex) - lr.Left) - lr.Right, lr.Top);
                    rectangle2 = new Rectangle(left + lr.Left, top, (r.Width - lr.Left) - lr.Right, lr.Top);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                    rectangle = new Rectangle((x + (img.Width / Totalindex)) - lr.Right, y, lr.Right, lr.Top);
                    rectangle2 = new Rectangle((left + r.Width) - lr.Right, top, lr.Right, lr.Top);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                    rectangle = new Rectangle((x + (img.Width / Totalindex)) - lr.Right, y + lr.Top, lr.Right,
                        (img.Height - lr.Top) - lr.Bottom);
                    rectangle2 = new Rectangle((left + r.Width) - lr.Right, top + lr.Top, lr.Right,
                        (r.Height - lr.Top) - lr.Bottom);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                    rectangle = new Rectangle((x + (img.Width / Totalindex)) - lr.Right, (y + img.Height) - lr.Bottom,
                        lr.Right, lr.Bottom);
                    rectangle2 = new Rectangle((left + r.Width) - lr.Right, (top + r.Height) - lr.Bottom, lr.Right,
                        lr.Bottom);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                    rectangle = new Rectangle(x + lr.Left, (y + img.Height) - lr.Bottom,
                        ((img.Width / Totalindex) - lr.Left) - lr.Right, lr.Bottom);
                    rectangle2 = new Rectangle(left + lr.Left, (top + r.Height) - lr.Bottom,
                        (r.Width - lr.Left) - lr.Right, lr.Bottom);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                    rectangle = new Rectangle(x + lr.Left, y + lr.Top, ((img.Width / Totalindex) - lr.Left) - lr.Right,
                        (img.Height - lr.Top) - lr.Bottom);
                    rectangle2 = new Rectangle(left + lr.Left, top + lr.Top, (r.Width - lr.Left) - lr.Right,
                        (r.Height - lr.Top) - lr.Bottom);
                    g.DrawImage(img, rectangle2, rectangle, GraphicsUnit.Pixel);
                }
            }
        }

        public static Rectangle HAlignWithin(System.Drawing.Size alignThis, Rectangle withinThis, ContentAlignment align)
        {
            if ((align & anyRight) != 0)
            {
                withinThis.X += withinThis.Width - alignThis.Width;
            }
            else if ((align & anyCenter) != 0)
            {
                withinThis.X += ((withinThis.Width - alignThis.Width) + 1) / 2;
            }
            withinThis.Width = alignThis.Width;
            return withinThis;
        }

        public static Rectangle VAlignWithin(System.Drawing.Size alignThis, Rectangle withinThis, ContentAlignment align)
        {
            if ((align & anyBottom) != 0)
            {
                withinThis.Y += withinThis.Height - alignThis.Height;
            }
            else if ((align & anyMiddle) != 0)
            {
                withinThis.Y += ((withinThis.Height - alignThis.Height) + 1) / 2;
            }
            withinThis.Height = alignThis.Height;
            return withinThis;
        }
    }

    public class ColorHelper
    {
        /// <summary>
        ///     颜色加深变亮,加深：correctionFactor<0,变亮： correctionFactor>0
        /// </summary>
        /// <param name="color"></param>
        /// <param name="correctionFactor">取值在-1和1之间</param>
        /// <returns></returns>
        public static Color ChangeColor(Color color, float correctionFactor)
        {
            float red = color.R;
            float green = color.G;
            float blue = color.B;
            if (correctionFactor < 0)
            {
                correctionFactor = 1 + correctionFactor;
                red *= correctionFactor;
                green *= correctionFactor;
                blue *= correctionFactor;
            }
            else
            {
                red = (255 - red) * correctionFactor + red;
                green = (255 - green) * correctionFactor + green;
                blue = (255 - blue) * correctionFactor + blue;
            }
            if (red < 0) red = 0;
            if (red > 255) red = 255;
            if (green < 0) green = 0;
            if (green > 255) green = 255;
            if (blue < 0) blue = 0;
            if (blue > 255) blue = 255;
            return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
        }
    }
}