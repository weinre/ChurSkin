namespace System.Windows.Forms.Animations
{
    using System;
    using System.Drawing;

    public class LevelScrollEffect : IEffects, IDisposable
    {
        private Bitmap bitmap_0;
        private Bitmap bitmap_1;
        private bool bool_0;
        private int int_0;

        public LevelScrollEffect()
        {
            //Class4.OCal7gSz1iJFF();
        }

        public void Dispose()
        {
            if (this.bitmap_0 != null)
            {
                this.bitmap_0.Dispose();
            }
            GC.SuppressFinalize(this);
        }

        public Bitmap DoEffect()
        {
            if ((this.bitmap_0 == null) || (this.bitmap_0.Size != this.bitmap_1.Size))
            {
                this.bitmap_0 = new Bitmap(this.bitmap_1.Width, this.bitmap_1.Height);
            }
            using (Graphics graphics = Graphics.FromImage(this.bitmap_0))
            {
                if (this.int_0 == 0)
                {
                    graphics.Clear(Color.Transparent);
                }
                if (this.int_0 <= Math.Ceiling((double) ((this.bitmap_0.Width - 100f) / 10f)))
                {
                    this.bool_0 = false;
                    RectangleF destRect = new RectangleF(this.int_0 * 10f, 0f, 10f, (float) this.bitmap_0.Height);
                    graphics.DrawImage(this.bitmap_1, destRect, destRect, GraphicsUnit.Pixel);
                    RectangleF srcRect = new RectangleF((this.int_0 + 1) * 10f, 0f, this.bitmap_0.Width - (this.int_0 * 10f), (float) this.bitmap_0.Height);
                    RectangleF ef3 = new RectangleF((this.int_0 + 1) * 10f, 0f, 100f, (float) this.bitmap_0.Height);
                    graphics.DrawImage(this.bitmap_1, ef3, srcRect, GraphicsUnit.Pixel);
                    this.int_0++;
                }
                else
                {
                    graphics.Clear(Color.Transparent);
                    graphics.DrawImage(this.bitmap_1, 0, 0);
                    this.bool_0 = true;
                }
            }
            return this.bitmap_0;
        }

        public void Reset()
        {
            this.int_0 = 0;
        }

        public bool CanDesc { get { return false; } }

        public bool IsAsc
        {
            set
            {
            }
        }

        public bool IsFinal
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

        public string Name { get { return "水平卷轴"; } }

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

