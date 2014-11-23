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
        //private int level = 0;
        //public string output = "";
        private LuceneIndexHandler indexHandler;
        private string currentElement;
        private List<string> elements;

        public XMLFileHandler(LuceneIndexHandler indexer)
        {
            //TODO: constructor
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
            //level += 1;
            //string spacing = "";
            //for (int i = 0; i < level; i++)
            //{
            //    spacing += " ";
            //}
            //output += spacing + localName + ":" + level + ": ";
            //nodeId += 1;
            //System.out.println(localName+":"+level+":"+nodeId);
            //output += localName+":"+level+":"+nodeId;
	    }

        public override void characters(char[] ch, int start, int length)
        {
            if (String.IsNullOrEmpty(currentElement))
            {
                return;
            }
            string currentElementContent = "";
            for (int i = start; i < start + length; i++)
            {
                currentElementContent += ch[i];
            }

            indexHandler.AddDocument(currentElement, currentElementContent);
        }

        public override void endElement(String uri, String localName, String qName)
        {
            currentElement = "";
            //...not sure what to do???
            //level -= 1;
            //output += Environment.NewLine;
        }
    }
}
