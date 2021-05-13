using System.Collections.Generic;
using System.Linq;

using MyResumeSiteModels.ApiResponses;

namespace MyResumeSite.Pages.ComponentOptions
{
    public class StandingsDisplayComponentOptions
    {
        public List<StandingsData> StandingsData { get; set; } = new List<StandingsData>();
        public LeaguesData LeaguesData { get; set; }

        public StandingsDisplayComponentOptions(List<StandingsData> standingsData, LeaguesData leaguesData)
        {
            StandingsData = standingsData?.OrderBy(s => s.position)?.ToList();
            LeaguesData = leaguesData;
        }
    }
}
