namespace BasicProcInjector.WpfInjectorHost.Utilities
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public static class ObservableCollectionExtensionsNew
    {
        public static void UpdateWith<T>(this ObservableCollection<T> target, IReadOnlyCollection<T> source)
        {
            if (target.Count > 0)
            {
                target.Clear();
            }

            if (source is null
                || source.Count <= 0)
            {
                return;
            }

            foreach (var item in source)
            {
                target.Add(item);
            }
        }
    }
}
