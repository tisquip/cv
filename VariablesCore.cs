﻿namespace MyResumeSiteModels
{
    public static class VariablesCore
    {
        public static string MessageHubUrlEndPointWithPreSlash => "/messages";
        public static string SignalRMethodNameFixturesUpdated => "RealTimeFixturesChanged";
        public static string SignalRMethodNameLiveMatch => "RealTimeLiveMatch";
        public static string SignalRMethodNameStandings => "RealTimeStandings";
        public static string ServerUrl => "https://myresumesitebackend.azurewebsites.net/";// "https://myresumesitebackend.azurewebsites.net/"; //"https://localhost:44371/";

        public static string SignalRFullEndpoint()
        {
            string messageEndPoint = MessageHubUrlEndPointWithPreSlash;
            messageEndPoint = messageEndPoint.Remove(0, 1);
            return ServerUrl + messageEndPoint;
        }
    }
}
