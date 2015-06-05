using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MangaDl
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        //[STAThread]
        private static void LoadConfig()
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
                    }
                }
            }
            catch (Exception e)
            {
                File.Create(Config.ConfigFile);
            }
        }

        public static void SaveConfig()
        {
            using (FileStream fs = new FileStream(Config.ConfigFile, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine(Config.SavePathStr + Config.Separator + Config.SavePath);
                }
            }
        }

        [STAThread]
        static void Main()
        {
            LoadConfig();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
