using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using Win32.Callback;
using Win32.Consts;
using Win32.Struct;

namespace Win32
{
    public class NativeMethods
    {
        public static int HIWORD(int n)
        {
            return (n >> 16) & 0xffff;
        }
        public static int HIWORD(IntPtr n)
        {
            return HIWORD(unchecked((int)(long)n));
        }
        public static int LOWORD(int n)
        {
            return n & 0xffff;
        }
        public static int LOWORD(IntPtr n)
        {
            return LOWORD(unchecked((int)(long)n));
        }

        public const int CWP_All = 0;
        public const int CWP_SKIPDISABLED = 2;
        public const int CWP_SKIPINVISIBL = 1;
        public const int EM_GETOLEINTERFACE = 0x43c;
      //  public const int ULW_ALPHA = 2;
        public const int WM_USER = 0x400;
        public static void MouseToMoveControl(IntPtr handle)
        {
            ReleaseCapture();
            SendMessage(handle, 0x112, 0xf012, 0);
        }
        private NativeMethods()
        {
        }

        [DllImport("shlwapi.dll", CharSet = CharSet.Auto)]
        public static extern bool PathCompactPathEx(StringBuilder pszOut, string pszPath, int cchMax, int reserved);

        //http://msdn.microsoft.com/en-us/library/bb663138.aspx

        /*public const string GUID_DEVINTERFACE_DISK = "53f56307-b6bf-11d0-94f2-00a0c91efb8b";
        public const string GUID_DEVINTERFACE_HUBCONTROLLER = "3abf6f2d-71c4-462a-8a92-1e6861e6af27";
        public const string GUID_DEVINTERFACE_MODEM = "2C7089AA-2E0E-11D1-B114-00C04FC2AAE4";
        public const string GUID_DEVINTERFACE_SERENUM_BUS_ENUMERATOR = "4D36E978-E325-11CE-BFC1-08002BE10318";
        public const string GUID_DEVINTERFACE_COMPORT = "86E0D1E0-8089-11D0-9CE4-08003E301F73";
        public const string GUID_DEVINTERFACE_PARALLEL = "97F76EF0-F883-11D0-AF1F-0000F800845C";*/
        // Win32 constants
        //private const int BROADCAST_QUERY_DENY = 0x424D5144;

        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, IntPtr NotificationFilter, int Flags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterDeviceNotification(IntPtr hHandle);

        /// <summary>
        ///     The SetupDiEnumDeviceInfo function retrieves a context structure for a device information element of the specified
        ///     device information set. Each call returns information about one device. The function can be called repeatedly
        ///     to get information about several devices.
        /// </summary>
        /// <param name="DeviceInfoSet">
        ///     A handle to the device information set for which to return an SP_DEVINFO_DATA structure
        ///     that represents a device information element.
        /// </param>
        /// <param name="MemberIndex">A zero-based index of the device information element to retrieve.</param>
        /// <param name="DeviceInfoData">
        ///     A pointer to an SP_DEVINFO_DATA structure to receive information about an enumerated
        ///     device information element. The caller must set DeviceInfoData.cbSize to sizeof(SP_DEVINFO_DATA).
        /// </param>
        /// <returns></returns>
        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern bool SetupDiEnumDeviceInfo(IntPtr DeviceInfoSet, uint MemberIndex,
            ref SP_DEVINFO_DATA DeviceInfoData);

        /// <summary>
        ///     A call to SetupDiEnumDeviceInterfaces retrieves a pointer to a structure that identifies a specific device
        ///     interface
        ///     in the previously retrieved DeviceInfoSet array. The call specifies a device interface by passing an array index.
        ///     To retrieve information about all of the device interfaces, an application can loop through the array,
        ///     incrementing the array index until the function returns zero, indicating that there are no more interfaces.
        ///     The GetLastError API function then returns No more data is available.
        /// </summary>
        /// <param name="hDevInfo">Input: Give it the HDEVINFO we got from SetupDiGetClassDevs()</param>
        /// <param name="devInfo">Input (optional)</param>
        /// <param name="interfaceClassGuid">Input</param>
        /// <param name="memberIndex">Input: "Index" of the device you are interested in getting the path for.</param>
        /// <param name="deviceInterfaceData">Output: This function fills in an "SP_DEVICE_INTERFACE_DATA" structure.</param>
        /// <returns></returns>
        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetupDiEnumDeviceInterfaces(
            IntPtr hDevInfo,
            IntPtr devInfo,
            ref Guid interfaceClassGuid, //ref
            uint memberIndex,
            ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData
            );

        /// <summary>
        ///     Gives us a device path, which is needed before CreateFile() can be used.
        /// </summary>
        /// <param name="hDevInfo">Input: Wants HDEVINFO which can be obtained from SetupDiGetClassDevs()</param>
        /// <param name="deviceInterfaceData">Input: Pointer to a structure which defines the device interface.</param>
        /// <param name="deviceInterfaceDetailData">Output: Pointer to a structure, which will contain the device path.</param>
        /// <param name="deviceInterfaceDetailDataSize">Input: Number of bytes to retrieve.</param>
        /// <param name="requiredSize">Output (optional): The number of bytes needed to hold the entire struct</param>
        /// <param name="deviceInfoData">Output</param>
        /// <returns></returns>
        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetupDiGetDeviceInterfaceDetail(
            IntPtr hDevInfo,
            ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, //ref
            ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData,
            uint deviceInterfaceDetailDataSize,
            out uint requiredSize,
            ref SP_DEVINFO_DATA deviceInfoData
            );

        /// <summary>
        ///     Frees up memory by destroying a DeviceInfoList
        /// </summary>
        /// <param name="hDevInfo"></param>
        /// <returns></returns>
        [DllImport(@"setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetupDiDestroyDeviceInfoList(IntPtr hDevInfo);

        //Input: Give it a handle to a device info list to deallocate from RAM.

        /// <summary>
        ///     Returns a HDEVINFO type for a device information set.
        ///     We will need the HDEVINFO as in input parameter for calling many of the other SetupDixxx() functions.
        /// </summary>
        /// <param name="ClassGuid"></param>
        /// <param name="Enumerator"></param>
        /// <param name="hwndParent"></param>
        /// <param name="Flags"></param>
        /// <returns></returns>
        [DllImport("setupapi.dll", CharSet = CharSet.Auto)] // 1st form using a ClassGUID
        public static extern IntPtr SetupDiGetClassDevs(
            ref Guid ClassGuid, //ref
            IntPtr Enumerator,
            IntPtr hwndParent,
            uint Flags
            );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto)] // 2nd form uses an Enumerator
        public static extern IntPtr SetupDiGetClassDevs(
            IntPtr ClassGuid,
            string Enumerator,
            IntPtr hwndParent,
            int Flags
            );

        /// <summary>
        ///     The SetupDiGetDeviceRegistryProperty function retrieves the specified device property.
        ///     This handle is typically returned by the SetupDiGetClassDevs or SetupDiGetClassDevsEx function.
        /// </summary>
        /// <param Name="DeviceInfoSet">Handle to the device information set that contains the interface and its underlying device.</param>
        /// <param Name="DeviceInfoData">Pointer to an SP_DEVINFO_DATA structure that defines the device instance.</param>
        /// <param Name="Property">Device property to be retrieved. SEE MSDN</param>
        /// <param Name="PropertyRegDataType">
        ///     Pointer to a variable that receives the registry data Type. This parameter can be
        ///     NULL.
        /// </param>
        /// <param Name="PropertyBuffer">Pointer to a buffer that receives the requested device property.</param>
        /// <param Name="PropertyBufferSize">SIZE of the buffer, in bytes.</param>
        /// <param Name="RequiredSize">
        ///     Pointer to a variable that receives the required buffer size, in bytes. This parameter can
        ///     be NULL.
        /// </param>
        /// <returns>If the function succeeds, the return value is nonzero.</returns>
        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool SetupDiGetDeviceRegistryProperty(
            IntPtr DeviceInfoSet,
            ref SP_DEVINFO_DATA DeviceInfoData, //ref
            uint Property,
            ref uint PropertyRegDataType,
            IntPtr PropertyBuffer,
            uint PropertyBufferSize,
            ref uint RequiredSize
            );

        /// <summary>
        ///     The CM_Get_Parent function obtains a device instance handle to the parent node of a specified device node, in the
        ///     local machine's device tree.
        /// </summary>
        /// <param name="pdnDevInst">
        ///     Caller-supplied pointer to the device instance handle to the parent node that this function
        ///     retrieves. The retrieved handle is bound to the local machine.
        /// </param>
        /// <param name="dnDevInst">Caller-supplied device instance handle that is bound to the local machine.</param>
        /// <param name="ulFlags">Not used, must be zero.</param>
        /// <returns>
        ///     If the operation succeeds, the function returns CR_SUCCESS. Otherwise, it returns one of the CR_-prefixed
        ///     error codes defined in cfgmgr32.h.
        /// </returns>
        [DllImport("setupapi.dll")]
        public static extern int CM_Get_Parent(
            out uint pdnDevInst,
            uint dnDevInst,
            int ulFlags
            );

        /// <summary>
        ///     The CM_Get_Device_ID function retrieves the device instance ID for a specified device instance, on the local
        ///     machine.
        /// </summary>
        /// <param name="dnDevInst">Caller-supplied device instance handle that is bound to the local machine.</param>
        /// <param name="Buffer">
        ///     Address of a buffer to receive a device instance ID string. The required buffer size can be
        ///     obtained by calling CM_Get_Device_ID_Size, then incrementing the received value to allow room for the string's
        ///     terminating NULL.
        /// </param>
        /// <param name="BufferLen">Caller-supplied length, in characters, of the buffer specified by Buffer.</param>
        /// <param name="ulFlags">Not used, must be zero.</param>
        /// <returns>
        ///     If the operation succeeds, the function returns CR_SUCCESS. Otherwise, it returns one of the CR_-prefixed
        ///     error codes defined in cfgmgr32.h.
        /// </returns>
        [DllImport("setupapi.dll", CharSet = CharSet.Auto)]
        public static extern int CM_Get_Device_ID(uint dnDevInst, IntPtr Buffer, int BufferLen, int ulFlags);

        /// <summary>
        ///     The SetupDiOpenDevRegKey function opens a registry key for device-specific configuration information.
        /// </summary>
        /// <param name="hDeviceInfoSet">
        ///     A handle to the device information set that contains a device information element that
        ///     represents the device for which to open a registry key.
        /// </param>
        /// <param name="DeviceInfoData">
        ///     A pointer to an SP_DEVINFO_DATA structure that specifies the device information element in
        ///     DeviceInfoSet.
        /// </param>
        /// <param name="Scope">
        ///     The scope of the registry key to open. The scope determines where the information is stored. The scope can be
        ///     global or specific to a hardware profile. The scope is specified by one of the following values:
        ///     DICS_FLAG_GLOBAL Open a key to store global configuration information. This information is not specific to a
        ///     particular hardware profile. For NT-based operating systems this opens a key that is rooted at HKEY_LOCAL_MACHINE.
        ///     The exact key opened depends on the value of the KeyType parameter.
        ///     DICS_FLAG_CONFIGSPECIFIC Open a key to store hardware profile-specific configuration information. This key is
        ///     rooted at one of the hardware-profile specific branches, instead of HKEY_LOCAL_MACHINE. The exact key opened
        ///     depends on the value of the KeyType parameter.
        /// </param>
        /// <param name="HwProfile">
        ///     A hardware profile value, which is set as follows:
        ///     If Scope is set to DICS_FLAG_CONFIGSPECIFIC, HwProfile specifies the hardware profile of the key that is to be
        ///     opened.
        ///     If HwProfile is 0, the key for the current hardware profile is opened.
        ///     If Scope is DICS_FLAG_GLOBAL, HwProfile is ignored.
        /// </param>
        /// <param name="KeyType">
        ///     The type of registry storage key to open, which can be one of the following values:
        ///     DIREG_DEV Open a hardware key for the device.
        ///     DIREG_DRV Open a software key for the device.
        ///     For more information about a device's hardware and software keys, see Driver Information in the Registry.
        /// </param>
        /// <param name="samDesired">
        ///     The registry security access that is required for the requested key. For information about
        ///     registry security access values of type REGSAM, see the Microsoft Windows SDK documentation.
        /// </param>
        /// <returns>
        ///     If the function is successful, it returns a handle to an opened registry key where private configuration data
        ///     pertaining to this device instance can be stored/retrieved.
        ///     If the function fails, it returns INVALID_HANDLE_VALUE. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("Setupapi", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetupDiOpenDevRegKey(IntPtr hDeviceInfoSet, ref SP_DEVINFO_DATA DeviceInfoData,
            uint Scope, uint HwProfile, uint KeyType, uint samDesired);

        /// <summary>
        ///     Retrieves the type and data for the specified value name associated with an open registry key.
        /// </summary>
        /// <param name="hKey">
        ///     A handle to an open registry key. The key must have been opened with the KEY_QUERY_VALUE access
        ///     right.
        /// </param>
        /// <param name="lpValueName">
        ///     The name of the registry value.
        ///     If lpValueName is NULL or an empty string, "", the function retrieves the type and data for the key's unnamed or
        ///     default value, if any.
        ///     If lpValueName specifies a key that is not in the registry, the function returns ERROR_FILE_NOT_FOUND.
        /// </param>
        /// <param name="lpReserved">This parameter is reserved and must be NULL.</param>
        /// <param name="lpType">
        ///     A pointer to a variable that receives a code indicating the type of data stored in the specified
        ///     value. The lpType parameter can be NULL if the type code is not required.
        /// </param>
        /// <param name="lpData">
        ///     A pointer to a buffer that receives the value's data. This parameter can be NULL if the data is
        ///     not required.
        /// </param>
        /// <param name="lpcbData">
        ///     A pointer to a variable that specifies the size of the buffer pointed to by the lpData parameter, in bytes. When
        ///     the function returns, this variable contains the size of the data copied to lpData.
        ///     The lpcbData parameter can be NULL only if lpData is NULL.
        ///     If the data has the REG_SZ, REG_MULTI_SZ or REG_EXPAND_SZ type, this size includes any terminating null character
        ///     or characters unless the data was stored without them. For more information, see Remarks.
        ///     If the buffer specified by lpData parameter is not large enough to hold the data, the function returns
        ///     ERROR_MORE_DATA and stores the required buffer size in the variable pointed to by lpcbData. In this case, the
        ///     contents of the lpData buffer are undefined.
        ///     If lpData is NULL, and lpcbData is non-NULL, the function returns ERROR_SUCCESS and stores the size of the data, in
        ///     bytes, in the variable pointed to by lpcbData. This enables an application to determine the best way to allocate a
        ///     buffer for the value's data.If hKey specifies HKEY_PERFORMANCE_DATA and the lpData buffer is not large enough to
        ///     contain all of the returned data, RegQueryValueEx returns ERROR_MORE_DATA and the value returned through the
        ///     lpcbData parameter is undefined. This is because the size of the performance data can change from one call to the
        ///     next. In this case, you must increase the buffer size and call RegQueryValueEx again passing the updated buffer
        ///     size in the lpcbData parameter. Repeat this until the function succeeds. You need to maintain a separate variable
        ///     to keep track of the buffer size, because the value returned by lpcbData is unpredictable.
        ///     If the lpValueName registry value does not exist, RegQueryValueEx returns ERROR_FILE_NOT_FOUND and the value
        ///     returned through the lpcbData parameter is undefined.
        /// </param>
        /// <returns>
        ///     If the function succeeds, the return value is ERROR_SUCCESS.
        ///     If the function fails, the return value is a system error code.
        ///     If the lpData buffer is too small to receive the data, the function returns ERROR_MORE_DATA.
        ///     If the lpValueName registry value does not exist, the function returns ERROR_FILE_NOT_FOUND.
        /// </returns>
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, EntryPoint = "RegQueryValueExW", SetLastError = true)]
        public static extern int RegQueryValueEx(IntPtr hKey, string lpValueName, uint lpReserved, out uint lpType,
            StringBuilder lpData, ref uint lpcbData);

        /// <summary>
        ///     Closes a handle to the specified registry key.
        /// </summary>
        /// <param name="hKey">A handle to the open key to be closed.</param>
        /// <returns>
        ///     If the function succeeds, the return value is ERROR_SUCCESS.
        ///     If the function fails, the return value is a nonzero error code defined in Winerror.h.
        /// </returns>
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern int RegCloseKey(IntPtr hKey);

        #region gdi32

        [DllImport("Msimg32.dll")]
        public static extern bool TransparentBlt(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest,
            int hHeightDest, IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc,
            uint crTransparent);

        [DllImport("gdi32.dll", EntryPoint = "BitBlt", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BitBlt([In] IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight,
            [In] IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

        [DllImport("gdi32.dll")]
        public static extern IntPtr GetStockObject(StockObjects fnObject);

        [DllImport("gdi32.dll")]
        public static extern bool Polyline(IntPtr hdc, [In] Point[] lppt, int cPoints);

        [DllImport("gdi32.dll")]
        public static extern bool PolylineTo(IntPtr hdc,Point[] lppt, uint cCount);

        [DllImport("gdi32.dll")]
        public static extern bool MoveToEx(IntPtr hdc, int X, int Y, IntPtr lpPoint);

        [DllImport("gdi32.dll")]
        public static extern bool LineTo(IntPtr hdc, int nXEnd, int nYEnd);

        [DllImport("gdi32.dll")]
        public static extern bool Rectangle(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport("gdi32.dll")]
        public static extern int OffsetClipRgn(IntPtr hdc, int nXOffset, int nYOffset);

        [DllImport("gdi32.dll")]
        public static extern int SetStretchBltMode(IntPtr hdc, StretchBltMode iStretchMode);

        [DllImport("gdi32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern int CombineRgn(IntPtr hRgn, IntPtr hRgn1, IntPtr hRgn2, int nCombineMode);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern bool SetWorldTransform(IntPtr hdc, [In] ref XFORM lpXform);

        [DllImport("gdi32.dll")]
        public static extern bool RestoreDC(IntPtr hdc, int nSavedDC);

        [DllImport("gdi32.dll")]
        public static extern int SaveDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern int SetGraphicsMode(IntPtr hdc, int iMode);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, int lpInitData);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDCA([MarshalAs(UnmanagedType.LPStr)] string lpszDriver,
            [MarshalAs(UnmanagedType.LPStr)] string lpszDevice, [MarshalAs(UnmanagedType.LPStr)] string lpszOutput,
            int lpInitData);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDCW([MarshalAs(UnmanagedType.LPWStr)] string lpszDriver,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszDevice, [MarshalAs(UnmanagedType.LPWStr)] string lpszOutput,
            int lpInitData);

        [DllImport("ole32.dll")]
        public static extern int CreateILockBytesOnHGlobal(IntPtr hGlobal, bool fDeleteOnRelease, out ILockBytes ppLkbyt);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr CreateRectRgn(int x1, int y1, int x2, int y2);

        [DllImport("gdi32.dll")]
        public static extern int CreateRoundRectRgn(int x1, int y1, int x2, int y2, int x3, int y3);

        [DllImport("gdi32.dll")]
        public static extern int ExcludeClipRect(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect,
            int nBottomRect);

        [DllImport("gdi32.dll")]
        public static extern bool GdiAlphaBlend(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest,
            int nHeightDest, IntPtr hdcSrc, int int_0, int int_1, int nWidthSrc, int nHeightSrc,
            BLENDFUNCTION blendFunction);

        #endregion

        #region user32
        [DllImport("user32.dll")]
        public static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("User32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);


        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetIconInfo(IntPtr hIcon, ref ICONINFO pICONINFO);

        [DllImport("user32.dll")]
        public static extern IntPtr CreateIconIndirect([In] ref ICONINFO piconinfo);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool AdjustWindowRectEx(ref RECT lpRect, int dwStyle, bool bMenu, int dwExStyle);

        [DllImport("user32.dll")]
        public static extern bool AnimateWindow(IntPtr whnd, int dwtime, int dwflag);

        [DllImport("user32.dll")]
        public static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(Point POINT); //获取当前鼠标下可视化控件的句柄 

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(int xPoint, int yPoint);

        [DllImport("user32.dll")]
        public static extern IntPtr ChildWindowFromPointEx(IntPtr pHwnd, Point pt, uint uFlgs);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr CreateWindowEx(int exstyle, string lpClassName, string lpWindowName, int dwStyle,
            int x, int y, int nWidth, int nHeight, IntPtr hwndParent, IntPtr Menu, IntPtr hInstance, IntPtr lpParam);

        [DllImport("user32.dll")]
        public static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("gdi32.dll")]
        public static extern bool DeleteDC(IntPtr hdc);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool DestroyIcon(IntPtr hIcon);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void DisableProcessWindowsGhosting();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DrawIconEx(IntPtr hdc, int xLeft, int yTop, IntPtr hIcon, int cxWidth, int cyHeight,
            int istepIfAniCur, IntPtr hbrFlickerFreeDraw, int diFlags);

        [DllImport("user32.dll")]
        public static extern bool EnableScrollBar(IntPtr hWnd, int wSBflags, int wArrows);

        [DllImport("user32.dll")]
        public static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT ps);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        [DllImport("user32.dll")]
        public static extern bool EqualRect([In] ref RECT lprc1, [In] ref RECT lprc2);

     

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass,
            string lpszWindow);

        [DllImport("user32.dll")]
        public static extern bool FlashWindow(IntPtr hWnd, bool bInvert);

        [DllImport("user32.dll")]
        public static extern uint GetCaretBlinkTime();

        [DllImport("user32.dll")]
        public static extern IntPtr GetClassLong(IntPtr hWnd, int nIndex);

        public static IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8)
            {
                return GetClassLongPtr_1(hWnd, nIndex);
            }
            return GetClassLong(hWnd, nIndex);
        }
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.U2)]
        public static extern short RegisterClassEx([In] ref WNDCLASSEX lpwcx);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnregisterClass(string lpClassName, IntPtr hInstance);
        [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
        private static extern IntPtr GetClassLongPtr_1(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, ref RECT r);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        public static extern bool GetComboBoxInfo(IntPtr hwndCombo, ref ComboBoxInfo info);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern int GetCurrentThreadId(); 

        [DllImport("user32.dll")]
        public static extern bool GetCursorInfo(out PCURSORINFO pci);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out Point lpPoint);

      //  [DllImport("CoreDll.dll")]
     //   public static extern bool GetCursorPos(ref POINT lpPoint);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr handle);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDCEx(IntPtr hWnd, IntPtr hrgnClip, int flags);

        [DllImport("user32.dll")]
        public static extern int SetParent(IntPtr hWndChild, int hWndNewParent);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern short GetKeyState(int nVirtKey);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd); //获取指定句柄的父级句柄

        [DllImport("user32.dll")]
        public static extern int GetScrollBarInfo(IntPtr hWnd, uint idObject, ref SCROLLBARINFO psbi);

        [DllImport("user32.dll")]
        public static extern bool GetScrollInfo(IntPtr hwnd, int fnBar, ref SCROLLINFO scrollInfo);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        public static extern int GetSystemMetrics(int nIndex); //获取屏幕的大小

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr handle);

        [DllImport("user32.dll")]
        public static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);

        [DllImport("user32")]
        public static extern int GetWindowLong(IntPtr hwnd, int nIndex);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowLong(HandleRef hWnd, int nIndex);

      //  [DllImport("user32.dll")]
      //  public static extern IntPtr GetWindowLongPtr(IntPtr hwnd, int nIndex);
        public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            if (IntPtr.Size == 8)
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            else
                return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLongW")]
        private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtrW")]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongW")]
        private static extern int GetWindowLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtrW")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

        public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
        {
            if (IntPtr.Size == 8)
                return GetWindowLongPtr64(hWnd, nIndex);
            else
                return new IntPtr(GetWindowLongPtr32(hWnd, nIndex));
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("comctl32.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool InitCommonControlsEx(ref INITCOMMONCONTROLSEX iccex);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool InvalidateRect(IntPtr hWnd, ref RECT rect, bool erase);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("uxtheme.dll")]
        public static extern bool IsAppThemed();

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool IsZoomed(IntPtr hWnd);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern bool KillTimer(IntPtr hWnd, uint uIDEvent);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursorFromFile(string fileName);

        [DllImport("user32.dll")]
        public static extern IntPtr SetCursor(IntPtr cursorHandle);

        [DllImport("user32.dll")]
        public static extern uint DestroyCursor(IntPtr cursorHandle);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadIcon(IntPtr hInstance, int lpIconName);

        [DllImport("user32.dll")]
        public static extern bool LockWindowUpdate(IntPtr hWndLock);

        [DllImport("user32.dll")]
        public static extern bool MessageBeep(int uType);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        [DllImport("kernel32.dll")]
        public static extern int MulDiv(int nNumber, int nNumerator, int nDenominator);

        [DllImport("user32.dll")]
        public static extern int OffsetRect(ref RECT lpRect, int x, int y);

        [DllImport("ole32.dll")]
        public static extern int OleCreateFromFile([In] ref Guid rclsid,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszFileName, [In] ref Guid riid, uint renderopt,
            ref FORMATETC pFormatEtc, IOleClientSite pClientSite, IStorage pStg,
            [MarshalAs(UnmanagedType.IUnknown)] out object ppvObj);

        [DllImport("ole32.dll")]
        public static extern int OleSetContainedObject([MarshalAs(UnmanagedType.IUnknown)] object pUnk, bool fContained);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool PtInRect(ref RECT lprc, Point pt);

        [DllImport("user32.dll")]
        public static extern bool RedrawWindow(IntPtr hWnd, ref RECT rectUpdate, IntPtr hrgnUpdate, int flags);

        [DllImport("user32.dll")]
        public static extern bool RedrawWindow(IntPtr hWnd, IntPtr rectUpdate, IntPtr hrgnUpdate, int flags);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr handle, IntPtr hdc);

        [DllImport("kernel32.dll")]
        public static extern int RtlMoveMemory(ref NMCUSTOMDRAW destination, IntPtr Source, int length);

        [DllImport("kernel32.dll")]
        public static extern int RtlMoveMemory(ref NMHDR destination, IntPtr source, int length);

        [DllImport("kernel32.dll")]
        public static extern int RtlMoveMemory(ref NMTTCUSTOMDRAW destination, IntPtr Source, int length);

        [DllImport("kernel32.dll")]
        public static extern int RtlMoveMemory(ref NMTTDISPINFO destination, IntPtr source, int length);

        [DllImport("kernel32.dll")]
        public static extern int RtlMoveMemory(IntPtr destination, ref NMTTDISPINFO Source, int length);

        [DllImport("kernel32.dll")]
        public static extern int RtlMoveMemory(ref Point destination, ref RECT Source, int length);

        [DllImport("user32.dll")]
        public static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);
        [DllImport("gdi32.dll", SetLastError = true)]
        public static extern IntPtr CreateDIBSection(IntPtr hdc, [In] ref BITMAPINFOHEADER pbmi, uint iUsage, out IntPtr ppvBits, IntPtr hSection, uint dwOffse);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetGUIThreadInfo(uint idThread, ref GUITHREADINFO lpgui);

        [DllImport("user32.dll", CharSet = CharSet.Auto, PreserveSig = false)]
        public static extern IRichEditOle SendMessage(IntPtr hWnd, int message, int wParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, ref RECT lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, ref SCROLLBARINFO lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, ref TOOLINFO lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, ref NMHDR lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam,
            [MarshalAs(UnmanagedType.LPTStr)] string lParam);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        public static extern int SetFocus(IntPtr hWnd);

        [DllImport("gdi32.dll")]
        public static extern uint SetPixel(IntPtr hdc, int X, int Y, int crColor);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern IntPtr SetTimer(IntPtr hWnd, int nIDEvent, uint uElapse, IntPtr lpTimerFunc);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLong(HandleRef hWnd, int nIndex, int dwNewLong);

      //  [DllImport("user32.dll")]
     //   public static extern IntPtr SetWindowLongPtr(IntPtr hwnd, int nIndex, IntPtr dwNewLong);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool SetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndAfter, int x, int y, int cx, int cy, uint flags);

        [DllImport("user32.dll")]
        public static extern int SetWindowRgn(IntPtr hwnd, int hRgn, bool bRedraw);

        [DllImport("user32.dll")]
        public static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool bRedraw);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, int hMod, int dwThreadId);

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

    

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("ole32.dll")]
        public static extern int StgCreateDocfileOnILockBytes(ILockBytes plkbyt, uint grfMode, uint reserved,
            out IStorage ppstgOpen);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("gdi32.dll")]
        public static extern bool StretchBlt(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest,
            int nHeightDest, IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc,
            TernaryRasterOperations dwRop);

        [DllImport("user32.dll")]
        public static extern bool TrackMouseEvent(ref TRACKMOUSEEVENT lpEventTrack);

        [DllImport("user32.dll")]
        public static extern IntPtr TrackPopupMenu(IntPtr hMenu, int uFlags, int x, int y, int nReserved, IntPtr hWnd,
            IntPtr par);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool TrackPopupMenuEx(IntPtr hMenu, uint uFlags, int x, int y, IntPtr hWnd,
            IntPtr tpmParams);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc, int crKey, ref BLENDFUNCTION pblend, int dwFlags);
        [DllImport("user32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern int UpdateWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pptSrc, int crKey, ref BLENDFUNCTION pblend, int dwFlags);


        [DllImport("user32.dll")]
        public static extern bool ValidateRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("kernel32.dll")]
        public static extern int WinExec(string lpCmdLine, int nCmdShow);

        //[DllImport("user32.dll", SetLastError = true)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //public static extern bool SystemParametersInfo(SPI uiAction, uint uiParam, ref T pvParam, SPIF fWinIni); // T = any type

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(SPI uiAction, uint uiParam, IntPtr pvParam, SPIF fWinIni);

        // For setting a string parameter
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, SPIF fWinIni);

        // For reading a string parameter
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, StringBuilder pvParam, SPIF fWinIni);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SystemParametersInfo(SPI uiAction, uint uiParam, ref RECT.ANIMATIONINFO pvParam,
            SPIF fWinIni);

        [DllImport("User32.dll")]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref RECT lpRect, uint fWinIni);

        #endregion

        [DllImport("ole32.dll")]
        public static extern int MkParseDisplayName(UCOMIBindCtx pbc, [MarshalAs(UnmanagedType.LPWStr)] string szUserName, out uint pchEaten, out UCOMIMoniker ppmk);
        [DllImport("ole32.dll")]
        public static extern int CreateBindCtx(uint reserved, out System.Runtime.InteropServices.ComTypes.IBindCtx ppbc);
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern IntPtr memcpy(IntPtr dest, IntPtr src, UIntPtr count);

        [DllImport("oleaut32.dll", PreserveSig = false)]
        public static extern void OleCreatePropertyFrame(IntPtr hwndOwner, int x, int y,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszCaption,
            int cObjects, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4, ArraySubType = UnmanagedType.IUnknown)] object[] lplpUnk,
            int cPages, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 6)] Guid[] lpPageClsID,
            int lcid, int dwReserved, int lpvReserved);

        /// <summary>
        ///     Supplies a pointer to an implementation of <b>IBindCtx</b> (a bind context object).
        ///     This object stores information about a particular moniker-binding operation.
        /// </summary>
        /// <param name="reserved">Reserved for future use; must be zero.</param>
        /// <param name="ppbc">
        ///     Address of <b>IBindCtx*</b> pointer variable that receives the
        ///     interface pointer to the new bind context object.
        /// </param>
        /// <returns>Returns <b>S_OK</b> on success.</returns>
        [DllImport("ole32.dll")]
        public static extern
        int CreateBindCtx(int reserved, out IBindCtx ppbc);

        /// <summary>
        ///     Converts a string into a moniker that identifies the object named by the string.
        /// </summary>
        /// <param name="pbc">Pointer to the IBindCtx interface on the bind context object to be used in this binding operation.</param>
        /// <param name="szUserName">Pointer to a zero-terminated wide character string containing the display name to be parsed. </param>
        /// <param name="pchEaten">Pointer to the number of characters of szUserName that were consumed.</param>
        /// <param name="ppmk">
        ///     Address of <b>IMoniker*</b> pointer variable that receives the interface pointer
        ///     to the moniker that was built from <b>szUserName</b>.
        /// </param>
        /// <returns>Returns <b>S_OK</b> on success.</returns>
        [DllImport("ole32.dll", CharSet = CharSet.Unicode)]
        public static extern int MkParseDisplayName(IBindCtx pbc, string szUserName, ref int pchEaten, out IMoniker ppmk);

        /// <summary>
        ///     Copy a block of memory.
        /// </summary>
        /// <param name="dst">Destination pointer.</param>
        /// <param name="src">Source pointer.</param>
        /// <param name="count">Memory block's length to copy.</param>
        /// <returns>Return's the value of <b>dst</b> - pointer to destination.</returns>
        [DllImport("ntdll.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe int memcpy(
            byte* dst,
            byte* src,
            int count);

        /// <summary>
        ///     Invokes a new property frame, that is, a property sheet dialog box.
        /// </summary>
        /// <param name="hwndOwner">Parent window of property sheet dialog box.</param>
        /// <param name="x">Horizontal position for dialog box.</param>
        /// <param name="y">Vertical position for dialog box.</param>
        /// <param name="caption">Dialog box caption.</param>
        /// <param name="cObjects">Number of object pointers in <b>ppUnk</b>.</param>
        /// <param name="ppUnk">Pointer to the objects for property sheet.</param>
        /// <param name="cPages">Number of property pages in <b>lpPageClsID</b>.</param>
        /// <param name="lpPageClsID">Array of CLSIDs for each property page.</param>
        /// <param name="lcid">Locale identifier for property sheet locale.</param>
        /// <param name="dwReserved">Reserved.</param>
        /// <param name="lpvReserved">Reserved.</param>
        /// <returns>Returns <b>S_OK</b> on success.</returns>
        [DllImport("oleaut32.dll")]
        public static extern int OleCreatePropertyFrame(
            IntPtr hwndOwner,
            int x,
            int y,
            [MarshalAs(UnmanagedType.LPWStr)] string caption,
            int cObjects,
            [MarshalAs(UnmanagedType.Interface, ArraySubType = UnmanagedType.IUnknown)] ref object ppUnk,
            int cPages,
            IntPtr lpPageClsID,
            int lcid,
            int dwReserved,
            IntPtr lpvReserved);


        [DllImport("dwmapi.dll")]
        public static extern int DwmEnableBlurBehindWindow(int hWnd, ref DWM_BLURBEHIND pBlurBehind);
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern int DwmEnableComposition(bool fEnable);
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern bool DwmIsCompositionEnabled();

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern int ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters,
        string lpDirectory, int nShowCmd);
        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern int FindExecutable(string filename, string directory, ref string result);
        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbSizeFileInfo, uint uFlags);
        [DllImport("shell32.dll")]
        public static extern uint ExtractIconEx(string lpszFile, int nIconIndex, int[] phiconLarge, int[] phiconSmall, uint nIcons);
    }



}