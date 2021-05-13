using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MyResumeSiteModels.ApiResponses;

namespace MyResumeSite.Pages.ComponentOptions
{
    public class UpcomingFixtureDisplayComponentOptions
    {
        public FixturesData FixturesData { get; set; }
        public bool NoCardMargin { get; }

        public UpcomingFixtureDisplayComponentOptions(FixturesData fixturesData, bool noCardMargin = false)
        {
            FixturesData = fixturesData;
            NoCardMargin = noCardMargin;
        }
    }
}
