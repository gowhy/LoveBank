using System.Linq;
using LoveBank.P2B.Domain.Messages;
using LoveBank.P2B.Domain.Messages.Repository;

namespace LoveBank.Core.MSData.Repositories
{
    public class MessageRepository:EntityRepository<MsgQueue>,IMessageRepository
    {
        public MessageRepository(DB dbContext) : base(dbContext) {
        }

        #region Implementation of IMessageRepository

        public MsgQueue GetNeedSendMessage() {

            var msg=Filter(x => !x.IsSend).OrderByDescending(x => x.Level).FirstOrDefault();

            return msg;

        }

        #endregion
    }
}
