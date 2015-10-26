using LoveBank.Common;
using LoveBank.Common.Data;
using LoveBank.Core;

namespace LoveBank.P2B.Domain{
    /// <summary>
    /// 领域服务基类定义
    /// </summary>
    public abstract class BaseService
    {
        public IDbProvider DbProvider
        {
            get { return IoC.Resolve<IUnitOfWork>() as IDbProvider; }
        }

    }
}
