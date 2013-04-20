using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmplDotNet
{
    public class ModelingCauseEventArgs : EventArgs
    {
        private IEvent event_;
        public IEvent Event
        {
            get
            {
                return event_;
            }
        }

        public ModelingCauseEventArgs(IEvent causedEvent)
        {
            event_ = causedEvent;
        }
    }

    public delegate void ModelingCauseEventHandler(object sender, ModelingCauseEventArgs args);
}
