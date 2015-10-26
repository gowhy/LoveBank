using System.Linq;
using System.Web.Mvc;
using LoveBank.Common;
using LoveBank.Core;
using LoveBank.Common.Data;
using LoveBank.Core.Domain;
using LoveBank.Core.Members;
using LoveBank.Services.Payments;


namespace LoveBank.Web.Admin
{

    public class FunctionHelper {
        public IDbProvider DbProvider { get { return IoC.Resolve<IUnitOfWork>() as IDbProvider; } }

        public string GetRoleName(int roleId) {
            var role = DbProvider.D<Role>().FirstOrDefault(x => x.ID == roleId);

            return role != null ? role.Name : "未分配";
        }

        /// <summary>
        /// 获得用户姓名
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUserName(int userId)
        {
            var user = DbProvider.GetByID<User>(userId);
            return user != null ? user.UserName : "-";
        }

        public string GetUserPhone(int userId)
        {
            var user = DbProvider.GetByID<User>(userId);
            return user.Phone ?? "";
        }

    

        public string GetAdminUserName(int adminID) {

            var adminUser = DbProvider.GetByID<AdminUser>(adminID);

            return adminUser == null ? "管理员不存在" : adminUser.UserName;
        }

        /// <summary>
        /// 获得会员组名称
        /// </summary>
        /// <param name="groupId">会员组ID</param>
        /// <returns></returns>
        public string GetUserGroup(int groupId)
        {
            var group = DbProvider.GetByID<UserGroup>(groupId);
            return group != null ? group.Name : "";
        }


       

        /// <summary>
        /// 获取地区
        /// </summary>
        /// <param name="id">地区ID</param>
        /// <returns></returns>
        public string GetRegionById(int id)
        {
            var region = DbProvider.GetByID<RegionConfig>(id);

            return region == null ? "不存在" : region.Name;
        }

       
        /// <summary>
        /// 获得地址字符窜
        /// </summary>
        /// <param name="lv1"></param>
        /// <param name="lv2"></param>
        /// <param name="lv3"></param>
        /// <param name="lv4"></param>
        /// <returns></returns>
        public string GetRegionAddr(int lv1,int lv2,int lv3,int lv4) {
            return GetRegionById(lv2) + " " + GetRegionById(lv3);
        }

        public string GetCarryStatus(int status)
        {
            var result = "";

            switch (status)
            {
                case 0 :
                    result = "未处理";
                    break;
                case 1:
                    result = "申请通过";
                    break;
                case 2:
                    result = "申请驳回";
                    break;
            }

            return result;
        }

   
        public string GetPaymentInfo(string key)
        {
            var p = IoC.Resolve<IPaymentService>().GetPayments().FirstOrDefault(x => x.Key == key);
            return p == null ? "不存在" : p.Name;
        }

    }

    public static class FunctionHelperExntensions
    {
        private static readonly FunctionHelper functionHelper=new FunctionHelper();

        public static FunctionHelper F(this HtmlHelper page)
        {
            return functionHelper;
        }
    }
}