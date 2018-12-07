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
	            // ���� "operation" ����
	            if (name.equals("action")) { 
	            	  System.out.println(name);
	                // ʹ���Զ��� MethodVisitor��ʵ�ʸ�д��������
	                wrappedMv = new AddSecurityCheckMethodAdapter(this.api); 
	            } 
	        } 
	        return wrappedMv; 
	  }



}
