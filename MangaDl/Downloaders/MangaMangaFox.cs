using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace MangaDl
{
    class MangaMangaFox : Manga
    {
        private string m_name;
        public string Name
        {
            get { return m_name; }
        }
        private HtmlWeb m_web;

        private List<ChapterDownloader> m_chapters = new List<ChapterDownloader>();
        public List<ChapterDownloader> Chapters
        {
            get { return m_chapters; }
        }

        private string m_url;
        public string Url
        {
            get { return m_url; }
            set { m_url = value; }
        }

        public MangaMangaFox()
        { }

        public MangaMangaFox(string url)
        {
            m_web = new HtmlWeb();
            m_url = url;
            ParseUrl();
        }

        private override void ParseUrl()
        {
            var tokens = m_url.Split('/').ToList();
            tokens.RemoveAll(s => s == "");
            m_name = tokens.Last();
        }

        public override void GetChapters()
        {
            HtmlDocument document = null;
            try
            {
                document = m_web.Load(m_url.ToString());
            }
            catch (Exception e)
            {
                return;
            }
            try
            {

                var chapterList = document.GetElementbyId("chapters");
                var chapters = chapterList.SelectNodes("//a[@class='tips']");

                foreach (var c in chapters)
                {
                    m_chapters.Add(new ChapterDownloaderMangaFox(c.Attributes["href"].Value, "image"));
                }
            }
            catch (Exception e)
            {
                return;
            }
        }
    }
}
