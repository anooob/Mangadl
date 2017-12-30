using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MangaDl
{
    class SearchMangaLife : SearchBase
    {
        private const string m_baseUrl = "http://mangalife.us";

        public SearchMangaLife(Action<List<MangaBase>> searchResultCallBack)
            : base(searchResultCallBack)
        {
            m_searchPrefix = m_baseUrl + "/search/?keyword=";
        }

        public override void GetSearchResults(string query)
        {
            var q = query.Replace(' ', '+').ToLower();
            m_searchUrl = m_baseUrl + "/directory";
            m_searchResults.Clear();

            var tw = new ThreadWorker(ParseResultDocument, query);
            tw.ThreadDone += CallSearchResultsCallback;
            tw.Start();
        }

        protected override void ParseResultDocument(object keyword)
        {
            if (!(keyword is string key))
            {
                return;
            }

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

            var results = document.DocumentNode.SelectNodes(@"//p[@class='seriesList chapOnly']/a[@class='ttip']");

            m_searchResults.Clear();

            if (results != null)
            {
                var res = results.Nodes().Where(x => x.InnerText.ToLower().Contains(key));
                foreach (var item in res)
                {
                    var link = item.ParentNode.Attributes["href"].Value.TrimStart('.');
                    if (link != null)
                    {
                        var url = m_baseUrl + link;
                        m_searchResults.Add(new MangaMangaLife(url));
                    }
                }
            }
        }
    }
}
