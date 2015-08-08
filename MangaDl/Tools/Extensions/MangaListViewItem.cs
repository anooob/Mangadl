using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MangaDl
{
    class MangaListViewItem : ListViewItem
    {
        public MangaBase Manga;

        public MangaListViewItem(string[] cols)
            : base(cols)
        { }
    }
}
