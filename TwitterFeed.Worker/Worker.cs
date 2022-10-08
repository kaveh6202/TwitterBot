using TwitterFeed.Infra.Interface;

namespace TwitterFeed.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ITwitterSyncer _twitterSyncer;

        public Worker(ILogger<Worker> logger, ITwitterSyncer twitterSyncer)
        {
            _logger = logger;
            _twitterSyncer = twitterSyncer;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _twitterSyncer.SyncTwitterTimelineToTelegram();
        }
    }
}