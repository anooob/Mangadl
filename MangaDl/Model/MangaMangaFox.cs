using HtmlAgilityPack;
using System;
using System.IO;
using System.Linq;

namespace MangaDl
{
    class MangaMangaFox : MangaBase
    {
        public override string ListName
        {
            get { return m_name + " - MangaFox"; }
        }

        public MangaMangaFox(string url)
            : base(url)
        {
            ParseUrl();
            m_site = MangaSite.MANGAFOX;
            m_path = Path.Combine(Config.SavePath, "Mangafox", m_name);
        }

        protected override void ParseUrl()
        {
            var tokens = m_url.Split('/').ToList();
            tokens.RemoveAll(s => s == "");
            m_name = tokens.Last();
        }

        public override void GetChapters(object param)
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
                return;
            }
            try
            {
                var chapterList = document.GetElementbyId("chapters");
                var chapters = chapterList.SelectNodes("//a[@class='tips']");

                m_chapters.Clear();

                foreach (var c in chapters)
                {
                    m_chapters.Add(new ChapterDownloaderMangaFox(c.Attributes["href"].Value, "image", m_name, m_path));
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
