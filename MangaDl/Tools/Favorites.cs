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

        public IEnumerable<KeyValuePair<string, string>> GetFavorites()
        {
            foreach (var item in FavoritesList)
            {
                yield return item;
            }
        }

        public void Add(MangaBase manga)
        {
            if (manga != null && !FavoritesList.Contains(new KeyValuePair<string,string>(manga.ListName, manga.Url)))
            {
                FavoritesList.Add(manga.ListName, manga.Url);
                SaveFavorites();
            }
        }

        public void Remove(MangaBase manga)
        {
            if (manga != null && FavoritesList.Contains(new KeyValuePair<string, string>(manga.ListName, manga.Url)))
            {
                FavoritesList.Remove(manga.ListName);
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
                        if (tokens.Count() < 2 || FavoritesList.Contains(new KeyValuePair<string,string>(tokens[0], tokens[1])))
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
