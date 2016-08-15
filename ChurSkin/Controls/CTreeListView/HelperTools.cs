//  ?Copyright 2002, Pierre ARNAUD, OPaC bright ideas, Switzerland.
//    All rights reserved.

using System.Drawing;

namespace System.Windows.Forms
{
    public class HelperTools
    {
        private HelperTools()
        {
        }

        public static SizeF MeasureDisplayString(Graphics graphics, string text, Font font)
        {
            const int width = 32;

            var bitmap = new Bitmap(width, 1, graphics);
            var size = graphics.MeasureString(text, font);
            var anagra = Graphics.FromImage(bitmap);

            var measured_width = (int) size.Width;

            if (anagra != null)
            {
                anagra.Clear(Color.White);
                anagra.DrawString(text + "|", font, Brushes.Black, width - measured_width, -font.Height/2);

                for (var i = width - 1; i >= 0; i--)
                {
                    measured_width--;
                    if (bitmap.GetPixel(i, 0).R == 0)
                    {
                        break;
                    }
                }
            }

            return new SizeF(measured_width, size.Height);
        }

        public static int MeasureDisplayStringWidth(Graphics graphics, string text, Font font)
        {
            return (int) MeasureDisplayString(graphics, text, font).Width;
        }
    }
}