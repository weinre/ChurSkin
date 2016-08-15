using System.Drawing;

namespace System.Windows.Forms
{
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