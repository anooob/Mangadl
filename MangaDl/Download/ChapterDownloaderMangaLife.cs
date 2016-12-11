using HtmlAgilityPack;
using System.IO;
using System.Text;

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
    }
}
