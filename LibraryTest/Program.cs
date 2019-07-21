using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;

namespace LibraryTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args).ConfigureLogging((context, logging) =>
            {
                logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                // Сначала необходимо создать лог в windows: 
                // new-eventlog -source Libtest -LogName LibtestLog
                logging.AddEventLog(new EventLogSettings()
                {
                    SourceName = "Libtest",
                    LogName = "LibtestLog",
                    Filter = (x, y) => y >= LogLevel.Warning
                });
            }).UseStartup<Startup>();
    }
}
