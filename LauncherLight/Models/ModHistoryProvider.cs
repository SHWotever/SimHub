using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherLight.Models
{
    public class ModHistoryProvider
    {
        private static Dictionary<string, DateTime> ModHistory = new Dictionary<string, DateTime>();

        private static bool IsFirstInit { get; set; }

        static ModHistoryProvider()
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.CarFirstSeen))
            {
                ModHistory = JsonConvert.DeserializeObject<Dictionary<string, DateTime>>(Properties.Settings.Default.CarFirstSeen) ?? new Dictionary<string, DateTime>();
            }
            else
            {
                IsFirstInit = true;
            }
        }

        public static DateTime GetFirstSeen(string model)
        {
            if (!ModHistory.ContainsKey(model))
            {
                ModHistory.Add(model, !IsFirstInit ? DateTime.Now : DateTime.MinValue);
                Properties.Settings.Default.CarFirstSeen = JsonConvert.SerializeObject(ModHistory);
                Properties.Settings.Default.Save();
            }
            return ModHistory[model];
        }
    }
}