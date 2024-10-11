
using WebApplication2.Persistence.Context;
using WebApplication2.Persistence.Models;
using WebApplication2.PolygonIntegration;

namespace WebApplication2.BackgroundServices
{
    public class PeriodicNotificationHostedService : BackgroundService
    {

        private readonly TimeSpan _period = TimeSpan.FromHours(6);
        private readonly PolygonClient polygonClient;
        private readonly IServiceScopeFactory scopeFactory;
        private readonly EmailNotificationService emailNotificationService;

        public PeriodicNotificationHostedService(PolygonClient polygonClient, IServiceScopeFactory scopeFactory, EmailNotificationService emailNotificationService)
        {
            this.polygonClient = polygonClient;
            this.scopeFactory = scopeFactory;
            this.emailNotificationService = emailNotificationService;
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(_period);
            do
            {
                var stockData = await polygonClient.GetStockDataAsync("AAPL");

                using (var scope = scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<UsersContext>();
                    dbContext.Set<StockData>().Add(stockData);
                    await dbContext.SaveChangesAsync();
                    var emails = dbContext.Set<Users>().Select(x => x.EmailAddress).Distinct().ToList();
                    foreach (var item in emails)
                    {
                        await emailNotificationService.SendStockDataEmailAsync(stockData, item);
                    }
                }
            } while (
                !stoppingToken.IsCancellationRequested &&
                await timer.WaitForNextTickAsync(stoppingToken));
        }
    }
}
