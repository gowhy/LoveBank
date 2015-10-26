using System;
using System.Runtime.Serialization;

namespace QDT.P2B.Domain.UserModule
{
    public class UserCreateException:Exception,ISerializable
    {
        public UserCreateException():base()
        {
            StatusCode = UserCreateException.Unknow;
        }

        public UserCreateException(string message):base(message)
        {
            StatusCode = UserCreateException.Unknow;
        }

        public UserCreateException(string message, Exception inner):base(message,inner)
        {
            StatusCode = UserCreateException.Unknow;
        }

        public UserCreateException(int statusCode)
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; set; }

        /// <summary>
        /// 未知的异常
        /// </summary>
        public const int Unknow = 0;

        /// <summary>
        /// Email已经存在
        /// </summary>
        public const int DuplicateEmail = 1;

        /// <summary>
        /// 用户名已经存在
        /// </summary>
        public const int DuplicateUserName = 2;

        /// <summary>
        /// 手机号码已经存在
        /// </summary>
        public const int DuplicateMobile = 3;

        /// <summary>
        /// 无效的Email
        /// </summary>
        public const int InvaildEmail = 4;

        /// <summary>
        /// 无效的手机号码
        /// </summary>
        public const int InvaildMobile = 5;

        /// <summary>
        /// 无效的密码
        /// </summary>
        public const int InvalidPassword = 6;
    }
}
