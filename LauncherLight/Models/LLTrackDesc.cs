using ACSharedMemory.Models.Track;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LauncherLight.Models
{
    public class LLTrackDesc : TrackDesc
    {
        public LLTrackDesc(string acpath, string trackCode, string trackConfig)
            : base(acpath, trackCode, trackConfig)
        {
            this.FirstSeen = ModHistoryProvider.GetFirstSeen((trackCode ?? "") + "_" + (trackConfig ?? ""));
        }

        private DateTime FirstSeen { get; set; }

        public LLTrackDesc()
            : base()
        {
            this.FirstSeen = DateTime.MinValue;
        }

        public bool IsRecentMod { get { return this.FirstSeen.Date >= DateTime.Now.Date.AddDays(-Properties.Settings.Default.LastModsDays); } }
    }
}