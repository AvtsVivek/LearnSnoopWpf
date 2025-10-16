using System.Text;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;


namespace WpfWindowTreeViewAnalysisOne.WpfUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProcessManager processManager = new();

        public MainWindow()
        {
            InitializeComponent();

            // StartProcessInjection();
        }
        private void StartProcessInjection()
        {
            var processId = GetProcessId();

            if (processId == -1)
            {
                MessageBox.Show(this,
                    "Could not get a valid process ID from the clipboard.", "Could not start snoop",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Get the process ID from clipboard

            if (!this.processManager.DoesProcessExist(processId))
            {
                MessageBox.Show(this, $"The process with the specified ID {processId} does not exist.", "Could not start snoop", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            IntPtr windowHandle = this.processManager.GetWindow(processId);

            var hwndSource = GetRootVisualFromHwnd(windowHandle);

            if (hwndSource == null)
            {
                MessageBox.Show(this, $"No HwndSource found for the given handle {windowHandle}, " +
                    $"indicating it might not be a WPF window or is not yet initialized.", 
                    "Could not start snoop", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var rootVisual = hwndSource?.RootVisual;

            object rootObject = rootVisual!;

            //if (Application.Current is not null
            //    && Application.Current.Dispatcher == presentationSourceDispatcher)
            //{
            //    rootObject = Application.Current;
            //}

            //if (rootObject is null)
            //{
            //    continue;
            //}

            var dispatcher = (rootObject as DispatcherObject)?.Dispatcher;
            var dispatcherRootObjectPair = new DispatcherRootObjectPair(dispatcher!, rootObject);

            // Check if we have already seen this pair
            //if (dispatcherRootObjectPairs.IndexOf(dispatcherRootObjectPair) == -1)
            //{
            //    dispatcherRootObjectPairs.Add(dispatcherRootObjectPair);
            //}            
        }

        public HwndSource GetRootVisualFromHwnd(IntPtr hWnd)
        {
            // Get the HwndSource associated with the window handle
            HwndSource hwndSource = HwndSource.FromHwnd(hWnd);

            if (hwndSource != null)
            {
                // The RootVisual property of HwndSource provides the root visual element of the WPF content
                return hwndSource;
            }
            else
            {
                // No HwndSource found for the given handle,
                // indicating it might not be a WPF window or is not yet initialized.
                return null!;
            }
        }

        private List<DispatcherRootObjectPair> GetDispatcherRootObjectPairList()
        {
            var dispatcherRootObjectPairs = new List<DispatcherRootObjectPair>();

            foreach (PresentationSource? presentationSource in PresentationSource.CurrentSources)
            {
                if (presentationSource is null)
                {
                    continue;
                }

                var presentationSourceDispatcher = presentationSource.Dispatcher;
                var rootVisual = presentationSource.RunInDispatcher(() => presentationSource.RootVisual);

                object rootObject = rootVisual;

                if (Application.Current is not null
                    && Application.Current.Dispatcher == presentationSourceDispatcher)
                {
                    rootObject = Application.Current;
                }

                if (rootObject is null)
                {
                    continue;
                }

                var dispatcher = (rootObject as DispatcherObject)?.Dispatcher ?? presentationSourceDispatcher;
                var dispatcherRootObjectPair = new DispatcherRootObjectPair(dispatcher, rootObject);

                // Check if we have already seen this pair
                if (dispatcherRootObjectPairs.IndexOf(dispatcherRootObjectPair) == -1)
                {
                    dispatcherRootObjectPairs.Add(dispatcherRootObjectPair);
                }
            }
            return dispatcherRootObjectPairs;
        }

        private int GetProcessId()
        {
            if (int.TryParse(Clipboard.GetText().Split('-')[0].Replace("ProcessId", string.Empty), out var processId))
            {
                return processId;
            }

            return -1;
        }

        private void StartProcessInjectionButton_Click(object sender, RoutedEventArgs e)
        {
            StartProcessInjection();
        }
    }
}