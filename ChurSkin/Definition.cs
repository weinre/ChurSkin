using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Definition
{
    public class WinApi
    {
        private const int SM_CXSCREEN = 0;
        private const int SM_CYSCREEN = 1;
        private const int SWP_SHOWWINDOW = 64; // 0x0040
        private static readonly IntPtr HWND_TOP = IntPtr.Zero;

        public static int ScreenX
        {
            get { return GetSystemMetrics(SM_CXSCREEN); }
        }

        public static int ScreenY
        {
            get { return GetSystemMetrics(SM_CYSCREEN); }
        }

        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        public static extern int GetSystemMetrics(int which);

        [DllImport("user32.dll")]
        public static extern void
            SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter,
                int X, int Y, int width, int height, uint flags);

        public static void SetWinFullScreen(IntPtr hwnd)
        {
            SetWindowPos(hwnd, HWND_TOP, 0, 0, ScreenX, ScreenY, SWP_SHOWWINDOW);
        }
    }

    /// <summary>
    ///     Class used to preserve / restore state of the form
    /// </summary>
    public class FormState
    {
        private Rectangle bounds;
        private FormBorderStyle brdStyle;
        private bool topMost;
        private FormWindowState winState;

        public FormState()
        {
            IsMaximized = false;
        }

        public bool IsMaximized { get; private set; }

        public void Maximize(Form targetForm)
        {
            if (!IsMaximized)
            {
                targetForm.SuspendLayout();
                IsMaximized = true;
                Save(targetForm);
                targetForm.WindowState = FormWindowState.Maximized;
                targetForm.FormBorderStyle = FormBorderStyle.None;
                targetForm.TopMost = false;
                WinApi.SetWinFullScreen(targetForm.Handle);
                targetForm.ResumeLayout(true);
            }
        }

        public void Save(Form targetForm)
        {
            targetForm.SuspendLayout();
            winState = targetForm.WindowState;
            brdStyle = targetForm.FormBorderStyle;
            topMost = targetForm.TopMost;
            bounds = targetForm.Bounds;
            targetForm.ResumeLayout(true);
        }

        public void Restore(Form targetForm)
        {
            targetForm.WindowState = winState;
            targetForm.FormBorderStyle = brdStyle;
            targetForm.TopMost = topMost;
            targetForm.Bounds = bounds;
            IsMaximized = false;
        }
    }
}