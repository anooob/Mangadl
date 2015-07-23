using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MangaDl
{
    abstract class ChapterDownloader
    {
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

        public abstract void UpdateStatus(Status status);
        public abstract void DownloadChapter();
        public abstract void ValidateChapter();
    }
}
