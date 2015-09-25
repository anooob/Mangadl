using HtmlAgilityPack;
using System.IO;
using System.Text;

namespace MangaDl
{
    class ChapterDownloaderMangaSee : ChapterDownloaderBase
    {
        public ChapterDownloaderMangaSee(string url, string imgElementName)
            : base(url, imgElementName)
        {
            m_chapter = new Chapter(url, MangaSite.MANGASEE);
        }

        protected override string GetImageUrl(HtmlDocument document)
        {
            var imgNode = document.DocumentNode.SelectNodes("//img")[0];

            if (imgNode == null)
                return null;

            var imgUrl = imgNode.Attributes["src"].Value;
            return imgUrl;
        }

        protected override string CreateDir() 
        {
            var dir = Path.Combine(Config.SavePath, "Mangasee", m_chapter.MangaName);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }

        protected override void CreatePageUrl(StringBuilder url, int pageNum)
        {
            url.Clear();
            url.Append(m_chapter.UrlPrefix).Append(pageNum);
        }
    }
}
