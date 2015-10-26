/*==============================================================
 * MailService.cs
 * Copyright (C) 2014 贵州惠众互联 Inc. All rights reserved.
 *==============================================================
 * Author:   罗应红
 * Date:     2014/02/18 15:55:36.
 * Version:  1.0
 * QQ:		 88962800
*==============================================================*/

using System;
using System.Linq;
using QDT.Common;
using QDT.Core;
using QDT.Core.Domain;
using QDT.Core.Domain.Enums;
using QDT.Services.DTO;
using QDT.Services.Email;

namespace QDT.Services.SmMailModule {
    public class MailService:ServiceBase,IMailService {
        public void AddEmail(EmailDTO dto) {
            var entity = dto.ToEntity();
            DbProvider.Add(entity);
            DbProvider.SaveChanges();
            new MailSendService().PromoteMailQueueSend();
            //初始化队列
        }

        public MailServer GetCurrentMailServer() {
            var data = DbProvider.D<MailServer>().FirstOrDefault(o => o.IsEffect);
            return data;
        }

        public void UpdateEmail(EmailDTO dto) {
            var entity = dto.ToEntity();
            var data = DbProvider.D<PromoteMsg>().FirstOrDefault(o=>o.InnerMsgType ==(int)MsgType.Email && o.ID == dto.Id);
            if(data == null) {
                throw new Exception("你所编辑的邮件不存在！请重新选择");
            }
            data.SmsType = entity.SmsType;
            data.IsHtml = entity.IsHtml;
            data.SendTime = entity.SendTime;
            data.Title = entity.Title;
            data.Content = entity.Content;
            data.DealId = entity.DealId;
            data.SendType = entity.SendType;
            data.SendTypeId = entity.SendTypeId;
            data.SendDefineData = entity.SendDefineData;
            DbProvider.Update(data);
            DbProvider.SaveChanges();
            //OnEmailUpdated;
        }

        public void DeleteMailForever(int[] ids) {
            if(!ids.Any()) {
                return;
            }
           foreach(var i in ids) {
               DbProvider.Delete<PromoteMsg>(o=>o.ID == i && o.InnerMsgType == (int)MsgType.Email );
           }
            DbProvider.SaveChanges();
        }

        public void DeleteMailServerForEver(int[] ids) {

            if (!ids.Any())
            {
                return;
            }
            foreach (var i in ids)
            {
                DbProvider.Delete<MailServer>(o => o.ID == i);
            }
            DbProvider.SaveChanges();
        }
    }
}