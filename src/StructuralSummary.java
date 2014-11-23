import java.util.Vector;


public class StructuralSummary {

    public int id;
    public String tagname;
    public Vector<StructuralSummary> children;

    public StructuralSummary ( int id, String tagname ) {
    	this.id = id;
    	this.tagname = tagname;
    	children = new Vector<StructuralSummary>(20);
    }
    
    public void prettyPrint(int level) {	
       /* Missing code start here */
    	

       /* Missing code end here */
    }
}
