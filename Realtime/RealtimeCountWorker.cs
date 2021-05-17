using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Realtime
{
    public class RealtimeCountWorker : BackgroundService
    {
        private readonly IHubContext<CountingHub> hub;
        private readonly NpgsqlConnection connection;
        private readonly ILogger<RealtimeCountWorker> logger;

        public RealtimeCountWorker(
            IHubContext<CountingHub> hub, 
            NpgsqlConnection connection,
            ILogger<RealtimeCountWorker> logger
        )
        {
            this.hub = hub;
            this.connection = connection;
            this.logger = logger;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await connection.OpenAsync(stoppingToken);
            connection.Notification += OnNotification;

            await using var command = new NpgsqlCommand("LISTEN total_count;", connection);
            await command.ExecuteNonQueryAsync(stoppingToken);
            
            while (!stoppingToken.IsCancellationRequested)
            {
                await connection.WaitAsync(stoppingToken);
            }

            await Task.CompletedTask;
        }

        private async void OnNotification(object sender, NpgsqlNotificationEventArgs args)
        {
            try
            {
                // let's update some pages
                await hub.Clients.All.SendAsync(
                    CountingHub.Events.UpdateCount, 
                    args.Payload
                );
                
                logger.LogInformation("Sent Payload [{Payload}] to client", args.Payload);
            }
            catch (Exception e)
            {
                logger.LogError(e, "unexpected error: [{Payload}]", args.Payload);
            }
        }

        public override void Dispose()
        {
            connection?.Dispose();
            base.Dispose();
        }
    }
}