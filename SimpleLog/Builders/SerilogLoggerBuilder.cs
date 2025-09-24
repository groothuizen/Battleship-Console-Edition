using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using Serilog;
using Serilog.Formatting.Json;

namespace SimpleLog.Builders
{
    public class SerilogLoggerBuilder
    {
        private string? _basePath;
        private string? _configPath;

        /// <summary>
        /// Sets the base path where the configuration file is located.
        /// </summary>
        public SerilogLoggerBuilder SetBasePath(string basePath)
        {
            _basePath = basePath;
            return this;
        }

        /// <summary>
        /// Sets the path from the base path where the configuration file is located.
        /// </summary>
        public SerilogLoggerBuilder SetConfigFile(string configPath)
        {
            _configPath = configPath;
            return this;
        }

        /// <summary>
        /// Builds a Serilog Logger from the specified configuration file, default path: "(base path)\settings\simplelogsettings.json".
        /// </summary>
        public ILogger Build() // maybe include a way to specify the json file's name
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath((_basePath ?? Directory.GetCurrentDirectory()))
                .AddJsonFile((_configPath ?? @"settings\simplelogsettings.json"), optional: false, reloadOnChange: true);
                //.AddJsonFile($@"simplelogsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)

            var config = builder.Build();

            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .Enrich.FromLogContext();

            var logMethodsSection = config.GetSection("SimpleLog:LogMethods");

            if (logMethodsSection.GetSection("Console").GetValue<bool>("Enabled")) logger.WriteTo.Console();

            if (logMethodsSection.GetSection("TXT").GetValue<bool>("Enabled"))
            {
                string? path = logMethodsSection.GetSection("TXT")["Path"];
                if (path != null)
                {
                    logger.WriteTo.File(path);
                }
            }

            if (logMethodsSection.GetSection("JSON").GetValue<bool>("Enabled"))
            {
                string? path = logMethodsSection.GetSection("JSON")["Path"];
                if (path != null)
                {
                    logger.WriteTo.File(new JsonFormatter(), path);
                }
            }

            return logger.CreateLogger();
        }
    }
}
