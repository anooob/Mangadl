using HtmlAgilityPack;
using System.Collections.Generic;

namespace MangaDl
{
    abstract class MangaBase
    {
        protected HtmlWeb m_web;

        protected MangaSite m_site;
        public MangaSite Site
        {
            get { return m_site; }
            set { m_site = value; }
        }

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

        protected bool m_isGettingChapters;
        public bool IsGettingChapters
        {
            get { return m_isGettingChapters; }
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
