using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace MangaDl
{
    abstract class ChapterDownloaderBase
    {
        protected string m_imgElementName;
        protected string m_chapterPath;
        protected HtmlWeb m_web;

        protected string m_mangaPath;
        public string MangaPath
        {
            get { return m_mangaPath; }
        }


        protected Chapter m_chapter;
        public Chapter Chapter
        {
            get { return m_chapter; }
        }

        protected uint m_id = 0;
        public uint Id
        {
            get { return m_id; }
        }

        public string ChapterName
        {
            get { return m_chapter.FullName; }
        }

        protected bool m_isDownloading = false;
        public bool IsDownloading
        {
            get { return m_isDownloading; }
        }

        protected bool m_isValidating = false;
        public bool IsValidating
        {
            get { return m_isValidating; }
        }

        protected List<ChapterListViewItem> m_items = new List<ChapterListViewItem>();
        public List<ChapterListViewItem> Items
        {
            get { return m_items; }
        }

        protected Status m_status = Status.READY;
        public Status Status
        {
            get { return m_status; }
        }

        protected int m_progress = 0;
        public int Progress
        {
            get { return m_progress; }
        }

        public Action<List<ChapterListViewItem>> RefreshItemCallback;

        public ChapterDownloaderBase(string url, string imgElementName)
        {
            m_id = Globals.ChapterId;
            m_imgElementName = imgElementName;
            m_web = new HtmlWeb();
        }

        protected void UpdateProgress(int progress)
        {
            m_progress = progress;

            if (RefreshItemCallback != null)
            {
                RefreshItemCallback(m_items);
            }
        }

        public void UpdateStatus(Status status)
        {
            m_status = status;

            if (RefreshItemCallback != null)
            {
                RefreshItemCallback(m_items);
            }
        }

        protected bool CheckImage(string imgUrl)
        {
            string imgName = imgUrl.Split('/').Last();
            string file = Path.Combine(m_chapterPath, imgName);
            if (File.Exists(file))
            {
                return true;
            }
            return false;
        }

        public void ValidateChapter()
        {
            try
            {
                UpdateProgress(0);
                UpdateStatus(Status.VALIDATING);
                m_isValidating = true;

                var chapterPath = Path.Combine(m_mangaPath, m_chapter.FullName);

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
                    CreatePageUrl(url, i);
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
                        UpdateStatus(Status.ERROR);
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

        protected bool DownloadImage(string url)
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

        public void DownloadChapter()
        {
            try
            {
                UpdateProgress(0);
                UpdateStatus(Status.DOWNLOADING);

                m_isDownloading = true;

                CreateDir();

                m_chapterPath = Path.Combine(m_mangaPath, m_chapter.FullName);

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
                    CreatePageUrl(url, i);
                    try
                    {
                        var site = webClient.DownloadString(url.ToString());
                        document.LoadHtml(site);
                    }
                    catch (Exception e)
                    {
                        Log.WriteLine(e.Message);
                        Log.WriteLine(e.StackTrace);
                        UpdateStatus(Status.ERROR);
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

        protected void CreateDir()
        {
            if (!Directory.Exists(m_mangaPath))
            {
                Directory.CreateDirectory(m_mangaPath);
            }
        }

        protected abstract string GetImageUrl(HtmlDocument document);
        protected abstract void CreatePageUrl(StringBuilder url, int pageNum);
        //public abstract void ValidateChapter();

    }
}
