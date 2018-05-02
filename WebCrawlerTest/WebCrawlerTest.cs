using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SingleDomainWebCrawler;
using HtmlAgilityPack;

namespace WebCrawlerTest
{
    [TestClass]
    public class WebCrawlerTest
    {

        [TestMethod]
        public void getPropertiesTest()
        {
            //Arrange
            WebCrawler crawler = new WebCrawler("http://www.bbc.co.uk/news", 2);
            //Assert
            Assert.AreEqual("http://www.bbc.co.uk/news", crawler.root);
            Assert.AreEqual(2, crawler.maxDepth);
        }

        [TestMethod]
        public void retrievePageTest()
        {
            //Arrange
            WebCrawler crawler = new WebCrawler("http://www.bbc.co.uk/news", 2);
            //Act
            HtmlDocument newDoc = crawler.retrieveURL(crawler.root);
            //Assert
            Assert.AreEqual("Home - BBC News", newDoc.DocumentNode.SelectSingleNode("//title").InnerText.ToString());
        }

        [TestMethod]
        public void addVisitedLinksTest()
        {
            //Arrange
            WebCrawler crawler = new WebCrawler("http://www.bbc.co.uk/news", 2);
            //Act
            HtmlDocument newDoc = crawler.retrieveURL(crawler.root);
            //Assert
            Assert.AreEqual(1, crawler.visitedUrls.Count);
            Assert.AreEqual("http://www.bbc.co.uk/news", crawler.visitedUrls[0]);
        }

        [TestMethod]
        public void scrapeLinksToQueueTest()
        {
            //Arrange
            WebCrawler crawler = new WebCrawler("http://www.bbc.co.uk/news", 2);
            //Act
            HtmlDocument doc = crawler.retrieveURL(crawler.root);
            crawler.scrapeLinksToQueue(crawler.currentDepthQueue, doc);
            //Assert
            Assert.IsTrue (crawler.currentDepthQueue.Count > 0);
            Assert.AreEqual("https://www.bbc.co.uk", crawler.currentDepthQueue.Peek());
        }

        [TestMethod]
        public void addOKInfoToOutputTest()
        {
            //Arrange
            WebCrawler crawler = new WebCrawler("http://www.bbc.co.uk/news", 2);
            //Act
            HtmlDocument doc = crawler.retrieveURL(crawler.root);
            crawler.addOKInfoToOutput(doc);
            //Assert
            Assert.AreNotEqual("", crawler.csvFile.content.ToString());
        }

        [TestMethod]
        public void addErrorInfoToOutputTest()
        {
            //Arrange
            WebCrawler crawler = new WebCrawler("nonsense", 2);
            //Act
            HtmlDocument doc = crawler.retrieveURL(crawler.root);
            //Assert
            Assert.AreEqual(null, doc);

        }

        [TestMethod]
        public void verifyResponseTest()
        {
            //Arrange
            WebCrawler crawler = new WebCrawler("http://www.bbc.co.uk/news", 2);
            //Act
            HtmlDocument doc = crawler.retrieveURL(crawler.root);
            //Assert
            Assert.AreEqual(true, crawler.verifyResponse(crawler.root, doc));
        }

        [TestMethod]
        public void tryDomainPrefixTest()
        {
            //Arrange
            WebCrawler crawler = new WebCrawler("http://www.bbc.co.uk/", 2);
            //Act
            HtmlDocument doc = crawler.retrieveURL("news");
            //Assert
            Assert.AreEqual(true, crawler.verifyResponse(crawler.root, doc));
        }
    }
}
