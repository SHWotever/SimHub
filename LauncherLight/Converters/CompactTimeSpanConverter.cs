using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace LauncherLight.Converters
{
    public class CompactTimeSpanConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public bool Hide { get; set; }

        public bool Invert { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;

            if (value is TimeSpan)
            {
                var time = (TimeSpan)value;
                string result = string.Empty;
                if (time.TotalHours >= 1)
                {
                    return ((int)time.TotalHours).ToString() + time.ToString(@"\:mm\:ss");
                }
                else
                {
                    return time.ToString(@"mm\:ss");
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}