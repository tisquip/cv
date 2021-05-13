using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR.Client;

using MyResumeSiteModels;
using MyResumeSiteModels.ApiResponses;

namespace MyResumeSite.Services
{
    public class SignalRService
    {
        public HubConnection HubConnection { get; private set; }
        public event EventHandler<Fixtures> LiveMatchUpdate;


        protected virtual void OnLiveMatchUpdate(string fixtureJson)
        {
            try
            {
                if (LiveMatchUpdate != null)
                {
                    Fixtures vts = Newtonsoft.Json.JsonConvert.DeserializeObject<Fixtures>(fixtureJson);
                    if (vts != null)
                    {
                        LiveMatchUpdate.Invoke(this, vts);
                    }
                }
            }
            catch (Exception ex)
            {
                ;
            }
        }

        public event EventHandler FixturesUpdates;
        protected virtual void OnFixturesUpdates()
        {
            EventHandler handler = FixturesUpdates;
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

        static bool _isAttemptingToConnect = false;
        private readonly NotificationService _notificationService;

        public SignalRService(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task ConnectIfNecessary()
        {
            if (!_isAttemptingToConnect && (HubConnection == null || HubConnection.State == HubConnectionState.Disconnected))
            {
                _isAttemptingToConnect = true;
                try
                {
                    string url = VariablesCore.SignalRFullEndpoint();
                    HubConnection = new HubConnectionBuilder()
                        .WithUrl(url)
                        .WithAutomaticReconnect()
                        .Build();

                    HubConnection.On(VariablesCore.SignalRMethodNameFixturesUpdated, () =>
                    {
                        OnFixturesUpdates();
                    });

                    HubConnection.On<string>(VariablesCore.SignalRMethodNameLiveMatch, (fixturesJson) =>
                    {
                        OnLiveMatchUpdate(fixturesJson);
                    });

                    HubConnection.On(VariablesCore.SignalRMethodNameStandings, () =>
                    {
                        OnStandingsUpdated();
                    });

                    await HubConnection.StartAsync();
                }
                catch (Exception ex)
                {
                    string m = ex.Message;
                    await _notificationService.ConsoleLog($"Error Connecting To Realtime:");
                    await _notificationService.ConsoleLog(ex);
                    throw;
                }

                await _notificationService.ConsoleLog($"Real Time Connection State : {HubConnection.State}");
                _isAttemptingToConnect = false;
            }
        }
    }
}
