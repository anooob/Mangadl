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
            set { m_pageCount = value; }
        }

        private ParserBase m_parser;

        public Chapter(string url, MangaSite type)
        {
            m_url = url;
            //TODO check for different sites, probably factory

            switch (type)
            { 
                case MangaSite.MANGAFOX:
                    m_parser = ParserMangaFox.GetInstance();
                    break;
                case MangaSite.MANGASEE:
                    m_parser = ParserMangaSee.GetInstance();
                    break;
            }
            if (m_parser != null)
            {
                m_parser.ParseUrl(this);
            }
        }

        public bool GetPageCount()
        {
            return m_parser.GetPageCount(this);
        }
    }
}
