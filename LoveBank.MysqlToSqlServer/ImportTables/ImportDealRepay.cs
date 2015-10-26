using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers;
using QDT.Common;

namespace QDT.MysqlToSqlServer.ImportTables
{
    public class ImportDealRepay : ImportTables
    {
        public void Import()
        {
            var mySqlHelper = new MySqlHelper(MySql);
            var sqlHelper = new SqlHelper(SqlServer);
            var mysql = string.Format("select * from qdt_deal_repay");
            var dt = mySqlHelper.ExecuteDataTable(mysql);
            var sql = "";
            var n = 0;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                sql += "INSERT INTO qdt_deal_repay (id,deal_id,user_id,repay_self_money,repay_money,manage_money,repay_time,true_repay_time,status) " +
                       "VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}');\r\n"
                       .FormatWith(dt.Rows[i]["id"], dt.Rows[i]["deal_id"], dt.Rows[i]["user_id"], 0, dt.Rows[i]["repay_money"], dt.Rows[i]["manage_money"], GetTime(dt.Rows[i]["repay_time"]), GetTime(dt.Rows[i]["true_repay_time"]), dt.Rows[i]["status"]).Replace("'NULL'", "NULL");
                if ((i + 1) % 100 != 0) continue;
                n += sqlHelper.ExecuteNonQuery(sql);
                sql = "";
            }
            if (!string.IsNullOrWhiteSpace(sql))
            n += sqlHelper.ExecuteNonQuery(sql);
            Console.WriteLine("成功导入deal_repay表：" + n + "条数据！");
        }
    }
}
