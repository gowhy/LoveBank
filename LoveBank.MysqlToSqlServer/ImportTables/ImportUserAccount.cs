using System;
using Helpers;
using QDT.Common;

namespace QDT.MysqlToSqlServer.ImportTables
{
    public class ImportUserAccount : ImportTables
    {
        public void Import()
        {

            var sqlHelper = new SqlHelper(SqlServer);
            var mysql = string.Format("select * from qdt_user");
            var dt = sqlHelper.ExecuteDataTable(mysql);
            var sql = "";
            var n = 0;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                sql += "INSERT INTO qdt_user_account (Id,Money,LockMoney,Point,Quota) " +
                       "VALUES ('{0}','{1}','{2}','{3}','{4}');\r\n"
                       .FormatWith(dt.Rows[i]["id"], dt.Rows[i]["money"],dt.Rows[i]["lock_money"], dt.Rows[i]["point"], dt.Rows[i]["quota"]);
                if ((i + 1) % 30 != 0) continue;
                n += sqlHelper.ExecuteNonQuery(sql);
                sql = "";
            }
            if (!string.IsNullOrWhiteSpace(sql))
                n += sqlHelper.ExecuteNonQuery(sql);
            Console.WriteLine("成功导入useraccount表：" + n + "条数据！");
        }
    }
}
