using System;
using Helpers;
using QDT.Common;

namespace QDT.MysqlToSqlServer.ImportTables
{
    public class ImportDealLoad : ImportTables
    {
        public void Import()
        {
            var mySqlHelper = new MySqlHelper(MySql);
            var sqlHelper = new SqlHelper(SqlServer);
            var mysql = string.Format("select * from qdt_deal_load");
            var dt = mySqlHelper.ExecuteDataTable(mysql);
            var sql = "";
            var n = 0;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                sql += "INSERT INTO qdt_deal_load (id,deal_id,user_id,user_name,money,create_time,is_repay) " +
                       "VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}');\r\n"
                       .FormatWith(dt.Rows[i]["id"], dt.Rows[i]["deal_id"], dt.Rows[i]["user_id"], dt.Rows[i]["user_name"], dt.Rows[i]["money"], GetTime(dt.Rows[i]["create_time"]), dt.Rows[i]["is_repay"]).Replace("'NULL'", "NULL");
                if ((i + 1) % 100 != 0) continue;
                n += sqlHelper.ExecuteNonQuery(sql);
                sql = "";
            }
            if (!string.IsNullOrWhiteSpace(sql))
            n += sqlHelper.ExecuteNonQuery(sql);
            Console.WriteLine("成功导入deal_load表：" + n + "条数据！");
        }
    }
    
}
