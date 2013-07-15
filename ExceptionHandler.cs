using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.ComponentModel;

namespace BF2statisticsLauncher
{
    /// <summary>
    /// A simple object to handle exceptions thrown during runtime
    /// </summary>
    public sealed class ExceptionHandler
    {
        private ExceptionHandler() { }

        /// <summary>
        /// Handles an exception on the main thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="t"></param>
        public static void OnThreadException(object sender, ThreadExceptionEventArgs t)
        {
            // Display the Exception Form
            ExceptionForm EForm = new ExceptionForm(t.Exception, true);
            EForm.Message = "An unhandled exception was thrown while trying to preform the requested task.\r\n"
                + "If you click Continue, the application will attempt to ignore this error, and continue. "
                + "If you click Quit, the application will close immediatly.";
            DialogResult Result = EForm.ShowDialog();

            // Kill the form on abort
            if (Result == DialogResult.Abort)
                Application.Exit();
        }

        /// <summary>
        /// Handles cross thread exceptions, that are unrecoverable
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Display the Exception Form
            Exception Ex = e.ExceptionObject as Exception;
            ExceptionForm EForm = new ExceptionForm(Ex, false);
            EForm.Message = "An unhandled exception was thrown while trying to preform the requested task.";
            EForm.ShowDialog();
            Application.Exit();
        }
    }
}
