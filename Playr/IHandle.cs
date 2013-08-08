using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leap;

namespace Playr
{

    public class IHandleOptions
    {
        public enum FiringMethod
        {
            OnFrame, OnInit, OnExit, OnConnect, OnDisconnect, OnFocusGained, OnFocusLost
        }
        public FiringMethod firingMethod;

        public IHandleOptions(FiringMethod method = FiringMethod.OnFrame)
        {
            firingMethod = method;
        }
    }

    /**
     * this defines a single event
     * that can be recognized and triggered
     * 
     * by default, this fires OnFrame
     **/
    class IHandle
    {
        
        public event EventHandler<Controller> Actions;

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

        public void InvokeAll(Controller controller)
        {
            Actions.Invoke(opts, controller);
        }
    }
}
