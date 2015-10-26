using System;
using System.Linq;
using LoveBank.Cache;
using LoveBank.Core;
using LoveBank.Core.Members;
using LoveBank.Common;

namespace LoveBank.Services.Members
{
    public partial class UserService : ServiceBase, IUserService
    {

        public User CreateUser(int groupid,string username, string password, string email, bool isEffect)
        {
            if (DbProvider.D<User>().Count(x => x.UserName == username) > 0) throw new UserCreateException(UserCreateException.DuplicateUserName);

            //if (DbProvider.D<User>().Count(x => x.Email == email) > 0) throw new UserCreateException(UserCreateException.DuplicateEmail);

            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("password","密码不能为空. ");

            //var group=DbProvider.GetByID<UserGroup>(groupid) ?? DbProvider.GetByID<UserGroup>(3);

            User user = new User();
            user.UserName = username;
            user.Password = password;
            user.AddTime = DateTime.Now;
        
            DbProvider.Add(user);
            
            DbProvider.SaveChanges();

            return user;
        }

        public User CreateUser(string username, string password, string email, bool isEffect)
        {
            return CreateUser(0, username, password, email, isEffect);
        }

        public User CreateUser(string username, string password, bool isEffect)
        {
            //if (DbProvider.D<User>().Count(x => x.UserName == username) > 0) throw new UserCreateException(UserCreateException.DuplicateUserName);

            //if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("password", "密码不能为空. ");

            //var group = DbProvider.GetByID<UserGroup>(0) ?? DbProvider.GetByID<UserGroup>(3);

            //var user = new User(group, username, "", password, isEffect);

            //DbProvider.Add(user);
            //DbProvider.SaveChanges();

            //return user;
            return new User();
        }

        public User CreateOfflineUser(string username, string password, string idCard, string realName, string phone)
        {
            //if (DbProvider.D<User>().Count(x => x.UserName == username) > 0) throw new UserCreateException(UserCreateException.DuplicateUserName);

            //if (DbProvider.D<User>().Count(x => x.Email == username + "@qdt.cc") > 0) throw new UserCreateException(UserCreateException.DuplicateEmail);

            //if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException("password", "密码不能为空. ");

            //var group = DbProvider.GetByID<UserGroup>(1) ?? DbProvider.GetByID<UserGroup>(1);

            //var user = new User(group, username, username + "@qdt.cc", password);

            //user.BindIDCard(idCard, realName);
            //user.GroupID = 1;
            //user.BindMobile(phone);

            //DbProvider.Add(user);

            //DbProvider.SaveChanges();

            //return user;
            return new User();
        }

        /// <summary>
        /// 验证用户
        /// </summary>
        /// <param name="nameOrEmailOrMobile"></param>
        /// <param name="password"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool ValidateUser(string nameOrEmailOrMobile, string password,out User user)
        {
            //Check.Argument.IsNotEmpty(nameOrEmailOrMobile,"nameOrEmailOrMobile");

            //var member = DbProvider.D<User>().FirstOrDefault(x => x.UserName == nameOrEmailOrMobile || x.Email == nameOrEmailOrMobile || x.Mobile == nameOrEmailOrMobile);

            //if ( member == null || member.Password!=password.Hash() || member.IsBorrower)
            //{
            //    user = null;
            //    return false;
            //}

            //user = member;

            //if (!member.IsEffect || member.IsDelete)
            //{
            //    return false;
            //}
            user = new User();
            return true;
            
        }

        public void UpdateUser(User user)
        {
            Check.Argument.IsNotNull(user, "user");

            DbProvider.Update(user);
            DbProvider.SaveChanges();
        }

        public User GetUserByID(int id)
        {
            return DbProvider.GetByID<User>(id);
        }

        public User GetUserByName(string username)
        {
            var cache = LoveBankCache.GetCacheService();
            var user = cache.RetrieveObject(CacheKeys.USERBYNAME.FormatWith(username.Hash())) as User;
            if (user == null)
            {
                user = DbProvider.NoTrack<User>().FirstOrDefault(x => x.UserName == username);
                cache.AddObject(CacheKeys.USERBYNAME.FormatWith(username.Hash()), user, 3);
            }
            return user;
        }

        public User GetUserByEmail(string email)
        {
            return new User();
            //return DbProvider.D<User>().FirstOrDefault(x => x.Email == email && x.EmailPassed);
        }

        public User GetUserByAll(string nameOrEmailOrMobile)
        {
            return new User();
            //return DbProvider.D<User>().FirstOrDefault(x => x.UserName == nameOrEmailOrMobile || x.Email == nameOrEmailOrMobile || x.Mobile == nameOrEmailOrMobile);
        }


  
  
    }
}
