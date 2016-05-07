using System;
using Serilog;
using Serilog.Events;

namespace HoneyBear.Spy.Serilog
{
    public sealed class Spy : ISpy
    {
        private readonly ILogger _logger;

        public Spy(ILogger logger)
        {
            _logger = logger;
        }

        public bool IsDebugEnabled => _logger.IsEnabled(LogEventLevel.Debug);

        public bool IsErrorEnabled => _logger.IsEnabled(LogEventLevel.Error);

        public bool IsFatalEnabled => _logger.IsEnabled(LogEventLevel.Fatal);

        public bool IsInfoEnabled => _logger.IsEnabled(LogEventLevel.Information);

        public bool IsWarnEnabled => _logger.IsEnabled(LogEventLevel.Warning);

        public void Debug(string format, params object[] args)
        {
            _logger.Debug(format, args);
        }

        public void Debug(Exception exception, string format, params object[] args)
        {
            _logger.Debug(exception, format, args);
        }

        public void Error(string format, params object[] args)
        {
            _logger.Error(format, args);
        }

        public void Error(Exception exception, string format, params object[] args)
        {
            _logger.Error(exception, format, args);
        }

        public void Fatal(string format, params object[] args)
        {
            _logger.Fatal(format, args);
        }

        public void Fatal(Exception exception, string format, params object[] args)
        {
            _logger.Fatal(exception, format, args);
        }

        public void Info(string format, params object[] args)
        {
            _logger.Information(format, args);
        }

        public void Info(Exception exception, string format, params object[] args)
        {
            _logger.Information(exception, format, args);
        }

        public void Warn(string format, params object[] args)
        {
            _logger.Warning(format, args);
        }

        public void Warn(Exception exception, string format, params object[] args)
        {
            _logger.Warning(exception, format, args);
        }
    }
}