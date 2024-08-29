
using Microsoft.EntityFrameworkCore;
using TextStorage.Persistence;

namespace TextStorage.Workers
{
    public class CleanUpPastesBackgroundService(IServiceScopeFactory serviceScopeFactory) : BackgroundService
    {
     
        private readonly IServiceScopeFactory  _serviceScopeFactory =serviceScopeFactory;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var serviceProvider = _serviceScopeFactory.CreateScope().ServiceProvider;
            var loadBalancer = serviceProvider.GetRequiredService<LoadBalancer>();

            while (!stoppingToken.IsCancellationRequested)
            {
                foreach (var conStr in loadBalancer.GetConnections())
                {
                    var options = new DbContextOptionsBuilder<TextStorageDbContext>()
                                            .UseSqlServer(conStr)
                                            .Options;

                    var dbContext = new TextStorageDbContext(options);


                    await dbContext.Pastes
                              .Where(x => x.ExpireOn < DateTime.Now)
                                  .ExecuteDeleteAsync();

                    await Task.Delay(5000);


                }
            }   
        }
    }
}
