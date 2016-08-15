using System.Drawing;

namespace System.Windows.Forms
{
    public class SkinFormCaptionRenderEventArgs : PaintEventArgs
    {
        public SkinFormCaptionRenderEventArgs(CForm skinForm, Graphics g, Rectangle clipRect, bool active)
            : base(g, clipRect)
        {
            SkinForm = skinForm;
            Active = active;
        }

        public bool Active { get; private set; }
        public CForm SkinForm { get; private set; }
    }
}