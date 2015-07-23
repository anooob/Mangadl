using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MangaDl
{
    abstract class Manga
    {
        protected abstract void ParseUrl();
        public abstract void GetChapters();
    }
}
