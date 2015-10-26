using System;
using Helpers;

namespace QDT.MysqlToSqlServer.ImportTables
{
    public class ImportArticle : ImportTables
    {

        public void Import()
        {
            var mySqlHelper = new MySqlHelper(MySql);
            var sqlHelper = new SqlHelper(SqlServer);
            var mysql = string.Format("select * from qdt_article");
            var dt = mySqlHelper.ExecuteDataTable(mysql);
            var sql = "";
            var n = 0;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                sql += "INSERT INTO qdt_article (id,title,content,cate_id,create_time,update_time,add_admin_id,is_effect,rel_url,update_admin_id,is_delete,click_count,sort,seo_title,seo_keyword,seo_description,uname,sub_title,brief) " +
                       "VALUES ('" + dt.Rows[i]["id"] + "','" + dt.Rows[i]["title"] + "','" + dt.Rows[i]["content"].ToString().Replace('\'', '\"') + "','" + dt.Rows[i]["cate_id"] + "','" + GetTime(dt.Rows[i]["create_time"]) + "','" + GetTime(dt.Rows[i]["update_time"]) + "','" + dt.Rows[i]["add_admin_id"] + "','" + dt.Rows[i]["is_effect"] + "','" + dt.Rows[i]["rel_url"] + "','" + dt.Rows[i]["update_admin_id"] + "','" + dt.Rows[i]["is_delete"] + "','" + dt.Rows[i]["click_count"] + "','" + dt.Rows[i]["sort"] + "','" + dt.Rows[i]["seo_title"] + "','" + dt.Rows[i]["seo_keyword"] + "','" + dt.Rows[i]["seo_description"] + "','" + dt.Rows[i]["uname"] + "','" + dt.Rows[i]["sub_title"] + "','" + dt.Rows[i]["brief"] + "');\r\n".Replace("'NULL'", "NULL");
                if ((i + 1) % 10 != 0) continue;
                n += sqlHelper.ExecuteNonQuery(sql);
                sql = "";
            }
            if (!string.IsNullOrWhiteSpace(sql))
            n += sqlHelper.ExecuteNonQuery(sql);
            Console.WriteLine("成功导入article表：" + n + "条数据！");
        }

    }
}
