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
            var pageString = tokens.Last().Split('.').First();
            var pageTokens = pageString.Split('-');

            for (int i = 0; i < pageTokens.Count(); i++)
            {
                if (pageTokens[i] == "chapter")
                {
                    return pageTokens[i + 1];
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
            //rip
            return null;
        }

        private string GetIndexToken(string[] tokens)
        {
            var pageString = tokens.Last().Split('.').First();
            var pageTokens = pageString.Split('-');

            for (int i = 0; i < pageTokens.Count(); i++)
            {
                if (pageTokens[i] == "index")
                {
                    return pageTokens[i + 1];
                }
            }

            return null;
        }

        public override void ParseUrl(Chapter chapter)
        {
            http://mangalife.org/read-online/Berserk-chapter-19-index-2-page-1.html
            //TODO validate URL
            if (chapter.Url == null || chapter.Url == string.Empty)
                return;

            var tokens = chapter.Url.Split('/');

            var chapterToken = GetChapterToken(tokens);
            if (!string.IsNullOrEmpty(chapterToken))
            {
                chapter.ChapterNum = float.Parse(chapterToken, CultureInfo.InvariantCulture);
            }

            var index = GetIndexToken(tokens);

            chapter.UrlPrefix = chapter.Url.Split(new string[] { ".html" }, StringSplitOptions.None).First().TrimEnd(new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });

            string format = "0000.##";

            if (string.IsNullOrEmpty(index))
            {
                chapter.ChapterName = "c_" + chapter.ChapterNum.ToString(format);
            }
            else
            {
                chapter.ChapterName = "c_" + chapter.ChapterNum.ToString(format) + "_i_" + index;
            }
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

            var node = document.DocumentNode.SelectNodes("//select[contains(@class, 'PageSelect')]").First();
            var pages = node.SelectNodes("*");

            chapter.PageCount = pages.Count;

            return true;
        }
    }
}
