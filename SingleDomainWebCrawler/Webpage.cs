using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleDomainWebCrawler
{
    public class Webpage
    {
        public string url;
        public string title;
        public string statusCode;
        public bool visited;

        public Webpage(string url, string title, string statusCode, bool visited) {
            this.url = url;
            this.title = title;
            this.statusCode = statusCode;
            this.visited = visited;
        }
    }
}
