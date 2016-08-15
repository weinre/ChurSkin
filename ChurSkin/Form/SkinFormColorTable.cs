using System.Drawing;

namespace System.Windows.Forms
{
    public class SkinFormColorTable
    {
        public static readonly Color _back;
        public static readonly Color _border;
        public static readonly Color _captionActive;
        public static readonly Color _captionDeactive;
        public static readonly Color _captionText;
        public static Color _controlBoxActive;
        public static Color _controlBoxDeactive;
        public static readonly Color _controlBoxHover;
        public static readonly Color _controlBoxInnerBorder;
        public static readonly Color _controlBoxPressed;
        private static readonly Color _controlCloseBoxHover;
        private static readonly Color _controlCloseBoxPressed;
        public static readonly Color _innerBorder;

        static SkinFormColorTable()
        {
            _captionActive = Color.Transparent;
            _captionDeactive = Color.Transparent;
            _captionText = Color.Black;
            _border = Color.FromArgb(100, 0, 0, 0);
            _innerBorder = Color.FromArgb(100, 250, 250, 250);
            _back = Color.FromArgb(0x80, 0xd0, 0xff);
            _controlBoxActive = Color.FromArgb(0x33, 0x99, 0xcc);
            _controlBoxDeactive = Color.FromArgb(0x58, 0xac, 0xda);
            _controlBoxHover = Color.FromArgb(150, 0x27, 0xaf, 0xe7);
            _controlBoxPressed = Color.FromArgb(150, 0x1d, 0x8e, 190);
            _controlCloseBoxHover = Color.FromArgb(0xd5, 0x42, 0x16);
            _controlCloseBoxPressed = Color.FromArgb(0xab, 0x35, 0x11);
            _controlBoxInnerBorder = Color.FromArgb(0x80, 250, 250, 250);
        }

        public virtual Color Back
        {
            get { return _back; }
        }

        public virtual Color Border
        {
            get { return _border; }
        }

        public virtual Color CaptionActive
        {
            get { return _captionActive; }
        }

        public virtual Color CaptionDeactive
        {
            get { return _captionDeactive; }
        }

        public virtual Color CaptionText
        {
            get { return _captionText; }
        }

        public virtual Color ControlBoxActive
        {
            get { return _controlBoxActive; }
            set { _controlBoxActive = value; }
        }

        public virtual Color ControlBoxDeactive
        {
            get { return _controlBoxDeactive; }
            set { _controlBoxDeactive = value; }
        }

        public virtual Color ControlBoxHover
        {
            get { return _controlBoxHover; }
        }

        public virtual Color ControlBoxInnerBorder
        {
            get { return _controlBoxInnerBorder; }
        }

        public virtual Color ControlBoxPressed
        {
            get { return _controlBoxPressed; }
        }

        public virtual Color ControlCloseBoxHover
        {
            get { return _controlCloseBoxHover; }
        }

        public virtual Color ControlCloseBoxPressed
        {
            get { return _controlCloseBoxPressed; }
        }

        public virtual Color InnerBorder
        {
            get { return _innerBorder; }
        }
    }
}