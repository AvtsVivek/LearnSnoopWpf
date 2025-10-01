namespace BasicProcInjector.WpfInjectorHost.Utilities
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    [ValueConversion(typeof(object), typeof(object))]
    [ValueConversion(typeof(object), typeof(Style))]
    public class NullStyleConverterNew : IValueConverter
    {
        public static readonly NullStyleConverterNew DefaultInstance = new();

        public object? Convert(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            if (value is not null)
            {
                return value;
            }

            return parameter switch
            {
                FrameworkElement fe => FrameworkElementHelperNew.GetStyle(fe),
                FrameworkContentElement fce => FrameworkElementHelperNew.GetStyle(fce),
                _ => null
            };
        }

        public object ConvertBack(object? value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            return Binding.DoNothing;
        }
    }
}
