using HtmlAgilityPack;
using System;
using System.IO;
using System.Linq;

namespace MangaDl
{
    class MangaMangaLife : MangaBase
    {
        private const string m_baseUrl = "http://manga.life";

        public override string ListName
        {
            get { return m_name + " - MangaLife"; }
        }

        public MangaMangaLife(string url)
            : base(url)
        {
            ParseUrl();
            Site = MangaSite.MANGALIFE;
            m_path = Path.Combine(Config.SavePath, "Mangalife", m_name);
        }

        protected override void ParseUrl()
        {
            var tokens = m_url.Split('/').ToList();
            tokens.RemoveAll(s => s == "");
            m_name = tokens.Last().Split('=').Last();
        }

        public override void GetChapters()
        {
            m_isGettingChapters = true;
            HtmlDocument document = new HtmlDocument(); ;
            try
            {
                using (var webClient = new WebClientGZ())
                {
                    var site = webClient.DownloadString(m_url);
                    document.LoadHtml(site);
                }
            }
            catch (Exception e)
            {
                Log.WriteLine(e.Message);
                Log.WriteLine(e.StackTrace);
                m_isGettingChapters = false;
            }
            try
            {
                var chapters = document.DocumentNode.SelectNodes("//a[@class=\"list-group-item hidden-lg hidden-md hidden-sm\"]");

                m_chapters.Clear();

                foreach (var c in chapters)
                {
                    if (c.OuterHtml != null && c.OuterHtml.Contains("dark_link"))
                    {
                        continue;
                    }
                    var url = c.Attributes["href"].Value.TrimStart('.');
                    m_chapters.Add(new ChapterDownloaderMangaLife(m_baseUrl + url, "", m_name, m_path));
                }
            }
            catch (Exception e)
            {
                Log.WriteLine(e.Message);
                Log.WriteLine(e.StackTrace);
            }
            finally
            {
                m_isGettingChapters = false;
            }
        }
    }
}
