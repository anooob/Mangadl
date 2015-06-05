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
        private delegate void SearchResultDelegate(List<Manga> list);
        private delegate void UpdateProgressDelegate(ChapterListViewItem chapterItem, int progress);
        private delegate void UpdateStatusDelegate(ChapterListViewItem chapterItem, Status status);

        private DownloadManagerMangaFox m_downloader;
        private Search m_search;

        public Form1()
        {
            InitializeComponent();
            dlpathTextbox.Text = Config.SavePath;

            chaptersListview.View = View.Details;
            chaptersListview.FullRowSelect = true;
            chaptersListview.Columns.Add("Name", 150);
            chaptersListview.Columns.Add("Progress", 55);
            chaptersListview.Columns.Add("Status", 95);

            queueListview.View = View.Details;
            queueListview.FullRowSelect = true;
            queueListview.Columns.Add("Name", 150);
            queueListview.Columns.Add("Progress", 55);
            queueListview.Columns.Add("Status", 95);

            searchListview.View = View.Details;
            searchListview.FullRowSelect = true;
            searchListview.MultiSelect = false;
            searchListview.Columns.Add("Name", 300);
            searchListview.Activation = ItemActivation.TwoClick;
            
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
                    var item = new ChapterListViewItem(new string[]{ c.Name, "0", "def" });
                    item.Status = Status.READY;
                    item.Chapter = c;
                    c.ListItem = item;
                    item.UpdateProgressCallback = OnUpdateProgress;
                    item.UpdateStatusCallback = OnUpdateStatus;
                    chaptersListview.Items.Add(item);
                }
            }
        }

        private void OnUpdateProgress(ChapterListViewItem chapterItem, int progress)
        {
            if (chapterItem == null)
                return;
            if (chaptersListview.InvokeRequired)
            {
                UpdateProgressDelegate call = new UpdateProgressDelegate(OnUpdateProgress);
                chaptersListview.Invoke(call, chapterItem, progress);
            }
            else 
            {
                chapterItem.SubItems[1].Text = progress.ToString();   
            }
        }

        private void OnUpdateStatus(ChapterListViewItem item, Status status)
        {
            if (item == null)
            {
                return;
            }
            if (chaptersListview.InvokeRequired)
            {
                UpdateStatusDelegate call = new UpdateStatusDelegate(OnUpdateStatus);
                chaptersListview.Invoke(call, item, status);
            }
            else 
            {
                item.Status = status;
            }
        }

        private void OnSearchCompleted(List<Manga> list)
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

        private void LoadChapters()
        {
            if (m_downloader != null && searchListview.SelectedItems.Count != 0)
            {
                var item = (searchListview.SelectedItems[0] as MangaListViewItem);
                if (item != null)
                {
                    chaptersListview.Items.Clear();
                    m_downloader.AbortDownload();
                    m_downloader.Url = item.Manga.Url;
                    m_downloader.GetChapters();
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
                Program.SaveConfig();
                dialog.Dispose();
            }
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
                    }
                }
                m_downloader.DownloadSelectedChapters(chaptersToDownload);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void abortAllButton_Click(object sender, EventArgs e)
        {
            if (m_downloader != null)
            {
                m_downloader.AbortDownload();
            }
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
    }

    class ChapterListViewItem : ListViewItem
    {
        public ChapterDownloader Chapter;

        public Action<ChapterListViewItem, int> UpdateProgressCallback;
        public Action<ChapterListViewItem, Status> UpdateStatusCallback;

        private Status m_status = Status.DEFAULT;
        public Status Status
        {
            get { return m_status; }
            set 
            {
                if (m_status != value)
                {
                    m_status = value;
                    switch (value)
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
        public Manga Manga;

        public MangaListViewItem(string[] cols)
            : base(cols)
        { }
    }
}
