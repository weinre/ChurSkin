namespace System.Windows.Forms.Animations
{
    using System;
    using System.Drawing;

    public interface IEffects : IDisposable
    {
        void Dispose();
        Bitmap DoEffect();
        void Reset();

        bool CanDesc { get; }

        bool IsAsc { set; }

        bool IsFinal { get; }

        string Name { get; }

        Bitmap Original { get; set; }
    }
}

