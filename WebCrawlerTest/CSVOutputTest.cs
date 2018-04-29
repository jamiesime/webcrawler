using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SingleDomainWebCrawler;

namespace WebCrawlerTest
{
    [TestClass]
    public class CSVOutputTest
    {
        [TestMethod]
        public void getPropertiesTest()
        {
            //Arrange
            CSVOutput newCSV = new CSVOutput("C:/Users/Jamie/Desktop/testCSV.csv");
            //Assert
            Assert.AreEqual("C:/Users/Jamie/Desktop/testCSV.csv", newCSV.path);
        }

        [TestMethod]
        public void appendEntryTest()
        {
            //Arrange
            CSVOutput CSV = new CSVOutput("C:/Users/Jamie/Desktop/testCSV.csv");
            //Act
            CSV.appendNewEntry("URL", "Title", "Status Code");
            //Assert
            Assert.AreEqual("URL,Title,Status Code\r\n", CSV.content.ToString());
        }
    }
}
