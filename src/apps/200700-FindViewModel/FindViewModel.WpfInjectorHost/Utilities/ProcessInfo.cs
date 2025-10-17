namespace FindViewModel.WpfInjectorHost.Utilities
{
    using FindViewModel.Core;
    using System.Diagnostics;


    public class ProcessInfo
    {
        private bool? isOwningProcessElevated;

        public ProcessInfo(int processId)
            : this(Process.GetProcessById(processId))
        {
        }

        public ProcessInfo(Process process)
        {
            this.Process = process;
        }

        public Process Process { get; }

        public bool IsProcessElevated => this.isOwningProcessElevated ??= NativeMethods.IsProcessElevated(this.Process);

    }
}
