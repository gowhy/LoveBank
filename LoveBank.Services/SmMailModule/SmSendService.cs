/*==============================================================
 * SmSendService.cs
 * Copyright (C) 2014 贵州惠众互联 Inc. All rights reserved.
 *==============================================================
 * Author:   罗应红
 * Date:     2014/03/10 11:02:40.
 * Version:  1.0
 * QQ:		 88962800
*==============================================================*/

using System;
using System.Linq;
using System.Threading;
using QDT.Common.Plugins;
using QDT.Core;
using QDT.Core.Domain;
using QDT.Core.Domain.Enums;

namespace QDT.Services.SmMailModule {
    public class SmSendService : ServiceBase, ISmSendService {
        private readonly ISmsSender _smsSender;

        public SmSendService() {
            _smsSender = new SmsSender();
        }

        #region ISmSendService Members

        public void BussinessSmQueuSend() {
            var thread = new Thread(SendBusinessSmQueue);
            thread.Start();
        }

        public SmsSendResult BusinessSmSend(int id) {
            throw new NotImplementedException();
        }

        public void PromoteSmQueueSend() {
            var thread = new Thread(SendPromoteMsgQueue);
            thread.Start();
        }

        public SmsSendResult PromoteSmSend(int id) {
            var msg = DbProvider.GetByID<PromoteMsgList>(id);
            SmsSendResult result = _smsSender.SendSm(msg.Dest, msg.Content);
            msg.IsSuccess = result.IsSuccess;
            msg.IsSend = true;
            msg.Result = result.Message;
            DbProvider.Update(msg);
            DbProvider.SaveChanges();
            return result;
        }

        public SmsSendResult SendDemo(string phoneNo) {
            return _smsSender.SendSm(phoneNo, "这是一条测试短信！");
        }

        #endregion

        #region private method

        #region bussinessMsg

        private void SendBusinessSmQueue() {
        }

        #endregion

        #region promoteMsg

        private void SendPromoteMsgQueue() {
            while (true) {
                PromoteMsg msg =
                    DbProvider.D<PromoteMsg>().FirstOrDefault(
                        o =>
                        o.InnerMsgType == (int) MsgType.ShortMsg && o.InnerSendStatus != (int) SendStatus.Sended
                        && o.InnerSendStatus != (int) SendStatus.Sending);
                if (msg == null) {
                    break;
                }
                SendPromoteMsg(msg);
            }
        }

        private void SendPromoteMsg(PromoteMsg msg) {
            if (msg.SendType == SendType.MemberGroup) {
                SendPromoteMsgGroup(msg);
                return;
            }
            if (msg.SendType == SendType.Custom) {
                SendPromoteMsgDest(msg);
                return;
            }
        }

        private void SendPromoteMsgGroup(PromoteMsg msg) {
            throw new NotImplementedException();
        }

        private void SendPromoteMsgDest(PromoteMsg msg) {
            string[] destList = msg.SendDefineData.Split(new[] {
                ','
            }, StringSplitOptions.RemoveEmptyEntries);
            if (!destList.Any()) {
                throw new Exception("自定义数据为空");
            }
            msg.SendStatus = SendStatus.Sending;
            DbProvider.Update(msg);
            DbProvider.SaveChanges();
            foreach (string d in destList) {
                var insertData = new PromoteMsgList {
                    Content = msg.Content,
                    CreateTime = DateTime.Now,
                    Dest = d,
                    IsHtml = msg.IsHtml,
                    IsSend = true,
                    IsSuccess = false,
                    MsgId = msg.ID,
                    Result = string.Empty,
                    SendType = MsgType.Email,
                    SendTime = DateTime.Now,
                    Title = msg.Title,
                    UserId = 0
                };
                DbProvider.Add(insertData);
                DbProvider.SaveChanges();
                SmsSendResult result = _smsSender.SendSm(d, msg.Content);

                if (result.IsSuccess) {
                    insertData.IsSuccess = true;
                    insertData.Result = "发送成功";
                } else {
                    insertData.IsSuccess = false;
                    insertData.Result = result.Message;
                }
                DbProvider.Update(insertData);
                DbProvider.SaveChanges();
            }
            msg.SendStatus = SendStatus.Sended;
            DbProvider.Update(msg);
            DbProvider.SaveChanges();
        }

        #endregion

        #endregion
    }
}