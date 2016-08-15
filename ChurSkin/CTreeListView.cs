using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Win32.Consts;
namespace System.Windows.Forms
{
    public delegate void RowMouseEventHandler(object sender, MouseEventArgs e);
    public delegate void RowSubItemEventHandler(object sender, MouseEventArgs e);
    public partial class CTreeListView : ContainerListView
    {

        #region Events

        public RowMouseEventHandler RowMouseEvent;

        public RowSubItemEventHandler RowSubItemEventHandler;

        protected override void OnSubControlMouseDown(object sender, MouseEventArgs e)
        {
            TreeListNode node = (TreeListNode)sender;

            UnselectNodes(nodes);

            node.Focused = true;
            node.Selected = true;
            //focusedIndex = firstSelected = i;
            if (e.Clicks >= 2)
                node.Toggle();

            // set selected items and indices collections							
            //selectedIndices.Add(i);						
            //selectedItems.Add(items[i]);
            Invalidate(ClientRectangle);
        }

        protected virtual void OnNodesChanged(object sender, EventArgs e)
        {
            AdjustScrollbars();
        }

        /// <summary>
        /// 某行鼠标事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnRowMouseEvent(object sender, MouseEventArgs e)
        {
            if (RowMouseEvent != null)
            {
                RowMouseEvent(sender, e);
            }

        }
        protected void OnRowSubItemEvent(object sender, MouseEventArgs e)
        {
            if (RowSubItemEventHandler != null)
            {
                RowSubItemEventHandler(sender, e);
            }
        }
        #endregion

        #region Variables
        protected TreeListNodeCollection nodes;
        protected int indent = 19;
        protected int itemheight = 20;
        protected bool showlines = false, showrootlines = false, showplusminus = true;

        protected ListDictionary pmRects;
        protected ListDictionary nodeRowRects;
        protected ListDictionary SubNodeRects;

        protected bool alwaysShowPM = false;

        protected Bitmap bmpMinus, bmpPlus;

        private TreeListNode curNode;
        private TreeListNode virtualParent;

        private TreeListNodeCollection selectedNodes;

        private bool mouseActivate = false;

        private bool allCollapsed = false;
        #endregion

        #region Constructor
        public CTreeListView()
            : base()
        {
            InitializeComponent();
            virtualParent = new TreeListNode();

            nodes = virtualParent.Nodes;
            nodes.Owner = virtualParent;
            nodes.MouseDown += new MouseEventHandler(OnSubControlMouseDown);
            nodes.NodesChanged += new EventHandler(OnNodesChanged);

            selectedNodes = new TreeListNodeCollection();

            nodeRowRects = new ListDictionary();
            pmRects = new ListDictionary();
            SubNodeRects = new ListDictionary();

            // myAssembly = Assembly.GetAssembly(typeof(CTreeListView));
            //Stream bitmapStream1 = myAssembly.GetManifestResourceStream("YNLD.Forms.TreeListView.Resources.tv_minus.bmp");
            bmpMinus = Properties.Resources.expan;//new Bitmap(bitmapStream1);

            //Stream bitmapStream2 = myAssembly.GetManifestResourceStream("YNLD.Forms.TreeListView.Resources.tv_plus.bmp");
            bmpPlus = Properties.Resources.unexpan;//new Bitmap(bitmapStream2);
        }
        #endregion

        #region 公开属性

        [Browsable(false)]
        public TreeListNodeCollection SelectedNodes
        {
            get { return GetSelectedNodes(virtualParent); }
        }

        [
        Category("Behavior"),
        Description("项目是否通过双击才能展开."),
        DefaultValue(false)
        ]
        public bool MouseActivte
        {
            get { return mouseActivate; }
            set { mouseActivate = value; }
        }

        [
        Category("Behavior"),
        Description("是否显示+/-号"),
        DefaultValue(false)
        ]
        public bool AlwaysShowPlusMinus
        {
            get { return alwaysShowPM; }
            set { alwaysShowPM = value; }
        }

        [
        Category("Data"),
        Description("树形列表节点集合."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Editor(typeof(CollectionEditor), typeof(UITypeEditor))
        ]
        public TreeListNodeCollection Nodes
        {
            get { return nodes; }
        }

        [Browsable(false)]
        public override ContainerListViewItemCollection Items
        {
            get { return items; }
        }

        [
        Category("Behavior"),
        Description("子节点的缩进宽度，以像素单位"),
        DefaultValue(19)
        ]
        public int Indent
        {
            get { return indent; }
            set { indent = value; }
        }

        [
        Category("Appearance"),
        Description("各节点的高度."),
        DefaultValue(18)
        ]
        public int ItemHeight
        {
            get { return itemheight; }
            set { itemheight = value; }
        }

        [
        Category("Behavior"),
        Description("是否显示连接线."),
        DefaultValue(true)
        ]
        public bool ShowLines
        {
            get { return showlines; }
            set { showlines = value; }
        }

        [
        Category("Behavior"),
        Description("是否在根节点直接显示连接线"),
        DefaultValue(false)
        ]
        public bool ShowRootLines
        {
            get { return showrootlines; }
            set { showrootlines = value; }
        }

        [
        Category("Behavior"),
        Description("是否显示+/-按钮"),
        DefaultValue(true)
        ]
        public bool ShowPlusMinus
        {
            get { return showplusminus; }
            set { showplusminus = value; }
        }
        #endregion

        #region Overrides
        public override bool PreProcessMessage(ref Message msg)
        {
            if (msg.Msg != WM_KEYDOWN) return base.PreProcessMessage(ref msg);
            if (nodes.Count <= 0) return base.PreProcessMessage(ref msg);
            if (curNode == null) return base.PreProcessMessage(ref msg);
            var keyCode = ((Keys)(int)msg.WParam);

            if (keyCode == Keys.Left)
            {
                if (curNode.IsExpanded)
                {
                    curNode.Collapse();
                }
                else if (curNode.ParentNode() != null)
                {
                    var t = (TreeListNode)curNode.ParentNode();
                    if (t.ParentNode() != null)
                    {
                        curNode.Selected = false;
                        curNode.Focused = false;
                        curNode = (TreeListNode)curNode.ParentNode();
                        curNode.Selected = true;
                        curNode.Focused = true;
                    }
                }
                Invalidate();
                //return true;
            }
            if (keyCode == Keys.Right)
            {
                if (!curNode.IsExpanded)
                {
                    curNode.Expand();
                }
                else if (curNode.IsExpanded && curNode.GetNodeCount(false) > 0)
                {
                    curNode.Selected = false;
                    curNode.Focused = false;
                    curNode = (TreeListNode)curNode.FirstChild();
                    curNode.Selected = true;
                    curNode.Focused = true;
                }

                Invalidate();
                //return true;
            }
            if (keyCode == Keys.Up)
            {
                if (curNode.PreviousSibling() == null && curNode.ParentNode() != null)
                {
                    var t = (TreeListNode)curNode.ParentNode();
                    if (t.ParentNode() != null)
                    {
                        curNode.Selected = false;
                        curNode.Focused = false;
                        curNode = (TreeListNode)curNode.ParentNode();
                        curNode.Selected = true;
                        curNode.Focused = true;
                    }

                    Invalidate();
                    //return true;
                }
                if (curNode.PreviousSibling() != null)
                {
                    var t = (TreeListNode)curNode.PreviousSibling();
                    if (t.GetNodeCount(false) > 0 && t.IsExpanded)
                    {
                        do
                        {
                            t = (TreeListNode)t.LastChild();
                            if (t.IsExpanded) continue;
                            curNode.Selected = false;
                            curNode.Focused = false;
                            curNode = t;
                            curNode.Selected = true;
                            curNode.Focused = true;
                        } while (t.GetNodeCount(false) > 0 && t.IsExpanded);
                    }
                    else
                    {
                        curNode.Selected = false;
                        curNode.Focused = false;
                        curNode = (TreeListNode)curNode.PreviousSibling();
                        curNode.Selected = true;
                        curNode.Focused = true;
                    }
                    Invalidate();
                    //return true;
                }
            }
            else if (keyCode == Keys.Down)
            {
                if (curNode.IsExpanded && curNode.GetNodeCount(false) > 0)
                {
                    curNode.Selected = false;
                    curNode.Focused = false;
                    curNode = (TreeListNode)curNode.FirstChild();
                    curNode.Selected = true;
                    curNode.Focused = true;
                }
                else if (curNode.NextSibling() == null && curNode.ParentNode() != null)
                {
                    var t = curNode;
                    do
                    {
                        t = (TreeListNode)t.ParentNode();
                        if (t.NextSibling() != null)
                        {
                            curNode.Selected = false;
                            curNode.Focused = false;
                            curNode = (TreeListNode)t.NextSibling();
                            curNode.Selected = true;
                            curNode.Focused = true;
                            break;
                        }
                    } while (t.NextSibling() == null && t.ParentNode() != null);
                }
                else if (curNode.NextSibling() != null)
                {
                    curNode.Selected = false;
                    curNode.Focused = false;
                    curNode = (TreeListNode)curNode.NextSibling();
                    curNode.Selected = true;
                    curNode.Focused = true;
                }

                Invalidate();
                //return true;
            }
            return base.PreProcessMessage(ref msg);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AdjustScrollbars();
            Invalidate();
        }


        private void AutoSetColWidth(TreeListNodeCollection nodes, ref int mwid, ref int twid, int i)
        {
            for (int j = 0; j < nodes.Count; j++)
            {
                if (i > 0)
                    twid = GetStringWidth(nodes[j].SubItems[i - 1].Text);
                else
                    twid = GetStringWidth(nodes[j].Text);
                twid += 5;
                if (twid > mwid)
                    mwid = twid;

                if (nodes[j].Nodes.Count > 0)
                {
                    AutoSetColWidth(nodes[j].Nodes, ref mwid, ref twid, i);
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            for (int i = 0; i < columns.Count; i++)
            {
                if (columnSizeRects.Length > 0 && MouseInRect(e, columnSizeRects[i]))
                {
                    // autosize column
                    if (e.Clicks == 2 && e.Button == MouseButtons.Left)
                    {
                        int mwid = 0;
                        int twid = 0;

                        AutoSetColWidth(nodes, ref mwid, ref twid, i);

                        twid = GetStringWidth(columns[i].Text);
                        if (twid > mwid) mwid = twid;
                        mwid += 5;
                        if (columns[i].Image != null) mwid += 18;
                        columns[i].Width = mwid;
                        GenerateColumnRects();
                    }// scale column
                    else
                    {
                        colScaleMode = true;
                        colScaleWid = columnRects[i].Width;
                        scaledCol = i;
                    }
                }
            }

            if (MouseInRect(e, rowsRect))
            {
                InvokeMouseEvent(e);

                TreeListNode cnode;
                // check if a nodes plus/minus has been clicked
                cnode = NodePlusClicked(e);
                if (cnode != null)
                {
                    cnode.Toggle();
                    AdjustScrollbars();
                    Invalidate(ClientRectangle);
                    return;
                }

                if (e.Button == MouseButtons.Left)
                {
                    // check if a noderow has been clicked
                    if (multiSelectMode == MultiSelectMode.Single)
                    {
                        UnselectNodes(nodes);
                        //selectedNodes.Clear();

                        cnode = NodeInNodeRow(e);
                        if (cnode != null)
                        {
                            cnode.Focused = true;
                            cnode.Selected = true;
                            curNode = cnode;

                            //selectedNodes.Add(curNode);
                            if (e.Clicks == 2 && !mouseActivate)
                            {
                                cnode.Toggle();
                                AdjustScrollbars();
                            }
                            else if (e.Clicks == 2 && mouseActivate)
                                OnItemActivate(new EventArgs());
                        }
                        else if (curNode != null)
                        {
                            curNode.Focused = false;
                            curNode.Selected = false;
                        }
                        Invalidate();
                    }
                    else if (multiSelectMode == MultiSelectMode.Range)
                    {
                        // to be implemented at a later date
                    }
                    else if (multiSelectMode == MultiSelectMode.Selective)
                    {
                        UnfocusNodes(nodes);

                        cnode = NodeInNodeRow(e);
                        if (cnode != null)
                        {
                            if (cnode.Selected)
                            {
                                // remove node from collection of selected nodes
                                //selectedNodes.Remove(curNode);

                                cnode.Focused = false;
                                cnode.Selected = false;
                                curNode = null;
                            }
                            else
                            {
                                cnode.Focused = true;
                                cnode.Selected = true;
                                curNode = cnode;

                                // add node to collection of selected nodes
                                //selectedNodes.Add(curNode);
                            }

                            if (e.Clicks == 2 && !mouseActivate)
                            {
                                cnode.Toggle();
                                AdjustScrollbars();
                            }
                            else if (e.Clicks == 2 && mouseActivate)
                                OnItemActivate(new EventArgs());

                            Invalidate();
                        }

                    }
                }
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (e.KeyCode == Keys.F5)
            {
                if (allCollapsed)
                    ExpandAll();
                else
                    CollapseAll();
            }
        }

        int rendcnt = 0;
        protected override void DrawRows(Graphics g, Rectangle r)
        {
            // render listview item rows
            int i;
            int totalRend = 0, childCount = 0;

            int maxrend = ClientRectangle.Height / itemheight + 1;
            int prior = vscrollBar.Value / itemheight - 1;
            if (prior < 0) prior = 0;
            int nodesprior = 0;
            int rootstart = 0;

            if (prior > 0)
            {
                for (i = 0; i < nodes.Count; i++)
                {
                    var nodescur = 0;
                    if (nodes[i].IsExpanded) nodescur = nodes[i].GetVisibleNodeCount(true);
                    nodesprior += nodescur;
                    nodesprior++;
                    if (nodesprior <= prior) continue;
                    nodesprior -= nodescur + 1;
                    rootstart = i;
                    break;
                }
            }

            totalRend = nodesprior;
            rendcnt = 0;

            nodeRowRects.Clear();
            pmRects.Clear();
            SubNodeRects.Clear();
            for (i = rootstart; i < nodes.Count && rendcnt < maxrend; i++)
            {
                RenderNodeRows(nodes[i], g, r, 0, i, ref totalRend, ref childCount, nodes.Count);
            }
        }

        protected override void DrawBackground(Graphics g, Rectangle r)
        {
            int i;
            int lwidth = 2, lheight = 1;

            g.FillRectangle(new SolidBrush(BackColor), r);

            // 表头被单击时绘画
            if (headerStyle == ColumnHeaderStyle.Clickable)
            {
                for (i = 0; i < columns.Count; i++)
                {
                    if (columns[i].Selected)
                    {
                        g.FillRectangle(new SolidBrush(colSortColor), r.Left + lwidth - hscrollBar.Value, r.Top + 2 + headerBuffer, columns[i].Width, r.Height - 4 - headerBuffer);
                        break;
                    }
                    lwidth += columns[i].Width;
                }
            }

            // 表头跟踪绘画
            if (doColTracking && (lastColHovered >= 0 && lastColHovered < columns.Count))
            {
                g.FillRectangle(new SolidBrush(colTrackColor), columnRects[lastColHovered].Left, 22, columnRects[lastColHovered].Width, r.Height - 22);
            }

            // 行跟踪绘画
            if (doRowTracking && (lastRowHovered >= 0 && lastRowHovered < items.Count))
            {
                g.FillRectangle(new SolidBrush(rowTrackColor), r.Left + 2, rowRects[lastRowHovered].Top, r.Left + r.Width - 4, rowHeight);
            }

            // 花表格线
            if (gridLines)
            {
                var p = new Pen(new SolidBrush(gridLineColor), 1.0f);
                lwidth = lheight = 1;

                // vertical
                for (i = 0; i < columns.Count; i++)
                {
                    if (r.Left + lwidth + columns[i].Width >= r.Left + r.Width - 2) break;
                    g.DrawLine(p,
                        r.Left + lwidth + columns[i].Width - hscrollBar.Value,
                        r.Top + 2 + headerBuffer,
                        r.Left + lwidth + columns[i].Width - hscrollBar.Value,
                        r.Top + r.Height - 2);
                    lwidth += columns[i].Width;
                }

                while (lheight <= r.Height)
                {
                    g.DrawLine(p,
                        r.Left + 2,
                        r.Top + headerBuffer + itemheight + lheight - vscrollBar.Value,
                        r.Left + r.Width,
                        r.Top + headerBuffer + itemheight + lheight - vscrollBar.Value);
                    lheight += itemheight;
                }
            }
        }

        #endregion

        #region Helper Functions
        private int vsize, hsize;
        public override void AdjustScrollbars()
        {
            if (nodes.Count > 0 || columns.Count > 0 && !colScaleMode)
            {
                allColsWidth = 0;
                for (int i = 0; i < columns.Count; i++)
                {
                    allColsWidth += columns[i].Width;
                }

                allRowsHeight = 0;
                for (int i = 0; i < nodes.Count; i++)
                {
                    allRowsHeight += itemheight + itemheight * ((TreeListNode)nodes[i]).GetVisibleNodeCount(true);
                }

                vsize = vscrollBar.Width;
                hsize = hscrollBar.Height;

                hscrollBar.Left = this.ClientRectangle.Left + 2;
                hscrollBar.Width = this.ClientRectangle.Width - vsize - 4;
                hscrollBar.Top = this.ClientRectangle.Top + this.ClientRectangle.Height - hscrollBar.Height - 2;
                hscrollBar.Maximum = allColsWidth;
                if (allColsWidth > this.ClientRectangle.Width - 4 - vsize)
                {
                    hscrollBar.Show();
                    hsize = hscrollBar.Height;
                }
                else
                {
                    hscrollBar.Hide();
                    hscrollBar.Value = 0;
                    hsize = 0;
                }

                vscrollBar.Left = this.ClientRectangle.Left + this.ClientRectangle.Width - vscrollBar.Width - 2;
                vscrollBar.Top = this.ClientRectangle.Top + headerBuffer + 2;
                vscrollBar.Height = this.ClientRectangle.Height - hsize - headerBuffer - 4;
                vscrollBar.Maximum = allRowsHeight;
                vscrollBar.LargeChange = (this.ClientRectangle.Height - headerBuffer - hsize - 4 > 0 ? this.ClientRectangle.Height - headerBuffer - hsize - 4 : 0);
                if (allRowsHeight > this.ClientRectangle.Height - headerBuffer - 4 - hsize)
                {
                    vscrollBar.Show();
                    vsize = vscrollBar.Width;
                }
                else
                {
                    vscrollBar.Hide();
                    vscrollBar.Value = 0;
                    vsize = 0;
                }

                hscrollBar.Width = this.ClientRectangle.Width - vsize - 4;
                hscrollBar.LargeChange = (this.ClientRectangle.Width - vsize - 4 > 0 ? this.ClientRectangle.Width - vsize - 4 : 0);
                hscrollBar.Top = this.ClientRectangle.Top + this.ClientRectangle.Height - hscrollBar.Height - 2;
                if (allColsWidth > this.ClientRectangle.Width - 4 - vsize)
                {
                    hscrollBar.Show();
                }
                else
                {
                    hscrollBar.Hide();
                    hscrollBar.Value = 0;
                    hsize = 0;
                }
            }
        }

        private void UnfocusNodes(TreeListNodeCollection nodecol)
        {
            for (int i = 0; i < nodecol.Count; i++)
            {
                UnfocusNodes(nodecol[i].Nodes);
                nodecol[i].Focused = false;
            }
        }

        private void UnselectNodes(TreeListNodeCollection nodecol)
        {
            for (int i = 0; i < nodecol.Count; i++)
            {
                UnselectNodes(nodecol[i].Nodes);
                nodecol[i].Focused = false;
                nodecol[i].Selected = false;
            }
        }

        private TreeListNode NodeInNodeRow(MouseEventArgs e)
        {
            IEnumerator ek = nodeRowRects.Keys.GetEnumerator();
            IEnumerator ev = nodeRowRects.Values.GetEnumerator();

            while (ek.MoveNext() && ev.MoveNext())
            {
                Rectangle r = (Rectangle)ek.Current;

                if (r.Left <= e.X && r.Left + r.Width >= e.X
                    && r.Top <= e.Y && r.Top + r.Height >= e.Y)
                {
                    return (TreeListNode)ev.Current;
                }
            }
            return null;
        }

        private object FindMouseSubNode(MouseEventArgs e)
        {
            IEnumerator ek = SubNodeRects.Keys.GetEnumerator();
            IEnumerator ev = SubNodeRects.Values.GetEnumerator();

            while (ek.MoveNext() && ev.MoveNext())
            {
                Rectangle r = (Rectangle)ek.Current;

                if (r.Left <= e.X && r.Left + r.Width >= e.X
                    && r.Top <= e.Y && r.Top + r.Height >= e.Y)
                {
                    return ev.Current;
                }
            }
            return null;
        }

        private TreeListNode NodePlusClicked(MouseEventArgs e)
        {
            IEnumerator ek = pmRects.Keys.GetEnumerator();
            IEnumerator ev = pmRects.Values.GetEnumerator();

            while (ek.MoveNext() && ev.MoveNext())
            {
                Rectangle r = (Rectangle)ek.Current;

                if (r.Left <= e.X && r.Left + r.Width >= e.X
                    && r.Top <= e.Y && r.Top + r.Height >= e.Y)
                {
                    return (TreeListNode)ev.Current;
                }
            }

            return null;
        }

        private void RenderNodeRows(TreeListNode node, Graphics g, Rectangle r, int level, int index, ref int totalRend, ref int childCount, int count)
        {
            if (node.IsVisible)
            {
                int eb = 10;
                // 渲染可视行
                if (((r.Top + itemheight * totalRend + eb / 4 - vscrollBar.Value + itemheight > r.Top)
                    && (r.Top + itemheight * totalRend + eb / 4 - vscrollBar.Value < r.Top + r.Height)))
                {
                    rendcnt++;
                    int lb = indent * level;		// level buffer
                    int ib = 0;		// icon buffer
                    int hb = headerBuffer;	// header buffer	
                    var linePen = new Pen(SystemBrushes.ControlDark, 1.0f) { DashStyle = DashStyle.Dot };
                    if (showrootlines || showplusminus) eb += 10;


                    #region 画图片
                    if ((node.Selected || node.Focused) && stateImageList != null)
                    {
                        if (node.ImageIndex >= 0 && node.ImageIndex < stateImageList.Images.Count)
                        {
                            stateImageList.Draw(g, r.Left + lb + eb + 2 - hscrollBar.Value, r.Top + hb + itemheight * totalRend + eb / 4 - 2 - vscrollBar.Value, 16, 16, node.ImageIndex);
                            ib = 18;
                        }
                    }
                    else
                    {
                        if (smallImageList != null && node.ImageIndex >= 0 && node.ImageIndex < smallImageList.Images.Count)
                        {
                            smallImageList.Draw(g, r.Left + lb + eb + 2 - hscrollBar.Value, r.Top + hb + itemheight * totalRend + eb / 4 - 2 - vscrollBar.Value, 16, 16, node.ImageIndex);
                            ib = 18;
                        }
                    }
                    #endregion
                    #region 画背景
                    int last = 0;
                    if (columns.Count > 0 && node.BackColor != this.BackColor)	//	画整行背景或某行第一列背景
                    {
                        for (int i = 0; i < columns.Count; i++)
                        {
                            var rect = new Rectangle(
                                last + 2 - hscrollBar.Value,
                                r.Top + hb + itemheight * totalRend + 2 - vscrollBar.Value,
                                columns[i].Width - 1,
                                itemheight - 1);
                            g.Clip = new Region(rect);

                            if (node.UseItemStyleForSubItems)
                            {
                                g.FillRectangle(new SolidBrush(node.BackColor), rect);
                            }
                            else
                            {
                                g.FillRectangle(
                                    new SolidBrush(node.BackColor),
                                    2,
                                    r.Top + hb + itemheight * totalRend + 2 - vscrollBar.Value,
                                    columns[0].Width - 1,
                                    itemheight);
                                break;
                            }

                            last += columns[i].Width;
                        }
                    }
                    last = 0;
                    if (columns.Count > 0)		//	画某一各背景
                    {
                        for (int j = 0; j < node.SubItems.Count && j < columns.Count; j++)
                        {
                            last += columns[j].Width + 2;

                            var rect = new Rectangle(
                                last - hscrollBar.Value,
                                r.Top + headerBuffer + 2,
                                (last + columns[j + 1].Width > r.Width - 1 ? r.Width - 1 : columns[j + 1].Width - 1),
                                r.Height - 5);

                            if (node.SubItems[j].ItemControl != null) continue;
                            if (node.SubItems[j].BackColor == this.BackColor) continue;
                            g.Clip = new Region(rect);
                            g.FillRectangle(
                                new SolidBrush(node.SubItems[j].BackColor),
                                last - hscrollBar.Value,
                                r.Top + hb + itemheight * totalRend + 2 - vscrollBar.Value,
                                columns[j].Width - 1,
                                itemheight);
                        }

                        last = 0;
                        for (int i = 0; i < columns.Count; i++)
                        {

                            var rect = new Rectangle(
                                last - hscrollBar.Value,
                                r.Top + hb + itemheight * totalRend + 2 - vscrollBar.Value,
                                columns[i].Width - 1,
                                itemheight);
                            if (i == 0)
                            {
                                SubNodeRects.Add(rect, node);
                            }
                            else
                            {
                                SubNodeRects.Add(rect, node.SubItems[i - 1]);
                            }
                            last += columns[i].Width + 2;

                        }
                    }

                    #endregion
                    #region 画焦点
                    //Rectangle sr = new Rectangle(r.Left + lb + ib + eb + 4 - hscrollBar.Value, r.Top + hb + itemheight * totalRend + 2 - vscrollBar.Value, allColsWidth - (lb + ib + eb + 4), itemheight);
                    var sr = new Rectangle(
                        2,
                        r.Top + hb + itemheight * totalRend + 2 - vscrollBar.Value,
                        r.Width - 4,
                        itemheight);

                    nodeRowRects.Add(sr, node);
                    g.Clip = new Region(sr);
                    // render selection and focus
                    if (node.Selected && isFocused)
                    {
                        g.FillRectangle(new SolidBrush(rowSelectColor), sr);
                    }
                    else if (node.Selected && !isFocused && !hideSelection)
                    {
                        g.FillRectangle(SystemBrushes.Control, sr);
                    }
                    else if (node.Selected && !isFocused && hideSelection)
                    {
                        ControlPaint.DrawFocusRectangle(g, sr);
                    }

                    if (node.Focused && ((isFocused && multiSelect) || !node.Selected))
                    {
                        ControlPaint.DrawFocusRectangle(g, sr);
                    }
                    #endregion
                    g.Clip = new Region(new Rectangle(r.Left + 2 - hscrollBar.Value, r.Top + hb + 2, columns[0].Width, r.Height - hb - 4));
                    #region 画根连接线
                    // render root lines if visible
                    if (r.Left + eb - hscrollBar.Value > r.Left)
                    {
                        if (showrootlines && level == 0)
                        {
                            if (index == 0)
                            {
                                g.DrawLine(linePen, r.Left + eb / 2 - hscrollBar.Value, r.Top + eb / 2 + hb - vscrollBar.Value, r.Left + eb - hscrollBar.Value, r.Top + eb / 2 + hb - vscrollBar.Value);
                                g.DrawLine(linePen, r.Left + eb / 2 - hscrollBar.Value, r.Top + eb / 2 + hb - vscrollBar.Value, r.Left + eb / 2 - hscrollBar.Value, r.Top + eb + hb - vscrollBar.Value);
                            }
                            else if (index == count - 1)
                            {
                                g.DrawLine(linePen, r.Left + eb / 2 - hscrollBar.Value, r.Top + eb / 2 + hb + itemheight * (totalRend) - vscrollBar.Value, r.Left + eb - hscrollBar.Value, r.Top + eb / 2 + hb + itemheight * (totalRend) - vscrollBar.Value);
                                g.DrawLine(linePen, r.Left + eb / 2 - hscrollBar.Value, r.Top + hb + itemheight * (totalRend) - vscrollBar.Value, r.Left + eb / 2 - hscrollBar.Value, r.Top + eb / 2 + hb + itemheight * (totalRend) - vscrollBar.Value);
                            }
                            else
                            {
                                g.DrawLine(linePen, r.Left + eb / 2 - hscrollBar.Value, r.Top + eb + hb + itemheight * (totalRend) - eb / 2 - vscrollBar.Value, r.Left + eb - hscrollBar.Value, r.Top + eb + hb + itemheight * (totalRend) - eb / 2 - vscrollBar.Value);
                                g.DrawLine(linePen, r.Left + eb / 2 - hscrollBar.Value, r.Top + eb + hb + itemheight * (totalRend - 1) - vscrollBar.Value, r.Left + eb / 2 - hscrollBar.Value, r.Top + eb + hb + itemheight * (totalRend) - vscrollBar.Value);
                            }

                            if (childCount > 0)
                                g.DrawLine(linePen, r.Left + eb / 2 - hscrollBar.Value, r.Top + hb + itemheight * (totalRend - childCount) - vscrollBar.Value, r.Left + eb / 2 - hscrollBar.Value, r.Top + hb + itemheight * (totalRend) - vscrollBar.Value);
                        }
                    }
                    #endregion
                    #region 画子连接线
                    // render child lines if visible
                    if (r.Left + lb + eb - hscrollBar.Value > r.Left)
                    {
                        if (showlines && level > 0)
                        {
                            if (index == count - 1)
                            {
                                g.DrawLine(linePen, r.Left + lb + eb / 2 - hscrollBar.Value, r.Top + eb / 2 + hb + itemheight * (totalRend) - vscrollBar.Value, r.Left + lb + eb - hscrollBar.Value, r.Top + eb / 2 + hb + itemheight * (totalRend) - vscrollBar.Value);
                                g.DrawLine(linePen, r.Left + lb + eb / 2 - hscrollBar.Value, r.Top + hb + itemheight * (totalRend) - vscrollBar.Value, r.Left + lb + eb / 2 - hscrollBar.Value, r.Top + eb / 2 + hb + itemheight * (totalRend) - vscrollBar.Value);
                            }
                            else
                            {
                                g.DrawLine(linePen, r.Left + lb + eb / 2 - hscrollBar.Value, r.Top + eb / 2 + hb + itemheight * (totalRend) - vscrollBar.Value, r.Left + lb + eb - hscrollBar.Value, r.Top + eb / 2 + hb + itemheight * (totalRend) - vscrollBar.Value);
                                g.DrawLine(linePen, r.Left + lb + eb / 2 - hscrollBar.Value, r.Top + hb + itemheight * (totalRend) - vscrollBar.Value, r.Left + lb + eb / 2 - hscrollBar.Value, r.Top + eb + hb + itemheight * (totalRend) - vscrollBar.Value);
                            }

                            if (childCount > 0)
                                g.DrawLine(linePen, r.Left + lb + eb / 2 - hscrollBar.Value, r.Top + hb + itemheight * (totalRend - childCount) - vscrollBar.Value, r.Left + lb + eb / 2 - hscrollBar.Value, r.Top + hb + itemheight * (totalRend) - vscrollBar.Value);
                        }
                    }
                    #endregion
                    #region 画加减按钮
                    // render +/- signs if visible
                    if (r.Left + lb + eb / 2 + 5 - hscrollBar.Value > r.Left)
                    {
                        if (showplusminus && (node.GetNodeCount(false) > 0 || alwaysShowPM))
                        {
                            if (index == 0 && level == 0)
                            {
                                RenderPlus(g, r.Left + lb + eb / 2 - 4 - hscrollBar.Value, r.Top + hb + eb / 2 - 4 - vscrollBar.Value, 8, 8, node);
                            }
                            else if (index == count - 1)
                            {

                                RenderPlus(g, r.Left + lb + eb / 2 - 4 - hscrollBar.Value, r.Top + hb + itemheight * totalRend + eb / 2 - 4 - vscrollBar.Value, 8, 8, node);
                            }
                            else
                            {
                                RenderPlus(g, r.Left + lb + eb / 2 - 4 - hscrollBar.Value, r.Top + hb + itemheight * totalRend + eb / 2 - 4 - vscrollBar.Value, 8, 8, node);
                            }
                        }
                    }
                    #endregion
                    #region 画文本
                    if (r.Left + columns[0].Width - hscrollBar.Value > r.Left)
                    {
                        g.DrawString(
                            TruncatedString(node.Text, columns[0].Width, lb + eb + ib + 6, g),
                            Font,
                            node.Selected && isFocused ? SystemBrushes.HighlightText : new SolidBrush(node.ForeColor),
                            r.Left + lb + ib + eb + 4 - hscrollBar.Value,
                            r.Top + hb + itemheight * totalRend + eb / 4 - vscrollBar.Value);
                    }
                    #endregion
                    #region 画子项目
                    last = 0;
                    if (columns.Count > 0)
                    {
                        for (int j = 0; j < node.SubItems.Count && j < columns.Count; j++)
                        {
                            last += columns[j].Width;
                            g.Clip = new Region(new Rectangle(last + 6 - hscrollBar.Value, r.Top + headerBuffer + 2, (last + columns[j + 1].Width > r.Width - 6 ? r.Width - 6 : columns[j + 1].Width - 6), r.Height - 5));
                            if (node.SubItems[j].ItemControl != null)
                            {
                                Control c = node.SubItems[j].ItemControl;
                                c.Location = new Point(r.Left + last + 4 - hscrollBar.Value, r.Top + (itemheight * totalRend) + headerBuffer + 4 - vscrollBar.Value);
                                c.ClientSize = new Size(columns[j + 1].Width - 6, rowHeight - 4);
                                c.Parent = this;
                            }
                            else
                            {
                                string sp = TruncatedString(node.SubItems[j].Text, columns[j + 1].Width, 9, g);
                                int x = 0;
                                switch (columns[j + 1].TextAlign)
                                {
                                    case HorizontalAlignment.Left:
                                        x = last + 6 - hscrollBar.Value;
                                        break;
                                    case HorizontalAlignment.Right:
                                        x = last + columns[j + 1].Width - StringTools.MeasureDisplayStringWidth(g, sp, this.Font) - 4 - hscrollBar.Value;
                                        break;
                                    default:
                                        x = last + (columns[j + 1].Width / 2) - (StringTools.MeasureDisplayStringWidth(g, sp, this.Font) / 2) - hscrollBar.Value;
                                        break;
                                }
                                g.DrawString(
                                        TruncatedString(node.SubItems[j].Text, columns[j + 1].Width, 9, g),
                                        this.Font,
                                        node.Selected && isFocused
                                        ? SystemBrushes.HighlightText
                                        : (node.UseItemStyleForSubItems ? new SolidBrush(node.ForeColor) : SystemBrushes.WindowText),
                                        x,
                                        r.Top + (itemheight * totalRend) + headerBuffer + 4 - vscrollBar.Value);
                            }
                        }
                    }
                    #endregion
                }

                totalRend++;// 节点数
                #region 画子节点
                if (node.IsExpanded)// 画子项
                {
                    childCount = 0;
                    var counts = node.GetNodeCount(false);
                    for (int n = 0; n < counts; n++)
                    {
                        RenderNodeRows(node.Nodes[n], g, r, level + 1, n, ref totalRend, ref childCount, node.Nodes.Count);
                    }
                }
                #endregion
                childCount = node.GetVisibleNodeCount(true);
            }
            else childCount = 0;
        }

        private void RenderPlus(Graphics g, int x, int y, int w, int h, TreeListNode node)
        {
            if (VisualStyles)
            {
                g.DrawImage(node.IsExpanded ? bmpMinus : bmpPlus, x, y);
            }
            else
            {
                g.DrawRectangle(new Pen(SystemBrushes.ControlDark), x, y, w, h);
                g.FillRectangle(new SolidBrush(Color.White), x + 1, y + 1, w - 1, h - 1);
                g.DrawLine(new Pen(new SolidBrush(Color.Black)), x + 2, y + 4, x + w - 2, y + 4);
                if (!node.IsExpanded) g.DrawLine(new Pen(new SolidBrush(Color.Black)), x + 4, y + 2, x + 4, y + h - 2);
            }
            pmRects.Add(new Rectangle(x, y, w, h), node);
        }

        private void InvokeMouseEvent(MouseEventArgs e)
        {
            var inode = NodeInNodeRow(e);
            if (inode != null)
            {
                OnRowMouseEvent(inode, e);
            }
            var lnode = FindMouseSubNode(e);
            if (lnode != null)
            {
                if (lnode.GetType() == typeof(ContainerSubListViewItem))
                {
                    var n = lnode as ContainerSubListViewItem;
                    OnRowSubItemEvent(n, e);
                }
                else if (lnode.GetType() == typeof(TreeListNode))
                {
                    var ln = lnode as TreeListNode;
                    if (ln != null)
                    {
                        var n = new ContainerSubListViewItem(ln.Text, ln.BackColor);
                        OnRowSubItemEvent(n, e);
                    }
                }
            }
        }

        #endregion

        #region 公开方法
        /// <summary>
        /// 收缩所有
        /// </summary>
        public void CollapseAll()
        {
            foreach (TreeListNode node in nodes)
            {
                node.CollapseAll();
            }
            allCollapsed = true;
            AdjustScrollbars();
            Invalidate();
        }
        /// <summary>
        /// 展开所有
        /// </summary>
        public void ExpandAll()
        {
            foreach (TreeListNode node in nodes)
            {
                node.ExpandAll();
            }
            allCollapsed = false;
            AdjustScrollbars();
            Invalidate();
        }

        /// <summary>
        /// 未使用
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public TreeListNode GetNodeAt(int x, int y)
        {
            // To be added
            return null;
        }

        /// <summary>
        /// 未使用
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public TreeListNode GetNodeAt(Point pt)
        {
            // To be added
            return null;
        }

        /// <summary>
        /// 获取所有被选中的项目
        /// </summary>
        /// <param name="node">节点</param>
        /// <returns></returns>
        private TreeListNodeCollection GetSelectedNodes(TreeListNode node)
        {
            TreeListNodeCollection list = new TreeListNodeCollection();

            for (int i = 0; i < node.Nodes.Count; i++)
            {
                // check if current node is selected
                if (node.Nodes[i].Selected)
                {
                    list.Add(node.Nodes[i]);
                }

                // chech if node is expanded and has
                // selected children
                if (node.Nodes[i].IsExpanded)
                {
                    TreeListNodeCollection list2 = GetSelectedNodes(node.Nodes[i]);
                    for (int j = 0; j < list2.Count; j++)
                    {
                        list.Add(list2[i]);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 获取所有节点数 
        /// </summary>
        /// <returns></returns>
        public int GetNodeCount()
        {
            int c = 0;

            c += nodes.Count;
            foreach (TreeListNode node in nodes)
            {
                c += GetNodeCount(node);
            }

            return c;
        }
        /// <summary>
        /// 获取所有节点数
        /// </summary>
        /// <param name="node">节点</param>
        /// <returns></returns>
        public int GetNodeCount(TreeListNode node)
        {
            int c = 0;
            c += node.Nodes.Count;
            foreach (TreeListNode n in node.Nodes)
            {
                c += GetNodeCount(n);
            }

            return c;
        }
        #endregion
    }

    public interface IParentChildList
    {
        object ParentNode();

        object PreviousSibling();
        object NextSibling();

        object FirstChild();
        object NextChild();
        object PreviousChild();
        object LastChild();
    }
    #region TreeListNode
    [DesignTimeVisible(false), TypeConverter("YNLD.Forms.TreeListView.TreeListNodeConverter")]
    public class TreeListNode : IParentChildList
    {
        #region Event Handlers
        public event MouseEventHandler MouseDown;

        private void OnSubItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            subitems[e.IndexChanged].MouseDown += OnSubItemMouseDown;
        }

        public void OnSubItemMouseDown(object sender, MouseEventArgs e)
        {
            if (MouseDown != null) MouseDown(this, e);
        }

        public void OnSubNodeMouseDown(object sender, MouseEventArgs e)
        {
            if (MouseDown != null) MouseDown(sender, e);
        }
        #endregion

        #region Variables
        private Color backcolor = SystemColors.Window;
        private Rectangle bounds;
        private bool ischecked = false;
        private bool focused = false;
        private Font font, nodeFont;
        private Color forecolor = SystemColors.WindowText;
        private int imageindex = 0;
        private int stateimageindex = 0;
        private int index = 0;
        private CTreeListView treelistview;
        private bool selected = false;
        private ContainerSubListViewItemCollection subitems;
        private object tag;
        private string text;
        private bool styleall = true;
        private bool hovered = false;

        private TreeListNode curChild = null;
        private TreeListNodeCollection nodes;
        private string fullPath = "";
        private bool expanded = false;
        private bool visible = true;

        private TreeListNode parent;
        #endregion

        #region Constructor
        public TreeListNode()
        {
            subitems = new ContainerSubListViewItemCollection();
            subitems.ItemsChanged += new ItemsChangedEventHandler(OnSubItemsChanged);

            nodes = new TreeListNodeCollection();
            nodes.Owner = this;
            nodes.MouseDown += OnSubNodeMouseDown;
        }
        #endregion

        #region Properties
        [
        Category("Behavior"),
        Description("设置父节点.")
        ]
        public TreeListNode Parent
        {
            set { parent = value; }
        }

        [
        Category("Data"),
        Description("获取当前节点的子节点."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Editor(typeof(CollectionEditor), typeof(UITypeEditor))
        ]
        public TreeListNodeCollection Nodes
        {
            get { return nodes; }
        }
        /// <summary>
        /// 是否被展开
        /// </summary>
        [Category("Behavior"),
        Description("是否被展开."),
        DefaultValue(false)]
        public bool IsExpanded
        {
            get { return expanded; }
            set { expanded = value; }
        }

        /// <summary>
        /// 是否可视
        /// </summary>
        [Category("Behavior"),
        Description("是否可视"),
        DefaultValue(true)]
        public bool IsVisible
        {
            get { return visible; }
            set { visible = value; }
        }

        [Category("Behavior")]
        public string FullPath
        {
            get { return fullPath; }
        }

        [Category("Appearance"), Description("背景色")]
        public Color BackColor
        {
            get { return backcolor; }
            set { backcolor = value; }
        }

        /// <summary>
        /// 区域
        /// </summary>
        [Browsable(false), Description("区域")]
        public Rectangle Bounds
        {
            get { return bounds; }
        }

        [Category("Behavior"), Description("选择框是否被选中")]
        public bool Checked
        {
            get { return ischecked; }
            set { ischecked = value; }
        }

        [Browsable(false), Description("是否存在焦点")]
        public bool Focused
        {
            get { return focused; }
            set { focused = value; }
        }

        [Category("Appearance"), Description("字体")]
        public Font Font
        {
            get { return font; }
            set { font = value; }
        }

        [Category("Appearance"), Description("字体颜色")]
        public Color ForeColor
        {
            get { return forecolor; }
            set { forecolor = value; }
        }

        [Category("Behavior"), Description("图片索引")]
        public int ImageIndex
        {
            get { return imageindex; }
            set { imageindex = value; }
        }

        [Browsable(false), Description("项目索引")]
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        public CTreeListView TreeListView
        {
            get { return treelistview; }
        }

        /// <summary>
        /// 是否被选则
        /// </summary>
        [Browsable(false), Description("是否被选则")]
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }
        /// <summary>
        /// 获取层次
        /// </summary>
        [Browsable(false), Description("获取层次")]
        public int Level
        {
            get
            {
                return (parent == null ? 0 : parent.Level + 1);
            }
        }
        [
        Category("Behavior"),
        Description("子项"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Editor(typeof(CollectionEditor), typeof(UITypeEditor))
        ]
        public ContainerSubListViewItemCollection SubItems
        {
            get { return subitems; }
        }

        /// <summary>
        /// 状态图片索引
        /// </summary>
        [Category("Behavior")]
        public int StateImageIndex
        {
            get { return stateimageindex; }
            set { stateimageindex = value; }
        }

        /// <summary>
        /// 关联数据
        /// </summary>
        [Browsable(false)]
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        /// <summary>
        /// 文本
        /// </summary>
        [Category("Appearance")]
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// 是否为子项目使用风格
        /// </summary>
        [Category("Behavior")]
        public bool UseItemStyleForSubItems
        {
            get { return styleall; }
            set { styleall = value; }
        }

        [Browsable(false)]
        public bool Hovered
        {
            get { return hovered; }
            set { hovered = value; }
        }
        #endregion

        #region Methods
        /// <summary>
        /// 折叠
        /// </summary>
        public void Collapse()
        {
            expanded = false;
        }
        /// <summary>
        /// 折叠所有
        /// </summary>
        public void CollapseAll()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].CollapseAll();
            }
            Collapse();
        }
        /// <summary>
        /// 展开
        /// </summary>
        public void Expand()
        {
            expanded = true;
        }
        /// <summary>
        /// 展开所有
        /// </summary>
        public void ExpandAll()
        {
            for (int i = 0; i < nodes.Count; i++)
                ((TreeListNode)nodes[i]).ExpandAll();

            expanded = true;
        }
        /// <summary>
        /// 获取节点数
        /// </summary>
        /// <param name="includeSubTrees">是否获取包含的子节点</param>
        /// <returns></returns>
        public int GetNodeCount(bool includeSubTrees)
        {
            int c = 0;

            if (includeSubTrees)
            {
                for (int n = 0; n < nodes.Count; n++)
                {
                    c += nodes[n].GetNodeCount(true);
                }
            }
            c += nodes.Count;
            return c;
        }

        /// <summary>
        /// 获取可视节点数
        /// </summary>
        /// <param name="includeSubTrees">是否包含子节点</param>
        /// <returns></returns>
        public int GetVisibleNodeCount(bool includeSubTrees)
        {
            int c = 0;

            if (expanded)
            {
                if (includeSubTrees)
                {
                    for (int n = 0; n < nodes.Count; n++)
                    {
                        if (nodes[n].IsExpanded)
                            c += nodes[n].GetVisibleNodeCount(true);
                    }
                }

                for (int n = 0; n < nodes.Count; n++)
                {
                    if (nodes[n].IsVisible)
                        c++;
                }
            }

            return c;
        }

        /// <summary>
        /// 删除节点
        /// </summary>
        public void Remove()
        {
            int c = nodes.IndexOf(curChild);
            nodes.Remove(curChild);
            if (nodes.Count > 0 && nodes.Count > c)
                curChild = nodes[c];
            else
                curChild = nodes[nodes.Count];
        }

        /// <summary>
        /// 切换折叠状态
        /// </summary>
        public void Toggle()
        {
            expanded = !expanded;
        }

        #endregion

        #region IParentChildList
        /// <summary>
        /// 获取父节点
        /// </summary>
        /// <returns></returns>
        public object ParentNode()
        {
            return parent;
        }

        /// <summary>
        /// 获取上一同级节点
        /// </summary>
        /// <returns></returns>
        public object PreviousSibling()
        {
            if (parent != null)
            {
                int thisIndex = parent.Nodes[this];
                if (thisIndex > 0)
                    return parent.Nodes[thisIndex - 1];
            }

            return null;
        }

        /// <summary>
        /// 获取下一个同级节点
        /// </summary>
        /// <returns></returns>
        public object NextSibling()
        {
            if (parent != null)
            {
                int thisIndex = parent.Nodes[this];	//	获取到当前节点索引
                if (thisIndex < parent.Nodes.Count - 1)
                    return parent.Nodes[thisIndex + 1];
            }
            return null;
        }

        /// <summary>
        /// 获取第一个节点
        /// </summary>
        /// <returns></returns>
        public object FirstChild()
        {
            curChild = Nodes[0];
            return curChild;
        }

        /// <summary>
        /// 获取最后一个节点
        /// </summary>
        /// <returns></returns>
        public object LastChild()
        {
            curChild = Nodes[Nodes.Count - 1];
            return curChild;
        }

        public object NextChild()
        {
            curChild = (TreeListNode)curChild.NextSibling();
            return curChild;
        }

        public object PreviousChild()
        {
            curChild = (TreeListNode)curChild.PreviousSibling();
            return curChild;
        }
        #endregion
    }

    public class TreeListNodeCollection : CollectionBase
    {
        #region Events
        public event MouseEventHandler MouseDown;
        public event EventHandler NodesChanged;

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (MouseDown != null)
                MouseDown(sender, e);
        }

        private void OnNodesChanged()
        {
            OnNodesChanged(this, new EventArgs());
        }

        private void OnNodesChanged(object sender, EventArgs e)
        {
            if (NodesChanged != null)
                NodesChanged(sender, e);
        }
        #endregion

        #region Variables
        private TreeListNode owner;
        #endregion

        public TreeListNodeCollection()
        {
        }

        public TreeListNodeCollection(TreeListNode owner)
        {
            this.owner = owner;
        }

        public TreeListNode Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        /// <summary>
        /// 获取总节点数
        /// </summary>
        public int TotalCount
        {
            get
            {
                int tcnt = 0;
                tcnt += List.Count;
                foreach (TreeListNode n in List)
                {
                    tcnt += n.Nodes.TotalCount;
                }

                return tcnt;
            }
        }

        #region Implementation
        public TreeListNode this[int index]
        {
            get
            {
                if (List.Count > 0)
                {
                    return List[index] as TreeListNode;
                }
                else
                    return null;
            }
            set
            {
                List[index] = value;
                TreeListNode tln = ((TreeListNode)List[index]);
                tln.MouseDown += new MouseEventHandler(OnMouseDown);
                tln.Nodes.NodesChanged += new EventHandler(OnNodesChanged);
                tln.Parent = owner;
                OnNodesChanged();
            }
        }

        public int this[TreeListNode item]
        {
            get { return List.IndexOf(item); }
        }

        public int Add(TreeListNode item)
        {
            item.MouseDown += new MouseEventHandler(OnMouseDown);
            item.Nodes.NodesChanged += new EventHandler(OnNodesChanged);
            item.Parent = owner;
            OnNodesChanged();
            return item.Index = List.Add(item);
        }

        public void AddRange(TreeListNode[] items)
        {
            lock (List.SyncRoot)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    items[i].MouseDown += new MouseEventHandler(OnMouseDown);
                    items[i].Nodes.NodesChanged += new EventHandler(OnNodesChanged);
                    items[i].Parent = owner;
                    items[i].Index = List.Add(items[i]);
                }
                OnNodesChanged();
            }
        }

        public void Remove(TreeListNode item)
        {
            List.Remove(item);
        }

        public new void Clear()
        {
            for (int i = 0; i < List.Count; i++)
            {
                ContainerSubListViewItemCollection col = ((TreeListNode)List[i]).SubItems;
                for (int j = 0; j < col.Count; j++)
                {
                    if (col[j].ItemControl != null)
                    {
                        col[j].ItemControl.Parent = null;
                        col[j].ItemControl.Visible = false;
                        col[j].ItemControl = null;
                    }
                }
                ((TreeListNode)List[i]).Nodes.Clear();
            }
            List.Clear();
            OnNodesChanged();
        }

        public int IndexOf(TreeListNode item)
        {
            return List.IndexOf(item);
        }
        #endregion
    }
    #endregion

    #region Type Converters
    public class TreeListNodeConverter : TypeConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor) && value is TreeListNode)
            {
                TreeListNode tln = (TreeListNode)value;

                ConstructorInfo ci = typeof(TreeListNode).GetConstructor(new Type[] { });
                if (ci != null)
                {
                    return new InstanceDescriptor(ci, null, false);
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
    #endregion


    //*----------------------


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
    [DesignTimeVisible(false), TypeConverter("YNLD.Forms.TreeListView.ListViewItemConverter")]
    public class ContainerListViewItem : ICloneable
    {
        public event MouseEventHandler MouseDown;

        #region Variables
        private Color backcolor;
        private readonly Rectangle bounds;
        private bool ischecked;
        private bool focused;
        private Font font;
        private Color forecolor;
        private int imageindex;
        private int stateimageindex;
        private int index;
        private ContainerListView listview;
        private bool selected;
        private ContainerSubListViewItemCollection subitems;
        private object tag;
        private string text;
        private bool styleall;
        private bool hovered;
        #endregion

        #region Constructors

        public ContainerListViewItem()
        {
            this.bounds = new Rectangle();
            subitems = new ContainerSubListViewItemCollection();
            subitems.ItemsChanged += OnSubItemsChanged;
        }

        public ContainerListViewItem(Rectangle bounds)
        {
            this.bounds = bounds;
            subitems = new ContainerSubListViewItemCollection();
            subitems.ItemsChanged += OnSubItemsChanged;
        }

        public ContainerListViewItem(ContainerListView Listview, int Index, Rectangle bounds)
        {
            index = Index;
            this.bounds = bounds;
            listview = Listview;
        }
        #endregion

        private void OnSubItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            subitems[e.IndexChanged].MouseDown += new MouseEventHandler(OnSubItemMouseDown);
        }

        private void OnSubItemMouseDown(object sender, MouseEventArgs e)
        {
            if (MouseDown != null)
                MouseDown(this, e);
        }

        #region 属性
        /// <summary>
        /// 背景色
        /// </summary>
        public Color BackColor
        {
            get { return backcolor; }
            set { backcolor = value; }
        }

        /// <summary>
        /// 区域
        /// </summary>
        public Rectangle Bounds
        {
            get { return bounds; }
        }

        /// <summary>
        /// 是否被选中
        /// </summary>
        public bool Checked
        {
            get { return ischecked; }
            set { ischecked = value; }
        }

        /// <summary>
        /// 是否获取焦点
        /// </summary>
        public bool Focused
        {
            get { return focused; }
            set { focused = value; }
        }

        /// <summary>
        /// 字体
        /// </summary>
        public Font Font
        {
            get { return font; }
            set { font = value; }
        }

        /// <summary>
        /// 字体颜色
        /// </summary>
        public Color ForeColor
        {
            get { return forecolor; }
            set { forecolor = value; }
        }

        /// <summary>
        /// 图片索引
        /// </summary>
        public int ImageIndex
        {
            get { return imageindex; }
            set { imageindex = value; }
        }

        /// <summary>
        /// 项目索引
        /// </summary>
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        /// <summary>
        /// 拥有此项的列表
        /// </summary>
        public ContainerListView ListView
        {
            get { return listview; }
        }

        /// <summary>
        /// 是否被选中
        /// </summary>
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        [
        Category("Behavior"),
        Description("子控件的Items集合。"),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Editor(typeof(CollectionEditor), typeof(UITypeEditor))
        ]
        public ContainerSubListViewItemCollection SubItems
        {
            get { return subitems; }
        }

        /// <summary>
        /// 状态图片索引
        /// </summary>
        public int StateImageIndex
        {
            get { return stateimageindex; }
            set { stateimageindex = value; }
        }

        /// <summary>
        /// 关联数据
        /// </summary>
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        /// <summary>
        /// 文本
        /// </summary>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        /// <summary>
        /// 是否为子项目使用风格
        /// </summary>
        public bool UseItemStyleForSubItems
        {
            get { return styleall; }
            set { styleall = value; }
        }

        [Browsable(false)]
        public bool Hovered
        {
            get { return hovered; }
            set { hovered = value; }
        }
        #endregion

        #region Methods
        public object Clone()
        {
            var lvi = new ContainerListViewItem();
            lvi.BackColor = backcolor;
            lvi.Focused = focused;
            lvi.Font = font;
            lvi.ForeColor = forecolor;
            lvi.ImageIndex = imageindex;
            lvi.Selected = selected;
            lvi.Tag = tag;
            lvi.Text = text;
            lvi.UseItemStyleForSubItems = styleall;

            return lvi;
        }
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
                ((ContainerListViewItem)List[index]).MouseDown += OnMouseDown;
            }
        }

        public int this[ContainerListViewItem item]
        {
            get { return List.IndexOf(item); }
        }
        public int Add(ContainerListViewItem item)
        {
            item.MouseDown += new MouseEventHandler(OnMouseDown);
            return item.Index = List.Add(item);
        }

        public void AddRange(ContainerListViewItem[] items)
        {
            lock (List.SyncRoot)
            {
                for (int i = 0; i < items.Length; i++)
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
            foreach (object t in List)
            {
                var col = ((ContainerListViewItem)t).SubItems;
                for (int j = 0; j < col.Count; j++)
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
    [DesignTimeVisible(false), TypeConverter("YNLD.Forms.TreeListView.SubListViewItemConverter")]
    public class ContainerSubListViewItem : ICloneable
    {
        public event MouseEventHandler MouseDown;
        public Color BackColor { get; set; }
        protected void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (MouseDown != null)
                MouseDown(sender, e);
        }

        private string text;
        private Control childControl;



        public ContainerSubListViewItem()
        {
            text = "SubItem";
        }

        public ContainerSubListViewItem(Control control)
        {
            text = "";
            Construct(control);
        }

        public ContainerSubListViewItem(string str)
        {
            text = str;
            Construct(null);
        }

        public ContainerSubListViewItem(string str, Color c)
        {
            text = str;
            BackColor = c;
            Construct(null);
        }

        private void Construct(Control control)
        {
            childControl = control;

            if (childControl != null)
            {
                childControl.MouseDown += OnMouseDown;
            }

        }

        public Control ItemControl
        {
            get
            {
                return childControl;
            }
            set
            {
                childControl = value;
                if (childControl != null) childControl.MouseDown += OnMouseDown;
            }
        }

        public object Clone()
        {
            var slvi = new ContainerSubListViewItem { ItemControl = null, Text = text };
            return slvi;
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public override string ToString()
        {
            return (childControl == null ? text : childControl.ToString());
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
            int index = List.Add(item);
            OnItemsChanged(new ItemsChangedEventArgs(index));
            return index;
        }

        public ContainerSubListViewItem Add(Control control)
        {
            ContainerSubListViewItem slvi = new ContainerSubListViewItem(control);
            lock (List.SyncRoot)
                OnItemsChanged(new ItemsChangedEventArgs(List.Add(slvi)));
            return slvi;
        }

        public ContainerSubListViewItem Add(string str)
        {
            ContainerSubListViewItem slvi = new ContainerSubListViewItem(str);
            lock (List.SyncRoot)
                OnItemsChanged(new ItemsChangedEventArgs(List.Add(slvi)));
            return slvi;
        }

        /// <summary>
        /// 添加子项
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
            ContainerSubListViewItem slvi = new ContainerSubListViewItem(str, c);
            lock (List.SyncRoot)
                OnItemsChanged(new ItemsChangedEventArgs(List.Add(slvi)));
            return slvi;
        }

        public void AddRange(ContainerSubListViewItem[] items)
        {
            lock (List.SyncRoot)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    OnItemsChanged(new ItemsChangedEventArgs(List.Add(items[i])));
                }
            }
        }
        #endregion
    }
    #endregion


    #region Column Header classes
    [DesignTimeVisible(false), TypeConverter("YNLD.Forms.TreeListView.ToggleColumnHeaderConverter")]
    public class ToggleColumnHeader : ICloneable
    {
        // send an internal event when a column is resized
        internal event EventHandler WidthResized;
        private void OnWidthResized()
        {
            if (WidthResized != null)
                WidthResized(this, new EventArgs());
        }

        private int index;
        private ContainerListView listview;
        private string text;
        private HorizontalAlignment textAlign;
        private int width;
        private bool visible;
        private bool hovered;
        private bool pressed;
        private bool selected;
        private ColumnScaleStyle scaleStyle;
        private Bitmap image;

        public ToggleColumnHeader()
        {
            index = 0;
            listview = null;
            textAlign = HorizontalAlignment.Left;
            width = 90;
            visible = true;
            hovered = false;
            pressed = false;
            scaleStyle = ColumnScaleStyle.Slide;
        }


        [Browsable(false)]
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        [
        Category("Appearance"),
        Description("The image to display in this header.")
        ]
        public Bitmap Image
        {
            get { return image; }
            set { image = value; }
        }

        [Category("Behavior"), Description("Determines how a column reacts when another column is scaled.")]
        public ColumnScaleStyle ScaleStyle
        {
            get { return scaleStyle; }
            set { scaleStyle = value; }
        }

        [Category("Data"), Description("The index of this column in the collection.")]
        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        [Category("Data"), Description("The parent listview of this column header.")]
        public ContainerListView ListView
        {
            get { return listview; }
        }

        [Category("Appearance"), Description("The title of this column header.")]
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        [Category("Behavior"), Description("The alignment of the column headers title.")]
        public HorizontalAlignment TextAlign
        {
            get { return textAlign; }
            set { textAlign = value; }
        }

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
        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        [Browsable(false)]
        public bool Hovered
        {
            get { return hovered; }
            set { hovered = value; }
        }

        [Browsable(false)]
        public bool Pressed
        {
            get { return pressed; }
            set { pressed = value; }
        }

        public object Clone()
        {
            ToggleColumnHeader ch = new ToggleColumnHeader();
            ch.Index = index;
            ch.Text = text;
            ch.TextAlign = textAlign;
            ch.Width = width;

            return ch;
        }

        public override string ToString()
        {
            return text;
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
                ToggleColumnHeader tch = new ToggleColumnHeader();
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
                ((ToggleColumnHeader)List[index]).WidthResized += new EventHandler(OnWidthResized);
            }
        }

        public virtual int Add(ToggleColumnHeader colhead)
        {
            colhead.WidthResized += new EventHandler(OnWidthResized);
            return colhead.Index = List.Add(colhead);
        }

        public virtual ToggleColumnHeader Add(string str, int width, HorizontalAlignment textAlign)
        {
            ToggleColumnHeader tch = new ToggleColumnHeader();
            tch.Text = str;
            tch.Width = width;
            tch.TextAlign = textAlign;
            tch.WidthResized += new EventHandler(OnWidthResized);

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
                for (int i = 0; i < items.Length; i++)
                {
                    items[i].WidthResized += new EventHandler(OnWidthResized);
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
    [DefaultProperty("Items")]
    public class ContainerListView : Control
    {
        #region Events
        public event LabelEditEventHandler AfterLabelEdit;
        public event LabelEditEventHandler BeforeLabelEdit;
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

            ContainerListViewItem item = ((ContainerListViewItem)sender);

            if (multiSelectMode == MultiSelectMode.Single)
            {
                selectedIndices.Clear();
                selectedItems.Clear();

                for (int i = 0; i < items.Count; i++)
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
                for (int i = 0; i < items.Count; i++)
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

            Invalidate(this.ClientRectangle);
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

        protected void PopMenu(System.Windows.Forms.ContextMenu theMenu, MouseEventArgs e)
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
        protected bool allowColumnReorder = false;
        protected BorderStyle borderstyle = BorderStyle.Fixed3D;
        private int borderWid = 2;
        protected ColumnHeaderCollection columns;
        protected ColumnHeaderStyle headerStyle = ColumnHeaderStyle.Nonclickable;
        protected int headerBuffer = 20;

        protected bool hideSelection = true;
        protected bool hoverSelection = false;
        protected bool multiSelect = false;
        protected MultiSelectMode multiSelectMode = MultiSelectMode.Single;

        protected ContainerListViewItemCollection items;
        protected bool labelEdit = false;
        protected ImageList smallImageList, stateImageList;
        protected Comparer sortComparer;
        protected bool scrollable = true;
        protected SortOrder sorting;
        protected string text;
        protected ContainerListViewItem topItem;

        protected ContainerListViewItemCollection selectedItems = null;
        protected ArrayList selectedIndices = null;

        protected bool visualStyles = false;

        protected ContainerListViewItem focusedItem;
        protected int focusedIndex = -1;
        protected bool isFocused = false;

        protected Rectangle headerRect;
        protected Rectangle[] columnRects;
        protected Rectangle[] columnSizeRects;
        protected int lastColHovered = -1;
        protected int lastColPressed = -1;
        protected int lastColSelected = -1;
        protected bool doColTracking = false;
        protected Color colTrackColor = Color.WhiteSmoke;
        protected Color colSortColor = Color.Gainsboro;
        protected int allColsWidth = 0;
        protected bool colScaleMode = false;
        protected int colScalePos = 0;
        protected int colScaleWid = 0;
        protected int scaledCol = -1;

        protected Rectangle rowsRect;
        protected Rectangle[] rowRects;
        protected int lastRowHovered = -1;
        protected int rowHeight = 18;
        protected int allRowsHeight = 0;
        protected bool doRowTracking = true;
        protected Color rowTrackColor = Color.WhiteSmoke;
        protected Color rowSelectColor = SystemColors.Highlight;
        protected bool fullRowSelect = true;
        protected int firstSelected = -1, lastSelected = -1;

        protected bool gridLines = false;
        protected Color gridLineColor = Color.WhiteSmoke;

        protected Point lastClickedPoint;

        protected bool captureFocusClick = false;

        protected ContextMenu headerMenu, itemMenu, contextMenu;
        /// <summary>
        /// 水平滚动条
        /// </summary>
        protected HScrollBar hscrollBar;
        /// <summary>
        /// 垂直滚动条
        /// </summary>
        protected VScrollBar vscrollBar;

        protected bool ensureVisible = true;
        #endregion

        #region 构造函数
        public ContainerListView()
        {
            Construct();
        }

        private void Construct()
        {
            rowHeight = 18;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
                ControlStyles.Opaque | ControlStyles.UserPaint | ControlStyles.DoubleBuffer |
                ControlStyles.Selectable | ControlStyles.UserMouse, true);

            this.BackColor = SystemColors.Window;

            columns = new ColumnHeaderCollection();
            items = new ContainerListViewItemCollection();
            selectedIndices = new ArrayList();
            selectedItems = new ContainerListViewItemCollection();

            hscrollBar = new HScrollBar { Parent = this, Minimum = 0, Maximum = 0, SmallChange = 10 };
            hscrollBar.Hide();

            vscrollBar = new VScrollBar { Parent = this, Minimum = 0, Maximum = 0, SmallChange = rowHeight };
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
        Editor(typeof(CollectionEditor), typeof(UITypeEditor))
        ]
        public ColumnHeaderCollection Columns
        {
            get { return columns; }
        }

        [
        Category("Data"),
        Description("项的集合."),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        Editor(typeof(CollectionEditor), typeof(UITypeEditor))
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
            get
            {
                return activation;
            }
            set
            {
                activation = value;
            }
        }

        [
        Category("Behavior"),
        Description("指定列标题是否能被重新排序."),
        DefaultValue(false)
        ]
        public bool AllowColumnReorder
        {
            get
            {
                return allowColumnReorder;
            }
            set
            {
                allowColumnReorder = value;
            }
        }

        [
        Category("Appearance"),
        Description("样式边框样式."),
        DefaultValue(BorderStyle.FixedSingle)
        ]
        public BorderStyle BorderStyle
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
                Invalidate(this.ClientRectangle);
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
                Invalidate(this.ClientRectangle);
                headerBuffer = headerStyle == ColumnHeaderStyle.None ? 0 : 20;
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
        DefaultValue(typeof(Color), "Color.WhiteSmoke")
        ]
        public Color ColumnTrackColor
        {
            get { return colTrackColor; }
            set { colTrackColor = value; }
        }

        [
        Category("Appearance"),
        Description("指定用于当前选定的排序列的颜色."),
        DefaultValue(typeof(Color), "Color.Gainsboro")
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
        DefaultValue(typeof(Color), "Color.WhiteSmoke")
        ]
        public Color RowTrackColor
        {
            get { return rowTrackColor; }
            set { rowTrackColor = value; }
        }

        [
        Category("Appearance"),
        Description("指定用于选定行的颜色."),
        DefaultValue(typeof(Color), "SystemColors.Highlight")
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
        DefaultValue(typeof(Color), "Color.WhiteSmoke")
        ]
        public Color GridLineColor
        {
            get { return gridLineColor; }
            set { gridLineColor = value; }
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
                Invalidate(this.ClientRectangle);
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
                Invalidate(this.ClientRectangle);
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
            set
            {
                visualStyles = value;
            }
        }

        /// <summary>
        /// 当前被选中的项目索引数组
        /// </summary>
        [Browsable(false)]
        public ArrayList SelectedIndices
        {
            get { return selectedIndices; }
        }

        /// <summary>
        /// 当前被选中的子项
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
                    Keys keyData = ((Keys)(int)msg.WParam) | ModifierKeys;
                    Keys keyCode = ((Keys)(int)msg.WParam);

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

                            Invalidate(this.ClientRectangle);
                        }

                        return true;
                    }
                    else if (keyData == Keys.Up)
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

                            Invalidate(this.ClientRectangle);
                        }

                        return true;
                    }
                }
            }

            return base.PreProcessMessage(ref msg);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle r = ClientRectangle;
            Graphics g = e.Graphics;

            DrawBackground(g, r);
            DrawRows(g, r);
            DrawHeaders(g, r);
            DrawExtra(g, r);
            DrawBorder(g, r);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GenerateHeaderRect();
            GenerateRowsRect();
            AdjustScrollbars();

            // invalidate subitem controls
            for (int i = 0; i < items.Count; i++)
            {
                ContainerListViewItem lvi = ((ContainerListViewItem)items[i]);
                for (int j = 0; j < lvi.SubItems.Count; j++)
                {
                    ContainerSubListViewItem slvi = lvi.SubItems[j];
                    if (slvi.ItemControl != null)
                        slvi.ItemControl.Invalidate(slvi.ItemControl.ClientRectangle);
                }
            }
            Invalidate();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == (int)WM.WM_GETDLGCODE)
            {
                m.Result = new IntPtr((int)DLGC.DLGC_WANTCHARS | (int)DLGC.DLGC_WANTARROWS | m.Result.ToInt32());
            }
        }

        private int mouseMoveTicks = 0;
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
                this.Focus();

                if (!captureFocusClick)
                    return;
            }

            lastClickedPoint = new Point(e.X, e.Y);

            // determine if a header was pressed
            if (headerStyle != ColumnHeaderStyle.None)
            {
                if (MouseInRect(e, headerRect) && e.Button == MouseButtons.Left)
                {
                    for (int i = 0; i < columns.Count; i++)
                    {
                        columns[i].Pressed = false;
                        if (MouseInRect(e, columnSizeRects[i]) && items.Count > 0)
                        {
                            if (columns[i].ScaleStyle == ColumnScaleStyle.Slide)
                            {
                                // autosize column
                                if (e.Clicks == 2 && e.Button == MouseButtons.Left && items.Count > 0)
                                {
                                    int mwid = 0;
                                    int twid = 0;
                                    for (int j = 0; j < items.Count; j++)
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
            bool selChange = false;
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

                        for (int i = 0; i < items.Count; i++)
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
                        for (int i = 0; i < items.Count; i++)
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
                                    for (int j = 0; j < items.Count; j++)
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
                                    for (int j = 0; j < items.Count; j++)
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
                                    for (int j = 0; j < items.Count; j++)
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
                        for (int i = 0; i < items.Count; i++)
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
                    return;
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
                if (MouseInRect(e, columnRects[lastColPressed]) && !MouseInRect(e, columnSizeRects[lastColPressed]) && e.Button == MouseButtons.Left)
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
                    vscrollBar.Value = (vscrollBar.Value - vscrollBar.SmallChange * (e.Delta / 100) < 0 ? 0 : vscrollBar.Value - vscrollBar.SmallChange * (e.Delta / 100));
                else if (hscrollBar.Visible)
                    hscrollBar.Value = (hscrollBar.Value - hscrollBar.SmallChange * (e.Delta / 100) < 0 ? 0 : hscrollBar.Value - hscrollBar.SmallChange * (e.Delta / 100));
            }
            else if (e.Delta < 0)
            {
                if (vscrollBar.Visible)
                    vscrollBar.Value = (vscrollBar.Value - vscrollBar.SmallChange * (e.Delta / 100) > vscrollBar.Maximum - vscrollBar.LargeChange ? vscrollBar.Maximum - vscrollBar.LargeChange : vscrollBar.Value - vscrollBar.SmallChange * (e.Delta / 100));
                else if (hscrollBar.Visible)
                    hscrollBar.Value = (hscrollBar.Value - hscrollBar.SmallChange * (e.Delta / 100) > hscrollBar.Maximum - hscrollBar.LargeChange ? hscrollBar.Maximum - hscrollBar.LargeChange : hscrollBar.Value - hscrollBar.SmallChange * (e.Delta / 100));
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
                    vscrollBar.Value = (vscrollBar.LargeChange > vscrollBar.Value ? 0 : vscrollBar.Value - vscrollBar.LargeChange);
                else if (hscrollBar.Visible)
                    hscrollBar.Value = (hscrollBar.LargeChange > hscrollBar.Value ? 0 : hscrollBar.Value - hscrollBar.LargeChange);
            }
            else if (e.KeyCode == Keys.PageDown)
            {
                if (vscrollBar.Visible)
                    vscrollBar.Value = (vscrollBar.Value + vscrollBar.LargeChange > vscrollBar.Maximum - vscrollBar.LargeChange ? vscrollBar.Maximum - vscrollBar.LargeChange : vscrollBar.Value + vscrollBar.LargeChange);
                else if (hscrollBar.Visible)
                    hscrollBar.Value = (hscrollBar.Value + hscrollBar.LargeChange > hscrollBar.Maximum - hscrollBar.LargeChange ? hscrollBar.Maximum - hscrollBar.LargeChange : hscrollBar.Value + hscrollBar.LargeChange);
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
            Invalidate(this.ClientRectangle);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            isFocused = false;
            Invalidate(this.ClientRectangle);
        }
        #endregion

        #region Helper Functions
        // wireing of child control events
        protected virtual void Attach()
        {
            items.MouseDown += new MouseEventHandler(OnSubControlMouseDown);
            columns.WidthResized += new EventHandler(OnColumnWidthResize);

            hscrollBar.ValueChanged += new EventHandler(OnScroll);
            vscrollBar.ValueChanged += new EventHandler(OnScroll);
        }

        protected virtual void Detach()
        {
            items.MouseDown -= new MouseEventHandler(OnSubControlMouseDown);
            columns.WidthResized -= new EventHandler(OnColumnWidthResize);

            hscrollBar.ValueChanged -= new EventHandler(OnScroll);
            vscrollBar.ValueChanged -= new EventHandler(OnScroll);
        }

        // Rectangle and region generation functions
        protected void GenerateColumnRects()
        {
            columnRects = new Rectangle[columns.Count];
            columnSizeRects = new Rectangle[columns.Count];
            int lwidth = 2 - hscrollBar.Value;
            int colwid = 0;
            allColsWidth = 0;

            CalcSpringWids(ClientRectangle);
            for (int i = 0; i < columns.Count; i++)
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
            headerRect = new Rectangle(this.ClientRectangle.Left + 2 - hscrollBar.Value, this.ClientRectangle.Top + 2, this.ClientRectangle.Width - 4, 20);
        }

        protected void GenerateRowsRect()
        {
            rowsRect = new Rectangle(this.ClientRectangle.Left + 2 - hscrollBar.Value, this.ClientRectangle.Top + (headerStyle == ColumnHeaderStyle.None ? 2 : 22), this.ClientRectangle.Width - 4, this.ClientRectangle.Height - (headerStyle == ColumnHeaderStyle.None ? 2 : 22));
        }

        protected void GenerateRowRects()
        {
            rowRects = new Rectangle[items.Count];
            int lheight = 2 + headerBuffer - vscrollBar.Value;
            int lftpos = ClientRectangle.Left + 2;
            allRowsHeight = items.Count * rowHeight;
            for (int i = 0; i < items.Count; i++)
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
                for (int i = 0; i < columns.Count; i++)
                {
                    allColsWidth += columns[i].Width;
                }

                allRowsHeight = items.Count * rowHeight;

                vsize = vscrollBar.Width;
                hsize = hscrollBar.Height;

                hscrollBar.Left = this.ClientRectangle.Left + 2;
                hscrollBar.Width = this.ClientRectangle.Width - vsize - 4;
                hscrollBar.Top = this.ClientRectangle.Top + this.ClientRectangle.Height - hscrollBar.Height - 2;
                hscrollBar.Maximum = allColsWidth;
                hscrollBar.LargeChange = (this.ClientRectangle.Width - vsize - 4 > 0 ? this.ClientRectangle.Width - vsize - 4 : 0);
                if (allColsWidth > this.ClientRectangle.Width - 4 - vsize)
                    hscrollBar.Show();
                else
                {
                    hscrollBar.Hide();
                    hsize = 0;
                    hscrollBar.Value = 0;
                }

                vscrollBar.Left = this.ClientRectangle.Left + this.ClientRectangle.Width - vscrollBar.Width - 2;
                vscrollBar.Top = this.ClientRectangle.Top + headerBuffer + 2;
                vscrollBar.Height = this.ClientRectangle.Height - hsize - headerBuffer - 4;
                vscrollBar.Maximum = allRowsHeight;
                vscrollBar.LargeChange = (this.ClientRectangle.Height - headerBuffer - hsize - 4 > 0 ? this.ClientRectangle.Height - headerBuffer - hsize - 4 : 0);
                if (allRowsHeight > this.ClientRectangle.Height - headerBuffer - 4 - hsize)
                    vscrollBar.Show();
                else
                {
                    vscrollBar.Hide();
                    vsize = 0;
                    vscrollBar.Value = 0;
                }

                hscrollBar.Width = this.ClientRectangle.Width - vsize - 4;
                hscrollBar.Top = this.ClientRectangle.Top + this.ClientRectangle.Height - hscrollBar.Height - 2;
                hscrollBar.LargeChange = (this.ClientRectangle.Width - vsize - 4 > 0 ? this.ClientRectangle.Width - vsize - 4 : 0);
                if (allColsWidth > this.ClientRectangle.Width - 4 - vsize)
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
                ContainerListViewItem item = items[focusedIndex];
                if (item != null && item.Focused && item.Selected)
                {
                    Rectangle r = ClientRectangle;
                    int i = items.IndexOf(item);
                    int pos = r.Top + (rowHeight * i) + headerBuffer + 2 - vscrollBar.Value;
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
            Graphics g = Graphics.FromImage(new Bitmap(32, 32));
            SizeF strSize = g.MeasureString(s, this.Font);
            return (int)strSize.Width;
        }
        protected string TruncatedString(string s, int width, int offset, Graphics g)
        {
            string sprint = "";
            int swid;
            int i;
            SizeF strSize;

            try
            {
                strSize = g.MeasureString(s, this.Font);
                swid = ((int)strSize.Width);
                i = s.Length;

                for (i = s.Length; i > 0 && swid > width - offset; i--)
                {
                    strSize = g.MeasureString(s.Substring(0, i), this.Font);
                    swid = ((int)strSize.Width);
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
        private int springWid = 0;
        private int springCnt = 0;
        private void CalcSpringWids(Rectangle r)
        {
            springCnt = 0;
            springWid = (r.Width - borderWid * 2);
            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].ScaleStyle == ColumnScaleStyle.Slide)
                    springWid -= columns[i].Width;
                else
                    springCnt++;
            }

            if (springCnt > 0 && springWid > 0)
                springWid = springWid / springCnt;
        }

        protected virtual void DrawBorder(Graphics g, Rectangle r)
        {
            // if running in XP with styles
            if (VisualStyles)
            {
                DrawBorderStyled(g, r);
                return;
            }

            Rectangle rect = this.ClientRectangle;
            if (borderstyle == BorderStyle.FixedSingle)
            {
                g.DrawRectangle(SystemPens.ControlDarkDark, r.Left, r.Top, r.Width, r.Height);
            }
            else if (borderstyle == BorderStyle.Fixed3D)
            {
                ControlPaint.DrawBorder3D(g, r.Left, r.Top, r.Width, r.Height, Border3DStyle.Sunken);
            }
            else if (borderstyle == BorderStyle.None)
            {
                // do not render any border
            }
        }

        protected virtual void DrawBackground(Graphics g, Rectangle r)
        {
            int i;
            int lwidth = 2, lheight = 1;

            g.FillRectangle(new SolidBrush(BackColor), r);

            // 表头被单击时绘画
            if (headerStyle == ColumnHeaderStyle.Clickable)
            {
                for (i = 0; i < columns.Count; i++)
                {
                    if (columns[i].Selected)
                    {
                        g.FillRectangle(new SolidBrush(colSortColor), r.Left + lwidth - hscrollBar.Value, r.Top + 2 + headerBuffer, columns[i].Width, r.Height - 4 - headerBuffer);
                        break;
                    }
                    lwidth += columns[i].Width;
                }
            }

            // 表头跟踪绘画
            if (doColTracking && (lastColHovered >= 0 && lastColHovered < columns.Count))
            {
                g.FillRectangle(new SolidBrush(colTrackColor), columnRects[lastColHovered].Left, 22, columnRects[lastColHovered].Width, r.Height - 22);
            }

            // 行跟踪绘画
            if (doRowTracking && (lastRowHovered >= 0 && lastRowHovered < items.Count))
            {
                g.FillRectangle(new SolidBrush(rowTrackColor), r.Left + 2, rowRects[lastRowHovered].Top, r.Left + r.Width - 4, rowHeight);
            }

            // 花表格线
            if (gridLines)
            {
                Pen p = new Pen(new SolidBrush(gridLineColor), 1.0f);
                lwidth = lheight = 1;

                // vertical
                for (i = 0; i < columns.Count; i++)
                {
                    if (r.Left + lwidth + columns[i].Width >= r.Left + r.Width - 2)
                        break;

                    g.DrawLine(p, r.Left + lwidth + columns[i].Width - hscrollBar.Value, r.Top + 2 + headerBuffer, r.Left + lwidth + columns[i].Width - hscrollBar.Value, r.Top + r.Height - 2);
                    lwidth += columns[i].Width;
                }


                // horizontal
                for (i = 0; i < items.Count; i++)
                {
                    g.DrawLine(p, r.Left + 2, r.Top + headerBuffer + rowHeight + lheight - vscrollBar.Value, r.Left + r.Width, r.Top + headerBuffer + rowHeight + lheight - vscrollBar.Value);
                    lheight += rowHeight;
                }
            }
        }

        protected virtual void DrawHeaders(Graphics g, Rectangle r)
        {
            // if running in XP with styles
            if (VisualStyles)
            {
                DrawHeadersStyled(g, r);
                return;
            }

            if (headerStyle != ColumnHeaderStyle.None)
            {
                g.FillRectangle(new SolidBrush(SystemColors.Control), r.Left + 2, r.Top + 2, r.Width - 2, headerBuffer);

                CalcSpringWids(r);

                // render column headers and trailing column header
                int last = 2;
                int i;

                int lp_scr = r.Left - hscrollBar.Value;

                g.Clip = new Region(new Rectangle(r.Left + 2, r.Top + 2, r.Width - 5, r.Top + headerBuffer));
                for (i = 0; i < columns.Count; i++)
                {
                    if ((lp_scr + last + columns[i].Width > r.Left + 2)
                        && (lp_scr + last < r.Left + r.Width - 2))
                    {
                        if (headerStyle == ColumnHeaderStyle.Clickable && columns[i].Pressed)
                            System.Windows.Forms.ControlPaint.DrawButton(g, lp_scr + last, r.Top + 2, columns[i].Width, r.Top + headerBuffer, ButtonState.Flat);
                        else
                            System.Windows.Forms.ControlPaint.DrawButton(g, lp_scr + last, r.Top + 2, columns[i].Width, r.Top + headerBuffer, ButtonState.Normal);

                        if (columns[i].Image != null)
                        {
                            g.DrawImage(columns[i].Image, new Rectangle(lp_scr + last + 4, r.Top + 3, 16, 16));
                            g.DrawString(TruncatedString(columns[i].Text, columns[i].Width, 25, g), this.Font, SystemBrushes.ControlText, (float)(lp_scr + last + 22), (float)(r.Top + 5));
                        }
                        else
                        {
                            string sp = "";
                            if (columns[i].TextAlign == HorizontalAlignment.Left)
                                g.DrawString(TruncatedString(columns[i].Text, columns[i].Width, 0, g), this.Font, SystemBrushes.ControlText, (float)(lp_scr + last + 4), (float)(r.Top + 5));
                            else if (columns[i].TextAlign == HorizontalAlignment.Right)
                            {
                                sp = TruncatedString(columns[i].Text, columns[i].Width, 0, g);
                                g.DrawString(sp, this.Font, SystemBrushes.ControlText, (float)(lp_scr + last + columns[i].Width - StringTools.MeasureDisplayStringWidth(g, sp, this.Font) - 4), (float)(r.Top + 5));
                            }
                            else
                            {
                                sp = TruncatedString(columns[i].Text, columns[i].Width, 0, g);
                                g.DrawString(sp, this.Font, SystemBrushes.ControlText, (float)(lp_scr + last + (columns[i].Width / 2) - (StringTools.MeasureDisplayStringWidth(g, sp, this.Font) / 2)), (float)(r.Top + 5));
                            }
                        }
                    }
                    last += columns[i].Width;
                }

                // only render trailing column header if the end of the
                // last column ends before the boundary of the listview 
                if (!(lp_scr + last + 5 > r.Left + r.Width))
                {
                    g.Clip = new Region(new Rectangle(r.Left + 2, r.Top + 2, r.Width - 5, r.Top + headerBuffer));
                    System.Windows.Forms.ControlPaint.DrawButton(g, lp_scr + last, r.Top + 2, r.Width - (r.Left + last) - 3 + hscrollBar.Value, r.Top + headerBuffer, ButtonState.Normal);
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
                int lp_scr = r.Left + 2 - hscrollBar.Value;	// left viewport position less scrollbar position
                int lp = r.Left + 2;						// left viewport position
                int tp_scr = r.Top + 2 + headerBuffer - vscrollBar.Value;	// top viewport position less scrollbar position
                int tp = r.Top + 2 + headerBuffer;			// top viewport position

                for (i = 0; i < items.Count; i++)
                {
                    j = 0;
                    last = 0;

                    // render item, but only if its within the viewport
                    if ((tp_scr + (rowHeight * i) + 2 > r.Top + 2)
                        && (tp_scr + (rowHeight * i) + 2 < r.Top + r.Height - 2 - hsize))
                    {
                        g.Clip = new Region(new Rectangle(r.Left + 2, r.Top + headerBuffer + 2, r.Width - vsize - 5, r.Height - hsize - 5));

                        int rowSelWidth = (allColsWidth < (r.Width - 5) || hscrollBar.Visible ? allColsWidth : r.Width - 5);
                        if (!fullRowSelect)
                            rowSelWidth = columns[0].Width - 2;

                        // render selected item highlights
                        if (items[i].Selected && isFocused)
                        {
                            g.FillRectangle(new SolidBrush(rowSelectColor), lp, tp_scr + (rowHeight * i), rowSelWidth, rowHeight);
                        }
                        else if (items[i].Selected && !isFocused && hideSelection)
                        {
                            ControlPaint.DrawFocusRectangle(g, new Rectangle(lp_scr, tp_scr + (rowHeight * i), rowSelWidth, rowHeight));
                        }
                        else if (items[i].Selected && !isFocused && !hideSelection)
                        {
                            g.FillRectangle(SystemBrushes.Control, lp, tp_scr + (rowHeight * i), rowSelWidth, rowHeight);
                        }

                        if (items[i].Focused && multiSelect && isFocused)
                        {
                            ControlPaint.DrawFocusRectangle(g, new Rectangle(lp_scr, tp_scr + (rowHeight * i), rowSelWidth, rowHeight));
                        }

                        // render item
                        if ((lp_scr + 2 + columns[j].Width > r.Left + 4))
                        {
                            g.Clip = new Region(new Rectangle(lp + 2, tp, (columns[j].Width > r.Width ? r.Width - 6 : columns[j].Width - 2), r.Height - hsize - 5));

                            if (smallImageList != null && (items[i].ImageIndex >= 0 && items[i].ImageIndex < smallImageList.Images.Count))
                            {
                                smallImageList.Draw(g, lp_scr + 4, tp_scr + (rowHeight * i) + 1, 16, 16, items[i].ImageIndex);
                                g.DrawString(TruncatedString(items[i].Text, columns[j].Width, 18, g), this.Font, (items[i].Selected && isFocused ? SystemBrushes.HighlightText : new SolidBrush(ForeColor)), (float)(lp_scr + 22), (float)(tp_scr + (rowHeight * i) + 2));
                            }
                            else
                            {
                                g.DrawString(TruncatedString(items[i].Text, columns[j].Width, 0, g), this.Font, (items[i].Selected && isFocused ? SystemBrushes.HighlightText : new SolidBrush(ForeColor)), (float)(lp_scr + 4), (float)(tp_scr + (rowHeight * i) + 2));
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
                                && (tp_scr + (rowHeight * i) + 2 > r.Top + 2)
                                && (tp_scr + (rowHeight * i) + 2 < r.Top + r.Height - 2 - hsize))
                            {
                                g.Clip = new Region(new Rectangle(lp_scr + last + 4, tp, (last + columns[j + 1].Width > r.Width - 6 ? r.Width - 6 : columns[j + 1].Width - 6), r.Height - hsize - 5));
                                if (items[i].SubItems[j].ItemControl != null)
                                {
                                    Control c = items[i].SubItems[j].ItemControl;
                                    c.Location = new Point(lp_scr + last + 2, tp_scr + (rowHeight * i) + 2);
                                    c.ClientSize = new Size(columns[j + 1].Width - 6, rowHeight - 4);
                                    c.Parent = this;
                                }
                                else
                                {
                                    string sp = "";
                                    if (columns[j + 1].TextAlign == HorizontalAlignment.Left)
                                    {
                                        g.DrawString(TruncatedString(items[i].SubItems[j].Text, columns[j + 1].Width, 12, g), this.Font, (items[i].Selected && isFocused ? SystemBrushes.HighlightText : SystemBrushes.WindowText), (float)(lp_scr + last + 4), (float)(tp_scr + (rowHeight * i) + 2));
                                    }
                                    else if (columns[j + 1].TextAlign == HorizontalAlignment.Right)
                                    {
                                        sp = TruncatedString(items[i].SubItems[j].Text, columns[j + 1].Width, 12, g);
                                        g.DrawString(sp, this.Font, (items[i].Selected && isFocused ? SystemBrushes.HighlightText : new SolidBrush(ForeColor)), (float)(lp_scr + last + columns[j + 1].Width - StringTools.MeasureDisplayStringWidth(g, sp, this.Font) - 2), (float)(tp_scr + (rowHeight * i) + 2));
                                    }
                                    else
                                    {
                                        sp = TruncatedString(items[i].SubItems[j].Text, columns[j + 1].Width, 12, g);
                                        g.DrawString(sp, this.Font, (items[i].Selected && isFocused ? SystemBrushes.HighlightText : new SolidBrush(ForeColor)), (float)(lp_scr + last + (columns[j + 1].Width / 2) - (StringTools.MeasureDisplayStringWidth(g, sp, this.Font) / 2)), (float)(tp_scr + (rowHeight * i) + 2));
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
                g.FillRectangle(SystemBrushes.Control, r.Width - vscrollBar.Width - borderWid, r.Height - hscrollBar.Height - borderWid, vscrollBar.Width, hscrollBar.Height);
            }
        }
        // visual styles rendering functions
        protected virtual void DrawBorderStyled(Graphics g, Rectangle r)
        {
            Region oldreg = g.Clip;
            g.Clip = new Region(r);
            g.DrawRectangle(new Pen(SystemBrushes.InactiveBorder), r.Left, r.Top, r.Width - 1, r.Height - 1);
            g.DrawRectangle(new Pen(BackColor), r.Left + 1, r.Top + 1, r.Width - 3, r.Height - 3);
            g.Clip = oldreg;
        }

        protected virtual void DrawHeadersStyled(Graphics g, Rectangle r)
        {
            if (headerStyle != ColumnHeaderStyle.None)
            {
                int colwid = 0;
                int i;
                int last = 2;
                CalcSpringWids(r);

                int lp_scr = r.Left - hscrollBar.Value;
                int lp = r.Left;
                int tp = r.Top + 2;

                System.IntPtr hdc = g.GetHdc();
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
                        Wrapper.DrawBackground("HEADER", "HEADERITEM", "NORMAL", hdc, lp_scr + last, tp, r.Width - last - 2 + hscrollBar.Value, headerBuffer, r.Left, r.Top, r.Width, headerBuffer);
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
                    g.Clip = new Region(new Rectangle(lp_scr + last + 2, tp, (r.Left + last + colwid > r.Left + r.Width ? (r.Width - (r.Left + last)) - 4 : colwid - 2) + hscrollBar.Value, r.Top + headerBuffer));
                    if (columns[i].Image != null)
                    {
                        g.DrawImage(columns[i].Image, new Rectangle(lp_scr + last + 4, r.Top + 3, 16, 16));
                        g.DrawString(TruncatedString(columns[i].Text, colwid, 25, g), this.Font, SystemBrushes.ControlText, (float)(r.Left + last + 22 - hscrollBar.Value), (float)(r.Top + 5));
                    }
                    else
                    {
                        string sp = TruncatedString(columns[i].Text, colwid, 0, g);
                        if (columns[i].TextAlign == HorizontalAlignment.Left)
                        {
                            g.DrawString(TruncatedString(columns[i].Text, colwid, 0, g), this.Font, SystemBrushes.ControlText, (float)(last + 4 - hscrollBar.Value), (float)(r.Top + 5));
                        }
                        else if (columns[i].TextAlign == HorizontalAlignment.Right)
                        {
                            g.DrawString(sp, this.Font, SystemBrushes.ControlText, (float)(last + colwid - StringTools.MeasureDisplayStringWidth(g, sp, this.Font) - 4 - hscrollBar.Value), (float)(r.Top + 5));
                        }
                        else
                        {
                            g.DrawString(sp, this.Font, SystemBrushes.ControlText, (float)(last + (colwid / 2) - (StringTools.MeasureDisplayStringWidth(g, sp, this.Font) / 2) - hscrollBar.Value), (float)(r.Top + 5));
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
            if (destinationType == typeof(InstanceDescriptor))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor) && value is ToggleColumnHeader)
            {
                ToggleColumnHeader lvi = (ToggleColumnHeader)value;

                ConstructorInfo ci = typeof(ToggleColumnHeader).GetConstructor(new Type[] { });
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
            if (destinationType == typeof(InstanceDescriptor))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor) && value is ContainerListViewItem)
            {
                ContainerListViewItem lvi = (ContainerListViewItem)value;

                ConstructorInfo ci = typeof(ContainerListViewItem).GetConstructor(new Type[] { });
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
            if (destinationType == typeof(InstanceDescriptor))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor) && value is ContainerSubListViewItem)
            {
                ContainerSubListViewItem lvi = (ContainerSubListViewItem)value;

                ConstructorInfo ci = typeof(ContainerSubListViewItem).GetConstructor(new Type[] { });
                if (ci != null)
                {
                    return new InstanceDescriptor(ci, null, false);
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
    #endregion

    public class StringTools
    {
        private StringTools() { }

        static public System.Drawing.SizeF MeasureDisplayString(System.Drawing.Graphics graphics, string text, System.Drawing.Font font)
        {
            const int width = 32;

            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(width, 1, graphics);
            System.Drawing.SizeF size = graphics.MeasureString(text, font);
            System.Drawing.Graphics anagra = System.Drawing.Graphics.FromImage(bitmap);

            int measured_width = (int)size.Width;

            if (anagra != null)
            {
                anagra.Clear(System.Drawing.Color.White);
                anagra.DrawString(text + "|", font, System.Drawing.Brushes.Black, width - measured_width, -font.Height / 2);

                for (int i = width - 1; i >= 0; i--)
                {
                    measured_width--;
                    if (bitmap.GetPixel(i, 0).R == 0)
                    {
                        break;
                    }
                }
            }

            return new System.Drawing.SizeF(measured_width, size.Height);
        }

        static public int MeasureDisplayStringWidth(System.Drawing.Graphics graphics, string text, System.Drawing.Font font)
        {
            return (int)MeasureDisplayString(graphics, text, font).Width;
        }
    }

    public class Wrapper
    {
        private Wrapper() { }

        [DllImport("OPaC.uxTheme.Win32.dll", CharSet = CharSet.Unicode, EntryPoint = "_Wrapper_IsAppThemed@0")]
        public static extern bool IsAppThemed();

        [DllImport("OPaC.uxTheme.Win32.dll", CharSet = CharSet.Unicode, EntryPoint = "_Wrapper_DrawBackground@48")]
        public static extern bool DrawBackground(string name, string part, string state, System.IntPtr hdc, int ox, int oy, int dx, int dy, int clip_ox, int clip_oy, int clip_dx, int clip_dy);

        [DllImport("OPaC.uxTheme.Win32.dll", CharSet = CharSet.Unicode, EntryPoint = "_Wrapper_DrawTabPageBackground@36")]
        public static extern bool DrawTabPageBackground(System.IntPtr hdc, int ox, int oy, int dx, int dy, int clip_ox, int clip_oy, int clip_dx, int clip_dy);

        [DllImport("OPaC.uxTheme.Win32.dll", CharSet = CharSet.Unicode, EntryPoint = "_Wrapper_DrawGroupBoxBackground@36")]
        public static extern bool DrawGroupBoxBackground(System.IntPtr hdc, int ox, int oy, int dx, int dy, int clip_ox, int clip_oy, int clip_dx, int clip_dy);

        [DllImport("OPaC.uxTheme.Win32.dll", CharSet = CharSet.Unicode, EntryPoint = "_Wrapper_DrawThemeParentBackground@8")]
        public static extern bool DrawThemeParentBackground(System.IntPtr hwnd, System.IntPtr hdc);

        [DllImport("OPaC.uxTheme.Win32.dll", CharSet = CharSet.Unicode, EntryPoint = "_Wrapper_DrawThemeParentBackgroundRect@24")]
        public static extern bool DrawThemeParentBackground(System.IntPtr hwnd, System.IntPtr hdc, int ox, int oy, int dx, int dy);

        [DllImport("OPaC.uxTheme.Win32.dll", CharSet = CharSet.Unicode, EntryPoint = "_Wrapper_GetTextColor@24")]
        public static extern bool GetTextColor(string name, string part, string state, out int r, out int g, out int b);

        [DllImport("OPaC.uxTheme.Win32.Dll", CharSet = CharSet.Unicode, EntryPoint = "_Wrapper_DrawThemeEdge@48")]
        public static extern bool DrawThemeEdge(string name, string part, string state, System.IntPtr hdc, int ox, int oy, int dx, int dy, int clip_ox, int clip_oy, int clip_dx, int clip_dy);
    }
}
