using System;
using Helpers;
using QDT.Common;

namespace QDT.MysqlToSqlServer.ImportTables
{
    public class ImportUserLog : ImportTables
    {
        public void Import()
        {
            var mySqlHelper = new MySqlHelper(MySql);
            var sqlHelper = new SqlHelper(SqlServer);

            var mysql = string.Format("select * from qdt_user_log");
            var dt = mySqlHelper.ExecuteDataTable(mysql);
            var sql = "";
            var n = 0;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                sql += "INSERT INTO qdt_user_log (id,log_info,log_time,user_id,log_admin_id,log_user_id,money,lock_money,point,quota) " +
                       "VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}');\r\n"
                       .FormatWith(dt.Rows[i]["id"], dt.Rows[i]["log_info"], GetTime(dt.Rows[i]["log_time"]), dt.Rows[i]["user_id"], dt.Rows[i]["log_admin_id"], dt.Rows[i]["log_user_id"], dt.Rows[i]["money"], dt.Rows[i]["lock_money"], dt.Rows[i]["point"], dt.Rows[i]["quota"]);
                if ((i+1)%100 != 0) continue;
                n += sqlHelper.ExecuteNonQuery(sql);
                sql = "";
            }
            if (!string.IsNullOrWhiteSpace(sql))
            n += sqlHelper.ExecuteNonQuery(sql);
            Console.WriteLine("成功导入user_log表：" + n + "条数据！");
        }
    }
}
