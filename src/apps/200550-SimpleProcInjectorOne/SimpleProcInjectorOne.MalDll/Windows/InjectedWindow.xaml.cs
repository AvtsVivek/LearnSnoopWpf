using System.Diagnostics;

namespace SimpleProcInjectorOne.MalDll.Windows
{
    /// <summary>
    /// Interaction logic for InjectedWindow.xaml
    /// </summary>
    public partial class InjectedWindow
    {
        private readonly Process _currentProcess;
        public InjectedWindow()
        {
            target = new();
            InitializeComponent();
            _currentProcess = Process.GetCurrentProcess();
            resultTextBox.Text = _currentProcess.Id.ToString();
        }

        private object target;

        public override object Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }

        protected override void Load(object rootToInspect)
        {

        }
    }
}
