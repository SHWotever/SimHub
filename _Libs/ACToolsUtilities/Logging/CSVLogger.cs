using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACToolsUtilities.Logging
{
    public class CSVLogger
    {
        public static string LogHeader(Type t)
        {
            StringBuilder sb = new StringBuilder();
            var members = t.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            foreach (var member in members)
            {
                sb.Append(member.Name);
                sb.Append(";");
            }
            sb.AppendLine();
            return sb.ToString();
        }

        public static string LogObject(object o)
        {
            StringBuilder sb = new StringBuilder();
            var members = o.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            foreach (var member in members)
            {
                if (!member.FieldType.IsArray)
                {
                    sb.Append(member.GetValue(o));
                }
                else
                {
                    Array value = member.GetValue(o) as Array;
                    foreach (var item in value)
                    {
                        sb.Append(item);
                        sb.Append("|");
                    }
                }

                sb.Append(";");
            }

            sb.AppendLine();
            return sb.ToString();
        }
    }
}
