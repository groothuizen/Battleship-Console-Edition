using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
        /// Sets the path from the base path to where the configuration file is located.
        /// </summary>
        public SerilogLoggerBuilder SetConfigFile(string configPath)
        {
            _configPath = configPath;
            return this;
        }

        /// <summary>
        /// Builds a Serilog Logger from the specified configuration file, default path: "(base path)\appsettings.json".
        /// </summary>
        public ILogger Build() // maybe include a way to specify the json file's name
        {
            var config = new ConfigurationBuilder()
                .SetBasePath((_basePath ?? Directory.GetCurrentDirectory()))
                .AddJsonFile((_configPath ?? "appsettings.json"), optional: false, reloadOnChange: true)
                //.AddJsonFile($@"simplelogsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .Build();

            return new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .Enrich.FromLogContext()
                .CreateLogger();
        }
    }
}
