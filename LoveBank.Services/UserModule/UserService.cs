using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QDT.Common;
using QDT.Core;
using QDT.Core.Members;

namespace QDT.Services.UserModule
{
    public class UserService : ServiceBase, IUserService
    {
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public User CreateUser(User user)
        {
            Check.Argument.IsNotNull(user, "user");

            if (DbProvider.D<User>().Count(x => x.UserName == user.UserName) > 0)
                throw new ApplicationException("用户名已经存在，不能出现相同的组名");

            if (DbProvider.D<User>().Count(x =>x.Email!="" && x.Email == user.Email) > 0)
                throw new ApplicationException("Email已经被使用");

            if (DbProvider.D<User>().Count(x => x.Mobile!="" && x.Mobile == user.Mobile) > 0)
                throw new ApplicationException("手机号已经被使用");

            DbProvider.Add(user);
            DbProvider.SaveChanges();

            return user;
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="email">电子邮件</param>
        /// <param name="mobile">手机号</param>
        /// <returns>当创建失败时候返回null,成功返回User对象</returns>
        public User CreateUser(string userName,string password,string email,string mobile) {
            var user = new User() {
                                  };
            return CreateUser(user);
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(User user)
        {
            if (DbProvider.D<User>().Count(x => x.Mobile!="" && x.Mobile == user.Mobile && x.ID != user.ID) > 0)
                throw new ApplicationException("手机号已经被使用");

            DbProvider.Update(user);
            DbProvider.SaveChanges();
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        public void DeleteUser(int[] id)
        {
            if (id == null) return;

            foreach (var i in id)
            {
                var user = DbProvider.GetByID<User>(i);

                user.IsDelete = true;

            }

            DbProvider.SaveChanges();
        }

        /// <summary>
        /// 彻底删除用户
        /// </summary>
        /// <param name="id"></param>
        public void ForeverDelUser(int[] id)
        {
            if (id == null || id.Count() == 0) return;

            foreach (var i in id)
            {
                var user = DbProvider.GetByID<User>(i);

                if (user == null) continue;

                DbProvider.Delete(user);
            }
            DbProvider.SaveChanges();
        }

        /// <summary>
        /// 新增用户组别
        /// </summary>
        /// <param name="userGroup"></param>
        /// <returns></returns>
        public UserGroup CreateUserGroup(UserGroup userGroup)
        {
            Check.Argument.IsNotNull(userGroup, "userGroup");

            if (DbProvider.D<UserGroup>().Count(x => x.Name == userGroup.Name) > 0)
                throw new ApplicationException("会员组名称已经存在,不能出现相同的组名");

            DbProvider.Add(userGroup);
            DbProvider.SaveChanges();

            return userGroup;
        }

        /// <summary>
        /// 更新用户组别
        /// </summary>
        /// <param name="userGroup"></param>
        public void UpdateUserGroup(UserGroup userGroup)
        {
            if (DbProvider.D<UserGroup>().Count(x => x.Name == userGroup.Name && x.ID != userGroup.ID) > 0)
                throw new ApplicationException("会员组名称已经存在,不能出现相同的组名");

            DbProvider.Update(userGroup);
            DbProvider.SaveChanges();
        }

        /// <summary>
        /// 彻底删除用户组别
        /// </summary>
        /// <param name="id"></param>
        public void ForeverDelUserGroup(int[] id)
        {
            if (id == null || id.Count() == 0) return;

            foreach (var i in id)
            {
                var group = DbProvider.GetByID<UserGroup>(i);

                if (group == null) continue;

                if (DbProvider.D<User>().Count(x => x.GroupID == i) > 0) throw new ApplicationException("{0} 会员组下存在会员".FormatWith(group.Name));

                DbProvider.Delete(group);
            }
            DbProvider.SaveChanges();
        }

    }
}
