namespace System.Windows.Forms.Animations
{
    using System;
    using System.Drawing;

    public class RotateZoomEffect : IEffects, IDisposable
    {
        private Bitmap bitmap_0;
        private Bitmap bitmap_1;
        private Bitmap bitmap_2;
        private bool bool_0;
        private bool bool_1;
        private float float_0;

        public RotateZoomEffect()
        {
            //Class4.OCal7gSz1iJFF();
            this.bool_0 = true;
            this.float_0 = 15f;
        }

        public void Dispose()
        {
            if (this.bitmap_0 != null)
            {
                this.bitmap_0.Dispose();
                this.bitmap_0 = null;
            }
            if (this.bitmap_1 != null)
            {
                this.bitmap_1.Dispose();
                this.bitmap_1 = null;
            }
            GC.SuppressFinalize(this);
        }

        public Bitmap DoEffect()
        {
            if (this.bitmap_1 == null)
            {
                if (this.bitmap_2.Width > this.bitmap_2.Height)
                {
                    this.bitmap_0 = new Bitmap(this.bitmap_2.Width, this.bitmap_2.Width);
                    this.bitmap_1 = new Bitmap(this.bitmap_0.Width, this.bitmap_0.Width);
                }
                else
                {
                    this.bitmap_0 = new Bitmap(this.bitmap_2.Height, this.bitmap_2.Height);
                    this.bitmap_1 = new Bitmap(this.bitmap_0.Height, this.bitmap_0.Height);
                }
                using (Graphics graphics = Graphics.FromImage(this.bitmap_1))
                {
                    graphics.DrawImage(this.bitmap_2, (int) ((this.bitmap_0.Width - this.bitmap_2.Width) / 2), (int) ((this.bitmap_0.Height - this.bitmap_2.Height) / 2));
                }
            }
            using (Graphics graphics2 = Graphics.FromImage(this.bitmap_0))
            {
                if (((this.float_0 <= 360f) && this.bool_0) || (!this.bool_0 && (this.float_0 > 0f)))
                {
                    this.bool_1 = false;
                    graphics2.Clear(Color.Transparent);
                    graphics2.TranslateTransform((float) (this.bitmap_0.Width / 2), (float) (this.bitmap_0.Height / 2));
                    graphics2.RotateTransform(this.float_0);
                    graphics2.ScaleTransform((this.float_0 / 360f) / 1f, (this.float_0 / 360f) / 1f);
                    graphics2.TranslateTransform((float) (-this.bitmap_0.Width / 2), (float) (-this.bitmap_0.Height / 2));
                    graphics2.DrawImage(this.bitmap_1, 0, 0);
                    graphics2.ResetTransform();
                    graphics2.Save();
                    if (this.bool_0)
                    {
                        this.float_0 += 15f;
                    }
                    else
                    {
                        this.float_0 -= 15f;
                    }
                }
                else
                {
                    this.bool_1 = true;
                    graphics2.Clear(Color.Transparent);
                    graphics2.DrawImage(this.bitmap_1, 0, 0);
                }
            }
            return this.bitmap_0;
        }

        public void Reset()
        {
            if (this.bool_0)
            {
                this.float_0 = 15f;
            }
            else
            {
                this.float_0 = 360f;
            }
            if (this.bitmap_1 != null)
            {
                this.bitmap_1.Dispose();
                this.bitmap_1 = null;
                this.bitmap_0.Dispose();
                this.bitmap_0 = null;
            }
        }

        public bool CanDesc { get { return true; } }

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

        public string Name { get { return "旋转放大"; } }

        public Bitmap Original
        {
            get
            {
                return
              this.bitmap_2;
            }
            set
            {
                this.bitmap_2 = value;
            }
        }
    }
}

