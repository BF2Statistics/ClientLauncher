using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BF2statisticsLauncher
{
    /// <summary>
    /// Tipsy is a simple class that can Add and Remove tooltips
    /// to form controls programatically during runtime.
    /// </summary>
    class Tipsy
    {
        /// <summary>
        /// A list of tooltips for controls
        /// </summary>
        private static readonly Dictionary<string, ToolTip> ToolTips = new Dictionary<string, ToolTip>();

        /// <summary>
        /// Returns the controls tooptip object. If the control does not have
        /// a tooltip, a new instance of a tooltip is returned instead
        /// </summary>
        /// <param name="controlName"></param>
        /// <returns></returns>
        public static ToolTip GetControlToolTip(string controlName)
        {
            if (ToolTips.ContainsKey(controlName))
            {
                return ToolTips[controlName];
            }
            else
            {
                ToolTip Tip = new ToolTip();
                ToolTips.Add(controlName, Tip);
                return Tip;
            }
        }

        /// <summary>
        /// Returns the controls tooptip object. If the control does not have
        /// a tooltip, a new instance of a tooltip is returned instead
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static ToolTip GetControlToolTip(Control control)
        {
            return GetControlToolTip(control.Name);
        }

        /// <summary>
        /// Sets a tooltip object for a control
        /// </summary>
        /// <param name="control"></param>
        /// <param name="text"></param>
        public static void SetToolTip(Control control, string text)
        {
            SetToolTip(control, text, false);
        }

        /// <summary>
        /// Sets a tooltip object for a control
        /// </summary>
        /// <param name="control"></param>
        /// <param name="text"></param>
        /// <param name="ShowAlways"></param>
        public static void SetToolTip(Control control, string text, bool ShowAlways)
        {
            // Prevent cross thread errors
            if (control.InvokeRequired)
            {
                control.Invoke((Action)delegate
                {
                    ToolTip tt = Tipsy.GetControlToolTip(control);
                    tt.ShowAlways = ShowAlways;
                    tt.SetToolTip(control, text);
                });
            }
            else
            {
                ToolTip tt = GetControlToolTip(control);
                tt.SetToolTip(control, text);
            }
        }

        /// <summary>
        /// Removes any and all tooltips for a control
        /// </summary>
        /// <param name="control"></param>
        public static void RemoveToolTip(Control control)
        {
            ToolTip T = ToolTips[control.Name];
            T.Dispose();
            ToolTips.Remove(control.Name);
        }
    }
}
