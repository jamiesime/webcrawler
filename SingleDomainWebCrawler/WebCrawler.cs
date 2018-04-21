using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleDomainWebCrawler
{
    class WebCrawler
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

    }
}
