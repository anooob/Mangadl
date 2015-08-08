using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MangaDl
{
    class ChapterListViewItem : ListViewItem
    {
        public ChapterDownloaderBase Chapter;
        private Status m_status;

        public void Refresh()
        {
            if (Chapter != null)
            {
                SubItems[1].Text = Chapter.Progress.ToString();
                if (m_status != Chapter.Status)
                {
                    m_status = Chapter.Status;
                    switch (m_status)
                    {
                        case Status.READY:
                            SubItems[2].Text = "Ready";
                            break;
                        case Status.DOWNLOADING:
                            SubItems[2].Text = "Downloading";
                            break;
                        case Status.ERROR:
                            SubItems[2].Text = "Error";
                            break;
                        case Status.VALIDATING:
                            SubItems[2].Text = "Validating";
                            break;
                        case Status.INCOMPLETE:
                            SubItems[2].Text = "Incomplete";
                            break;
                        case Status.WAITING:
                            SubItems[2].Text = "Waiting";
                            break;
                    }
                }
            }
        }

        public ChapterListViewItem(string[] cols)
            : base(cols)
        { }
    }
}
