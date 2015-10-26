using System;
using QDT.MysqlToSqlServer.ImportTables;

namespace QDT.MysqlToSqlServer.ToSqlServer
{
    public class StartImport
    {
        public void Start()
        {
//            new ImportAdmin().Import();
//            new ImportArticle().Import();
//            new ImportArticleCate().Import();
//            new ImportBank().Import();
//            new ImportDeal().Import();
//            new ImportDealCate().Import();
//            new ImportDealLoad().Import();
//            new ImportDealLoadRepay().Import();
//            new ImportDealLoanType().Import();
//            new ImportDealMsgList().Import();
//            new ImportDealRepay().Import();
//            new ImportRole().Import();
//            new ImportRoleAccess().Import();
//            new ImportUserGroup().Import();
//            new ImportUserLog().Import();
//            new ImportUser().Import();
//            new ImportUserSta().Import();
//            new ImportUserCarry().Import();
//            new ImportChargeOrder().Import();
//            new ImportPaymentNotice().Import();
            new ImportUserAccount().Import();
            Console.WriteLine("数据迁移成功！");

        }
    }
}
