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

        public HtmlWeb web;

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
            addHeadersToOutput();
            HtmlDocument currentDoc = retrieveURL(root);
            scrapeLinksToQueue(currentDepthQueue, currentDoc);
            addOKInfoToOutput(currentDoc);
            for (int i = 0; i < maxDepth; i++)
            {
                while (currentDepthQueue.Any())
                {
                    string url = currentDepthQueue.Dequeue();
                    if (!visitedUrls.Contains(url))
                    {
                            currentStatCode = null;
                            currentDoc = retrieveURL(url);
                            if (verifyResponse(url, currentDoc))
                            {
                                scrapeLinksToQueue(nextDepthQueue, currentDoc);
                                addOKInfoToOutput(currentDoc);
                                visitedUrls.Add(url);
                            }
                            else
                            {
                                addErrorInfoToOutput(url);
                            }
                    }
                    Console.Write("\r Pages crawled: " + pagesCrawled);
                }
                currentDepthQueue = nextDepthQueue;
                i++;
            }
            endAndOutput();
        }

        public HtmlDocument retrieveURL(string url)
        {
            currentUrl = url;
            web = new HtmlWeb();
            HtmlDocument doc = new HtmlDocument();
            try
            {
               doc = web.Load(@"" + currentUrl);
            }
            catch (Exception)
            {
                currentUrl = root + url;
                doc = web.Load(@"" + currentUrl);
            }
            if(doc.DocumentNode.ChildNodes.Count == 0 || doc == null)
            {
                return null;
            }
            return doc;
        }

        public bool verifyResponse(string url, HtmlDocument doc)
        {
            currentStatCode = web.StatusCode.ToString();
            if(currentStatCode == "OK" && doc != null)
            {
                return true;
            }
            return false;
           
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

        public void addHeadersToOutput()
        {
            csvFile.appendNewEntry("Title", "URL", "Status Code");
        }

        public void addErrorInfoToOutput(string url)
        {
            csvFile.appendNewEntry("Title unknown", url, currentStatCode);
        }

        public void addOKInfoToOutput(HtmlDocument doc)
        {
            string title = doc.DocumentNode.SelectSingleNode("//title").InnerText.ToString();
            csvFile.appendNewEntry($" \"{title}\" ", currentUrl, currentStatCode);
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
