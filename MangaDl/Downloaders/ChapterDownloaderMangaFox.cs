using HtmlAgilityPack;
using System.IO;
using System.Text;

namespace MangaDl
{
    class ChapterDownloaderMangaFox : ChapterDownloaderBase
    {
        public ChapterDownloaderMangaFox(string url, string imgElementName, string mangaName, string mangaPath)
            : base(url, imgElementName)
        {
            m_chapter = new Chapter(url, MangaSite.MANGAFOX, mangaName);
            m_mangaPath = mangaPath;
        }

        protected override string GetImageUrl(HtmlDocument document)
        {
            var imgNode = document.GetElementbyId(m_imgElementName);

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
