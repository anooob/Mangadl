using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace MangaDl
{
    class ThreadWorker
    {
        public event EventHandler ThreadDone;
        private Action m_task;
        public Action Task
        {
            get { return m_task; }
        }
        public Thread Thread;

        public ThreadWorker(Action task)
        {
            m_task = task;
        }

        public void Run()
        {
            try
            {
                if (m_task != null)
                {
                    m_task();
                }
                if (ThreadDone != null)
                {
                    ThreadDone(this, EventArgs.Empty);
                }
            }
            catch (ThreadAbortException e)
            {
                //ThreadDone(this, EventArgs.Empty);
            }
        }
    }

    class DownloadManagerMangaFox : Downloader
    {
        const string m_imgElementName = "image";
        private string m_url;

        private Action<List<ChapterDownloader>> m_getChaptersCallback;

        private Thread m_infoWorker;
        private List<Thread> m_workers = new List<Thread>();

        public string Url
        {
            get { return m_url; }
            set { m_url = value; }
        }

        public DownloadManagerMangaFox(Action<List<ChapterDownloader>> getChaptersCallback)
        {
            m_getChaptersCallback = getChaptersCallback;
        }

        public DownloadManagerMangaFox()
        {
        }

        private void CallGetChaptersCallback(object sender, EventArgs e)
        {
            if (m_getChaptersCallback != null && sender != null)
            {
                var manga = ((sender as ThreadWorker).Task.Target as Manga);
                if (manga != null)
                {
                    m_getChaptersCallback(manga.Chapters);
                }
            }
        }

        private void WorkerFinishedCallback(object sender, EventArgs e)
        {
            var tw = (sender as ThreadWorker);
            if (tw != null)
            {
                if (tw.Thread != null)
                {
                    m_workers.Remove(tw.Thread);
                }
            }
        }

        public void AbortDownload()
        {
            foreach (var w in m_workers)
            {
                if (w.IsAlive)
                {
                    w.Abort();
                }
            }
            m_workers.Clear();
        }

        public void ValidateChapters(List<ChapterDownloader> list)
        {
            foreach (var c in list)
            {
                if (c.IsDownloading || c.IsValidating)
                {
                    continue;
                }
                ThreadWorker tw = new ThreadWorker(c.ValidateChapter);
                tw.ThreadDone += WorkerFinishedCallback;
                var t = new Thread(tw.Run);
                tw.Thread = t;
                m_workers.Add(t);
                t.IsBackground = true;
                t.Start();
            }
        }

        public void DownloadSelectedChapters(List<ChapterDownloader> list)
        {
            foreach (var c in list)
            {
                if (c.IsDownloading || c.IsValidating)
                {
                    continue;
                }
                ThreadWorker tw = new ThreadWorker(c.DownloadChapter);
                tw.ThreadDone += WorkerFinishedCallback;
                var t = new Thread(tw.Run);
                tw.Thread = t;
                m_workers.Add(t);
                t.IsBackground = true;
                t.Start();
            }
        }

        public void GetChapters()
        {
            var m = new Manga(m_url);

            if(m_infoWorker != null && m_infoWorker.IsAlive)
            {
                m_infoWorker.Abort();
            }

            ThreadWorker tw = new ThreadWorker(m.GetChapters);
            tw.ThreadDone += CallGetChaptersCallback;
            m_infoWorker = new Thread(tw.Run);
            m_infoWorker.IsBackground = true;
            m_infoWorker.Start();
        }
    }
}
