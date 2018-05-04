using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using RobotsTxt;
using System.Net;

namespace SingleDomainWebCrawler
{
    public class WebCrawler
    {

        public string root { get;  private set; }
        public string currentUrl { get; private set; }
        public string currentStatCode { get; private set; }
        public int maxDepth { get; private set; }
        public Int32 pagesCrawled { get; private set; }

        private HtmlWeb web;

        public Queue<string> currentDepthQueue { get; private set; } = new Queue<string>();
        public Queue<string> nextDepthQueue { get; private set; } = new Queue<string>();

        public List<string> visitedUrls { get; private set; } = new List<string>(); 

        public CSVOutput csvFile { get; private set; } = new CSVOutput("C:/Users/Jamie/Desktop");

        public WebCrawler()
        {

        }

        public WebCrawler(string root, int depth)
        {
            this.root = root;
            this.maxDepth = depth;
        }


        public HtmlDocument retrieveURL(string url)
        {
            currentUrl = url;
            web = new HtmlWeb();
            HtmlDocument doc = null;
            try
            {
                doc = web.Load(@"" + currentUrl);
            }
            catch (Exception)
            {
                doc = tryDomainPrefix(currentUrl);
                if (doc == null || doc.DocumentNode.ChildNodes.Count == 0)
                {
                    return null;
                }
                else
                {
                    visitedUrls.Add(url);
                    return doc;
                }
            }
            visitedUrls.Add(url);
            return doc;
        }

        public HtmlDocument tryDomainPrefix(string url)
        {
            currentUrl = root + url;
            try
            {
                HtmlDocument doc = web.Load(@"" + currentUrl);
                return doc;
            }
            catch (Exception)
            {
                return null;
            }
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
            csvFile.appendNewEntry(title, currentUrl, currentStatCode);
            pagesCrawled++;
        }

        public void endAndOutput()
        {
            csvFile.save();
            Console.WriteLine("\nCrawl completed at depth of " + maxDepth + ".");
            Console.WriteLine("CSV Output has been saved at " + csvFile.path);
            Console.WriteLine("Press any key to close.");
            Console.ReadLine();
        }


        public void performCrawl()
        {
            Console.WriteLine("Crawling...");
            addHeadersToOutput();
            HtmlDocument currentDoc = retrieveURL(root);
            if (verifyResponse(currentUrl, currentDoc))
            {
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
            else
            {
                Console.WriteLine("Not a valid URL. Press any key to exit.");
                Console.ReadLine();
            }
        }

        public void getUserInput()
        {
            Console.WriteLine("Please enter a URL to begin the crawl from.");
            root = Console.ReadLine();
            getMaxDepth();
            
        }

        public void getMaxDepth()
        {
            Console.WriteLine("What is the maximum depth to crawl to? (1 - 10)");
            try
            {
                int depth = Convert.ToInt32(Console.ReadLine());
                if(depth > 10 || depth < 1)
                {
                    Console.WriteLine("The depth must be between 1 and 10.");
                    getMaxDepth();
                }
                else
                {
                    maxDepth = depth;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("The depth has to be an integer.");
                getMaxDepth();
            }
        }

        /* nuget package isn't working, redo with regex
        public Robots loadRobots()
        {
            string robotsUri = @"" + root + "/robots.txt";
            string fileName = @"C:/Users/Jamie/Desktop/robots.txt";
            WebClient client = new WebClient();
            client.DownloadFile(robotsUri, fileName);
            client.UseDefaultCredentials = true;
            Robots robots = Robots.Load("C:/Users/Jamie/Desktop/robots.txt");
            return robots;
        }
        */

    }
}
