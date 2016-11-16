using System;
using System.Configuration;
using System.Reflection;
using HoneyBear.Spy.Configuration;
using HoneyBear.Spy.Serilog.Configuration;
using HoneyBear.Spy.Serilog.Enrichers;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Environment = HoneyBear.Spy.Configuration.Environment;

namespace HoneyBear.Spy.Serilog
{
    internal sealed class SpyBuilder : ISpyBuilder, ISpyLogLevel, ISpyFileLocation, ISpyFileName
    {
        private readonly ISpyLogLevelDecider _customLogLevelDecider;
        private readonly ISpyLogLevelDecider _defaultLogLevelDecider;
        private readonly LoggerConfiguration _configuration;
        private LogEventLevel? _logLevel;
        private string _filePath;
        private string _productName;
        private string _fileName;

        private const LogEventLevel DefaultLogLevel = LogEventLevel.Information;
        private const string DefaultFilePath = @"C:\Logs";
        private const string Template =
            "{Timestamp:HH:mm:ss.fffzzz}\t[{Level}]\t[{MachineName}]\t[{ProcessId}]\t[{ThreadId}]\t[{ClassName}]\t[{MethodName}]\t[{Message}]{NewLine}{Exception}";

        public SpyBuilder()
        {
            _configuration = new LoggerConfiguration();
            _defaultLogLevelDecider = new DefaultLogLevelDecider();
        }

        public SpyBuilder(ISpyLogLevelDecider customLogLevelDecider)
            : this()
        {
            _customLogLevelDecider = customLogLevelDecider;
        }

        public ISpyLogLevel WithLogLevel => this;

        public ISpyFileLocation WithFileLocation => this;

        public ISpyFileName WithFileName => this;

        ISpyBuilder ISpyLogLevel.Value(string level)
        {
            LogEventLevel value;
            if (Enum.TryParse(level, true, out value))
                _logLevel = value;
            return this;
        }

        ISpyBuilder ISpyLogLevel.OverriddenFromAppSetting() =>
            WithLogLevel.Value(ConfigurationManager.AppSettings["HoneyBear.Spy.LogLevel"]);

        ISpyBuilder ISpyLogLevel.ByEnvironment(string environment)
        {
            if (_customLogLevelDecider != null)
            {
                _logLevel = _customLogLevelDecider.ByEnvironment(environment);
                return this;
            }

            _logLevel = _defaultLogLevelDecider.ByEnvironment(environment);
            return this;
        }

        ISpyBuilder ISpyLogLevel.ByEnvironment(Environment environment)
        {
            if (_customLogLevelDecider != null)
            {
                _logLevel = _customLogLevelDecider.ByEnvironment(environment);
                return this;
            }

            _logLevel = _defaultLogLevelDecider.ByEnvironment(environment);
            return this;
        }

        ISpyBuilder ISpyLogLevel.ByEnvironmentFromAppSetting() =>
            WithLogLevel.ByEnvironment(ConfigurationManager.AppSettings["HoneyBear.Spy.Environment"]);

        ISpyBuilder ISpyFileLocation.Value(string path)
        {
            if (!string.IsNullOrEmpty(path))
                _filePath = path;
            return this;
        }

        ISpyBuilder ISpyFileLocation.OverriddenFromAppSetting() =>
            WithFileLocation.Value(ConfigurationManager.AppSettings["HoneyBear.Spy.LogFilePath"]);

        ISpyBuilder ISpyFileLocation.OverriddenFromEnvironmentVariable(string name) =>
            WithFileLocation.Value(System.Environment.GetEnvironmentVariable(name));

        ISpyBuilder ISpyFileLocation.OverriddenFromEnvironmentVariable() =>
            WithFileLocation.OverriddenFromEnvironmentVariable("HoneyBear.Spy.LogDir");

        public ISpyBuilder AppendProductName(string name)
        {
            if (!string.IsNullOrEmpty(name))
                _productName = name;
            return this;
        }

        public ISpyBuilder AppendProductNameFromAppSetting() =>
            AppendProductName(ConfigurationManager.AppSettings["HoneyBear.Spy.ProductName"]);

        ISpyBuilder ISpyFileName.Value(string name)
        {
            if (!string.IsNullOrEmpty(name))
                _fileName = name;
            return this;
        }

        ISpyBuilder ISpyFileName.OverriddenFromAppSetting() =>
            WithFileName.Value(ConfigurationManager.AppSettings["HoneyBear.Spy.LogFileName"]);

        public ISpy Create()
        {
            if (!_logLevel.HasValue)
                _logLevel = DefaultLogLevel;

            if (string.IsNullOrEmpty(_filePath))
                _filePath = DefaultFilePath;

            if (!string.IsNullOrEmpty(_productName))
                _filePath = $"{_filePath}\\{_productName}";

            if (string.IsNullOrEmpty(_fileName))
            {
                string appName;
                if (AssemblyNameIsAvailable)
                    appName = Assembly.GetEntryAssembly().GetName().Name;
                else if (!TryReadApplicationNameFromAppSettings(out appName))
                    throw new ApplicationNameNotSpecified();
                _fileName = string.Concat(appName, "-{Date}.log");
            }

            var fullFilePath = $"{_filePath}\\{_fileName}";

            _configuration
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .Enrich.With<ClassAndMethodEnricher>();

            _configuration.WriteTo.ColoredConsole(_logLevel.Value, Template);

            _configuration.WriteTo.RollingFile(fullFilePath, _logLevel.Value, Template);

            _configuration.MinimumLevel.ControlledBy(new LoggingLevelSwitch(_logLevel.Value));

            return new Spy(_configuration.CreateLogger());
        }

        private static bool TryReadApplicationNameFromAppSettings(out string appName)
        {
            appName = ConfigurationManager.AppSettings["HoneyBear.Spy.ApplicationName"];
            return appName != null;
        }

        private static bool AssemblyNameIsAvailable => Assembly.GetEntryAssembly() != null;
    }
}