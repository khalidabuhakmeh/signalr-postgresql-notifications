using Microsoft.AspNetCore.SignalR;

namespace Realtime
{
    public class CountingHub : Hub
    {
        public static class Events
        {
            public const string UpdateCount = "count_update";            
        }
        
        public const string Endpoint = "/realtime/counts";
    }
}