using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
// added for single instance
using System.Threading;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace myDewControllerPro3
{
    static class Program
    {
        // GUId of myDewControllerPro3
        static Mutex mutex = new Mutex(true, "{cdb7addd-fcbf-487f-8774-2e85e3269069}");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new myDewController());
                mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("Only one instance of myDewControllerPro3 can be run at a time.", "myDewControllerPro3", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }

            // Application.EnableVisualStyles();
            // Application.SetCompatibleTextRenderingDefault(false);
            // Application.Run(new myDewController());
        }
    }
}
