using System;
using System.Web;
using System.Web.Mvc;
using LoveBank.Common;
using LoveBank.Common.Plugins.Email;
using LoveBank.P2B.Domain.Config;
using LoveBank.Services;
using LoveBank.Services.Members;
using LoveBank.Core.Members;
using LoveBank.MVC;
using System.Linq;
using LoveBank.P2B.Domain.Messages;
using System.Web.Hosting;
using LoveBank.MVC.Security;
using LoveBank.Services.AdminModule;
using MvcContrib.UI.Grid;
using LoveBank.Core.Domain;
using System.Collections.Generic;
using LoveBank.Web.Admin.Controllers;
using LoveBank.Web.Admin.Models;


namespace LoveBank.Web.Controllers
{
    [SecurityModule(Name = "管理员分组")]
    public class RoleController : BaseController
    {
        protected IAdminService AdminService;
        protected ISecurityService SecurityService;

        public RoleController(IAdminService adminService,ISecurityService securityService)
        {
            Check.Argument.IsNotNull(adminService, "adminService");
            Check.Argument.IsNotNull(securityService, "securityService");

            AdminService = adminService;
            SecurityService = securityService;
        }
        //
        // GET: /Role/
        [SecurityNode(Name = "管理员组列表")]
        public ActionResult Index(int? page, GridSortOptions sort)
        {
            const int pageSize = 10;
            var pageNumber = page ?? 1;

     
            var source = DbProvider.D<Role>().Where(x => !x.IsDelete).OrderBy(x=>x.ID);

            return View(source.ToPagedList(pageNumber - 1, pageSize));
        }

        [SecurityNode(Name = "添加页面")]
        public ActionResult Add()
        {
            var roleModel = CreateEmptyRoleModel();
            return View(roleModel);
        }

        [HttpPost]
        [SecurityNode(Name = "添加执行")]
        public ActionResult PostAdd(RoleEditRole model)
        {
            AdminService.CreateRole(model.Name, model.RoleModule, model.RoleNode, model.IsEffect);
            return Success("管理员组添加成功");
        }

        [SecurityNode(Name = "编辑页面")]
        public ActionResult Edit(int id)
        {
            var role = DbProvider.GetByID<Role>(id);
            if (role == null) return Error("管理员组不存在");

            var roleModel = CreateRoleModel(role, AdminService.GetPermissions(role.ID));
            return View(roleModel);
        }

        [HttpPost]
        [SecurityNode(Name = "编辑操作")]
        public ActionResult PostEdit(RoleEditRole model)
        {
            AdminService.UpdateRole(model.ID,model.Name,model.IsEffect);
           
            AdminService.UpdateRoleAccess(model.ID,model.RoleModule,model.RoleNode);

            return Success("操作成功");
        }

        [SecurityNode(Name = "管理员分组回收站")]
        public ActionResult Trash(int? page, GridSortOptions sort)
        {
            const int pageSize = 10;
            var pageNumber = page ?? 1;

         
            var source = DbProvider.D<Role>().Where(x => x.IsDelete).OrderBy(x=>x.ID);

            return View(source.ToPagedList(pageNumber - 1, pageSize));
        }

        [HttpPost]
        public ActionResult Delete(int[] id)
        {
            AdminService.DelRole(id);
            return Success("操作成功");
        }

        [HttpPost]
        public ActionResult ForeverDel(int[] id)
        {
            AdminService.ForeverDelRole(id);
            return Success("删除成功");
        }

        [HttpPost]
        public ActionResult Restore(int[] id)
        {
            AdminService.RestoreRole(id);
            return Success("恢复成功");
        }

        private RoleModel CreateEmptyRoleModel()
        {
            return CreateRoleModel(null,new PermissionCollection());
        }

        private RoleModel CreateRoleModel(Role role,PermissionCollection permissions)
        {

            var model = role==null?new RoleModel(): new RoleModel
                            {
                                ID = role.ID,
                                Name = role.Name,
                                IsEffect = role.IsEffect
                            };

            IDictionary<string, RoleModuleModel> modules = new Dictionary<string, RoleModuleModel>();

            foreach (var node in SecurityManager.Instance.Nodes)
            {
                var v = node.Value;
                if (!modules.ContainsKey(v.ModuleKey))
                {
                    modules.Add(v.ModuleKey, new RoleModuleModel { Name = v.Module, Value = v.ModuleKey,IsSelect = permissions.HasModule(v)});
                }

                modules[v.ModuleKey].RoleNodes.Add(new RoleNodeModel()
                {
                    IsDisable = false,
                    IsSelect = !modules[v.ModuleKey].IsSelect && permissions.HasNode(v),
                    Name = v.Name,
                    Value = v.NodeKey                    
                });
            }

            model.Modules = modules.Values.ToList();

            return model;
        }


    }
}
