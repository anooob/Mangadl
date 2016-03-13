using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MangaDl
{
    class Favorites
    {
        private const string FavoritesFile = "favorites.txt";
        private List<FavoriteRow> FavoritesList = new List<FavoriteRow>();

        public Favorites()
        {
            LoadFavorites();
        }

        public IEnumerable<FavoriteRow> GetFavorites()
        {
            foreach (var item in FavoritesList)
            {
                yield return item;
            }
        }

        public void Add(MangaBase manga)
        {
            if (manga != null)
            {
                var row = new FavoriteRow() { Name = manga.ListName, Url = manga.Url };
                if (!FavoritesList.Contains(row))
                {
                    FavoritesList.Add(row);
                    SaveFavorites();
                }
            }
        }

        public void Remove(MangaBase manga)
        {
            if (manga != null)
            {
                var row = new FavoriteRow() { Name = manga.ListName, Url = manga.Url };
                if (FavoritesList.Contains(row))
                {
                    FavoritesList.Remove(row);
                    SaveFavorites();
                }
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
                        var serializer = new XmlSerializer(typeof(FavoriteEntity));
                        var entity = new FavoriteEntity();
                        entity.Rows = FavoritesList;
                        serializer.Serialize(writer, entity);
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
            var serializer = new XmlSerializer(typeof(FavoriteEntity));
            try
            {
                using (StreamReader reader = new StreamReader(FavoritesFile))
                {
                    var favorites = (FavoriteEntity)serializer.Deserialize(reader);
                    if (favorites != null)
                    {
                        FavoritesList = favorites.Rows;
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
