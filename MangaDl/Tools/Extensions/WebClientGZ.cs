using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace MangaDl
{
    class WebClientGZ : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = (HttpWebRequest)base.GetWebRequest(address);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            return request;
        }
    }
}
