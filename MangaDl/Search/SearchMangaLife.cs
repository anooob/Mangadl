using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;

namespace MangaDl
{
    class SearchDetail
    {
        public string i { get; set; }
        public string s { get; set; }
        public List<string> a { get; set; }
    }

    class SearchMangaLife : SearchBase
    {
        private const string m_baseUrl = "https://manga4life.com";

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

            using (var client = new HttpClient())
            {
                var response = client.PostAsync(m_baseUrl + "/_search.php", null).Result;
                var responseString = response.Content.ReadAsStringAsync().Result;

                var results = JsonConvert.DeserializeObject<List<SearchDetail>>(responseString);
                m_searchResults.Clear();

                if (results != null && results.Any())
                {
                    foreach (var result in results.Where(x => x.i.ToLower().Contains((string)keyword)))
                    {
                        var url = $"{m_baseUrl}/manga/{result.i}";
                        m_searchResults.Add(new MangaMangaLife(url));
                    }
                }
            }
        }
    }
}
