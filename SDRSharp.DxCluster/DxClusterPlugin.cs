using System.Windows.Forms;
using SDRSharp.Common;
using SDRSharp.DxCluster;
using SDRSharp.Radio;
using SDRSharp.PanView;
using System.Configuration;


namespace SDRSharp.DxCluster
{
    public class DxClusterPlugin : ISharpPlugin, ICanLazyLoadGui, ISupportStatus, IExtendedNameProvider
    {
        private ControlPanel _gui;
        private ISharpControl _control;
        private DxClusterProcess _process;
        // App setting;
        public Configuration config = ConfigurationManager.OpenExeConfiguration(System.Reflection.Assembly.GetExecutingAssembly().Location);

        public string DisplayName => "Dx Cluster";
        
        public string Category => "E29AHU";
        
        public string MenuItemName => DisplayName;
        
        public bool IsActive => _gui != null && _gui.Visible;

        public UserControl Gui
        {
            get
            {
                LoadGui();
                return _gui;
            }
        }

        public void LoadGui()
        {
            if (_gui == null)
            {
                _gui = new ControlPanel(_control, _process, this);
            }
        }

        public void Initialize(ISharpControl control)
        {
            _control = control;
            _process = new DxClusterProcess(_control);

        }

        public void Close()
        {
            
        }
    }
}
