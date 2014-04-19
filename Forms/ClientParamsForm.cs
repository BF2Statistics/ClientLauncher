using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace BF2statisticsLauncher
{
    public partial class ClientParamsForm : Form
    {
        /// <summary>
        /// The parameter string that was set with the "Save" button is pushed
        /// </summary>
        public static string ParamString = String.Empty;

        /// <summary>
        /// A string containing the unknown (parsable) parameters
        /// </summary>
        protected string UnknownVals = String.Empty;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Params">The current param string</param>
        public ClientParamsForm(string Params)
        {
            InitializeComponent();
            ParamString = Params;

            // Set resolution to defaults
            HeightText.Text = Screen.PrimaryScreen.Bounds.Height.ToString();
            WidthText.Text = Screen.PrimaryScreen.Bounds.Width.ToString();

            // Parse params screen
            ParseParamString();

            // Set tooltips
            SetToolTips();
        }

        /// <summary>
        /// Uses the Tipsy class to set the tooltips to most of the options
        /// </summary>
        private void SetToolTips()
        {
            Tipsy.SetToolTip(WindowedMode, "If checked, Battlfield 2 will be started in windowed mode");
            Tipsy.SetToolTip(CustomRes, "If checked, Battlefield 2 will be forced to use the custom resolution below");
            Tipsy.SetToolTip(AutoLogin, "If checked, the account name below will automatically login");
            Tipsy.SetToolTip(AccountName, "Account name is Case-Sensitive!");
            Tipsy.SetToolTip(AccountPass, "Password is Case-Sensitive!");
            Tipsy.SetToolTip(JoinServerIp, "To auto join a server, make sure to enable Auto Login!");
            Tipsy.SetToolTip(PlayNow, "If checked, BF2 will automatically uses the 'Play Now' functionality");
            Tipsy.SetToolTip(Restart, "Used by BF2 to restart the game without showing video for example when mod switching."
                + Environment.NewLine + "Can also be used to start BF 2 without showing videos");
            Tipsy.SetToolTip(DisableSwiff, "Disables the swiff player. Basically this disables the flash that is used at the main menu area.");
            Tipsy.SetToolTip(NoSound, "If checked, Battlefield 2 will start with sound disabled.");
            Tipsy.SetToolTip(LowPriority, "If checked,  Battlefield 2 will start with a lower process priority(less CPU intensive, lower performance)");
        }

        /// <summary>
        /// This method takes a complete query param string for BF2 and parses it
        /// </summary>
        private void ParseParamString()
        {
            Regex Reg = new Regex(@"\+(?<name>[a-z]+)[\s]+(?<value>[a-z0-9.<>=_-]+)", RegexOptions.IgnoreCase);
            MatchCollection Matches = Reg.Matches(ParamString);
            foreach (Match M in Matches)
            {
                string Key = M.Groups["name"].Value.ToLower();
                string Value = M.Groups["value"].Value;
                switch (Key)
                {
                    case "joinserver":
                        JoinServerIp.Text = Value;
                        break;
                    case "port":
                        JoinServerPort.Value = int.Parse(Value);
                        break;
                    case "password":
                        JoinServerPass.Text = Value;
                        break;
                    case "fullscreen":
                        WindowedMode.Checked = (Value == "0");
                        break;
                    case "szx":
                        HeightText.Text = Value;
                        CustomRes.Checked = true;
                        break;
                    case "szy":
                        WidthText.Text = Value;
                        CustomRes.Checked = true;
                        break;
                    case "playername":
                        AccountName.Text = Value;
                        AutoLogin.Checked = true;
                        break;
                    case "playerpassword":
                        AccountPass.Text = Value;
                        AutoLogin.Checked = true;
                        break;
                    case "playnow":
                        PlayNow.Checked = (Value == "1");
                        break;
                    case "restart":
                        Restart.Checked = (Value == "1");
                        break;
                    case "disableswiff":
                        DisableSwiff.Checked = (Value == "1");
                        break;
                    case "nosound":
                        NoSound.Checked = (Value == "1");
                        break;
                    case "lowpriority":
                        LowPriority.Checked = (Value == "1");
                        break;
                    default:
                        UnknownVals = String.Concat(UnknownVals, "+", M.Groups["name"].Value, " " ,Value, " ");
                        break;
                }
            }
        }

        /// <summary>
        /// Event fire when the Cancel button is pushed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Event fired when the Save button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveBtn_Click(object sender, EventArgs e)
        {
            StringBuilder Params = new StringBuilder();

            // Windowed Mode
            if (WindowedMode.Checked)
                Params.Append("+fullscreen 0 ");

            // Custom Resolution
            if (CustomRes.Checked)
                Params.AppendFormat("+szy {0} +szx {1} ", WidthText.Text, HeightText.Text);

            // Join Server
            if (!String.IsNullOrWhiteSpace(JoinServerIp.Text))
            {
                Params.AppendFormat("+joinServer {0} +port {1} ", JoinServerIp.Text, JoinServerPort.Value);
                if (!String.IsNullOrWhiteSpace(JoinServerPass.Text))
                    Params.AppendFormat("+password {0} ", JoinServerPass.Text);
            }

            // Auto Login
            if (AutoLogin.Checked)
            {
                // Account name
                if (!String.IsNullOrWhiteSpace(AccountName.Text))
                    Params.AppendFormat("+playerName {0} ", AccountName.Text);

                // Account Pass
                if (!String.IsNullOrWhiteSpace(AccountPass.Text))
                    Params.AppendFormat("+playerPassword {0} ", AccountPass.Text);
            }

            // Misc Params
            if (PlayNow.Checked)
                Params.Append("+playNow 1 ");

            if (Restart.Checked)
                Params.Append("+restart 1 ");

            if (DisableSwiff.Checked)
                Params.Append("+disableSwiff 1 ");

            if (NoSound.Checked)
                Params.Append("+noSound 1 ");

            if (LowPriority.Checked)
                Params.Append("+lowPriority 1 ");

            // Set the param string
            ParamString = String.Concat(Params.ToString(), UnknownVals).Trim();
            this.DialogResult = DialogResult.OK;
        }

        private void CustomRes_CheckedChanged(object sender, EventArgs e)
        {
            HeightText.Enabled = CustomRes.Checked;
            WidthText.Enabled = CustomRes.Checked;
        }

        private void AutoLogin_CheckedChanged(object sender, EventArgs e)
        {
            AccountName.Enabled = AutoLogin.Checked;
            AccountPass.Enabled = AutoLogin.Checked;
        }
    }
}
