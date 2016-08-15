using System.Drawing;
using System.Drawing.Drawing2D;

namespace System.Windows.Forms
{
    public partial class CMessageBoxBase : CForm
    {
        private const int Spacing = 12;
        private Button[] _innerButtons;
        private Rectangle _messageRect;

        public CMessageBoxBase()
        {
            InitializeComponent();
        }

        protected new Rectangle IconRect { get; private set; }
        protected MessageBoxArgs Message { get; private set; }

        protected Rectangle MessageRect
        {
            get { return _messageRect; }
        }

        private void BtnClick(object sender, EventArgs e)
        {
            DialogResult = ((Button) sender).DialogResult;
            Close();
        }

        private void CalcFinalSizes()
        {
            var num = 0;
            foreach (CButton button in _innerButtons)
            {
                if (num != 0)
                {
                    num += 12;
                }
                num += button.Width;
            }
            var num4 = _messageRect.Bottom + 12;
            if ((Message.Icon != null) && ((IconRect.Bottom + 12) > num4))
            {
                num4 = IconRect.Bottom + 12;
            }
            var size2 = MinimumSizeFromMiniminClientSize();
            var width = size2.Width;
            if (width == 0)
            {
                width = SystemInformation.MinimumWindowSize.Width;
            }
            if (width < (_messageRect.Right + 12))
            {
                width = _messageRect.Right + 12;
            }
            if (width < ((num + 12) + 12))
            {
                width = (num + 12) + 12;
            }
            var num7 = (SystemInformation.WorkingArea.Width/3)*2;
            var num9 =
                (((TextRenderer.MeasureText(Text, CaptionFont, new Size(num7, CaptionHeight),
                    TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine).Width +
                   MiniSize.Width) + MaxSize.Width) + CloseBoxSize.Width) + SysBottomSize.Width;
            var num10 = Math.Min(num7, num9);
            if (width < num10)
            {
                width = num10;
            }
            Width = width + BorderPadding.Right;
            Height = ((num4 + _innerButtons[0].Height) + BorderPadding.Bottom) + 3;
            if (Height < size2.Height)
            {
                Height = size2.Height;
                num4 = ((Height - _innerButtons[0].Height) - BorderPadding.Bottom) - 3;
            }
            var x = BorderPadding.Left + ((DisplayRectangle.Width - num)/2);
            for (var i = 0; i < _innerButtons.Length; i++)
            {
                _innerButtons[i].Location = new Point(x, num4 + 2);
                x += _innerButtons[i].Width + 12;
            }
            if (Message.Icon == null)
            {
                _messageRect.Offset((width - (_messageRect.Right + 12))/2, 0);
            }
            if ((Message.Icon != null) && (_messageRect.Height < IconRect.Height))
            {
                _messageRect.Offset(0, (IconRect.Height - _messageRect.Height)/2);
            }
        }

        private void CalcIconBounds()
        {
            var x = BorderPadding.Left + 12;
            var y = (BorderPadding.Top + CaptionHeight) + 12;
            if (Message.Icon != null)
            {
                IconRect = new Rectangle(x, y, Message.Icon.Width, Message.Icon.Height);
            }
            else
            {
                IconRect = new Rectangle(x, y, 0, 0);
            }
        }

        private void CalcMessageBounds()
        {
            var y = IconRect.Y;
            var x = (Message.Icon == null) ? (BorderPadding.Left + 12) : (IconRect.Right + 12);
            var workingArea = SystemInformation.WorkingArea;
            var size2 = MaximumSizeFromMaximinClientSize();
            var proposedSize = size2;
            if (proposedSize.Width <= 0)
            {
                proposedSize.Width = (workingArea.Width/3)*2;
            }
            if (proposedSize.Height <= 0)
            {
                proposedSize.Height = workingArea.Height;
            }
            proposedSize.Width -= (BorderPadding.Horizontal + 12) + x;
            proposedSize.Height = 0x7fffffff;
            if (proposedSize.Width < 10)
            {
                proposedSize.Width = 10;
            }
            SizeF ef = TextRenderer.MeasureText(Message.Text, Font, proposedSize,
                TextFormatFlags.HidePrefix | TextFormatFlags.EndEllipsis | TextFormatFlags.ExternalLeading |
                TextFormatFlags.WordBreak);
            var width = (int) Math.Ceiling(ef.Width);
            var height = size2.Height;
            if (height <= 0)
            {
                height = workingArea.Height;
            }
            height -= (((BorderPadding.Horizontal + CaptionHeight) + y) + 12) + _innerButtons[0].Height;
            if (height < 10)
            {
                height = 10;
            }
            var num4 = (int) Math.Ceiling(ef.Height);
            if (num4 > height)
            {
                num4 = height;
            }
            _messageRect = new Rectangle(x, y, width, num4);
        }

        private void CreateButtons()
        {
            var buttons = Message.Buttons;
            var defaultButton = Message.DefaultButton;
            Button[] buttonArray = null;
            switch (buttons)
            {
                case MessageBoxButtons.OKCancel:
                    buttonArray = new Button[]
                    {
                        InnerCreateButton(DialogButton.BtnOk, DialogResult.OK),
                        InnerCreateButton(DialogButton.BtnCancel, DialogResult.Cancel)
                    };
                    CancelButton = buttonArray[1];
                    break;

                case MessageBoxButtons.AbortRetryIgnore:
                    buttonArray = new Button[]
                    {
                        InnerCreateButton(DialogButton.BtnAbort, DialogResult.Abort),
                        InnerCreateButton(DialogButton.BtnRetry, DialogResult.Retry),
                        InnerCreateButton(DialogButton.BtnIgnore, DialogResult.Ignore)
                    };
                    CancelButton = buttonArray[2];
                    break;

                case MessageBoxButtons.YesNoCancel:
                    buttonArray = new Button[]
                    {
                        InnerCreateButton(DialogButton.BtnYes, DialogResult.Yes),
                        InnerCreateButton(DialogButton.BtnNo, DialogResult.No),
                        InnerCreateButton(DialogButton.BtnCancel, DialogResult.Cancel)
                    };
                    CancelButton = buttonArray[2];
                    break;

                case MessageBoxButtons.YesNo:
                    buttonArray = new Button[]
                    {
                        InnerCreateButton(DialogButton.BtnYes, DialogResult.Yes),
                        InnerCreateButton(DialogButton.BtnNo, DialogResult.No)
                    };
                    CancelButton = buttonArray[1];
                    break;

                case MessageBoxButtons.RetryCancel:
                    buttonArray = new Button[]
                    {
                        InnerCreateButton(DialogButton.BtnRetry, DialogResult.Retry),
                        InnerCreateButton(DialogButton.BtnCancel, DialogResult.Cancel)
                    };
                    CancelButton = buttonArray[1];
                    break;

                default:
                    buttonArray = new Button[] {InnerCreateButton(DialogButton.BtnOk, DialogResult.OK)};
                    CancelButton = buttonArray[0];
                    break;
            }
            var button2 = defaultButton;
            if (button2 != MessageBoxDefaultButton.Button1)
            {
                if (button2 != MessageBoxDefaultButton.Button2)
                {
                    if ((button2 == MessageBoxDefaultButton.Button3) && (buttonArray.Length > 2))
                    {
                        AcceptButton = buttonArray[2];
                    }
                }
                else if (buttonArray.Length > 1)
                {
                    AcceptButton = buttonArray[1];
                }
            }
            else
            {
                AcceptButton = buttonArray[0];
            }
            _innerButtons = buttonArray;
        }

        protected virtual DialogResult DoShowDialog(IWin32Window owner)
        {
            ShowDialog(owner);
            return DialogResult;
        }

        public void DrawAlphaPart(Form form, Graphics g)
        {
            Color[] colorArray2 =
            {
                Color.FromArgb(0, Color.White), Color.FromArgb(0xe1, Color.White),
                Color.FromArgb(240, Color.White)
            };
            var numArray = new float[3];
            numArray[1] = 0.38f;
            numArray[2] = 1f;
            var numArray2 = numArray;
            var blend = new ColorBlend(3);
            blend.Colors = colorArray2;
            blend.Positions = numArray2;
            var num = 0x23;
            var rect = new RectangleF(0f, 0f, form.Width, form.Height - 0x23);
            using (var brush = new LinearGradientBrush(rect, Color.White, Color.Black, LinearGradientMode.Vertical))
            {
                brush.InterpolationColors = blend;
                g.FillRectangle(brush, rect);
            }
            using (var pen = new Pen(Color.FromArgb(0xff, Color.White), 0.1f))
            {
                g.DrawLine(pen, new Point(0, form.Height - num), new Point(form.Width, form.Height - num));
            }
            using (var brush2 = new SolidBrush(Color.FromArgb(0xcd, Color.White)))
            {
                g.FillRectangle(brush2, new Rectangle(0, (form.Height - num) + 1, form.Width, (form.Height - num) + 1));
            }
        }

        //private void InitializeComponent()
        //{

        //}

        private CButton InnerCreateButton(string text, DialogResult result)
        {
            var button = new CButton();
            button.Round=new Padding(1);
            button.Size = new Size(60, 0x1a);
            button.ShowBorder = false;
            button.UseVisualStyleBackColor = true;
            button.Text = text;
            button.BackColor = Color.Transparent;
            button.DialogResult = result;
            button.Click += BtnClick;
            Controls.Add(button);
            return button;
        }

        private void MessageBoxForm_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            DrawAlphaPart(this, g);
            if (Message.Icon != null)
            {
                g.DrawImage(Message.Icon, IconRect);
            }
            if (!string.IsNullOrEmpty(Message.Text))
            {
                TextRenderer.DrawText(g, Message.Text, Font, _messageRect, ForeColor,
                    TextFormatFlags.HidePrefix | TextFormatFlags.EndEllipsis | TextFormatFlags.ExternalLeading |
                    TextFormatFlags.WordBreak);
            }
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (e.KeyChar == '\x0003')
            {
                e.Handled = true;
                Clipboard.SetDataObject(Message.Caption + Environment.NewLine + Environment.NewLine + Message.Text, true);
            }
            base.OnKeyPress(e);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible && !ContainsFocus)
            {
                Activate();
            }
        }

        private DialogResult ShowMessageBoxDialog()
        {
            Text = Message.Caption;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MinimizeBox = false;
            MaximizeBox = false;
            var owner = Message.Owner;
            if (owner == null)
            {
                var activeForm = ActiveForm;
                if ((activeForm != null) && !activeForm.InvokeRequired)
                {
                    owner = activeForm;
                }
            }
            if (owner != null)
            {
                var control = owner as Control;
                if (control != null)
                {
                    if (!control.Visible)
                    {
                        owner = null;
                    }
                    else
                    {
                        var form = control.FindForm();
                        if ((form != null) &&
                            ((!form.Visible || (form.WindowState == FormWindowState.Minimized)) ||
                             ((form.Right <= 0) || (form.Bottom <= 0))))
                        {
                            owner = null;
                        }
                    }
                }
            }
            if (owner == null)
            {
                ShowInTaskbar = true;
                StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                ShowInTaskbar = false;
                StartPosition = FormStartPosition.CenterParent;
            }
            CreateButtons();
            CalcIconBounds();
            CalcMessageBounds();
            CalcFinalSizes();
            var form2 = owner as Form;
            if ((form2 != null) && form2.TopMost)
            {
                TopMost = true;
            }
            TopMost = true;

            return DoShowDialog(owner);
        }

        public DialogResult ShowMessageBoxDialog(MessageBoxArgs message)
        {
            Message = message;
            return ShowMessageBoxDialog();
        }
    }
}