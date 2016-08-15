using System.Drawing;

namespace System.Windows.Forms
{
    public class ControlBoxManager : IDisposable
    {
        private ControlBoxState _closBoxState;
        private ControlBoxState _maximizeBoxState;
        private ControlBoxState _minimizeBoxState;
        private bool _mouseDown;
        private CForm _owner;
        private ControlBoxState _SysBottomState;

        public ControlBoxManager(CForm owner)
        {
            _owner = owner;
        }

        public Rectangle CloseBoxRect
        {
            get
            {
                if (CloseBoxVisibale)
                {
                    var controlBoxOffset = ControlBoxOffset;
                    var closeBoxSize = _owner.CloseBoxSize;
                    return new Rectangle((_owner.Width - controlBoxOffset.X) - closeBoxSize.Width, controlBoxOffset.Y,
                        closeBoxSize.Width, closeBoxSize.Height);
                }
                return Rectangle.Empty;
            }
        }

        public ControlBoxState CloseBoxState
        {
            get { return _closBoxState; }
            protected set
            {
                if (_closBoxState != value)
                {
                    _closBoxState = value;
                    if (_owner != null)
                    {
                        Invalidate(CloseBoxRect);
                    }
                }
            }
        }

        public bool CloseBoxVisibale
        {
            get { return _owner.ControlBox; }
        }

        public Point ControlBoxOffset
        {
            get { return _owner.ControlBoxOffset; }
        }

        public int ControlBoxSpace
        {
            get { return _owner.ControlBoxSpace; }
        }

        public Rectangle MaximizeBoxRect
        {
            get
            {
                if (MaximizeBoxVisibale)
                {
                    var controlBoxOffset = ControlBoxOffset;
                    var maxSize = _owner.MaxSize;
                    return new Rectangle((CloseBoxRect.X - ControlBoxSpace) - maxSize.Width, controlBoxOffset.Y,
                        maxSize.Width, maxSize.Height);
                }
                return Rectangle.Empty;
            }
        }

        public ControlBoxState MaximizeBoxState
        {
            get { return _maximizeBoxState; }
            protected set
            {
                if (_maximizeBoxState != value)
                {
                    _maximizeBoxState = value;
                    if (_owner != null)
                    {
                        Invalidate(MaximizeBoxRect);
                    }
                }
            }
        }

        public bool MaximizeBoxVisibale
        {
            get { return (_owner.ControlBox && _owner.MaximizeBox); }
        }

        public Rectangle MinimizeBoxRect
        {
            get
            {
                if (MinimizeBoxVisibale)
                {
                    var controlBoxOffset = ControlBoxOffset;
                    var miniSize = _owner.MiniSize;
                    return
                        new Rectangle(
                            MaximizeBoxVisibale
                                ? ((MaximizeBoxRect.X - ControlBoxSpace) - miniSize.Width)
                                : ((CloseBoxRect.X - ControlBoxSpace) - miniSize.Width), controlBoxOffset.Y,
                            miniSize.Width, miniSize.Height);
                }
                return Rectangle.Empty;
            }
        }

        public ControlBoxState MinimizeBoxState
        {
            get { return _minimizeBoxState; }
            protected set
            {
                if (_minimizeBoxState != value)
                {
                    _minimizeBoxState = value;
                    if (_owner != null)
                    {
                        Invalidate(MinimizeBoxRect);
                    }
                }
            }
        }

        public bool MinimizeBoxVisibale
        {
            get { return (_owner.ControlBox && _owner.MinimizeBox); }
        }

        public Rectangle SysBottomRect
        {
            get
            {
                if (SysBottomVisibale)
                {
                    var controlBoxOffset = ControlBoxOffset;
                    var sysBottomSize = _owner.SysBottomSize;
                    return
                        new Rectangle(
                            MinimizeBoxVisibale
                                ? ((MinimizeBoxRect.X - ControlBoxSpace) - sysBottomSize.Width)
                                : (MaximizeBoxVisibale
                                    ? ((MaximizeBoxRect.X - ControlBoxSpace) - sysBottomSize.Width)
                                    : ((CloseBoxRect.X - ControlBoxSpace) - sysBottomSize.Width)), controlBoxOffset.Y,
                            sysBottomSize.Width, sysBottomSize.Height);
                }
                return Rectangle.Empty;
            }
        }

        public ControlBoxState SysBottomState
        {
            get { return _SysBottomState; }
            protected set
            {
                if (_SysBottomState != value)
                {
                    _SysBottomState = value;
                    if (_owner != null)
                    {
                        Invalidate(SysBottomRect);
                    }
                }
            }
        }

        public bool SysBottomVisibale
        {
            get { return _owner.SysBottomVisibale; }
        }

        public void Dispose()
        {
            _owner = null;
        }

        private void HideToolTip()
        {
            if (_owner != null)
            {
                _owner.ToolTip.Active = false;
            }
        }

        private void Invalidate(Rectangle rect)
        {
            _owner.Invalidate(rect);
        }

        private void ProcessMouseDown(Point mousePoint, Rectangle closeBoxRect, Rectangle minimizeBoxRect,
            Rectangle maximizeBoxRect, Rectangle sysbottomRect, bool closeBoxVisibale, bool minimizeBoxVisibale,
            bool maximizeBoxVisibale, bool sysbottomVisibale)
        {
            _mouseDown = true;
            if (closeBoxVisibale && closeBoxRect.Contains(mousePoint))
            {
                CloseBoxState = ControlBoxState.Pressed;
            }
            else if (minimizeBoxVisibale && minimizeBoxRect.Contains(mousePoint))
            {
                MinimizeBoxState = ControlBoxState.Pressed;
            }
            else if (SysBottomVisibale && sysbottomRect.Contains(mousePoint))
            {
                SysBottomState = ControlBoxState.Pressed;
            }
            else if (maximizeBoxVisibale && maximizeBoxRect.Contains(mousePoint))
            {
                MaximizeBoxState = ControlBoxState.Pressed;
            }
        }

        private void ProcessMouseLeave(bool closeBoxVisibale, bool minimizeBoxVisibale, bool maximizeBoxVisibale,
            bool sysbottomVisibale)
        {
            if (closeBoxVisibale)
            {
                if (CloseBoxState == ControlBoxState.Pressed)
                {
                    CloseBoxState = ControlBoxState.PressedLeave;
                }
                else
                {
                    CloseBoxState = ControlBoxState.Normal;
                }
            }
            if (minimizeBoxVisibale)
            {
                if (MinimizeBoxState == ControlBoxState.Pressed)
                {
                    MinimizeBoxState = ControlBoxState.PressedLeave;
                }
                else
                {
                    MinimizeBoxState = ControlBoxState.Normal;
                }
            }
            if (sysbottomVisibale)
            {
                if (SysBottomState == ControlBoxState.Pressed)
                {
                    SysBottomState = ControlBoxState.PressedLeave;
                }
                else
                {
                    SysBottomState = ControlBoxState.Normal;
                }
            }
            if (maximizeBoxVisibale)
            {
                if (MaximizeBoxState == ControlBoxState.Pressed)
                {
                    MaximizeBoxState = ControlBoxState.PressedLeave;
                }
                else
                {
                    MaximizeBoxState = ControlBoxState.Normal;
                }
            }
            HideToolTip();
        }

        private void ProcessMouseMove(Point mousePoint, Rectangle closeBoxRect, Rectangle minimizeBoxRect,
            Rectangle maximizeBoxRect, Rectangle sysbottomRect, bool closeBoxVisibale, bool minimizeBoxVisibale,
            bool maximizeBoxVisibale, bool sysbottomVisibale)
        {
            var toolTipText = string.Empty;
            var flag = true;
            if (closeBoxVisibale)
            {
                if (closeBoxRect.Contains(mousePoint))
                {
                    flag = false;
                    if (!_mouseDown)
                    {
                        if (CloseBoxState != ControlBoxState.Hover)
                        {
                            toolTipText = "关闭";
                        }
                        CloseBoxState = ControlBoxState.Hover;
                    }
                    else if (CloseBoxState == ControlBoxState.PressedLeave)
                    {
                        CloseBoxState = ControlBoxState.Pressed;
                    }
                }
                else if (!_mouseDown)
                {
                    CloseBoxState = ControlBoxState.Normal;
                }
                else if (CloseBoxState == ControlBoxState.Pressed)
                {
                    CloseBoxState = ControlBoxState.PressedLeave;
                }
            }
            if (minimizeBoxVisibale)
            {
                if (minimizeBoxRect.Contains(mousePoint))
                {
                    flag = false;
                    if (!_mouseDown)
                    {
                        if (MinimizeBoxState != ControlBoxState.Hover)
                        {
                            toolTipText = "最小化";
                        }
                        MinimizeBoxState = ControlBoxState.Hover;
                    }
                    else if (MinimizeBoxState == ControlBoxState.PressedLeave)
                    {
                        MinimizeBoxState = ControlBoxState.Pressed;
                    }
                }
                else if (!_mouseDown)
                {
                    MinimizeBoxState = ControlBoxState.Normal;
                }
                else if (MinimizeBoxState == ControlBoxState.Pressed)
                {
                    MinimizeBoxState = ControlBoxState.PressedLeave;
                }
            }
            if (maximizeBoxVisibale)
            {
                if (maximizeBoxRect.Contains(mousePoint))
                {
                    flag = false;
                    if (!_mouseDown)
                    {
                        if (MaximizeBoxState != ControlBoxState.Hover)
                        {
                            toolTipText = (_owner.WindowState == FormWindowState.Maximized) ? "还原" : "最大化";
                        }
                        MaximizeBoxState = ControlBoxState.Hover;
                    }
                    else if (MaximizeBoxState == ControlBoxState.PressedLeave)
                    {
                        MaximizeBoxState = ControlBoxState.Pressed;
                    }
                }
                else if (!_mouseDown)
                {
                    MaximizeBoxState = ControlBoxState.Normal;
                }
                else if (MaximizeBoxState == ControlBoxState.Pressed)
                {
                    MaximizeBoxState = ControlBoxState.PressedLeave;
                }
            }
            if (sysbottomVisibale)
            {
                if (sysbottomRect.Contains(mousePoint))
                {
                    flag = false;
                    if (!_mouseDown)
                    {
                        if (SysBottomState != ControlBoxState.Hover)
                        {
                            toolTipText = _owner.SysBottomToolTip;
                        }
                        SysBottomState = ControlBoxState.Hover;
                    }
                    else if (SysBottomState == ControlBoxState.PressedLeave)
                    {
                        SysBottomState = ControlBoxState.Pressed;
                    }
                }
                else if (!_mouseDown)
                {
                    SysBottomState = ControlBoxState.Normal;
                }
                else if (SysBottomState == ControlBoxState.Pressed)
                {
                    SysBottomState = ControlBoxState.PressedLeave;
                }
            }
            if (toolTipText != string.Empty)
            {
                HideToolTip();
                ShowTooTip(toolTipText);
            }
            if (flag)
            {
                HideToolTip();
            }
        }

        public void ProcessMouseOperate(Point mousePoint, MouseOperate operate)
        {
            if (_owner.ControlBox)
            {
                var closeBoxRect = CloseBoxRect;
                var minimizeBoxRect = MinimizeBoxRect;
                var maximizeBoxRect = MaximizeBoxRect;
                var sysBottomRect = SysBottomRect;
                var closeBoxVisibale = CloseBoxVisibale;
                var minimizeBoxVisibale = MinimizeBoxVisibale;
                var maximizeBoxVisibale = MaximizeBoxVisibale;
                var sysBottomVisibale = SysBottomVisibale;
                switch (operate)
                {
                    case MouseOperate.Move:
                        ProcessMouseMove(mousePoint, closeBoxRect, minimizeBoxRect, maximizeBoxRect, sysBottomRect,
                            closeBoxVisibale, minimizeBoxVisibale, maximizeBoxVisibale, sysBottomVisibale);
                        return;

                    case MouseOperate.Down:
                        ProcessMouseDown(mousePoint, closeBoxRect, minimizeBoxRect, maximizeBoxRect, sysBottomRect,
                            closeBoxVisibale, minimizeBoxVisibale, maximizeBoxVisibale, sysBottomVisibale);
                        return;

                    case MouseOperate.Up:
                        ProcessMouseUP(mousePoint, closeBoxRect, minimizeBoxRect, maximizeBoxRect, sysBottomRect,
                            closeBoxVisibale, minimizeBoxVisibale, maximizeBoxVisibale, sysBottomVisibale);
                        return;

                    case MouseOperate.Leave:
                        ProcessMouseLeave(closeBoxVisibale, minimizeBoxVisibale, maximizeBoxVisibale, sysBottomVisibale);
                        return;

                    case MouseOperate.Hover:
                        return;
                }
            }
        }

        private void ProcessMouseUP(Point mousePoint, Rectangle closeBoxRect, Rectangle minimizeBoxRect,
            Rectangle maximizeBoxRect, Rectangle sysbottomRect, bool closeBoxVisibale, bool minimizeBoxVisibale,
            bool maximizeBoxVisibale, bool sysbottomVisible)
        {
            _mouseDown = false;
            if (closeBoxVisibale)
            {
                if (closeBoxRect.Contains(mousePoint) && (CloseBoxState == ControlBoxState.Pressed))
                {
                    _owner.Close();
                    CloseBoxState = ControlBoxState.Normal;
                    return;
                }
                CloseBoxState = ControlBoxState.Normal;
            }
            if (minimizeBoxVisibale)
            {
                if (minimizeBoxRect.Contains(mousePoint) && (MinimizeBoxState == ControlBoxState.Pressed))
                {
                    if (_owner.ShowInTaskbar)
                    {
                        // this._owner.SuspendLayout();
                        _owner.WindowState = FormWindowState.Minimized;
                        // this._owner.ResumeLayout(false);
                    }
                    else
                    {
                        _owner.Hide();
                    }
                    MinimizeBoxState = ControlBoxState.Normal;
                    return;
                }
                MinimizeBoxState = ControlBoxState.Normal;
            }
            if (sysbottomVisible)
            {
                if (sysbottomRect.Contains(mousePoint) && (SysBottomState == ControlBoxState.Pressed))
                {
                    _owner.SysbottomAv(_owner);
                    SysBottomState = ControlBoxState.Normal;
                    return;
                }
                MinimizeBoxState = ControlBoxState.Normal;
            }
            if (maximizeBoxVisibale)
            {
                if (maximizeBoxRect.Contains(mousePoint) && (MaximizeBoxState == ControlBoxState.Pressed))
                {
                    if (_owner.WindowState == FormWindowState.Maximized)
                    {
                        //  this._owner.SuspendLayout();
                        _owner.WindowState = FormWindowState.Normal;
                        // this._owner.ResumeLayout(true);
                    }
                    else
                    {
                        //  this._owner.SuspendLayout();
                        _owner.WindowState = FormWindowState.Maximized;
                        //  this._owner.ResumeLayout(true);
                    }
                    MaximizeBoxState = ControlBoxState.Normal;
                }
                else
                {
                    MaximizeBoxState = ControlBoxState.Normal;
                }
            }
        }

        private void ShowTooTip(string toolTipText)
        {
            if (_owner != null)
            {
                _owner.ToolTip.Active = true;
                _owner.ToolTip.SetToolTip(_owner, toolTipText);
            }
        }
    }
}