using System;
using LoveBank.Core;

namespace LoveBank.P2B.Domain.Messages
{
    public class MsgQueue : Entity {


        public MsgQueue(){}

        public MsgQueue(string dest,int userId) {
            Dest = dest;
            UserId = userId;
            CreateTime = DateTime.Now;
            SendTime = DateTime.Now;
            Title = "";
        }

        /// <summary>
        /// 高优先级
        /// </summary>
        public const int Level_Hight = 2;

        /// <summary>
        /// 中等优先级
        /// </summary>
        public const int Level_Middle = 1;

        /// <summary>
        /// 低优先级
        /// </summary>
        public const int Level_Low = 0;

        /// <summary>
        /// 优先级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public MsgType Type { set { InnerType = (int)value; } get { return (MsgType) InnerType; } }

        public int InnerType { private set; get; }

        /// <summary>
        /// 手机号或邮件地址
        /// </summary>
        public string Dest { private set; get; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { private set; get; }

        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { set; get; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { set; get; }

        /// <summary>
        /// 是否发送
        /// </summary>
        public bool IsSend { set; get; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { set; get; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { private set; get; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { set; get; }

        /// <summary>
        ///  发送结果
        /// </summary>
        public string Result { set; get; }

        public void UpdateSend() {

            IsSend = true;

            SendTime = DateTime.Now;

        }


    }
}