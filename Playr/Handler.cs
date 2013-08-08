using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leap;

namespace Playr
{
    class Handler : Listener
    {
        public List<IHandle> Handles;
        private Controller controller;
        public Handler() : base()
        {
            controller = new Controller();
            controller.AddListener(this);
            controller.SetPolicyFlags(Controller.PolicyFlag.POLICYBACKGROUNDFRAMES);
        }

        public void Shutdown()
        {
            controller.RemoveListener(this);
            base.Dispose();
        }

        override public void OnInit(Controller arg0)
        {
            foreach (IHandle handle in Handles)
            {
                if (handle.Options.firingMethod == IHandleOptions.FiringMethod.OnInit)
                    handle.InvokeAll(controller);
            }

        }

        override public void OnExit(Controller arg0)
        {
            foreach (IHandle handle in Handles)
            {
                if (handle.Options.firingMethod == IHandleOptions.FiringMethod.OnExit)
                    handle.InvokeAll(controller);
            }
        }

        override public void OnConnect(Controller arg0)
        {
            foreach (IHandle handle in Handles)
            {
                if (handle.Options.firingMethod == IHandleOptions.FiringMethod.OnConnect)
                    handle.InvokeAll(controller);
            }
        }

        override public void OnDisconnect(Controller arg0)
        {
            foreach (IHandle handle in Handles)
            {
                if (handle.Options.firingMethod == IHandleOptions.FiringMethod.OnDisconnect)
                    handle.InvokeAll(controller);
            }
        }

        override public void OnFocusGained(Controller arg0)
        {
            foreach (IHandle handle in Handles)
            {
                if (handle.Options.firingMethod == IHandleOptions.FiringMethod.OnFocusGained)
                    handle.InvokeAll(controller);
            }
        }

        override public void OnFocusLost(Controller arg0)
        {
            foreach (IHandle handle in Handles)
            {
                if (handle.Options.firingMethod == IHandleOptions.FiringMethod.OnFocusLost)
                    handle.InvokeAll(controller);
            }
        }

        override public void OnFrame(Controller arg0)
        {
            foreach (IHandle handle in Handles)
            {
                if (handle.Options.firingMethod == IHandleOptions.FiringMethod.OnFrame)
                    handle.InvokeAll(controller);
            }
        }
    }
}
