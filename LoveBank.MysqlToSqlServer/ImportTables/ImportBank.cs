using System;
using Helpers;
using QDT.Common;

namespace QDT.MysqlToSqlServer.ImportTables
{
    public class ImportBank : ImportTables
    {
        public void Import()
        {
            var mySqlHelper = new MySqlHelper(MySql);
            var sqlHelper = new SqlHelper(SqlServer);
            var mysql = string.Format("select * from qdt_bank");
            var dt = mySqlHelper.ExecuteDataTable(mysql);
            var sql = "";
            var n = 0;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                sql += "INSERT INTO qdt_bank (id,name,is_rec,day,sort) " +
                       "VALUES ('{0}','{1}','{2}','{3}','{4}');\r\n"
                       .FormatWith(dt.Rows[i]["id"], dt.Rows[i]["name"], dt.Rows[i]["is_rec"], dt.Rows[i]["day"], dt.Rows[i]["sort"]);
                if ((i + 1) % 10 != 0) continue;
                n += sqlHelper.ExecuteNonQuery(sql);
                sql = "";
            }
            if (!string.IsNullOrWhiteSpace(sql))
            n += sqlHelper.ExecuteNonQuery(sql);
            Console.WriteLine("成功导入bank表：" + n + "条数据！");
        }
    }
}
