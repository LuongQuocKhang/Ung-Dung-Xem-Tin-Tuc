﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NavigationDrawerPopUpMenu2
{
    interface ICrawl
    {
        string CrawlDataFromUrl(string Url, HttpClient httpclient);
    }
}