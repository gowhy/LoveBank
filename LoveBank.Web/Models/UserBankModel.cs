namespace LoveBank.Web.Models
{
    public class UserBankModel
    {
        public int BankId { set; get; }
        public int OtherBank { set; get; }
        public string BankCard { set; get; }
        public string BankZone { set; get; }
        public int RegionLv2 { set; get; }
        public int RegionLv3 { set; get; }
    }
}