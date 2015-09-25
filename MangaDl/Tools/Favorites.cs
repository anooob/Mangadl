using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MangaDl
{
    class Favorites
    {
        private const string FavoritesFile = "favorites.txt";
        private const char separator = ';';
        private Dictionary<string, string> FavoritesList = new Dictionary<string,string>();

        public Favorites()
        {
            LoadFavorites();
        }

        public void AddFavorite(string name, string url)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(url))
            {
                FavoritesList.Add(name, url);
                SaveFavorites();
            }
        }

        public void SaveFavorites() 
        {
            try
            {
                using (FileStream fs = new FileStream(FavoritesFile, FileMode.Truncate, FileAccess.Write))
                {
                    using (StreamWriter writer = new StreamWriter(fs))
                    {
                        foreach (var item in FavoritesList)
                        {
                            writer.WriteLine(item.Key + separator + item.Value);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException)
                {
                    var file = File.Create(FavoritesFile);
                    file.Close();
                }
            }
        }

        public void LoadFavorites() 
        {
            FavoritesList.Clear();
            try
            {
                using (StreamReader reader = File.OpenText(FavoritesFile))
                {
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        var tokens = line.Split(separator);
                        if (tokens.Count() < 2)
                        {
                            continue;
                        }
                        FavoritesList.Add(tokens[0], tokens[1]);
                    }
                }
            }
            catch (Exception e)
            {
                if (e is FileNotFoundException)
                {
                    var file = File.Create(FavoritesFile);
                    file.Close();
                    SaveFavorites();
                }
            }
        }
    }
}
