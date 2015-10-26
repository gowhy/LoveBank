using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Helpers;
using QDT.Common;

namespace QDT.MysqlToSqlServer.ImportTables
{
    public class ImportDealCate : ImportTables
    {
        public void Import()
        {
            var mySqlHelper = new MySqlHelper(MySql);
            var sqlHelper = new SqlHelper(SqlServer);
            var mysql = string.Format("select * from qdt_deal_cate");
            var dt = mySqlHelper.ExecuteDataTable(mysql);
            var sql = "";
            var n = 0;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                sql += "INSERT INTO qdt_deal_cate (id,name,brief,is_delete,is_effect,sort,uname,icon) " +
                       "VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}');\r\n"
                       .FormatWith(dt.Rows[i]["id"], dt.Rows[i]["name"], dt.Rows[i]["brief"], dt.Rows[i]["is_delete"], dt.Rows[i]["is_effect"], dt.Rows[i]["sort"], dt.Rows[i]["uname"], dt.Rows[i]["icon"]);
                if ((i + 1) % 10 != 0) continue;
                n += sqlHelper.ExecuteNonQuery(sql);
                sql = "";
            }
            if (!string.IsNullOrWhiteSpace(sql))
            n += sqlHelper.ExecuteNonQuery(sql);
            Console.WriteLine("成功导入deal_cate表：" + n + "条数据！");
        }
    }
}
