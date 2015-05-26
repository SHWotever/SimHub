using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACHub.Plugins.DataPlugins.DataCore
{
    /// <summary>
    /// USer defined expression
    /// </summary>
    public class Expression
    {
        /// <summary>
        /// CTor
        /// </summary>
        public Expression()
        {
            this.Assemblies = new List<string>();
        }
        /// <summary>
        /// Referenced assemblies
        /// </summary>
        public List<string> Assemblies { get; set; }

        /// <summary>
        /// Main code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Expression name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Is core expression ?
        /// </summary>
        public bool IsCore { get; set; }
    }

    /// <summary>
    /// Plugon settings
    /// </summary>
    public class DataCorePluginSettings
    {
        /// <summary>
        /// CTOr
        /// </summary>
        public DataCorePluginSettings()
        {
            this.Expressions = new List<Expression>();
        }

        /// <summary>
        /// USer expressions
        /// </summary>
        [JsonIgnore]
        public List<Expression> Expressions { get; set; }

    }
}
