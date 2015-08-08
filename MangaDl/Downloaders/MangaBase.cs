using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MangaDl
{
    abstract class MangaBase
    {
        protected string m_name;
        public string Name
        {
            get { return m_name; }
        }

        protected abstract void ParseUrl();
        public abstract void GetChapters();
    }
}
