namespace Snoop.WpfClient.Utilities
{
    using System.Windows;

    public static class ResourceKeyHelperNew
    {
        public static bool IsValidResourceKey(object? key)
        {
            return key is not null
                   && ReferenceEquals(key, DependencyProperty.UnsetValue) == false;
        }
    }
}
