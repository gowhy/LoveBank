using System;
using Helpers;
using QDT.Common;

namespace QDT.MysqlToSqlServer.ImportTables
{
    public class ImportUser : ImportTables
    {
        public void Import()
        {
            var mySqlHelper = new MySqlHelper(MySql);
            var sqlHelper = new SqlHelper(SqlServer);
            var mysql = string.Format("select * from qdt_user");
            var dt = mySqlHelper.ExecuteDataTable(mysql);
            var sql = "";
            var n = 0;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                sql += "INSERT INTO qdt_user (id,user_name,user_pwd,sex,create_time,update_time,login_time,login_ip,group_id,is_effect," +
                       "is_delete,email,email_passed,real_name,idno,idcard_passed,mobile,mobile_passed,pay_pwd,pay_pwd_passed," +
                       "is_lender,is_borrower,money,lock_money,point,quota,pid) " +
                       "VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}');\r\n"
                       .FormatWith(dt.Rows[i]["id"], dt.Rows[i]["user_name"], dt.Rows[i]["user_pwd"].ToString().ToUpper(), dt.Rows[i]["sex"], GetTime(dt.Rows[i]["create_time"]), GetTime(dt.Rows[i]["update_time"]), GetTime(dt.Rows[i]["login_time"]), dt.Rows[i]["login_ip"], dt.Rows[i]["group_id"], dt.Rows[i]["is_effect"],
                       dt.Rows[i]["is_delete"], dt.Rows[i]["email"], true, dt.Rows[i]["real_name"], dt.Rows[i]["idno"], dt.Rows[i]["idcardpassed"], dt.Rows[i]["mobile"], dt.Rows[i]["mobilepassed"], dt.Rows[i]["pay_pwd"].ToString().ToUpper(), !string.IsNullOrWhiteSpace(dt.Rows[i]["pay_pwd"].ToString()),
                       !dt.Rows[i]["group_id"].Equals(2), dt.Rows[i]["group_id"].Equals(2), dt.Rows[i]["money"], dt.Rows[i]["lock_money"], dt.Rows[i]["point"], dt.Rows[i]["quota"], dt.Rows[i]["pid"]).Replace("'NULL'", "NULL");
                if ((i + 1) % 30 != 0) continue;
                n += sqlHelper.ExecuteNonQuery(sql);
                sql = "";
            }
            if (!string.IsNullOrWhiteSpace(sql))
            n += sqlHelper.ExecuteNonQuery(sql);
            Console.WriteLine("成功导入user表：" + n + "条数据！");
        }
    }
}
