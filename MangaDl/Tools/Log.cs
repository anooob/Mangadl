using System;
using System.IO;

namespace MangaDl
{
    class Log
    {
        private static string m_currentLogFile = null;
        private static string m_logLocation = ".\\Logs";

        private static object m_locker = new object();

        private static int GetLogNumber()
        {
            if (!Directory.Exists(m_logLocation))
            {
                Directory.CreateDirectory(m_logLocation);
            }
            return Directory.GetFiles(m_logLocation).Length + 1;
        }

        private static void CreateLogFile()
        {
            m_currentLogFile = Path.Combine(m_logLocation, "Log" + GetLogNumber() + ".txt");
            if (File.Exists(m_currentLogFile))
            {
                File.Delete(m_currentLogFile);
            }
            var file = File.Create(m_currentLogFile);
            file.Close();
        }

        public static void WriteLine(string msg)
        {
            lock (m_locker)
            {
                if (m_currentLogFile == null)
                {
                    CreateLogFile();
                }
                using (FileStream fs = new FileStream(m_currentLogFile, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        var time = DateTime.Now;
                        writer.WriteLine(time.ToString() + ": " + msg);
                    }
                }
            }
        }
    }
}
