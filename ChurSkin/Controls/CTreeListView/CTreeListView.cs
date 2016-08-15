using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Reflection; 
using ChurSkins.Properties;

namespace System.Windows.Forms
{
    public delegate void RowMouseEventHandler(object sender, MouseEventArgs e);

    public delegate void RowSubItemEventHandler(object sender, MouseEventArgs e);

    #region CTreeListView

    /// <summary>
    ///     TreeListView provides a hybrid listview whos first
    ///     column can behave as a treeview. This control extends
    ///     ContainerListView, allowing subitems to contain
    ///     controls.
    /// </summary>
    public class CTreeListView : CListView
    {
        #region Constructor

        public CTreeListView()
        {
            MouseActivte = false;
            virtualParent = new TreeListNode();

            nodes = virtualParent.Nodes;
            nodes.Owner = virtualParent;
            nodes.MouseDown += OnSubControlMouseDown;
            nodes.NodesChanged += OnNodesChanged;

            selectedNodes = new TreeListNodeCollection();

            nodeRowRects = new ListDictionary();
            pmRects = new ListDictionary();
            SubNodeRects = new ListDictionary();

            var myAssembly = Assembly.GetAssembly(typeof (CTreeListView));
            //Stream bitmapStream1 = myAssembly.GetManifestResourceStream("ChurSkins.Resources.tv_minus.bmp");
            //bmpMinus = new Bitmap(bitmapStream1);
            bmpMinus = Resources.expan;
            //Stream bitmapStream2 = myAssembly.GetManifestResourceStream("ChurSkins.Resources.tv_plus.bmp");
            //bmpPlus = new Bitmap(bitmapStream2);
            bmpPlus = Resources.unexpan;
        }

        #endregion

        #region Events

        public RowMouseEventHandler RowMouseEvent;

        public RowSubItemEventHandler RowSubItemEventHandler;

        protected override void OnSubControlMouseDown(object sender, MouseEventArgs e)
        {
            var node = (TreeListNode) sender;

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
        ///     某行鼠标事件
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
        protected bool showlines, showrootlines, showplusminus = true;

        protected ListDictionary pmRects;
        protected ListDictionary nodeRowRects;
        protected ListDictionary SubNodeRects;

        protected bool alwaysShowPM;

        protected Bitmap bmpMinus, bmpPlus;

        private TreeListNode curNode;
        private readonly TreeListNode virtualParent;

        private TreeListNodeCollection selectedNodes;

        private bool allCollapsed;

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
        public bool MouseActivte { get; set; }

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
            Editor(typeof (CollectionEditor), typeof (UITypeEditor))
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
            var keyCode = ((Keys) (int) msg.WParam);

            if (keyCode == Keys.Left)
            {
                if (curNode.IsExpanded)
                {
                    curNode.Collapse();
                }
                else if (curNode.ParentNode() != null)
                {
                    var t = (TreeListNode) curNode.ParentNode();
                    if (t.ParentNode() != null)
                    {
                        curNode.Selected = false;
                        curNode.Focused = false;
                        curNode = (TreeListNode) curNode.ParentNode();
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
                    curNode = (TreeListNode) curNode.FirstChild();
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
                    var t = (TreeListNode) curNode.ParentNode();
                    if (t.ParentNode() != null)
                    {
                        curNode.Selected = false;
                        curNode.Focused = false;
                        curNode = (TreeListNode) curNode.ParentNode();
                        curNode.Selected = true;
                        curNode.Focused = true;
                    }

                    Invalidate();
                    //return true;
                }
                if (curNode.PreviousSibling() != null)
                {
                    var t = (TreeListNode) curNode.PreviousSibling();
                    if (t.GetNodeCount(false) > 0 && t.IsExpanded)
                    {
                        do
                        {
                            t = (TreeListNode) t.LastChild();
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
                        curNode = (TreeListNode) curNode.PreviousSibling();
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
                    curNode = (TreeListNode) curNode.FirstChild();
                    curNode.Selected = true;
                    curNode.Focused = true;
                }
                else if (curNode.NextSibling() == null && curNode.ParentNode() != null)
                {
                    var t = curNode;
                    do
                    {
                        t = (TreeListNode) t.ParentNode();
                        if (t.NextSibling() != null)
                        {
                            curNode.Selected = false;
                            curNode.Focused = false;
                            curNode = (TreeListNode) t.NextSibling();
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
                    curNode = (TreeListNode) curNode.NextSibling();
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
            for (var j = 0; j < nodes.Count; j++)
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

            for (var i = 0; i < columns.Count; i++)
            {
                if (columnSizeRects.Length > 0 && MouseInRect(e, columnSizeRects[i]))
                {
                    // autosize column
                    if (e.Clicks == 2 && e.Button == MouseButtons.Left)
                    {
                        var mwid = 0;
                        var twid = 0;

                        AutoSetColWidth(nodes, ref mwid, ref twid, i);

                        twid = GetStringWidth(columns[i].Text);
                        if (twid > mwid) mwid = twid;
                        mwid += 5;
                        if (columns[i].Image != null) mwid += 18;
                        columns[i].Width = mwid;
                        GenerateColumnRects();
                    } // scale column
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
                            if (e.Clicks == 2 && !MouseActivte)
                            {
                                cnode.Toggle();
                                AdjustScrollbars();
                            }
                            else if (e.Clicks == 2 && MouseActivte)
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

                            if (e.Clicks == 2 && !MouseActivte)
                            {
                                cnode.Toggle();
                                AdjustScrollbars();
                            }
                            else if (e.Clicks == 2 && MouseActivte)
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

        private int rendcnt;

        protected override void DrawRows(Graphics g, Rectangle r)
        {
            // render listview item rows
            int i;
            int totalRend = 0, childCount = 0;

            var maxrend = ClientRectangle.Height/itemheight + 1;
            var prior = vscrollBar.Value/itemheight - 1;
            if (prior < 0) prior = 0;
            var nodesprior = 0;
            var rootstart = 0;

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
                for (var i = 0; i < columns.Count; i++)
                {
                    allColsWidth += columns[i].Width;
                }

                allRowsHeight = 0;
                for (var i = 0; i < nodes.Count; i++)
                {
                    allRowsHeight += itemheight + itemheight*nodes[i].GetVisibleNodeCount(true);
                }

                vsize = vscrollBar.Width;
                hsize = hscrollBar.Height;

                hscrollBar.Left = ClientRectangle.Left + 2;
                hscrollBar.Width = ClientRectangle.Width - vsize - 4;
                hscrollBar.Top = ClientRectangle.Top + ClientRectangle.Height - hscrollBar.Height - 2;
                hscrollBar.Maximum = allColsWidth;
                if (allColsWidth > ClientRectangle.Width - 4 - vsize)
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

                vscrollBar.Left = ClientRectangle.Left + ClientRectangle.Width - vscrollBar.Width - 2;
                vscrollBar.Top = ClientRectangle.Top + headerBuffer + 2;
                vscrollBar.Height = ClientRectangle.Height - hsize - headerBuffer - 4;
                vscrollBar.Maximum = allRowsHeight;
                vscrollBar.LargeChange = (ClientRectangle.Height - headerBuffer - hsize - 4 > 0
                    ? ClientRectangle.Height - headerBuffer - hsize - 4
                    : 0);
                if (allRowsHeight > ClientRectangle.Height - headerBuffer - 4 - hsize)
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

                hscrollBar.Width = ClientRectangle.Width - vsize - 4;
                hscrollBar.LargeChange = (ClientRectangle.Width - vsize - 4 > 0 ? ClientRectangle.Width - vsize - 4 : 0);
                hscrollBar.Top = ClientRectangle.Top + ClientRectangle.Height - hscrollBar.Height - 2;
                if (allColsWidth > ClientRectangle.Width - 4 - vsize)
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
            for (var i = 0; i < nodecol.Count; i++)
            {
                UnfocusNodes(nodecol[i].Nodes);
                nodecol[i].Focused = false;
            }
        }

        private void UnselectNodes(TreeListNodeCollection nodecol)
        {
            for (var i = 0; i < nodecol.Count; i++)
            {
                UnselectNodes(nodecol[i].Nodes);
                nodecol[i].Focused = false;
                nodecol[i].Selected = false;
            }
        }

        private TreeListNode NodeInNodeRow(MouseEventArgs e)
        {
            var ek = nodeRowRects.Keys.GetEnumerator();
            var ev = nodeRowRects.Values.GetEnumerator();

            while (ek.MoveNext() && ev.MoveNext())
            {
                var r = (Rectangle) ek.Current;

                if (r.Left <= e.X && r.Left + r.Width >= e.X
                    && r.Top <= e.Y && r.Top + r.Height >= e.Y)
                {
                    return (TreeListNode) ev.Current;
                }
            }
            return null;
        }

        private object FindMouseSubNode(MouseEventArgs e)
        {
            var ek = SubNodeRects.Keys.GetEnumerator();
            var ev = SubNodeRects.Values.GetEnumerator();

            while (ek.MoveNext() && ev.MoveNext())
            {
                var r = (Rectangle) ek.Current;

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
            var ek = pmRects.Keys.GetEnumerator();
            var ev = pmRects.Values.GetEnumerator();

            while (ek.MoveNext() && ev.MoveNext())
            {
                var r = (Rectangle) ek.Current;

                if (r.Left <= e.X && r.Left + r.Width >= e.X
                    && r.Top <= e.Y && r.Top + r.Height >= e.Y)
                {
                    return (TreeListNode) ev.Current;
                }
            }

            return null;
        }

        private void RenderNodeRows(TreeListNode node, Graphics g, Rectangle r, int level, int index, ref int totalRend,
            ref int childCount, int count)
        {
            if (node.IsVisible)
            {
                var eb = 10;
                // 渲染可视行
                if (((r.Top + itemheight*totalRend + eb/4 - vscrollBar.Value + itemheight > r.Top)
                     && (r.Top + itemheight*totalRend + eb/4 - vscrollBar.Value < r.Top + r.Height)))
                {
                    rendcnt++;
                    var lb = indent*level; // level buffer
                    var ib = 0; // icon buffer
                    var hb = headerBuffer; // header buffer	
                    var linePen = new Pen(SystemBrushes.ControlDark, 1.0f) {DashStyle = DashStyle.Dot};
                    if (showrootlines || showplusminus) eb += 10;

                    #region 画图片

                    if ((node.Selected || node.Focused) && stateImageList != null)
                    {
                        if (node.ImageIndex >= 0 && node.ImageIndex < stateImageList.Images.Count)
                        {
                            stateImageList.Draw(g, r.Left + lb + eb + 2 - hscrollBar.Value,
                                r.Top + hb + itemheight*totalRend + eb/4 - 2 - vscrollBar.Value, 16, 16, node.ImageIndex);
                            ib = 18;
                        }
                    }
                    else
                    {
                        if (smallImageList != null && node.ImageIndex >= 0 &&
                            node.ImageIndex < smallImageList.Images.Count)
                        {
                            smallImageList.Draw(g, r.Left + lb + eb + 2 - hscrollBar.Value,
                                r.Top + hb + itemheight*totalRend + eb/4 - 2 - vscrollBar.Value, 16, 16, node.ImageIndex);
                            ib = 18;
                        }
                    }

                    #endregion

                    #region 画背景

                    var last = 0;
                    if (columns.Count > 0 && node.BackColor != BackColor) //	画整行背景或某行第一列背景
                    {
                        for (var i = 0; i < columns.Count; i++)
                        {
                            var rect = new Rectangle(
                                last + 2 - hscrollBar.Value,
                                r.Top + hb + itemheight*totalRend + 2 - vscrollBar.Value,
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
                                    r.Top + hb + itemheight*totalRend + 2 - vscrollBar.Value,
                                    columns[0].Width - 1,
                                    itemheight);
                                break;
                            }

                            last += columns[i].Width;
                        }
                    }
                    last = 0;
                    if (columns.Count > 0) //	画某一各背景
                    {
                        for (var j = 0; j < node.SubItems.Count && j < columns.Count; j++)
                        {
                            last += columns[j].Width + 2;

                            var rect = new Rectangle(
                                last - hscrollBar.Value,
                                r.Top + headerBuffer + 2,
                                (last + columns[j + 1].Width > r.Width - 1 ? r.Width - 1 : columns[j + 1].Width - 1),
                                r.Height - 5);

                            if (node.SubItems[j].ItemControl != null) continue;
                            if (node.SubItems[j].BackColor == BackColor) continue;
                            g.Clip = new Region(rect);
                            g.FillRectangle(
                                new SolidBrush(node.SubItems[j].BackColor),
                                last - hscrollBar.Value,
                                r.Top + hb + itemheight*totalRend + 2 - vscrollBar.Value,
                                columns[j].Width - 1,
                                itemheight);
                        }

                        last = 0;
                        for (var i = 0; i < columns.Count; i++)
                        {
                            var rect = new Rectangle(
                                last - hscrollBar.Value,
                                r.Top + hb + itemheight*totalRend + 2 - vscrollBar.Value,
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
                        r.Top + hb + itemheight*totalRend + 2 - vscrollBar.Value,
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

                    g.Clip =
                        new Region(new Rectangle(r.Left + 2 - hscrollBar.Value, r.Top + hb + 2, columns[0].Width,
                            r.Height - hb - 4));

                    #region 画根连接线

                    // render root lines if visible
                    if (r.Left + eb - hscrollBar.Value > r.Left)
                    {
                        if (showrootlines && level == 0)
                        {
                            if (index == 0)
                            {
                                g.DrawLine(linePen, r.Left + eb/2 - hscrollBar.Value,
                                    r.Top + eb/2 + hb - vscrollBar.Value, r.Left + eb - hscrollBar.Value,
                                    r.Top + eb/2 + hb - vscrollBar.Value);
                                g.DrawLine(linePen, r.Left + eb/2 - hscrollBar.Value,
                                    r.Top + eb/2 + hb - vscrollBar.Value, r.Left + eb/2 - hscrollBar.Value,
                                    r.Top + eb + hb - vscrollBar.Value);
                            }
                            else if (index == count - 1)
                            {
                                g.DrawLine(linePen, r.Left + eb/2 - hscrollBar.Value,
                                    r.Top + eb/2 + hb + itemheight*(totalRend) - vscrollBar.Value,
                                    r.Left + eb - hscrollBar.Value,
                                    r.Top + eb/2 + hb + itemheight*(totalRend) - vscrollBar.Value);
                                g.DrawLine(linePen, r.Left + eb/2 - hscrollBar.Value,
                                    r.Top + hb + itemheight*(totalRend) - vscrollBar.Value,
                                    r.Left + eb/2 - hscrollBar.Value,
                                    r.Top + eb/2 + hb + itemheight*(totalRend) - vscrollBar.Value);
                            }
                            else
                            {
                                g.DrawLine(linePen, r.Left + eb/2 - hscrollBar.Value,
                                    r.Top + eb + hb + itemheight*(totalRend) - eb/2 - vscrollBar.Value,
                                    r.Left + eb - hscrollBar.Value,
                                    r.Top + eb + hb + itemheight*(totalRend) - eb/2 - vscrollBar.Value);
                                g.DrawLine(linePen, r.Left + eb/2 - hscrollBar.Value,
                                    r.Top + eb + hb + itemheight*(totalRend - 1) - vscrollBar.Value,
                                    r.Left + eb/2 - hscrollBar.Value,
                                    r.Top + eb + hb + itemheight*(totalRend) - vscrollBar.Value);
                            }

                            if (childCount > 0)
                                g.DrawLine(linePen, r.Left + eb/2 - hscrollBar.Value,
                                    r.Top + hb + itemheight*(totalRend - childCount) - vscrollBar.Value,
                                    r.Left + eb/2 - hscrollBar.Value,
                                    r.Top + hb + itemheight*(totalRend) - vscrollBar.Value);
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
                                g.DrawLine(linePen, r.Left + lb + eb/2 - hscrollBar.Value,
                                    r.Top + eb/2 + hb + itemheight*(totalRend) - vscrollBar.Value,
                                    r.Left + lb + eb - hscrollBar.Value,
                                    r.Top + eb/2 + hb + itemheight*(totalRend) - vscrollBar.Value);
                                g.DrawLine(linePen, r.Left + lb + eb/2 - hscrollBar.Value,
                                    r.Top + hb + itemheight*(totalRend) - vscrollBar.Value,
                                    r.Left + lb + eb/2 - hscrollBar.Value,
                                    r.Top + eb/2 + hb + itemheight*(totalRend) - vscrollBar.Value);
                            }
                            else
                            {
                                g.DrawLine(linePen, r.Left + lb + eb/2 - hscrollBar.Value,
                                    r.Top + eb/2 + hb + itemheight*(totalRend) - vscrollBar.Value,
                                    r.Left + lb + eb - hscrollBar.Value,
                                    r.Top + eb/2 + hb + itemheight*(totalRend) - vscrollBar.Value);
                                g.DrawLine(linePen, r.Left + lb + eb/2 - hscrollBar.Value,
                                    r.Top + hb + itemheight*(totalRend) - vscrollBar.Value,
                                    r.Left + lb + eb/2 - hscrollBar.Value,
                                    r.Top + eb + hb + itemheight*(totalRend) - vscrollBar.Value);
                            }

                            if (childCount > 0)
                                g.DrawLine(linePen, r.Left + lb + eb/2 - hscrollBar.Value,
                                    r.Top + hb + itemheight*(totalRend - childCount) - vscrollBar.Value,
                                    r.Left + lb + eb/2 - hscrollBar.Value,
                                    r.Top + hb + itemheight*(totalRend) - vscrollBar.Value);
                        }
                    }

                    #endregion

                    #region 画加减按钮

                    // render +/- signs if visible
                    if (r.Left + lb + eb/2 + 5 - hscrollBar.Value > r.Left)
                    {
                        if (showplusminus && (node.GetNodeCount(false) > 0 || alwaysShowPM))
                        {
                            if (index == 0 && level == 0)
                            {
                                RenderPlus(g, r.Left + lb + eb/2 - 4 - hscrollBar.Value,
                                    r.Top + hb + eb/2 - 4 - vscrollBar.Value, 8, 8, node);
                            }
                            else if (index == count - 1)
                            {
                                RenderPlus(g, r.Left + lb + eb/2 - 4 - hscrollBar.Value,
                                    r.Top + hb + itemheight*totalRend + eb/2 - 4 - vscrollBar.Value, 8, 8, node);
                            }
                            else
                            {
                                RenderPlus(g, r.Left + lb + eb/2 - 4 - hscrollBar.Value,
                                    r.Top + hb + itemheight*totalRend + eb/2 - 4 - vscrollBar.Value, 8, 8, node);
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
                            r.Top + hb + itemheight*totalRend + eb/4 - vscrollBar.Value);
                    }

                    #endregion

                    #region 画子项目

                    last = 0;
                    if (columns.Count > 0)
                    {
                        for (var j = 0; j < node.SubItems.Count && j < columns.Count; j++)
                        {
                            last += columns[j].Width;
                            g.Clip =
                                new Region(new Rectangle(last + 6 - hscrollBar.Value, r.Top + headerBuffer + 2,
                                    (last + columns[j + 1].Width > r.Width - 6 ? r.Width - 6 : columns[j + 1].Width - 6),
                                    r.Height - 5));
                            if (node.SubItems[j].ItemControl != null)
                            {
                                var c = node.SubItems[j].ItemControl;
                                c.Location = new Point(r.Left + last + 4 - hscrollBar.Value,
                                    r.Top + (itemheight*totalRend) + headerBuffer + 4 - vscrollBar.Value);
                                c.ClientSize = new Size(columns[j + 1].Width - 6, rowHeight - 4);
                                c.Parent = this;
                            }
                            else
                            {
                                var sp = TruncatedString(node.SubItems[j].Text, columns[j + 1].Width, 9, g);
                                var x = 0;
                                switch (columns[j + 1].TextAlign)
                                {
                                    case HorizontalAlignment.Left:
                                        x = last + 6 - hscrollBar.Value;
                                        break;
                                    case HorizontalAlignment.Right:
                                        x = last + columns[j + 1].Width -
                                            StringTools.MeasureDisplayStringWidth(g, sp, Font) - 4 - hscrollBar.Value;
                                        break;
                                    default:
                                        x = last + (columns[j + 1].Width/2) -
                                            (StringTools.MeasureDisplayStringWidth(g, sp, Font)/2) - hscrollBar.Value;
                                        break;
                                }
                                g.DrawString(
                                    TruncatedString(node.SubItems[j].Text, columns[j + 1].Width, 9, g),
                                    Font,
                                    node.Selected && isFocused
                                        ? SystemBrushes.HighlightText
                                        : (node.UseItemStyleForSubItems
                                            ? new SolidBrush(node.ForeColor)
                                            : SystemBrushes.WindowText),
                                    x,
                                    r.Top + (itemheight*totalRend) + headerBuffer + 4 - vscrollBar.Value);
                            }
                        }
                    }

                    #endregion
                }

                totalRend++; // 节点数

                #region 画子节点

                if (node.IsExpanded) // 画子项
                {
                    childCount = 0;
                    var counts = node.GetNodeCount(false);
                    for (var n = 0; n < counts; n++)
                    {
                        RenderNodeRows(node.Nodes[n], g, r, level + 1, n, ref totalRend, ref childCount,
                            node.Nodes.Count);
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
                if (lnode.GetType() == typeof (ContainerSubListViewItem))
                {
                    var n = lnode as ContainerSubListViewItem;
                    OnRowSubItemEvent(n, e);
                }
                else if (lnode.GetType() == typeof (TreeListNode))
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
        ///     收缩所有
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
        ///     展开所有
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
        ///     未使用
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
        ///     未使用
        /// </summary>
        /// <param name="pt"></param>
        /// <returns></returns>
        public TreeListNode GetNodeAt(Point pt)
        {
            // To be added
            return null;
        }

        /// <summary>
        ///     获取所有被选中的项目
        /// </summary>
        /// <param name="node">节点</param>
        /// <returns></returns>
        private TreeListNodeCollection GetSelectedNodes(TreeListNode node)
        {
            var list = new TreeListNodeCollection();

            for (var i = 0; i < node.Nodes.Count; i++)
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
                    var list2 = GetSelectedNodes(node.Nodes[i]);
                    for (var j = 0; j < list2.Count; j++)
                    {
                        list.Add(list2[i]);
                    }
                }
            }

            return list;
        }

        /// <summary>
        ///     获取所有节点数
        /// </summary>
        /// <returns></returns>
        public int GetNodeCount()
        {
            var c = 0;

            c += nodes.Count;
            foreach (TreeListNode node in nodes)
            {
                c += GetNodeCount(node);
            }

            return c;
        }

        /// <summary>
        ///     获取所有节点数
        /// </summary>
        /// <param name="node">节点</param>
        /// <returns></returns>
        public int GetNodeCount(TreeListNode node)
        {
            var c = 0;
            c += node.Nodes.Count;
            foreach (TreeListNode n in node.Nodes)
            {
                c += GetNodeCount(n);
            }

            return c;
        }

        #endregion
    }

    #endregion

    #region TreeListNode

    [DesignTimeVisible(false), TypeConverter("ChurSkins.TreeListNodeConverter")]
    public class TreeListNode : IParentChildList
    {
        #region Constructor

        public TreeListNode()
        {
            Hovered = false;
            StateImageIndex = 0;
            Selected = false;
            TreeListView = null;
            Index = 0;
            ImageIndex = 0;
            Focused = false;
            Checked = false;
            IsExpanded = false;
            SubItems = new ContainerSubListViewItemCollection();
            SubItems.ItemsChanged += OnSubItemsChanged;

            Nodes = new TreeListNodeCollection();
            Nodes.Owner = this;
            Nodes.MouseDown += OnSubNodeMouseDown;
        }

        #endregion

        #region Event Handlers

        public event MouseEventHandler MouseDown;

        private void OnSubItemsChanged(object sender, ItemsChangedEventArgs e)
        {
            SubItems[e.IndexChanged].MouseDown += OnSubItemMouseDown;
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
        private readonly Rectangle bounds = Rectangle.Empty;
        private Color forecolor = SystemColors.WindowText;
        private bool styleall = true;

        private TreeListNode curChild;
        private readonly string fullPath = "";
        private bool visible = true;

        private TreeListNode parent;

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
            Editor(typeof (CollectionEditor), typeof (UITypeEditor))
        ]
        public TreeListNodeCollection Nodes { get; private set; }

        /// <summary>
        ///     是否被展开
        /// </summary>
        [Category("Behavior"),
         Description("是否被展开."),
         DefaultValue(false)]
        public bool IsExpanded { get; set; }

        /// <summary>
        ///     是否可视
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
        ///     区域
        /// </summary>
        [Browsable(false), Description("区域")]
        public Rectangle Bounds
        {
            get { return bounds; }
        }

        [Category("Behavior"), Description("选择框是否被选中")]
        public bool Checked { get; set; }

        [Browsable(false), Description("是否存在焦点")]
        public bool Focused { get; set; }

        [Category("Appearance"), Description("字体")]
        public Font Font { get; set; }

        [Category("Appearance"), Description("字体颜色")]
        public Color ForeColor
        {
            get { return forecolor; }
            set { forecolor = value; }
        }

        [Category("Behavior"), Description("图片索引")]
        public int ImageIndex { get; set; }

        [Browsable(false), Description("项目索引")]
        public int Index { get; set; }

        public CTreeListView TreeListView { get; private set; }

        /// <summary>
        ///     是否被选则
        /// </summary>
        [Browsable(false), Description("是否被选则")]
        public bool Selected { get; set; }

        /// <summary>
        ///     获取层次
        /// </summary>
        [Browsable(false), Description("获取层次")]
        public int Level
        {
            get { return (parent == null ? 0 : parent.Level + 1); }
        }

        [
            Category("Behavior"),
            Description("子项"),
            DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
            Editor(typeof (CollectionEditor), typeof (UITypeEditor))
        ]
        public ContainerSubListViewItemCollection SubItems { get; private set; }

        /// <summary>
        ///     状态图片索引
        /// </summary>
        [Category("Behavior")]
        public int StateImageIndex { get; set; }

        /// <summary>
        ///     关联数据
        /// </summary>
        [Browsable(false)]
        public object Tag { get; set; }

        /// <summary>
        ///     文本
        /// </summary>
        [Category("Appearance")]
        public string Text { get; set; }

        /// <summary>
        ///     是否为子项目使用风格
        /// </summary>
        [Category("Behavior")]
        public bool UseItemStyleForSubItems
        {
            get { return styleall; }
            set { styleall = value; }
        }

        [Browsable(false)]
        public bool Hovered { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     折叠
        /// </summary>
        public void Collapse()
        {
            IsExpanded = false;
        }

        /// <summary>
        ///     折叠所有
        /// </summary>
        public void CollapseAll()
        {
            for (var i = 0; i < Nodes.Count; i++)
            {
                Nodes[i].CollapseAll();
            }
            Collapse();
        }

        /// <summary>
        ///     展开
        /// </summary>
        public void Expand()
        {
            IsExpanded = true;
        }

        /// <summary>
        ///     展开所有
        /// </summary>
        public void ExpandAll()
        {
            for (var i = 0; i < Nodes.Count; i++)
                Nodes[i].ExpandAll();

            IsExpanded = true;
        }

        /// <summary>
        ///     获取节点数
        /// </summary>
        /// <param name="includeSubTrees">是否获取包含的子节点</param>
        /// <returns></returns>
        public int GetNodeCount(bool includeSubTrees)
        {
            var c = 0;

            if (includeSubTrees)
            {
                for (var n = 0; n < Nodes.Count; n++)
                {
                    c += Nodes[n].GetNodeCount(true);
                }
            }
            c += Nodes.Count;
            return c;
        }

        /// <summary>
        ///     获取可视节点数
        /// </summary>
        /// <param name="includeSubTrees">是否包含子节点</param>
        /// <returns></returns>
        public int GetVisibleNodeCount(bool includeSubTrees)
        {
            var c = 0;

            if (IsExpanded)
            {
                if (includeSubTrees)
                {
                    for (var n = 0; n < Nodes.Count; n++)
                    {
                        if (Nodes[n].IsExpanded)
                            c += Nodes[n].GetVisibleNodeCount(true);
                    }
                }

                for (var n = 0; n < Nodes.Count; n++)
                {
                    if (Nodes[n].IsVisible)
                        c++;
                }
            }

            return c;
        }

        /// <summary>
        ///     删除节点
        /// </summary>
        public void Remove()
        {
            var c = Nodes.IndexOf(curChild);
            Nodes.Remove(curChild);
            if (Nodes.Count > 0 && Nodes.Count > c)
                curChild = Nodes[c];
            else
                curChild = Nodes[Nodes.Count];
        }

        /// <summary>
        ///     切换折叠状态
        /// </summary>
        public void Toggle()
        {
            IsExpanded = !IsExpanded;
        }

        #endregion

        #region IParentChildList

        /// <summary>
        ///     获取父节点
        /// </summary>
        /// <returns></returns>
        public object ParentNode()
        {
            return parent;
        }

        /// <summary>
        ///     获取上一同级节点
        /// </summary>
        /// <returns></returns>
        public object PreviousSibling()
        {
            if (parent != null)
            {
                var thisIndex = parent.Nodes[this];
                if (thisIndex > 0)
                    return parent.Nodes[thisIndex - 1];
            }

            return null;
        }

        /// <summary>
        ///     获取下一个同级节点
        /// </summary>
        /// <returns></returns>
        public object NextSibling()
        {
            if (parent != null)
            {
                var thisIndex = parent.Nodes[this]; //	获取到当前节点索引
                if (thisIndex < parent.Nodes.Count - 1)
                    return parent.Nodes[thisIndex + 1];
            }
            return null;
        }

        /// <summary>
        ///     获取第一个节点
        /// </summary>
        /// <returns></returns>
        public object FirstChild()
        {
            curChild = Nodes[0];
            return curChild;
        }

        /// <summary>
        ///     获取最后一个节点
        /// </summary>
        /// <returns></returns>
        public object LastChild()
        {
            curChild = Nodes[Nodes.Count - 1];
            return curChild;
        }

        public object NextChild()
        {
            curChild = (TreeListNode) curChild.NextSibling();
            return curChild;
        }

        public object PreviousChild()
        {
            curChild = (TreeListNode) curChild.PreviousSibling();
            return curChild;
        }

        #endregion
    }

    public class TreeListNodeCollection : CollectionBase
    {
        public TreeListNodeCollection()
        {
        }

        public TreeListNodeCollection(TreeListNode owner)
        {
            Owner = owner;
        }

        public TreeListNode Owner { get; set; }

        /// <summary>
        ///     获取总节点数
        /// </summary>
        public int TotalCount
        {
            get
            {
                var tcnt = 0;
                tcnt += List.Count;
                foreach (TreeListNode n in List)
                {
                    tcnt += n.Nodes.TotalCount;
                }

                return tcnt;
            }
        }

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

        #endregion

        #region Implementation

        public TreeListNode this[int index]
        {
            get
            {
                if (List.Count > 0)
                {
                    return List[index] as TreeListNode;
                }
                return null;
            }
            set
            {
                List[index] = value;
                var tln = ((TreeListNode) List[index]);
                tln.MouseDown += OnMouseDown;
                tln.Nodes.NodesChanged += OnNodesChanged;
                tln.Parent = Owner;
                OnNodesChanged();
            }
        }

        public int this[TreeListNode item]
        {
            get { return List.IndexOf(item); }
        }

        public int Add(TreeListNode item)
        {
            item.MouseDown += OnMouseDown;
            item.Nodes.NodesChanged += OnNodesChanged;
            item.Parent = Owner;
            OnNodesChanged();
            return item.Index = List.Add(item);
        }

        public void AddRange(TreeListNode[] items)
        {
            lock (List.SyncRoot)
            {
                for (var i = 0; i < items.Length; i++)
                {
                    items[i].MouseDown += OnMouseDown;
                    items[i].Nodes.NodesChanged += OnNodesChanged;
                    items[i].Parent = Owner;
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
            for (var i = 0; i < List.Count; i++)
            {
                var col = ((TreeListNode) List[i]).SubItems;
                for (var j = 0; j < col.Count; j++)
                {
                    if (col[j].ItemControl != null)
                    {
                        col[j].ItemControl.Parent = null;
                        col[j].ItemControl.Visible = false;
                        col[j].ItemControl = null;
                    }
                }
                ((TreeListNode) List[i]).Nodes.Clear();
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
            if (destinationType == typeof (InstanceDescriptor))
            {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
            Type destinationType)
        {
            if (destinationType == typeof (InstanceDescriptor) && value is TreeListNode)
            {
                var tln = (TreeListNode) value;

                var ci = typeof (TreeListNode).GetConstructor(new Type[] {});
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