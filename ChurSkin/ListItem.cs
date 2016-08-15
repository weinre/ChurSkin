using System.Drawing;

namespace System.Windows.Forms
{
    [Serializable]
    public class ListItem
    {
        private int _index;

        public int Index
        {
            get { return _index; }
            set
            {
                value += value;
                _index = value;
            }
        }

        public Rectangle Bounds { get; set; }
        public int Alpha { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
    }
}