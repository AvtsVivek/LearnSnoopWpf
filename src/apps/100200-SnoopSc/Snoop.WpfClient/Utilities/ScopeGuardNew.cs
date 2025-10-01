namespace Snoop.WpfClient.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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
