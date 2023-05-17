using DosingApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace DosingApp.Utils
{
    public class LogUtils
    {
        string logFilePath = string.Empty;

        public LogUtils(string path)
        {
            logFilePath = Path.Combine(path, "log.txt");
            if (!File.Exists(logFilePath))
            {
                using (StreamWriter writer = new StreamWriter(logFilePath, true))
                {
                    writer.WriteLine("Starting logging at " + DateTime.Now.ToString());
                }
            }
        }

        public void Log(string message)
        {
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine(DateTime.Now.ToString() + " : " + message);
            }
        }
    }
}
