namespace ShareBear.BackgroundServices
{
    public class TimerWrapper: Timer
    {
        public TimerWrapper()
        {

        }
    }

    public class ContainerDeletionService : BackgroundService
    {
        private Timer timer;
        private readonly ILogger<ContainerDeletionService> _logger;
        private int executionCount = 0;

        public ContainerDeletionService(ILogger<ContainerDeletionService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            //timer = new Timer(HandleDeletion, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            await HandleDeletion();

        }

        private async Task HandleDeletion()
        {

        }

        private void DoWork(object? state)
        {
            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
                "Timed Hosted Service is working. Count: {Count}", count);
        }
    }
}
