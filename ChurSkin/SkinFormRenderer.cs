using System.ComponentModel;
using System.Drawing;
using System.Security.Permissions;

namespace System.Windows.Forms
{
    public abstract class SkinFormRenderer
    {
        private static readonly object EventRenderSkinFormBorder;
        private static readonly object EventRenderSkinFormCaption;
        private static readonly object EventRenderSkinFormControlBox;
        private EventHandlerList _events;

        static SkinFormRenderer()
        {
            EventRenderSkinFormCaption = new object();
            EventRenderSkinFormBorder = new object();
            EventRenderSkinFormControlBox = new object();
        }

        protected EventHandlerList Events
        {
            get
            {
                if (_events == null)
                {
                    _events = new EventHandlerList();
                }
                return _events;
            }
        }

        public event SkinFormBorderRenderEventHandler RenderSkinFormBorder
        {
            add { AddHandler(EventRenderSkinFormBorder, value); }
            remove { RemoveHandler(EventRenderSkinFormBorder, value); }
        }

        public event SkinFormCaptionRenderEventHandler RenderSkinFormCaption
        {
            add { AddHandler(EventRenderSkinFormCaption, value); }
            remove { RemoveHandler(EventRenderSkinFormCaption, value); }
        }

        public event SkinFormControlBoxRenderEventHandler RenderSkinFormControlBox
        {
            add { AddHandler(EventRenderSkinFormControlBox, value); }
            remove { RemoveHandler(EventRenderSkinFormControlBox, value); }
        }

        [UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
        protected void AddHandler(object key, Delegate value)
        {
            Events.AddHandler(key, value);
        }

        public abstract Region CreateRegion(CForm form);

        public void DrawSkinFormBorder(SkinFormBorderRenderEventArgs e)
        {
            OnRenderSkinFormBorder(e);
            var handler = Events[EventRenderSkinFormBorder] as SkinFormBorderRenderEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void DrawSkinFormCaption(SkinFormCaptionRenderEventArgs e)
        {
            OnRenderSkinFormCaption(e);
            var handler = Events[EventRenderSkinFormCaption] as SkinFormCaptionRenderEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public void DrawSkinFormControlBox(SkinFormControlBoxRenderEventArgs e)
        {
            OnRenderSkinFormControlBox(e);
            var handler = Events[EventRenderSkinFormControlBox] as SkinFormControlBoxRenderEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public abstract void InitSkinForm(CForm form);
        protected abstract void OnRenderSkinFormBorder(SkinFormBorderRenderEventArgs e);
        protected abstract void OnRenderSkinFormCaption(SkinFormCaptionRenderEventArgs e);
        protected abstract void OnRenderSkinFormControlBox(SkinFormControlBoxRenderEventArgs e);

        [UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
        protected void RemoveHandler(object key, Delegate value)
        {
            Events.RemoveHandler(key, value);
        }
    }
}