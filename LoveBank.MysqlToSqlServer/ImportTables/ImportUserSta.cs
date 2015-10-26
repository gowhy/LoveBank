using System;
using Helpers;
using QDT.Common;

namespace QDT.MysqlToSqlServer.ImportTables
{
    public class ImportUserSta : ImportTables
    {
        public void Import()
        {
            var mySqlHelper = new MySqlHelper(MySql);
            var sqlHelper = new SqlHelper(SqlServer);
            var mysql = string.Format("select * from qdt_user_sta");
            var dt = mySqlHelper.ExecuteDataTable(mysql);
            var sql = "";
            var n = 0;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                sql += "INSERT INTO qdt_user_sta (user_id,borrow_amount,repay_amount,need_repay_amount,deal_count,success_deal_count,load_earnings,load_count,load_money,load_repay_money,load_wait_repay_money,reback_load_count,wait_reback_load_count) " +
                       "VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}');\r\n"
                       .FormatWith(dt.Rows[i]["user_id"], dt.Rows[i]["borrow_amount"], dt.Rows[i]["repay_amount"], dt.Rows[i]["need_repay_amount"], dt.Rows[i]["deal_count"], dt.Rows[i]["success_deal_count"], dt.Rows[i]["load_earnings"]
                       , dt.Rows[i]["load_count"], dt.Rows[i]["load_money"], dt.Rows[i]["load_repay_money"], dt.Rows[i]["load_wait_repay_money"], dt.Rows[i]["reback_load_count"], dt.Rows[i]["wait_reback_load_count"]);
                if ((i + 1) % 100 != 0) continue;
                n += sqlHelper.ExecuteNonQuery(sql);
                sql = "";
            }
            if (!string.IsNullOrWhiteSpace(sql))
            n += sqlHelper.ExecuteNonQuery(sql);
            Console.WriteLine("成功导入user_sta表：" + n + "条数据！");
        }
    }
}
