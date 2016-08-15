using System.Drawing;

namespace System.Windows.Forms
{
    public class MessageBoxArgs
    {
        public MessageBoxArgs()
        {
        }

        public MessageBoxArgs(IWin32Window owner, string text, string caption, MessageBoxButtons buttons, Bitmap icon,
            MessageBoxDefaultButton defaultButton)
        {
            Owner = owner;
            Text = text;
            Caption = caption;
            Buttons = buttons;
            Icon = icon;
            DefaultButton = defaultButton;
        }

        public MessageBoxButtons Buttons { get; set; }
        public string Caption { get; set; }
        public MessageBoxDefaultButton DefaultButton { get; set; }
        public Bitmap Icon { get; set; }
        public IWin32Window Owner { get; set; }
        public string Text { get; set; }
    }
}