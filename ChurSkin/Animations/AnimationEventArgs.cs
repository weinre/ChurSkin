namespace System.Windows.Forms.Animations
{
    using System;
    using System.Drawing;

    public class AnimationEventArgs : EventArgs
    {
        private Bitmap bitmap_0;
        private bool bool_0;
        private int int_0;

        public AnimationEventArgs(Bitmap current, int index, bool final)
        {
            //Class4.OCal7gSz1iJFF();
            this.bitmap_0 = current;
            this.int_0 = index;
            this.bool_0 = final;
        }

        public Bitmap CurrentFrame
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

        public int CurrentIndex
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
    }
}

