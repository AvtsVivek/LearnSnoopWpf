namespace BasicProcInjector.WpfInjectorHost.Utilities
{
    using System;
    using System.Collections.Generic;

    public class CacheManagerNew
    {
        public static readonly CacheManagerNew Instance = new();

        private CacheManagerNew()
        {
            this.Participants.Add(BindingDiagnosticHelperNew.Instance);
            this.Participants.Add(SystemResourcesCacheNew.Instance);
            this.Participants.Add(ResourceKeyCacheNew.Instance);
        }

        public int UsageCount { get; private set; }

        public List<ICacheManagedNew> Participants { get; } = new();

        public void IncreaseUsageCount()
        {
            if (this.UsageCount == 0)
            {
                foreach (var participant in this.Participants)
                {
                    participant.Activate();
                }
            }

            ++this.UsageCount;
        }

        public void DecreaseUsageCount()
        {
            --this.UsageCount;

            if (this.UsageCount == 0)
            {
                foreach (var participant in this.Participants)
                {
                    participant.Dispose();
                }
            }
        }
    }

    public interface ICacheManagedNew : IDisposable
    {
        void Activate();
    }
}
