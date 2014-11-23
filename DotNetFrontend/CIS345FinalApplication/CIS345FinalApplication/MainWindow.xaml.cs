using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using javax.xml.parsers;
using org.xml.sax;

namespace CIS345FinalApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LuceneIndexHandler indexer = null;
        XMLFileHandler handler = null;

        public MainWindow()
        {
            InitializeComponent();
            txtOutput.Text = "";

            string fileDir = "./xmlDocs";
            java.io.File[] files = new java.io.File(fileDir).listFiles();
            indexer = new LuceneIndexHandler();
            
            foreach (java.io.File file in files)
            {
                string filePath = file.getPath();
                string fileExt = "";
                string fileName = "";

                int dotIndex = file.getName().LastIndexOf('.');
                if (dotIndex > 0)
                {
                    fileExt = file.getName().Substring(dotIndex + 1);
                    txtOutput.Text += "Extension: " + fileExt + Environment.NewLine;

                    if (fileExt.Equals("xml", StringComparison.OrdinalIgnoreCase))
                    {
                        SAXParserFactory pFactory = SAXParserFactory.newInstance();
                        pFactory.setValidating(false);
                        pFactory.setNamespaceAware(true);
                        SAXParser parser = pFactory.newSAXParser();
                        XMLReader reader = parser.getXMLReader();

                        handler = new XMLFileHandler(indexer);
                        reader.setContentHandler(handler);
                        reader.parse(new InputSource(new java.io.FileReader(filePath)));
                    }
                }
            }
            cmbTag.ItemsSource = handler.GetElements();
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.Text = "";
            if (String.IsNullOrEmpty(cmbTag.Text) && String.IsNullOrEmpty(txtContent.Text))
            {
                txtOutput.Text = "Please enter either a tag or a value to search for.";
                return;
            }
            else if (!String.IsNullOrEmpty(cmbTag.Text))
            {
                if (!handler.GetElements().Contains(cmbTag.Text))
                {
                    txtOutput.Text = "Tag '" + cmbTag.Text + "' not found in XML document.";
                    return;
                }
            }

            List<string> results = indexer.SearchIndex(cmbTag.Text, txtOutput.Text);

            if (results.Count() < 1)
            {
                txtOutput.Text = "No results found.";
                return;
            }

            txtOutput.Text += "Results:" + Environment.NewLine;
            for (int i = 0; i < results.Count(); i++)
            {
                txtOutput.Text += (i + 1) + ": " + results.ElementAt(i) + Environment.NewLine;
            }
        }
    }
}
