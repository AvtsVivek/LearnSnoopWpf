namespace Snoop.WpfClient
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProcessManager processManager = new();
        private SnoopClient snoopClient = new();

        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void StartSnoopButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the process ID from clipboard
            if (int.TryParse(Clipboard.GetText(), out var processId))
            {
                // Check if we got a valid process ID
                if (!this.processManager.DoesProcessExist(processId))
                {
                    MessageBox.Show(this, $"The process with the specified ID {processId} does not exist.", "Could not start snoop", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var windowHandle = this.processManager.GetWindow(processId);

                if (windowHandle == IntPtr.Zero)
                {
                    MessageBox.Show(this, $"Could not find a window for the specified process ID: {processId}",
                        "Could not start snoop", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var windowInfo = this.processManager.GetWindowInfo(windowHandle);

                var result = this.snoopClient.StartSnoopProcess(processId, windowHandle);

                if (result?.Success == false)
                {
                    MessageBox.Show(this, $"Could not start snoop for the specified process ID {processId}." +
                        Environment.NewLine +
                        $"{result.AttachException}", "Could not start snoop", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show(this, "Could not get a valid process ID from the clipboard.", "Could not start snoop", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}