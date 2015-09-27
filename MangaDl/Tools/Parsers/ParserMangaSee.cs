using HtmlAgilityPack;
using System;
using System.Globalization;
using System.Linq;

namespace MangaDl
{
    class ParserMangaSee : ParserBase
    {
        private static ParserMangaSee m_instance;

        public static ParserMangaSee GetInstance()
        {
            if (m_instance == null)
            {
                lock (m_locker)
                {
                    if (m_instance == null)
                    {
                        m_instance = new ParserMangaSee();
                    }
                }
            }
            return m_instance;
        }

        private ParserMangaSee()
        { 
        }

        protected override string GetChapterToken(string[] tokens)
        {
            foreach (var s in tokens)
            {
                if (s.StartsWith("chapter"))
                {
                    return s.Split('=').Last();
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
            foreach (var s in tokens)
            { 
                if (s.StartsWith("series"))
                {
                    return s.Split('=').Last();
                }
            }
            return null;
        }

        public override void ParseUrl(Chapter chapter)
        {
            //TODO validate URL
            if (chapter.Url == null || chapter.Url == string.Empty)
                return;

            var urlParams = chapter.Url.Split('?').Last();
            var tokens = urlParams.Split('&');

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

            var node = document.DocumentNode.SelectNodes("//select[@class='form-control selectWidth' and @name='page']")[0];
            var pages = node.SelectNodes("*");
            
            chapter.PageCount = pages.Count;

            return true;
        }
    }
}
