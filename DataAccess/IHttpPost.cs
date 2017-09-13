﻿using DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IHttpPost
    {
        string HttpURL { get; set; }
        CookieContainer HttpCookies { get; set; }
        string HttpUserAgent { get; set; }
        Task<string> HttpPostAsync(string data, HttpDataServices httpDataServices);
        void HttpDataServices();
        Stream GetStreamForResponse(HttpWebResponse webResponse);
    }
}
