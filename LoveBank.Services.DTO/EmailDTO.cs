
using System;

namespace LoveBank.Services.DTO {
    public class EmailDTO {
        public int Id { set; get; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public int Type { set; get; }
        /// <summary>
        /// 消息方式
        /// </summary>
        public int SmsType { set; get; }
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
        public int SendStatus { set; get; }

        /// <summary>
        /// 标Id
        /// <remarks>目前未发现有和标关联的消息，此处作为保留字段，默认值为0</remarks>
        /// </summary>
        public int DealId { set; get; }

        /// <summary>
        /// 发送类型
        /// </summary>
        public int SendType { set; get; }

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

        
    }
}