using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Permissions;

namespace System.Windows.Forms
{
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
}