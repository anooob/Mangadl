using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaDl
{
    public class FavoriteRow
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var row = obj as FavoriteRow;

            if (obj == null)
            { 
                return false;
            }
            if (row.Name != Name)
            {
                return false;
            }
            if (row.Url != Url)
            {
                return false;
            }
            return true;
        }
    }
}
