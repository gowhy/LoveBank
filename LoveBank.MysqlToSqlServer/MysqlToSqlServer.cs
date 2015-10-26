using System;
using QDT.MysqlToSqlServer.ToSqlServer;

namespace QDT.MysqlToSqlServer
{
    class MysqlToSqlServer
    {
        static void Main(string[] args)
        {
            new StartImport().Start();
            Console.ReadKey();
        }
    }
}
