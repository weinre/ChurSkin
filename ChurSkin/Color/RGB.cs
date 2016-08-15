using System.Drawing;

namespace System.Windows.Forms
{
    public class RGB
    {
        public const short BIndex = 0;
        public const short GIndex = 1;
        public const short RIndex = 2;

        public RGB()
        {
        }

        public RGB(Color color)
        {
            R = color.R;
            G = color.G;
            B = color.B;
        }

        public RGB(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public byte B { get; set; }

        public Color Color
        {
            get { return Color.FromArgb(R, G, B); }
            set
            {
                R = value.R;
                G = value.G;
                B = value.B;
            }
        }

        public byte G { get; set; }
        public byte R { get; set; }

        public override string ToString()
        {
            return string.Format("RGB [R={0}, G={1}, B={2}]", R, G, B);
        }
    }
}