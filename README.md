# webcrawler
Simple web crawler from single URL, using c# and html-agility pack

Simple console application that takes a url and a maximum depth. It loads that url, scrapes all the valid links on the page,
and then continues to do that for every page it linked to, and so on, in a breadth-first pattern until the maximum breadth is reached.

The title, Url and status code of each page is output to a CSV file at the end of the crawl. 
