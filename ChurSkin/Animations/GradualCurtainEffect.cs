namespace System.Windows.Forms.Animations
{
    using System;
    using System.Drawing;

    public class GradualCurtainEffect : IEffects, IDisposable
    {
        private Bitmap bitmap_0;
        private bool bool_0;
        private bool bool_1;
        private int int_0;
        private int int_1;
        private int int_2;

        public GradualCurtainEffect()
        {
            //Class4.OCal7gSz1iJFF();
            this.bool_0 = true;
            this.int_0 = 80;
            this.int_2 = 15;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Bitmap DoEffect()
        {
            if (((this.int_1 < (this.bitmap_0.Height - this.int_0)) && this.bool_0) || (!this.bool_0 && (this.int_1 > 0)))
            {
                this.bool_1 = false;
                Bitmap image = (Bitmap) ImageEffects.GradualAlpha(this.bitmap_0, new Rectangle(0, this.int_1, this.bitmap_0.Width, this.int_0), 1);
                using (Graphics graphics = Graphics.FromImage(image))
                {
                    graphics.Clip = new Region(new Rectangle(0, this.int_1 + this.int_0, this.bitmap_0.Width, this.bitmap_0.Height));
                    graphics.Clear(Color.Transparent);
                }
                if (this.bool_0)
                {
                    this.int_1 += this.int_2;
                    return image;
                }
                this.int_1 -= this.int_2;
                return image;
            }
            this.bool_1 = true;
            return this.bitmap_0;
        }

        public void Reset()
        {
            if (this.bool_0)
            {
                this.int_1 = 0;
            }
            else
            {
                this.int_1 = this.bitmap_0.Height - this.int_0;
            }
        }

        public bool CanDesc { get { return true; } }

        public int ChangeHeight
        {
            get
            {
                return
              this.int_2;
            }
            set
            {
                this.int_2 = value;
            }
        }

        public int GradualHeight
        {
            get
            {
                return
              this.int_0;
            }
            set
            {
                this.int_0 = value;
            }
        }

        public bool IsAsc
        {
            get
            {
                return
              this.IsAsc;
            }
            set
            {
                this.bool_0 = value;
            }
        }

        public bool IsFinal
        {
            get
            {
                return
              this.bool_1;
            }
            set
            {
                this.bool_1 = value;
            }
        }

        public string Name { get { return "渐变拉幕"; } }

        public Bitmap Original
        {
            get
            {
                return
              this.bitmap_0;
            }
            set
            {
                this.bitmap_0 = value;
            }
        }
    }
}

