using System.Data.Entity.ModelConfiguration;
using LoveBank.Core.Members;

namespace LoveBank.Core.MSData.Mapping.Members
{
    public class UserMapping:EntityTypeConfiguration<User>
    {
        public UserMapping()
        {
            //HasKey(x => x.ID);
            //Property(x => x.UserName).HasColumnName("user_name");
            //Property(x => x.Password).HasColumnName("user_pwd");
            //Property(x=>x.Sex).HasColumnName("sex");
            //Property(x => x.CreateTime).HasColumnName("create_time");
            //Property(x => x.UpdateTime).HasColumnName("update_time");
            //Property(x => x.LoginTime).HasColumnName("login_time");
            //Property(x => x.LoginIP).HasColumnName("login_ip");
            //Property(x => x.GroupID).HasColumnName("group_id");
            //Property(x => x.IsEffect).HasColumnName("is_effect");
            //Property(x => x.IsDelete).HasColumnName("is_delete");

            //Property(x => x.Email).HasColumnName("email");
            //Property(x => x.EmailPassed).HasColumnName("email_passed");

            //Property(x => x.RealName).HasColumnName("real_name");
            //Property(x => x.IDCard).HasColumnName("idno");
            //Property(x => x.IDCardPassed).HasColumnName("idcard_passed");
            
            //Property(x => x.Mobile).HasColumnName("mobile");
            //Property(x => x.MobilePassed).HasColumnName("mobile_passed");

            //Property(x => x.SafePassword).HasColumnName("pay_pwd");
            //Property(x => x.SafePasswordPassed).HasColumnName("pay_pwd_passed");

            //Property(x => x.IsLender).HasColumnName("is_lender");
            //Property(x => x.IsBorrower).HasColumnName("is_borrower");

            //Property(x => x.BirthYear).HasColumnName("birth_year");
            //Property(x => x.BirthMonth).HasColumnName("birth_month");
            //Property(x => x.BirthDay).HasColumnName("birth_day");
            //Property(x => x.RecommendID).HasColumnName("pid");

            //Property(x => x.IdValidatorTime).HasColumnName("IdValidatorTime");

            //HasRequired(x => x.UserAccount).WithMany().HasForeignKey(x => x.UserAccountID);

            //Ignore(x => x.UserSta);

            ToTable(DB.TPref("user"));
        }
    }

    
}
