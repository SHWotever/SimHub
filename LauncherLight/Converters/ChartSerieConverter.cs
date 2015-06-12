using ACSharedMemory.Models.Car;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace LauncherLight.Converters
{
    [ImplementPropertyChanged]
    public class SerieItem
    {
        public string X { get; set; }

        public float Y { get; set; }
    }

    public class ChartSerieConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;

            if (value is List<List<string>>)
            {
                ObservableCollection<SerieItem> result = new ObservableCollection<SerieItem>();
                var lstValue = value as List<List<string>>;
                foreach (var item in lstValue)
                {
                    result.Add(new SerieItem { X = item[0], Y = (float)System.Convert.ToDouble(item[1]) });
                }
                return result;
            }
            if (value is CarInfo)
            {
                ObservableCollection<SerieItem> result = new ObservableCollection<SerieItem>();
                var lstValue = (value as CarInfo).torqueCurve;// as List<List<string>>;
                foreach (var item in lstValue)
                {
                    result.Add(new SerieItem { X = item[0], Y = (float)System.Convert.ToDouble(item[1]) });
                }
                return result;
            }
            return value;
        }

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