using System;
using Helpers;

namespace QDT.MysqlToSqlServer.ImportTables
{
    public class ImportAdmin : ImportTables
    {
        public void Import()
        {
            var mySqlHelper = new MySqlHelper(MySql);
            var sqlHelper = new SqlHelper(SqlServer);
            var mysql = string.Format("select * from qdt_admin");
            var dt = mySqlHelper.ExecuteDataTable(mysql);
            var sql = "";
            var n = 0;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                sql += "INSERT INTO qdt_admin (id,name,password,is_effect,is_delete,role_id,login_time,login_ip) " +
                       "VALUES ('" + dt.Rows[i]["id"] + "','" + dt.Rows[i]["adm_name"] + "','" + dt.Rows[i]["adm_password"].ToString().ToUpper() + "','" + dt.Rows[i]["is_effect"] + "','" + dt.Rows[i]["is_delete"] + "','" + dt.Rows[i]["role_id"] + "','" + GetTime(dt.Rows[i]["login_time"]) + "','" + dt.Rows[i]["login_ip"] + "');".Replace("'NULL'", "NULL");
                if ((i + 1) % 100 != 0) continue;
                n += sqlHelper.ExecuteNonQuery(sql);
                sql = "";
            }
            if (!string.IsNullOrWhiteSpace(sql))
            n += sqlHelper.ExecuteNonQuery(sql);
            Console.WriteLine("成功导入admin表：" + n + "条数据！");
        }
    }
}
