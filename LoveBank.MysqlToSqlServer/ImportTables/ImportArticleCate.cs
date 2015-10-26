using System;
using Helpers;
using QDT.Common;

namespace QDT.MysqlToSqlServer.ImportTables
{
    public class ImportArticleCate : ImportTables
    {
        public void Import()
        {
            var mySqlHelper = new MySqlHelper(MySql);
            var sqlHelper = new SqlHelper(SqlServer);
            var mysql = string.Format("select * from qdt_article_cate");
            var dt = mySqlHelper.ExecuteDataTable(mysql);
            var sql = "";
            var n = 0;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                sql += "INSERT INTO qdt_article_cate (id,title,brief,pid,is_effect,is_delete,type_id,sort) " +
                       "VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}');\r\n"
                       .FormatWith(dt.Rows[i]["id"], dt.Rows[i]["title"], dt.Rows[i]["brief"], dt.Rows[i]["pid"], dt.Rows[i]["is_effect"], dt.Rows[i]["is_delete"], dt.Rows[i]["type_id"], dt.Rows[i]["sort"]).Replace("'NULL'", "NULL");
                if ((i + 1) % 10 != 0) continue;
                n += sqlHelper.ExecuteNonQuery(sql);
                sql = "";
            }
            if (!string.IsNullOrWhiteSpace(sql))
            n += sqlHelper.ExecuteNonQuery(sql);
            Console.WriteLine("成功导入article_cate表：" + n + "条数据！");
        }
    }
}
