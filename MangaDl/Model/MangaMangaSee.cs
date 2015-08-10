using HtmlAgilityPack;
using System;
using System.Linq;

namespace MangaDl
{
    class MangaMangaSee : MangaBase
    {
        private const string m_baseUrl = "http://mangasee.co";

        public MangaMangaSee(string url)
            : base(url)
        {
            ParseUrl();
        }
        protected override void ParseUrl()
        {
            var tokens = m_url.Split('/').ToList();
            tokens.RemoveAll(s => s == "");
            m_name = tokens.Last().Split('=').Last();
        }

        public override void GetChapters()
        {
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
                return;
            }
            try
            {
                var chapters = document.DocumentNode.SelectNodes("//a[@class=\"chapter_link\"]");

                m_chapters.Clear();

                foreach (var c in chapters)
                {
                    if (c.OuterHtml != null && c.OuterHtml.Contains("dark_link"))
                    {
                        continue;
                    }
                    var url = c.Attributes["href"].Value.TrimStart('.');
                    m_chapters.Add(new ChapterDownloaderMangaSee(m_baseUrl + url, ""));
                }
            }
            catch (Exception e)
            {
                Log.WriteLine(e.Message);
                Log.WriteLine(e.StackTrace);
                return;
            }
        }
    }
}
