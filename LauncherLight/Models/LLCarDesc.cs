using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace LauncherLight.Models
{
    [ImplementPropertyChanged]
    public class LLCarDesc : ACSharedMemory.Models.Car.CarDesc
    {
        public LLCarDesc(string ACPAth, string model) : base(ACPAth, model)
        {
            this.FirstSeen = ModHistoryProvider.GetFirstSeen(model);
        }

        private DateTime FirstSeen { get; set; }

        public LLCarDesc() : base()
        {
            this.FirstSeen = DateTime.MinValue;
        }

        public bool IsRecentMod { get { return this.FirstSeen >= DateTime.Now.Date.AddDays(-Properties.Settings.Default.LastModsDays); } }
    }
}