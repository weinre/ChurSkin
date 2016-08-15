using System.Drawing;

namespace System.Windows.Forms
{
    public static class CMessageBox
    {
        private static Bitmap GetIcon(MessageBoxIcon icon)
        {
            switch (icon)
            {
                case MessageBoxIcon.Hand:
                    return Properties.Resources.error;
                case MessageBoxIcon.None:
                    return Properties.Resources.ok;

                case MessageBoxIcon.Question:
                    return Properties.Resources.question;

                case MessageBoxIcon.Exclamation:
                    return Properties.Resources.warning;

                case MessageBoxIcon.Asterisk:
                    return Properties.Resources.info;
            }
            return null;
        }

        public static DialogResult Show(string text)
        {
            return Show((IWin32Window)null, text);
        }

        public static DialogResult Show(string text, string caption)
        {
            return Show(null, text, caption, MessageBoxButtons.OK);
        }

        public static DialogResult Show(IWin32Window owner, string text)
        {
            return Show(owner, text, "提示");
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons)
        {
            return Show(null, text, caption, buttons);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption)
        {
            return Show(owner, text, caption, MessageBoxButtons.OK);
        }

        public static DialogResult Error(string text)
        {
            return Show(text, "出现错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static DialogResult OK(string text)
        {
            return Show(text, "温馨提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return Show(null, text, caption, buttons, icon);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons)
        {
            return Show(owner, text, caption, buttons, MessageBoxIcon.None);
        }

        public static DialogResult Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon,
            MessageBoxDefaultButton defaultButton)
        {
            return Show(null, text, caption, buttons, icon, defaultButton);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons,
            MessageBoxIcon icon)
        {
            return Show(owner, text, caption, buttons, icon, MessageBoxDefaultButton.Button1);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons,
            MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
        {
            return Show(owner, text, caption, buttons, GetIcon(icon), defaultButton, icon);
        }

        public static DialogResult Show(IWin32Window owner, string text, string caption, MessageBoxButtons buttons,
            Bitmap icon, MessageBoxDefaultButton defaultButton, MessageBoxIcon beepType)
        {
            //Win32.NativeMethods.MessageBeep((int) beepType);
            var form = new CMessageBoxBase();
            return form.ShowMessageBoxDialog(new MessageBoxArgs(owner, text, caption, buttons, icon, defaultButton));
        }
    }
}