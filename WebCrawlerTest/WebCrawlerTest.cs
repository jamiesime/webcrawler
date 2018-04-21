using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SingleDomainWebCrawler;

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
    }
}
