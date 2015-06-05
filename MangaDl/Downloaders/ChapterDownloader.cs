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
    class ChapterDownloader
    {
        private string m_url;
        private float m_chapterNum;
        private float m_volumeNum;
        private string m_imgElementName;
        private string m_urlPrefix;
        private string m_chapterPath;
        private HtmlWeb m_web;

        private string m_mangaName;
        public string MangaName
        {
            get { return m_mangaName; }
        }

        public string FullName
        {
            get { return m_mangaName + "_" + m_chapterName; }
        }
        
        private List<ChapterListViewItem> m_items = new List<ChapterListViewItem>();
        public List<ChapterListViewItem> Items
        {
            get { return m_items; }
        }

        private uint m_id = 0;
        public uint Id
        {
            get { return m_id; }
        }

        private int m_progress = 0;
        public int Progress
        {
            get { return m_progress; }
        }

        private Status m_status = Status.READY;
        public Status Status
        {
            get { return m_status; }
        }

        private bool m_isDownloading = false;
        public bool IsDownloading
        {
            get { return m_isDownloading; }
        }

        private bool m_isValidating = false;
        public bool IsValidating
        {
            get { return m_isValidating; }
        }

        private int m_pageCount;
        public int PageCount
        {
            get { return m_pageCount; }
        }

        private string m_chapterName;
        public string Name
        {
            get { return m_chapterName; }
        }

        public Action<List<ChapterListViewItem>> RefreshItemCallback;

        public ChapterDownloader(string url, string imgElementName)
        {
            m_id = Globals.ChapterId;
            m_url = url;
            m_imgElementName = imgElementName;
            m_web = new HtmlWeb();
            ParseUrl();
        }

        private string GetChapterToken(string [] tokens)
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

        private string GetVolumeToken(string[] tokens)
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

        private void ParseUrl()
        {
            //TODO validate URL
            if (m_url == null || m_url == string.Empty)
                return;

            var tokens = m_url.Split('/');
            m_mangaName = tokens[4];

            var chapterToken = GetChapterToken(tokens);
            if(chapterToken != null)
            {
                var chapterNum = chapterToken.TrimStart(new char[] { 'c' });
                m_chapterNum = float.Parse(chapterNum, CultureInfo.InvariantCulture);

                var volumeToken = GetVolumeToken(tokens);
                if (volumeToken != null)
                {
                    var volumeNum = volumeToken.TrimStart(new char[] { 'v' });
                    m_volumeNum = float.Parse(volumeNum, CultureInfo.InvariantCulture);
                }
                else 
                {
                    m_volumeNum = -1;
                }
                m_urlPrefix = m_url.Split(new string[] { tokens.Last() }, StringSplitOptions.None)[0];

                if (m_volumeNum != -1)
                {
                    m_chapterName = "Vol_" + m_volumeNum + "_Chapter_" + m_chapterNum;
                }
                else 
                {
                    m_chapterName = "Chapter_" + m_chapterNum;
                }
            }
        }

        private bool GetPageCount()
        {
            HtmlDocument document = null;
            try
            {
                document = m_web.Load(m_url.ToString());
            }
            catch (WebException e)
            {
                return false;
            }

            var node = document.DocumentNode.SelectNodes("//select[@class='m']")[0];
            m_pageCount = (node.ChildNodes.Count - 5) / 2;

            return true;
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

        public void UpdateStatus(Status status)
        {
            m_status = status;

            if (RefreshItemCallback != null)
            {
                RefreshItemCallback(m_items);
            }
        }

        public void ValidateChapter()
        {
            try
            {
                UpdateProgress(0);
                UpdateStatus(Status.VALIDATING);
                m_isValidating = true;
                var dir = Path.Combine(Config.SavePath, m_mangaName);
                if (!Directory.Exists(dir))
                {
                    UpdateStatus(Status.INCOMPLETE);
                    return;
                }

                var chapterPath = Path.Combine(dir, m_chapterName);

                if (chapterPath != null && !Directory.Exists(chapterPath))
                {
                    UpdateStatus(Status.INCOMPLETE);
                    return;
                }

                HtmlDocument document;
                StringBuilder url = new StringBuilder();

                if (!GetPageCount())
                    return;

                int foundImages = 0;

                for (int i = 1; i <= m_pageCount; i++)
                {
                    url.Clear();
                    url.Append(m_urlPrefix).Append(i).Append(".html");

                    try
                    {
                        document = m_web.Load(url.ToString());
                    }
                    catch (WebException e)
                    {
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

                    float progress = (float)foundImages / (float)m_pageCount * 100f;
                    UpdateProgress((int)progress);
                }

                if (foundImages == m_pageCount)
                {
                    UpdateStatus(Status.READY);
                }
                else
                {
                    UpdateStatus(Status.INCOMPLETE);
                }

            }
            catch (Exception)
            {
                UpdateStatus(Status.ERROR);
                m_isValidating = false;
            }
            finally
            {
                m_isValidating = false;
            }
        }

        public void DownloadChapter()
        {
            try
            {
                UpdateProgress(0);
                UpdateStatus(Status.DOWNLOADING);

                m_isDownloading = true;
                var dir = Path.Combine(Config.SavePath, m_mangaName);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                m_chapterPath = Path.Combine(dir, m_chapterName);

                if (m_chapterPath != null && !Directory.Exists(m_chapterPath))
                {
                    Directory.CreateDirectory(m_chapterPath);
                }

                HtmlDocument document;
                StringBuilder url = new StringBuilder();

                if (!GetPageCount())
                {
                    UpdateStatus(Status.ERROR);
                    return;
                }

                int downloadedImages = 0;
                for (int i = 1; i <= m_pageCount; i++)
                {
                    url.Clear();
                    url.Append(m_urlPrefix).Append(i).Append(".html");

                    try
                    {
                        document = m_web.Load(url.ToString());
                    }
                    catch (WebException e)
                    {
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

                    float progress = (float)downloadedImages / (float)m_pageCount * 100f;
                    UpdateProgress((int)progress);
                }
                UpdateStatus(Status.READY);
            }
            catch (Exception)
            {
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
