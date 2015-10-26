using System;
using System.Collections.Generic;
using System.Linq;

namespace QDT.P2B.Domain.ProjectModule
{
    /// <summary>
    /// 理财投资项目类
    /// </summary>
    public class Project : Entity,IAggregeRoot
    {
        public Project()
        {
            CreateTime = DateTime.Now;
            UpdateTime = DateTime.Now;
            Status = ProjectStatus.Wait;
            Sort = 0;
            IsEffect = false;
            IsDelete = false;
            IsRecommend = false;
            IsSendBadMsg = false;
            IsSendContract = false;
        }

        //public Project(User borrower, ProjectType type,AlgorithmInfo loadAlgorithm, string name, string subName, decimal borrowAmount, decimal minLoanMoney, decimal maxLoanMoney, int repayTime, int repayTimetype, double rate)
        //    :this()
        //{
        //    Check.Argument.IsNotNull(borrower, "borrower");
        //    Check.Argument.IsNotNull(type, "cate");
        //    Check.Argument.IsNotNull(loadAlgorithm, "repayment");

        //    UserID = borrower.ID;
        //    TypeID = type.ID;
        //    RepaymentID = loadAlgorithm.ID;
        //    Name = name;
        //    SubName = subName;
        //    BorrowAmount = borrowAmount;
        //    MinLoanMoney = minLoanMoney;
        //    MaxLoanMoney = maxLoanMoney;
        //    RepayTime = repayTime;
        //    RepayTimeType = repayTimetype;
        //    Rate = rate;
        //}

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 项目简称
        /// </summary>
        public string SubName { get; set; }

        /// <summary>
        /// 项目类型ID
        /// </summary>
        public int TypeID { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// 还款方式ID
        /// </summary>
        public int RepaymentID { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsEffect { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 借款金额
        /// </summary>
        public decimal BorrowAmount { get; set; }

        /// <summary>
        /// 还款限期，当RepayTimeType为0的时候代表天数，当RepayTimeType为1的时候代表月数
        /// </summary>
        public int RepayTime { get; set; }


        /// <summary>
        /// 还款时间类型，0代表天，1代表月
        /// </summary>
        public int RepayTimeType { get; set; }

        /// <summary>
        /// 利率
        /// </summary>
        public double Rate { get; set; }

        /// <summary>
        /// 每个人限制购买次数，0为不限制
        /// </summary>
        public int BuyCount { get; set; }

        /// <summary>
        /// 已经融到的总金额
        /// </summary>
        public decimal LoadMoney { get; set; }

        /// <summary>
        /// 已经还款总金额（包括利息）
        /// </summary>
        public decimal RepayMoney { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 成功时间
        /// </summary>
        public DateTime? SuccessTime { get; set; }

        /// <summary>
        /// 还款开始时间
        /// </summary>
        public DateTime? RepayStartTime { get; set; }

        /// <summary>
        /// 最后还款时间
        /// </summary>
        public DateTime? LastRepayTime { get; set; }

        /// <summary>
        /// 下次还款时间
        /// </summary>
        public DateTime? NextRepayTime { get; set; }

        /// <summary>
        /// 流标时间
        /// </summary>
        public DateTime? BadTime { get; set; }

        /// <summary>
        /// 投标最后结束时间
        /// </summary>
        public DateTime? LastEndTime {
            get {
                if(StartTime==null) return null;
                return StartTime.Value.AddDays(EndDate);
            }
        }

        public int InnerStatus { get; set; }

        /// <summary>
        /// 项目状态
        /// </summary>
        public ProjectStatus Status
        {
            get { return (ProjectStatus)InnerStatus; }
            set { InnerStatus = (int)value; }
        }
        /// <summary>
        ///  筹款期限
        /// </summary>
        public int EndDate { get; set; }

        /// <summary>
        /// 是否发送流标信息
        /// </summary>
        public bool IsSendBadMsg { get; set; }

        /// <summary>
        /// 流标信息
        /// </summary>
        public string BadMsg { get; set; }

        /// <summary>
        /// 标题颜色
        /// </summary>
        public string TitleColor { get; set; }

        /// <summary>
        /// 是否发送合同
        /// </summary>
        public bool IsSendContract { get; set; }

        /// <summary>
        /// 最小投资金额
        /// </summary>
        public decimal MinLoanMoney { get; set; }

        /// <summary>
        /// 最大投资金额
        /// </summary>
        public decimal MaxLoanMoney { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 担保范围
        /// </summary>
        public int Warrant { get; set; }


        /// <summary>
        /// 服务费率
        /// </summary>
        public double ServicesFee { get; set; }

        /// <summary>
        /// 是否推荐，置顶
        /// </summary>
        public bool IsRecommend { get; set; }

        /// <summary>
        /// 扩展属性值
        /// </summary>
        public virtual ICollection<ProjectExtend> ExtendAttributes { get; set; }

        /// <summary>
        /// 返回指定属性的属性值，如果不存在，返回string.empty
        /// </summary>
        /// <param name="fieldID"></param>
        /// <returns></returns>
        public string GetAttribute(int fieldID)
        {
            var attr = ExtendAttributes.ToList().FirstOrDefault(x => x.FeildID == fieldID);
            return attr == null ? "" : attr.Value;
        }

        /// <summary>
        /// 获得投标进度
        /// </summary>
        public double Progress {
            get { return Convert.ToDouble(LoadMoney/BorrowAmount*100); }
        }

        /// <summary>
        /// 获得还款进度
        /// </summary>
        public double RepayProgress
        {
            get { return Convert.ToDouble(RepayMoney / BorrowAmount * 100) > 100 ? 100 : Convert.ToDouble(RepayMoney / BorrowAmount * 100); }
        }

        /// <summary>
        /// 可投余额
        /// </summary>
        public decimal NeedMoney {
            get { return BorrowAmount - LoadMoney; }
        }
    }
}
