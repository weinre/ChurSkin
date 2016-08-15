namespace System.Windows.Forms
{
    public partial class CContextMenuStrip : ContextMenuStrip
    {
        ////#region 窗体边框阴影效果变量申明
        //const int CS_DropSHADOW = 0x20000;
        //const int GCL_STYLE = (-26);
        ////声明Win32 API
        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //public static extern int GetClassLong(IntPtr hwnd, int nIndex);
        public CContextMenuStrip()
        {
            ToolStripManager.Renderer = new CToolStripRenderer();
            InitializeComponent();
            DropShadowEnabled = false;
            //  SetClassLong(this.Handle, GCL_STYLE, GetClassLong(this.Handle, GCL_STYLE) | CS_DropSHADOW); //API函数加载，实现窗体边框阴影效果
        }
    }
}