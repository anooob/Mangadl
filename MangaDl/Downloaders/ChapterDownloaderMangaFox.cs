using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace MangaDl
{
    class ChapterDownloaderMangaFox : ChapterDownloaderBase
    {
        public ChapterDownloaderMangaFox(string url, string imgElementName)
            : base(url, imgElementName)
        {
            m_chapter = new Chapter(url, MangaSite.MANGAFOX);
        }

        protected override string GetImageUrl(HtmlDocument document)
        {
            var imgNode = document.GetElementbyId(m_imgElementName);

            if (imgNode == null)
                return null;

            var imgUrl = imgNode.Attributes["src"].Value;
            return imgUrl;
        }


        private bool CheckImage(string imgUrl)
        {
            string imgName = imgUrl.Split('/').Last();
            string file = Path.Combine(m_chapterPath, imgName);
            if (File.Exists(file))
            {
                return true;
            }
            return false;
        }

        protected override string CreateDir()
        {
            var dir = Path.Combine(Config.SavePath, "Mangafox", m_chapter.MangaName);

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return dir;
        }

        protected override void CreatePageUrl(StringBuilder url, int pageNum)
        {
            url.Clear();
            url.Append(m_chapter.UrlPrefix).Append(pageNum).Append(".html");
        }

        public override void ValidateChapter()
        {
            try
            {
                UpdateProgress(0);
                UpdateStatus(Status.VALIDATING);
                m_isValidating = true;
                var dir = Path.Combine(Config.SavePath, m_chapter.MangaName);
                if (!Directory.Exists(dir))
                {
                    UpdateStatus(Status.INCOMPLETE);
                    return;
                }

                var chapterPath = Path.Combine(dir, m_chapter.FullName);

                if (chapterPath != null && !Directory.Exists(chapterPath))
                {
                    UpdateStatus(Status.INCOMPLETE);
                    return;
                }

                HtmlDocument document = new HtmlDocument();
                StringBuilder url = new StringBuilder();

                if (m_chapter == null || !m_chapter.GetPageCount())
                    return;

                int pageCount = m_chapter.PageCount;

                int foundImages = 0;

                for (int i = 1; i <= pageCount; i++)
                {
                    url.Clear();
                    url.Append(m_chapter.UrlPrefix).Append(i).Append(".html");

                    try
                    {
                        using (var webClient = new WebClientGZ())
                        {
                            var site = webClient.DownloadString(url.ToString());
                            document.LoadHtml(site);
                        }
                    }
                    catch (Exception e)
                    {
                        Log.WriteLine(e.Message);
                        Log.WriteLine(e.StackTrace);
                        return;
                    }
                    var imgUrl = GetImageUrl(document);
                    if (imgUrl == null)
                    {
                        return;
                    }

                    if (!Directory.Exists(chapterPath))
                    {
                        throw new Exception("File not found.");
                    }

                    string imgName = imgUrl.Split('/').Last();
                    string file = Path.Combine(chapterPath, imgName);
                    if (File.Exists(file))
                    {
                        foundImages++;

                    }

                    float progress = (float)foundImages / (float)pageCount * 100f;
                    UpdateProgress((int)progress);
                }

                if (foundImages == pageCount)
                {
                    UpdateStatus(Status.READY);
                }
                else
                {
                    UpdateStatus(Status.INCOMPLETE);
                }

            }
            catch (Exception e)
            {
                Log.WriteLine(e.Message);
                Log.WriteLine(e.StackTrace);
                UpdateStatus(Status.ERROR);
                m_isValidating = false;
            }
            finally
            {
                m_isValidating = false;
            }
        }
    }
}
