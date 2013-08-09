using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leap;

namespace Playr
{

    

    /**
     * this defines a single event
     * that can be recognized and triggered
     * 
     * by default, this fires OnFrame
     **/
    class IHandle
    {

        public class IHandleOptions
        {
            public static const long TIMESTAMP_NEVER_FIRED = -1;

            public enum FiringMethod
            {
                OnFrame, OnInit, OnExit, OnConnect, OnDisconnect, OnFocusGained, OnFocusLost
            }
            /*public struct RequiredDistance
            {
                public struct Entry{
                    public Axis axis;
                    public float distance;
                }
                public enum Axis { X, Y, Z }
                public Entry[] entries;
            }*/
            public FiringMethod firingMethod;
            public long firedTimestamp = TIMESTAMP_NEVER_FIRED;
            public long repeatTime = 200000;
            //public RequiredDistance requiredDistance;

            public IHandleOptions(FiringMethod method = FiringMethod.OnFrame)
            {
                firingMethod = method;
                //RequiredDistance.Entry entry;

                /*entry.axis = RequiredDistance.Axis.X;
                entry.distance = 200;
                requiredDistance.entries = new RequiredDistance.Entry[1] { entry };*/
            }

        }

        public event EventHandler<Controller> Actions;

        private List<Vector> _pastQueue = new List<Vector>();

        private IHandleOptions opts;
        public IHandleOptions Options
        {
            get{ return opts;}
            set{ opts = value;}
        }

        public IHandle(IHandleOptions opts)
        {
            this.opts = opts;
        }

        public IHandle()
        {
            this.opts = new IHandleOptions();
        }

        public bool InvokeAll(Controller controller, HandsTracker handsTracker = null, long frameTime = -1)
        {
            //now pass a combination of IHandleOptions and HandsTracker to the method

            if (frameTime == -1 && opts.firingMethod == IHandleOptions.FiringMethod.OnFrame)
                return false;

            if ((opts.firingMethod == IHandleOptions.FiringMethod.OnFrame) && (opts.firedTimestamp == IHandleOptions.TIMESTAMP_NEVER_FIRED || opts.firedTimestamp < frameTime - opts.repeatTime))
                Actions.Invoke(opts, controller);

            return true;
        }

    }
}
