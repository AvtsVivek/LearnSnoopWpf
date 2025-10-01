namespace Snoop.WpfClient
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Snoop.WpfClient.Utilities;

    internal class ProcessManager
    {
        private readonly Dictionary<int, IList<IntPtr>> processesAndWindows;

        public ProcessManager()
        {
            this.processesAndWindows = NativeMethodsNew.GetProcessesAndWindows();
        }

        public IntPtr GetWindow(int processId)
        {
            if (this.processesAndWindows.TryGetValue(processId, out var windows) && windows.Any())
            {
                return windows.First();
            }

            return IntPtr.Zero;
        }

        public WindowInfoNew GetWindowInfo(IntPtr intPtr)
        {
            return WindowInfoNew.GetWindowInfo(intPtr);
        }

        public bool DoesProcessExist(int processId)
        {
            try
            {
                // Attempt to get the process by its ID.
                // This will throw an ArgumentException if no process with the given ID exists.
                Process process = Process.GetProcessById(processId);

                // If no exception is thrown, the process exists.
                // You can also check process.HasExited to ensure it's still running.
                return !process.HasExited;
            }
            catch (ArgumentException)
            {
                // The process with the specified ID does not exist.
                return false;
            }
            catch (InvalidOperationException)
            {
                // The process has exited.
                return false;
            }
            catch (Exception ex)
            {
                // Handle other potential exceptions, e.g., security exceptions.
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
    }
}