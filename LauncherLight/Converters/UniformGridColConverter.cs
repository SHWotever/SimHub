using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace LauncherLight.Converters
{
    public class UniformGridColConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;

            var val = System.Convert.ToDouble(value);
            return Math.Max((int)((val - 40) / MinItemWidth), MinColumns);

            return value;
        }

        public int MinItemWidth { get; set; }

        public int MinColumns { get; set; }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}