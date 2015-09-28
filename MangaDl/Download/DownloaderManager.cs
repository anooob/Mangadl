using System;
using System.Collections.Generic;

namespace MangaDl
{    
    class DownloadManager
    {
        private Action<List<ChapterDownloaderBase>> m_getChaptersCallback;

        private ThreadWorker m_infoWorker;
        private List<ThreadWorker> m_workers = new List<ThreadWorker>();

        private Dictionary<string, ThreadWorker> m_downloadQueue = new Dictionary<string, ThreadWorker>();

        public DownloadManager(Action<List<ChapterDownloaderBase>> getChaptersCallback)
        {
            m_getChaptersCallback = getChaptersCallback;
        }

        public DownloadManager()
        {
        }

        private void CallGetChaptersCallback(object sender, EventArgs e)
        {
            if (m_getChaptersCallback != null && sender != null)
            {
                var manga = ((sender as ThreadWorker).Task.Target as MangaBase);
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
                m_workers.Remove(tw);

                var c = tw.Task.Target as ChapterDownloaderBase;
                if (c != null)
                {
                    m_downloadQueue.Remove(c.ChapterName);
                }
            }
            RefreshQueue();
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

        public void ValidateChapters(List<ChapterDownloaderBase> list)
        {
            foreach (var c in list)
            {
                if (c.IsDownloading || c.IsValidating)
                {
                    continue;
                }
                ThreadWorker tw = new ThreadWorker(c.ValidateChapter);
                tw.ThreadDone += WorkerFinishedCallback;
                m_workers.Add(tw);
                tw.Start();
            }
        }

        public void RefreshQueue()
        {
            foreach (var tw in m_downloadQueue)
            {
                var c = tw.Value.Task.Target as ChapterDownloaderBase;
                if (c == null || c.IsDownloading || c.IsValidating || m_workers.Count >= Config.ThreadLimit)
                {
                    continue;
                }

                m_workers.Add(tw.Value);
                tw.Value.Start();
            }
        }

        public void DownloadSelectedChapters(List<ChapterDownloaderBase> list)
        {
            foreach (var c in list)
            {
                if (m_downloadQueue.ContainsKey(c.ChapterName))
                {
                    continue;
                }
                c.UpdateStatus(Status.WAITING);
                ThreadWorker tw = new ThreadWorker(c.DownloadChapter);
                tw.ThreadDone += WorkerFinishedCallback;
                m_downloadQueue.Add(c.ChapterName, tw);
            }
            RefreshQueue();
        }

        public void GetChapters(MangaBase m)
        {
            if(m_infoWorker != null && m_infoWorker.IsAlive)
            {
                m_infoWorker.Abort();
            }
            var tw = new ThreadWorker(m.GetChapters);
            tw.ThreadDone += CallGetChaptersCallback;
            m_infoWorker = tw;
            tw.Start();
        }
    }
}
