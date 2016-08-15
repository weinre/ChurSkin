using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Win32;
using Win32.Struct;
using Win32.Consts;

namespace System.Windows.Forms
{
    public class ImageDlgBase : Form
    {
        private Image m_BgImg;
        private string m_WndClsName = Guid.NewGuid().ToString("N");
        private IntPtr m_FakeWndHandle;
        private WndProcDelegate m_DefWndProcDelegate;
        private WndProcDelegate m_CtrlWndProcDelegate;
        private bool m_bIsRefreshing = false;
        private Dictionary<IntPtr, IntPtr> m_WndProcMap = new Dictionary<IntPtr, IntPtr>();

        /// <summary>
        /// Set the background image from file
        /// </summary>
        [Category("Appearance")]
        [Description("Set the background image from file")]
        public Image DlgBgImg
        {
            set { m_BgImg = value; }
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (m_BgImg == null)
                return;

            base.AllowTransparency = true;
            base.Opacity = 0.5;
            base.FormBorderStyle = FormBorderStyle.None;
            base.Width = m_BgImg.Width;
            base.Height = m_BgImg.Height;

            this.Move += new EventHandler(this.OnDlgMove);
            this.FormClosed += new FormClosedEventHandler(this.OnDlgClosed);

            m_DefWndProcDelegate = new WndProcDelegate(NativeMethods.DefWindowProc);
            m_CtrlWndProcDelegate = new WndProcDelegate(this.CtrlWndProc);
            HookChildControl(this);

            CreateFakeWnd();
        }

        void OnDlgClosed(object sender, FormClosedEventArgs e)
        {
            DestroyFakeWnd();
            m_BgImg.Dispose();
            m_BgImg = null;
        }


        void HookChildControl(Control ctrl)
        {
            if (NativeMethods.IsWindow(ctrl.Handle))
            {
                m_WndProcMap[ctrl.Handle] = NativeMethods.GetWindowLongPtr(ctrl.Handle, WindowsLong.GWL_WNDPROC);
                NativeMethods.SetWindowLongPtr(ctrl.Handle, GWL.GWL_WNDPROC, Marshal.GetFunctionPointerForDelegate(m_CtrlWndProcDelegate));
            }

            if (!ctrl.HasChildren)
                return;
            foreach (Control child in ctrl.Controls)
            {
                HookChildControl(child);
            }
        }


        IntPtr CtrlWndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            if (!m_WndProcMap.ContainsKey(hWnd))
                return m_DefWndProcDelegate(hWnd, msg, wParam, lParam);

            IntPtr nRet = NativeMethods.CallWindowProc(m_WndProcMap[hWnd], hWnd, msg, wParam, lParam);

            switch (msg)
            {
                case (uint)WindowsMessage.WM_PAINT:
                case (uint)WindowsMessage.WM_CTLCOLOREDIT:
                case (uint)WindowsMessage.WM_CTLCOLORBTN:
                case (uint)WindowsMessage.WM_CTLCOLORSTATIC:
                case (uint)WindowsMessage.WM_CTLCOLORMSGBOX:
                case (uint)WindowsMessage.WM_CTLCOLORDLG:
                case (uint)WindowsMessage.WM_CTLCOLORLISTBOX:
                case (uint)WindowsMessage.WM_CTLCOLORSCROLLBAR:
                case (uint)WindowsMessage.WM_CAPTURECHANGED:
                    RefreshFakeWnd();
                    break;

                default:
                    break;
            }

            return nRet;
        }


        void OnDlgMove(object sender, EventArgs e)
        {
            if (!NativeMethods.IsWindow(m_FakeWndHandle))
                return;

            NativeMethods.MoveWindow(m_FakeWndHandle, this.Left, this.Top, this.Width, this.Height, false);
            RefreshFakeWnd();
        }

        protected override void WndProc(ref Message msg)
        {
            // make the window movable by drag on the client area
            if (msg.Msg == ((int)WindowsMessage.WM_LBUTTONDOWN))
            {
                msg.Msg = (int)WindowsMessage.WM_NCLBUTTONDOWN;
                msg.LParam = IntPtr.Zero;
                msg.WParam = new IntPtr((int)MousePositionCodes.HTCAPTION);
            }
            base.WndProc(ref msg);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            NativeMethods.ShowWindow(m_FakeWndHandle, (int)(this.Visible ? WindowShowStyle.Show : WindowShowStyle.Hide));
        }


        protected void CreateFakeWnd()
        {
            WNDCLASSEX wndClsEx = new WNDCLASSEX();
            wndClsEx.Init();
            wndClsEx.style = WndClassType.CS_VREDRAW | WndClassType.CS_HREDRAW;
            wndClsEx.lpfnWndProc = m_DefWndProcDelegate;
            wndClsEx.cbClsExtra = 0;
            wndClsEx.cbWndExtra = 0;
            wndClsEx.hInstance = NativeMethods.GetModuleHandle(null);
            wndClsEx.hIcon = IntPtr.Zero;
            wndClsEx.hIconSm = IntPtr.Zero;
            wndClsEx.hCursor = IntPtr.Zero;
            wndClsEx.hbrBackground = IntPtr.Zero;
            wndClsEx.lpszClassName = m_WndClsName;
            wndClsEx.lpszMenuName = null;

            bool success = NativeMethods.RegisterClassEx(ref wndClsEx) != 0;
            Debug.Assert(success, "RegisterWndClass failed.");
            UInt32 dwExStyle = WS_EX.WS_EX_LAYERED |
                WS_EX.WS_EX_TRANSPARENT |
                WS_EX.WS_EX_NOACTIVATE |
                WS_EX.WS_EX_LEFT;
            UInt32 dwStyle = (uint)WindowStyle.WS_VISIBLE | (uint)WindowStyle.WS_OVERLAPPED;
            m_FakeWndHandle = NativeMethods.CreateWindowEx((Int32)dwExStyle
                , m_WndClsName
                , null
                , (Int32)dwStyle
                , this.Left
                , this.Top
                , m_BgImg.Width
                , m_BgImg.Height
                , this.Handle
                , IntPtr.Zero
                , NativeMethods.GetModuleHandle(null)
                , IntPtr.Zero
                );
            Debug.Assert(NativeMethods.IsWindow(m_FakeWndHandle), "CreateWindowEx failed.");
        }

        protected void DestroyFakeWnd()
        {
            if (m_FakeWndHandle != IntPtr.Zero)
            {
                NativeMethods.DestroyWindow(m_FakeWndHandle);
                m_FakeWndHandle = IntPtr.Zero;

                NativeMethods.UnregisterClass(m_WndClsName, NativeMethods.GetModuleHandle(null));
            }
        }


        protected void RefreshFakeWnd()
        {
            if (m_bIsRefreshing)
                return;

            if (!NativeMethods.IsWindow(m_FakeWndHandle))
                return;

            m_bIsRefreshing = true;
            POINT ptSrc = new POINT(0, 0);
            POINT ptWinPos = new POINT(this.Left, this.Top);
            SIZE szWin = new SIZE(m_BgImg.Width, m_BgImg.Height);
            byte biAlpha = 0xFF;
            BLENDFUNCTION stBlend = new BLENDFUNCTION(BlendOp.AC_SRC_OVER, 0, biAlpha, BlendOp.AC_SRC_ALPHA);

            IntPtr hDC = NativeMethods.GetDC(m_FakeWndHandle);
            if (hDC == IntPtr.Zero)
            {
                m_bIsRefreshing = false;
                Debug.Assert(false, "GetDC failed.");
                return;
            }

            IntPtr hdcMemory = NativeMethods.CreateCompatibleDC(hDC);

            int nBytesPerLine = ((m_BgImg.Width * 32 + 31) & (~31)) >> 3;
            BITMAPINFOHEADER stBmpInfoHeader = new BITMAPINFOHEADER();
            stBmpInfoHeader.Init();
            stBmpInfoHeader.biWidth = m_BgImg.Width;
            stBmpInfoHeader.biHeight = m_BgImg.Height;
            stBmpInfoHeader.biPlanes = 1;
            stBmpInfoHeader.biBitCount = 32;
            stBmpInfoHeader.biCompression = CompressionType.BI_RGB;
            stBmpInfoHeader.biClrUsed = 0;
            stBmpInfoHeader.biSizeImage = (uint)(nBytesPerLine * m_BgImg.Height);

            IntPtr pvBits = IntPtr.Zero;
            IntPtr hbmpMem = NativeMethods.CreateDIBSection(hDC
                , ref stBmpInfoHeader
                , DIBColorTableIdentifier.DIB_RGB_COLORS
                , out pvBits
                , IntPtr.Zero
                , 0
                );
            Debug.Assert(hbmpMem != IntPtr.Zero, "CreateDIBSection failed.");

            if (hbmpMem != null)
            {
                IntPtr hbmpOld = NativeMethods.SelectObject(hdcMemory, hbmpMem);

                Graphics graphic = Graphics.FromHdcInternal(hdcMemory);

                graphic.DrawImage(m_BgImg, 0, 0, m_BgImg.Width, m_BgImg.Height);

                foreach (Control ctrl in this.Controls)
                {
                    if (!ctrl.Visible)
                        continue;

                    using (Bitmap bmp = new Bitmap(ctrl.Width, ctrl.Height))
                    {
                        Rectangle rect = new Rectangle(0, 0, ctrl.Width, ctrl.Height);
                        ctrl.DrawToBitmap(bmp, rect);

                        graphic.DrawImage(bmp, ctrl.Left, ctrl.Top, ctrl.Width, ctrl.Height);
                    }
                }

                GUITHREADINFO stGuiThreadInfo = new GUITHREADINFO();
                stGuiThreadInfo.Init();
                if (NativeMethods.GetGUIThreadInfo((uint)NativeMethods.GetCurrentThreadId(), ref stGuiThreadInfo))
                {
                    if (NativeMethods.IsWindow(stGuiThreadInfo.hwndCaret))
                    {
                        int height = stGuiThreadInfo.rcCaret.Bottom - stGuiThreadInfo.rcCaret.Top;
                        POINT ptCaret = new POINT(stGuiThreadInfo.rcCaret.Left, stGuiThreadInfo.rcCaret.Top);

                        NativeMethods.ClientToScreen(stGuiThreadInfo.hwndCaret, ref ptCaret);
                        NativeMethods.ScreenToClient(this.Handle, ref ptCaret);

                        graphic.DrawLine(new Pen(new SolidBrush(Color.Black))
                            , ptCaret.X
                            , ptCaret.Y
                            , ptCaret.X
                            , ptCaret.Y + height
                            );
                    }
                }


                NativeMethods.UpdateLayeredWindow(m_FakeWndHandle
                    , hDC
                    , ref ptWinPos
                    , ref szWin
                    , hdcMemory
                    , ref ptSrc
                    , 0
                    , ref stBlend
                    , UpdateLayerWindowParameter.ULW_ALPHA
                    );

                graphic.Dispose();
                NativeMethods.SelectObject(hbmpMem, hbmpOld);
                NativeMethods.DeleteObject(hbmpMem);
            }

            NativeMethods.DeleteDC(hdcMemory);
            NativeMethods.DeleteDC(hDC);

            m_bIsRefreshing = false;
        }

    }
}
