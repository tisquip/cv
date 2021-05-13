
using System.Net.Http;

using Microsoft.JSInterop;

namespace MyResumeSite.Services
{
    public class UtilityService
    {
        public NotificationService NotificationService { get;  }
        public SignalRService SignalRService { get; }
        public DataProvidor DataProvidor { get; }
        public UtilityService(IJSRuntime jSRuntime, HttpClient httpClient)
        {
            NotificationService = new NotificationService(jSRuntime);
            SignalRService = new SignalRService(NotificationService);
            DataProvidor = new DataProvidor(SignalRService, httpClient, NotificationService);
        }
    }
}
