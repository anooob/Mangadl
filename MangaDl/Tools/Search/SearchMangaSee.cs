using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MangaDl.Tools.Search
{
    class SearchMangaSee : SearchBase
    {
        public SearchMangaSee(Action<List<MangaBase>> searchResultCallBack)
            : base(searchResultCallBack)
        {
            m_searchPrefix = "http://mangasee.co/advanced-search/result.php?seriesName=";
        }

        protected override void ParseResultDocument()
        {



        }
    }
}
