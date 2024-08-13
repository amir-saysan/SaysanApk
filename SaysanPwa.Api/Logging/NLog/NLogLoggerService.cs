using NLog;

namespace SaysanPwa.Api.Logging.NLog;

public class NLogLoggerService : ILoggerService
{
    private readonly Logger _logger;

    public NLogLoggerService()
    {
        _logger = LogManager.GetCurrentClassLogger();
    }

    public void LogInfo(string message) => _logger.Info(message);
    public void LogWarn(string message) => _logger.Warn(message);

    public void LogDebug(string message) => _logger.Debug(message);

    public void LogError(string message) => _logger.Error(message);

    public void LogError(Exception ex) => _logger.Error(ex);

    public void LogError(Exception ex, string message) => _logger.Error(ex, message);

}
