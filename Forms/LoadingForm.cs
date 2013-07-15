using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace BF2statisticsLauncher
{
    public partial class LoadingForm : Form
    {
        const int WM_SYSCOMMAND = 0x0112;

        const int SC_MOVE = 0xF010;

        const int WS_SYSMENU = 0x80000;

        /// <summary>
        /// Hides the Close, Minimize, and Maximize buttons
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style &= ~WS_SYSMENU;
                return cp;
            }
        }

        /// <summary>
        /// Our isntance of the update form
        /// </summary>
        private static LoadingForm Instance;

        /// <summary>
        /// Text alignment
        /// </summary>
        public static HorizontalAlignment TextAlign = HorizontalAlignment.Center;

        /// <summary>
        /// Delegate for cross thread call to close
        /// </summary>
        private delegate void CloseDelegate();

        /// <summary>
        /// Delegate for cross thread call to update text
        /// </summary>
        private delegate void UpdateStatus();

        /// <summary>
        /// Main calling method. Opens a new instance of the form, and displays it
        /// </summary>
        /// <param name="WindowTitle"></param>
        public static void ShowScreen(Form Parent)
        {
            // Make sure it is currently not open and running.
            if (Instance != null && !Instance.IsDisposed)
                return;

            Instance = new LoadingForm();
            Instance.Location = new Point(Parent.Location.X + (Parent.Width / 2) - 150, Parent.Location.Y + (Parent.Height / 2) - 28);
            Thread thread = new Thread(new ThreadStart(LoadingForm.ShowForm));
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            Thread.Sleep(100); // Wait for Run to work
        }

        /// <summary>
        /// Method called to close the update form
        /// </summary>
        public static void CloseForm()
        {
            if (Instance != null && !Instance.IsDisposed)
                Instance.Invoke(new CloseDelegate(LoadingForm.CloseFormInternal));
        }

        /// <summary>
        /// Threaded method. Runs the form application
        /// </summary>
        private static void ShowForm()
        {
            Application.Run(Instance);
        }

        /// <summary>
        /// Method called from delegate, to close the form
        /// </summary>
        private static void CloseFormInternal()
        {
            Instance.Close();
        }

        public LoadingForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Prevents the form from being dragable
        /// </summary>
        /// <param name="message"></param>
        protected override void WndProc(ref Message message)
        {
            switch (message.Msg)
            {
                case WM_SYSCOMMAND:
                    int command = message.WParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                        return;
                    break;
            }

            base.WndProc(ref message);
        }
    }
}
