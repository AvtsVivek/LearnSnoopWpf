namespace Snoop.Windows
{
    /// <summary>
    /// Interaction logic for SimpleWindow.xaml
    /// </summary>
    public partial class SimpleWindow
    {
        public SimpleWindow()
        {
            InitializeComponent();
        }

        private object target;

        public override object Target
        {
            get {
                return target; 
            }
            set {
                target = value;
            }
        }

        protected override void Load(object rootToInspect)
        {

        }
    }
}
