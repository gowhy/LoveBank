using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using LoveBank.Common;
using LoveBank.Common.Data;
using LoveBank.Core;
using LoveBank.Core.Domain;
using LoveBank.Core.Members;

namespace LoveBank.Web
{

    public class FunctionHelper {
        public IDbProvider DbProvider { get { return IoC.Resolve<IUnitOfWork>() as IDbProvider; } }

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
        /// 获取用户名
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public string GetUserName(int id)
        {
            var user = DbProvider.GetByID<User>(id);
            return user == null ? "不存在" :  user.UserName;
        }

   

        public User GetUser(int id)
        {
            return DbProvider.GetByID<User>(id);
        }


        public string SubString(string str, int length=16)
        {
            if (str.Length < length) return str;
            return str.Substring(0, length-2) + "...";
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