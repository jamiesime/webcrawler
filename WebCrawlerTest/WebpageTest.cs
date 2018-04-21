using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SingleDomainWebCrawler;

namespace WebCrawlerTest
{
    [TestClass]
    public class WebpageTest
    {
        [TestMethod]
        public void getPropertiesTest()
        {
            //Arrange
            Webpage testPage = new Webpage("http://www.bbc.co.uk/news", "Home - BBC News", "OK", true);
            //Assert
            Assert.AreEqual("http://www.bbc.co.uk/news", testPage.url);
            Assert.AreEqual("OK", testPage.statusCode);
            Assert.AreEqual("Home - BBC News", testPage.title);
            Assert.AreEqual(true, testPage.visited);
        }

    }
}
