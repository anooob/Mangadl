using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MangaDl
{
    class Globals
    {
        private static uint m_chapterId = 0;
        public static uint ChapterId
        {
            get { return m_chapterId++; }
        }
    }
}
