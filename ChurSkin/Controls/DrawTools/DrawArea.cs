using Com;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WhiteBoard.DocToolkit;

namespace ChurSkins.DrawTools
{
    /// <summary>
    /// Working area.
    /// Handles mouse input and draws graphics objects.
    /// </summary>
    public class DrawArea : UserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;
        private Capture capture = new Capture();

        #region settting
        private int n_VideoCapabilities = -1;
        private string mediaSubtypes = string.Empty;

        public int SetVideoResolution(int i, string mediasubtypes)
        {
            if (!capture.IsRunning || i < 0 || i > 20) return 0;
            if (n_VideoCapabilities == i && mediasubtypes == mediaSubtypes) return 0;
            CloseDevices();
            n_VideoCapabilities = i;
            mediaSubtypes = mediasubtypes;
            OpenDevice();
            return 1;
        }
        public int SetVideoResolution(int i)
        {
            if (!capture.IsRunning || i < 0 || i > 20) return 0;
            CloseDevices();
            n_VideoCapabilities = i;
            OpenDevice();
            return 1;
        }
        public string GetListResolution()
        {
            return capture.GetListResolution();
        }
        public string GetResolution
        {
            get { return capture.GetResolution; }
        }
        public string GetMedisSubTypes { get { return capture.GetMediaSubTypes; } }

        public List<string> GetListMediaSubTypes
        {
            get { return capture.GetListMediaSubTypes; }
        }
        private string ImageFormat = "jpg";

        public int SetImageFormat(string imageFormat)
        {
            this.ImageFormat = imageFormat;
            return 1;
        }
        public bool IsTimeAsWaterText = false;
        public string WaterText = string.Empty;
        public int OpenPropertyPage()
        {
            return capture.OpenPropertyPage(this.Handle);
        }
        public Bitmap SnapBitmap;

        int cropMode, colorMode, angle;
        string Barcode; bool barcode = false;
        public int GetSnap()
        {
            return capture.GetSanp(ref SnapBitmap, cropMode, colorMode, angle, ref Barcode, barcode);
        }
        public int SaveSnap(string savepath, int quality)
        {
            if (SnapBitmap == null || quality < 0 || quality > 100) return 0;
            Com.Capture.Save(SnapBitmap, savepath, WaterText, quality, IsTimeAsWaterText, ImageFormat);
            return 1;
        }
        public int SetRotate(int angle)
        {
            if (angle < 0) return 0;
            if (angle > 0 && angle < 90) return 0;
            if (angle > 90 && angle < 180) return 0;
            if (angle > 180 && angle < 270) return 0;
            if (angle > 270) return 0;
            this.angle = angle;
            return 1;
        }
        #endregion

        #region Constructor, Dispose

        public DrawArea()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // DrawArea
            // 
            this.Name = "DrawArea";
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DrawArea_MouseUp);
            //   this.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawArea_Paint);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawArea_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DrawArea_MouseDown);
            this.MouseWheel += new MouseEventHandler(DrawingBoard_MouseWheel);
            capture.control = this;
            SetStyles();

        }
        private void SetStyles()
        {
            this.SetStyle(
                   ControlStyles.AllPaintingInWmPaint |
                   ControlStyles.OptimizedDoubleBuffer |
                   ControlStyles.ResizeRedraw |
                   ControlStyles.Selectable |
                   ControlStyles.DoubleBuffer |
                   ControlStyles.SupportsTransparentBackColor |
                   ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.Opaque, false);
            this.UpdateStyles();
        }
        public int Zooms(float _scale)
        {
            if (!capture.IsRunning) return 0;
            scale = _scale;
            if (scale <= 1) { scale = 1f; }
            return 1;
        }
        float scale = 1f;
        public uint VID = 0, PID = 0;
        public void OpenDevice()
        {
            capture.OpenDevices(ref VID, ref PID, n_VideoCapabilities, mediaSubtypes);
        }
        public void CloseDevices()
        {
            capture.CloseDevices();
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            // is it required to update this's size/position
            if ((capture.needSizeUpdate) || (capture.firstFrameNotProcessed))
            {
                capture.needSizeUpdate = false;
            }
            if (!DesignMode && capture.IsRunning)
            {
                capture.paintBackground(angle, e, 0, scale);
            }
        }
        public Color penColor = Color.Black;
        public int penWidth = 1;
        public DashStyle ds = DashStyle.Solid;
        protected override void OnPaint(PaintEventArgs e)
        {
            //   base.OnPaint(e);
           // lock (capture.sync)
            //{
                // SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255));
                //e.Graphics.FillRectangle(brush, this.ClientRectangle);

                if (graphicsList != null)
                {
                    graphicsList.Draw(e.Graphics, penColor, penWidth, ds);
                }

                DrawNetSelection(e.Graphics);

                // brush.Dispose();
            //}
        }

        public void SaveDrawImg(string path)
        {
            this.GetSnap();
            if (this.SnapBitmap == null) return;
            Bitmap bitmap = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(bitmap);

            Rectangle rect = this.ClientRectangle;
            Rectangle drawrect = Rectangle.Empty;
            double ratio = (double)SnapBitmap.Width / SnapBitmap.Height;
            //上下有边
            if (rect.Width < rect.Height * ratio)
            {
                drawrect.X = 0;
                drawrect.Height = (int)Math.Round(rect.Width / ratio);
                drawrect.Y = (Height - drawrect.Height) / 2;
                drawrect.Width = Width;
            }
            else//左右有边
            {
                drawrect.Y = 0;
                drawrect.Width = (int)Math.Round(rect.Height * ratio);
                drawrect.X = (Width - drawrect.Width) / 2;
                drawrect.Height = Height;
            }
            g.DrawImage(SnapBitmap, drawrect);
            if (graphicsList != null)
            {
                graphicsList.Draw(g, penColor, penWidth, ds);
            }
            bitmap.Save(path);
            g.Dispose();
            bitmap.Dispose();
        }
        #endregion
        #region 拖动
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (capture.IsRunning)
            {
                if (cropMode == 2)
                {
                    capture.SetMouseDown(e.Location);
                }
                capture.mouseDown = true;
                capture.pointdown = e.Location;
            }
        }
        public bool IsRunning
        {
            get { return capture.IsRunning; }
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.Cursor = Cursors.Default;
        }
        public bool canMove;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            this.Focus();
            base.OnMouseMove(e);
            if (canMove)
                capture.SetMouseMove(cropMode, e.Location);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            capture.SetMouseUp(cropMode);
        }
        protected override bool IsInputKey(Keys keyData)
        {
            capture.SetIsInputKey(keyData, ref scale);
            base.IsInputKey(keyData);
            return true;
        }

        #endregion
        #region Enumerations

        public enum DrawToolType
        {
            Pointer,
            Rectangle,
            Ellipse,
            Line,
            Polygon,
            NumberOfDrawTools,
            Hand
        };

        #endregion

        #region Members

        private GraphicsList graphicsList;    // list of draw objects
        // (instances of DrawObject-derived classes)

        private DrawToolType activeTool;      // active drawing tool
        private Tool[] tools;                 // array of tools

        // group selection rectangle
        private Rectangle netRectangle;
        private bool drawNetRectangle = false;

        // Information about owner form
        private Form owner;
        private DocManager docManager;

        #endregion

        #region Properties

        /// <summary>
        /// Reference to the owner form
        /// </summary>
        public Form Owner
        {
            get
            {
                return owner;
            }
            set
            {
                owner = value;
            }
        }

        /// <summary>
        /// Reference to DocManager
        /// </summary>
        public DocManager DocManager
        {
            get
            {
                return docManager;
            }
            set
            {
                docManager = value;
            }
        }

        /// <summary>
        /// Group selection rectangle. Used for drawing.
        /// </summary>
        public Rectangle NetRectangle
        {
            get
            {
                return netRectangle;
            }
            set
            {
                netRectangle = value;
            }
        }

        /// <summary>
        /// Flas is set to true if group selection rectangle should be drawn.
        /// </summary>
        public bool DrawNetRectangle
        {
            get
            {
                return drawNetRectangle;
            }
            set
            {
                drawNetRectangle = value;
            }
        }

        /// <summary>
        /// Active drawing tool.
        /// </summary>
        public DrawToolType ActiveTool
        {
            get
            {
                return activeTool;
            }
            set
            {
                activeTool = value;
            }
        }

        /// <summary>
        /// List of graphics objects.
        /// </summary>
        public GraphicsList GraphicsList
        {
            get
            {
                return graphicsList;
            }
            set
            {
                graphicsList = value;
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Draw graphic objects and 
        /// group selection rectangle (optionally)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void DrawArea_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        //{
        //    SolidBrush brush = new SolidBrush(Color.FromArgb(255, 255, 255));
        //    e.Graphics.FillRectangle(brush,
        //        this.ClientRectangle);

        //    if (graphicsList != null)
        //    {
        //        graphicsList.Draw(e.Graphics);
        //    }

        //    DrawNetSelection(e.Graphics);

        //    brush.Dispose();
        //}

        /// <summary>
        /// Mouse down.
        /// Left button down event is passed to active tool.
        /// Right button down event is handled in this class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawArea_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (activeTool == DrawArea.DrawToolType.Hand) return;
            if (e.Button == MouseButtons.Left)
                tools[(int)activeTool].OnMouseDown(this, e);
            else if (e.Button == MouseButtons.Right)
                OnContextMenu(e);
        }
        private void DrawingBoard_MouseWheel(object sender, MouseEventArgs e)
        {
            //放大矢量
            if (capture.IsRunning)
            {
                scale += e.Delta / 1.2f;
                // scale1 += w1 / w2 * scale;
                if (e.Delta < 0)
                {
                    if (scale <= 1) { scale = 1f; }
                }
                Zooms(scale);
            }
            this.Invalidate();
        }

        public void comzoom()
        {
            scale += 100;
            this.Invalidate();
        }
        public void naroow()
        {
            scale -= 100;
            if (scale <= 1) { scale = 1f; }
            this.Invalidate();
        }
        public void restore()
        {
            scale = 1;
            this.Invalidate();
        }

        /// <summary>
        /// Mouse move.
        /// Moving without button pressed or with left button pressed
        /// is passed to active tool.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawArea_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (activeTool == DrawToolType.Hand) return;
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.None)
                tools[(int)activeTool].OnMouseMove(this, e);
            else
                this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// Mouse up event.
        /// Left button up event is passed to active tool.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawArea_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // if (e.Button == MouseButtons.Left)
            //tools[(int)activeTool].OnMouseUp(this, e);
            GraphicsList.UnselectAll();
            Refresh();
        }

        #endregion

        #region Other Functions

        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="docManager"></param>
        public void Initialize(Form owner, DocManager docManager)
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            // Keep reference to owner form
            this.Owner = owner;
            this.DocManager = docManager;

            // set default tool
            activeTool = DrawToolType.Pointer;

            // create list of graphic objects
            graphicsList = new GraphicsList();

            // create array of drawing tools
            tools = new Tool[(int)DrawToolType.NumberOfDrawTools];
            tools[(int)DrawToolType.Pointer] = new ToolPointer();
            tools[(int)DrawToolType.Rectangle] = new ToolRectangle();
            tools[(int)DrawToolType.Ellipse] = new ToolEllipse();
            tools[(int)DrawToolType.Line] = new ToolLine();
            tools[(int)DrawToolType.Polygon] = new ToolPolygon();
        }

        /// <summary>
        /// Set dirty flag (file is changed after last save operation)
        /// </summary>
        public void SetDirty()
        {
            DocManager.Dirty = true;
        }

        /// <summary>
        ///  Draw group selection rectangle
        /// </summary>
        /// <param name="g"></param>
        public void DrawNetSelection(Graphics g)
        {
            if (!DrawNetRectangle)
                return;

            ControlPaint.DrawFocusRectangle(g, NetRectangle, Color.Black, Color.Transparent);
        }

        /// <summary>
        /// Right-click handler
        /// </summary>
        /// <param name="e"></param>
        private void OnContextMenu(MouseEventArgs e)
        {
            return;
            // Change current selection if necessary

            Point point = new Point(e.X, e.Y);

            int n = GraphicsList.Count;
            DrawObject o = null;

            for (int i = 0; i < n; i++)
            {
                if (GraphicsList[i].HitTest(point) == 0)
                {
                    o = GraphicsList[i];
                    break;
                }
            }

            if (o != null)
            {
                if (!o.Selected)
                    GraphicsList.UnselectAll();

                // Select clicked object
                o.Selected = true;
            }
            else
            {
                GraphicsList.UnselectAll();
            }

            Refresh();

            // Show context menu.
            // Make ugly trick which saves a lot of code.
            // Get menu items from Edit menu in main form and
            // make context menu from them.
            // These menu items are handled in the parent form without
            // any additional efforts.

            MainMenu mainMenu = Owner.Menu;    // Main menu
            MenuItem editItem = mainMenu.MenuItems[1];            // Edit submenu

            // Make array of items for ContextMenu constructor
            // taking them from the Edit submenu
            MenuItem[] items = new MenuItem[editItem.MenuItems.Count];

            for (int i = 0; i < editItem.MenuItems.Count; i++)
            {
                items[i] = editItem.MenuItems[i];
            }

            //Owner.SetStateOfControls();  // enable/disable menu items//设置菜单的选种状态

            // Create and show context menu
            ContextMenu menu = new ContextMenu(items);
            menu.Show(this, point);

            // Restore items in the Edit menu (without this line Edit menu
            // is empty after forst right-click)
            editItem.MergeMenu(menu);
        }



        #endregion
    }
}
