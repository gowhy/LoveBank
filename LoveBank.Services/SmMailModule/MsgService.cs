
using System;
using System.Collections.Generic;
using System.Linq;
using LoveBank.Common;
using LoveBank.Common.Plugins;
using LoveBank.Core;
using LoveBank.Core.Domain;
using LoveBank.P2B.Domain.Messages;
using LoveBank.Services.DTO;
using MsgType = LoveBank.Core.Domain.Enums.MsgType;

namespace LoveBank.Services.SmMailModule {
    public class MsgService : ServiceBase,IMsgService {

        //private readonly ISmSendService _smSendService;

        public MsgService() {
        }

        //public MsgService(ISmSendService smSendService) {
        //    Check.Argument.IsNotNull(smSendService,"smSendService");
        //    _smSendService = smSendService;
        //}

        public IList<MsgTemplate> GetAllMsgTemplate() {
            return DbProvider.D<MsgTemplate>().ToList();
        }

        public MsgTemplate QueryMsgTemplateByIdentityName(string identityName) {
            return
                DbProvider.D<MsgTemplate>().FirstOrDefault(
                    o => o.Key.Equals(identityName, StringComparison.InvariantCultureIgnoreCase));
        }

        public MsgTemplate QueryMsgTemplateById(int id) {
            return DbProvider.GetByID<MsgTemplate>(id);
        }

        public void UpdateMsgTemplate(int id, string content, bool isHtml) {
            var msgTmp = DbProvider.GetByID<MsgTemplate>(id);
            if(msgTmp == null) {
                throw new Exception("消息模板不存在");
            }
            msgTmp.IsHtml = isHtml;
            msgTmp.Content = content;
            DbProvider.Update(msgTmp);
            DbProvider.SaveChanges();
        }

        //public void AddPromoteSm(SmDTO dto) {
        //    var entity = dto.ToEnttiy();
        //    DbProvider.Add(entity);
        //    DbProvider.SaveChanges();
        //    var smSendService = IoC.Resolve<ISmSendService>();
        //    smSendService.PromoteSmQueueSend();
        //}



        public void ActiveSms(int id) {
            var data = DbProvider.GetByID<SmServer>(id);
            if(data == null) {
                return;
            }
            var allData = DbProvider.D<SmServer>().Where(o => o.ID != id);
            foreach(var  d in allData) {
                d.IsEffect = false;
                DbProvider.Update(d);
            }
            data.IsEffect = true;
            DbProvider.Update(data);
            DbProvider.SaveChanges();
        }

        //public SmsSendResult SendSignlePromoteMsg(int id) {
        //    var smSendService = IoC.Resolve<ISmSendService>();
        //   return  smSendService.PromoteSmSend(id);
        //}
    }
}