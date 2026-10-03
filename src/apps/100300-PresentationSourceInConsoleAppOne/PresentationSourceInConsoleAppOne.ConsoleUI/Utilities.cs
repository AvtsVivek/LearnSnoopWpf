
using System.Windows;

namespace PresentationSourceInConsoleAppOne.ConsoleUI
{
    public class Utilities
    {
        public void GetProcessDetailsAndCopyToClipboard()
        {
            var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
            var processDetails = $"ProcessId{currentProcess.Id}-ProcessName{currentProcess.ProcessName}";


            var thread = new Thread(() => Clipboard.SetText(processDetails));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
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
    }
}
