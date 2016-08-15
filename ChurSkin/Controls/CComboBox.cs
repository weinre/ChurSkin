using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms
{
    [ToolboxBitmap(typeof(Button)), DefaultEvent("SelectedIndexChanged")]
    public partial class CComboBox : CButton
    {
       /// private string[] menuItems;

        public event EventHandler SelectedIndexChanged;
        private CToolStripDropDown _resizeHost;
        private CListBox listbox;
        public CComboBox()
        {
            listbox = new CListBox(this);

            Click += delegate (object sender, EventArgs e)
            {
                _resizeHost.Show(this);
            };
            listbox.Round = new Padding(1);
            listbox.ItemClick += (a, b) =>
            {
                _resizeHost.Close();
                if (this.Text != listbox.SelectItem.Text)
                {
                    Text = listbox.SelectItem.Text;
                    if (SelectedIndexChanged != null)
                    {
                        SelectedIndexChanged.Invoke(this, EventArgs.Empty);
                    }
                }
            };
            _resizeHost = new CToolStripDropDown(listbox);
            
        }

         /// <summary>
        /// 当前选中索引
        /// </summary>
        [Browsable(false)]
        public int SelectedIndex
        {
            set { listbox.SelectedIndex = value; }
            get { return listbox.SelectedIndex; }
        }
        /// <summary>
        /// 当前选中文字
        /// </summary>
        [Browsable(false)]
        public string SelectedText
        {
            get { return this.Text; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("内部列表控件")]
        public CListBox ItemList
        {
            get
            {
                //if (this.listbox == null)
                //{
                //    this.listbox = new CListBox();
                //    this.listbox.ItemClick += new EventHandler<ItemClickEventArgs>(this.method_15);
                //    listbox.ItemClick += (a, b) =>
                //    {
                //        _resizeHost.Close();
                //        Text = listbox.SelectItem.Text;
                //    };
                //}
                return this.listbox;
            }
        }

        public string[] Items
        {
            get { return listbox.Items; }
            set { listbox.Items = value; }
        }

        public void Clear()
        {
            listbox.Clear();
        }
        public void Add(object item)
        {
            this.listbox.Add(item); 
        }

        public void AddRange(object[] item)
        {
            listbox.AddRange(item); 
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var bitmap = Properties.Resources.箭头)
            {
                e.Graphics.DrawImage(bitmap, new Rectangle(Width - bitmap.Width - 5, (Height - bitmap.Height) / 2 + 1, bitmap.Width, bitmap.Height),
                    new Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
            }
        }
    }
}
