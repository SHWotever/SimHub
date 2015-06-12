namespace MarkupConverter
{
    public interface IMarkupConverter
    {
        string ConvertXamlToHtml(string xamlText);

        string ConvertHtmlToXaml(string htmlText);

        string ConvertRtfToHtml(string rtfText);

        string ConvertHtmlToRtf(string htmlText);
    }

    public class MarkupConverter : IMarkupConverter
    {
        public string ConvertXamlToHtml(string xamlText)
        {
            return HtmlFromXamlConverter.ConvertXamlToHtml(xamlText, false);
        }

        public string ConvertHtmlToXaml(string htmlText)
        {
            return HtmlToXamlConverter.ConvertHtmlToXaml(htmlText, true);
        }

        public string ConvertRtfToHtml(string rtfText)
        {
            return RtfToHtmlConverter.ConvertRtfToHtml(rtfText);
        }

        public string ConvertHtmlToRtf(string htmlText)
        {
            return HtmlToRtfConverter.ConvertHtmlToRtf(htmlText);
        }
    }
}