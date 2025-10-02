namespace BasicProcInjector.WpfInjectorHost
{
    using BasicProcInjector.WpfInjectorHost.Utilities;
    using BasicProcInjector.InjectorLauncher;
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

            if(string.IsNullOrWhiteSpace(AppSettings.Default.FirstOrSecond))
            {
                AppSettings.Default.FirstOrSecond = "First";
                AppSettings.Default.Save();
            }

            if (AppSettings.Default.FirstOrSecond == "Second")
            {
                this.SecondRadioButton.IsChecked = true;
            }
            else
            {
                this.FirstRadioButton.IsChecked = true;
            }
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

                WindowInfo windowInfo = this.processManager.GetWindowInfo(windowHandle);

                var processWrapper = ProcessWrapper.From(processId, windowHandle);

                this.snoopClient.StartSnoopProcessNew(processId, windowHandle);
            }
            else
            {
                MessageBox.Show(this, "Could not get a valid process ID from the clipboard.", "Could not start snoop", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FirstSecondRadioButton_Click(object sender, RoutedEventArgs e)
        {
            if(sender == this.FirstRadioButton)
            {
                this.FirstRadioButton.IsChecked = true;
            }
            else
            {
                this.SecondRadioButton.IsChecked = true;
            }

            AppSettings.Default.FirstOrSecond = sender == this.FirstRadioButton ? "First" : "Second";
            AppSettings.Default.Save();
        }
    }
}