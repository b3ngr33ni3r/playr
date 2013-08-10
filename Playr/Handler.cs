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
        private HandsTracker handsTracker;
        public Handler() : base()
        {
            Handles = new List<IHandle>();
            handsTracker = new HandsTracker(1000);
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
            //Frame frame = arg0.Frame();
            foreach (IHandle handle in Handles)
            {
                if (handle.Options.firingMethod == IHandle.IHandleOptions.FiringMethod.OnInit)
                    handle.InvokeAll(controller/*,frame.Timestamp*/);
            }

        }

        override public void OnExit(Controller arg0)
        {
            //Frame frame = arg0.Frame();
            foreach (IHandle handle in Handles)
            {
                if (handle.Options.firingMethod == IHandle.IHandleOptions.FiringMethod.OnInit)
                    handle.InvokeAll(controller/*,frame.Timestamp*/);
            }
        }

        override public void OnConnect(Controller arg0)
        {
            //Frame frame = arg0.Frame();
            foreach (IHandle handle in Handles)
            {
                if (handle.Options.firingMethod == IHandle.IHandleOptions.FiringMethod.OnInit)
                    handle.InvokeAll(controller/*,frame.Timestamp*/);
            }
        }

        override public void OnDisconnect(Controller arg0)
        {
            //Frame frame = arg0.Frame();
            foreach (IHandle handle in Handles)
            {
                if (handle.Options.firingMethod == IHandle.IHandleOptions.FiringMethod.OnInit)
                    handle.InvokeAll(controller/*,frame.Timestamp*/);
            }
        }

        override public void OnFocusGained(Controller arg0)
        {
            //Frame frame = arg0.Frame();
            foreach (IHandle handle in Handles)
            {
                if (handle.Options.firingMethod == IHandle.IHandleOptions.FiringMethod.OnInit)
                    handle.InvokeAll(controller/*,frame.Timestamp*/);
            }
        }

        override public void OnFocusLost(Controller arg0)
        {
            //Frame frame = arg0.Frame();
            foreach (IHandle handle in Handles)
            {
                if (handle.Options.firingMethod == IHandle.IHandleOptions.FiringMethod.OnInit)
                    handle.InvokeAll(controller/*,frame.Timestamp*/);
            }
        }

        override public void OnFrame(Controller arg0)
        {
            Frame frame = arg0.Frame();
            handsTracker.AddOrUpdate(frame.Hands); //hey bitch, watch this...its gunna eat memory if you don't clean it out at a set interval
            handsTracker.Clean(frame.Timestamp, 800000);

            foreach (IHandle handle in Handles)
            {
                if (handle.Options.firingMethod == IHandle.IHandleOptions.FiringMethod.OnInit)
                    handle.InvokeAll(controller, handsTracker, frame.Timestamp);
            }
        }
    }
}
