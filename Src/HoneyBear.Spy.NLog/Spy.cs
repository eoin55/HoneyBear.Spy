using System;
using System.Diagnostics;
using NLog;

namespace HoneyBear.Spy.NLog
{
    public sealed class Spy : ISpy
    {
        private static readonly Func<Logger> _logger = () => Init();

        private static Logger Init()
        {
            var trace = new StackTrace();
            const int index = 3;
            if (trace.FrameCount < index)
                return LogManager.GetCurrentClassLogger();

            var reflectedType = trace.GetFrame(index).GetMethod().ReflectedType;
            return
                reflectedType != null
                ? LogManager.GetLogger(reflectedType.FullName)
                : LogManager.GetCurrentClassLogger();
        }

        public bool IsDebugEnabled => _logger().IsEnabled(LogLevel.Debug);

        public bool IsErrorEnabled => _logger().IsEnabled(LogLevel.Error);

        public bool IsFatalEnabled => _logger().IsEnabled(LogLevel.Fatal);

        public bool IsInfoEnabled => _logger().IsEnabled(LogLevel.Info);

        public bool IsWarnEnabled => _logger().IsEnabled(LogLevel.Warn);

        public void Debug(string format, params object[] args)
            => _logger().Debug(format, args);

        public void Debug(Exception exception, string format, params object[] args)
            => _logger().Debug(exception, format, args);

        public void Error(string format, params object[] args)
            => _logger().Error(format, args);

        public void Error(Exception exception, string format, params object[] args)
            => _logger().Error(exception, format, args);

        public void Fatal(string format, params object[] args)
            => _logger().Fatal(format, args);

        public void Fatal(Exception exception, string format, params object[] args)
            => _logger().Fatal(exception, format, args);

        public void Info(string format, params object[] args)
            => _logger().Info(format, args);

        public void Info(Exception exception, string format, params object[] args)
            => _logger().Info(exception, format, args);

        public void Warn(string format, params object[] args)
            => _logger().Warn(format, args);

        public void Warn(Exception exception, string format, params object[] args)
            => _logger().Warn(exception, format, args);
    }
}