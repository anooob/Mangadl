using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace MangaDl
{
    class SearchMangaFox : SearchBase
    {
        public SearchMangaFox(Action<List<MangaBase>> searchResultCallBack)
            : base(searchResultCallBack)
        {
            m_searchPrefix = "http://mangafox.me/search.php?name_method=cw&name=";
            m_searchPostfix = "&advopts=1";
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

            try
            {
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
            catch (Exception e)
            {
                Log.WriteLine(e.Message);
                Log.WriteLine(e.StackTrace);
                return;
            }
        }
    }
}
