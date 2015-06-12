using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Markup;

namespace LauncherLight.Converters
{
    public class HtmlToRichConverter : MarkupExtension, IValueConverter
    {
        private MarkupConverter.MarkupConverter markupConverter = new MarkupConverter.MarkupConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return DependencyProperty.UnsetValue;

            if (!string.IsNullOrEmpty(value.ToString()))
            {
                var content = this.markupConverter.ConvertHtmlToXaml(value.ToString());
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
                return (FlowDocument)XamlReader.Load(stream);
            }

            return DependencyProperty.UnsetValue;
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