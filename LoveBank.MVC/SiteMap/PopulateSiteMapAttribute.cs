using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using LoveBank.Common;

namespace LoveBank.MVC.SiteMap
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class PopulateSiteMapAttribute : FilterAttribute, IResultFilter
    {
        private static string defaultViewDataKey = "siteMap";

        public PopulateSiteMapAttribute(SiteMapDictionary siteMaps)
        {
            Check.Argument.IsNotNull(siteMaps, "siteMaps");

            SiteMaps = siteMaps;
        }

        public PopulateSiteMapAttribute()
            : this(SiteMapManager.SiteMaps)
        {
        }

        public static string DefaultViewDataKey
        {
            get
            {
                return defaultViewDataKey;
            }

            set
            {
                Check.Argument.IsNotEmpty(value, "value");

                defaultViewDataKey = value;
            }
        }

        public string SiteMapName
        {
            get;
            set;
        }

        public string ViewDataKey
        {
            get;
            set;
        }

        public SiteMapDictionary SiteMaps
        {
            get;
            private set;
        }

        public virtual void OnResultExecuting(ResultExecutingContext filterContext)
        {
            Check.Argument.IsNotNull(filterContext, "filterContext");

            var siteMap = string.IsNullOrEmpty(SiteMapName) ? SiteMaps.DefaultSiteMap : SiteMaps[SiteMapName];
            var viewDataKey = ViewDataKey ?? DefaultViewDataKey;

            filterContext.Controller.ViewData[viewDataKey] = siteMap;
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            // Do nothing, just sleep.
        }
    }
}
