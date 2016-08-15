using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Reflection;

namespace System.Windows.Forms
{
    public class ItemString
    {
        public string Text { get; set; }
    }

    //[DefaultMember("Item")]
    //[ListBindable(false)]
    [Serializable]
    public class ListItem//: IList, ICollection, IEnumerable
    {
        private int _index;

        public int Index
        {
            get { return _index; }
            set
            {
              //  value += value;
                _index = value;
            }
        }

        public Rectangle Bounds { get; set; }
        public int Alpha { get; set; }
        public string Text { get; set; }
        public string Value { get; set; } 
        //public int Count { get; } 
        //public bool IsReadOnly { get; } 
        //public int Add(object item); 
        //public void AddRange(object[] items); 
        //public void Clear(); 
        //public bool Contains(object value); 
        //public void CopyTo(object[] destination, int arrayIndex); 
        //public IEnumerator GetEnumerator(); 
        //public int IndexOf(object value); 
        //public void Insert(int index, object item); 
        //public void Remove(object value); 
        //public void RemoveAt(int index);
    }

    public enum MouseState
    {
        move,
        press,
        up,
        leave
    }
    public enum RoundStyle
    {
        /// <summary>
        ///     四个角都不是圆角。
        /// </summary>
        None = 0,

        /// <summary>
        ///     四个角都为圆角。
        /// </summary>
        All = 1,

        /// <summary>
        ///     左边两个角为圆角。
        /// </summary>
        Left = 2,

        /// <summary>
        ///     右边两个角为圆角。
        /// </summary>
        Right = 3,

        /// <summary>
        ///     上边两个角为圆角。
        /// </summary>
        Top = 4,

        /// <summary>
        ///     下边两个角为圆角。
        /// </summary>
        Bottom = 5,

        /// <summary>
        ///     左下角为圆角。
        /// </summary>
        BottomLeft = 6,

        /// <summary>
        ///     右下角为圆角。
        /// </summary>
        BottomRight = 7
    }
    public enum ControlState
    {
        /// <summary>
        ///     正常。
        /// </summary>
        Normal,

        /// <summary>
        ///     鼠标进入。
        /// </summary>
        Hover,

        /// <summary>
        ///     鼠标按下。
        /// </summary>
        Pressed,

        /// <summary>
        ///     获得焦点。
        /// </summary>
        Focused
    }

    public enum Alpha
    {
        /// <summary>
        ///     100
        /// </summary>
        MoveOrUp = 100,

        /// <summary>
        ///     130
        /// </summary>
        PressOrDown = 130,

        /// <summary>
        ///     80
        /// </summary>
        Normal = 80, //80

        /// <summary>
        ///     60
        /// </summary>
        DisEnable = 60,

        /// <summary>
        ///     0
        /// </summary>
        None = 0,

        /// <summary>
        ///     255
        /// </summary>
        Max = 255,

        /// <summary>
        ///     200
        /// </summary>
        a200 = 200,

        /// <summary>
        ///     180
        /// </summary>
        a180 = 180,

        /// <summary>
        ///     150
        /// </summary>
        a150 = 150
    }

    public enum ControlBoxState
    {
        Normal,
        Hover,
        Pressed,
        PressedLeave
    }

    public enum MouseOperate
    {
        Move,
        Down,
        Up,
        Leave,
        Hover,
        Normal
    }

    public enum MobileStyle
    {
        None,
        TitleMobile,
        Mobile
    }

    public enum ControlBoxStyle
    {
        None,
        Minimize,
        Maximize,
        Close,
        SysBottom
    }

    public enum GrayscaleStyle
    {
        BT907,
        RMY,
        Y
    }

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