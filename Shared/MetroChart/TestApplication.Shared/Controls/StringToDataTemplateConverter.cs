using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if NETFX_CORE
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

#else

using System.Windows;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Controls;

#endif

namespace TestApplication.Shared
{
    public class StringToDataTemplateConverter : IValueConverter
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
            if(value == null)
            {
                return null;
            }

            var resources = Application.Current.Resources.MergedDictionaries.ToList();

            foreach (var dict in resources)
            {
                foreach (var objkey in dict.Keys)
                {
                    if (objkey.ToString() == value.ToString())
                    {
                        return dict[objkey] as DataTemplate;
                    }
                }
            }

            return null;
        }
    }
}
