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
        private Action<List<Manga>> m_searchResultCallback;
        private string m_searchUrl;
        private List<Manga> m_searchResults = new List<Manga>();


        public Search(Action<List<Manga>> searchResultCallBack)
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
            var web = new HtmlWeb();
            HtmlDocument document = null;
            try
            {
                document = web.Load(m_searchUrl);
            }
            catch (WebException e)
            {
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
                        m_searchResults.Add(new Manga(url));
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
                        m_searchResults.Add(new Manga(url));
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
            var t = new Thread(tw.Run);
            t.IsBackground = true;
            t.Start();
        }
    }
}
