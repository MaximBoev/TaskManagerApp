using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerApp.Utils
{
    public class TaskLogger
    {
        private static readonly string LogFilePath = "Logs/tasks.log";

        static TaskLogger()
        {
            Directory.CreateDirectory("Logs");
        }

        public static void Log(string message)
        {
            string logLine = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            File.AppendAllText(LogFilePath, logLine + Environment.NewLine);
        }
    }
}
