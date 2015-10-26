using System;
using System.IO;
using System.Net;

namespace LoveBank.Common
{
    public abstract class HttpRequest : IHttpRequest
    {
        protected string Uri;
        private string encoding = "GBK";

        protected HttpRequest(string uri)
        {
            Uri = uri;
        }

        protected virtual string Method { get; set; }

        protected virtual string ContentType { get; set; }

        public virtual string ContentEncoding
        {
            get { return encoding; }
            set { encoding = value; }
        }

        public virtual string Request()
        {
            var req = WebRequest.Create(ConstructUri()) as HttpWebRequest;

            AppendHeaders(req.Headers);

            if (!string.IsNullOrEmpty(Method))
            {
                req.Method = Method;
            }

            if (!string.IsNullOrEmpty(ContentType))
            {
                req.ContentType = ContentType;
            }

            //"GET" 请求不支持内容
            if (req.Method == HttpMethod.Post)
            {
                using (Stream reqStream = req.GetRequestStream())
                {
                    WriteBody(reqStream);
                }
            }

            WebResponse resp = null;
            try
            {
                resp = req.GetResponse();
            }
            catch (WebException wex)
            {
                var webResp = wex.Response as HttpWebResponse;
                if (null == webResp)
                {
                    throw new HttpException(req.RequestUri.AbsoluteUri, "Network unavailable.");
                }

                var httpEx = new HttpException(req.RequestUri.AbsoluteUri, (int) webResp.StatusCode,
                                               webResp.StatusDescription, wex);

                throw httpEx;
            }

            return RetriveResponse(resp);
        }

        /// <summary>
        ///     接受请求的响应内容
        /// </summary>
        /// <param name="webResponse"></param>
        /// <returns></returns>
        protected virtual string RetriveResponse(WebResponse webResponse)
        {
            string respContent = string.Empty;
            Stream respStream = webResponse.GetResponseStream();
            using (var reader = new StreamReader(respStream))
            {
                if (respStream != null)
                {
                    try
                    {
                        //触发Bom读的情况，不知道具体原因，能部分解决二进制流的html获取不完整的情况
                        reader.Peek();
                    }
                    catch (Exception)
                    {
                    }
                }
                respContent = reader.ReadToEnd();
            }
            webResponse.Close();

            return respContent;
        }

        /// <summary>
        ///     再继承类中去重写这个方法，用于构造完整的请求路径
        /// </summary>
        /// <returns></returns>
        protected virtual string ConstructUri()
        {
            return Uri;
        }

        public virtual string GetConstructedUri()
        {
            return ConstructUri();
        }

        protected virtual void AppendHeaders(WebHeaderCollection headers)
        {
            //Nothing to do.
        }

        /// <summary>
        ///     将数据写入请求二进制流里面
        ///     <remarks>Http GET type request is not supported.</remarks>
        /// </summary>
        /// <param name="reqStream"></param>
        protected virtual void WriteBody(Stream reqStream)
        {
            //Nothing to do
        }
    }
}