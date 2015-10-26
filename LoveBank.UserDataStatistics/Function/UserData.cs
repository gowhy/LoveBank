using Crawl.UserDataStatistics.Common;
using Crawl.UserDataStatistics.Helper;

namespace Crawl.UserDataStatistics.Function
{
    public class UserData : Base
    {
        public void Run()
        {
            var sqlHelper = new SqlHelper(SqlServer);
            var money = sqlHelper.ExecuteScalar<decimal>("select SUM(ac.Money) from qdt_user as us, qdt_user_account as ac where us.UserAccountID=ac.Id and us.group_id='3';");
            var lockMoney = sqlHelper.ExecuteScalar<decimal>("select SUM(ac.LockMoney) from qdt_user as us, qdt_user_account as ac where us.UserAccountID=ac.Id and us.group_id='3';");
            var chargeMoney =
                sqlHelper.ExecuteScalar<decimal>(
                    "select SUM(od.pay_amount) from qdt_user as us, qdt_charge_order as od where us.id=od.user_id and us.group_id='3' and od.update_time>='{0}' and od.update_time<'{1}';"
                        .FormatWith(StartTime, EndTime));
            var carryMoney =
                sqlHelper.ExecuteScalar<decimal>(
                    "select SUM(ca.Money) from qdt_user as us, qdt_user_carry as ca where us.id=ca.user_id and us.group_id='3' and ca.Status='1' and ca.update_time>='{0}' and ca.update_time<'{1}';"
                        .FormatWith(StartTime, EndTime));
            var carryLock =
                sqlHelper.ExecuteScalar<decimal>(
                    "select SUM(ca.Money) from qdt_user as us, qdt_user_carry as ca where us.id=ca.user_id and us.group_id='3' and ca.Status='0' and ca.Create_Time>='{0}' and ca.Create_Time<'{1}';"
                        .FormatWith(StartTime, EndTime));
            var investLock =
                sqlHelper.ExecuteScalar<decimal>(
                    "select SUM(cr.Money) from qdt_user as us, qdt_credit as cr where us.id=cr.UserID and us.group_id='3' and AssignmentID='0' and (cr.Status='1' or cr.Status='2') and cr.CreateTime>='{0}' and cr.CreateTime<'{1}';"
                        .FormatWith(StartTime, EndTime));
            var repayMoney = sqlHelper.ExecuteScalar<decimal>(
                "SELECT SUM(re.RepayMoney) FROM qdt_credit_repayment as re, qdt_credit as cr, qdt_user as us where re.TrueRepayTime>='{0}' AND re.TrueRepayTime<'{1}' AND re.CreditId=cr.Id AND cr.UserID=us.id AND us.group_id='3';"
                .FormatWith(StartTime, EndTime));
            var insertSql =
                "insert into qdt_user_money (money,chargemoney,carrymoney,lockmoney,carrylock,investlock,repaymoney) values ('{0}','{1}','{2}','{3}','{4}','{5}',{6});"
                    .FormatWith(money, chargeMoney, carryMoney, lockMoney, carryLock, investLock,repayMoney);
            sqlHelper.ExecuteNonQuery(insertSql);
        }
    }
}
