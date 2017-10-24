using System;
using System.Net.Http;
using Microsoft.Owin.Hosting;
using System.Configuration;
using Jarvis_Brain.Code;

namespace Jarvis_Brain
{
    class Program
    {
        static void Main()
        {
            ErrorFileLogger errorLog = new ErrorFileLogger("Main()");
            try
            {
                string baseAddress = ConfigurationManager.AppSettings.Get("BaseAddress");
                if (!string.IsNullOrEmpty(baseAddress))
                {
                    using (WebApp.Start<Startup>(url: baseAddress))
                    {
                        Console.WriteLine("Server Started:");
                        Console.WriteLine("Press Enter to quit.");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Base Address is null or empty, check App.Config");
                    Console.WriteLine("Press Enter to quit.");
                    throw new System.Exception("Base Address is null or empty, check App.Config");
                }
            }
            catch(Exception ex)
            {
                errorLog.WriteToErrorLog(ex.Message);
            }
        }
    }
}
