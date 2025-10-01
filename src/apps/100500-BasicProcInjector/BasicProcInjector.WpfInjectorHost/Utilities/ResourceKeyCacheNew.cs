namespace BasicProcInjector.WpfInjectorHost.Utilities
{
    using System.Collections.Generic;
    using System.Windows;

    public class ResourceKeyCacheNew : ICacheManagedNew
    {
        private readonly Dictionary<object, object> keys = new();

        public static readonly ResourceKeyCacheNew Instance = new();

        private ResourceKeyCacheNew()
        {
        }

        public object? GetOrAddKey(DependencyObject element, object value)
        {
            var resourceKey = this.GetKey(value);

            if (resourceKey is null)
            {
                resourceKey = ResourceDictionaryKeyHelpersNew.GetKeyOfResourceItem(element, value);
                this.Cache(value, resourceKey);
            }

            return resourceKey;
        }

        public object? GetKey(object value)
        {
            if (this.keys.TryGetValue(value, out var key))
            {
                return key;
            }

            return null;
        }

        public void Cache(object value, object key)
        {
            if (this.keys.ContainsKey(value) == false)
            {
                this.keys.Add(value, key);
            }
        }

        public bool Contains(object element)
        {
            return this.keys.ContainsKey(element);
        }

        public void Activate()
        {
        }

        public void Dispose()
        {
            this.keys.Clear();
        }
    }
}
