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
        private string m_imgElementName;
        private string m_chapterPath;
        private HtmlWeb m_web;

        public ChapterDownloaderMangaFox(string url, string imgElementName)
        {
            m_id = Globals.ChapterId;
            m_chapter = new Chapter(url);
            m_imgElementName = imgElementName;
            m_web = new HtmlWeb();
        }

        private string GetImageUrl(HtmlDocument document)
        {
            var imgNode = document.GetElementbyId(m_imgElementName);

            if (imgNode == null)
                return null;

            var imgUrl = imgNode.Attributes["src"].Value;
            return imgUrl;
        }

        private bool DownloadImage(string url)
        {
            string imgName = url.Split('/').Last();
            string file = Path.Combine(m_chapterPath, imgName);

            if (!File.Exists(file))
            {
                try
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
                    HttpWebResponse httpWebReponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    Stream stream = httpWebReponse.GetResponseStream();
                    var img = Image.FromStream(stream);
                    if (img.Width == 1 && img.Height == 1)
                        return false;
                    img.Save(file);
                }
                catch (Exception e)
                {
                    Log.WriteLine(e.Message);
                    Log.WriteLine(e.StackTrace);
                    return false;
                }
            }
            return true;
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

        private void UpdateProgress(int progress)
        {
            m_progress = progress;

            if (RefreshItemCallback != null)
            {
                RefreshItemCallback(m_items);
            }
        }

        public override void UpdateStatus(Status status)
        {
            m_status = status;

            if (RefreshItemCallback != null)
            {
                RefreshItemCallback(m_items);
            }
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

        public override void DownloadChapter()
        {
            try
            {
                UpdateProgress(0);
                UpdateStatus(Status.DOWNLOADING);

                m_isDownloading = true;
                var dir = Path.Combine(Config.SavePath, m_chapter.MangaName);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                m_chapterPath = Path.Combine(dir, m_chapter.FullName);

                if (m_chapterPath != null && !Directory.Exists(m_chapterPath))
                {
                    Directory.CreateDirectory(m_chapterPath);
                }
                var webClient = new WebClientGZ();
                var document = new HtmlDocument();
                StringBuilder url = new StringBuilder();

                if (m_chapter == null || !m_chapter.GetPageCount())
                {
                    UpdateStatus(Status.ERROR);
                    return;
                }

                int pageCount = m_chapter.PageCount;

                int downloadedImages = 0;
                for (int i = 1; i <= pageCount; i++)
                {
                    url.Clear();
                    url.Append(m_chapter.UrlPrefix).Append(i).Append(".html");

                    try
                    {
                        var site = webClient.DownloadString(url.ToString());
                        document.LoadHtml(site);
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
                UpdateStatus(Status.READY);
            }
            catch (Exception e)
            {
                Log.WriteLine(e.Message);
                Log.WriteLine(e.StackTrace);
                UpdateStatus(Status.ERROR);
                m_isDownloading = false;
            }
            finally
            {
                m_isDownloading = false;
            }
        }
    }
}
