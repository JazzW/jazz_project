package org.jazz.helper.database.common;

import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;
 
public final class NSQL {
 
 
    private String sql_naming;
    private String sql_execute;
    private String[] names;
 
    private NSQL() {
        // �û�����ʵ��������
        // ͨ��get������ȡ����ʵ��
    }
     
    public boolean hasName() {
        return names.length > 0;
    }
 
    public void setParameter(PreparedStatement ps, String name, Object value) throws SQLException {
        for (int a = 0, b = names.length - 1; a <= b; a++, b--) {
            if (names[a].equals(name)) {
                ps.setObject(a + 1, value);
            }
            if (a != b && names[b].equals(name)) {
                ps.setObject(b + 1, value);
            }
        }
    }
 
    public void setParameters(PreparedStatement ps, Map<String, Object> values) throws SQLException {
        for (int index = 0; index < names.length; index++) {
            ps.setObject(index + 1, values.get(names[index]));
        }
    }
 
    /**
     * ��ȡ�������ݿ�ִ�е�SQL���
     * 
     * @return
     */
    public String getSql() {
        return sql_execute;
    }
 
    /**
     * ��ȡ�û����������SQL���
     * 
     * @return
     */
    public String getNamingSql() {
        return sql_naming;
    }
 
    public static NSQL get(String sql) {
    	NSQL nsql = NSQL.parse(sql);
 
        return nsql;
    }
    /**
     * ��������SQL����ȡ����NSQlʵ����java(JDBC)�ṩSQL���������������ͨ��?��ʶ����λ�ã�
     * ͨ���˶����������������ʽʹ��SQL��䣬����������?��ʼ�������?name��
     * ���磺SELECT * FROM table WHERE name = ?key AND email = ?key;
     * 
     * @param sql
     * @return
     */
    public static NSQL parse(String sql) {
        // SELECT * FROM table WHERE name = ?key AND email = ?key;
        // A~Z a~z 01~9 _
        if (sql == null)
            throw new NullPointerException("SQL String is null");
 
        char c;
        List<String> names = new ArrayList<String>();
        StringBuilder sql_builder = new StringBuilder();
        StringBuilder name_builder = new StringBuilder();
        for (int index = 0; index < sql.length(); index++) {
            c = sql.charAt(index);
            sql_builder.append(c);
            if ('?' == c) {
                while (++index < sql.length()) {
                    c = sql.charAt(index);
                    if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || c == '_' || (c >= '0' && c <= '9')) {
                        name_builder.append(c);
                    } else {
                        sql_builder.append(c);
                        break;
                    }
                }
                names.add(name_builder.toString());
                name_builder.setLength(0);
            }
        }
        NSQL dbsql = new NSQL();
        dbsql.sql_naming = sql;
        dbsql.sql_execute = sql_builder.toString();
        dbsql.names = names.toArray(dbsql.names = new String[names.size()]);
        return dbsql;
    }
 
    public String toString() {
        return "NAMING: " + sql_naming + "\nEXECUTE: " + sql_execute;
    }
}
