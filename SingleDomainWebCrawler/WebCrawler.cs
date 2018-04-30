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
        public string currentUrl;
        public string currentStatCode;
        public int maxDepth;
        public Int32 pagesCrawled;

        public Queue<string> currentDepthQueue = new Queue<string>();
        public Queue<string> nextDepthQueue = new Queue<string>();

        public List<string> visitedUrls = new List<string>();

        public CSVOutput csvFile = new CSVOutput("C:/Users/Jamie/Desktop");

        public WebCrawler(string root, int maxDepth)
        {
            this.root = root;
            this.maxDepth = maxDepth;
        }

        public void performCrawl()
        {
            Console.WriteLine("Crawling...");
            HtmlDocument currentDoc = retrievePage(root);
            scrapeLinksToQueue(currentDepthQueue, currentDoc);
            addInfoToOutput(currentDoc);
            for (int i = 0; i < maxDepth; i++)
            {
                while (currentDepthQueue.Any())
                {
                    string url = currentDepthQueue.Dequeue();
                    if (!visitedUrls.Contains(url))
                    {
                        try
                        {
                            currentDoc = retrievePage(url);
                            if (currentStatCode == "OK")
                            {
                                scrapeLinksToQueue(nextDepthQueue, currentDoc);
                                addInfoToOutput(currentDoc);
                            }
                        }
                        catch(Exception)
                        {
                            continue;
                        }
                    }
                    Console.Write("\r{0}% Pages crawled: " + pagesCrawled);
                }
                foreach(string link in nextDepthQueue)
                {
                    currentDepthQueue.Enqueue(link);
                }
                i++;
            }
            endAndOutput();
        }

        public HtmlDocument retrievePage(string url)
        {
            currentUrl = url;
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load(@"" + url);
            currentStatCode = web.StatusCode.ToString();
            visitedUrls.Add(url);
            return doc;
        }

        public void scrapeLinksToQueue(Queue<string> thisQueue, HtmlDocument doc)
        {
            var linkNodes = doc.DocumentNode.SelectNodes("//a[@href]");
            if (linkNodes != null)
            {
                foreach (HtmlNode link in linkNodes)
                {
                    thisQueue.Enqueue(link.Attributes["href"].Value.ToString());
                }
            }
        }

        public void addInfoToOutput(HtmlDocument doc)
        {
            string title = doc.DocumentNode.SelectSingleNode("//title").ToString();
            csvFile.appendNewEntry(title, currentUrl, currentStatCode);
            pagesCrawled++;
        }

        public void endAndOutput()
        {
            csvFile.save();
            Console.WriteLine("Crawl completed at depth of " + maxDepth + ".");
            Console.WriteLine("CSV Output has been saved at " + csvFile.path);
            Console.WriteLine("Press any key to close.");
            Console.ReadLine();
        }

    }
}
