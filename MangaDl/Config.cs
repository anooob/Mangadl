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
    }
}
