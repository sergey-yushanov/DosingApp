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
        readonly string logsFilePath = string.Empty;

        public LogUtils(string folderPath, string logsFolder)
        {
            string logsFolderPath = Path.Combine(folderPath, logsFolder);
            if (!File.Exists(logsFolderPath))
            {
                Directory.CreateDirectory(logsFolderPath);
            }

            logsFilePath = Path.Combine(logsFolderPath, "log.txt");
            if (!File.Exists(logsFilePath))
            {
                using (StreamWriter writer = new StreamWriter(logsFilePath, true))
                {
                    writer.WriteLine("Starting logging at " + DateTime.Now.ToString());
                }
            }
        }

        public void Log(string message)
        {
            using (StreamWriter writer = new StreamWriter(logsFilePath, true))
            {
                writer.WriteLine(DateTime.Now.ToString() + " : " + message);
            }
        }
    }
}
