using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MangaDl
{
    enum Status
    { 
        READY,
        DOWNLOADING,
        ERROR,
        VALIDATING,
        INCOMPLETE,
        WAITING,
        DEFAULT
    }

    enum MangaSite
    { 
        MANGAFOX,
        MANGASEE
    }

    static class Config
    {
        public static string ConfigFile = "config.txt";
        public static char Separator = '=';

        public static string SavePathStr = "path";
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

        public static string ThreadLimitStr = "threads";
        
        private static uint m_threadLimit = 3;
        public static uint ThreadLimit
        {
            get { return m_threadLimit; }
            set { m_threadLimit = value; }
        }

        public static void LoadConfig()
        {
            try
            {
                using (StreamReader reader = File.OpenText(Config.ConfigFile))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (line.Contains(Config.SavePathStr))
                        {
                            Config.SavePath = line.Split(Config.Separator).Last();
                        }
                        if (line.Contains(Config.ThreadLimitStr))
                        {
                            Config.ThreadLimit = uint.Parse(line.Split(Config.Separator).Last());
                        }
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
            }
        }

        public static void SaveConfig()
        {
            using (FileStream fs = new FileStream(Config.ConfigFile, FileMode.Truncate, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine(Config.SavePathStr + Config.Separator + Config.SavePath);
                    writer.WriteLine(Config.ThreadLimitStr + Config.Separator + Config.ThreadLimit.ToString());
                }
            }
        }
    }
}
