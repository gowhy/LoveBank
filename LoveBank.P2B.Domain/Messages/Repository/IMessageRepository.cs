using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LoveBank.Common.Data;

namespace LoveBank.P2B.Domain.Messages.Repository
{
    public interface IMessageRepository:IRepository<MsgQueue> {
        
        MsgQueue GetNeedSendMessage();
    }
}
