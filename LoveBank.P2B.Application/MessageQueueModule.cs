using System;
using System.Linq;
using System.Threading;
using System.Web;
using Crawl.Common;
using Crawl.Common.Data;
using Crawl.Common.Plugins.Email;
using Crawl.Common.Plugins.Sms;
using Crawl.Core;
using Crawl.P2B.Domain.Config;
using Crawl.P2B.Domain.Messages;
using Crawl.P2B.Domain.Messages.Repository;
using Crawl.Services;
using Crawl.Services.Deals;
using log4net;

namespace Crawl.P2B.Application {
    public class MessageQueueModule : IHttpModule {
        private static Timer eventTimer;
        private static Timer autoBidTimer;
        #region Implementation of IHttpModule

        private IMessageRepository MsgRepository { get { return DbProvider().Repository<MsgQueue>() as IMessageRepository; } }

        public void Init(HttpApplication context) {
            ILog log = LogManager.GetLogger("Loger");
            log.Info("初始化MessageQueueModule");
            try {
                var msgConfig = SettingManager.Get<MessageConfig>();

                if (eventTimer == null && msgConfig.Enable) {
                    eventTimer = new Timer(MessageEventWorkCallback, null, 1000, msgConfig.DueTime);
                    log.Info("队列启动");
                }
                
            }
            catch (Exception ex) {
                log.Error(ex);
            }
        }

        public void Dispose() {
            eventTimer = null;
        }

        private static int _autoTime = 0;

        private void MessageEventWorkCallback(object info) {
            ThreadPool.QueueUserWorkItem(state => Execute());

            if (!SettingManager.Get<MessageConfig>().AutoBid) return;

            if (_autoTime == 2)
            {
                ThreadPool.QueueUserWorkItem(state => AutoBid());

                Interlocked.Exchange(ref _autoTime, 0);
            }
            else
            {
                Interlocked.Increment(ref _autoTime);
            }
        }

        public class ProjectForAuto {
            public int ID { get; set; }
            public int Month { get; set; }
            public double Rate { get; set; }
        }

        public class AutoBidValue {
            public int ID { get; set; }
            public int UserID { get; set; }
            public decimal LoadMoney { get; set; }
            public decimal MinLoadMoney { get; set; }
            public decimal Money { get; set; }
        }

        private void AutoBid() {

        var db = IoC.Resolve<IUnitOfWork>() as IDbProvider;

        if (db == null) return;

        var projects = db.QuerySql<ProjectForAuto>("select ID,repay_time as Month,Rate from dbo.qdt_deal where deal_status=1 and is_delete=0 and is_effect=1 and DATEDIFF(mi,start_time,getdate())>5 and (load_money/borrow_amount)<0.9").ToList();

        if(!projects.Any()) return;

            foreach (var projectForAuto in projects)
            {
                var sql = "select top 1 b.id as ID,b.user_id as UserID,b.load_money as LoadMoney,b.min_load_money as MinLoadMoney,(a.money-b.retain_money) as Money from dbo.qdt_user u inner join dbo.qdt_user_autobid b on u.id=b.user_id inner join dbo.qdt_user_account a on u.UserAccountID=a.id where (a.money-b.retain_money)>b.min_load_money and b.deal_date_upper_limit>={0} and b.money_rate_lower_limit<={1} order by b.LastTime".FormatWith(projectForAuto.Month, projectForAuto.Rate);

                var auto = db.QuerySql<AutoBidValue>(sql).FirstOrDefault();

                if(auto==null) continue;

                var filterSql = "select id from qdt_credit where UserID='{0}' and ProjectID='{1}' and IsAuto='1';".FormatWith(auto.UserID, projectForAuto.ID);

                var isBid = db.QuerySql<int>(filterSql).Count();

                if (isBid != 0) continue;

                var loadMoney = auto.Money > auto.LoadMoney ? Math.Floor(auto.LoadMoney) : Math.Floor(auto.Money) ;

                var dealService =IoC.Current.Resolve<IDealService>();

                try
                {
                    dealService.Bid(auto.UserID, projectForAuto.ID, loadMoney, true);
                    db.ExecuteSql("update dbo.qdt_user_autobid set LastTime=getdate() where id=" + auto.ID);
                    break;
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        private void Execute() {
            try {
                MsgQueue msg = MsgRepository.GetNeedSendMessage();

                if (msg == null) return;

                msg.UpdateSend();

                DbProvider().SaveChanges();

                if (msg.Type == MsgType.SMS) {
                    SendSmsMessage(msg);
                }

                if (msg.Type == MsgType.Email) {
                    SendMailMessage(msg);
                }
            }
            catch (Exception ex) {
                ILog log = LogManager.GetLogger("Loger");
                log.Error(ex);
            }
        }

        private void SendSmsMessage(MsgQueue msg) {
            string type = SettingManager.Get<MessageConfig>().SmsType;

            SmsServer smsSever = DbProvider().D<SmsServer>().FirstOrDefault(x => x.ClassName == type);

            if (smsSever == null) {
                var m = DbProvider().GetByID<MsgQueue>(msg.ID);
                m.IsSuccess = false;
                m.Result = "未设置默认短信通道";
                DbProvider().SaveChanges();
                return;
            }

            SMSSender sender = SMSSender.CreateInstance(new SMSAttribute {
                                                                             Config = smsSever.Config.Values.ToArray(), Name = smsSever.ServerName, SmsAccount = smsSever.UserName, SmsPassword = smsSever.Password, TypeName = smsSever.ClassName
                                                                         });

            if (sender == null) {
                var m = DbProvider().GetByID<MsgQueue>(msg.ID);
                m.IsSuccess = false;
                m.Result = "短信通道不存在";
                DbProvider().SaveChanges();
                return;
            }

            try {
                sender.Send(msg.Dest, msg.Content);
                var m = DbProvider().GetByID<MsgQueue>(msg.ID);
                m.IsSuccess = true;
                DbProvider().SaveChanges();
            }
            catch (Exception ex) {
                var m = DbProvider().GetByID<MsgQueue>(msg.ID);
                m.IsSuccess = false;
                m.Result = ex.Message;
                DbProvider().SaveChanges();
            }
        }


        private void SendMailMessage(MsgQueue msg) {
            var config = SettingManager.Get<EmailConfig>();

            IEmailSender sender = new EmailSender(config.SmtpServer, config.SmtpPort, config.Name, config.SmtpUserName, config.SmtpPassword, config.IsSSL);

            sender.SendMail(msg.Dest, msg.Title, msg.Content, msg.ID, (o, e) => {
                                                                          var m = DbProvider().GetByID<MsgQueue>(e.UserState);
                                                                          if (e.Error != null) {
                                                                              m.IsSuccess = false;
                                                                              m.Result = e.Error.Message;
                                                                          } else {
                                                                              m.IsSuccess = true;
                                                                          }
                                                                          DbProvider().SaveChanges();
                                                                      });
        }

        private IDbProvider DbProvider() {
            return IoC.Resolve<IUnitOfWork>() as IDbProvider;
        }

        #endregion
    }
}