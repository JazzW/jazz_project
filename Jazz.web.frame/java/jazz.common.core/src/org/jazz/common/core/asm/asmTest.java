package org.jazz.common.core.asm;

import java.io.File; 
import java.io.FileOutputStream;
import java.io.IOException;

import org.objectweb.asm.*; 

public class asmTest {

	public static void main(String[] args) throws IOException {
		// TODO Auto-generated method stub
		new Myaction().action();
		 ClassReader cr = new ClassReader("org.jazz.common.core.asm.Myaction");
		 ClassWriter cw = new ClassWriter(ClassWriter.COMPUTE_MAXS);
	
		 cr.accept(new AddSecurityCheckClassAdapter(Opcodes.ASM6,cw), ClassReader.SKIP_DEBUG);
		 byte[] data = cw.toByteArray(); 
		 File file = new File("Myaction.class"); 
		 FileOutputStream fout = new FileOutputStream(file); 
		 fout.write(data); 
		 fout.close(); 
			new Myaction().action();
			new Myaction().action();
	}

}
