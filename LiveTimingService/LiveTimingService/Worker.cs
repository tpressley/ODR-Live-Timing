using Microsoft.Extensions.Logging;

namespace ODR.LiveTiming.UploadService;

public class Worker : BackgroundService
{
    private readonly GitUploadService _fileService;
    private readonly ILogger<Worker> _logger;
    private readonly Options Options; 
    public Worker(
        GitUploadService fileService,
        Options options,
        ILogger<Worker> logger)
    {
        (_fileService, _logger, Options) = (fileService, logger, options);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _fileService.MoveFileAndCommit();
                await Task.Delay(TimeSpan.FromSeconds(Options.FrequencyInSeconds), stoppingToken);
            }
        }
        catch (TaskCanceledException)
        {
            // When the stopping token is canceled, for example, a call made from services.msc,
            // we shouldn't exit with a non-zero exit code. In other words, this is expected...
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Message}", ex.Message);
            File.AppendAllText("log.txt", DateTime.Now + ex.Message + ex.StackTrace + Environment.NewLine);

            // Terminates this process and returns an exit code to the operating system.
            // This is required to avoid the 'BackgroundServiceExceptionBehavior', which
            // performs one of two scenarios:
            // 1. When set to "Ignore": will do nothing at all, errors cause zombie services.
            // 2. When set to "StopHost": will cleanly stop the host, and log errors.
            //
            // In order for the Windows Service Management system to leverage configured
            // recovery options, we need to terminate the process with a non-zero exit code.
            Environment.Exit(1);
        }
    }
}
