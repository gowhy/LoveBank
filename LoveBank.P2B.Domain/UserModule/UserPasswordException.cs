using System;
using System.Runtime.Serialization;

namespace QDT.P2B.Domain.UserModule
{
    public class UserPasswordException : Exception, ISerializable
    {
        public UserPasswordException():base()
        {
            StatusCode = UserCreateException.Unknow;
        }

        public UserPasswordException(string message):base(message)
        {
            StatusCode = UserCreateException.Unknow;
        }

        public UserPasswordException(string message, Exception inner):base(message,inner)
        {
            StatusCode = UserCreateException.Unknow;
        }

        public UserPasswordException(int statusCode)
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; set; }

        /// <summary>
        /// 无效的旧密码
        /// </summary>
        public const int InvaildOldPassword = 1;

        /// <summary>
        /// 重复设置安全密码
        /// </summary>
        public const int DuplicateSafePassword = 2;

        /// <summary>
        /// 未激活安全密码
        /// </summary>
        public const int UnactivatedSafePassword = 3;
    }
}
