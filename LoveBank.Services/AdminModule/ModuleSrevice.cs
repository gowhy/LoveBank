using System.Collections.Generic;
using System.Linq;
using QDT.Core;
using QDT.Core.Domain;

namespace QDT.Services.AdminModule
{
    public class ModuleSrevice:ServiceBase
    {

        public IDictionary<RoleModule,IList<RoleNode>> GetModules()
        {
            var moduleSet = DbProvider.D<RoleModule>().Where(x=>!x.IsDelete).ToList();
            var nodeSet = DbProvider.D<RoleNode>().Where(x => !x.IsDelete).ToList();
            var result = new Dictionary<RoleModule, IList<RoleNode>>();
            foreach (var module in moduleSet)
            {
                result.Add(module,nodeSet.Where(x=>x.ModuleID==module.ID).ToList());
            }
            return result;
        }

        
    }
}
