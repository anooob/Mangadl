using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MangaDl
{
    enum Status
    {
        READY,
        DOWNLOADING,
        ERROR,
        VALIDATING,
        INCOMPLETE,
        WAITING,
        DEFAULT
    }

    enum MangaSite
    {
        MANGAFOX,
        MANGASEE
    }
}
