using System;
using Helpers;
using QDT.Common;

namespace QDT.MysqlToSqlServer.ImportTables
{
    public class ImportPaymentNotice : ImportTables
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

            var mysql = string.Format("select * from qdt_payment_notice");
            var dt = mySqlHelper.ExecuteDataTable(mysql);
            var sql = "";
            var n = 0;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                if (Convert.ToDecimal(dt.Rows[i]["Money"]).ToString().Length > 16) continue;
                sql += "INSERT INTO qdt_payment_notice (id,NoticeSn,CreateTime,PayTime,OrderId,IsPaid,UserId,PaymentKey,Memo,Money,OuterNoticeSn) " +
                       "VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}');\r\n"
                       .FormatWith(dt.Rows[i]["id"], dt.Rows[i]["notice_sn"], GetTime(dt.Rows[i]["Create_Time"]), GetTime(dt.Rows[i]["Pay_Time"]), dt.Rows[i]["Order_Id"], dt.Rows[i]["Is_Paid"], dt.Rows[i]["User_Id"], GetPaymentKey(Convert.ToInt32(dt.Rows[i]["payment_id"])), dt.Rows[i]["Memo"], dt.Rows[i]["Money"],
                       dt.Rows[i]["Outer_Notice_Sn"]).Replace("'NULL'", "NULL");
                if ((i+1)%100 != 0) continue;
                n += sqlHelper.ExecuteNonQuery(sql);
                sql = "";
            }
            if (!string.IsNullOrWhiteSpace(sql))
            n += sqlHelper.ExecuteNonQuery(sql);
            Console.WriteLine("成功导入payment_notice表：" + n + "条数据！");
        }
    }
}
