using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinerGUI.Gui.Form
{
    public delegate void GlobalMouseClickEventHander(object sender, MouseEventArgs e);
    public abstract class FrameForm : System.Windows.Forms.Form
    {
        
        public event FramedEventHandler FramedEvents;

        public event GlobalMouseClickEventHander GlobalMouseClick;
        public FrameForm()
        {
            BindControlMouseClicks(this);
        }


        private void BindControlMouseClicks(Control con)
        {
            con.MouseClick += delegate (object sender, MouseEventArgs e)
            {
                TriggerMouseClicked(sender, e);
            };
            // bind to controls already added
            foreach (Control i in con.Controls)
            {
                BindControlMouseClicks(i);
            }
            // bind to controls added in the future
            con.ControlAdded += delegate (object sender, ControlEventArgs e)
            {
                BindControlMouseClicks(e.Control);
            };
        }
        private void TriggerMouseClicked(object sender, MouseEventArgs e)
        {
            if (GlobalMouseClick != null)
            {
                GlobalMouseClick(sender, e);
            }
        }
        public virtual void FireFrameEvent(String eventName, object data)
        {
            FramedEventHandler handler = FramedEvents;
            if (handler != null)
            {
                handler(eventName, data);
            }
        }
    }
}
