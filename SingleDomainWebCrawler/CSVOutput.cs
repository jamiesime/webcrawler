using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleDomainWebCrawler
{
    public class CSVOutput
    {
        public string path;
        public StringBuilder content = new StringBuilder();

        public CSVOutput(string path)
        {
            this.path = path;
        }

        public void appendNewEntry(string url, string title, string statusCode)
        {
            content.AppendLine(url + "," + title + "," + statusCode);
        }
    }
}
