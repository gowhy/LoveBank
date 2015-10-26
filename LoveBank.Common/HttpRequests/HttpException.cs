using System;

namespace LoveBank.Common
{
    public class HttpException : SystemException
    {
        public HttpException()
        {
        }

        public HttpException(string uri, string message)
            : base(message)
        {
            //代表服务器返回错误消息的状态
            ErrorCode = 2000;
            Uri = uri;
        }

        public HttpException(string uri, int errorCode, string message)
            : this(uri, message)
        {
            ErrorCode = errorCode;
        }

        public HttpException(string uri, int errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            Uri = uri;
            ErrorCode = errorCode;
        }

        public string Uri { get; set; }

        public int ErrorCode { get; set; }
    }
}