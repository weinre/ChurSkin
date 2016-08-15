// ContainerListView class
// Author: Jon Rista
// Email: jrista@hotmail.com
// 
// ContainerListView provides a listview type control
// that allows controls to be contained in sub item
// cells. The control is also fully compatible with
// Windows XP visual styles.
////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using ChurSkins.Helpers;
using ChurSkins.uxTheme;

namespace System.Windows.Forms
{

    #region 枚举类型

    /// <summary>
    ///     windows消息
    /// </summary>
    public enum WinMsg
    {
        WM_GETDLGCODE = 0x0087,
        WM_SETREDRAW = 0x000B,
        WM_CANCELMODE = 0x001F,
        WM_NOTIFY = 0x4e,
        WM_KEYDOWN = 0x100,
        WM_KEYUP = 0x101,
        WM_CHAR = 0x0102,
        WM_SYSKEYDOWN = 0x104,
        WM_SYSKEYUP = 0x105,
        WM_COMMAND = 0x111,
        WM_MENUCHAR = 0x120,
        WM_MOUSEMOVE = 0x200,
        WM_LBUTTONDOWN = 0x201,
        WM_MOUSELAST = 0x20a,
        WM_USER = 0x0400,
        WM_REFLECT = WM_USER + 0x1c00
    }

    /// <summary>
    ///     窗口消息
    /// </summary>
    public enum DialogCodes
    {
        DLGC_WANTARROWS = 0x0001, /* Control wants arrow keys         */
        DLGC_WANTTAB = 0x0002, /* Control wants tab keys           */
        DLGC_WANTALLKEYS = 0x0004, /* Control wants all keys           */
        DLGC_WANTMESSAGE = 0x0004, /* Pass message to control          */
        DLGC_HASSETSEL = 0x0008, /* Understands EM_SETSEL message    */
        DLGC_DEFPUSHBUTTON = 0x0010, /* Default pushbutton               */
        DLGC_UNDEFPUSHBUTTON = 0x0020, /* Non-default pushbutton           */
        DLGC_RADIOBUTTON = 0x0040, /* Radio button                     */
        DLGC_WANTCHARS = 0x0080, /* Want WM_CHAR messages            */
        DLGC_STATIC = 0x0100, /* Static item: don't include       */
        DLGC_BUTTON = 0x2000 /* Button item: can be checked      */
    }


    public enum ColumnScaleStyle
    {
        Slide,
        Spring
    }

    public enum MultiSelectMode
    {
        Single,
        Range,
        Selective
    }

    #endregion

    #region 事件处理程序和参数

    public class ItemsChangedEventArgs : EventArgs
    {
        public int IndexChanged = -1;

        public ItemsChangedEventArgs(int indexChanged)
        {
            IndexChanged = indexChanged;
        }
    }

    public delegate void ItemsChangedEventHandler(object sender, ItemsChangedEventArgs e);

    #endregion

    #region 列表子项

    [DesignTimeVisible(false), TypeConverter("ChurSkins.ListViewItemConverter")]
    public class ContainerListViewItem : ICloneable
    {
        #region Variables

        private readonly Rectangle bounds;

        #endregion

        #region Methods

        public object Clone()
        {
            var lvi = new ContainerListViewItem();
            lvi.BackColor = BackColor;
            lvi.Focused = Focused;
            lvi.Font = Font;
            lvi.ForeColor = ForeColor;
            lvi.ImageIndex = ImageIndex;
            lvi.Selected = Selected;
            lvi.Tag = Tag;
            lvi.Text = Text;
            lvi.UseItemStyleForSubItems = UseItemStyleForSubItems;

            return lvi;
        }

        #endregion

        public event MouseEventHandler MouseDown;

        private void OnSubItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            SubItems[e.IndexChanged].MouseDown += OnSubItemMouseDown;
        }

        private void OnSubItemMouseDown(object sender, MouseEventArgs e)
        {
            if (MouseDown != null)
                MouseDown(this, e);
        }

        #region Constructors

        public ContainerListViewItem()
        {
            bounds = new Rectangle();
            SubItems = new ContainerSubListViewItemCollection();
            SubItems.ItemsChanged += OnSubItemsChanged;
        }

        public ContainerListViewItem(Rectangle bounds)
        {
            this.bounds = bounds;
            SubItems = new ContainerSubListViewItemCollection();
            SubItems.ItemsChanged += OnSubItemsChanged;
        }

        public ContainerListViewItem(CListView Listview, int Index, Rectangle bounds)
        {
            this.Index = Index;
            this.bounds = bounds;
            ListView = Listview;
        }

        #endregion

        #region 属性

        /// <summary>
        ///     背景色
        /// </summary>
        public Color BackColor { get; set; }

        /// <summary>
        ///     区域
        /// </summary>
        public Rectangle Bounds
        {
            get { return bounds; }
        }

        /// <summary>
        ///     是否被选中
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        ///     是否获取焦点
        /// </summary>
        public bool Focused { get; set; }

        /// <summary>
        ///     字体
        /// </summary>
        public Font Font { get; set; }

        /// <summary>
        ///     字体颜色
        /// </summary>
        public Color ForeColor { get; set; }

        /// <summary>
        ///     图片索引
        /// </summary>
        public int ImageIndex { get; set; }

        /// <summary>
        ///     项目索引
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        ///     拥有此项的列表
        /// </summary>
        public CListView ListView { get; private set; }

        /// <summary>
        ///     是否被选中
        /// </summary>
        public bool Selected { get; set; }

        [
            Category("Behavior"),
            Description("子控件的Items集合。"),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
            Editor(typeof (CollectionEditor), typeof (UITypeEditor))
        ]
        public ContainerSubListViewItemCollection SubItems { get; private set; }

        /// <summary>
        ///     状态图片索引
        /// </summary>
        public int StateImageIndex { get; set; }

        /// <summary>
        ///     关联数据
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        ///     文本
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///     是否为子项目使用风格
        /// </summary>
        public bool UseItemStyleForSubItems { get; set; }

        [Browsable(false)]
        public bool Hovered { get; set; }

        #endregion
    }

    public class ContainerListViewItemCollection : CollectionBase
    {
        public event MouseEventHandler MouseDown;

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (MouseDown != null)
                MouseDown(sender, e);
        }

        #region 接口实现

        public ContainerListViewItem this[int index]
        {
            get { return List[index] as ContainerListViewItem; }
            set
            {
                List[index] = value;
                ((ContainerListViewItem) List[index]).MouseDown += OnMouseDown;
            }
        }

        public int this[ContainerListViewItem item]
        {
            get { return List.IndexOf(item); }
        }

        public int Add(ContainerListViewItem item)
        {
            item.MouseDown += OnMouseDown;
            return item.Index = List.Add(item);
        }

        public void AddRange(ContainerListViewItem[] items)
        {
            lock (List.SyncRoot)
            {
                for (var i = 0; i < items.Length; i++)
                {
                    items[i].MouseDown += OnMouseDown;
                    items[i].Index = List.Add(items[i]);
                }
            }
        }

        public void Remove(ContainerListViewItem item)
        {
            item.MouseDown -= OnMouseDown;
            List.Remove(item);
        }

        public new void Clear()
        {
            foreach (var t in List)
            {
                var col = ((ContainerListViewItem) t).SubItems;
                for (var j = 0; j < col.Count; j++)
                {
                    if (col[j].ItemControl == null) continue;
                    col[j].ItemControl.Parent = null;
                    col[j].ItemControl.Visible = false;
                    col[j].ItemControl = null;
                }
            }
            List.Clear();
        }

        public int IndexOf(ContainerListViewItem item)
        {
            return List.IndexOf(item);
        }

        #endregion
    }

    #endregion

    #region ContainerSubListViewItem classes

    [DesignTimeVisible(false), TypeConverter("ChurSkins.SubListViewItemConverter")]
    public class ContainerSubListViewItem : ICloneable
    {
        private Control childControl;

        public ContainerSubListViewItem()
        {
            Text = "SubItem";
        }

        public ContainerSubListViewItem(Control control)
        {
            Text = "";
            Construct(control);
        }

        public ContainerSubListViewItem(string str)
        {
            Text = str;
            Construct(null);
        }

        public ContainerSubListViewItem(string str, Color c)
        {
            Text = str;
            BackColor = c;
            Construct(null);
        }

        public Color BackColor { get; set; }

        public Control ItemControl
        {
            get { return childControl; }
            set
            {
                childControl = value;
                if (childControl != null) childControl.MouseDown += OnMouseDown;
            }
        }

        public string Text { get; set; }

        public object Clone()
        {
            var slvi = new ContainerSubListViewItem {ItemControl = null, Text = Text};
            return slvi;
        }

        public event MouseEventHandler MouseDown;

        protected void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (MouseDown != null)
                MouseDown(sender, e);
        }

        private void Construct(Control control)
        {
            childControl = control;

            if (childControl != null)
            {
                childControl.MouseDown += OnMouseDown;
            }
        }

        public override string ToString()
        {
            return (childControl == null ? Text : childControl.ToString());
        }
    }

    public class ContainerSubListViewItemCollection : CollectionBase
    {
        public event ItemsChangedEventHandler ItemsChanged;

        protected void OnItemsChanged(ItemsChangedEventArgs e)
        {
            if (ItemsChanged != null)
                ItemsChanged(this, e);
        }

        #region Interface Implementations

        public ContainerSubListViewItem this[int index]
        {
            get
            {
                try
                {
                    return List[index] as ContainerSubListViewItem;
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                List[index] = value;
                OnItemsChanged(new ItemsChangedEventArgs(index));
            }
        }

        public int Add(ContainerSubListViewItem item)
        {
            var index = List.Add(item);
            OnItemsChanged(new ItemsChangedEventArgs(index));
            return index;
        }

        public ContainerSubListViewItem Add(Control control)
        {
            var slvi = new ContainerSubListViewItem(control);
            lock (List.SyncRoot)
                OnItemsChanged(new ItemsChangedEventArgs(List.Add(slvi)));
            return slvi;
        }

        public ContainerSubListViewItem Add(string str)
        {
            var slvi = new ContainerSubListViewItem(str);
            lock (List.SyncRoot)
                OnItemsChanged(new ItemsChangedEventArgs(List.Add(slvi)));
            return slvi;
        }

        /// <summary>
        ///     添加子项
        /// </summary>
        /// <param name="str">文本</param>
        /// <param name="c">背景色</param>
        /// <returns></returns>
        public ContainerSubListViewItem Add(string str, Color c)
        {
            if (c == Color.Empty)
            {
                c = SystemColors.Control;
            }
            var slvi = new ContainerSubListViewItem(str, c);
            lock (List.SyncRoot)
                OnItemsChanged(new ItemsChangedEventArgs(List.Add(slvi)));
            return slvi;
        }

        public void AddRange(ContainerSubListViewItem[] items)
        {
            lock (List.SyncRoot)
            {
                for (var i = 0; i < items.Length; i++)
                {
                    OnItemsChanged(new ItemsChangedEventArgs(List.Add(items[i])));
                }
            }
        }

        #endregion
    }

    #endregion

    #region Column Header classes

    [DesignTimeVisible(false), TypeConverter("ChurSkins.ToggleColumnHeaderConverter")]
    public class ToggleColumnHeader : ICloneable
    {
        private int width;

        public ToggleColumnHeader()
        {
            Index = 0;
            ListView = null;
            TextAlign = HorizontalAlignment.Left;
            width = 90;
            Visible = true;
            Hovered = false;
            Pressed = false;
            ScaleStyle = ColumnScaleStyle.Slide;
        }

        [Browsable(false)]
        public bool Selected { get; set; }

        [
            Category("Appearance"),
            Description("The image to display in this header.")
        ]
        public Bitmap Image { get; set; }

        [Category("Behavior"), Description("Determines how a column reacts when another column is scaled.")]
        public ColumnScaleStyle ScaleStyle { get; set; }

        [Category("Data"), Description("The index of this column in the collection.")]
        public int Index { get; set; }

        [Category("Data"), Description("The parent listview of this column header.")]
        public CListView ListView { get; private set; }

        [Category("Appearance"), Description("The title of this column header.")]
        public string Text { get; set; }

        [Category("Behavior"), Description("The alignment of the column headers title.")]
        public HorizontalAlignment TextAlign { get; set; }

        [Category("Behavior"), Description("The width in pixels of this column header."), DefaultValue(90)]
        public int Width
        {
            get { return width; }
            set
            {
                width = value;
                OnWidthResized();
            }
        }

        [Category("Behavior"), Description("Determines wether the control is visible or hidden.")]
        public bool Visible { get; set; }

        [Browsable(false)]
        public bool Hovered { get; set; }

        [Browsable(false)]
        public bool Pressed { get; set; }

        public object Clone()
        {
            var ch = new ToggleColumnHeader();
            ch.Index = Index;
            ch.Text = Text;
            ch.TextAlign = TextAlign;
            ch.Width = width;

            return ch;
        }

        // send an internal event when a column is resized
        internal event EventHandler WidthResized;

        private void OnWidthResized()
        {
            if (WidthResized != null)
                WidthResized(this, new EventArgs());
        }

        public override string ToString()
        {
            return Text;
        }
    }

    public class ColumnHeaderCollection : CollectionBase
    {
        internal event EventHandler WidthResized;

        private void OnWidthResized(object sender, EventArgs e)
        {
            if (WidthResized != null)
                WidthResized(sender, e);
        }

        #region Interface Implementations

        public ToggleColumnHeader this[int index]
        {
            get
            {
                var tch = new ToggleColumnHeader();
                try
                {
                    tch = List[index] as ToggleColumnHeader;
                }
                catch
                {
                    Debug.WriteLine("Column at index " + index + " does not exist.");
                }
                return tch;
            }
            set
            {
                List[index] = value;
                ((ToggleColumnHeader) List[index]).WidthResized += OnWidthResized;
            }
        }

        public virtual int Add(ToggleColumnHeader colhead)
        {
            colhead.WidthResized += OnWidthResized;
            return colhead.Index = List.Add(colhead);
        }

        public virtual ToggleColumnHeader Add(string str, int width, HorizontalAlignment textAlign)
        {
            var tch = new ToggleColumnHeader();
            tch.Text = str;
            tch.Width = width;
            tch.TextAlign = textAlign;
            tch.WidthResized += OnWidthResized;

            lock (List.SyncRoot)
            {
                tch.Index = List.Add(tch);
            }
            return tch;
        }

        public virtual void AddRange(ToggleColumnHeader[] items)
        {
            lock (List.SyncRoot)
            {
                for (var i = 0; i < items.Length; i++)
                {
                    items[i].WidthResized += OnWidthResized;
                    List.Add(items[i]);
                }
            }
        }

        #endregion
    }

    #endregion

    #region ContainerListView

    #region ContainerListView Delegates

    public delegate void ContextMenuEventHandler(object sender, MouseEventArgs e);

    public delegate void ItemMenuEventHandler(object sender, MouseEventArgs e);

    public delegate void HeaderMenuEventHandler(object sender, MouseEventArgs e);

    #endregion

    /// <summary>
    ///     Provides a listview control in detail mode that
    ///     provides containers for each cell in a row/column.
    ///     The container can hold almost any object that derives
    ///     directly or indirectly from Control.
    /// </summary>
    [DefaultProperty("Items")]
    public class CListView : UserControl
    {
        #region Events

        //public event LabelEditEventHandler AfterLabelEdit;
        //public event LabelEditEventHandler BeforeLabelEdit;
        public event ColumnClickEventHandler ColumnClick;
        public event EventHandler ItemActivate;
        public event EventHandler SelectedIndexChanged;

        protected void OnAfterLabelEdit(LabelEditEventArgs e)
        {
        }

        protected void OnBeforeLabelEdit(LabelEditEventArgs e)
        {
        }

        protected void OnColumnClick(ColumnClickEventArgs e)
        {
            if (ColumnClick != null)
                ColumnClick(this, e);
        }

        protected void OnItemActivate(EventArgs e)
        {
            if (ItemActivate != null)
                ItemActivate(this, e);
        }

        protected void OnSelectedIndexChanged(EventArgs e)
        {
            if (SelectedIndexChanged != null)
                SelectedIndexChanged(this, e);
        }

        // This handler links any click events on an items subcontrols
        // to this control. Clicks will activate or deactivate the 
        // row containing the subcontrol.
        protected virtual void OnSubControlMouseDown(object sender, MouseEventArgs e)
        {
            //Debug.WriteLine("Subcontrol clicked.");

            var item = ((ContainerListViewItem) sender);

            if (multiSelectMode == MultiSelectMode.Single)
            {
                selectedIndices.Clear();
                selectedItems.Clear();

                for (var i = 0; i < items.Count; i++)
                {
                    items[i].Focused = false;
                    items[i].Selected = false;
                    if (items[i].Equals(item))
                    {
                        items[i].Focused = true;
                        items[i].Selected = true;
                        focusedIndex = firstSelected = i;

                        // set selected items and indices collections							
                        selectedIndices.Add(i);
                        selectedItems.Add(items[i]);
                    }
                }
                OnSelectedIndexChanged(new EventArgs());
            }
            else if (multiSelectMode == MultiSelectMode.Range)
            {
            }
            else if (multiSelectMode == MultiSelectMode.Selective)
            {
                // unfocus the previously focused item
                for (var i = 0; i < items.Count; i++)
                    items[i].Focused = false;

                if (item.Selected)
                {
                    item.Focused = false;
                    item.Selected = false;
                    selectedIndices.Remove(items[item]);
                    selectedItems.Remove(item);
                    OnSelectedIndexChanged(new EventArgs());
                }
                else
                {
                    item.Focused = true;
                    item.Selected = true;
                    selectedIndices.Add(items[item]);
                    selectedItems.Add(item);
                    OnSelectedIndexChanged(new EventArgs());
                }
                focusedIndex = items[item];
            }

            Invalidate(ClientRectangle);
        }

        // The ContainerListView provides three context menus.
        // One for the header, one for the visible rows, and
        // one for the whole control, which is fallen back
        // on when the header and item menus do not exist.
        public event ContextMenuEventHandler ContextMenuEvent;
        public event ItemMenuEventHandler ItemMenuEvent;
        public event HeaderMenuEventHandler HeaderMenuEvent;

        protected void OnContextMenuEvent(MouseEventArgs e)
        {
            if (ContextMenuEvent != null)
                ContextMenuEvent(this, e);

            PopMenu(contextMenu, e);
        }

        protected void OnItemMenuEvent(MouseEventArgs e)
        {
            if (ItemMenuEvent != null)
                ItemMenuEvent(this, e);
            else if (itemMenu == null)
                OnContextMenuEvent(e);
            else
                PopMenu(itemMenu, e);
        }

        protected void OnHeaderMenuEvent(MouseEventArgs e)
        {
            if (HeaderMenuEvent != null)
                HeaderMenuEvent(this, e);
            else if (headerMenu == null)
                OnContextMenuEvent(e);
            else
                PopMenu(headerMenu, e);
        }

        protected void PopMenu(ContextMenu theMenu, MouseEventArgs e)
        {
            if (theMenu != null)
                theMenu.Show(this, new Point(e.X, e.Y));
        }

        // Handlers for scrollbars scroll
        protected void OnScroll(object sender, EventArgs e)
        {
            GenerateColumnRects();
            Invalidate();
        }

        // Handler for column width resizing
        protected void OnColumnWidthResize(object sender, EventArgs e)
        {
            GenerateColumnRects();
        }

        #endregion

        #region 变量声明

        protected ItemActivation activation;
        protected bool allowColumnReorder;
        protected BorderStyle borderstyle = BorderStyle.Fixed3D;
        private int borderWid = 2;
        protected ColumnHeaderCollection columns;
        protected ColumnHeaderStyle headerStyle = ColumnHeaderStyle.Nonclickable;
        protected int headerBuffer = 25;

        protected bool hideSelection = true;
        protected bool hoverSelection;
        protected bool multiSelect;
        protected MultiSelectMode multiSelectMode = MultiSelectMode.Single;

        protected ContainerListViewItemCollection items;
        protected bool labelEdit;
        protected ImageList smallImageList, stateImageList;
        protected Comparer sortComparer;
        protected bool scrollable = true;
        protected SortOrder sorting;
        protected string text;
        protected ContainerListViewItem topItem;

        protected ContainerListViewItemCollection selectedItems;
        protected ArrayList selectedIndices;

        protected bool visualStyles;

        protected ContainerListViewItem focusedItem;
        protected int focusedIndex = -1;
        protected bool isFocused;

        protected Rectangle headerRect;
        protected Rectangle[] columnRects;
        protected Rectangle[] columnSizeRects;
        protected int lastColHovered = -1;
        protected int lastColPressed = -1;
        protected int lastColSelected = -1;
        protected bool doColTracking;
        protected Color colTrackColor = Color.WhiteSmoke;
        protected Color colSortColor = Color.Gainsboro;
        protected int allColsWidth;
        protected bool colScaleMode;
        protected int colScalePos;
        protected int colScaleWid;
        protected int scaledCol = -1;

        protected Rectangle rowsRect;
        protected Rectangle[] rowRects;
        protected int lastRowHovered = -1;
        protected int rowHeight = 18;
        protected int allRowsHeight;
        protected bool doRowTracking = true;
        protected Color rowTrackColor = Color.WhiteSmoke;
        protected Color rowSelectColor = SystemColors.Highlight;
        protected bool fullRowSelect = true;
        protected int firstSelected = -1, lastSelected = -1;

        protected bool gridLines = true;

        protected Color gridLineColor
        {
            get { return ColorHelper.ChangeColor(BackColor, .5f); }
        } //= Color.WhiteSmoke;

        protected Point lastClickedPoint;

        protected bool captureFocusClick;

        protected ContextMenu headerMenu, itemMenu, contextMenu;

        /// <summary>
        ///     水平滚动条
        /// </summary>
        protected HScrollBar hscrollBar;

        /// <summary>
        ///     垂直滚动条
        /// </summary>
        protected VScrollBar vscrollBar;

        protected bool ensureVisible = true;

        #endregion

        #region 构造函数

        public CListView()
        {
            Construct();
        }

        public override Color BackColor
        {
            get { return Color.Transparent; }
        }

        private void Construct()
        {
            rowHeight = 18;

            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.Opaque | ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.Selectable |
                     ControlStyles.UserMouse, true);
            SetStyle(ControlStyles.Opaque, false);
            UpdateStyles();
            //this.BackColor = //SystemColors.Window;

            columns = new ColumnHeaderCollection();
            items = new ContainerListViewItemCollection();
            selectedIndices = new ArrayList();
            selectedItems = new ContainerListViewItemCollection();

            hscrollBar = new HScrollBar {Parent = this, Minimum = 0, Maximum = 0, SmallChange = 10};
            hscrollBar.Hide();

            vscrollBar = new VScrollBar {Parent = this, Minimum = 0, Maximum = 0, SmallChange = rowHeight};
            vscrollBar.Hide();

            Attach();

            GenerateColumnRects();
            GenerateHeaderRect();
        }

        #endregion

        #region 公开界面属性

        [
            Category("Behavior"),
            Description("选中项目是否始终可见"),
            DefaultValue(true)
        ]
        public bool EnsureVisible
        {
            get { return ensureVisible; }
            set { ensureVisible = value; }
        }

        [
            Category("Behavior"),
            Description("是否获取焦点单击"),
            DefaultValue(false)
        ]
        public bool CaptureFocusClick
        {
            get { return captureFocusClick; }
            set { captureFocusClick = value; }
        }

        [
            Category("Behavior"),
            Description("上下文菜单显示时的标题右单击控件。")
        ]
        public ContextMenu HeaderMenu
        {
            get { return headerMenu; }
            set { headerMenu = value; }
        }

        [
            Category("Behavior"),
            Description("当一个项目被右击时显示的菜单")
        ]
        public ContextMenu ItemMenu
        {
            get { return itemMenu; }
            set { itemMenu = value; }
        }

        [
            Category("Behavior"),
            Description("控制被右击时显示的菜单")
        ]
        public override ContextMenu ContextMenu
        {
            get { return contextMenu; }
            set { contextMenu = value; }
        }

        [
            Category("Behavior"),
            Description("列标题集合"),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
            Editor(typeof (CollectionEditor), typeof (UITypeEditor))
        ]
        public ColumnHeaderCollection Columns
        {
            get { return columns; }
        }

        [
            Category("Data"),
            Description("项的集合."),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
            Editor(typeof (CollectionEditor), typeof (UITypeEditor))
        ]
        public virtual ContainerListViewItemCollection Items
        {
            get { return items; }
        }

        [
            Category("Behavior"),
            Description("指定什么行动启动和项目."),
            DefaultValue(ItemActivation.Standard)
        ]
        public ItemActivation Activation
        {
            get { return activation; }
            set { activation = value; }
        }

        [
            Category("Behavior"),
            Description("指定列标题是否能被重新排序."),
            DefaultValue(false)
        ]
        public bool AllowColumnReorder
        {
            get { return allowColumnReorder; }
            set { allowColumnReorder = value; }
        }

        [
            Category("Appearance"),
            Description("样式边框样式."),
            DefaultValue(BorderStyle.FixedSingle)
        ]
        public new BorderStyle BorderStyle
        {
            get { return borderstyle; }
            set
            {
                borderstyle = value;
                if (borderstyle == BorderStyle.Fixed3D)
                {
                    borderWid = 2;
                }
                else
                {
                    borderWid = 1;
                }
                Invalidate(ClientRectangle);
            }
        }

        [
            Category("Appearance"),
            Description("指定列标题是否响应鼠标点击."),
            DefaultValue(ColumnHeaderStyle.Nonclickable)
        ]
        public ColumnHeaderStyle HeaderStyle
        {
            get { return headerStyle; }
            set
            {
                headerStyle = value;
                Invalidate(ClientRectangle);
                headerBuffer = headerStyle == ColumnHeaderStyle.None ? 0 : 25;
            }
        }

        [
            Category("Behavior"),
            Description("当鼠标悬停在列标题上时是否突出显示该列."),
            DefaultValue(false)
        ]
        public bool ColumnTracking
        {
            get { return doColTracking; }
            set { doColTracking = value; }
        }

        [
            Category("Appearance"),
            Description("列突出显示时的颜色."),
            DefaultValue(typeof (Color), "Color.WhiteSmoke")
        ]
        public Color ColumnTrackColor
        {
            get { return colTrackColor; }
            set { colTrackColor = value; }
        }

        [
            Category("Appearance"),
            Description("指定用于当前选定的排序列的颜色."),
            DefaultValue(typeof (Color), "Color.Gainsboro")
        ]
        public Color ColumnSortColor
        {
            get { return colSortColor; }
            set { colSortColor = value; }
        }

        [
            Category("Behavior"),
            Description("当鼠标悬停在某行时是否突出显示."),
            DefaultValue(true)
        ]
        public bool RowTracking
        {
            get { return doRowTracking; }
            set { doRowTracking = value; }
        }

        [
            Category("Appearance"),
            Description("某行突出显示的颜色."),
            DefaultValue(typeof (Color), "Color.WhiteSmoke")
        ]
        public Color RowTrackColor
        {
            get { return rowTrackColor; }
            set { rowTrackColor = value; }
        }

        [
            Category("Appearance"),
            Description("指定用于选定行的颜色."),
            DefaultValue(typeof (Color), "SystemColors.Highlight")
        ]
        public Color RowSelectColor
        {
            get { return rowSelectColor; }
            set { rowSelectColor = value; }
        }

        [
            Category("Behavior"),
            Description("当某项被选择时，它的子项是否同样被突出显示."),
            DefaultValue(true)
        ]
        public bool FullRowSelect
        {
            get { return fullRowSelect; }
            set { fullRowSelect = value; }
        }

        [
            Category("Appearance"),
            Description("指定用于网格线的颜色."),
            DefaultValue(typeof (Color), "Color.WhiteSmoke")
        ]
        public Color GridLineColor
        {
            get { return ColorHelper.ChangeColor(BackColor, .5f); }
            // get { return gridLineColor; }
            //set { gridLineColor = value; }
        }

        [
            Category("Behavior"),
            Description("是否显示网格线."),
            DefaultValue(true)
        ]
        public bool GridLines
        {
            get { return gridLines; }
            set
            {
                gridLines = value;
                Invalidate(ClientRectangle);
            }
        }

        [
            Category("Behavior"),
            Description("小图标列表(16x16).")
        ]
        public ImageList SmallImageList
        {
            get { return smallImageList; }
            set
            {
                smallImageList = value;
                Invalidate(ClientRectangle);
            }
        }

        [
            Category("Behavior"),
            Description("自定义状态图像列表 (16x16).")
        ]
        public ImageList StateImageList
        {
            get { return stateImageList; }
            set { stateImageList = value; }
        }

        [
            Category("Behavior"),
            Description("项目是否可被编辑")
        ]
        public bool LabelEdit
        {
            get { return labelEdit; }
            set { labelEdit = value; }
        }

        [
            Category("Behavior"),
            Description("当控件失去焦点时，是否隐藏当前选中项"),
            DefaultValue(true)
        ]
        public bool HideSelection
        {
            get { return hideSelection; }
            set { hideSelection = value; }
        }

        [
            Category("Behavior"),
            Description("当鼠标悬停在它很短的时间时是否自动选择一行"),
            DefaultValue(false)
        ]
        public bool HoverSelection
        {
            get { return hoverSelection; }
            set { hoverSelection = value; }
        }

        [
            Category("Behavior"),
            Description("是否允许多选"),
            DefaultValue(false)
        ]
        public bool MultiSelect
        {
            get { return multiSelect; }
            set { multiSelect = value; }
        }

        [
            Category("Appearance"),
            Description("是否使用xp风格样式"),
            DefaultValue(false)
        ]
        public bool VisualStyles
        {
            get
            {
                bool val;
                try
                {
                    val = visualStyles && Wrapper.IsAppThemed();
                }
                catch
                {
                    val = visualStyles;
                }
                return val;
            }
            set { visualStyles = value; }
        }

        /// <summary>
        ///     当前被选中的项目索引数组
        /// </summary>
        [Browsable(false)]
        public ArrayList SelectedIndices
        {
            get { return selectedIndices; }
        }

        /// <summary>
        ///     当前被选中的子项
        /// </summary>
        [Browsable(false)]
        public ContainerListViewItemCollection SelectedItems
        {
            get { return selectedItems; }
        }

        #endregion

        #region Overrides

        protected const int WM_KEYDOWN = 0x0100;
        protected const int VK_LEFT = 0x0025;
        protected const int VK_UP = 0x0026;
        protected const int VK_RIGHT = 0x0027;
        protected const int VK_DOWN = 0x0028;

        public override bool PreProcessMessage(ref Message msg)
        {
            if (msg.Msg == WM_KEYDOWN)
            {
                if (focusedItem != null && items.Count > 0)
                {
                    var keyData = ((Keys) (int) msg.WParam) | ModifierKeys;
                    var keyCode = ((Keys) (int) msg.WParam);

                    if (keyData == Keys.Down)
                    {
                        if (focusedIndex < items.Count - 1)
                        {
                            focusedItem.Focused = false;
                            focusedItem.Selected = false;
                            focusedIndex++;

                            items[focusedIndex].Focused = true;
                            items[focusedIndex].Selected = true;
                            focusedItem = items[focusedIndex];

                            MakeSelectedVisible();

                            Invalidate(ClientRectangle);
                        }

                        return true;
                    }
                    if (keyData == Keys.Up)
                    {
                        if (focusedIndex > 0)
                        {
                            focusedItem.Focused = false;
                            focusedItem.Selected = false;
                            focusedIndex--;

                            items[focusedIndex].Focused = true;
                            items[focusedIndex].Selected = true;
                            focusedItem = items[focusedIndex];

                            MakeSelectedVisible();

                            Invalidate(ClientRectangle);
                        }

                        return true;
                    }
                }
            }

            return base.PreProcessMessage(ref msg);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //Rectangle r = ClientRectangle;
            //Graphics g = e.Graphics;

            //DrawBackground(g, r);
            //DrawRows(g, r);
            //DrawHeaders(g, r);
            //DrawExtra(g, r);
            //DrawBorder(g, r);

            var r = ClientRectangle;
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            DrawBorder(g, r);
            DrawBackground(g, r);
            DrawHeaders(g, r);
            DrawRows(g, r);
            //DrawHeaders(g, r);
            DrawExtra(g, r);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GenerateHeaderRect();
            GenerateRowsRect();
            AdjustScrollbars();

            // invalidate subitem controls
            for (var i = 0; i < items.Count; i++)
            {
                var lvi = items[i];
                for (var j = 0; j < lvi.SubItems.Count; j++)
                {
                    var slvi = lvi.SubItems[j];
                    if (slvi.ItemControl != null)
                        slvi.ItemControl.Invalidate(slvi.ItemControl.ClientRectangle);
                }
            }
            Invalidate();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == (int) WinMsg.WM_GETDLGCODE)
            {
                m.Result =
                    new IntPtr((int) DialogCodes.DLGC_WANTCHARS | (int) DialogCodes.DLGC_WANTARROWS | m.Result.ToInt32());
            }
        }

        private int mouseMoveTicks;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            int i;

            base.OnMouseMove(e);

            //lastColHovered = -1;
            lastRowHovered = -1;

            // if the mouse button is currently
            // pressed down on a header column,
            // moving will attempt to move the
            // position of that column
            if (lastColPressed >= 0 && allowColumnReorder)
            {
                if (Math.Abs(e.X - lastClickedPoint.X) > 3)
                {
                    // set rect for drag pos
                }
            }
            else if (colScaleMode)
            {
                lastColHovered = -1;
                Cursor.Current = Cursors.VSplit;
                colScalePos = e.X - lastClickedPoint.X;
                if (colScalePos + colScaleWid <= 0)
                    columns[scaledCol].Width = 1;
                else
                    columns[scaledCol].Width = colScalePos + colScaleWid;

                Invalidate();
            }
            else
            {
                if (columns.Count > 0)
                {
                    // check region mouse is in
                    //  header			
                    if (headerStyle != ColumnHeaderStyle.None)
                    {
                        //GenerateHeaderRect();

                        Cursor.Current = Cursors.Default;
                        if (MouseInRect(e, headerRect))
                        {
                            if (columnRects.Length < columns.Count)
                                GenerateColumnRects();

                            for (i = 0; i < columns.Count; i++)
                            {
                                if (MouseInRect(e, columnRects[i]))
                                {
                                    columns[i].Hovered = true;
                                    lastColHovered = i;
                                }
                                else
                                {
                                    columns[i].Hovered = false;
                                }

                                if (MouseInRect(e, columnSizeRects[i]))
                                {
                                    Cursor.Current = Cursors.VSplit;
                                }
                            }
                            Invalidate();
                            if (++mouseMoveTicks > 10)
                            {
                                mouseMoveTicks = 0;
                                Thread.Sleep(1);
                            }
                            return;
                        }
                    }
                }

                if (lastColHovered >= 0)
                {
                    columns[lastColHovered].Hovered = false;
                    lastColHovered = -1;
                    Invalidate();
                }

                if (items.Count > 0)
                {
                    //  rows
                    GenerateRowsRect();
                    if (MouseInRect(e, rowsRect))
                    {
                        GenerateRowRects();
                        for (i = 0; i < items.Count; i++)
                        {
                            if (MouseInRect(e, rowRects[i]))
                            {
                                items[i].Hovered = true;
                                lastRowHovered = i;
                                break;
                            }
                            items[i].Hovered = false;
                        }
                        Invalidate();
                    }
                }
            }

            if (++mouseMoveTicks > 10)
            {
                mouseMoveTicks = 0;
                Thread.Sleep(1);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (!isFocused)
            {
                Focus();

                if (!captureFocusClick)
                    return;
            }

            lastClickedPoint = new Point(e.X, e.Y);

            // determine if a header was pressed
            if (headerStyle != ColumnHeaderStyle.None)
            {
                if (MouseInRect(e, headerRect) && e.Button == MouseButtons.Left)
                {
                    for (var i = 0; i < columns.Count; i++)
                    {
                        columns[i].Pressed = false;
                        if (MouseInRect(e, columnSizeRects[i]) && items.Count > 0)
                        {
                            if (columns[i].ScaleStyle == ColumnScaleStyle.Slide)
                            {
                                // autosize column
                                if (e.Clicks == 2 && e.Button == MouseButtons.Left && items.Count > 0)
                                {
                                    var mwid = 0;
                                    var twid = 0;
                                    for (var j = 0; j < items.Count; j++)
                                    {
                                        if (i > 0 && items[j].SubItems.Count > 0)
                                            twid = GetStringWidth(items[j].SubItems[i - 1].Text) + 10;
                                        else if (i == 0)
                                            twid = GetStringWidth(items[j].Text) + 10;
                                        twid += 5;
                                        if (twid > mwid)
                                            mwid = twid;
                                    }

                                    twid = GetStringWidth(columns[i].Text);
                                    if (twid > mwid)
                                        mwid = twid;

                                    mwid += 5;

                                    columns[i].Width = mwid;
                                }
                                // scale column
                                else
                                {
                                    colScaleMode = true;
                                    colScaleWid = columnRects[i].Width;
                                    scaledCol = i;
                                }
                            }
                        }
                        else if (MouseInRect(e, columnRects[i]) && !MouseInRect(e, columnSizeRects[i]))
                        {
                            columns[i].Pressed = true;
                            lastColPressed = i;
                        }
                    }
                    Invalidate();
                    return;
                }
            }

            // determine if a row was pressed			
            var selChange = false;
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                if (MouseInRect(e, rowsRect) && items.Count > 0)
                {
                    GenerateRowRects();
                    //focusedIndex = -1;
                    if (multiSelectMode == MultiSelectMode.Single)
                    {
                        selectedIndices.Clear();
                        selectedItems.Clear();

                        for (var i = 0; i < items.Count; i++)
                        {
                            items[i].Focused = false;
                            items[i].Selected = false;
                            if (MouseInRect(e, rowRects[i]))
                            {
                                items[i].Focused = true;
                                items[i].Selected = true;
                                focusedIndex = firstSelected = i;

                                // set selected items and indices collections							
                                selectedIndices.Add(i);
                                selectedItems.Add(items[i]);

                                MakeSelectedVisible();
                            }
                        }
                        OnSelectedIndexChanged(new EventArgs());
                    }
                    else if (multiSelectMode == MultiSelectMode.Range)
                    {
                        for (var i = 0; i < items.Count; i++)
                        {
                            if (firstSelected == -1)
                            {
                                items[i].Focused = true;
                                items[i].Selected = true;
                                firstSelected = focusedIndex = i;

                                // set selected items and indices collections
                                selectedIndices.Clear();
                                selectedIndices.Add(i);

                                selectedItems.Clear();
                                selectedItems.Add(items[i]);

                                MakeSelectedVisible();

                                OnSelectedIndexChanged(new EventArgs());
                            }
                            else
                            {
                                // unfocus the previously focused item
                                if (focusedIndex >= 0 && focusedIndex < items.Count)
                                    items[focusedIndex].Focused = false;

                                if (MouseInRect(e, rowRects[i]))
                                {
                                    lastSelected = focusedIndex = i;
                                }

                                if (lastSelected >= 0 && lastSelected < firstSelected)
                                {
                                    selectedIndices.Clear();
                                    selectedItems.Clear();
                                    for (var j = 0; j < items.Count; j++)
                                    {
                                        if (j <= firstSelected && j >= lastSelected)
                                        {
                                            items[j].Selected = true;

                                            // set selected items and indices collections										
                                            selectedIndices.Add(j);
                                            selectedItems.Add(items[j]);
                                            selChange = true;
                                        }
                                        else
                                            items[j].Selected = false;

                                        if (j == lastSelected)
                                            items[j].Focused = true;
                                        else
                                            items[j].Focused = false;
                                    }
                                }
                                else if (lastSelected >= 0 && lastSelected > firstSelected)
                                {
                                    selectedIndices.Clear();
                                    selectedItems.Clear();
                                    for (var j = 0; j < items.Count; j++)
                                    {
                                        if (j >= firstSelected && j <= lastSelected)
                                        {
                                            items[j].Selected = true;

                                            // set selected items and indices collections
                                            selectedIndices.Add(j);
                                            selectedItems.Add(items[j]);
                                            selChange = true;
                                        }
                                        else
                                            items[j].Selected = false;

                                        if (j == lastSelected)
                                            items[j].Focused = true;
                                        else
                                            items[j].Focused = false;
                                    }
                                }
                                else
                                {
                                    selectedIndices.Clear();
                                    selectedItems.Clear();
                                    for (var j = 0; j < items.Count; j++)
                                    {
                                        if (j == firstSelected && j == lastSelected)
                                        {
                                            items[j].Selected = true;
                                            items[j].Focused = true;
                                            selectedIndices.Add(j);
                                            selectedItems.Add(items[j]);
                                            selChange = true;
                                        }
                                        else
                                        {
                                            items[j].Selected = false;
                                            items[j].Focused = false;
                                        }
                                    }
                                }
                            }

                            if (selChange)
                            {
                                MakeSelectedVisible();
                                OnSelectedIndexChanged(new EventArgs());
                            }
                        }
                    }
                    else if (multiSelectMode == MultiSelectMode.Selective)
                    {
                        for (var i = 0; i < items.Count; i++)
                        {
                            if (MouseInRect(e, rowRects[i]))
                            {
                                // unfocus the previously focused item
                                if (focusedIndex >= 0 && focusedIndex < items.Count)
                                    items[focusedIndex].Focused = false;

                                if (items[i].Selected)
                                {
                                    items[i].Focused = false;
                                    items[i].Selected = false;
                                    selectedIndices.Remove(i);
                                    selectedItems.Remove(items[i]);

                                    MakeSelectedVisible();

                                    OnSelectedIndexChanged(new EventArgs());
                                }
                                else
                                {
                                    items[i].Focused = true;
                                    items[i].Selected = true;
                                    selectedIndices.Add(i);
                                    selectedItems.Add(items[i]);

                                    MakeSelectedVisible();

                                    OnSelectedIndexChanged(new EventArgs());
                                }
                                focusedIndex = i;
                            }
                        }
                    }


                    if (focusedIndex >= 0)
                        focusedItem = items[focusedIndex];
                    else
                        focusedItem = null;

                    Invalidate();
                }
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            int i;

            base.OnMouseUp(e);

            lastClickedPoint = Point.Empty;

            if (colScaleMode)
            {
                //columns[scaledCol].Width += colScalePos;
                colScaleMode = false;
                colScalePos = 0;
                scaledCol = -1;
                colScaleWid = 0;

                AdjustScrollbars();
            }

            if (lastColPressed >= 0)
            {
                columns[lastColPressed].Pressed = false;
                if (MouseInRect(e, columnRects[lastColPressed]) && !MouseInRect(e, columnSizeRects[lastColPressed]) &&
                    e.Button == MouseButtons.Left)
                {
                    // invoke column click event
                    OnColumnClick(new ColumnClickEventArgs(lastColPressed));

                    // change currently selected column
                    if (lastColSelected >= 0)
                        columns[lastColSelected].Selected = false;

                    columns[lastColPressed].Selected = true;
                    lastColSelected = lastColPressed;
                }
            }
            lastColPressed = -1;

            // Check for context click
            if (e.Button == MouseButtons.Right)
            {
                if (MouseInRect(e, headerRect))
                    OnHeaderMenuEvent(e);
                else if (MouseInRect(e, rowsRect))
                {
                    for (i = 0; i < items.Count; i++)
                    {
                        if (MouseInRect(e, rowRects[i]))
                        {
                            OnItemMenuEvent(e);
                            break;
                        }
                    }
                    if (i >= items.Count)
                        OnContextMenuEvent(e);
                }
                else
                    OnContextMenuEvent(e);
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (vscrollBar.Visible)
                    vscrollBar.Value = (vscrollBar.Value - vscrollBar.SmallChange*(e.Delta/100) < 0
                        ? 0
                        : vscrollBar.Value - vscrollBar.SmallChange*(e.Delta/100));
                else if (hscrollBar.Visible)
                    hscrollBar.Value = (hscrollBar.Value - hscrollBar.SmallChange*(e.Delta/100) < 0
                        ? 0
                        : hscrollBar.Value - hscrollBar.SmallChange*(e.Delta/100));
            }
            else if (e.Delta < 0)
            {
                if (vscrollBar.Visible)
                    vscrollBar.Value = (vscrollBar.Value - vscrollBar.SmallChange*(e.Delta/100) >
                                        vscrollBar.Maximum - vscrollBar.LargeChange
                        ? vscrollBar.Maximum - vscrollBar.LargeChange
                        : vscrollBar.Value - vscrollBar.SmallChange*(e.Delta/100));
                else if (hscrollBar.Visible)
                    hscrollBar.Value = (hscrollBar.Value - hscrollBar.SmallChange*(e.Delta/100) >
                                        hscrollBar.Maximum - hscrollBar.LargeChange
                        ? hscrollBar.Maximum - hscrollBar.LargeChange
                        : hscrollBar.Value - hscrollBar.SmallChange*(e.Delta/100));
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            //Debug.WriteLine(e.KeyChar);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (multiSelect)
            {
                if (e.KeyCode == Keys.ControlKey)
                {
                    multiSelectMode = MultiSelectMode.Selective;
                }
                else if (e.KeyCode == Keys.ShiftKey)
                {
                    multiSelectMode = MultiSelectMode.Range;
                }
            }

            if (!multiSelect && e.KeyCode == Keys.Return)
            {
                OnItemActivate(new EventArgs());
            }

            if (e.KeyCode == Keys.Home)
            {
                if (vscrollBar.Visible)
                    vscrollBar.Value = 0;
                else if (hscrollBar.Visible)
                    hscrollBar.Value = 0;
            }
            else if (e.KeyCode == Keys.End)
            {
                if (vscrollBar.Visible)
                    vscrollBar.Value = vscrollBar.Maximum - vscrollBar.LargeChange;
                else if (hscrollBar.Visible)
                    hscrollBar.Value = hscrollBar.Maximum - hscrollBar.LargeChange;
            }
            else if (e.KeyCode == Keys.PageUp)
            {
                if (vscrollBar.Visible)
                    vscrollBar.Value = (vscrollBar.LargeChange > vscrollBar.Value
                        ? 0
                        : vscrollBar.Value - vscrollBar.LargeChange);
                else if (hscrollBar.Visible)
                    hscrollBar.Value = (hscrollBar.LargeChange > hscrollBar.Value
                        ? 0
                        : hscrollBar.Value - hscrollBar.LargeChange);
            }
            else if (e.KeyCode == Keys.PageDown)
            {
                if (vscrollBar.Visible)
                    vscrollBar.Value = (vscrollBar.Value + vscrollBar.LargeChange >
                                        vscrollBar.Maximum - vscrollBar.LargeChange
                        ? vscrollBar.Maximum - vscrollBar.LargeChange
                        : vscrollBar.Value + vscrollBar.LargeChange);
                else if (hscrollBar.Visible)
                    hscrollBar.Value = (hscrollBar.Value + hscrollBar.LargeChange >
                                        hscrollBar.Maximum - hscrollBar.LargeChange
                        ? hscrollBar.Maximum - hscrollBar.LargeChange
                        : hscrollBar.Value + hscrollBar.LargeChange);
            }
            Invalidate(ClientRectangle);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (!e.Shift)
            {
                multiSelectMode = MultiSelectMode.Single;
            }
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            isFocused = true;
            Invalidate(ClientRectangle);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            isFocused = false;
            Invalidate(ClientRectangle);
        }

        #endregion

        #region Helper Functions

        // wireing of child control events
        protected virtual void Attach()
        {
            items.MouseDown += OnSubControlMouseDown;
            columns.WidthResized += OnColumnWidthResize;

            hscrollBar.ValueChanged += OnScroll;
            vscrollBar.ValueChanged += OnScroll;
        }

        protected virtual void Detach()
        {
            items.MouseDown -= OnSubControlMouseDown;
            columns.WidthResized -= OnColumnWidthResize;

            hscrollBar.ValueChanged -= OnScroll;
            vscrollBar.ValueChanged -= OnScroll;
        }

        // Rectangle and region generation functions
        protected void GenerateColumnRects()
        {
            columnRects = new Rectangle[columns.Count];
            columnSizeRects = new Rectangle[columns.Count];
            var lwidth = 2 - hscrollBar.Value;
            var colwid = 0;
            allColsWidth = 0;

            CalcSpringWids(ClientRectangle);
            for (var i = 0; i < columns.Count; i++)
            {
                colwid = (columns[i].ScaleStyle == ColumnScaleStyle.Spring ? springWid : columns[i].Width);
                columnRects[i] = new Rectangle(lwidth, 2, colwid, 20);
                columnSizeRects[i] = new Rectangle(lwidth + colwid - 4, ClientRectangle.Top + 2, 4, headerBuffer);
                lwidth += colwid + 1;
                allColsWidth += colwid;
            }
        }

        protected void GenerateHeaderRect()
        {
            headerRect = new Rectangle(ClientRectangle.Left + 2 - hscrollBar.Value, ClientRectangle.Top + 2,
                ClientRectangle.Width - 4, 20);
        }

        protected void GenerateRowsRect()
        {
            rowsRect = new Rectangle(ClientRectangle.Left + 2 - hscrollBar.Value,
                ClientRectangle.Top + (headerStyle == ColumnHeaderStyle.None ? 2 : 22), ClientRectangle.Width - 4,
                ClientRectangle.Height - (headerStyle == ColumnHeaderStyle.None ? 2 : 22));
        }

        protected void GenerateRowRects()
        {
            rowRects = new Rectangle[items.Count];
            var lheight = 2 + headerBuffer - vscrollBar.Value;
            var lftpos = ClientRectangle.Left + 2;
            allRowsHeight = items.Count*rowHeight;
            for (var i = 0; i < items.Count; i++)
            {
                rowRects[i] = new Rectangle(lftpos, lheight, ClientRectangle.Width - 4, rowHeight - 1);
                lheight += rowHeight;
            }
        }

        // Adjust scroll bar settings and visibility
        private int vsize, hsize;

        public virtual void AdjustScrollbars()
        {
            if (items.Count > 0 || columns.Count > 0 && !colScaleMode)
            {
                allColsWidth = 0;
                for (var i = 0; i < columns.Count; i++)
                {
                    allColsWidth += columns[i].Width;
                }

                allRowsHeight = items.Count*rowHeight;

                vsize = vscrollBar.Width;
                hsize = hscrollBar.Height;

                hscrollBar.Left = ClientRectangle.Left + 2;
                hscrollBar.Width = ClientRectangle.Width - vsize - 4;
                hscrollBar.Top = ClientRectangle.Top + ClientRectangle.Height - hscrollBar.Height - 2;
                hscrollBar.Maximum = allColsWidth;
                hscrollBar.LargeChange = (ClientRectangle.Width - vsize - 4 > 0 ? ClientRectangle.Width - vsize - 4 : 0);
                if (allColsWidth > ClientRectangle.Width - 4 - vsize)
                    hscrollBar.Show();
                else
                {
                    hscrollBar.Hide();
                    hsize = 0;
                    hscrollBar.Value = 0;
                }

                vscrollBar.Left = ClientRectangle.Left + ClientRectangle.Width - vscrollBar.Width - 2;
                vscrollBar.Top = ClientRectangle.Top + headerBuffer + 2;
                vscrollBar.Height = ClientRectangle.Height - hsize - headerBuffer - 4;
                vscrollBar.Maximum = allRowsHeight;
                vscrollBar.LargeChange = (ClientRectangle.Height - headerBuffer - hsize - 4 > 0
                    ? ClientRectangle.Height - headerBuffer - hsize - 4
                    : 0);
                if (allRowsHeight > ClientRectangle.Height - headerBuffer - 4 - hsize)
                    vscrollBar.Show();
                else
                {
                    vscrollBar.Hide();
                    vsize = 0;
                    vscrollBar.Value = 0;
                }

                hscrollBar.Width = ClientRectangle.Width - vsize - 4;
                hscrollBar.Top = ClientRectangle.Top + ClientRectangle.Height - hscrollBar.Height - 2;
                hscrollBar.LargeChange = (ClientRectangle.Width - vsize - 4 > 0 ? ClientRectangle.Width - vsize - 4 : 0);
                if (allColsWidth > ClientRectangle.Width - 4 - vsize)
                    hscrollBar.Show();
                else
                {
                    hscrollBar.Hide();
                    hsize = 0;
                    hscrollBar.Value = 0;
                }
            }
        }

        // mouse movement/click helpers
        protected bool MouseInRect(MouseEventArgs me, Rectangle rect)
        {
            return (me.X >= rect.Left && me.X <= rect.Left + rect.Width)
                   && (me.Y >= rect.Top && me.Y <= rect.Top + rect.Height);
        }

        protected void MakeSelectedVisible()
        {
            if (focusedIndex > -1 && ensureVisible)
            {
                var item = items[focusedIndex];
                if (item != null && item.Focused && item.Selected)
                {
                    var r = ClientRectangle;
                    var i = items.IndexOf(item);
                    var pos = r.Top + (rowHeight*i) + headerBuffer + 2 - vscrollBar.Value;
                    try
                    {
                        if (pos + rowHeight > r.Top + r.Height)
                        {
                            vscrollBar.Value += Math.Abs((r.Top + r.Height) - (pos + rowHeight));
                        }
                        else if (pos < r.Top + headerBuffer)
                        {
                            vscrollBar.Value -= Math.Abs(r.Top + headerBuffer - pos);
                        }
                    }
                    catch (ArgumentException)
                    {
                        if (vscrollBar.Value > vscrollBar.Maximum)
                            vscrollBar.Value = vscrollBar.Maximum;
                        else if (vscrollBar.Value < vscrollBar.Minimum)
                            vscrollBar.Value = vscrollBar.Minimum;
                    }
                }
            }
        }

        protected int GetStringWidth(string s)
        {
            var g = Graphics.FromImage(new Bitmap(32, 32));
            var strSize = g.MeasureString(s, Font);
            return (int) strSize.Width;
        }

        protected string TruncatedString(string s, int width, int offset, Graphics g)
        {
            var sprint = "";
            int swid;
            int i;
            SizeF strSize;

            try
            {
                strSize = g.MeasureString(s, Font);
                swid = ((int) strSize.Width);
                i = s.Length;

                for (i = s.Length; i > 0 && swid > width - offset; i--)
                {
                    strSize = g.MeasureString(s.Substring(0, i), Font);
                    swid = ((int) strSize.Width);
                }

                if (i < s.Length)
                    if (i - 3 <= 0)
                        sprint = s.Substring(0, 1) + "...";
                    else
                        sprint = s.Substring(0, i - 3) + "...";
                else
                    sprint = s.Substring(0, i);
            }
            catch
            {
            }

            return sprint;
        }

        // rendering helpers
        private int springWid;
        private int springCnt;

        private void CalcSpringWids(Rectangle r)
        {
            springCnt = 0;
            springWid = (r.Width - borderWid*2);
            for (var i = 0; i < columns.Count; i++)
            {
                if (columns[i].ScaleStyle == ColumnScaleStyle.Slide)
                    springWid -= columns[i].Width;
                else
                    springCnt++;
            }

            if (springCnt > 0 && springWid > 0)
                springWid = springWid/springCnt;
        }

        protected virtual void DrawBorder(Graphics g, Rectangle r)
        {
            var rect = ClientRectangle;
            rect.Width--;
            rect.Height--;
            g.DrawPath(new Pen(color), DrawHelper.CreateRoundPath2(rect, 5));
            //// if running in XP with styles
            //if (VisualStyles)
            //{
            //    DrawBorderStyled(g, r);
            //    return;
            //}

            //Rectangle rect = this.ClientRectangle;
            //if (borderstyle == BorderStyle.FixedSingle)
            //{
            //    g.DrawRectangle(SystemPens.ControlDarkDark, r.Left, r.Top, r.Width, r.Height);
            //}
            //else if (borderstyle == BorderStyle.Fixed3D)
            //{
            //    ControlPaint.DrawBorder3D(g, r.Left, r.Top, r.Width, r.Height, Border3DStyle.Sunken);
            //}
            //else if (borderstyle == BorderStyle.None)
            //{
            //    // do not render any border
            //}
        }

        private Alpha alpha = Alpha.Normal;

        private Color color
        {
            get { return Color.FromArgb((int) alpha, bgColor.R, bgColor.G, bgColor.B); }
        }

        private Color bgColor = Color.White;

        public Color BgColor
        {
            get { return bgColor; }
            set
            {
                bgColor = value;
                //  gridLineColor = ColorHelper.ChangeColor(BackColor, .5f);
                Invalidate();
            }
        }

        protected virtual void DrawBackground(Graphics g, Rectangle r)
        {
            int i;
            int lwidth = 2, lheight = 1;

            g.FillPath(new SolidBrush(color), DrawHelper.CreateRoundPath2(r, 5));

            // 表头被单击时绘画
            if (headerStyle == ColumnHeaderStyle.Clickable)
            {
                for (i = 0; i < columns.Count; i++)
                {
                    if (columns[i].Selected)
                    {
                        g.FillRectangle(new SolidBrush(colSortColor), r.Left + lwidth - hscrollBar.Value,
                            r.Top + 2 + headerBuffer, columns[i].Width, r.Height - 4 - headerBuffer);
                        break;
                    }
                    lwidth += columns[i].Width;
                }
            }

            // 表头跟踪绘画
            if (doColTracking && (lastColHovered >= 0 && lastColHovered < columns.Count))
            {
                g.FillRectangle(new SolidBrush(colTrackColor), columnRects[lastColHovered].Left, 22,
                    columnRects[lastColHovered].Width, r.Height - 22);
            }

            // 行跟踪绘画
            if (doRowTracking && (lastRowHovered >= 0 && lastRowHovered < items.Count))
            {
                g.FillRectangle(new SolidBrush(rowTrackColor), r.Left + 2, rowRects[lastRowHovered].Top,
                    r.Left + r.Width - 4, rowHeight);
            }

            // 花表格线
            if (gridLines)
            {
                var p = new Pen(new SolidBrush(gridLineColor), 1.0f);
                lwidth = lheight = 1;

                // vertical
                for (i = 0; i < columns.Count; i++)
                {
                    if (r.Left + lwidth + columns[i].Width >= r.Left + r.Width - 2)
                        break;

                    g.DrawLine(p, r.Left + lwidth + columns[i].Width - hscrollBar.Value, r.Top + 2 + headerBuffer,
                        r.Left + lwidth + columns[i].Width - hscrollBar.Value, r.Top + r.Height - 2);
                    lwidth += columns[i].Width;
                }


                // horizontal
                for (i = 0; i < items.Count; i++)
                {
                    g.DrawLine(p, r.Left + 2, r.Top + headerBuffer + rowHeight + lheight - vscrollBar.Value,
                        r.Left + r.Width, r.Top + headerBuffer + rowHeight + lheight - vscrollBar.Value);
                    lheight += rowHeight;
                }
            }
        }

        /// <summary>
        ///     画表格的头部
        /// </summary>
        /// <param name="g"></param>
        /// <param name="r"></param>
        protected virtual void DrawHeaders(Graphics g, Rectangle r)
        {
            //return;
            // if running in XP with styles
            if (VisualStyles)
            {
                DrawHeadersStyled(g, r);
                return;
            }

            if (headerStyle != ColumnHeaderStyle.None)
            {
                g.FillRectangle(new SolidBrush(color), r.Left + 2, r.Top + 2, r.Width - 5, headerBuffer);

                CalcSpringWids(r);

                // render column headers and trailing column header
                var last = 2;
                int i;

                var lp_scr = r.Left - hscrollBar.Value;

                // g.Clip = new Region(new Rectangle(r.Left + 2, r.Top + 2, r.Width - 5, r.Top + headerBuffer));
                for (i = 0; i < columns.Count; i++)
                {
                    if ((lp_scr + last + columns[i].Width > r.Left + 2) && (lp_scr + last < r.Left + r.Width - 2))
                    {
                        //按钮按下和弹起的块块
                        if (headerStyle == ColumnHeaderStyle.Clickable && columns[i].Pressed)
                        {
                            alpha = Alpha.PressOrDown;
                            g.FillRectangle(new SolidBrush(color),
                                new Rectangle(lp_scr + last, r.Top + 2, columns[i].Width, r.Top + headerBuffer));
                        }
                        else
                        {
                            alpha = Alpha.Normal;
                        }
                        //  System.Windows.Forms.ControlPaint.DrawButton(g, lp_scr + last, r.Top + 2, columns[i].Width, r.Top + headerBuffer, ButtonState.Flat);
                        // else
                        //g.FillRectangle(new SolidBrush(color), new Rectangle(lp_scr + last, r.Top + 2, columns[i].Width, r.Top + headerBuffer));

                        // System.Windows.Forms.ControlPaint.DrawButton(g, lp_scr + last, r.Top + 2, columns[i].Width, r.Top + headerBuffer, ButtonState.Normal);

                        if (columns[i].Image != null)
                        {
                            g.DrawImage(columns[i].Image, new Rectangle(lp_scr + last + 4, r.Top + 3, 16, 16));
                            g.DrawString(TruncatedString(columns[i].Text, columns[i].Width, 25, g), Font,
                                SystemBrushes.ControlText, lp_scr + last + 22, r.Top + 5);
                        }
                        else
                        {
                            // StringFormat sf = new StringFormat();
                            // sf.LineAlignment = StringAlignment.Center;

                            //Rectangle txtrect =new Rectangle();
                            var sp = "";
                            if (columns[i].TextAlign == HorizontalAlignment.Left)
                            {
                                // txtrect=
                                g.DrawString(TruncatedString(columns[i].Text, columns[i].Width, 0, g), Font,
                                    SystemBrushes.ControlText, lp_scr + last + 4, r.Top + 7
                                    );
                            }
                            else if (columns[i].TextAlign == HorizontalAlignment.Right)
                            {
                                sp = TruncatedString(columns[i].Text, columns[i].Width, 0, g);
                                g.DrawString(sp, Font, SystemBrushes.ControlText,
                                    lp_scr + last + columns[i].Width -
                                    StringTools.MeasureDisplayStringWidth(g, sp, Font) - 4, r.Top + 7
                                    );
                            }
                            else
                            {
                                sp = TruncatedString(columns[i].Text, columns[i].Width, 0, g);
                                g.DrawString(sp, Font, SystemBrushes.ControlText,
                                    lp_scr + last + (columns[i].Width/2) -
                                    (StringTools.MeasureDisplayStringWidth(g, sp, Font)/2), r.Top + 7
                                    );
                            }
                        }
                    }
                    last += columns[i].Width;
                }

                // only render trailing column header if the end of the
                // last column ends before the boundary of the listview 
                //也就是最后多的那一项
                if (!(lp_scr + last + 5 > r.Left + r.Width))
                {
                    // g.Clip = new Region(new Rectangle(r.Left + 2, r.Top + 2, r.Width - 5, r.Top + headerBuffer));
                    //g.FillRectangle(new SolidBrush(color), new Rectangle(lp_scr + last, r.Top + 2, columns[i].Width, r.Top + headerBuffer));

                    //  System.Windows.Forms.ControlPaint.DrawButton(g, lp_scr + last, r.Top + 2, r.Width - (r.Left + last) - 3 + hscrollBar.Value, r.Top + headerBuffer, ButtonState.Normal);
                }
            }
        }

        protected virtual void DrawRows(Graphics g, Rectangle r)
        {
            if (columns.Count > 0)
            {
                // render listview item rows
                int last;
                int j, i;

                // set up some commonly used values
                // to cut down on cpu cycles and boost
                // the lists performance
                var lp_scr = r.Left + 2 - hscrollBar.Value; // left viewport position less scrollbar position
                var lp = r.Left + 2; // left viewport position
                var tp_scr = r.Top + 2 + headerBuffer - vscrollBar.Value;
                    // top viewport position less scrollbar position
                var tp = r.Top + 2 + headerBuffer; // top viewport position

                for (i = 0; i < items.Count; i++)
                {
                    j = 0;
                    last = 0;

                    // render item, but only if its within the viewport
                    if ((tp_scr + (rowHeight*i) + 2 > r.Top + 2)
                        && (tp_scr + (rowHeight*i) + 2 < r.Top + r.Height - 2 - hsize))
                    {
                        g.Clip =
                            new Region(new Rectangle(r.Left + 2, r.Top + headerBuffer + 2, r.Width - vsize - 5,
                                r.Height - hsize - 5));

                        var rowSelWidth = (allColsWidth < (r.Width - 5) || hscrollBar.Visible
                            ? allColsWidth
                            : r.Width - 5);
                        if (!fullRowSelect)
                            rowSelWidth = columns[0].Width - 2;

                        // render selected item highlights
                        if (items[i].Selected && isFocused)
                        {
                            g.FillRectangle(new SolidBrush(rowSelectColor), lp, tp_scr + (rowHeight*i), rowSelWidth,
                                rowHeight);
                        }
                        else if (items[i].Selected && !isFocused && hideSelection)
                        {
                            ControlPaint.DrawFocusRectangle(g,
                                new Rectangle(lp_scr, tp_scr + (rowHeight*i), rowSelWidth, rowHeight));
                        }
                        else if (items[i].Selected && !isFocused && !hideSelection)
                        {
                            g.FillRectangle(SystemBrushes.Control, lp, tp_scr + (rowHeight*i), rowSelWidth, rowHeight);
                        }

                        if (items[i].Focused && multiSelect && isFocused)
                        {
                            ControlPaint.DrawFocusRectangle(g,
                                new Rectangle(lp_scr, tp_scr + (rowHeight*i), rowSelWidth, rowHeight));
                        }

                        // render item
                        if ((lp_scr + 2 + columns[j].Width > r.Left + 4))
                        {
                            g.Clip =
                                new Region(new Rectangle(lp + 2, tp,
                                    (columns[j].Width > r.Width ? r.Width - 6 : columns[j].Width - 2),
                                    r.Height - hsize - 5));

                            if (smallImageList != null &&
                                (items[i].ImageIndex >= 0 && items[i].ImageIndex < smallImageList.Images.Count))
                            {
                                smallImageList.Draw(g, lp_scr + 4, tp_scr + (rowHeight*i) + 1, 16, 16,
                                    items[i].ImageIndex);
                                g.DrawString(TruncatedString(items[i].Text, columns[j].Width, 18, g), Font,
                                    (items[i].Selected && isFocused
                                        ? SystemBrushes.HighlightText
                                        : new SolidBrush(ForeColor)), lp_scr + 22, tp_scr + (rowHeight*i) + 2);
                            }
                            else
                            {
                                g.DrawString(TruncatedString(items[i].Text, columns[j].Width, 0, g), Font,
                                    (items[i].Selected && isFocused
                                        ? SystemBrushes.HighlightText
                                        : new SolidBrush(ForeColor)), lp_scr + 4, tp_scr + (rowHeight*i) + 2);
                            }
                        }
                    }

                    // render sub items
                    if (columns.Count > 0)
                    {
                        for (j = 0; j < items[i].SubItems.Count && j < columns.Count - 1; j++)
                        {
                            last += columns[j].Width;

                            // only render subitem if it is visible within the viewport
                            if ((lp_scr + 2 + last + columns[j].Width > r.Left + 4)
                                && (lp_scr + last < r.Left + r.Width - 4)
                                && (tp_scr + (rowHeight*i) + 2 > r.Top + 2)
                                && (tp_scr + (rowHeight*i) + 2 < r.Top + r.Height - 2 - hsize))
                            {
                                g.Clip =
                                    new Region(new Rectangle(lp_scr + last + 4, tp,
                                        (last + columns[j + 1].Width > r.Width - 6
                                            ? r.Width - 6
                                            : columns[j + 1].Width - 6), r.Height - hsize - 5));
                                if (items[i].SubItems[j].ItemControl != null)
                                {
                                    var c = items[i].SubItems[j].ItemControl;
                                    c.Location = new Point(lp_scr + last + 2, tp_scr + (rowHeight*i) + 2);
                                    c.ClientSize = new Size(columns[j + 1].Width - 6, rowHeight - 4);
                                    c.Parent = this;
                                }
                                else
                                {
                                    var sp = "";
                                    if (columns[j + 1].TextAlign == HorizontalAlignment.Left)
                                    {
                                        g.DrawString(
                                            TruncatedString(items[i].SubItems[j].Text, columns[j + 1].Width, 12, g),
                                            Font,
                                            (items[i].Selected && isFocused
                                                ? SystemBrushes.HighlightText
                                                : SystemBrushes.WindowText), lp_scr + last + 4,
                                            tp_scr + (rowHeight*i) + 2);
                                    }
                                    else if (columns[j + 1].TextAlign == HorizontalAlignment.Right)
                                    {
                                        sp = TruncatedString(items[i].SubItems[j].Text, columns[j + 1].Width, 12, g);
                                        g.DrawString(sp, Font,
                                            (items[i].Selected && isFocused
                                                ? SystemBrushes.HighlightText
                                                : new SolidBrush(ForeColor)),
                                            lp_scr + last + columns[j + 1].Width -
                                            StringTools.MeasureDisplayStringWidth(g, sp, Font) - 2,
                                            tp_scr + (rowHeight*i) + 2);
                                    }
                                    else
                                    {
                                        sp = TruncatedString(items[i].SubItems[j].Text, columns[j + 1].Width, 12, g);
                                        g.DrawString(sp, Font,
                                            (items[i].Selected && isFocused
                                                ? SystemBrushes.HighlightText
                                                : new SolidBrush(ForeColor)),
                                            lp_scr + last + (columns[j + 1].Width/2) -
                                            (StringTools.MeasureDisplayStringWidth(g, sp, Font)/2),
                                            tp_scr + (rowHeight*i) + 2);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        protected virtual void DrawExtra(Graphics g, Rectangle r)
        {
            if (hscrollBar.Visible && vscrollBar.Visible)
            {
                g.ResetClip();
                g.FillRectangle(SystemBrushes.Control, r.Width - vscrollBar.Width - borderWid,
                    r.Height - hscrollBar.Height - borderWid, vscrollBar.Width, hscrollBar.Height);
            }
        }

        // visual styles rendering functions
        protected virtual void DrawBorderStyled(Graphics g, Rectangle r)
        {
            var oldreg = g.Clip;
            g.Clip = new Region(r);
            g.DrawRectangle(new Pen(SystemBrushes.InactiveBorder), r.Left, r.Top, r.Width - 1, r.Height - 1);
            g.DrawRectangle(new Pen(BackColor), r.Left + 1, r.Top + 1, r.Width - 3, r.Height - 3);
            g.Clip = oldreg;
        }

        protected virtual void DrawHeadersStyled(Graphics g, Rectangle r)
        {
            if (headerStyle != ColumnHeaderStyle.None)
            {
                var colwid = 0;
                int i;
                var last = 2;
                CalcSpringWids(r);

                var lp_scr = r.Left - hscrollBar.Value;
                var lp = r.Left;
                var tp = r.Top + 2;

                var hdc = g.GetHdc();
                try
                {
                    // render column headers and trailing column header					
                    for (i = 0; i < columns.Count; i++)
                    {
                        colwid = (columns[i].ScaleStyle == ColumnScaleStyle.Spring ? springWid : columns[i].Width);
                        if (headerStyle == ColumnHeaderStyle.Clickable && columns[i].Pressed)
                            Wrapper.DrawBackground("HEADER", "HEADERITEM", "PRESSED", hdc,
                                lp_scr + last, tp,
                                colwid, headerBuffer,
                                lp, tp, r.Width - 6, headerBuffer);
                        else if (headerStyle != ColumnHeaderStyle.None && columns[i].Hovered)
                            Wrapper.DrawBackground("HEADER", "HEADERITEM", "HOT", hdc,
                                lp_scr + last, tp,
                                colwid, headerBuffer,
                                lp, tp, r.Width - 6, headerBuffer);
                        else
                            Wrapper.DrawBackground("HEADER", "HEADERITEM", "NORMAL", hdc,
                                lp_scr + last, tp,
                                colwid, headerBuffer,
                                lp, tp, r.Width - 6, headerBuffer);
                        last += colwid;
                    }
                    // only render trailing column header if the end of the
                    // last column ends before the boundary of the listview 
                    if (!(r.Left + last + 2 - hscrollBar.Value > r.Left + r.Width))
                    {
                        Wrapper.DrawBackground("HEADER", "HEADERITEM", "NORMAL", hdc, lp_scr + last, tp,
                            r.Width - last - 2 + hscrollBar.Value, headerBuffer, r.Left, r.Top, r.Width, headerBuffer);
                    }
                }
                catch
                {
                }
                finally
                {
                    g.ReleaseHdc(hdc);
                }

                last = 1;
                for (i = 0; i < columns.Count; i++)
                {
                    colwid = (columns[i].ScaleStyle == ColumnScaleStyle.Spring ? springWid : columns[i].Width);
                    g.Clip =
                        new Region(new Rectangle(lp_scr + last + 2, tp,
                            (r.Left + last + colwid > r.Left + r.Width ? (r.Width - (r.Left + last)) - 4 : colwid - 2) +
                            hscrollBar.Value, r.Top + headerBuffer));
                    if (columns[i].Image != null)
                    {
                        g.DrawImage(columns[i].Image, new Rectangle(lp_scr + last + 4, r.Top + 3, 16, 16));
                        g.DrawString(TruncatedString(columns[i].Text, colwid, 25, g), Font, SystemBrushes.ControlText,
                            r.Left + last + 22 - hscrollBar.Value, r.Top + 5);
                    }
                    else
                    {
                        var sp = TruncatedString(columns[i].Text, colwid, 0, g);
                        if (columns[i].TextAlign == HorizontalAlignment.Left)
                        {
                            g.DrawString(TruncatedString(columns[i].Text, colwid, 0, g), Font, SystemBrushes.ControlText,
                                last + 4 - hscrollBar.Value, r.Top + 5);
                        }
                        else if (columns[i].TextAlign == HorizontalAlignment.Right)
                        {
                            g.DrawString(sp, Font, SystemBrushes.ControlText,
                                last + colwid - StringTools.MeasureDisplayStringWidth(g, sp, Font) - 4 -
                                hscrollBar.Value, r.Top + 5);
                        }
                        else
                        {
                            g.DrawString(sp, Font, SystemBrushes.ControlText,
                                last + (colwid/2) - (StringTools.MeasureDisplayStringWidth(g, sp, Font)/2) -
                                hscrollBar.Value, r.Top + 5);
                        }
                    }
                    last += colwid;
                }
            }
        }

        #endregion
    }

    #endregion

    #region Type Converters

    public class ToggleColumnHeaderConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof (InstanceDescriptor))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
            Type destinationType)
        {
            if (destinationType == typeof (InstanceDescriptor) && value is ToggleColumnHeader)
            {
                var lvi = (ToggleColumnHeader) value;

                var ci = typeof (ToggleColumnHeader).GetConstructor(new Type[] {});
                if (ci != null)
                {
                    return new InstanceDescriptor(ci, null, false);
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class ListViewItemConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof (InstanceDescriptor))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
            Type destinationType)
        {
            if (destinationType == typeof (InstanceDescriptor) && value is ContainerListViewItem)
            {
                var lvi = (ContainerListViewItem) value;

                var ci = typeof (ContainerListViewItem).GetConstructor(new Type[] {});
                if (ci != null)
                {
                    return new InstanceDescriptor(ci, null, false);
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    public class SubListViewItemConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof (InstanceDescriptor))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
            Type destinationType)
        {
            if (destinationType == typeof (InstanceDescriptor) && value is ContainerSubListViewItem)
            {
                var lvi = (ContainerSubListViewItem) value;

                var ci = typeof (ContainerSubListViewItem).GetConstructor(new Type[] {});
                if (ci != null)
                {
                    return new InstanceDescriptor(ci, null, false);
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    #endregion
}