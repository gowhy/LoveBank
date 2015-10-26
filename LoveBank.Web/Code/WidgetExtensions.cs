using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace LoveBank.Web
{
    public static class WidgetExtensions
    {
        public static string ViewKey = "ViewPath";

        public static MvcHtmlString Widget(this HtmlHelper htmlHelper, string actionName)
        {
            return htmlHelper.Action(actionName,"Widget");
        }

        public static MvcHtmlString Widget(this HtmlHelper htmlHelper,string actionName,string viewPath)
        {
            return Widget(htmlHelper,actionName,viewPath,new object());
        }

        
        public static MvcHtmlString Widget(this HtmlHelper htmlHelper,string actionName,string viewPath,object routeValues)
        {
            var routes = new RouteValueDictionary(routeValues) {{ViewKey, viewPath}};

            return Widget(htmlHelper, actionName, routes);
        }

        public static MvcHtmlString Widget(this HtmlHelper htmlHelper, string actionName, object routeValue)
        {
            return Widget(htmlHelper,actionName,new RouteValueDictionary(routeValue));
        }

        private static MvcHtmlString Widget(HtmlHelper htmlHelper,string actionName,RouteValueDictionary routeValueDictionary) {
            return htmlHelper.Action(actionName, "Widget", routeValueDictionary);
        }

    }
}