using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using org.xml.sax;
using org.xml.sax.helpers;

namespace CIS345FinalApplication
{
    public class XMLFileHandler : DefaultHandler
    {
        private LuceneIndexHandler indexHandler;
        private string currentElement;
        private List<string> elements;
        private string currentFileName;

        public XMLFileHandler(LuceneIndexHandler indexer, string filePath)
        {
            currentFileName = filePath;
            indexHandler = indexer;
            elements = new List<string>();
        }

        public List<string> GetElements()
        {
            return elements;
        }

        public override void startElement (String uri, String localName, String qName, Attributes atts )
        {
            currentElement = localName;
            if (!elements.Contains(localName))
            {
                elements.Add(localName);
            }
	    }

        public override void characters(char[] ch, int start, int length)
        {
            if (String.IsNullOrEmpty(currentElement))
            {
                return;
            }
            string fileContextString = "";
            string currentElementContent = "";

            for (int i = 0; i < ch.Length; i++)
            {
                fileContextString += ch[i];
                if (i >= start && i < (start + length))
                {
                    currentElementContent += ch[i];
                }
            }

            indexHandler.AddDocument(currentElement, currentElementContent, currentFileName, fileContextString);
        }

        public override void endElement(String uri, String localName, String qName)
        {
            currentElement = "";
        }
    }
}
