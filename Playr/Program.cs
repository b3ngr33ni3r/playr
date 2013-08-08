using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Playr
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// 
        /// closing has a weird workflow...
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            App a = new App();
            a.FormClosed += delegate(object s, FormClosedEventArgs e) { Application.Exit(); };
            Application.Run();

            
        }
    }
}
