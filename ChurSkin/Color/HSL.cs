namespace System.Windows.Forms
{
    public class HSL
    {
        private int _hue;
        private double _luminance;
        private double _saturation;

        public HSL()
        {
        }

        public HSL(int hue, double saturation, double luminance)
        {
            Hue = hue;
            Saturation = saturation;
            Luminance = luminance;
        }

        public int Hue
        {
            get { return _hue; }
            set
            {
                if (value < 0)
                {
                    _hue = 0;
                }
                else if (value <= 360)
                {
                    _hue = value;
                }
                else
                {
                    _hue = value%360;
                }
            }
        }

        public double Luminance
        {
            get { return _luminance; }
            set
            {
                if (value < 0.0)
                {
                    _luminance = 0.0;
                }
                else
                {
                    _luminance = Math.Min(value, 1.0);
                }
            }
        }

        public double Saturation
        {
            get { return _saturation; }
            set
            {
                if (value < 0.0)
                {
                    _saturation = 0.0;
                }
                else
                {
                    _saturation = Math.Min(value, 1.0);
                }
            }
        }

        public override string ToString()
        {
            return string.Format("HSL [H={0}, S={1}, L={2}]", _hue, _saturation, _luminance);
        }
    }
}