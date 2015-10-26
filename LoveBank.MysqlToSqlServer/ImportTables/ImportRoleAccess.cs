using System;
using Helpers;
using QDT.Common;

namespace QDT.MysqlToSqlServer.ImportTables
{
    public class ImportRoleAccess : ImportTables
    {
        public void Import()
        {
            var mySqlHelper = new MySqlHelper(MySql);
            var sqlHelper = new SqlHelper(SqlServer);
            var mysql = string.Format("select * from qdt_role_access");
            var dt = mySqlHelper.ExecuteDataTable(mysql);
            var sql = "";
            var n = 0;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                sql += "INSERT INTO qdt_role_access (id,role_id,node,module) " +
                       "VALUES ('{0}','{1}','{2}','{3}');\r\n"
                       .FormatWith(dt.Rows[i]["id"], dt.Rows[i]["role_id"], dt.Rows[i]["node_id"], dt.Rows[i]["module_id"]);
                if ((i + 1) % 100 != 0) continue;
                n += sqlHelper.ExecuteNonQuery(sql);
                sql = "";
            }
            if (!string.IsNullOrWhiteSpace(sql))
             n += sqlHelper.ExecuteNonQuery(sql);
            Console.WriteLine("成功导入role_access表：" + n + "条数据！");
        }
    }
}
