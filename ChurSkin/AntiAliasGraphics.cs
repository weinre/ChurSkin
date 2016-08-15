using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    public class AntiAliasGraphics : IDisposable
    {
        private readonly Graphics _graphics;
        private readonly SmoothingMode _oldMode;

        public AntiAliasGraphics(Graphics graphics)
        {
            _graphics = graphics;
            _oldMode = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
        }

        public void Dispose()
        {
            _graphics.SmoothingMode = _oldMode;
        }
    }
}