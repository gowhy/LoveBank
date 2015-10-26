using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace LoveBank.Common
{
    public class HttpPost : HttpRequest
    {
        private readonly ParamCollection postParams = new ParamCollection();

        public HttpPost(string uri)
            : base(uri)
        {
            base.Method = HttpMethod.Post;
            base.ContentType = "application/x-www-form-urlencoded";
        }

        public HttpPost(string uri, string postData)
            : this(uri)
        {
            PostData = postData;
        }

        public ParamCollection Params
        {
            get { return postParams; }
        }

        public virtual string PostData { get; set; }


        protected override void WriteBody(Stream reqStream)
        {
            string postData = PostData;
            if (string.IsNullOrWhiteSpace(postData))
            {
                postData = PreparePostBody(postParams);
            }

            if (!string.IsNullOrWhiteSpace(postData))
            {
                byte[] dataBytes = Encoding.GetEncoding(ContentEncoding).GetBytes(postData);
                reqStream.Write(dataBytes, 0, dataBytes.Length);
            }
        }

        protected override void AppendHeaders(WebHeaderCollection headers)
        {
            headers.Add(HttpRequestHeader.ContentEncoding, ContentEncoding);
            base.AppendHeaders(headers);
        }


        private string PreparePostBody(IEnumerable<ParamPair> customPostParams)
        {
            var parameters = new ParamCollection();

            if (null != customPostParams)
            {
                foreach (ParamPair item in customPostParams)
                {
                    parameters.Add(item.Name, item.Value);
                }
            }

            string postBody = ConstructPostBody(parameters);
            return postBody;
        }

        private static string ConstructPostBody(IEnumerable<ParamPair> parameters)
        {
            var bodyBuilder = new StringBuilder();
            foreach (ParamPair item in parameters)
            {
                string name = item.Name;
                string val = item.Value;

                bodyBuilder.Append(string.Format("{0}={1}&", name, val));
            }
            string result = bodyBuilder.ToString().TrimEnd('&');
            return result;
        }
    }
}