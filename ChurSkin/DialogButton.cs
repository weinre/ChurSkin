using System.Globalization;

namespace System.Windows.Forms
{
    internal static class DialogButton
    {
        internal static readonly string BtnAbort;
        internal static readonly string BtnCancel;
        internal static readonly string BtnIgnore;
        internal static readonly string BtnNo;
        internal static readonly string BtnOk;
        internal static readonly string BtnRetry;
        internal static readonly string BtnYes;

        static DialogButton()
        {
            if (lang == "zh-CN")
            {
                BtnCancel = "取消";
                BtnIgnore = "忽略";
                BtnNo = "否";
                BtnOk = "确定";
                BtnRetry = "重试";
                BtnYes = "是";
                BtnAbort = "中止";
            }
            else if (lang == "en-US" || lang == "en-NG")
            {
                BtnCancel = "Cancel";
                BtnIgnore = "Ignore";
                BtnNo = "No";
                BtnOk = "Ok";
                BtnRetry = "Retry";
                BtnYes = "Yes";
                BtnAbort = "Aborted";
            }
            else if (lang == "zh-HK" || lang == "zh-TW")
            {
                BtnCancel = "取消";
                BtnIgnore = "忽略";
                BtnNo = "否";
                BtnOk = "確定";
                BtnRetry = "重試";
                BtnYes = "是";
                BtnAbort = "中止";
            }
        }

        internal static string lang
        {
            get { return CultureInfo.InstalledUICulture.Name; }
        }
    }
}