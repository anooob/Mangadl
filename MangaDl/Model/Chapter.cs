using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MangaDl
{
    class Chapter
    {
        private float m_chapterNum;
        public float ChapterNum
        {
            get { return m_chapterNum; }
            set { m_chapterNum = value; }
        }

        private float m_volumeNum;
        public float VolumeNum
        {
            get { return m_volumeNum; }
            set { m_volumeNum = value; }
        }

        private uint m_id = 0;
        public uint Id
        {
            get { return m_id; }
        }

        private string m_url;
        public string Url
        {
            get { return m_url; }
            set { m_url = value; }
        }

        private string m_urlPrefix;
        public string UrlPrefix
        {
            get { return m_urlPrefix; }
            set { m_urlPrefix = value; }
        }

        private string m_mangaName;
        public string MangaName
        {
            get { return m_mangaName; }
            set { m_mangaName = value; }
        }

        private string m_chapterName;
        public string ChapterName
        {
            get { return m_chapterName; }
            set { m_chapterName = value; }
        }

        public string FullName
        {
            get { return m_mangaName + "_" + m_chapterName; }
        }

        private int m_pageCount;
        public int PageCount
        {
            get { return m_pageCount; }
        }

        public Chapter(string url)
        {
            m_url = url;
            //TODO check for different sites
            ParserMangaFox.GetInstance().ParseUrl(this);
        }

        public bool GetPageCount()
        {
            HtmlDocument document = new HtmlDocument();
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
                m_pageCount = -1;
                return false;
            }

            var node = document.DocumentNode.SelectNodes("//select[@class='m']")[0];
            m_pageCount = (node.ChildNodes.Count - 5) / 2;

            return true;
        }
    }
}
