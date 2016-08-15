using System.Drawing;

namespace System.Windows.Forms
{
    public class BackEventArgs
    {
        public BackEventArgs(Image beforeBack, Image afterBack)
        {
            BeforeBack = beforeBack;
            AfterBack = afterBack;
        }

        public Image AfterBack { get; private set; }
        public Image BeforeBack { get; private set; }
    }
}