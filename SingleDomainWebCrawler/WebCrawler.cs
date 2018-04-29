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
        public Queue<string> pageQueue = new Queue<string>();
        public List<string> visitedUrls = new List<string>();
        public CSVOutput csvFile = new CSVOutput("C:/");

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

        public void scrapeLinksToQueue(HtmlDocument doc)
        {
            foreach(HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                pageQueue.Enqueue(link.ToString());
            }
        }

    }
}
