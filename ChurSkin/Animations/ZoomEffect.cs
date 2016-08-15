namespace System.Windows.Forms.Animations
{
    using System;
    using System.Drawing;

    public class ZoomEffect : IEffects, IDisposable
    {
        private Bitmap bitmap_0;
        private Bitmap bitmap_1;
        private bool bool_0;
        private bool bool_1;
        private int int_0;
        private int int_1;

        public ZoomEffect()
        {
            //Class4.OCal7gSz1iJFF();
            this.int_0 = 30;
            this.bool_0 = true;
            this.bool_1 = true;
        }

        public void Dispose()
        {
            if (this.bitmap_0 != null)
            {
                this.bitmap_0.Dispose();
                this.bitmap_0 = null;
            }
            GC.SuppressFinalize(this);
        }

        public Bitmap DoEffect()
        {
            if (this.bitmap_1 != null)
            {
                float num;
                if ((this.bitmap_0 == null) || (this.bitmap_0.Size != this.bitmap_1.Size))
                {
                    this.bitmap_0 = new Bitmap(this.bitmap_1.Width, this.bitmap_1.Height);
                }
                if (this.bool_0)
                {
                    num = (float) ((1.0 * this.int_1) / ((double) (this.int_0 - 1)));
                }
                else
                {
                    num = 1f - ((float) ((1.0 * this.int_1) / ((double) (this.int_0 - 1))));
                }
                using (Graphics graphics = Graphics.FromImage(this.bitmap_0))
                {
                    graphics.Clear(Color.Transparent);
                    Size size = this.bitmap_1.Size;
                    Size size2 = new Size((int) ((1.0 * size.Width) * num), (int) ((1.0 * size.Height) * num));
                    graphics.DrawImage(this.bitmap_1, new RectangleF((float) ((size.Width - size2.Width) / 2), (float) ((size.Height - size2.Height) / 2), (float) size2.Width, (float) size2.Height));
                }
                this.int_1++;
                if (this.int_1 >= this.int_0)
                {
                    this.bool_1 = true;
                    this.int_1 = this.int_0 - 1;
                }
                else
                {
                    this.bool_1 = false;
                }
            }
            return this.bitmap_0;
        }

        public void Reset()
        {
            this.int_1 = 0;
        }

        public bool CanDesc { get { return true; } }

        public bool IsAsc
        {
            get
            {
                return
              this.bool_0;
            }
            set
            {
                this.bool_0 = value;
            }
        }

        public bool IsFinal { get { return this.bool_1; } }

        public string Name { get { return "缩放特效"; } }

        public Bitmap Original
        {
            get
            {
                return
              this.bitmap_1;
            }
            set
            {
                this.bitmap_1 = value;
            }
        }
    }
}

