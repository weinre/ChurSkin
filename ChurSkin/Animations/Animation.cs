namespace System.Windows.Forms.Animations
{
    using System;
    using System.Drawing;
    using System.Threading;
    using System.Windows.Forms;

    public class Animation : IDisposable
    {
        private bool bool_0;
        private IEffects ieffects;
        private int int_0;
        private System.Windows.Forms.Timer timer;

        public event EventHandler<AnimationEventArgs> AnimationEnd;

        public event EventHandler AnimationStart;

        public event EventHandler<AnimationEventArgs> AnimationStarted;

        internal event EventHandler AnimationEnded;

        public event EventHandler<AnimationEventArgs> FrameChanged;

        public Animation()
        {
            //Class4.OCal7gSz1iJFF();
            this.timer = new System.Windows.Forms.Timer();
            this.bool_0 = true;
            this.timer.Interval = 12;
            this.timer.Tick += new EventHandler(this.timer_0_Tick);
        }

        public Animation(IEffects effect)
        {
            //Class4.OCal7gSz1iJFF();
            this.timer = new System.Windows.Forms.Timer();
            this.bool_0 = true;
            this.ieffects = effect;
            this.timer.Interval = 12;
            this.timer.Tick += new EventHandler(this.timer_0_Tick);
        }

        public void Dispose()
        {
            if (this.timer != null)
            {
                this.timer.Dispose();
                this.timer = null;
            }
            if (this.ieffects != null)
            {
                this.ieffects.Dispose();
                this.ieffects = null;
            }
            GC.SuppressFinalize(this);
        }

        protected virtual void OnAnimationEnd(AnimationEventArgs e)
        {
            if (this.AnimationEnd != null)
            {
                this.AnimationEnd(this, e);
            }
        }

        protected virtual void OnAnimationEnded(EventArgs e)
        {
            if (this.AnimationEnded != null)
            {
                this.AnimationEnded(this, e);
            }
        }

        protected virtual void OnAnimationStart(EventArgs e)
        {
            if (this.AnimationStart != null)
            {
                this.AnimationStart(this, e);
            }
        }

        protected virtual void OnAnimationStarted(AnimationEventArgs e)
        {
            if (this.AnimationStarted != null)
            {
                this.AnimationStarted(this, e);
            }
        }

        protected virtual void OnFrameChanged(AnimationEventArgs e)
        {
            if (this.FrameChanged != null)
            {
                this.FrameChanged(this, e);
            }
        }

        public void Start()
        {
            if (this.ieffects != null)
            {
                this.OnAnimationStart(EventArgs.Empty);
                this.ieffects.Reset();
                this.int_0 = 0;
                this.timer.Start();
            }
        }

        public void Stop()
        {
            this.timer.Stop();
        }

        private void timer_0_Tick(object sender, EventArgs e)
        {
            AnimationEventArgs args = new AnimationEventArgs(this.ieffects.DoEffect(), this.int_0, false);
            if (this.ieffects.IsFinal)
            {
                args.IsFinal = true;
            }
            this.OnFrameChanged(args);
            if (this.int_0 == 0)
            {
                this.OnAnimationStarted(args);
            }
            if (this.ieffects.IsFinal)
            {
                this.timer.Stop();
                this.OnAnimationEnd(args);
                this.OnAnimationEnded(args);
            }
            this.int_0++;
        }

        public bool Asc
        {
            get
            {
                return
              this.bool_0;
            }
            set
            {
                if (((this.bool_0 != value) && (this.ieffects != null)) && this.ieffects.CanDesc)
                {
                    this.bool_0 = value;
                    this.ieffects.IsAsc = this.bool_0;
                }
            }
        }

        public IEffects Effect
        {
            get
            {
                return
               this.ieffects;
            }
            set
            {
                if (this.ieffects != value)
                {
                    this.ieffects = value;
                }
            }
        }

        public int Interval
        {
            get
            {
                return
                   this.timer.Interval;
            }
            set
            {
                this.timer.Interval = value;
            }
        }

        public Bitmap Original
        {
            get
            {
                if (this.ieffects != null)
                {
                    return this.ieffects.Original;
                }
                return null;
            }
            set
            {
                if (this.ieffects != null)
                {
                    this.ieffects.Original = value;
                }
            }
        }
    }
}

