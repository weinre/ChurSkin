using System.ComponentModel;
using System.Drawing;
using Win32.Consts;

namespace System.Windows.Forms
{
    //[ToolboxBitmap(typeof(TextBox)), Designer(typeof(TextBox))]
    [ToolboxItem(false)]
    public class CTextBoxEx : TextBox
    {
        private string _waterText = string.Empty;

        [Description("水印文字"), Category("自定义属性")]
        public string WaterText
        {
            get { return _waterText; }
            set
            {
                _waterText = value;
                Invalidate();
            }
        }

        private void WmPaintWater(ref Message m)
        {
            using (var g = Graphics.FromHwnd(Handle))
            {
                if (Text.Length == 0 &&
                    !string.IsNullOrEmpty(_waterText) &&
                    !Focused)
                {
                    var flags = TextFormatFlags.EndEllipsis | TextFormatFlags.VerticalCenter;
                    if (RightToLeft == RightToLeft.Yes)
                    {
                        flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
                    }
                    TextRenderer.DrawText(g, _waterText, Share.DefaultFont, ClientRectangle, Color.DarkGray, flags);
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == (int) WindowsMessage.WM_PAINT)
                WmPaintWater(ref m); //绘制水印
        }
    }
}