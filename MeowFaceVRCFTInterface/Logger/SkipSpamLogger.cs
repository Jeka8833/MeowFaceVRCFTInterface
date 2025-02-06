using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace MeowFaceVRCFTInterface.Logger;

public class SkipSpamLogger : ILogger
{
    private readonly object _mutex = new();
    private readonly ILogger _baseLogger;

    public long SendEveryMillis { get; set; } = 5_000;

    private long _nextAllowSendMillis = Stopwatch.GetTimestamp();
    private int _skippedCount;

    public SkipSpamLogger(ILogger logger)
    {
        _baseLogger = logger;
    }

    public ILogger GetRegularLogger()
    {
        return _baseLogger;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return _baseLogger.BeginScope(state);
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _baseLogger.IsEnabled(logLevel);
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId,
        TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        lock (_mutex)
        {
            if (Stopwatch.GetTimestamp() - _nextAllowSendMillis >= 0)
            {
                _nextAllowSendMillis = Stopwatch.GetTimestamp() + SendEveryMillis * 10_000L;

                if (_skippedCount > 0)
                {
                    _baseLogger.LogInformation("[SkipSpamLogger] {} messages were ignored.", _skippedCount);

                    _skippedCount = 0;
                }

                _baseLogger.Log(logLevel, eventId, state, exception, formatter);
            }
            else
            {
                _skippedCount++;
            }
        }
    }
}