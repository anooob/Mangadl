using HtmlAgilityPack;
using MangaDl;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MangaDl
{
    class ParserMangaFox : Parser
    {
        private static ParserMangaFox m_instance;

        public static ParserMangaFox GetInstance()
        {
            if (m_instance == null)
            {
                lock (m_locker)
                {
                    if (m_instance == null)
                    {
                        m_instance = new ParserMangaFox();
                    }
                }
            }
            return m_instance;
        }

        private ParserMangaFox()
        { 
        }

        protected override string GetChapterToken(string[] tokens)
        {
            var regex = new Regex("c{1}\\d\\d\\d");
            foreach (var s in tokens)
            {
                if (regex.IsMatch(s))
                {
                    return s;
                }
            }
            return null;
        }

        protected override string GetVolumeToken(string[] tokens)
        {
            var regex = new Regex("v{1}\\d\\d");
            foreach (var s in tokens)
            {
                if (regex.IsMatch(s))
                {
                    return s;
                }
            }
            return null;
        }

        public override void ParseUrl(Chapter chapter)
        {
            //TODO validate URL
            if (chapter.Url == null || chapter.Url == string.Empty)
                return;

            var tokens = chapter.Url.Split('/');
            chapter.MangaName = tokens[4];

            var chapterToken = GetChapterToken(tokens);
            if (chapterToken != null)
            {
                var chapterNum = chapterToken.TrimStart(new char[] { 'c' });
                chapter.ChapterNum = float.Parse(chapterNum, CultureInfo.InvariantCulture);

                var volumeToken = GetVolumeToken(tokens);
                if (volumeToken != null)
                {
                    var volumeNum = volumeToken.TrimStart(new char[] { 'v' });
                    chapter.VolumeNum = float.Parse(volumeNum, CultureInfo.InvariantCulture);
                }
                else
                {
                    chapter.VolumeNum = -1;
                }
                chapter.UrlPrefix = chapter.Url.Split(new string[] { tokens.Last() }, StringSplitOptions.None)[0];

                string format = "0000.##";

                if (chapter.VolumeNum != -1)
                {
                    chapter.ChapterName = "v_" + chapter.VolumeNum.ToString("000") + "_c_" + chapter.ChapterNum.ToString(format);
                }
                else
                {
                    chapter.ChapterName = "c_" + chapter.ChapterNum.ToString(format);
                }
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

            var node = document.DocumentNode.SelectNodes("//select[@class='m']")[0];
            chapter.PageCount = (node.ChildNodes.Count - 5) / 2;

            return true;
        }
    }
}
