using System.Configuration;
using System.Reflection;
using HoneyBear.Spy.Configuration;
using HoneyBear.Spy.NLog.Configuration;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using Environment = HoneyBear.Spy.Configuration.Environment;

namespace HoneyBear.Spy.NLog
{
    internal sealed class SpyBuilder : ISpyBuilder, ISpyLogLevel, ISpyFileLocation, ISpyFileName
    {
        private readonly ISpyLogLevelDecider _customLogLevelDecider;
        private readonly ISpyLogLevelDecider _defaultLogLevelDecider;
        private readonly LoggingConfiguration _configuration;
        private static readonly LogLevel _defaultLogLevel = LogLevel.Info;
        private LogLevel _logLevel;
        private string _productName;
        private string _filePath;
        private string _appName;
        private const string DefaultFilePath = @"C:\Logs";
        private const int DefaultMaxNumberOfArchiveFiles = 50;
        private const int DefaultMaxFileSizeInBytes = 5242880; // 50Mb

        private const string Template =
            "${longdate}\t[${level:uppercase=true}]\t[${machinename}]\t[${processid}]\t[${threadid}]\t[${logger}]\t[${message}]\t${exception:format=tostring}";

        public SpyBuilder()
        {
            _defaultLogLevelDecider = new DefaultLogLevelDecider();
            _configuration = new LoggingConfiguration();
        }

        public SpyBuilder(ISpyLogLevelDecider customLogLevelDecider)
            : this()
        {
            _customLogLevelDecider = customLogLevelDecider;
        }

        public ISpyLogLevel WithLogLevel => this;

        public ISpy Create()
        {
            if (_logLevel == null)
                _logLevel = _defaultLogLevel;

            if (string.IsNullOrEmpty(_filePath))
                _filePath = DefaultFilePath;

            if (!string.IsNullOrEmpty(_productName))
                _filePath = $"{_filePath}\\{_productName}";

            if (string.IsNullOrEmpty(_appName))
                ResolveApplicationName();

            ConfigureColoredConsoleTarget();
            ConfigureCombinedRollingFileTarget();
            ConfigureLogLevelRollingFileTarget();

            LogManager.Configuration = _configuration;
            return new Spy();
        }

        public ISpyFileLocation WithFileLocation => this;

        public ISpyFileName WithFileName => this;

        ISpyBuilder ISpyLogLevel.Value(string level)
        {
            if (string.IsNullOrEmpty(level))
                return this;

            _logLevel = LogLevel.FromString(level);
            return this;
        }

        ISpyBuilder ISpyFileName.OverriddenFromAppSetting() =>
            WithFileName.Value(ConfigurationManager.AppSettings["HoneyBear.Spy.LogFileName"]);

        ISpyBuilder ISpyFileName.Value(string name)
        {
            if (string.IsNullOrEmpty(name))
                return this;

            _appName = name;
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

        ISpyBuilder ISpyFileLocation.Value(string path)
        {
            if (!string.IsNullOrEmpty(path))
                _filePath = path;
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

        private void ResolveApplicationName()
        {
            if (AssemblyNameIsAvailable)
                _appName = Assembly.GetEntryAssembly().GetName().Name;
            else if (!TryReadApplicationNameFromAppSettings(out _appName))
                throw new ApplicationNameNotSpecified();
        }

        private void ConfigureColoredConsoleTarget()
        {
            var target =
                new ColoredConsoleTarget("ColoredConsoleTarget")
                {
                    Layout = new SimpleLayout(Template)
                };
            AddTarget(target);
        }

        private void ConfigureCombinedRollingFileTarget()
        {
            var target =
                new FileTarget("CombinedLogLevelRollingFile")
                {
                    Layout = new SimpleLayout(Template),
                    FileName = $"{_filePath}\\{string.Concat(_appName, "-${shortdate}.All.log")}",
                    ArchiveFileName = $"{_filePath}\\{string.Concat(_appName, "-${shortdate}_{##}.All.log")}",
                    MaxArchiveFiles = DefaultMaxNumberOfArchiveFiles,
                    ArchiveAboveSize = DefaultMaxFileSizeInBytes
                };
            AddTarget(target);
        }

        private void ConfigureLogLevelRollingFileTarget()
        {
            var target =
                new FileTarget("LogLevelRollingFile")
                {
                    Layout = new SimpleLayout(Template),
                    FileName = $"{_filePath}\\{string.Concat(_appName, "-${shortdate}.${level}.log")}",
                    ArchiveFileName = $"{_filePath}\\{string.Concat(_appName, "-${shortdate}_{##}.${level}.log")}",
                    MaxArchiveFiles = DefaultMaxNumberOfArchiveFiles,
                    ArchiveAboveSize = DefaultMaxFileSizeInBytes
                };
            AddTarget(target);
        }

        private void AddTarget(Target target)
        {
            _configuration.AddTarget(target);
            _configuration.AddRule(_logLevel, LogLevel.Fatal, target);
        }

        private static bool TryReadApplicationNameFromAppSettings(out string appName)
        {
            appName = ConfigurationManager.AppSettings["HoneyBear.Spy.ApplicationName"];
            return appName != null;
        }

        private static bool AssemblyNameIsAvailable => Assembly.GetEntryAssembly() != null;
    }
}