using System.Web;

namespace LoveBank.MVC
{
    public static class StringExtensions
    {
        public static IHtmlString ToHtml(this string source) {
            return new HtmlString(source);
        }
    }
}
