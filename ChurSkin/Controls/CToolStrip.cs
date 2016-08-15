namespace System.Windows.Forms
{
    public partial class CToolStrip : ToolStrip
    {
        public CToolStrip()
        {
            ToolStripManager.Renderer = new CToolStripRenderer();
            InitializeComponent();
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.Focus();
        }
    }
}