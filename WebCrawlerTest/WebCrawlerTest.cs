﻿using System;
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

        [TestMethod]
        public void addVisitedLinksTest()
        {
            //Arrange
            WebCrawler crawler = new WebCrawler("http://www.bbc.co.uk/news", 5);
            //Act
            HtmlDocument newDoc = crawler.retrievePage(crawler.root);
            //Assert
            Assert.AreEqual(1, crawler.visitedUrls.Count);
            Assert.AreEqual("http://www.bbc.co.uk/news", crawler.visitedUrls[0]);
        }

        [TestMethod]
        public void scrapeLinksToQueueTest()
        {
            //Arrange
            WebCrawler crawler = new WebCrawler("http://www.bbc.co.uk/news", 5);
            //Act
            HtmlDocument doc = crawler.retrievePage(crawler.root);
            crawler.scrapeLinksToQueue(crawler.currentDepthQueue, doc);
            //Assert
            Assert.IsTrue (crawler.currentDepthQueue.Count > 0);
            Assert.AreEqual("https://www.bbc.co.uk", crawler.currentDepthQueue.Peek());
        }

        [TestMethod]
        public void addInfoToOutputTest()
        {
            //Arrange
            WebCrawler crawler = new WebCrawler("http://www.bbc.co.uk/news", 5);
            //Act
            HtmlDocument doc = crawler.retrievePage(crawler.root);
            crawler.addInfoToOutput(doc);
            //Assert
            Assert.AreNotEqual("", crawler.csvFile.content.ToString());
        }
    }
}
