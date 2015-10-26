using System;
using System.Linq;
using LoveBank.Common;
using LoveBank.Core;
using LoveBank.Core.Domain;
using System.Collections.Generic;
using LoveBank.MVC.Security;

namespace LoveBank.Services.AdminModule
{
    public class AdminService : ServiceBase, IAdminService, ISecurityService
    {

        /// <summary>
        /// 验证管理员
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public virtual bool Validate(string userName, string password)
        {
            AdminUser admin = null;
            return Validate(userName, password, out admin);
        }

        /// <summary>
        /// 验证管理员
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="admin"></param>
        /// <returns></returns>
        public virtual bool Validate(string userName, string password, out AdminUser admin)
        {
            var validate = false;
            admin = null;

            admin = DbProvider.D<AdminUser>().FirstOrDefault(x => x.UserName == userName);

            if (admin != null)
            {
                if (admin.Password == password)
                {
                    validate = true;
                }
            }
            if (validate)
            {
                admin.LoginTime = DateTime.Now;
                DbProvider.SaveChanges();
            }
            return validate;
        }

        /// <summary>
        /// 获得角色列表
        /// </summary>
        /// <returns></returns>
        public IList<Role> GetRoles()
        {
            return DbProvider.D<Role>().ToList();
        }

        /// <summary>
        /// 获得回收站的角色列表
        /// </summary>
        /// <returns></returns>
        public IList<Role> GetTrashRoles()
        {
            return DbProvider.D<Role>().Where(x => x.IsDelete).ToList();
        }

        /// <summary>
        /// 建立一个新的角色
        /// </summary>
        /// <param name="name"></param>
        /// <param name="modules"></param>
        /// <param name="nodes"></param>
        /// <param name="isEffect"> </param>
        /// <returns></returns>
        public Role CreateRole(string name, string[] modules, string[] nodes, bool isEffect = true)
        {
            if(ExistRole(name)) throw new ApplicationException("管理员分组已经存在");

            var role = new Role
            {
                Name = name.Trim(),
                Accesses = new List<RoleAccess>(),
                IsDelete = false,
                IsEffect = isEffect
            };

            AddModulesForRole(role, modules);
            AddNodesForRole(role, nodes);

            DbProvider.Add(role);
            DbProvider.SaveChanges();

            return role;
        }
        
        /// <summary>
        /// 更新管理员分组
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="roleName"></param>
        /// <param name="isEffect"></param>
        public void UpdateRole(int roleId,string roleName,bool isEffect)
        {
            var role = DbProvider.GetByID<Role>(roleId);

            if(role==null) throw new ApplicationException("管理员分组不存在");

            if(DbProvider.D<Role>().Count(x=>x.ID!=roleId&&x.Name==roleName)>0) throw new ApplicationException("管理员分组名称已经存在");

            role.Name = roleName;
            role.IsEffect = isEffect;

            DbProvider.SaveChanges();
        }

        /// <summary>
        /// 为角色添加基于模块的访问权限
        /// </summary>
        /// <param name="role"></param>
        /// <param name="modules"></param>
        private static void AddModulesForRole(Role role, IEnumerable<string> modules)
        {
            foreach (var mid in modules??new string[]{})
            {
                role.Accesses.Add(new RoleAccess { RoleId = role.ID,Module = mid, Node = string.Empty });
            }
        }

        /// <summary>
        /// 为角色添加基于节点的访问权限
        /// </summary>
        /// <param name="role"></param>
        /// <param name="nodes"></param>
        private static void AddNodesForRole(Role role, IEnumerable<string> nodes)
        {
            foreach (var nid in nodes ?? new string[] { })
            {
                role.Accesses.Add(new RoleAccess { RoleId =role.ID ,Module = string.Empty, Node = nid });
            }
        }

        /// <summary>
        /// 更新角色的访问权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="modules"></param>
        /// <param name="nodes"></param>
        public void UpdateRoleAccess(int roleid, string[] modules, string[] nodes)
        {
            var role=DbProvider.GetByID<Role>(roleid);
            if (role == null) return;

            DbProvider.Delete<RoleAccess>(x=>x.RoleId==roleid);

            AddModulesForRole(role, modules);
            AddNodesForRole(role, nodes);
            
            DbProvider.SaveChanges();
        }

        /// <summary>
        /// 清空角色的所有访问权限
        /// </summary>
        /// <param name="roleid"></param>
        public void ClearRoleAccess(int roleid)
        {
            var role = DbProvider.D<Role>().FirstOrDefault(x => x.ID == roleid);
            if (role != null)
            {
                role.Accesses.Clear();
            }
        }

        /// <summary>
        /// 废弃角色
        /// </summary>
        /// <param name="roleid"></param>
        public void TrashRole(int roleid)
        {
            var role = DbProvider.D<Role>().FirstOrDefault(x => x.ID == roleid);
            if (role != null)
            {
                role.IsDelete = true;
                DbProvider.SaveChanges();
            }
        }

        /// <summary>
        /// 恢复角色
        /// </summary>
        /// <param name="roleid"></param>
        public void RecoverRole(int roleid)
        {
            var role = DbProvider.D<Role>().FirstOrDefault(x => x.ID == roleid);
            if (role != null)
            {
                role.IsDelete = false;
                DbProvider.SaveChanges();
            }
        }

        /// <summary>
        /// 彻底移除角色
        /// </summary>
        /// <param name="roleid"></param>
        public void RemoveRole(int roleid)
        {
            var role = DbProvider.D<Role>().FirstOrDefault(x => x.ID == roleid);
            if (role != null)
            {
                role.Accesses.Clear();
                DbProvider.Delete(role);
                DbProvider.SaveChanges();
            }
        }

        /// <summary>
        /// 创建管理员
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public AdminUser CreateAdmin(AdminUser admin)
        {
            Check.Argument.IsNotNull(admin, "admin");

            DbProvider.Add(admin);
            DbProvider.SaveChanges();

            return admin;
        }

        /// <summary>
        /// 管理员是否存在
        /// </summary>
        /// <param name="adminName">管理员帐号</param>
        /// <returns></returns>
        public bool ExistAdmin(string adminName)
        {
            return DbProvider.D<AdminUser>().Count(x => x.UserName == adminName) > 0;
        }

        /// <summary>
        /// 角色是否存在
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public bool ExistRole(string roleName)
        {
            return DbProvider.D<Role>().Count(x => x.Name == roleName) > 0;
        }

        /// <summary>
        /// 获得管理员
        /// </summary>
        /// <param name="name">管理员帐号名称</param>
        /// <returns></returns>
        public AdminUser GetAdmin(string name)
        {
            return DbProvider.D<AdminUser>().FirstOrDefault(x => x.UserName == name);
        }

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="id"></param>
        public void DelAdmin(int[] id)
        {
            if (id == null) return;

            foreach (var i in id)
            {
                var admin = DbProvider.GetByID<AdminUser>(i);
                
                if (admin == null) continue;
                
                //if(admin.IsDefaultAdmin) throw new ApplicationException("{0}系统管理员不能删除".FormatWith(admin.Name));

                //admin.IsDelete = true;
            }
            DbProvider.SaveChanges();

        }

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="id"></param>
        public void DelAdmin(int id)
        {
            DelAdmin(new[]{id});
        }

        /// <summary>
        /// 删除管理员组
        /// </summary>
        /// <param name="id"></param>
        public void DelRole(int[] id)
        {
            if (id == null) return;

            foreach (var i in id)
            {
                var role = DbProvider.GetByID<Role>(i);

                if (DbProvider.D<AdminUser>().Count(x => x.RoleID == i) > 0) throw new ApplicationException("{0}存在管理员".FormatWith(role.Name));

                role.IsDelete = true;
            }

            DbProvider.SaveChanges();
        }

        /// <summary>
        /// 彻底删除管理员
        /// </summary>
        /// <param name="id"></param>
        public void ForeverDelAdmin(int[] id)
        {
            if (id == null) return;

            foreach (var i in id)
            {
                var admin = DbProvider.GetByID<AdminUser>(i);

                if (admin == null) continue;

                if (admin.IsDefaultAdmin) throw new ApplicationException("{0}系统管理员不能删除");

                DbProvider.Delete(admin);
            }
            DbProvider.SaveChanges();
        }

        /// <summary>
        /// 彻底删除角色
        /// </summary>
        /// <param name="id"></param>
        public void ForeverDelRole(int[] id)
        {
            if (id == null) return;
            foreach (var roleId in id)
            {

                var id1 = roleId;
                DbProvider.Delete<RoleAccess>(x => x.RoleId == id1);

                DbProvider.Delete<Role>(x => x.ID == id1);
            }
            DbProvider.SaveChanges();
        }

        /// <summary>
        /// 从垃圾站恢复管理员
        /// </summary>
        /// <param name="id"></param>
        public void RestoreAdmin(int[] id)
        {
            if (id == null) return;

            foreach (var i in id)
            {
                var admin = DbProvider.GetByID<AdminUser>(i);

                if (admin == null) continue;

                if (admin.IsDefaultAdmin) throw new ApplicationException("{0}系统管理员不能删除");

                //admin.IsDelete = false;
            }
            DbProvider.SaveChanges();
        }

        /// <summary>
        /// 从垃圾站恢复管理员组
        /// </summary>
        /// <param name="id"></param>
        public void RestoreRole(int[] id)
        {
            if (id == null) return;

            foreach (var i in id)
            {
                var role = DbProvider.GetByID<Role>(i);

                role.IsDelete = false;
            }
            DbProvider.SaveChanges();
        }

        /// <summary>
        /// 设置默认管理员
        /// </summary>
        /// <param name="adminId"></param>
        public void SetDefaultAdmin(int adminId)
        {
            var newAdmin = DbProvider.D<AdminUser>().FirstOrDefault(x => x.ID == adminId);

            if(newAdmin==null) throw new ArgumentNullException("管理员不存在或者已经被删除");

            var oldDefault = DbProvider.D<AdminUser>().Where(x => x.IsDefaultAdmin);
            
            oldDefault.Each(x=>x.IsDefaultAdmin=false);

            newAdmin.IsDefaultAdmin = true;

            DbProvider.SaveChanges();

        }

        /// <summary>
        /// 获得角色的权限点
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public PermissionCollection GetPermissions(int role)
        {
            var permissions = new PermissionCollection();

            DbProvider.D<RoleAccess>().Where(x => x.RoleId == role).ToList().ForEach(access => permissions.Add(new Permission { ModuleKey = access.Module, NodeKey = access.Node }));

            return permissions;
        }

        /// <summary>
        /// 获得用户的权限点
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public PermissionCollection GetPermissions(string userName)
        {
            var admin = DbProvider.D<AdminUser>().FirstOrDefault(x => x.UserName == userName);
            return admin == null ? new PermissionCollection() : GetPermissions(admin.RoleID);
        }
    }
}
