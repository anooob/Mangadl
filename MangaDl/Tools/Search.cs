using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace MangaDl
{
    class Search
    {
        private const string m_searchPrefix = "http://mangafox.me/search.php?name_method=cw&name=";
        private const string m_searchPostfix = "&advopts=1";
        private Action<List<MangaMangaFox>> m_searchResultCallback;
        private string m_searchUrl;
        private List<MangaMangaFox> m_searchResults = new List<MangaMangaFox>();


        public Search(Action<List<MangaMangaFox>> searchResultCallBack)
        {
            m_searchResultCallback = searchResultCallBack;
        }

        private void CallSearchResultsCallback(object sender, EventArgs e)
        {
            if (m_searchResultCallback != null)
            {
                m_searchResultCallback(m_searchResults);
            }
        }

        private void ParseDocument()
        {
            HtmlDocument document = new HtmlDocument();
            try
            {
                using (var webClient = new WebClientGZ())
                {
                    var site = webClient.DownloadString(m_searchUrl);
                    document.LoadHtml(site);
                }
            }
            catch (Exception e)
            {
                Log.WriteLine(e.Message);
                Log.WriteLine(e.StackTrace);
                return;
            }
            var table = document.GetElementbyId("listing");
            var resultsClosed = table.SelectNodes("//a[@class='series_preview manga_close']");
            var resultsOpen = table.SelectNodes("//a[@class='series_preview manga_open']");

            if (resultsClosed != null)
            {
                foreach (var r in resultsClosed)
                {
                    var url = r.Attributes["href"].Value;
                    if (url != null)
                    {
                        m_searchResults.Add(new MangaMangaFox(url));
                    }
                }
            }

            if (resultsOpen != null)
            {
                foreach (var r in resultsOpen)
                {
                    var url = r.Attributes["href"].Value;
                    if (url != null)
                    {
                        m_searchResults.Add(new MangaMangaFox(url));
                    }
                }
            }
        }

        public void GetSearchResults(string query)
        {
            var q = query.Replace(' ', '+').ToLower();
            m_searchUrl = m_searchPrefix + q + m_searchPostfix;
            m_searchResults.Clear();

            var tw = new ThreadWorker(ParseDocument);
            tw.ThreadDone += CallSearchResultsCallback;
            tw.Start();
        }
    }
}
