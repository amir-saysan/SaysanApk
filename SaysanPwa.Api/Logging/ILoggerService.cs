namespace SaysanPwa.Api.Logging;

public interface ILoggerService
{
    public void LogInfo(string message);
    public void LogWarn(string message);
    public void LogDebug(string message);
    public void LogError(string message);
    public void LogError(Exception ex);
    public void LogError(Exception ex, string message);
}
