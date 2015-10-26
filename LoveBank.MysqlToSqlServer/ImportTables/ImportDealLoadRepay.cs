using System;
using Helpers;
using QDT.Common;

namespace QDT.MysqlToSqlServer.ImportTables
{
    public class ImportDealLoadRepay : ImportTables
    {
        public void Import()
        {
            var mySqlHelper = new MySqlHelper(MySql);
            var sqlHelper = new SqlHelper(SqlServer);
            var mysql = string.Format("select * from qdt_deal_load_repay");
            var dt = mySqlHelper.ExecuteDataTable(mysql);
            var sql = "";
            var n = 0;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                sql += "INSERT INTO qdt_deal_load_repay (id,deal_id,user_id,self_money,repay_money,manage_money,impose_money,repay_time,true_repay_time,status,u_key) " +
                       "VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}');\r\n"
                       .FormatWith(dt.Rows[i]["id"], dt.Rows[i]["deal_id"], dt.Rows[i]["user_id"], dt.Rows[i]["self_money"], dt.Rows[i]["repay_money"], dt.Rows[i]["manage_money"], dt.Rows[i]["impose_money"], GetTime(dt.Rows[i]["repay_time"]), GetTime(dt.Rows[i]["true_repay_time"]), dt.Rows[i]["status"], dt.Rows[i]["u_key"]).Replace("'NULL'", "NULL");
                if ((i + 1) % 100 != 0) continue;
                n += sqlHelper.ExecuteNonQuery(sql);
                sql = "";
            }
            if (!string.IsNullOrWhiteSpace(sql))
            n += sqlHelper.ExecuteNonQuery(sql);
            Console.WriteLine("成功导入deal_load_repay表：" + n + "条数据！");
        }
    }
    
}
