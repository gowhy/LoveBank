using System;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LoveBank.Web
{
    #region EmailAddress
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class EmailAddressAttribute : RegularExpressionAttribute
    {

        protected const string pattern = @"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$";

        static EmailAddressAttribute()
        {
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(EmailAddressAttribute), typeof(RegularExpressionAttributeAdapter));
        }

        public EmailAddressAttribute()
            : base(pattern)
        {
        }
    }
    #endregion

    #region NumberData
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class NumberDataAttribute : RegularExpressionAttribute
    {
        protected const string pattern = @"^[0-9]{1,5}$";
        
        static NumberDataAttribute()
        {
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(NumberDataAttribute), typeof(RegularExpressionAttributeAdapter));
        }

        public NumberDataAttribute()
            : base(pattern)
        {
        }
    }
    #endregion

    #region PhoneNumber
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class PhoneNumberAttribute : RegularExpressionAttribute
    {
        protected const string pattern = @"^1[3-9][0-9]\d{8}$";

        static PhoneNumberAttribute()
        {
            DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(PhoneNumberAttribute), typeof(RegularExpressionAttributeAdapter));
        }

        public PhoneNumberAttribute()
            : base(pattern)
        {
        }
    }
    #endregion

}