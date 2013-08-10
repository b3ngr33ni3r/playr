using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leap;

namespace Playr
{
    namespace EventDriven
    {
        class MotionEventArgument
        {

            //technically since the sender would be a controller,
            //it doesn't make much sense to pass a frame..
            private Frame frame;
            public Frame Frame
            {
                get { return frame; }
            }
            
            public MotionEventArgument(Frame frame)
            {
                this.frame = frame;
            }
        }

        class OnFrameMotionEventArgument : MotionEventArgument
        {
            private List<Vector> motionsSinceLastMotion;
            public List<Vector> MotionsSinceLastMotion
            {
                get { return motionsSinceLastMotion; }
            }

            public OnFrameMotionEventArgument(Frame frame, List<Vector> motionsSinceLastMotion)
                : base(frame)
            {
                this.motionsSinceLastMotion = motionsSinceLastMotion;
            }
        }

        class Motion : Listener
        {
            private Controller motionController;
            private List<Vector> positionsCache;

            public Motion(bool requestBackground = true)
                : base()
            {
                positionsCache = new List<Vector>();
                motionController = new Controller();

                if (requestBackground)
                    motionController.SetPolicyFlags(Controller.PolicyFlag.POLICYBACKGROUNDFRAMES);

                motionController.AddListener(this);
            }

            ~Motion()
            {
                Dispose();
            }

            public void Dispose()
            {
                motionController.RemoveListener(this);
                motionController.Dispose();
            }

            public event EventHandler<MotionEventArgument> OnFrame;
            public event EventHandler<MotionEventArgument> OnInit;
            public event EventHandler<MotionEventArgument> OnExit;
            public event EventHandler<MotionEventArgument> OnConnect;
            public event EventHandler<MotionEventArgument> OnDisconnect;
            public event EventHandler<MotionEventArgument> OnFocusGained;
            public event EventHandler<MotionEventArgument> OnFocusLost;

            override public void OnFrame(Controller arg0)
            {
                OnFrame.Invoke(arg0, new OnFrameMotionEventArgument(arg0.Frame(),positionsCache));
                positionsCache.Add(
            }

            override public void OnInit(Controller arg0)
            {
                OnInit.Invoke(arg0, new MotionEventArgument(arg0.Frame()));
            }

            override public void OnExit(Controller arg0)
            {
                OnExit.Invoke(arg0, new MotionEventArgument(arg0.Frame()));
            }

            override public void OnConnect(Controller arg0)
            {
                OnConnect.Invoke(arg0, new MotionEventArgument(arg0.Frame()));
            }

            override public void OnDisconnect(Controller arg0)
            {
                OnDisconnect.Invoke(arg0, new MotionEventArgument(arg0.Frame()));
            }

            override public void OnFocusGained(Controller arg0)
            {
                OnFocusGained.Invoke(arg0, new MotionEventArgument(arg0.Frame()));
            }

            override public void OnFocusLost(Controller arg0)
            {
                OnFocusLost.Invoke(arg0, new MotionEventArgument(arg0.Frame()));
            }

        }
    }
}
