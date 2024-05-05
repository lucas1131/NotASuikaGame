using UnityEngine;

class DebugLogger : ILogger
{
    public void LogInfo(string msg) => Debug.Log(msg);
    public void LogWarning(string msg) => Debug.LogWarning(msg);
    public void LogError(string msg) => Debug.LogError(msg);
}

class AppLogger : ILogger
{
    // Although these classes are the same, this is intended to use a proper logging like firebase or something
    // instead of using unity's Debug.Log. But as I dont have any cloud services for this, its just the same as above.
    public void LogInfo(string msg) => Debug.Log(msg);
    public void LogWarning(string msg) => Debug.LogWarning(msg);
    public void LogError(string msg) => Debug.LogError(msg);
}

public class LoggerFactory
{
    public static ILogger Create()
    {

#if UNITY_EDITOR
        return new DebugLogger();
#else
        return new AppLogger();
#endif
    }
}
