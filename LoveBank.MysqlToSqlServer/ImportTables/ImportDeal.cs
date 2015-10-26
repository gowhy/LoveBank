using System;
using Helpers;
using QDT.Common;

namespace QDT.MysqlToSqlServer.ImportTables
{
    public class ImportDeal : ImportTables
    {
        public void Import()
        {
            var mySqlHelper = new MySqlHelper(MySql);
            var sqlHelper = new SqlHelper(SqlServer);
            var mysql = string.Format("select * from qdt_deal");
            var dt = mySqlHelper.ExecuteDataTable(mysql);
            var sql = "";
            var n = 0;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                sql += "INSERT INTO qdt_deal (id,name,sub_name,cate_id,user_id,description,is_effect,is_delete,borrow_amount,min_loan_money," +
                       "repay_time,rate,create_time,update_time,is_recommend,buy_count,load_money,repay_money,start_time,success_time," +
                       "repay_start_time,last_repay_time,next_repay_time,bad_time,deal_status,enddate,services_fee,is_send_bad_msg,bad_msg,loantype," +
                       "warrant,titlecolor,is_send_contract,max_loan_money,icon,repay_time_type) " +
                       "VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}','{31}','{32}','{33}','{34}','{35}');\r\n"
                       .FormatWith(dt.Rows[i]["id"], dt.Rows[i]["name"], dt.Rows[i]["sub_name"], dt.Rows[i]["cate_id"], dt.Rows[i]["user_id"], dt.Rows[i]["description"].ToString().Replace('\'', '\"').Replace("./help/id-", "/Help/Index/").Replace("?", ""), dt.Rows[i]["is_effect"], dt.Rows[i]["is_delete"], dt.Rows[i]["borrow_amount"], dt.Rows[i]["min_loan_money"],
                       dt.Rows[i]["repay_time"], dt.Rows[i]["rate"], GetTime(dt.Rows[i]["create_time"]), GetTime(dt.Rows[i]["update_time"]), dt.Rows[i]["is_recommend"], dt.Rows[i]["buy_count"], dt.Rows[i]["load_money"], dt.Rows[i]["repay_money"], GetTime(dt.Rows[i]["start_time"]), GetTime(dt.Rows[i]["success_time"]),
                       GetTime(dt.Rows[i]["repay_start_time"]), GetTime(dt.Rows[i]["last_repay_time"]), GetTime(dt.Rows[i]["next_repay_time"]), GetTime(dt.Rows[i]["bad_time"]), dt.Rows[i]["deal_status"], dt.Rows[i]["enddate"], dt.Rows[i]["services_fee"], dt.Rows[i]["is_send_bad_msg"], dt.Rows[i]["bad_msg"], dt.Rows[i]["loantype"],
                       dt.Rows[i]["warrant"], dt.Rows[i]["titlecolor"], dt.Rows[i]["is_send_contract"], dt.Rows[i]["max_loan_money"], dt.Rows[i]["icon"], dt.Rows[i]["repay_time_type"]).Replace("'NULL'","NULL");
                if ((i + 1) % 10 != 0) continue;
                n += sqlHelper.ExecuteNonQuery(sql);
                sql = "";
            }
            if (!string.IsNullOrWhiteSpace(sql))
            n += sqlHelper.ExecuteNonQuery(sql);
            Console.WriteLine("成功导入deal表：" + n + "条数据！");
        }
    }
}
