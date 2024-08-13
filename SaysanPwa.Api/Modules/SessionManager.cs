namespace SaysanPwa.Api.Modules;

public class SessionManager
{
    private static List<string> _sessions = new();
    public static int _sessionLimit = 5;

    public static int AddSession(string sessionId)
    {
        if (IsSessionLimitReached())
        {
            return -1;
        }
        else
        {
            _sessions.Add(sessionId);
            return _sessions.Count;
        }
    }

    public static int RemoveSession(string sessionId)
    {
        if (_sessions.Contains(sessionId) && _sessions.Count > 0)
        {
            _sessions.RemoveAll(p => p == sessionId);
            return _sessions.Count;
        }
        return -1;
    }

    public static bool IsSessionExist(string sessionId) => _sessions.Count > 0 && _sessions.Contains(sessionId);

    public static bool IsSessionLimitReached() => _sessionLimit.Equals(_sessions.Count);

    public static void SetMaxSessionCount(int maxSessionCount) => _sessionLimit = maxSessionCount;

}