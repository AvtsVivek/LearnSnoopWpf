namespace BasicProcInjector.WpfInjectorHost.Utilities
{
    using System;

    public class ScopeGuardNew : IDisposable
    {
        public ScopeGuardNew(Action? enterAction = null, Action? exitAction = null)
        {
            this.EnterAction = enterAction;
            this.ExitAction = exitAction;
        }

        public Action? EnterAction { get; }

        public Action? ExitAction { get; }

        public ScopeGuardNew Guard()
        {
            this.EnterAction?.Invoke();
            return this;
        }

        public void Dispose()
        {
            this.ExitAction?.Invoke();
        }
    }
}
