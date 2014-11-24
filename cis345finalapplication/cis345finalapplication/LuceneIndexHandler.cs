using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using org.apache.lucene.store;
using org.apache.lucene.analysis.standard;
using org.apache.lucene.index;
using org.apache.lucene.document;
using org.apache.lucene.search;
using org.apache.lucene.queryparser.classic;
using System.Collections.ObjectModel;

namespace CIS345FinalApplication
{
    public class LuceneIndexHandler
    {
        private Directory index;
        private StandardAnalyzer analyzer; //the analyzer is deprecated and not included...
        private IndexWriterConfig config;
        private IndexWriter indexWriter;
        private bool indexOpen = false;

        public LuceneIndexHandler()
        {
            index = new RAMDirectory();
            analyzer = new StandardAnalyzer(org.apache.lucene.util.Version.LATEST);
            config = new IndexWriterConfig(org.apache.lucene.util.Version.LATEST, analyzer);
            indexWriter = new IndexWriter(index, config);
            indexOpen = true;
        }

        public void AddDocument(string tag, string value, string filePath)
        {
            if (!indexOpen)
            {
                //throw new Exception("The IndexWriter has been closed. Documents can only be added before a query has been executed.");
            }
            Document doc = new Document();
            doc.add(new TextField("element", tag, Field.Store.YES));

            //Can use a string field for content if you don't want it tokenized.
            //We should evaluate if we need to worry about tokenization.
            doc.add(new TextField("content", value, Field.Store.YES));
            doc.add(new TextField("filepath", filePath, Field.Store.YES));
            indexWriter.addDocument(doc);
        }

        //TODO: Consider using an enumeration for the tags or something like that
        public List<SearchResult> SearchIndex(string tag, string value)
        {
            if (indexOpen)
            {
                indexWriter.close();
                indexOpen = false;
            }

            if (String.IsNullOrEmpty(tag) && String.IsNullOrEmpty(value))
            {
                return new List<SearchResult>();
            }

            List<SearchResult> results = new List<SearchResult>();
            String queryString = "";

            // If the tag name is empty, the user doesn't care about the tag, search all tags
            if (String.IsNullOrEmpty(tag))
            {
                // The query string should end up looking like this: (content:"theContent")
                // Fround at http://www.lucenetutorial.com/lucene-query-syntax.html
                queryString = "content:" + value;
            } 
            else
            {
                // The query string should end up looking like this: (element:"elementName" AND content:"theContent")
                // Fround at http://www.lucenetutorial.com/lucene-query-syntax.html
                queryString = "element:" + tag + " AND content:" + value;
            }
            
            // Prpare the query parser with the query string created above.
            //Query q = new QueryParser(org.apache.lucene.util.Version.LATEST, queryString.Substring(0, queryString.IndexOf(":") - 1), analyzer).parse(queryString);
            Query q = new QueryParser(org.apache.lucene.util.Version.LATEST, "element", analyzer).parse(queryString);

            int hitsPerPage = 10;
            IndexReader reader = DirectoryReader.open(index);
            IndexSearcher searcher = new IndexSearcher(reader);
            TopScoreDocCollector collector = TopScoreDocCollector.create(hitsPerPage, true);
            searcher.search(q, collector);
            ScoreDoc[] hits = collector.topDocs().scoreDocs;
	        
            //System.out.println("Found " + hits.length + " hits.");
            for(int i=0; i < hits.Length; i++)
            {
                int docId = hits[i].doc;
                Document d = searcher.doc(docId);
                string filepath = d.get("filepath");
                string element = d.get("element");
                string content = d.get("content");
                results.Add(new SearchResult() { Element = element, Content = content, Filepath = filepath });
            }
	      
            // reader can only be closed when there
            // is no need to access the documents any more.
            reader.close();

            return results;
        }
    }

    public class SearchResult
    {
        public string Filepath { get; set; }
        public string Element { get; set; }
        public string Content { get; set; }
    }
}
