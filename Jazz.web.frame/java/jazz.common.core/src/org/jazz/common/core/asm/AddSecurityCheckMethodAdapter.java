package org.jazz.common.core.asm;

import org.objectweb.asm.MethodVisitor;

public class AddSecurityCheckMethodAdapter extends MethodVisitor {

	public AddSecurityCheckMethodAdapter(int api) {
		super(api);
		// TODO Auto-generated constructor stub
	}

	public void visitCode() { 
		System.out.println("visit code");
		if (mv != null) {
		      mv.visitCode();
		    }
	 } 
}
