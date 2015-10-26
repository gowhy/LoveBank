
using System.Collections.Generic;
using LoveBank.Common.Plugins;
using LoveBank.Core.Domain;
using LoveBank.P2B.Domain.Messages;
using LoveBank.Services.DTO;

namespace LoveBank.Services.SmMailModule {
    public interface IMsgService {
        /// <summary>
        /// 取得所有消息模版
        /// </summary>
        /// <returns></returns>
        IList<MsgTemplate> GetAllMsgTemplate();
        /// <summary>
        /// 根据标识名取得相应消息模版
        /// </summary>
        /// <param name="identityName"></param>
        /// <returns></returns>
        MsgTemplate QueryMsgTemplateByIdentityName(string identityName);
        /// <summary>
        /// 根据Id取得相应消息模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MsgTemplate QueryMsgTemplateById(int id);
        /// <summary>
        /// 更新模板
        /// </summary>
        /// <param name="id">模板Id</param>
        /// <param name="content">模板内容</param>
        /// <param name="isHtml">是否支持Html</param>
        void UpdateMsgTemplate(int id, string content, bool isHtml);
    
    
    
        /// <summary>
        /// 激活短信接口，并将其它置为不可用
        /// </summary>
        /// <param name="id"></param>
        void ActiveSms(int id);
        ///// <summary>
        ///// 发送单条推广短信
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //SmsSendResult SendSignlePromoteMsg(int id);

    }
}