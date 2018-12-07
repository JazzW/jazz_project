package org.jazz.common.core.asm;
import org.objectweb.asm.*; 

public class AddSecurityCheckClassAdapter  extends ClassVisitor{




	public AddSecurityCheckClassAdapter(int api, ClassVisitor classVisitor) {
		super(api, classVisitor);
		// TODO Auto-generated constructor stub
	}

	public MethodVisitor visitMethod(
		      final int access,
		      final String name,
		      final String descriptor,
		      final String signature,
		      final String[] exceptions) {
		  
		  System.out.println("visit method"+name);
		  MethodVisitor mv = super.visitMethod(access, name, descriptor, signature, exceptions);
	        MethodVisitor wrappedMv = mv; 
	        if (mv != null) { 
	            // 对于 "operation" 方法
	            if (name.equals("action")) { 
	            	  System.out.println(name);
	                // 使用自定义 MethodVisitor，实际改写方法内容
	                wrappedMv = new AddSecurityCheckMethodAdapter(this.api); 
	            } 
	        } 
	        return wrappedMv; 
	  }



}
