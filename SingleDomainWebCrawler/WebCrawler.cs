using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace SingleDomainWebCrawler
{
    public class WebCrawler
    {

        public string root;
        public int maxDepth;
        public Queue<Webpage> pageQueue;
        public List<string> visitedUrls;
        public CSVOutput csvFile;

        public WebCrawler(string root, int maxDepth)
        {
            this.root = root;
            this.maxDepth = maxDepth;
        }

        public HtmlDocument retrievePage(string url)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(@"" + url);
            return doc;
        }

        //public List<string> scrapeLinks(HtmlDocument doc)
        //{

        //}

    }
}
