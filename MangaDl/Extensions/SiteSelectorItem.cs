
namespace MangaDl
{
    class SiteSelectorItem
    {
        private MangaSite m_site;
        public MangaSite Site
        {
            get { return m_site; }
        }

        private string m_name;

        public SiteSelectorItem(MangaSite site, string name)
        {
            m_site = site;
            m_name = name;
        }

        public override string ToString()
        {
            return m_name;
        }
    }
}
