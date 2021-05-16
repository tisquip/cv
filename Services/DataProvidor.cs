using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

using MyResumeSiteModels.ApiResponses;

namespace MyResumeSite.Services
{
    public class DataProvidor : IDisposable
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

            _signalRService.FixturesUpdates -= SignalRService_FixturesUpdates;
            _signalRService.FixturesUpdates += SignalRService_FixturesUpdates;

            _signalRService.LiveMatchUpdate -= SignalRService_LiveMatchUpdate;
            _signalRService.LiveMatchUpdate += SignalRService_LiveMatchUpdate;

            _signalRService.StandingsUpdated -= SignalRService_StandingsUpdated;
            _signalRService.StandingsUpdated += SignalRService_StandingsUpdated;
        }

        async void SignalRService_StandingsUpdated(object sender, EventArgs e)
        {
            await SetStandingsFromPreLoad();
        }

        void SignalRService_LiveMatchUpdate(object sender, Fixtures fixtures)
        {
            LiveMatches = fixtures;
            OnLiveGameRecieved();
        }

        async void SignalRService_FixturesUpdates(object sender, EventArgs e)
        {
            await SetScheduleFromPreLoad();
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

            if (Standings?.Any() ?? false)
            {
                var Scottish = Standings.FirstOrDefault(s => s.StandingsApiResponse.data.Any(s => s.name.ToUpper() == "1st Phase".ToUpper()));
                if (Scottish != null)
                {
                    Scottish.StandingsApiResponse.data = Scottish.StandingsApiResponse.data.Where(d => d.name.ToUpper() == "1st Phase".ToUpper()).ToArray();
                }

                var Danish = Standings.FirstOrDefault(s => s.StandingsApiResponse.data.Any(s => s.name.ToUpper() == "Regular Season".ToUpper()));
                if (Danish != null)
                {
                    Danish.StandingsApiResponse.data = Danish.StandingsApiResponse.data.Where(d => d.name.ToUpper() == "Regular Season".ToUpper()).ToArray();
                }

                Standings = new List<StandingsApiResponseWithLeagueData>();
                if (Scottish != null)
                {
                    Standings.Add(Scottish);
                }

                if (Danish != null)
                {
                    Standings.Add(Danish);
                }
            }

            OnStandingsUpdated();

            IsFetchingStandings = false;
            OnFetchingDataStatusChanged();
        }

        private async Task SetScheduleFromPreLoad()
        {
            IsFetchingSchedule = true;
            OnFetchingDataStatusChanged();

            FixturesWithLeagues = await _httpClient.GetFromJsonAsync<FixturesWithLeagues>("api/PreLoad/Schedule");

            OnSheduleUpdated();

            IsFetchingSchedule = false;
            OnFetchingDataStatusChanged();
        }

        private async Task SetLiveGamesFromPreLoad()
        {
            IsFetchingLiveMatches = true;
            OnFetchingDataStatusChanged();

            LiveMatches = await _httpClient.GetFromJsonAsync<Fixtures>("api/PreLoad/LiveMatches");

            OnLiveGameRecieved();

            IsFetchingLiveMatches = false;
            OnFetchingDataStatusChanged();
        }

        public void Dispose()
        {
            _signalRService.FixturesUpdates -= SignalRService_FixturesUpdates;

            _signalRService.LiveMatchUpdate -= SignalRService_LiveMatchUpdate;

            _signalRService.StandingsUpdated -= SignalRService_StandingsUpdated;
        }
    }
}
