
using MyResumeSiteModels.ApiResponses;

namespace MyResumeSite.Pages.ComponentOptions
{
    public class LiveMatchDisplayComponentOptions
    {
        public FixturesData FixturesData { get; set; }
        public bool NoCardMargin { get; set; }

        public LiveMatchDisplayComponentOptions(FixturesData fixturesData, bool noCardMargin = false)
        {
            FixturesData = fixturesData;
            NoCardMargin = noCardMargin;
        }
    }
}
