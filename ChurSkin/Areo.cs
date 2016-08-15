using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
    public static class Areo
    {
        //使窗体边框呈现Areo效果，并支持边框向内延伸
        [DllImport("dwmapi.dll", PreserveSig = false, EntryPoint = "DwmExtendFrameIntoClientArea")]
        public static extern void AreoFrame(IntPtr handle, Margin margin);

        //系统是否开启Areo效果
        [DllImport("dwmapi.dll", PreserveSig = false, EntryPoint = "DwmIsCompositionEnabled")]
        public static extern bool IsAreoEnabled();

        //开启或关闭系统Areo效果
        [DllImport("dwmapi.dll", PreserveSig = false, EntryPoint = "DwmEnableComposition")]
        public static extern void EnableAreo(bool bEnable);

        //使窗体背景呈现Areo效果
        [DllImport("dwmapi.dll", PreserveSig = false, EntryPoint = "DwmEnableBlurBehindWindow")]
        public static extern void AreoWindow(IntPtr handle, AreoParams areoParams);

        //Areo左、右边框宽度和上、下边框的高度
        [StructLayout(LayoutKind.Sequential)]
        public class Margin
        {
            public int LeftWidth, RightWidth, TopHeight, BottomHeight;

            public Margin(int left, int right, int top, int bottom)
            {
                LeftWidth = left;
                RightWidth = right;
                TopHeight = top;
                BottomHeight = bottom;
            }
        }

        //窗体背景Areo效果参数
        [StructLayout(LayoutKind.Sequential)]
        public class AreoParams
        {
            public uint Flags; //窗体背景Areo效果标记
            [MarshalAs(UnmanagedType.Bool)] public bool Enable; //窗体背景Areo效果开关
            public IntPtr AreoRegion; //窗体背景Areo区域指针
            [MarshalAs(UnmanagedType.Bool)] public bool TransitionOnMaximized; //窗体最大化时是否过渡
            public const uint ENABLE = 0x00000001; //窗体背景Areo效果
            public const uint REGION = 0x00000002; //窗体Areo区域
            public const uint TRANSITIONONMAXIMIZED = 0x00000004; //窗体最大化时过渡
        }
    }
}