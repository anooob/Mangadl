using HtmlAgilityPack;
using System.Collections.Generic;

namespace MangaDl
{
    abstract class MangaBase
    {
        protected HtmlWeb m_web;

        protected List<ChapterDownloaderBase> m_chapters = new List<ChapterDownloaderBase>();
        public List<ChapterDownloaderBase> Chapters
        {
            get { return m_chapters; }
        }

        protected string m_url;
        public string Url
        {
            get { return m_url; }
            set { m_url = value; }
        }

        protected string m_name;
        public string Name
        {
            get { return m_name; }
        }

        public MangaBase(string url)
        {
            m_url = url;
            m_web = new HtmlWeb();
        }

        protected abstract void ParseUrl();
        public abstract void GetChapters();
    }
}
