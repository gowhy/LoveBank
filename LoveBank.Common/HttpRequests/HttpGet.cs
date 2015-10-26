using System.Web;

namespace LoveBank.Common
{
    public class HttpGet : HttpRequest
    {
        private readonly ParamCollection queryParams = new ParamCollection();

        public HttpGet(string uri)
            : base(uri)
        {
            base.Method = HttpMethod.Get;
        }

        public virtual ParamCollection Params
        {
            get { return queryParams; }
        }

        protected override string ConstructUri()
        {
            string uri = Uri;
            if (null != Params && Params.Count > 0)
            {
                uri += "?";
                foreach (ParamPair item in Params)
                {
                    uri += string.Format("{0}={1}&", HttpUtility.UrlEncode(item.Name), HttpUtility.UrlEncode(item.Value));
                }
                uri = uri.TrimEnd('&');
            }
            return uri;
        }
    }
}