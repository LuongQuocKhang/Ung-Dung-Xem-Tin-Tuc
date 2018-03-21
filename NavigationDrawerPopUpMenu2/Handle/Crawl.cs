using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NavigationDrawerPopUpMenu2
{
    public class Crawl : ICrawl
    {
        private static volatile Crawl instance;

        internal static Crawl Instance
        {
            get
            {
                if (instance == null) instance = new Crawl();
                    return instance;
            }
        }

        public string CrawlDataFromUrl(string Url, HttpClient httpclient)
        {
            return WebUtility.HtmlDecode(httpclient.GetStringAsync(Url).Result);
        }
    }
}
