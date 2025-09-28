using System.Diagnostics;
using System.Windows;

namespace SimpleWpfOne
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Process _currentProcess = Process.GetCurrentProcess();
        public MainWindow()
        {
            InitializeComponent();
            resultTextBox.Text = _currentProcess.Id.ToString();
            CopyProcessIdToClipboard();
        }

        private void plusButton_Click(object sender, RoutedEventArgs e)
        {
            var number = int.Parse(firstTextBox.Text);
            number++;
            firstTextBox.Text = number.ToString();
        }
        private void copyProcessIdButton_Click(object sender, RoutedEventArgs e)
        {
            CopyProcessIdToClipboard();
        }
        private void CopyProcessIdToClipboard()
        {
            Clipboard.SetText(_currentProcess.Id.ToString());
        }
    }
}