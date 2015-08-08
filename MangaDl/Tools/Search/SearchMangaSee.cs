using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MangaDl
{
    class SearchMangaSee : SearchBase
    {
        private const string m_baseUrl = "http://mangasee.co";

        public SearchMangaSee(Action<List<MangaBase>> searchResultCallBack)
            : base(searchResultCallBack)
        {
            m_searchPrefix = m_baseUrl + "/advanced-search/result.php?seriesName=";
        }

        protected override void ParseResultDocument()
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

            var results = document.DocumentNode.SelectNodes("//h1");

            if (results != null)
            {
                for (int i = 1; i < results.Count - 2; i++)
                {
                    var link = results[i].FirstChild.Attributes["href"].Value.TrimStart('.');
                    if (link != null)
                    {
                        var url = m_baseUrl + link;
                        m_searchResults.Add(new MangaMangaSee(url));
                    }
                }
            }
        }
    }
}
