using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace System.Windows.Forms
{
    public class ControlBaseCollection : IList, ICollection, IEnumerable
    {
        private CControlBase _cControlBase;
        private List<CControlBase> list;

        public ControlBaseCollection(CControlBase owner)
        {
            this.list = new List<CControlBase>();
            this._cControlBase = owner;
        }

        public void Add(CControlBase cControl)
        {
            if (cControl == null)
            {
                throw new ArgumentNullException("不能添加Null");
            }
            if (this.IndexOf(cControl) < 0)
            {
                this.list.Add(cControl);
                this._cControlBase.OnControlAdded(new ControlEventArgs(cControl));
            }
        }

        public void AddRange(CControlBase[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                this.Add(items[i]);
            }
        }

        public void Clear()
        {
            int count = this.list.Count;
            for (int i = 0; i < count; i++)
            {
                this.RemoveAt(0);
            }
            this._cControlBase.Invalidate();
        }

        public bool Contains(CControlBase value) =>
            this.list.Contains(value);

        public void CopyTo(Array array, int index)
        {
            this.list.CopyTo((CControlBase[])array, index);
        }

        public IEnumerator GetEnumerator() =>
            this.list.GetEnumerator();

        public int IndexOf(CControlBase value) =>
            this.list.IndexOf(value);

        public void Insert(int index, CControlBase cControl)
        {
            if (!this.list.Contains(cControl))
            {
                this.list.Insert(index, cControl);
                this._cControlBase.OnControlAdded(new ControlEventArgs(cControl));
            }
        }

        public void Remove(CControlBase value)
        {
            this.list.Remove(value);
            this._cControlBase.OnControlRemoved(new ControlEventArgs(value));
        }

        public void RemoveAt(int index)
        {
            CControlBase cControl = this.list[index];
            this.list.RemoveAt(index);
            this._cControlBase.OnControlRemoved(new ControlEventArgs(cControl));
        }

        public void RemoveRange(int start, int count)
        {
            int num2 = ((start + count) > this.list.Count) ? this.list.Count : (start + count);
            for (int i = start; i < num2; i++)
            {
                this.RemoveAt(start);
            }
        }

        int IList.Add(object value)
        {
            if (value is CControlBase)
            {
                this.list.Add((CControlBase)value);
                return this.list.IndexOf((CControlBase)value);
            }
            return -1;
        }

        bool IList.Contains(object value) =>
            this.list.Contains((CControlBase)value);

        int IList.IndexOf(object value) =>
            this.list.IndexOf((CControlBase)value);

        void IList.Insert(int index, object value)
        {
            if (value is CControlBase)
            {
                this.list.Insert(index, (CControlBase)value);
            }
        }

        void IList.Remove(object value)
        {
            if (value is CControlBase)
            {
                this.list.Remove((CControlBase)value);
            }
        }

        public int Count => this.list.Count;

        public CControlBase this[int index] => this.list[index];

        bool ICollection.IsSynchronized => true;

        object ICollection.SyncRoot => this;

        bool IList.IsFixedSize => false;

        bool IList.IsReadOnly => false;

        object IList.this[int index]
        {
            get { return this.list[index]; }
            set { }
        }
    }

    public class CControlCollectionEditor : CollectionEditor
    {
        public CControlCollectionEditor(Type type) : base(type)
        {
            // Class4.OCal7gSz1iJFF(); 
        }

        protected override bool CanSelectMultipleInstances() =>
            true;

        protected override object CreateInstance(Type itemType)
        {
            if (itemType == typeof(CLabel))
            {
                return new CLabel();
            }
            if (itemType == typeof(CControlBase))
            {
                return new CControlBase();
            }
            if (itemType == typeof(CButton))
            {
                return new CButton();
            }
            if (itemType == typeof(CPictureBox))
            {
                return new CPictureBox();
            }
            if (itemType == typeof(CCheckBox))
            {
                return new CCheckBox();
            }
            if (itemType == typeof(CRadioButton))
            {
                return new CRadioButton();
            }
            if (itemType == typeof(CComboBox))
            {
                return new CComboBox();
            }
            if (itemType == typeof(CTextBox))
            {
                return new CTextBox();
            }
            //if (itemType == typeof(CScrollBar))
            //{
            //    return new CScrollBar();
            //}
            return null;
        }

        protected override Type[] CreateNewItemTypes() =>
            new Type[] { typeof(CControlBase), typeof(CLabel), typeof(CButton), typeof(CPictureBox),
                typeof(CCheckBox), typeof(CRadioButton), typeof(CComboBox), typeof(CTextBox),
                //typeof(CScrollBar)
            };
    }
}
