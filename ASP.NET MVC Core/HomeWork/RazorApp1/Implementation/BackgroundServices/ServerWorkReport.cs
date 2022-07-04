using RazorApp1.Domain.Services.MailService;
using System.Diagnostics;

namespace RazorApp1.Implementation.BackgroundServices
{
    public class ServerWorkReport : BackgroundService
    {
        private readonly TimeSpan _timer = TimeSpan.FromHours(1);
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ServerWorkReport> _logger;

        public ServerWorkReport(IServiceProvider serviceProvider, ILogger<ServerWorkReport> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(_timer);
            var swServer = Stopwatch.StartNew();
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await SendReport($"Server works product {swServer.Elapsed.Hours} hours", stoppingToken);
            }
        }

        private async Task SendReport(string body, CancellationToken token)
        {
            using var scope = _serviceProvider.CreateAsyncScope();
            var mailService = scope.ServiceProvider.GetRequiredService<IMailService>();

            if (mailService != null)
            {
                await mailService.SendEmailAsync(nameof(ServerWorkReport), body, token);
            }
            else
            {
                throw new NullReferenceException("IMailService is null reference...");
            }
        }
    }
}
