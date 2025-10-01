namespace BasicProcInjector.WpfInjectorHost.Utilities
{
    using BasicProcInjector.Core;
    using System;
    using System.Windows;
    using System.Windows.Forms.Integration;

    public abstract class SnoopMainBaseWindowNew : SnoopBaseWindowNew
    {
        private Window? ownerWindow;

        public object? RootObject { get; private set; }

        public abstract object? Target { get; set; }

        public Window Inspect(object rootObject)
        {
            ExceptionHandlerNew.AddExceptionHandler(this.Dispatcher);

            this.RootObject = rootObject;

            this.Load(rootObject);

            this.ownerWindow = SnoopWindowUtilsNew.FindOwnerWindow(this);

            if (TransientSettingsDataNew.Current?.SetOwnerWindow == true)
            {
                this.Owner = this.ownerWindow;
            }
            else if (this.ownerWindow is not null)
            {
                // if we have an owner window, but the owner should not be set, we still have to close ourself if the potential owner window got closed
                this.ownerWindow.Closed += this.OnOwnerWindowOnClosed;
            }

            LogHelperNew.WriteLine("Showing snoop UI...");

            if (System.Windows.Forms.Application.OpenForms.Count > 0)
            {
                // this is windows forms -> wpf interop

                // call ElementHost.EnableModelessKeyboardInterop to allow the Snoop UI window
                // to receive keyboard messages. if you don't call this method,
                // you will be unable to edit properties in the property grid for windows forms interop.
                ElementHost.EnableModelessKeyboardInterop(this);
            }

            this.ShowActivated = TransientSettingsDataNew.Current?.ShowActivated is not false;
            this.Show();

            LogHelperNew.WriteLine("Shown snoop UI.");

            return this;
        }

        private void OnOwnerWindowOnClosed(object? o, EventArgs eventArgs)
        {
            if (this.ownerWindow is not null)
            {
                this.ownerWindow.Closed -= this.OnOwnerWindowOnClosed;
            }

            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            ExceptionHandlerNew.RemoveExceptionHandler(this.Dispatcher);

            base.OnClosed(e);
        }

        protected abstract void Load(object rootToInspect);
    }
}
