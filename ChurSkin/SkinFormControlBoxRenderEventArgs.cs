using System.Drawing;

namespace System.Windows.Forms
{
    public class SkinFormControlBoxRenderEventArgs : PaintEventArgs
    {
        public SkinFormControlBoxRenderEventArgs(CForm form, Graphics graphics, Rectangle clipRect, bool active,
            ControlBoxStyle controlBoxStyle, ControlBoxState controlBoxState)
            : base(graphics, clipRect)
        {
            Form = form;
            Active = active;
            ControlBoxtate = controlBoxState;
            ControlBoxStyle = controlBoxStyle;
        }

        public bool Active { get; private set; }
        public ControlBoxStyle ControlBoxStyle { get; private set; }
        public ControlBoxState ControlBoxtate { get; private set; }
        public CForm Form { get; private set; }
    }
}