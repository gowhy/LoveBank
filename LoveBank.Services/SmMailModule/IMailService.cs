/*==============================================================
 * IMailService.cs
 * Copyright (C) 2014 贵州惠众互联 Inc. All rights reserved.
 *==============================================================
 * Author:   罗应红
 * Date:     2014/02/17 11:27:45.
 * Version:  1.0
 * QQ:		 88962800
*==============================================================*/

using System;
using QDT.Core.Domain;
using QDT.Services.DTO;

namespace QDT.Services.SmMailModule {
    /// <summary>
    /// 邮件服务
    /// </summary>
    public interface IMailService {
        //event EventHandler<EmailEventargs> OnMailAdded;
        //event EventHandler OnMailQueueSended;
        //event EventHandler<EmailEventargs> OnMailUpdated;
        /// <summary>
        /// 新增邮件消息
        /// </summary>
        /// <param name="dto"></param>
        void AddEmail(EmailDTO dto);
        /// <summary>
        /// 取得当前正在使用的邮件服务器
        /// </summary>
        /// <returns></returns>
        MailServer GetCurrentMailServer();
        /// <summary>
        /// 编辑邮件
        /// </summary>
        /// <param name="dto"></param>
        void UpdateEmail(EmailDTO dto);
        /// <summary>
        /// 永久删除邮件
        /// </summary>
        /// <param name="ids"></param>
        void DeleteMailForever(int[] ids);
        /// <summary>
        /// 永久删除邮件服务器
        /// </summary>
        /// <param name="ids"></param>
        void DeleteMailServerForEver(int[] ids);
    }
}