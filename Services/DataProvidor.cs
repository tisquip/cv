using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using MyResumeSiteModels.ApiResponses;

namespace MyResumeSite.Services
{
    public class DataProvidor
    {
        public event EventHandler SheduleUpdated;

        protected virtual void OnSheduleUpdated()
        {
            EventHandler handler = SheduleUpdated;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public event EventHandler LiveGameRecieved;

        protected virtual void OnLiveGameRecieved()
        {
            EventHandler handler = LiveGameRecieved;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }


        public event EventHandler StandingsUpdated;

        protected virtual void OnStandingsUpdated()
        {
            EventHandler handler = StandingsUpdated;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public event EventHandler FetchingDataStatusChanged;

        protected virtual void OnFetchingDataStatusChanged()
        {
            EventHandler handler = FetchingDataStatusChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }


        public bool IsFetchingLiveMatches { get; private set; }
        public bool IsFetchingStandings { get; private set; }
        public bool IsFetchingSchedule { get; private set; }



        public FixturesWithLeagues FixturesWithLeagues { get; private set; }
        public Fixtures LiveMatches { get; private set; }
        public List<StandingsApiResponseWithLeagueData> Standings { get; private set; } = new List<StandingsApiResponseWithLeagueData>();

        private readonly SignalRService _signalRService;
        private readonly HttpClient _httpClient;
        private readonly NotificationService _notificationService;

        public DataProvidor(SignalRService signalRService, HttpClient httpClient, NotificationService notificationService)
        {
            _signalRService = signalRService;
            _httpClient = httpClient;
            _notificationService = notificationService;
        }

        public async Task GetPreLoadData()
        {
            try
            {
                await _notificationService.ConsoleLog("Pre load Called");
                Task taskLive = SetLiveGamesFromPreLoad();

                Task taskShedule = SetScheduleFromPreLoad();

                Task taskStandings = SetStandingsFromPreLoad();

                await Task.WhenAll(taskLive, taskShedule, taskStandings);

                await _notificationService.ConsoleLog("Pre load Successuly completed");


            }
            catch (Exception ex)
            {
                await _notificationService.ConsoleLog("Error occured in Preload");
                await _notificationService.ConsoleLog(ex);

            }

        }

        private async Task SetStandingsFromPreLoad()
        {
            IsFetchingStandings = true;
            OnFetchingDataStatusChanged();

            Standings = (await _httpClient.GetFromJsonAsync<List<StandingsApiResponseWithLeagueData>>("api/PreLoad/Standings")) ?? new List<StandingsApiResponseWithLeagueData>();

            IsFetchingStandings = false;
            OnFetchingDataStatusChanged();
        }

        private async Task SetScheduleFromPreLoad()
        {
            IsFetchingSchedule = true;
            OnFetchingDataStatusChanged();

            FixturesWithLeagues = await _httpClient.GetFromJsonAsync<FixturesWithLeagues>("api/PreLoad/Schedule");

            IsFetchingSchedule = false;
            OnFetchingDataStatusChanged();
        }

        private async Task SetLiveGamesFromPreLoad()
        {
            IsFetchingLiveMatches = true;
            OnFetchingDataStatusChanged();

            LiveMatches = await _httpClient.GetFromJsonAsync<Fixtures>("api/PreLoad/LiveMatches");

            IsFetchingLiveMatches = true;
            OnFetchingDataStatusChanged();
        }
    }
}
