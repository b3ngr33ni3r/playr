using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Leap;

namespace Playr
{
    class App : Form
    {
        /*class AppListener : Listener
        {
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

            
            public const int KEYEVENTF_EXTENDEDKEY = 0x0001; //Key down flag
            public const int KEYEVENTF_KEYUP = 0x0002; //Key up flag
            public const int VK_CONTROL = 0x11; //Right Control key code
            public const int VK_SHIFT = 0x10;
            public const int VK_TAB = 0x09;

            public AppListener()
                : base()
            {

            }

            override public void OnInit(Controller arg0)
            {

            }

            override public void OnExit(Controller arg0)
            {

            }

            override public void OnConnect(Controller arg0)
            {

            }

            override public void OnDisconnect(Controller arg0)
            {

            }

            override public void OnFocusGained(Controller arg0)
            {

            }

            override public void OnFocusLost(Controller arg0)
            {

            }

            private long[] staleStamps = new long[3]{-1,-1,-1};
            private Queue<Vector> positions = new Queue<Vector>();


            override public void OnFrame(Controller arg0)
            {
                Frame frame = arg0.Frame();
                HandList hands = frame.Hands;
                foreach (Hand h in hands)
                {
                    if (h.IsValid)
                    {
                        Vector location = h.PalmPosition; 
                        Vector velocity = h.PalmVelocity;

                        positions.Enqueue(location);

                        if (MaxAbsValue(velocity,"x") && velocity.x > 0 && PreviousPositionsContain(200,"x"))
                        {
                            //System.Windows.Forms.MessageBox.Show("timestamp: "+frame.Timestamp);

                            if (staleStamps[0] == -1 || (staleStamps[0] != -1 && staleStamps[0] <= frame.Timestamp - 2000000))
                            {
                                staleStamps[0] = frame.Timestamp;
                                keybd_event(VK_CONTROL, 0x45, KEYEVENTF_EXTENDEDKEY, 0);
                                keybd_event(VK_TAB, 0x45, KEYEVENTF_EXTENDEDKEY, 0);
                                keybd_event(VK_CONTROL, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                                keybd_event(VK_TAB, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                                RecognizedEvent();
                            }
                        }
                        else if (MaxAbsValue(velocity, "x") && velocity.x < 0 && PreviousPositionsContain(200, "x"))
                        {
                            if (staleStamps[1] == -1 || (staleStamps[1] != -1 && staleStamps[1] <= frame.Timestamp - 2000000))
                            {
                                staleStamps[1] = frame.Timestamp;
                                keybd_event(VK_CONTROL, 0x45, KEYEVENTF_EXTENDEDKEY, 0);
                                keybd_event(VK_SHIFT, 0x45, KEYEVENTF_EXTENDEDKEY, 0);
                                keybd_event(VK_TAB, 0x45, KEYEVENTF_EXTENDEDKEY, 0);
                                keybd_event(VK_CONTROL, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                                keybd_event(VK_SHIFT, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                                keybd_event(VK_TAB, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0);
                                RecognizedEvent();
                            }
                        }
                        else if (MaxAbsValue(velocity, "y") && PreviousPositionsContain(400, "y"))
                        {
                            if (staleStamps[2] == -1 || (staleStamps[2] != -1 && staleStamps[2] <= frame.Timestamp - 2000000))
                            {
                                staleStamps[2] = frame.Timestamp;
                                Shell32.Shell objShel = new Shell32.Shell();
                                // Hide the desktop
                                ((Shell32.IShellDispatch4)objShel).ToggleDesktop();
                                RecognizedEvent();
                            }
                        }

                        if (positions.Count > 100)
                            positions.Clear();
                    }
                }
            }

            private bool MaxAbsValue(Vector v, string axis)
            {
                switch (axis.ToLower())
                {
                    case "x":
                        return (Math.Abs(v.x) > Math.Abs(v.y) && Math.Abs(v.x) > Math.Abs(v.z)); 
                    case "y":
                        return (Math.Abs(v.y) > Math.Abs(v.x) && Math.Abs(v.y) > Math.Abs(v.z));
                    case "z":
                        return (Math.Abs(v.z) > Math.Abs(v.x) && Math.Abs(v.z) > Math.Abs(v.y));
                }
                return false;
            }

            private bool PreviousPositionsContain(float distance, string axis)
            {
                switch (axis.ToLower())
                {
                    case "x":
                        float lowestx = positions.Peek().x;
                        float highestx = positions.Peek().x;
                        foreach (Vector v in positions)
                        {
                            if (v.x < lowestx)
                                lowestx = v.x;
                            if (v.x > highestx)
                                highestx = v.x;
                        }
                        return ((((lowestx < 0) ? Math.Abs(lowestx) : lowestx) + ((highestx < 0) ? Math.Abs(highestx) : highestx)) > distance);
                    case "y":
                        float lowesty = positions.Peek().y;
                        float highesty = positions.Peek().y;
                        foreach (Vector v in positions)
                        {
                            if (v.y < lowesty)
                                lowesty = v.y;
                            if (v.y > highesty)
                                highesty = v.y;
                        }
                        return ((((lowesty < 0) ? Math.Abs(lowesty) : lowesty) + ((highesty < 0) ? Math.Abs(highesty) : highesty)) > distance);
                    case "z":
                        float lowestz = positions.Peek().z;
                        float highestz = positions.Peek().z;
                        foreach (Vector v in positions)
                        {
                            if (v.z < lowestz)
                                lowestz = v.z;
                            if (v.z > highestz)
                                highestz = v.z;
                        }
                        return ((((lowestz < 0) ? Math.Abs(lowestz) : lowestz) + ((highestz < 0) ? Math.Abs(highestz) : highestz)) > distance);
                }
                return false;
            }

            private void RecognizedEvent()
            {
                positions.Clear();
            }
        }*/

        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);

        private NotifyIcon notifyIcon;
        
        public App()
        {
            this.Visible = false;
            this.Hide();
            base.SetVisibleCore(false);

            System.Drawing.Icon icon = System.Drawing.Icon.FromHandle(Playr.Properties.Resources.playr_icon.GetHicon());
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = icon;
            notifyIcon.Visible = true;
            notifyIcon.ContextMenu = new ContextMenu(new MenuItem[] { new MenuItem("Exit", delegate(object s, EventArgs e) { this.OnFormClosing(new FormClosingEventArgs(CloseReason.UserClosing, false)); }) });

            DestroyIcon(icon.Handle);

            Handler handler = new Handler();

            IHandle ToggleDesktop = new IHandle();
            ToggleDesktop.Actions += delegate(object sender,IHandle.IHandleActionArguments actionArgs){
                IHandle.IHandleEventArguments arguments = (IHandle.IHandleEventArguments)sender;
                //Controller controller = actionArgs.Controller;

                bool toggleState = false;

                foreach (PositionTracker tracker in arguments.Tracker.Values)
                    if (tracker.ContainsDistanceOnAxis(300,Axis.Y))
                        toggleState = true;

                if (toggleState)
                {
                    Shell32.Shell objShel = new Shell32.Shell();
                    // Hide the desktop
                    ((Shell32.IShellDispatch4)objShel).ToggleDesktop();
                }
            };

            handler.Handles.Add(ToggleDesktop);
            this.FormClosing += delegate(object sender, FormClosingEventArgs e) { handler.Shutdown(); notifyIcon.Visible = false; this.OnFormClosed(new FormClosedEventArgs(CloseReason.UserClosing)); };
        }

        
    }
}
