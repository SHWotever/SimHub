using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimingClient.Plugins.DataPlugins.DataCore
{

    public class Expression
    {
        public Expression()
        {
            this.Assemblies = new List<string>();
        }
        public List<string> Assemblies { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
    }
    public class DataCorePluginSettings
    {
        public DataCorePluginSettings()
        {
            this.Expressions = new List<Expression>();
        }
        public List<Expression> Expressions { get; set; }

    }
}
