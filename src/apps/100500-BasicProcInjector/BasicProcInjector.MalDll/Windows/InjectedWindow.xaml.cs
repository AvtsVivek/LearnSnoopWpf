namespace BasicProcInjector.MalDll.Windows
{
    /// <summary>
    /// Interaction logic for InjectedWindow.xaml
    /// </summary>
    public partial class InjectedWindow
    {
        public InjectedWindow()
        {
            target = new();
            InitializeComponent();
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
