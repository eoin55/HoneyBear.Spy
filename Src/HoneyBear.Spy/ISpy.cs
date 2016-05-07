using System;

namespace HoneyBear.Spy
{
    public interface ISpy
    {
        void Debug(string format, params object[] args);
        void Debug(Exception exception, string format, params object[] args);
        bool IsDebugEnabled { get; }

        void Info(string format, params object[] args);
        void Info(Exception exception, string format, params object[] args);
        bool IsInfoEnabled { get; }

        void Warn(string format, params object[] args);
        void Warn(Exception exception, string format, params object[] args);
        bool IsWarnEnabled { get; }

        void Error(string format, params object[] args);
        void Error(Exception exception, string format, params object[] args);
        bool IsErrorEnabled { get; }

        void Fatal(string format, params object[] args);
        void Fatal(Exception exception, string format, params object[] args);
        bool IsFatalEnabled { get; }
    }
}