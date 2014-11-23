import java.io.File;
import java.io.FileReader;
import java.util.Vector;

import javax.xml.parsers.SAXParser;
import javax.xml.parsers.SAXParserFactory;

import org.xml.sax.Attributes;
import org.xml.sax.InputSource;
import org.xml.sax.XMLReader;
import org.xml.sax.helpers.DefaultHandler;


public class StructuralSummaryHandler extends DefaultHandler {
	int level;
	StructuralSummary[] summary;
	int nodeId;
	public static String output = "";

	public StructuralSummaryHandler () {
	    level = 0;
	    summary = new StructuralSummary[100];
	    nodeId = 0;
	    summary[0] = new StructuralSummary(0,"");
	}

	public void startElement ( String uri, String localName, String qName, Attributes atts ) {
           /* Missing code start here */
		level += 1;
		nodeId += 1;
    	System.out.println(localName+":"+level+":"+nodeId);
    	output += localName+":"+level+":"+nodeId;
		
           /* Missing code end here */
	}

	public void endElement ( String uri, String localName, String qName) {
           /* Missing code start here */
    	level -= 1;

           /* Missing code end here */
	}
	
	public static void main(String[] args) throws Exception 
	{
		IndexTest();
	}
	
	public static String IndexTest() throws Exception
	{
		//String output = "";
		// Set the directory for xml files
	    String fileDir = "./xmlDocs"; 			// "." is the relative directory for project
	    
	    // Get all xml files in the directory
	    File[] files = new File(fileDir).listFiles();
	   
	    // For each file found,
	    for (File file: files) {
	    	// Determine its file type
	    	String filePath = file.getPath();
	    	String fileExt = "";
	    	String fileName = "";
	    	int dotIndex = file.getName().lastIndexOf('.');
	    	if (dotIndex > 0) {
//		    	System.out.println("File type " + filePath.substring(dotIndex+1));
		    	fileExt = file.getName().substring(dotIndex+1);
		    	// If the file extention is xml, 
		    	if (fileExt.equalsIgnoreCase("xml")) {
		    		fileName = file.getName().substring(0, dotIndex);
		    		
		    		// Attempt to parse the xml structure
//			    	System.out.println("File to Parse " + filePath + "");
				    System.out.println("********** Summary of "+fileName+" **********");
				    output += "********** Summary of "+fileName+" **********";
				    SAXParserFactory pfactory = SAXParserFactory.newInstance();
				    pfactory.setValidating(false);
				    pfactory.setNamespaceAware(true);
				    SAXParser parser = pfactory.newSAXParser();
				    XMLReader reader = parser.getXMLReader();
				    StructuralSummaryHandler handler1 = new StructuralSummaryHandler();
				    reader.setContentHandler(handler1);
				    reader.parse(new InputSource(new FileReader(filePath)));
//				    StructuralSummary ss1 = handler1.summary[0].children.elementAt(0);
//				    ss1.prettyPrint(0);
				    System.out.println("**********End Summary of "+fileName+ "**********\n");
				    output += "**********End Summary of "+fileName+ "**********\n";
		    	}
	    	}
	    }
//	    StructuralSummaryHandler handler2 = new StructuralSummaryHandler();
//	    reader.setContentHandler(handler2);
//	    reader.parse(new InputSource(new FileReader(dir+file2)));
//	    StructuralSummary ss2 = handler2.summary[0].children.elementAt(0);
//	    System.out.println("Structural Summary of "+file2+":");
//	    ss2.prettyPrint(0);
	    
	    return output;
	}
}
