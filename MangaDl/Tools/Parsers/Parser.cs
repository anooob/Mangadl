using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MangaDl
{
    abstract class Parser
    {
       // protected static object m_locker = new object();

        protected abstract string GetChapterToken(string[] tokens);
        protected abstract string GetVolumeToken(string[] tokens);
        public abstract void ParseUrl(Chapter chapter);
        public abstract bool GetPageCount(Chapter chapter);
    }
}
