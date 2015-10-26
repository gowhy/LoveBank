using QDT.Core;
using QDT.Core.Members;

namespace QDT.Services.UserModule
{
    public interface IUserService : IServcie
    {
        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        User CreateUser(User user);

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="user"></param>
        void UpdateUser(User user);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        void DeleteUser(int[] id);

        /// <summary>
        /// 彻底删除用户
        /// </summary>
        /// <param name="id"></param>
        void ForeverDelUser(int[] id);

        /// <summary>
        /// 新增用户组别
        /// </summary>
        /// <param name="userGroup"></param>
        /// <returns></returns>
        UserGroup CreateUserGroup(UserGroup userGroup);

        /// <summary>
        /// 更新用户组别
        /// </summary>
        /// <param name="userGroup"></param>
        void UpdateUserGroup(UserGroup userGroup);

        /// <summary>
        /// 彻底删除用户组别
        /// </summary>
        /// <param name="id"></param>
        void ForeverDelUserGroup(int[] id);

    }
}