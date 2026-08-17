using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace EAxWiki.Monitor;

/// <summary>
/// Minimal ILoggerProvider writing to {stateDir}/logs/monitor-{yyyy-MM-dd}.log with the PS
/// monitor's "yyyy-MM-dd HH:mm:ss [phase] message" shape (phase = last category segment).
/// </summary>
public sealed class MonitorFileLoggerProvider : ILoggerProvider
{
    private readonly string _logDir;

    public MonitorFileLoggerProvider(string stateDir)
    {
        _logDir = Path.Combine(stateDir, "logs");
        Directory.CreateDirectory(_logDir);
    }

    public ILogger CreateLogger(string categoryName)
    {
        var shortName = categoryName.Split('.').LastOrDefault() ?? categoryName;
        return new FileLogger(this, shortName);
    }

    public void Dispose() { }

    private sealed class FileLogger : ILogger
    {
        private readonly MonitorFileLoggerProvider _parent;
        private readonly string _name;

        public FileLogger(MonitorFileLoggerProvider parent, string name)
        {
            _parent = parent;
            _name = name;
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            var message = formatter(state, exception);
            if (exception != null) message += $" {exception.Message}";
            var line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{_name}] {message}";
            var stamp = DateTime.Now.ToString("yyyy-MM-dd");
            File.AppendAllText(Path.Combine(_parent._logDir, $"monitor-{stamp}.log"), line + Environment.NewLine);
        }
    }
}