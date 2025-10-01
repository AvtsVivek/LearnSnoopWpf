namespace Snoop.WpfClient.Utilities
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;

    public class SystemResourcesCacheNew : ICacheManagedNew
    {
        public static readonly SystemResourcesCacheNew Instance = new();

        private SystemResourcesCacheNew()
        {
        }

        public ObservableCollection<SystemResourcesCacheEntry> SystemResources { get; } = new();

        public void Reset()
        {
            this.SystemResources.Clear();
        }

        public SystemResourcesCacheNew Reload()
        {
            this.Reset();

            var type = typeof(ResourceDictionary).Assembly.GetType("System.Windows.SystemResources");

            if (type is null)
            {
                return this;
            }

            var dictionariesField = type.GetField("_dictionaries", BindingFlags.Static | BindingFlags.NonPublic)
                                    ?? type.GetField("dictionaries", BindingFlags.Static | BindingFlags.NonPublic);

            var dictionaries = (IDictionary?)dictionariesField?.GetValue(null);

            if (dictionaries is null)
            {
                return this;
            }

#pragma warning disable CS8605
            foreach (DictionaryEntry dictionariesEntry in dictionaries)
#pragma warning restore CS8605
            {
                var assembly = (Assembly)dictionariesEntry.Key;
                var resourceDictionaries = dictionariesEntry.Value;

                if (resourceDictionaries is null)
                {
                    continue;
                }

                var genericDictionaryField = resourceDictionaries.GetType().GetField("_genericDictionary", BindingFlags.Instance | BindingFlags.NonPublic)
                                             ?? resourceDictionaries.GetType().GetField("genericDictionary", BindingFlags.Instance | BindingFlags.NonPublic);

                var genericDictionary = (ResourceDictionary?)genericDictionaryField?.GetValue(resourceDictionaries);

                var themedDictionaryField = resourceDictionaries.GetType().GetField("_themedDictionary", BindingFlags.Instance | BindingFlags.NonPublic)
                                            ?? resourceDictionaries.GetType().GetField("themedDictionary", BindingFlags.Instance | BindingFlags.NonPublic);

                var themedDictionary = (ResourceDictionary?)themedDictionaryField?.GetValue(resourceDictionaries);

                var cacheEntry = new SystemResourcesCacheEntry(assembly, genericDictionary, themedDictionary);
                this.SystemResources.Add(cacheEntry);
            }

            return this;
        }

        public void Dispose()
        {
            this.Reset();
        }

        public void Activate()
        {
            this.Reload();
        }
    }

    public class SystemResourcesCacheEntry
    {
        public SystemResourcesCacheEntry(Assembly assembly, ResourceDictionary? genericDictionary, ResourceDictionary? themedDictionary)
        {
            this.Assembly = assembly;
            this.Generic = genericDictionary;
            this.Themed = themedDictionary;
        }

        public Assembly Assembly { get; }

        public ResourceDictionary? Generic { get; }

        public ResourceDictionary? Themed { get; }
    }
}
