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
using System.Text.RegularExpressions;
using System.Threading;

namespace CIS345FinalApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LuceneIndexHandler indexer = null;
        private XMLFileHandler handler = null;
        public static MainWindow Program = null;
        List<string> elementList;
        List<string> fileList;
        public MainWindow()
        {
            InitializeComponent();
            Program = this;

            txtContent.IsReadOnly = true;
            txtHits.IsReadOnly = true;
            cmbFile.IsReadOnly = true;
            cmbFile.Focusable = false;
            cmbFile.IsHitTestVisible = false;
            cmbTag.IsReadOnly = true;
            cmbTag.Focusable = false;
            cmbTag.IsHitTestVisible = false;
            btnQuery.IsEnabled = false;
            txtOutput.Text = "Building indices";
            txtOutput.Foreground = Brushes.Red;

            elementList = new List<string>();
            fileList = new List<string>();
            indexer = new LuceneIndexHandler();
            
            Thread indexBuilder = new Thread(new ParameterizedThreadStart(BuildIndices));
            indexBuilder.Start();
        }

        public void FinishedIndexing()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                cmbTag.ItemsSource = elementList;
                cmbFile.ItemsSource = fileList;
                txtContent.IsReadOnly = false;
                txtHits.IsReadOnly = false;
                cmbFile.IsReadOnly = false;
                cmbFile.Focusable = true;
                cmbFile.IsHitTestVisible = true;

                cmbTag.IsReadOnly = false;
                cmbTag.Focusable = true;
                cmbTag.IsHitTestVisible = true;

                btnQuery.IsEnabled = true;
                txtOutput.Foreground = Brushes.Black;
                txtOutput.Text = "Ready";
            }));
        }

        public void BuildIndices(object data)
        {
            string fileDir = "./xmlDocs";
            java.io.File[] files = new java.io.File(fileDir).listFiles();
            
            elementList.Add("");
            fileList.Add("");
            if (files.Length == 0)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    txtOutput.Text = "No XML files to index.";
                    MessageBox.Show("No XML files to index.");
                }));
                return;
            }
            int numFiles = files.Length;
            int currentFileNum = 1;

            foreach (java.io.File file in files)
            {
                string filePath = file.getAbsolutePath();
                string fileExt = "";
                string fileName = file.getName();
                fileList.Add(fileName);

                int dotIndex = file.getName().LastIndexOf('.');
                if (dotIndex > 0)
                {
                    fileExt = file.getName().Substring(dotIndex + 1);

                    if (fileExt.Equals("xml", StringComparison.OrdinalIgnoreCase))
                    {
                        SAXParserFactory pFactory = SAXParserFactory.newInstance();
                        pFactory.setValidating(false);
                        pFactory.setNamespaceAware(true);
                        SAXParser parser = pFactory.newSAXParser();
                        XMLReader reader = parser.getXMLReader();

                        handler = new XMLFileHandler(indexer, fileName, currentFileNum, numFiles, elementList);
                        reader.setContentHandler(handler);
                        try
                        {
                            reader.parse(new InputSource(new java.io.FileReader(filePath)));
                        }
                        catch (Exception exc)
                        {
                            Dispatcher.BeginInvoke(new Action(() =>
                            {
                                string message = "There was an error parsing file " + fileName + ". Continuing indexing.";
                                txtOutput.Text = message;
                                MessageBox.Show(message);
                            }));
                        }
                    }
                }
                currentFileNum++;
            }

            FinishedIndexing();
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.Text = "";
            if ((String.IsNullOrEmpty(txtContent.Text) && String.IsNullOrEmpty(cmbFile.Text) && String.IsNullOrEmpty(cmbTag.Text)) ||
                (!String.IsNullOrEmpty(cmbFile.Text) && String.IsNullOrEmpty(txtContent.Text) && String.IsNullOrEmpty(cmbTag.Text)))
            {
                txtOutput.Text = "At least the 'Search Query' or 'Search Element' search parameter must be present.";
                return;
            }
            else if (!String.IsNullOrEmpty(txtContent.Text))
            {
                if (txtContent.Text.Substring(0, 1).Equals("*") || txtContent.Text.Substring(0, 1).Equals("?"))
                {
                    txtOutput.Text = "A query string can not start with a * or ?. If you want to serach for a literal *, use \"*\".";
                    return;
                }
                else if (txtContent.Text.IndexOf('"') >= 0 && (txtContent.Text.IndexOf('"') != 0 || txtContent.Text.LastIndexOf('"') != txtContent.Text.Length - 1))
                {
                    txtOutput.Text = "A literal search query must start and end with a \" mark.";
                    return;
                }
                else if (txtContent.Text.Equals("\""))
                {
                    txtOutput.Text = "You can not search for a single \" mark.";
                    return;
                }
            }

            int hitsPerPage = 10;
            const int hitsPerPageMaximum = 100;
            string hitsError = "The 'Number of Results' field must be a valid integer from 1 - " + hitsPerPageMaximum.ToString() + ".";
            if (!String.IsNullOrEmpty(txtHits.Text))
            {
                try
                {
                    hitsPerPage = Convert.ToInt32(txtHits.Text);
                }
                catch (Exception exc)
                {
                    txtOutput.Text = hitsError;
                    return;
                }
            }

            if (hitsPerPage < 1 || hitsPerPage > hitsPerPageMaximum)
            {
                txtOutput.Text = hitsError;
                return;
            }

            List<SearchResult> results = indexer.SearchIndex(cmbTag.Text, txtContent.Text, cmbFile.Text, hitsPerPage);

            if (results.Count == 0)
            {
                txtOutput.Text = "No Results";
            }

            dataResults.ItemsSource = results;
        }
    }
}
