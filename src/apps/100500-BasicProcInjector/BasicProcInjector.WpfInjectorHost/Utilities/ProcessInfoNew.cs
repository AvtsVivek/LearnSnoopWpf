namespace BasicProcInjector.WpfInjectorHost.Utilities
{
    using BasicProcInjector.Core;
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Input;


    public class ProcessInfoNew
    {
        private bool? isOwningProcessElevated;

        public ProcessInfoNew(int processId)
            : this(Process.GetProcessById(processId))
        {
        }

        public ProcessInfoNew(Process process)
        {
            this.Process = process;
        }

        public Process Process { get; }

        public bool IsProcessElevated => this.isOwningProcessElevated ??= NativeMethodsNew.IsProcessElevated(this.Process);

    }
}
