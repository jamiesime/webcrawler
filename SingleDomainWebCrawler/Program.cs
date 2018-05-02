using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace SingleDomainWebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            WebCrawler crawler = new WebCrawler("https://www.sfdebris.com/", 1);
            Console.WriteLine("Enter any key to begin crawl.");
            crawler.performCrawl();
        }
    }
}
