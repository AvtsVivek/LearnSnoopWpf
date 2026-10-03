using System.Diagnostics;
using System.Windows;
using MessageBox = System.Windows.Forms.MessageBox;

namespace PresentationSourceWinFormsOne.WinFormsUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var presentationSourceCount = GetPresentationSourceCount();

            var processId = Process.GetCurrentProcess().Id;

            textBox1.Multiline = true;

            textBox1.Text = $"Presentation Source Count: {presentationSourceCount}" 
                + Environment.NewLine
                + $"Process Id: {processId}";

            var utilities = new Utilities();

            utilities.GetProcessDetailsAndCopyToClipboard();
        }

        public int GetPresentationSourceCount()
        {
            var presentationSourceCount = 0;
            foreach (PresentationSource? presentationSource in PresentationSource.CurrentSources)
            {
                presentationSourceCount++;
            }
            return presentationSourceCount;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var presentationSourceCount = GetPresentationSourceCount();
            MessageBox.Show($"Presentation count is: {presentationSourceCount}");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
