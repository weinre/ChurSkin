using System.Drawing;

namespace System.Windows.Forms
{
    public sealed class ColorConverterEx
    {
        private static readonly int[] BT907;
        private static readonly int[] RMY;
        private static readonly int[] Y;

        static ColorConverterEx()
        {
            BT907 = new[] {0x84d, 0x1bf2, 0x2d1, 0x2710};
            RMY = new[] {500, 0x1a3, 0x51, 0x3e8};
            Y = new[] {0x12b, 0x24b, 0x72, 0x3e8};
        }

        private ColorConverterEx()
        {
        }

        private static byte GetGray(RGB rgb, int[] coefficient)
        {
            return (byte) ((((rgb.R*coefficient[0]) + (rgb.G*coefficient[1])) + (rgb.B*coefficient[2]))/coefficient[3]);
        }

        public static RGB HslToRgb(HSL hsl)
        {
            var rgb = new RGB();
            HslToRgb(hsl, rgb);
            return rgb;
        }

        public static void HslToRgb(HSL hsl, RGB rgb)
        {
            if (hsl.Saturation == 0.0)
            {
                rgb.R = rgb.G = rgb.B = (byte) (hsl.Luminance*255.0);
            }
            else
            {
                var vH = hsl.Hue/360.0;
                var num = (hsl.Luminance < 0.5)
                    ? (hsl.Luminance*(1.0 + hsl.Saturation))
                    : ((hsl.Luminance + hsl.Saturation) - (hsl.Luminance*hsl.Saturation));
                var num2 = (2.0*hsl.Luminance) - num;
                rgb.R = (byte) (255.0*HueToRGB(num2, num, vH + 0.33333333333333331));
                rgb.G = (byte) (255.0*HueToRGB(num2, num, vH));
                rgb.B = (byte) (255.0*HueToRGB(num2, num, vH - 0.33333333333333331));
            }
        }

        private static double HueToRGB(double v1, double v2, double vH)
        {
            if (vH < 0.0)
            {
                vH++;
            }
            if (vH > 1.0)
            {
                vH--;
            }
            if ((6.0*vH) < 1.0)
            {
                return (v1 + (((v2 - v1)*6.0)*vH));
            }
            if ((2.0*vH) < 1.0)
            {
                return v2;
            }
            if ((3.0*vH) < 2.0)
            {
                return (v1 + (((v2 - v1)*(0.66666666666666663 - vH))*6.0));
            }
            return v1;
        }

        public static RGB RgbToGray(RGB source)
        {
            var dest = new RGB();
            RgbToGray(source, dest);
            return dest;
        }

        public static RGB RgbToGray(RGB source, GrayscaleStyle style)
        {
            var dest = new RGB();
            RgbToGray(source, dest, style);
            return dest;
        }

        public static void RgbToGray(RGB source, RGB dest)
        {
            RgbToGray(source, dest, GrayscaleStyle.BT907);
        }

        public static void RgbToGray(RGB source, RGB dest, GrayscaleStyle style)
        {
            byte gray = 0x7f;
            switch (style)
            {
                case GrayscaleStyle.BT907:
                    gray = GetGray(source, BT907);
                    break;

                case GrayscaleStyle.RMY:
                    gray = GetGray(source, RMY);
                    break;

                case GrayscaleStyle.Y:
                    gray = GetGray(source, Y);
                    break;
            }
            dest.R = dest.G = dest.B = gray;
        }

        public static HSL RgbToHsl(RGB rgb)
        {
            var hsl = new HSL();
            RgbToHsl(rgb, hsl);
            return hsl;
        }

        public static void RgbToHsl(RGB rgb, HSL hsl)
        {
            var num = rgb.R/255.0;
            var num2 = rgb.G/255.0;
            var num3 = rgb.G/255.0;
            var num4 = Math.Min(Math.Min(num, num2), num3);
            var num5 = Math.Max(Math.Max(num, num2), num3);
            var num6 = num5 - num4;
            hsl.Luminance = (num5 + num4)/2.0;
            if (num6 == 0.0)
            {
                hsl.Hue = 0;
                hsl.Saturation = 0.0;
            }
            else
            {
                double num7;
                hsl.Saturation = (hsl.Luminance < 0.5) ? (num6/(num5 + num4)) : (num6/((2.0 - num5) - num4));
                var num8 = (((num5 - num)/6.0) + (num6/2.0))/num6;
                var num10 = (((num5 - num2)/6.0) + (num6/2.0))/num6;
                var num9 = (((num5 - num3)/6.0) + (num6/2.0))/num6;
                if (num == num5)
                {
                    num7 = num9 - num10;
                }
                else if (num2 == num5)
                {
                    num7 = (0.33333333333333331 + num8) - num9;
                }
                else
                {
                    num7 = (0.66666666666666663 + num10) - num8;
                }
                if (num7 < 0.0)
                {
                    num7++;
                }
                if (num7 > 1.0)
                {
                    num7--;
                }
                hsl.Hue = (int) (num7*360.0);
            }
        }

        public static HSL smethod_0(Color color)
        {
            int num11;
            double num13;
            var num = color.R/255.0;
            var num2 = color.G/255.0;
            var num3 = color.B/255.0;
            var num4 = Math.Min(Math.Min(num, num2), num3);
            var num5 = Math.Max(Math.Max(num, num2), num3);
            var num6 = num5 - num4;
            var luminance = (num5 + num4)/2.0;
            if (num6 == 0.0)
            {
                num11 = 0;
                num13 = 0.0;
            }
            else
            {
                double num10;
                num13 = (luminance < 0.5) ? (num6/(num5 + num4)) : (num6/((2.0 - num5) - num4));
                var num12 = (((num5 - num)/6.0) + (num6/2.0))/num6;
                var num9 = (((num5 - num2)/6.0) + (num6/2.0))/num6;
                var num8 = (((num5 - num3)/6.0) + (num6/2.0))/num6;
                if (num == num5)
                {
                    num10 = num8 - num9;
                }
                else if (num2 == num5)
                {
                    num10 = (0.33333333333333331 + num12) - num8;
                }
                else
                {
                    num10 = (0.66666666666666663 + num9) - num12;
                }
                if (num10 < 0.0)
                {
                    num10++;
                }
                if (num10 > 1.0)
                {
                    num10--;
                }
                num11 = (int) (num10*360.0);
            }
            return new HSL(num11, num13, luminance);
        }

        public static Color smethod_1(HSL hsl)
        {
            byte num;
            byte num2;
            byte num3;
            if (hsl.Saturation == 0.0)
            {
                num3 = num2 = num = (byte) (hsl.Luminance*255.0);
            }
            else
            {
                var vH = hsl.Hue/360.0;
                var num5 = (hsl.Luminance < 0.5)
                    ? (hsl.Luminance*(1.0 + hsl.Saturation))
                    : ((hsl.Luminance + hsl.Saturation) - (hsl.Luminance*hsl.Saturation));
                var num6 = (2.0*hsl.Luminance) - num5;
                num3 = (byte) (255.0*HueToRGB(num6, num5, vH + 0.33333333333333331));
                num2 = (byte) (255.0*HueToRGB(num6, num5, vH));
                num = (byte) (255.0*HueToRGB(num6, num5, vH - 0.33333333333333331));
            }
            return Color.FromArgb(num3, num2, num);
        }
    }
}