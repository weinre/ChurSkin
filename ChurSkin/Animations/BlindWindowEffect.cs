namespace System.Windows.Forms.Animations
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;

    public class BlindWindowEffect : IEffects, IDisposable
    {
        private Bitmap bitmap_0;
        private Bitmap bitmap_1;
        private bool bool_0;
        private bool bool_1;
        private float float_0;
        private int int_0;

        public BlindWindowEffect()
        {
            //Class4.OCal7gSz1iJFF();
            this.float_0 = 50f;
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
                if (this.int_0 < this.float_0)
                {
                    this.bool_1 = false;
                    using (GraphicsPath path = new GraphicsPath())
                    {
                        using (TextureBrush brush = new TextureBrush(this.bitmap_1))
                        {
                            if (this.bool_0)
                            {
                                for (int i = 0; i < Math.Ceiling((double) (((float) this.bitmap_0.Height) / this.float_0)); i++)
                                {
                                    RectangleF rect = new RectangleF(0f, (this.float_0 * i) + this.int_0, (float) this.bitmap_0.Width, 1f);
                                    path.AddRectangle(rect);
                                }
                            }
                            else
                            {
                                for (int j = 0; j < Math.Ceiling((double) (((float) this.bitmap_0.Height) / this.float_0)); j++)
                                {
                                    RectangleF ef2 = new RectangleF(0f, (this.float_0 * j) + this.int_0, (float) this.bitmap_0.Width, 1f);
                                    path.AddRectangle(ef2);
                                }
                            }
                            graphics.FillPath(brush, path);
                        }
                    }
                    this.int_0++;
                }
                else
                {
                    graphics.Clear(Color.Transparent);
                    graphics.DrawImage(this.bitmap_1, 0, 0);
                    this.bool_1 = true;
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

        public float LineHeight
        {
            get
            {
                return
              this.float_0;
            }
            set
            {
                this.float_0 = value;
            }
        }

        public string Name { get { return "百叶窗"; } }

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

