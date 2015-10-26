using System;
using Helpers;
using QDT.Common;

namespace QDT.MysqlToSqlServer.ImportTables
{
    public class ImportDealMsgList : ImportTables
    {
        public void Import()
        {
            var mySqlHelper = new MySqlHelper(MySql);
            var sqlHelper = new SqlHelper(SqlServer);
            var mysql = string.Format("select * from qdt_deal_msg_list");
            var dt = mySqlHelper.ExecuteDataTable(mysql);
            var sql = "";
            var n = 0;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                sql += "INSERT INTO qdt_deal_msg_list (id,dest,send_type,content,send_time,is_send,create_time,user_id,result,is_success,is_html,title,is_youhui,youhui_id) " +
                       "VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}');\r\n"
                       .FormatWith(dt.Rows[i]["id"], dt.Rows[i]["dest"], Convert.ToInt32(dt.Rows[i]["send_type"]), dt.Rows[i]["content"].ToString().Replace('\'', '\"'), GetTime(dt.Rows[i]["send_time"]), dt.Rows[i]["is_send"], GetTime(dt.Rows[i]["create_time"]), dt.Rows[i]["user_id"], dt.Rows[i]["result"], dt.Rows[i]["is_success"], dt.Rows[i]["is_html"], dt.Rows[i]["title"].ToString().Replace('\'', '\"'), dt.Rows[i]["is_youhui"], dt.Rows[i]["youhui_id"]).Replace("'NULL'", "NULL");
                if ((i + 1) % 10 != 0) continue;
                n += sqlHelper.ExecuteNonQuery(sql);
                sql = "";
            }
            if (!string.IsNullOrWhiteSpace(sql))
            n += sqlHelper.ExecuteNonQuery(sql);
            Console.WriteLine("成功导入deal_msg_list表：" + n + "条数据！");
        }
    }
    
}
