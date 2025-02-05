using Microsoft.Extensions.Logging;

namespace MeowFaceVRCFTInterface.Logger
{
    // If they fix it, it will be easier to remove this extension.
    public class VrcftExceptionFixerLogger : ILogger
    {
        private readonly ILogger _baseLogger;

        public VrcftExceptionFixerLogger(ILogger logger)
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

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            string addException(TState state, Exception? exception)
            {
                if (exception != null)
                {
                    return $"{formatter(state, exception)}\n{exception}";
                }
                else
                {
                    return formatter(state, exception);
                }
            }

            _baseLogger.Log(logLevel, eventId, state, exception, addException);
        }
    }
}
