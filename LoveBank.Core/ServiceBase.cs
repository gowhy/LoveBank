using LoveBank.Common;
using LoveBank.Common.Data;
using LoveBank.Core.Domain;
using System.Linq;

namespace LoveBank.Core
{
    public interface IServcie
    {
        /// <summary>
        /// 获得数据持久化操作对象
        /// </summary>
        IDbProvider DbProvider { get; }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        void UpdateEntity(object entity);
    }

    public abstract class ServiceBase:IServcie
    {
        public IDbProvider DbProvider
        {
            get { return IoC.Resolve<IUnitOfWork>() as IDbProvider; }
        }

        public void UpdateEntity(object entity)
        {
            DbProvider.Update(entity);
            DbProvider.SaveChanges();
        }

    }
}
