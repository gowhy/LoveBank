using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LoveBank.Common;

namespace LoveBank.MVC
{
    public static class UrlExtenstions
    {
        /// <summary>
        /// Returns an absolute url for an action
        /// </summary>
        /// <param name="url"></param>
        /// <param name="controller">Controller Name</param>
        /// <param name="action">Action Name</param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public static string AbsoluteAction(this UrlHelper url, string action, string controller, object routeValues)
        {
            Uri requestUrl = url.RequestContext.HttpContext.Request.Url;
            string absoluteAction = "{0}://{1}{2}".FormatWith(requestUrl.Scheme, requestUrl.Authority, url.Action(action, controller, routeValues));
            return absoluteAction;
        }

        /// <summary>
        /// Returns an absolute url for an action
        /// </summary>
        /// <param name="url"></param>
        /// <param name="controller"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string AbsoluteAction(this UrlHelper url, string action, string controller)
        {
            return url.AbsoluteAction(action, controller, new object { });
        }

        /// <summary>
        /// Returns an absolute url for an action
        /// </summary>
        /// <param name="url"></param>
        /// <param name="action"></param>
        /// <param name="routeValues"></param>
        /// <returns></returns>
        public static string AbsoluteAction(this UrlHelper url, string action, object routeValues)
        {
            Uri requestUrl = url.RequestContext.HttpContext.Request.Url;
            string absoluteAction = "{0}://{1}{2}".FormatWith(requestUrl.Scheme, requestUrl.Authority, url.Action(action, routeValues));
            return absoluteAction;
        }

        /// <summary>
        /// Returns an absolute url for an action
        /// </summary>
        /// <param name="url"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string AbsoluteAction(this UrlHelper url, string action)
        {
            return url.AbsoluteAction(action, new object { });
        }

        public static string Content(this UrlHelper url, string contentPath, bool absolute)
        {
            Uri requestUrl = url.RequestContext.HttpContext.Request.Url;
            if (absolute)
            {
                string absoluteAction = "{0}://{1}{2}".FormatWith(requestUrl.Scheme, requestUrl.Authority, url.Content(contentPath));
                return absoluteAction;
            }
            return url.Content(contentPath);
        }
    }
}
