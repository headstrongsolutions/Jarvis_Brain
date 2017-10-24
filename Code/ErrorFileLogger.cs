using System.Configuration;
using Jarvis_Brain.Models;
using System.IO;
using System;

namespace Jarvis_Brain.Code
{
    public class ErrorFileLogger
    {
        private string location;
        public string _location { get; set; }

        string filePath = ConfigurationManager.AppSettings.Get("ErrorLogFilePath");

        public ErrorFileLogger(string _location)
        {
            this.location = _location;
        }

        public void WriteToErrorLog(string message)
        {
            var errorLogString = ConvertErrorLogToString(new ErrorLog()
            {
                Location = location,
                DateTime = DateTime.Now,
                ExceptionMessage = message
            });

            if (ErrorLogFileExists())
            {
                WriteToExistingFile(errorLogString);
            }
            else
            {
                WriteToANewFile(errorLogString);
            }
            
        }

        private string ConvertErrorLogToString(ErrorLog errorLog)
        {
            return string.Format("STARTS==={3}{0}\t{1}{3}Exception:{3}{2}{3}ENDS==={3}", errorLog.Location, errorLog.DateTime, errorLog.ExceptionMessage, Environment.NewLine);
        }

        private bool ErrorLogFileExists()
        {
            var fileExists = File.Exists(filePath);
            return fileExists;
        }

        private void WriteToANewFile(string errorLogString)
        {
            System.IO.File.WriteAllText(filePath, errorLogString);
        }

        private void WriteToExistingFile(string errorLogString)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(filePath, true))
            {
                file.WriteLine(errorLogString);
            }
        }
    }
}
