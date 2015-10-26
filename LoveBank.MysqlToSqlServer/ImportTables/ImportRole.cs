using System;
using Helpers;
using QDT.Common;

namespace QDT.MysqlToSqlServer.ImportTables
{
    public class ImportRole : ImportTables
    {
        public void Import()
        {
            var mySqlHelper = new MySqlHelper(MySql);
            var sqlHelper = new SqlHelper(SqlServer);
            var mysql = string.Format("select * from qdt_role");
            var dt = mySqlHelper.ExecuteDataTable(mysql);
            var sql = "";
            var n = 0;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                sql += "INSERT INTO qdt_role (id,name,is_effect,is_delete) " +
                       "VALUES ('{0}','{1}','{2}','{3}');\r\n"
                       .FormatWith(dt.Rows[i]["id"], dt.Rows[i]["name"], dt.Rows[i]["is_effect"], dt.Rows[i]["is_delete"]);
                if ((i + 1) % 100 != 0) continue;
                n += sqlHelper.ExecuteNonQuery(sql);
                sql = "";
            }
            if (!string.IsNullOrWhiteSpace(sql))
            n += sqlHelper.ExecuteNonQuery(sql);
            Console.WriteLine("成功导入role表：" + n + "条数据！");
        }
    }
}
