
namespace MangaDl
{
    abstract class ParserBase
    {
        protected static object m_locker = new object();

        protected abstract string GetChapterToken(string[] tokens);
        protected abstract string GetVolumeToken(string[] tokens);
        public abstract void ParseUrl(Chapter chapter);
        public abstract bool GetPageCount(Chapter chapter);
    }
}
