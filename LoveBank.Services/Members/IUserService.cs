using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveBank.Core.Domain;
using LoveBank.Core.Members;

namespace LoveBank.Services.Members
{
    public interface IUserService
    {
        /// <summary>
        /// 创建用户，如果成功将返回创建好的用户信息对象
        /// 当使用此方法创建用户时，为了有效保障Email的有效性，建议Email走验证逻辑
        /// </summary>
        /// <param name="groupid">用户组ID</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="email">用户的Email</param>
        /// <param name="isEffect">Bool值，指示是否用户可以登录</param>
        /// <returns></returns>
        /// <exception cref="UserCreateException">创建失败将返回失败异常，失败异常包含一些特殊的状态码</exception>
        User CreateUser(int groupid, string username, string password, string email, bool isEffect);

        /// <summary>
        /// 创建用户，如果成功将返回创建好的用户信息对象
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="email">用户的Email</param>
        /// <param name="isEffect">Bool值，指示是否用户可以登录</param>
        /// <returns></returns>
        /// <exception cref="UserCreateException">创建失败将返回失败异常，失败异常包含一些特殊的状态码</exception>
        User CreateUser(string username, string password, string email,bool isEffect);

        /// <summary>
        /// 创建用户，如果成功将返回创建好的用户信息对象
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="isEffect"></param>
        /// <returns></returns>
        User CreateUser(string username, string password, bool isEffect);

        User CreateOfflineUser(string username, string password, string idCard, string realName, string phone);

        /// <summary>
        /// 验证提供的用户名（邮箱，手机）和密码是否正确，以及用户是否处于有效状态
        /// </summary>
        /// <param name="nameOrEmailOrMobile">用户名，邮箱或手机</param>
        /// <param name="password">密码</param>
        /// <param name="user">如果登录成功返回user引用，否则引用为null</param>
        /// <returns></returns>
        bool ValidateUser(string nameOrEmailOrMobile, string password, out User user);

        /// <summary>
        /// 更新指定的用户信息
        /// </summary>
        /// <param name="user"></param>
        void UpdateUser(User user);

        /// <summary>
        /// 通过用户ID获得用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns>如果存在用户，返回用户信息对象，否则返回null</returns>
        User GetUserByID(int id);

        /// <summary>
        /// 通过用户帐号获取用户
        /// </summary>
        /// <param name="username">用户帐号</param>
        /// <returns>如果存在用户，返回用户信息对象，否则返回null</returns>
        User GetUserByName(string username);

        /// <summary>
        /// 通过邮箱获取用户
        /// </summary>
        /// <param name="email">邮箱</param>
        /// <returns>如果存在用户，返回用户信息对象，否则返回null</returns>
        User GetUserByEmail(string email);

        User GetUserByAll(string nameOrEmailOrMobile);

 

    }
}
