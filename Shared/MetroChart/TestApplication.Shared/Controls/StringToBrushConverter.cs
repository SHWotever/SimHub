using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if NETFX_CORE
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI;
using Windows.UI.Xaml.Media;

#else

using System.Windows;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Media;

#endif

namespace TestApplication.Shared
{
    public class StringToBrushConverter : IValueConverter
    {
#if NETFX_CORE

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return InternalConvert(value, targetType, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

#else
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return InternalConvert(value, targetType, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

#endif

        public object InternalConvert(object value, Type targetType, object parameter)
        {
            if (value == null)
            {
                return null;
            }

            string colorName = value.ToString();
            SolidColorBrush scb = new SolidColorBrush();
            switch (colorName as string)
            {
                case "Magenta":
                    scb.Color = Colors.Magenta;
                    return scb;
                case "Purple":
                    scb.Color = Colors.Purple;
                    return scb;
                case "Brown":
                    scb.Color = Colors.Brown; 
                    return scb;
                case "Orange":
                    scb.Color = Colors.Orange;
                    return scb;
                case "Blue":
                    scb.Color = Colors.Blue;
                    return scb;
                case "Red":
                    scb.Color = Colors.Red;
                    return scb;
                case "Yellow":
                    scb.Color = Colors.Yellow;
                    return scb;
                case "Green":
                    scb.Color = Colors.Green;
                    return scb;
                default:
                    return null;
            }
        }
    }
}
