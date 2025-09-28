using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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