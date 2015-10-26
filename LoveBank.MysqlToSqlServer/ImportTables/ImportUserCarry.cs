using System;
using Helpers;
using QDT.Common;

namespace QDT.MysqlToSqlServer.ImportTables
{
    public class ImportUserCarry : ImportTables
    {
        public void Import()
        {
            var mySqlHelper = new MySqlHelper(MySql);
            var sqlHelper = new SqlHelper(SqlServer);
            var mysql = string.Format("select * from qdt_user_carry");
            var dt = mySqlHelper.ExecuteDataTable(mysql);
            var sql = "";
            var n = 0;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                sql += "INSERT INTO qdt_user_carry (Id,User_Id,Money,Fee,Bank_Id,Bank_Card,Create_Time,Update_Time,Status,Msg,[Desc],Real_Name,Bank_Zone,Region_Lv1,Region_Lv2,Region_Lv3,Region_Lv4) " +
                       "VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}');\r\n"
                       .FormatWith(dt.Rows[i]["Id"], dt.Rows[i]["User_Id"], dt.Rows[i]["Money"], dt.Rows[i]["Fee"], dt.Rows[i]["Bank_Id"], dt.Rows[i]["BankCard"], GetTime(dt.Rows[i]["Create_Time"]), GetTime(dt.Rows[i]["Update_Time"]), 
                       dt.Rows[i]["Status"], dt.Rows[i]["Msg"], dt.Rows[i]["Desc"], dt.Rows[i]["Real_Name"], dt.Rows[i]["BankZone"], dt.Rows[i]["Region_Lv1"], dt.Rows[i]["Region_Lv2"], dt.Rows[i]["Region_Lv3"], dt.Rows[i]["Region_Lv4"]).Replace("'NULL'", "NULL");
                if ((i + 1) % 100 != 0) continue;
                n += sqlHelper.ExecuteNonQuery(sql);
                sql = "";
            }
            if (!string.IsNullOrWhiteSpace(sql))
            n += sqlHelper.ExecuteNonQuery(sql);
            Console.WriteLine("成功导入user_carry表：" + n + "条数据！");
        }
    }
}
