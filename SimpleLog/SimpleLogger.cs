using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Serilog;
using SimpleLog.Builders;

namespace SimpleLog
{
    public static class LogService
    {
        public static void StartSerilog()
        {
            try
            {
                Log.Logger = new SerilogLoggerBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .SetConfigFile("appsettings.json")
                    .Build();

                Log.Information("Serilog Logger built successfully");
            }
            catch (Exception ex)
            {
                throw new Exception($"Serilog Logger build failed: {ex.Message}");
            }

        }

        //public static IHostBuilder CreateHostBuilder()
        //{
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("simplelogsettings.json", optional: false, reloadOnChange: true);
        //    //.AddJsonFile($@"simplelogsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)

        //    var config = builder.Build();

        //    return Host.CreateDefaultBuilder()
        //        .UseSerilog((context, configuration) =>
        //        {
        //            configuration.ReadFrom.Configuration(config);
        //        });
        //}
    }
}
