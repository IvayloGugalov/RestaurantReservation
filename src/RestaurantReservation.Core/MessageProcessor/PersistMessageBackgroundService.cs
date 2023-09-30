using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace RestaurantReservation.Core.MessageProcessor;

public class PersistMessageBackgroundService : BackgroundService
{
    private readonly ILogger<PersistMessageBackgroundService> logger;
    private readonly IServiceProvider serviceProvider;
    private readonly MessageOptions options;
    private Task? executingTask;

    public PersistMessageBackgroundService(
        ILogger<PersistMessageBackgroundService> logger,
        IServiceProvider serviceProvider,
        IOptions<MessageOptions> options)
    {
        this.logger = logger;
        this.serviceProvider = serviceProvider;
        this.options = options.Value;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this.logger.LogInformation("PersistMessage Background Service Start");

        this.executingTask = this.ProcessAsync(stoppingToken);

        return this.executingTask;
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        this.logger.LogInformation("PersistMessage Background Service Stop");

        return base.StopAsync(cancellationToken);
    }

    private async Task ProcessAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await using (var scope = this.serviceProvider.CreateAsyncScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IMessageProcessor>();
                await service.ProcessAllAsync(stoppingToken);
            }

            var delay = this.options.Interval is not null
                ? TimeSpan.FromSeconds((int)this.options.Interval)
                : TimeSpan.FromSeconds(30);

            await Task.Delay(delay, stoppingToken);
        }
    }
}
