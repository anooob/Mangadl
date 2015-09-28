using HtmlAgilityPack;
using System;
using System.Globalization;
using System.Linq;

namespace MangaDl
{
    class ParserMangaLife : ParserBase
    {
        private static ParserMangaLife m_instance;

        public static ParserMangaLife GetInstance()
        {
            if (m_instance == null)
            {
                lock (m_locker)
                {
                    if (m_instance == null)
                    {
                        m_instance = new ParserMangaLife();
                    }
                }
            }
            return m_instance;
        }

        private ParserMangaLife()
        { 
        }

        protected override string GetChapterToken(string[] tokens)
        {
            foreach (var s in tokens)
            {
                if (s.StartsWith("chapter"))
                {
                    return s.Split('-').Last();
                }
            }
            return null;
        }

        protected override string GetVolumeToken(string[] tokens)
        {
            return null;
        }

        private string GetSeriesName(string[] tokens)
        {
            int index = -1;

            for (int i = 0; i < tokens.Count(); i++)
            {
                if (tokens[i].Contains("read-online"))
                {
                    index = i + 1;
                    break;
                }
            }

            if (index != -1)
            {
                return tokens[index];
            }

            return null;
        }

        public override void ParseUrl(Chapter chapter)
        {
            //http://manga.life/read-online/Berserk/chapter-1/index-2/page-1
            //TODO validate URL
            if (chapter.Url == null || chapter.Url == string.Empty)
                return;

            var tokens = chapter.Url.Split('/');

            var seriesName = GetSeriesName(tokens);
            if (seriesName != null && string.IsNullOrEmpty(chapter.MangaName))
            {
                chapter.MangaName = seriesName;
            }

            var chapterToken = GetChapterToken(tokens);
            if(chapterToken != null)
            {
                chapter.ChapterNum = float.Parse(chapterToken, CultureInfo.InvariantCulture);
            }
            chapter.UrlPrefix = chapter.Url.TrimEnd(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });

            string format = "0000.##";

            chapter.ChapterName = "c_" + chapter.ChapterNum.ToString(format);
        }

        public override bool GetPageCount(Chapter chapter)
        {
            HtmlDocument document = new HtmlDocument();
            try
            {
                using (var webClient = new WebClientGZ())
                {
                    var site = webClient.DownloadString(chapter.Url);
                    document.LoadHtml(site);
                }
            }
            catch (Exception e)
            {
                Log.WriteLine(e.Message);
                Log.WriteLine(e.StackTrace);
                chapter.PageCount = -1;
                return false;
            }

            var node = document.DocumentNode.SelectNodes("//select[@class='changePageSelect']")[0];
            var pages = node.SelectNodes("*");
            
            chapter.PageCount = pages.Count;

            return true;
        }
    }
}
