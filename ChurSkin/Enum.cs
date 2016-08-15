using System;
using System.Collections.Generic;
using System.Text;

namespace System.Windows.Forms
{
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
    public enum AnimationTypes
    {
        ZoomEffect,
        GradualCurtainEffect,
        FadeinFadeoutEffect,
        RotateZoomEffect,
        ThreeDTurn,
        Custom
    }
    public enum ControlBoxState
    {
        Normal,
        Hover,
        Pressed,
        PressedLeave
    }
    public enum ControlBoxStyle
    {
        None,
        Minimize,
        Maximize,
        Close,
        SysBottom
    }
    public enum MouseState
    {
        move,
        press,
        up,
        leave
    }
    public enum ControlState
    {
        Normal,
        Hover,
        Pressed,
        Focused
    }
    public enum MobileStyle
    {
        None,
        TitleMobile,
        Mobile
    }
    public enum MouseOperate
    {
        Move,
        Down,
        Up,
        Leave,
        Hover
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
}
