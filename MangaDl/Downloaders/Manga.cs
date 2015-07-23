using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MangaDl
{
    abstract class Manga
    {
        protected abstract void ParseUrl();
        protected abstract void GetChapters();
    }
}
