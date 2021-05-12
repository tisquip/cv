namespace MyResumeSite
{
    public static class Variables
    {
        public static string ServerSignalREndpoint => "https://localhost:44371/messages/";
        public static string SignalRMethodNameFixturesUpdated => "RealTimeFixturesChanged";
        public static string SignalRMethodNameLiveMatch => "RealTimeLiveMatch";
    }
}
