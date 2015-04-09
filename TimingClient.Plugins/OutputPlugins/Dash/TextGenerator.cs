using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimingClient.Plugins.OutputPlugins.Dash
{
  
    public class TextGenerator
    {




        public static string GetText(global::TimingClient.Plugins.PluginManager manager, List<ScreenPart> parts)
        {
            string result = "";

            foreach (var part in parts)
            {
                string text = string.Empty;
                if (part.Text != null)
                {
                    text = part.Text;
                }
                else if (part.Expression != null)
                {
                    var value = manager.GetPropertyValue(part.Expression);
                    if (!string.IsNullOrEmpty(part.FormatString))
                    {
                        try
                        {
                            var format = part.FormatString;
                            bool addsign = false;
                            if (value is TimeSpan && format.StartsWith("+"))
                            {
                                addsign = true;
                                format = format.Substring(1);
                            }
                            text = string.Format("{0:" + format + "}", value);
                            if (addsign)
                            {
                                text = (((TimeSpan)value).TotalMilliseconds >= 0 ? "+" : "-") + text;
                            }
                        }
                        catch
                        {
                            text = "{Format Error}";
                        }
                    }
                    else
                    {
                        text = string.Format("{0}", manager.GetPropertyValue(part.Expression));
                    }
                }

                if (part.FixedLength > 0)
                {
                    text = global::SerialDash.SerialDashController.Format(part.FixedLength > 0 ? part.FixedLength : text.Length, text, part.RightAlign);
                }
                else
                {
                    text = SerialDash.SerialDashController.ReplaceChars(text);
                }
                result += text;
            }

            return result;
        }
    }
}