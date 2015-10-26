using System;
using System.Text;
using LitJson;
using LoveBank.Common;

namespace LoveBank.MVC.UI.Image
{
    public class UpYun
    {
        public const string FormSecret = "XF3x5+DSVa9/rweFHKEyMEbDqCA=";

        public const string SavePath = "/{year}/{mon}/upload_{filename}{.suffix}";

        public const string AllowType = "jpg,gif,png";

        public UpYun()
            : this("qiandt-image")
        {
        }

        public UpYun(string bucket)
        {
            Bucket = bucket;
        }

        public string Bucket { get; set; }

        /// <summary>
        /// 原图安全密码，如果为空表示不制定
        /// </summary>
        public string ContentSecret { get; set; }

        /// <summary>
        /// 回调Url,如果制定，上传成功后将调用此URL,否则body返回
        /// </summary>
        public string NotifyUrl { get; set; }

        private static string ExpirTime
        {
            get
            {
                //过期时间为当前时间后10分钟
                var dt = DateTime.Now.AddMinutes(10);
                string date = dt.ConvertDateTimeInt().ToString();

                return date;
            }
        }

        public string UploadUrl
        {
            get
            {
                return "http://v0.api.upyun.com/" + Bucket;
            }
        }

        public string GetPolicy()
        {
            var data = new JsonData();

            data["bucket"] = Bucket;
            data["save-key"] = SavePath;
            data["expiration"] = ExpirTime;
            data["allow-file-type"] = AllowType;

            if (!string.IsNullOrWhiteSpace(ContentSecret)) data["content-secret"] = ContentSecret;

            if (!string.IsNullOrWhiteSpace(NotifyUrl)) data["notify-url"] = NotifyUrl;

            return Convert.ToBase64String(Encoding.UTF8.GetBytes(data.ToJson()));
        }

        public string GetSignature()
        {
            return (GetPolicy() + "&" + FormSecret).Hash().ToLower();
        }
    }
}
