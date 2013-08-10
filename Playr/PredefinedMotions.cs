using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leap;

namespace Playr
{
    namespace EventDriven
    {
        class PredefinedMotions
        {
            protected PredefinedMotions()
            {

            }

            public static void ToggleDesktop(object sender, MotionEventArgument argument)
            {
                Shell32.Shell objShel = new Shell32.Shell();
                ((Shell32.IShellDispatch4)objShel).ToggleDesktop();
            }
        }
    }
}
