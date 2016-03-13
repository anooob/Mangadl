using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace MangaDl
{
    static class Config
    {
        public const string ConfigFile = "config.txt";

        private static string DefaultPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private static string m_savePath = DefaultPath;
        public static string SavePath
        {
            get { return m_savePath; }
            set 
            {
                if (Directory.Exists(value))
                {
                    m_savePath = value;
                }
                else
                {
                    m_savePath = DefaultPath;
                }
            }
        }
        
        private static uint m_threadLimit = 3;
        public static uint ThreadLimit
        {
            get { return m_threadLimit; }
            set { m_threadLimit = value; }
        }

        public static void LoadConfig()
        {
            var serializer = new XmlSerializer(typeof(ConfigEntity));
            try
            {
                using (StreamReader reader = new StreamReader(Config.ConfigFile))
                {
                    var config = (ConfigEntity)serializer.Deserialize(reader);
                    if (config != null)
                    {
                        m_savePath = config.Path;
                        m_threadLimit = config.Threads;
                    }
                }
            }
            catch (Exception e)
            {
                if (e is FormatException)
                {
                    Config.ThreadLimit = 3;
                }
                else if (e is FileNotFoundException)
                {
                    var file = File.Create(Config.ConfigFile);
                    file.Close();
                    SaveConfig();
                }
                else
                {
                    Log.WriteLine(e.Message);
                    Log.WriteLine(e.StackTrace);
                }
            }
        }

        public static void SaveConfig()
        {
            var serializer = new XmlSerializer(typeof(ConfigEntity));
            var entity = new ConfigEntity();

            entity.Path = m_savePath;
            entity.Threads = m_threadLimit;
            using (FileStream fs = new FileStream(Config.ConfigFile, FileMode.Truncate, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    serializer.Serialize(writer, entity);
                }
            }
        }
    }
}
