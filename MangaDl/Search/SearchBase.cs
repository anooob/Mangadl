using System;
using System.Collections.Generic;

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

        protected void CallSearchResultsCallback(object sender, EventArgs e)
        {
            if (m_searchResultCallback != null)
            {
                m_searchResultCallback(m_searchResults);
            }
        }

        public abstract void GetSearchResults(string query);
        protected abstract void ParseResultDocument(object keyword);
    }
}
