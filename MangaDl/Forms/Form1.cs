using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HtmlAgilityPack;
using System.Net.Http;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace MangaDl
{
    public partial class Form1 : Form
    {
        private delegate void ChapterInfoDelegate(List<ChapterDownloader> list);
        private delegate void SearchResultDelegate(List<MangaMangaFox> list);
        private delegate void RefreshItemDelegate(List<ChapterListViewItem> items);

        private DownloadManagerMangaFox m_downloader;
        private Search m_search;

        private Dictionary<string, ChapterDownloaderMangaFox> m_chapters = new Dictionary<string,ChapterDownloaderMangaFox>();

        public Form1()
        {
            m_downloader = new DownloadManagerMangaFox(OnGetChapterInfoCompleted);

            InitializeComponent();
            dlpathTextbox.Text = Config.SavePath;

            chaptersListview.View = View.Details;
            chaptersListview.FullRowSelect = true;
            chaptersListview.Columns.Add("Name", 260);
            chaptersListview.Columns.Add("Progress", 60);
            chaptersListview.Columns.Add("Status", 100);

            queueListview.View = View.Details;
            queueListview.FullRowSelect = true;
            queueListview.Columns.Add("Name", 260);
            queueListview.Columns.Add("Progress", 60);
            queueListview.Columns.Add("Status", 100);

            searchListview.View = View.Details;
            searchListview.FullRowSelect = true;
            searchListview.MultiSelect = false;
            searchListview.Columns.Add("Name", 300);
            searchListview.Activation = ItemActivation.TwoClick;

            threadLimitTextBox.Text = Config.ThreadLimit.ToString();
            
            m_downloader = new DownloadManagerMangaFox(OnGetChapterInfoCompleted);
            m_search = new Search(OnSearchCompleted);
        }

        private void OnGetChapterInfoCompleted(List<ChapterDownloader> list)
        {
            if (list == null)
                return;

            if (chaptersListview.InvokeRequired)
            {
                ChapterInfoDelegate call = new ChapterInfoDelegate(OnGetChapterInfoCompleted);
                chaptersListview.Invoke(call, list);
            }
            else 
            {
                foreach (var c in list)
                {
                    //m_chapters.Add(c.Id, c);
                    var item = new ChapterListViewItem(new string[]{ c.ChapterName, "0", "Ready" });
                    item.Chapter = c;
                    c.Items.Add(item);
                    item.Refresh();
                    item.Name = c.Id.ToString();
                    c.RefreshItemCallback = OnRefreshItem;
                    chaptersListview.Items.Add(item);
                }
            }
        }

        private void OnRefreshItem(List<ChapterListViewItem> items)
        {
            if (chaptersListview.InvokeRequired)
            {
                RefreshItemDelegate call = new RefreshItemDelegate(OnRefreshItem);
                chaptersListview.Invoke(call, items);
            }
            else
            {
                foreach (var item in items)
                {
                    if (item != null)
                    {
                        item.Refresh();
                    }
                }
            }
        }

        private void OnSearchCompleted(List<MangaMangaFox> list)
        {
            if (list == null)
                return;

            if (searchListview.InvokeRequired)
            {
                SearchResultDelegate call = new SearchResultDelegate(OnSearchCompleted);
                searchListview.Invoke(call, list);
            }
            else
            {
                foreach (var m in list)
                {
                    var item = new MangaListViewItem(new string[] { m.Name });
                    item.Manga = m;
                    searchListview.Items.Add(item);
                }
            }
        }

        private void ClearChapterListView(ListView list)
        {
            foreach (ChapterListViewItem item in list.Items)
            {
                if (item != null)
                {
                    item.Chapter.Items.Remove(item);
                }
            }
            list.Items.Clear();
        }

        private void LoadChapters()
        {
            if (m_downloader != null && searchListview.SelectedItems.Count != 0)
            {
                var item = (searchListview.SelectedItems[0] as MangaListViewItem);
                if (item != null)
                {
                    ClearChapterListView(chaptersListview);
                    m_downloader.GetChapters(item.Manga);
                }
            }
        }

        private void loadButton_Click(object sender, EventArgs e)
        {
            LoadChapters();
        }

        private void searchListview_DoubleClick(object sender, EventArgs e)
        {
            if (searchListview.SelectedItems.Count == 1)
            {
                LoadChapters();
            }
        }
        
        private void selectFolderButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Config.SavePath = dialog.SelectedPath;
                dlpathTextbox.Text = dialog.SelectedPath;
                Config.SaveConfig();
                dialog.Dispose();
            }
        }

        private void AddChapterToQueue(ChapterDownloader chapter)
        {
            var item = new ChapterListViewItem(new string[] { chapter.ChapterName, "0", "Ready" });
            item.Chapter = chapter;
            chapter.Items.Add(item);
            item.Refresh();
            item.Name = chapter.Id.ToString();
            chapter.RefreshItemCallback = OnRefreshItem;
            queueListview.Items.Add(item);
        }

        private void DownloadSelected()
        {
            if (m_downloader != null)
            {
                var chaptersToDownload = new List<ChapterDownloader>();

                foreach (var c in chaptersListview.SelectedItems)
                {
                    var item = (c as ChapterListViewItem);
                    if (item != null)
                    {
                        chaptersToDownload.Add(item.Chapter);
                        AddChapterToQueue(item.Chapter);
                    }
                }
                m_downloader.DownloadSelectedChapters(chaptersToDownload);
            }
        }

        private void downloadButton_Click(object sender, EventArgs e)
        {
            DownloadSelected();
        }

        private void downloadAllButton_Click(object sender, EventArgs e)
        {
            if (m_downloader != null)
            {
                var chaptersToDownload = new List<ChapterDownloader>();

                foreach (var c in chaptersListview.Items)
                {
                    var item = (c as ChapterListViewItem);
                    if (item != null)
                    {
                        chaptersToDownload.Add(item.Chapter);
                        AddChapterToQueue(item.Chapter);
                    }
                }
                m_downloader.DownloadSelectedChapters(chaptersToDownload);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void SearchManga()
        {
            searchListview.Items.Clear();
            m_search.GetSearchResults(urlTextBox.Text);
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            SearchManga();
        }

        private void openFolderButton_Click(object sender, EventArgs e)
        {
            var dir = Config.SavePath;
            if (Directory.Exists(dir))
            {
                Process.Start(dir);
            }
        }

        private void urlTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                SearchManga();
                e.Handled = true;
            }
        }

        private void validateButton_Click(object sender, EventArgs e)
        {
            var chapters = new List<ChapterDownloader>();
            foreach (var c in chaptersListview.Items)
            {
                var item = (c as ChapterListViewItem);
                if (item != null)
                {
                    chapters.Add(item.Chapter);
                }
            }
            m_downloader.ValidateChapters(chapters);
        }

        private void threadLimitTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void threadLimitButton_Click(object sender, EventArgs e)
        {
            if (m_downloader != null && threadLimitTextBox.Text != "")
            {
                Config.ThreadLimit = uint.Parse(threadLimitTextBox.Text);
                Config.SaveConfig();
                m_downloader.RefreshQueue();
            }
        }

        private void abortAllButton_Click(object sender, EventArgs e)
        {
            if (m_downloader != null)
            {
                m_downloader.AbortDownload();
            }
        }
    }

    class ChapterListViewItem : ListViewItem
    {
        public ChapterDownloader Chapter;
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

    class MangaListViewItem : ListViewItem
    {
        public MangaMangaFox Manga;

        public MangaListViewItem(string[] cols)
            : base(cols)
        { }
    }
}
