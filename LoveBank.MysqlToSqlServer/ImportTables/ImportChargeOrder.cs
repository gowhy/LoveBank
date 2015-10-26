using System;
using Helpers;
using QDT.Common;

namespace QDT.MysqlToSqlServer.ImportTables
{
    public class ImportChargeOrder : ImportTables
    {

        private static string GetPaymentKey(int id)
        {
            switch (id)
            {
                case 3:
                    return "GuofubaoPayment";
                case 4:
                    return "ChinaBankDirectPayment";
                case 5:
                    return "GuofubaoQuickPayment";
                case 6:
                    return "ChinaBankPayment";
                default:
                    return "~~";
            }
        }

        public void Import()
        {
            var mySqlHelper = new MySqlHelper(MySql);
            var sqlHelper = new SqlHelper(SqlServer);

            var mysql = string.Format("select * from qdt_deal_order");
            var dt = mySqlHelper.ExecuteDataTable(mysql);
            var sql = "";
            var n = 0;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToDecimal(dt.Rows[i]["total_price"]).ToString().Length > 16) continue;
                sql += "INSERT INTO qdt_charge_order (id,order_sn,user_id,status,total_price,pay_amount,payment_key,bank_code,is_delete,memo,admin_memo,create_time,update_time) " +
                       "VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}');\r\n"
                       .FormatWith(dt.Rows[i]["id"], dt.Rows[i]["order_sn"], dt.Rows[i]["user_id"], dt.Rows[i]["pay_status"], dt.Rows[i]["total_price"], dt.Rows[i]["pay_amount"], GetPaymentKey(Convert.ToInt32(dt.Rows[i]["payment_id"])), dt.Rows[i]["bank_id"], dt.Rows[i]["is_delete"], dt.Rows[i]["memo"],
                       dt.Rows[i]["admin_memo"], GetTime(dt.Rows[i]["create_time"]), GetTime(dt.Rows[i]["update_time"])).Replace("'NULL'", "NULL");
                if ((i+1)%100 != 0) continue;
                n += sqlHelper.ExecuteNonQuery(sql);
                sql = "";
            }
            if (!string.IsNullOrWhiteSpace(sql))
            n += sqlHelper.ExecuteNonQuery(sql);
            Console.WriteLine("成功导入charge_order表：" + n + "条数据！");
        }
    }
}
