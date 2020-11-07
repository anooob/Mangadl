using HtmlAgilityPack;
using MangaDl.Model.Json.MangaLife;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MangaDl
{
    class ChapterDownloaderMangaLife : ChapterDownloaderBase
    {
        public ChapterDownloaderMangaLife(string url, string imgElementName, string mangaName, string mangaPath)
            : base(url, imgElementName)
        {
            m_chapter = new Chapter(url, MangaSite.MANGALIFE, mangaName);
            m_mangaPath = mangaPath;
        }

        protected override string GetImageUrl(HtmlDocument document)
        {
            var imgNode = document.DocumentNode.SelectNodes("//img[contains(@class, 'CurImage')]")[0];

            if (imgNode == null)
                return null;

            var imgUrl = imgNode.Attributes["src"].Value;
            return imgUrl;
        }

        protected override void CreatePageUrl(StringBuilder url, int pageNum)
        {
            url.Clear();
            url.Append(m_chapter.UrlPrefix).Append(pageNum).Append(".html");
        }

        protected override void DownloadImages(int pageCount)
        {
            HtmlDocument document = new HtmlDocument();
            using (var webClient = new WebClientGZ())
            {
                var site = webClient.DownloadString(m_chapter.Url);
                document.LoadHtml(site);
            }

            var ex = new Regex("vm.CurPathName = \"(?<siteName>.+)\";");
            var siteName = ex.Match(document.ParsedText).Groups["siteName"].Value;

            ex = new Regex("vm.CurChapter = (?<data>.+);");
            var rawData = ex.Match(document.ParsedText).Groups["data"].Value;

            var data = JsonConvert.DeserializeObject<ChapterJson>(rawData);

            pageCount = int.Parse(data.Page);

            int downloadedImages = 0;

            string chapterNum = data.Chapter.Substring(1, 4);

            for (int i = 1; i <= pageCount; i++)
            {
                string page = i.ToString().PadLeft(3, '0');
                var imgUrl = $"https://{siteName}/manga/{m_chapter.MangaName}/{chapterNum}-{page}.png";

                if (!Directory.Exists(m_chapterPath))
                {
                    throw new Exception("File not found.");
                }

                if (DownloadImage(imgUrl))
                {
                    downloadedImages++;
                }
                float progress = (float)downloadedImages / (float)pageCount * 100f;
                UpdateProgress((int)progress);
            }
        }
    }
}
