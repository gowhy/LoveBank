using System.Collections.Generic;
using LoveBank.Core;
using LoveBank.Core.Domain;
using LoveBank.MVC.Security;

namespace LoveBank.Services.AdminModule
{
    public interface IAdminService : IServcie
    {
        /// <summary>
        /// 创建管理员
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        AdminUser CreateAdmin(AdminUser admin);

        /// <summary>
        /// 管理员是否存在
        /// </summary>
        /// <param name="adminName">管理员帐号</param>
        /// <returns></returns>
        bool ExistAdmin(string adminName);

        /// <summary>
        /// 验证管理员
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool Validate(string userName, string password);

        /// <summary>
        /// 验证管理员
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="admin"></param>
        /// <returns></returns>
        bool Validate(string userName, string password, out AdminUser admin);

        /// <summary>
        /// 获得角色列表
        /// </summary>
        /// <returns></returns>
        IList<Role> GetRoles();

        /// <summary>
        /// 获得回收站的角色列表
        /// </summary>
        /// <returns></returns>
        IList<Role> GetTrashRoles();

        /// <summary>
        /// 建立一个新的角色
        /// </summary>
        /// <param name="name"></param>
        /// <param name="modules"></param>
        /// <param name="nodes"></param>
        /// <param name="isEffect"></param>
        /// <returns></returns>
        Role CreateRole(string name, string[] modules, string[] nodes, bool isEffect = true);

        /// <summary>
        /// 更新管理员分组
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="roleName"></param>
        /// <param name="isEffect"></param>
        void UpdateRole(int roleId, string roleName, bool isEffect);

        /// <summary>
        /// 更新角色的访问权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="modules"></param>
        /// <param name="nodes"></param>
        void UpdateRoleAccess(int roleid, string[] modules, string[] nodes);

        /// <summary>
        /// 清空角色的所有访问权限
        /// </summary>
        /// <param name="roleid"></param>
        void ClearRoleAccess(int roleid);

        /// <summary>
        /// 废弃角色
        /// </summary>
        /// <param name="roleid"></param>
        void TrashRole(int roleid);

        /// <summary>
        /// 恢复角色
        /// </summary>
        /// <param name="roleid"></param>
        void RecoverRole(int roleid);

        /// <summary>
        /// 彻底移除角色
        /// </summary>
        /// <param name="roleid"></param>
        void RemoveRole(int roleid);

        /// <summary>
        /// 获得管理员
        /// </summary>
        /// <param name="name">管理员帐号名称</param>
        /// <returns></returns>
        AdminUser GetAdmin(string name);

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="id"></param>
        void DelAdmin(int[] id);

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="id"></param>
        void DelAdmin(int id);

        /// <summary>
        /// 删除管理员组
        /// </summary>
        /// <param name="id"></param>
        void DelRole(int[] id);

        /// <summary>
        /// 彻底删除管理员
        /// </summary>
        /// <param name="id"></param>
        void ForeverDelAdmin(int[] id);

        /// <summary>
        /// 彻底删除角色
        /// </summary>
        /// <param name="id"></param>
        void ForeverDelRole(int[] id);

        /// <summary>
        /// 从垃圾站恢复管理员
        /// </summary>
        /// <param name="id"></param>
        void RestoreAdmin(int[] id);

        /// <summary>
        /// 从垃圾站恢复管理员组
        /// </summary>
        /// <param name="id"></param>
        void RestoreRole(int[] id);

        /// <summary>
        /// 设置默认管理员
        /// </summary>
        /// <param name="adminId"></param>
        void SetDefaultAdmin(int adminId);

        /// <summary>
        /// 获得角色的权限点
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        PermissionCollection GetPermissions(int role);
    }
}