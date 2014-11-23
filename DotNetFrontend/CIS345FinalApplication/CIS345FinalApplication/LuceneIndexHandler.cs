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

        public void AddDocument(string tag, string value)
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
            indexWriter.addDocument(doc);
        }

        //TODO: Consider using an enumeration for the tags or something like that
        public List<string> SearchIndex(string tag, string value)
        {
            if (indexOpen)
            {
                indexWriter.close();
                indexOpen = false;
            }

            if (String.IsNullOrEmpty(tag) && String.IsNullOrEmpty(value))
            {
                return new List<string>();
            }

            List<string> results = new List<string>();

            Query q = new QueryParser(org.apache.lucene.util.Version.LATEST, "element", analyzer).parse(tag);

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
                results.Add(d.get("element") + ": " + d.get("content"));
                //System.out.println((i + 1) + ". " + d.get("isbn") + "\t" + d.get("title"));
            }
	      
            // reader can only be closed when there
            // is no need to access the documents any more.
            reader.close();

            return results;
        }
    }
}
