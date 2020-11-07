
namespace MangaDl
{
    class Chapter
    {
        public float ChapterNum { get; set; }
        public float VolumeNum { get; set; }

        private uint m_id = 0;
        public uint Id
        {
            get { return m_id; }
        }

        public string Url { get; set; }
        public string UrlPrefix { get; set; }
        public string MangaName { get; set; }
        public string ChapterName { get; set; }

        public string FullName => MangaName + "_" + ChapterName;
        public int PageCount { get; set; }

        private ParserBase m_parser;

        public Chapter(string url, MangaSite type, string mangaName)
        {
            Url = url;
            //TODO check for different sites, probably factory
            MangaName = mangaName;
            switch (type)
            { 
                case MangaSite.MANGAFOX:
                    m_parser = ParserMangaFox.GetInstance();
                    break;
                case MangaSite.MANGALIFE:
                    m_parser = ParserMangaLife.GetInstance();
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
