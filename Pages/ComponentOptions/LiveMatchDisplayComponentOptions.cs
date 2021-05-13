
using MyResumeSiteModels.ApiResponses;

namespace MyResumeSite.Pages.ComponentOptions
{
    public class LiveMatchDisplayComponentOptions
    {
        public FixturesData FixturesData { get; set; }

        public LiveMatchDisplayComponentOptions(FixturesData fixturesData)
        {
            FixturesData = fixturesData;
        }
    }
}
