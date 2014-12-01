using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using org.xml.sax;
using org.xml.sax.helpers;
using System.Windows.Threading;

namespace CIS345FinalApplication
{
    public class XMLFileHandler : ContentHandler
    {
        private LuceneIndexHandler indexHandler;
        private Locator locator;
        private string currentElement;
        private List<string> elements;
        public string currentFileName;
        public MainWindow program;
        private int currentFileNum = 1;
        private int numFiles = 0;

        public XMLFileHandler(LuceneIndexHandler indexer, string filePath, int currFileNum, int numberFiles, List<string> elementList)
        {
            currentFileNum = currFileNum;
            numFiles = numberFiles;
            currentFileName = filePath;
            indexHandler = indexer;
            elements = elementList;
        }

        public void setDocumentLocator(Locator locator)
        {
            this.locator = locator;
        }

        public List<string> GetElements()
        {
            return elements;
        }

        public void startElement (String uri, String localName, String qName, Attributes atts )
        {
            if (locator != null)
            {
                MainWindow.Program.Dispatcher.BeginInvoke(new Action(() => { MainWindow.Program.txtOutput.Text = "Building Indices: On File " + currentFileName + "(" + currentFileNum.ToString() + " of " + numFiles.ToString() + "), On Line: " + locator.getLineNumber().ToString() + ", Current Element: " + localName; }));
            }
            currentElement = localName;
            if (!elements.Contains(localName))
            {
                elements.Add(localName);
            }
	    }

        public void characters(char[] ch, int start, int length)
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

        public void endElement(String uri, String localName, String qName)
        {
            currentElement = "";
        }


        public void endDocument()
        {
            //throw new NotImplementedException();
        }

        public void endPrefixMapping(string str)
        {
            //throw new NotImplementedException();
        }

        public void ignorableWhitespace(char[] charr, int i1, int i2)
        {
            //throw new NotImplementedException();
        }

        public void processingInstruction(string str1, string str2)
        {
            //throw new NotImplementedException();
        }

        public void skippedEntity(string str)
        {
            //throw new NotImplementedException();
        }

        public void startDocument()
        {
            //throw new NotImplementedException();
        }

        public void startPrefixMapping(string str1, string str2)
        {
            //throw new NotImplementedException();
        }
    }
}
