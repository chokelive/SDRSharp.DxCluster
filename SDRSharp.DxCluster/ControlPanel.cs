using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SDRSharp.Common;
using SDRSharp.DxCluster;
using SDRSharp.PanView;
using SDRSharp.Radio;

namespace SDRSharp.DxCluster
{
    public partial class ControlPanel : UserControl
    {
        private ISharpControl _control;
        private DxClusterProcess _process;
        private DxClusterPlugin _plugin;

        public ControlPanel(ISharpControl control, DxClusterProcess process, DxClusterPlugin plugin)
        {
            _control = control;
            _process = process;
            _plugin = plugin;
            InitializeComponent();

            Timer updateControl = new Timer();
            updateControl.Interval = 1000;
            updateControl.Tick += UpdateControl_Tick;
            updateControl.Start();


        }

        private void UpdateControl_Tick(object sender, EventArgs e)
        {
            if (_process.clusterConnected == true)
            {
                lbl_cluter_status.Text = "Cluster Status : Connected";
                lbl_cluter_status.ForeColor = Color.GreenYellow;
            }
            else
            {
                lbl_cluter_status.Text = "Cluster Status : Disconnect";
                lbl_cluter_status.ForeColor = Color.OrangeRed;
            }
        }

        private void ControlPanel_Load(object sender, EventArgs e)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            lbl_version.Text = "v" + fvi.FileMajorPart + "." + fvi.FileMinorPart + " Dev by E29AHU";

            // Load config
            txt_callsign.Text = _plugin.config.AppSettings.Settings["mycallsign"].Value;
        }

        private void chk_enable_CheckedChanged(object sender, EventArgs e)
        {

            // Save config
            _plugin.config.AppSettings.Settings["mycallsign"].Value = txt_callsign.Text;
            _plugin.config.Save(ConfigurationSaveMode.Modified);

            if (chk_enable.Checked)
            {
                if (txt_callsign.Text != "")
                {
                    _process.connectCluster(txt_callsign.Text);
                    
                }
                else
                {
                    MessageBox.Show("Please enter your callsign.");
                }
            }
            else
            {
                _process.disconnectCluster();
            }
            
        }
    }
}
