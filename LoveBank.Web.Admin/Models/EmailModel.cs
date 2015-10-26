﻿
using System;
using LoveBank.Core.Domain;
using LoveBank.Core.Domain.Enums;
using LoveBank.Services.DTO;

namespace LoveBank.Web.Admin.Models {
    public class EmailModel {
     

        public EmailModel() {
        }

        /// <summary>
        /// 标识
        /// </summary>
        public int Id { set; get; }
        public SmsType SmsType { set { InnerSmsType = (int)value; } get { return (SmsType)InnerSmsType; } }
        public int InnerSmsType { private set; get; }
        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { set; get; }
        /// <summary>
        /// 短信内容
        /// </summary>
        public string Content { set; get; }
        /// <summary>
        /// 发送日期
        /// </summary>
        public DateTime SendTime { set; get; }
        /// <summary>
        /// 发送状态
        /// </summary>
        public SendStatus SendStatus { set { InnerSendStatus = (int)value; } get { return (SendStatus)InnerSendStatus; } }

        public int InnerSendStatus { private set; get; }
        /// <summary>
        /// 标Id
        /// <remarks>目前未发现有和标关联的消息，此处作为保留字段，默认值为0</remarks>
        /// </summary>
        public int DealId { set; get; }
        /// <summary>
        /// 发送类型
        /// </summary>
        public SendType SendType { set { InnerSendType = (int)value; } get { return (SendType)InnerSendType; } }

        public int InnerSendType { private set; get; }
        /// <summary>
        /// 发送类型Id
        /// <remarks>
        ///     若发送方式为会员组，则为会员组Id,若发送方式为自定义则为0
        /// </remarks>
        /// </summary>
        public int SendTypeId { set; get; }
        /// <summary>
        /// 手机号或邮件地址
        /// </summary>
        public string SendDefineData { set; get; }
        /// <summary>
        /// 是否支持Html
        /// </summary>
        public bool IsHtml { set; get; } 
        public EmailDTO ToDTO() {
            return new EmailDTO
            {
                Content = Content,
                DealId = DealId,
                Id = Id,
                IsHtml = IsHtml,
                SendDefineData = SendDefineData,
                SendStatus =  (int)SendStatus,
                SendTime =  SendTime,
                SendType =  (int)SendType,
                SendTypeId =  SendTypeId,
                SmsType = (int)SmsType,
                Title =  Title,
                Type = (int)MsgType.Email
            };
        }
    }
}