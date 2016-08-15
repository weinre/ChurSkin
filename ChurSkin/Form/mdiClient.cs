using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Runtime.InteropServices;
using Win32;
using Win32.Consts;
using Win32.Struct;
using Point = System.Drawing.Point;

namespace System.Windows.Forms
{
    [ToolboxBitmap(typeof (CmdiClient))]
    public class CmdiClient : NativeWindow, IComponent, IDisposable
    {
        #region Private Fields

        private Form parentForm;
        private BorderStyle borderStyle;
        private Color backColor;
        private bool autoScroll;
        private Image image;
        private ContentAlignment imageAlign;
        private bool stretchImage;
        private ISite site;

        #endregion // Private Fields

        #region Public Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Slusser.Components.MdiClientController" /> class.
        /// </summary>
        public CmdiClient()
            : this(null)
        {
        }


        /// <summary>
        ///     Initializes a new instance of the <see cref="Slusser.Components.MdiClientController" /> class
        ///     for the given MDI form.
        /// </summary>
        /// <param name="parentForm">The MDI form.</param>
        public CmdiClient(Form parentForm)
        {
            // Initialize the variables.
            site = null;
            this.parentForm = null;
            MdiClient = null;
            backColor = SystemColors.AppWorkspace;
            borderStyle = BorderStyle.Fixed3D;
            autoScroll = true;
            image = null;
            imageAlign = ContentAlignment.MiddleCenter;
            stretchImage = false;

            // Set the ParentForm property.
            ParentForm = parentForm;
        }

        #endregion // Public Constructors

        #region Public Events

        /// <summary>
        ///     Occurs when the control is redrawn.
        /// </summary>
        [Category("Appearance"), Description("Occurs when a control needs repainting.")]
        public event PaintEventHandler Paint;


        /// <summary>
        ///     Occurs when the control is disposed.
        /// </summary>
        [Browsable(false)]
        public event EventHandler Disposed;


        /// <summary>
        ///     Occurs when the <see cref="System.Windows.Forms.NativeWindow" /> handle
        ///     is assigned.
        /// </summary>
        [Browsable(false)]
        public event EventHandler HandleAssigned;

        #endregion // Public Events

        #region Public Properties

        /// <summary>
        ///     Gets or sets the <see cref="System.ComponentModel.ISite" /> of
        ///     the <see cref="System.ComponentModel.Component" />.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ISite Site
        {
            get { return site; }
            set
            {
                site = value;

                if (site == null)
                    return;

                // If the component is dropped onto a form during design-time,
                // set the ParentForm property.
                var host = (value.GetService(typeof (IDesignerHost)) as IDesignerHost);
                if (host != null)
                {
                    var parent = host.RootComponent as Form;
                    if (parent != null)
                        ParentForm = parent;
                }
            }
        }


        /// <summary>
        ///     Gets or sets the form that the <see cref="System.Windows.Forms.MdiClient" />
        ///     control is assigned to.
        /// </summary>
        [Browsable(false)]
        public Form ParentForm
        {
            get { return parentForm; }
            set
            {
                // If the ParentForm has previously been set,
                // unwire events connected to the old parent.
                if (parentForm != null)
                    parentForm.HandleCreated -= ParentFormHandleCreated;

                parentForm = value;

                if (parentForm == null)
                    return;

                // If the parent form has not been created yet,
                // wait to initialize the MDI client until it is.
                if (parentForm.IsHandleCreated)
                {
                    InitializeMdiClient();
                    RefreshProperties();
                }
                else
                    parentForm.HandleCreated += ParentFormHandleCreated;
            }
        }


        /// <summary>
        ///     Gets the <see cref="System.Windows.Forms.MdiClient" /> being controlled.
        /// </summary>
        [Browsable(false)]
        public MdiClient MdiClient { get; private set; }


        /// <summary>
        ///     Gets or sets the background color for the control.
        /// </summary>
        [Category("Appearance"), DefaultValue(typeof (Color), "AppWorkspace")]
        [Description("The backcolor used to display text and graphics in the control.")]
        public Color BackColor
        {
            // Use the BackColor property of the MdiClient control. This is one of
            // the few properties in the MdiClient class that actually works.
            get
            {
                if (MdiClient != null)
                    return MdiClient.BackColor;

                return backColor;
            }
            set
            {
                backColor = value;
                if (MdiClient != null)
                    MdiClient.BackColor = value;
            }
        }


        /// <summary>
        ///     Indicates the border style for the control.
        /// </summary>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">
        ///     The value
        ///     assigned to the property is not a member of
        ///     the <see cref="System.Windows.Forms.BorderStyle" /> enumeration.
        /// </exception>
        [DefaultValue(BorderStyle.Fixed3D), Category("Appearance")]
        [Description("Indicates whether the MDI area should have a border.")]
        public BorderStyle BorderStyle
        {
            get { return borderStyle; }
            set
            {
                // Error-check the enum.
                if (!Enum.IsDefined(typeof (BorderStyle), value))
                    throw new InvalidEnumArgumentException("value", (int) value, typeof (BorderStyle));

                borderStyle = value;

                if (MdiClient == null)
                    return;

                // This property can actually be visible in design-mode,
                // but to keep it consistent with the others,
                // prevent this from being show at design-time.
                if (site != null && site.DesignMode)
                    return;

                // There is no BorderStyle property exposed by the MdiClient class,
                // but this can be controlled by Win32 functions. A Win32 ExStyle
                // of WS_EX_CLIENTEDGE is equivalent to a Fixed3D border and a
                // Style of WS_BORDER is equivalent to a FixedSingle border.

                // This code is inspired Jason Dori's article:
                // "Adding designable borders to user controls".
                // http://www.codeproject.com/cs/miscctrl/CsAddingBorders.asp

                // Get styles using Win32 calls
                var style = NativeMethods.GetWindowLong(MdiClient.Handle, GWL.GWL_STYLE);
                var exStyle = NativeMethods.GetWindowLong(MdiClient.Handle, GWL.GWL_EXSTYLE);

                // Add or remove style flags as necessary.
                switch (borderStyle)
                {
                    case BorderStyle.Fixed3D:
                        exStyle |= (int) WindowStyle.WS_EX_CLIENTEDGE;
                        style &= ~(int) WindowStyle.WS_BORDER;
                        break;

                    case BorderStyle.FixedSingle:
                        exStyle &= ~(int) WindowStyle.WS_EX_CLIENTEDGE;
                        style |= (int) WindowStyle.WS_BORDER;
                        break;

                    case BorderStyle.None:
                        style &= ~(int) WindowStyle.WS_BORDER;
                        exStyle &= ~(int) WindowStyle.WS_EX_CLIENTEDGE;
                        break;
                }

                // Set the styles using Win32 calls
                NativeMethods.SetWindowLong(MdiClient.Handle, GWL.GWL_STYLE, style);
                NativeMethods.SetWindowLong(MdiClient.Handle, GWL.GWL_EXSTYLE, exStyle);

                // Cause an update of the non-client area.
                UpdateStyles();
            }
        }


        /// <summary>
        ///     Gets or sets a value indicating whether the container will allow the user to
        ///     scroll to any controls placed outside of its visible boundaries.
        /// </summary>
        [DefaultValue(true), Category("Layout")]
        [Description(
            "Determines whether scrollbars will automatically appear if controls are placed outside the MDI client area."
            )]
        public bool AutoScroll
        {
            get { return autoScroll; }
            set
            {
                // By default the MdiClient control scrolls. It can appear though that
                // there are no scrollbars by turning them off when the non-client
                // area is calculated. I decided to expose this method following
                // the .NET vernacular of an AutoScroll property.

                autoScroll = value;
                if (MdiClient != null)
                    UpdateStyles();
            }
        }


        /// <summary>
        ///     Gets or sets the image that the
        ///     <see cref="Slusser.Components.MdiClientController" /> displays.
        /// </summary>
        [DefaultValue(null), Category("Appearance")]
        [Description("The image displayed in the MDI client area.")]
        public Image Image
        {
            get { return image; }
            set
            {
                image = value;
                if (MdiClient != null)
                    MdiClient.Invalidate();
            }
        }


        /// <summary>
        ///     Gets or sets the alignment of the background image.
        /// </summary>
        /// <exception cref="System.ComponentModel.InvalidEnumArgumentException">
        ///     The
        ///     value assigned to the property is not a member of
        ///     the <see cref="System.Drawing.ContentAlignment" /> enumeration.
        /// </exception>
        [Category("Appearance"), DefaultValue(ContentAlignment.MiddleCenter)]
        [Description("Determines the position of the image within the MDI client area.")]
        public ContentAlignment ImageAlign
        {
            get { return imageAlign; }
            set
            {
                // Error-check the enum.
                if (!Enum.IsDefined(typeof (ContentAlignment), value))
                    throw new InvalidEnumArgumentException("value", (int) value, typeof (ContentAlignment));

                imageAlign = value;
                if (MdiClient != null)
                    MdiClient.Invalidate();
            }
        }


        /// <summary>
        ///     Gets or sets a value indicating whether the
        ///     <see cref="Slusser.Components.MdiClientController.Image" /> should
        ///     be stretched to fill the entire client area.
        /// </summary>
        [Category("Appearance"), DefaultValue(false)]
        [Description("Determines whether the image should be scaled to fill the entire client area.")]
        public bool StretchImage
        {
            get { return stretchImage; }
            set
            {
                stretchImage = value;
                if (MdiClient != null)
                    MdiClient.Invalidate();
            }
        }


        /// <summary>
        ///     Gets the handle for this window.
        /// </summary>
        [Browsable(false)]
        public new IntPtr Handle
        {
            // Hide this from the property grid during design-time.
            get { return base.Handle; }
        }

        #endregion // Public Properties

        #region Public Methods

        /// <summary>
        ///     Releases all resources used by the
        ///     <see cref="System.ComponentModel.Component" />.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        ///     Reestablishes a connection to the <see cref="System.Windows.Forms.MdiClient" />
        ///     control if the <see cref="Slusser.Components.MdiClientController.ParentForm" />
        ///     hasn't changed but its <see cref="System.Windows.Forms.Form.IsMdiContainer" />
        ///     property has.
        /// </summary>
        public void RenewMdiClient()
        {
            // Reinitialize the MdiClient and its properties.
            InitializeMdiClient();
            RefreshProperties();
        }

        #endregion // Public Methods

        #region Protected Methods

        /// <summary>
        ///     Releases the unmanaged resources used by the
        ///     <see cref="System.ComponentModel.Component" /> and optionally releases the
        ///     managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged
        ///     resources; <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                lock (this)
                {
                    if (site != null && site.Container != null)
                        site.Container.Remove(this);

                    if (Disposed != null)
                        Disposed(this, EventArgs.Empty);
                }
            }
        }


        /// <summary>
        ///     Invokes the default window procedure associated with this window.
        /// </summary>
        /// <param name="m">A <see cref="System.Windows.Forms.Message" /> that is associated with the current Windows message. </param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                //Do all painting in WM_PAINT to reduce flicker.
                case (int) WindowsMessage.WM_ERASEBKGND:
                    return;

                case (int) WindowsMessage.WM_PAINT:

                    // This code is influenced by Steve McMahon's article:
                    // "Painting in the MDI Client Area".
                    // http://vbaccelerator.com/article.asp?id=4306

                    // Use Win32 to get a Graphics object.
                    var paintStruct = new PAINTSTRUCT();
                    var screenHdc = NativeMethods.BeginPaint(m.HWnd, ref paintStruct);

                    using (var screenGraphics = Graphics.FromHdc(screenHdc))
                    {
                        // Get the area to be updated.
                        var clipRect = new Rectangle(
                            paintStruct.rcPaint.Left,
                            paintStruct.rcPaint.Top,
                            paintStruct.rcPaint.Right - paintStruct.rcPaint.Left,
                            paintStruct.rcPaint.Bottom - paintStruct.rcPaint.Top);

                        // Double-buffer by painting everything to an image and
                        // then drawing the image.
                        var width = (MdiClient.ClientRectangle.Width > 0 ? MdiClient.ClientRectangle.Width : 0);
                        var height = (MdiClient.ClientRectangle.Height > 0 ? MdiClient.ClientRectangle.Height : 0);
                        using (Image i = new Bitmap(width, height))
                        {
                            using (var g = Graphics.FromImage(i))
                            {
                                // This code comes from J Young's article:
                                // "Generating missing Paint event for TreeView and ListView".
                                // http://www.codeproject.com/cs/miscctrl/genmissingpaintevent.asp

                                // Draw base graphics and raise the base Paint event.
                                var hdc = g.GetHdc();
                                var printClientMessage =
                                    Message.Create(m.HWnd, (int) WindowsMessage.WM_PRINTCLIENT, hdc, IntPtr.Zero);
                                DefWndProc(ref printClientMessage);
                                g.ReleaseHdc(hdc);

                                // Draw the image here.
                                if (image != null)
                                    DrawImage(g, clipRect);

                                // Call our OnPaint here to draw graphics over the
                                // original and raise our Paint event.
                                OnPaint(new PaintEventArgs(g, clipRect));
                            }

                            // Now draw all the graphics at once.
                            screenGraphics.DrawImage(i, MdiClient.ClientRectangle);
                        }
                    }

                    NativeMethods.EndPaint(m.HWnd, ref paintStruct);
                    return;

                case (int) WindowsMessage.WM_SIZE:

                    // Repaint on every resize.
                    MdiClient.Invalidate();
                    break;


                case (int) WindowsMessage.WM_NCCALCSIZE:

                    // If AutoScroll is set to false, hide the scrollbars when the control
                    // calculates its non-client area.
                    if (!autoScroll)
                        ShowScrollBar(m.HWnd, SB.SB_BOTH, 0 /*false*/);
                    break;
            }

            base.WndProc(ref m);
        }

        [DllImport("user32.dll")]
        private static extern int ShowScrollBar(IntPtr hWnd, int wBar, int bShow);


        /// <summary>
        ///     Raises the <see cref="Slusser.Components.MdiClientController.Paint" /> event.
        /// </summary>
        /// <param name="e">
        ///     A <see cref="System.Windows.Forms.PaintEventArgs" /> that
        ///     contains the event data.
        /// </param>
        protected virtual void OnPaint(PaintEventArgs e)
        {
            // Raise the Paint event.
            if (Paint != null)
                Paint(this, e);
        }


        /// <summary>
        ///     Raises the <see cref="Slusser.Components.MdiClientController.HandleAssigned" /> event.
        /// </summary>
        /// <param name="e">
        ///     A <see cref="System.EventArgs" /> that contains the event
        ///     data.
        /// </param>
        protected virtual void OnHandleAssigned(EventArgs e)
        {
            // Raise the HandleAssigned event.
            if (HandleAssigned != null)
                HandleAssigned(this, e);
        }

        #endregion // Protected Methods

        #region Private Methods

        private void InitializeMdiClient()
        {
            // If the mdiClient has previously been set, unwire events connected
            // to the old MDI.
            if (MdiClient != null)
                MdiClient.HandleDestroyed -= MdiClientHandleDestroyed;

            if (parentForm == null)
                return;

            // Get the MdiClient from the parent form.
            for (var i = 0; i < parentForm.Controls.Count; i++)
            {
                // If the form is an MDI container, it will contain an MdiClient control
                // just as it would any other control.
                MdiClient = parentForm.Controls[i] as MdiClient;
                if (MdiClient != null)
                {
                    // Assign the MdiClient Handle to the NativeWindow.
                    ReleaseHandle();
                    AssignHandle(MdiClient.Handle);

                    // Raise the HandleAssigned event.
                    OnHandleAssigned(EventArgs.Empty);

                    // Monitor the MdiClient for when its handle is destroyed.
                    MdiClient.HandleDestroyed += MdiClientHandleDestroyed;
                }
            }
        }


        private void DrawImage(Graphics g, Rectangle clipRect)
        {
            // Paint the background image.
            if (stretchImage)
                g.DrawImage(image, MdiClient.ClientRectangle);
            else
            {
                // Calculate the location of the image. (Note: this logic could be calculated during sizing
                // instead of during painting to improve performance.)
                var pt = Point.Empty;
                switch (imageAlign)
                {
                    case ContentAlignment.TopLeft:
                        pt = new Point(0, 0);
                        break;
                    case ContentAlignment.TopCenter:
                        pt = new Point((MdiClient.ClientRectangle.Width/2) - (image.Width/2), 0);
                        break;
                    case ContentAlignment.TopRight:
                        pt = new Point(MdiClient.ClientRectangle.Width - image.Width, 0);
                        break;
                    case ContentAlignment.MiddleLeft:
                        pt = new Point(0,
                            (MdiClient.ClientRectangle.Height/2) - (image.Height/2));
                        break;
                    case ContentAlignment.MiddleCenter:
                        pt = new Point((MdiClient.ClientRectangle.Width/2) - (image.Width/2),
                            (MdiClient.ClientRectangle.Height/2) - (image.Height/2));
                        break;
                    case ContentAlignment.MiddleRight:
                        pt = new Point(MdiClient.ClientRectangle.Width - image.Width,
                            (MdiClient.ClientRectangle.Height/2) - (image.Height/2));
                        break;
                    case ContentAlignment.BottomLeft:
                        pt = new Point(0, MdiClient.ClientRectangle.Height - image.Height);
                        break;
                    case ContentAlignment.BottomCenter:
                        pt = new Point((MdiClient.ClientRectangle.Width/2) - (image.Width/2),
                            MdiClient.ClientRectangle.Height - image.Height);
                        break;
                    case ContentAlignment.BottomRight:
                        pt = new Point(MdiClient.ClientRectangle.Width - image.Width,
                            MdiClient.ClientRectangle.Height - image.Height);
                        break;
                }

                // Paint the image with the calculated coordinates and image size.
                g.DrawImage(image, new Rectangle(pt, image.Size));
            }
        }


        private void UpdateStyles()
        {
            // To show style changes, the non-client area must be repainted. Using the
            // control's Invalidate method does not affect the non-client area.
            // Instead use a Win32 call to signal the style has changed.

            NativeMethods.SetWindowPos(MdiClient.Handle, IntPtr.Zero, 0, 0, 0, 0,
                SWP.SWP_NOACTIVATE | SWP.SWP_NOMOVE | SWP.SWP_NOSIZE | SWP.SWP_NOZORDER |
                SWP.SWP_NOOWNERZORDER | SWP.SWP_FRAMECHANGED);
        }


        private void MdiClientHandleDestroyed(object sender, EventArgs e)
        {
            // If the MdiClient handle has been released, drop the reference and
            // release the handle.
            if (MdiClient != null)
            {
                MdiClient.HandleDestroyed -= MdiClientHandleDestroyed;
                MdiClient = null;
            }

            ReleaseHandle();
        }


        private void ParentFormHandleCreated(object sender, EventArgs e)
        {
            // The form has been created, unwire the event, and initialize the MdiClient.
            parentForm.HandleCreated -= ParentFormHandleCreated;
            InitializeMdiClient();
            RefreshProperties();
        }


        private void RefreshProperties()
        {
            // Refresh all the properties
            BackColor = backColor;
            BorderStyle = borderStyle;
            AutoScroll = autoScroll;
            Image = image;
            ImageAlign = imageAlign;
            StretchImage = stretchImage;
        }

        // If you don't own a Weezer album then you're really missing out on some
        // fantastic music.

        #endregion // Private Methods
    }
}