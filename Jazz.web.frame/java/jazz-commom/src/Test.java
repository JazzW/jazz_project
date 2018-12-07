

import org.jazz.common.enitiy.attribute.DBItemAttribute;
import org.jazz.common.enitiy.attribute.DBModelAttribute;
import org.jazz.common.enitiy.attribute.KeyAttribute;

@DBModelAttribute(TBName = "[Table]")
public class Test {
	
	@KeyAttribute(IdenityKey = false)
	@DBItemAttribute(ColName = "Id", Display = "±àºÅ")
	private int Id;
	
	@DBItemAttribute(ColName = "Name", Display = "Ãû×Ö")
	private String Name;
	
	@DBItemAttribute(ColName = "Mess", Display = "ÐÅÏ¢")
	private String Mess;
	
	public int getId(){
		return this.Id;
	}
	
	public void setId(int id){
		this.Id=id;
	}

	public String getMess() {
		return Mess;
	}

	public void setMess(String mess) {
		Mess = mess;
	}

	public String getName() {
		return Name;
	}

	public void setName(String name) {
		Name = name;
	}
}
