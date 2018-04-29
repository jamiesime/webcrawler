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
            WebCrawler crawler = new WebCrawler("http://www.bbc.co.uk/news", 5);
            //Assert
            Assert.AreEqual("http://www.bbc.co.uk/news", crawler.root);
            Assert.AreEqual(5, crawler.maxDepth);
        }

        [TestMethod]
        public void retrievePageTest()
        {
            //Arrange
            WebCrawler crawler = new WebCrawler("http://www.bbc.co.uk/news", 5);
            //Act
            HtmlDocument newDoc = crawler.retrievePage(crawler.root);
            //Assert
            Assert.AreEqual("Home - BBC News", newDoc.DocumentNode.SelectSingleNode("//title").InnerText.ToString());
        }
    }
}
