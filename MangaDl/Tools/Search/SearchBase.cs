using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MangaDl
{
    abstract class SearchBase
    {
        protected Action<List<MangaBase>> m_searchResultCallback;
        protected List<MangaBase> m_searchResults = new List<MangaBase>();
        protected string m_searchUrl;

        protected string m_searchPrefix = "";
        protected string m_searchPostfix = "";

        public SearchBase(Action<List<MangaBase>> searchResultCallBack)
        {
            m_searchResultCallback = searchResultCallBack;
        }

        public void GetSearchResults(string query)
        {
            var q = query.Replace(' ', '+').ToLower();
            m_searchUrl = m_searchPrefix + q;
            m_searchResults.Clear();

            var tw = new ThreadWorker(ParseResultDocument);
            tw.ThreadDone += CallSearchResultsCallback;
            tw.Start();
        }

        protected void CallSearchResultsCallback(object sender, EventArgs e)
        {
            if (m_searchResultCallback != null)
            {
                m_searchResultCallback(m_searchResults);
            }
        }

        protected abstract void ParseResultDocument();
    }
}
