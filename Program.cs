using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Jarvis_Brain
{
    /// <summary>
    /// Entry point for the application
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Raises the CreateWebHostBuilder method 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // This awesomeness allows incoming CLI parameters to add to the startup process
            // e.g. dotnet <applicationName>.dll --server.urls "http://*:5002"
            var config = new ConfigurationBuilder()
            .AddCommandLine(args)
            .Build();

            var host = new WebHostBuilder()
                .UseConfiguration(config)
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();
                host.Run();
        }
    }
}
