
using Microsoft.JSInterop;

namespace MyResumeSite.Services
{
    public class UtilityService
    {
        public NotificationService NotificationService { get;  }
        public SignalRService SignalRService { get; }

        public UtilityService(IJSRuntime jSRuntime)
        {
            NotificationService = new NotificationService(jSRuntime);
            SignalRService = new SignalRService(NotificationService);
        }
    }
}
