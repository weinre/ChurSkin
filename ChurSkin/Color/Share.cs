using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    public static class Share
    {
        /// <summary>
        ///     窗体圆角大小-6
        /// </summary>
        public static int FormRadius = 6;

        /// <summary>
        ///     窗体阴影宽度-4
        /// </summary>
        public static int FormShadownWidth = 4;

        /// <summary>
        ///     窗体边框颜色-黑色
        /// </summary>
        public static Color FormShadownColor = Color.Black;

        /// <summary>
        ///     窗体边框阴影-否
        /// </summary>
        public static bool FormShadown = false;

        /// <summary>
        ///     窗体的背景颜色-白雾
        /// </summary>
        public static Color FormBackColor = Color.WhiteSmoke;

        /// <summary>
        ///     禁用边框颜色-淡灰色
        /// </summary>
        public static Color DisableBorderColor = Color.Gainsboro;

        /// <summary>
        ///     禁用背景颜色-白雾
        /// </summary>
        public static Color DisabelBackColor = Color.WhiteSmoke;

        /// <summary>
        ///     禁用文字颜色-灰
        /// </summary>
        public static Color DisabelFontColor = Color.Gray;

        /// <summary>
        ///     边框颜色-浅灰色 183
        /// </summary>
        public static Color BorderColor = Color.FromArgb(183, 183, 183); // Color.LightGray;

        /// <summary>
        ///     渐变顶部 288
        /// </summary>
        public static Color LineTop = Color.FromArgb(228, 228, 228);

        /// <summary>
        ///     渐变底部
        /// </summary>
        public static Color LineBottom = Color.FromArgb(211, 211, 211);

        /// <summary>
        ///     细线 184
        /// </summary>
        public static Color Line = Color.FromArgb(184, 184, 184);

        /// <summary>
        ///     背景和聚焦颜色-宝蓝
        /// </summary>
        public static Color FocusBackColor = ColorTranslator.FromHtml("#1979CA");// FromArgb(#1979CA)//Color.RoyalBlue;

        /// <summary>
        ///     边框和聚焦颜色-深蓝
        /// </summary>
        public static Color FocusBorderColor = ColorTranslator.FromHtml("#1979CA");

        /// <summary>
        ///     背景颜色-白
        /// </summary>
        public static Color BackColor = Color.White;

        /// <summary>
        ///     默认字体/大小-微软雅黑/9f
        /// </summary>
        public static Font DefaultFont = new Font(fontName, 9f);
        public static string fontName
        {
            get
            {
                string fn = string.Empty;
                int major = Environment.OSVersion.Version.Major;
                fn = major >= 6 ? "微软雅黑" : "宋体";
                return fn;
            }
        }
        /// <summary>
        ///     默认的圆角大小/5
        /// </summary>
        public static int DefaultRadius = 5;

        public static void GraphicSetup(Graphics graphObj)
        {
            if (graphObj != null)
            {
                graphObj.CompositingQuality = CompositingQuality.AssumeLinear;
                //graphObj.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                graphObj.SmoothingMode = SmoothingMode.AntiAlias;
                graphObj.InterpolationMode = InterpolationMode.HighQualityBilinear;
            }
        }
    }
}