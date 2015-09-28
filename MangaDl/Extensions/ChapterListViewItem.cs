using System.Globalization;
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
                    var status = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(m_status.ToString().ToLower());
                    SubItems[2].Text = status;
                }
            }
        }

        public ChapterListViewItem(string[] cols)
            : base(cols)
        { }
    }
}
