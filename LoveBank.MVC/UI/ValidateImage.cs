using System.Collections;
using System.Linq;
using System.Web;

namespace LoveBank.MVC.UI
{
    public class ValidateImage : IHtmlString
    {
        private readonly string _input;
        private readonly string _image;
        private readonly string _click;

        public ValidateImage()
        {
            _input = null; // "<input type=\"text\" name=\"Valicode\" data-required />";
            _image = "<img id=\"ValicodeImg\" style=\"margin-left: 3px;vertical-align: middle;\" src=\"/Valicode/Image\" alt=\"验证码\" />  ";
            _click = "$(function () {$(\"#ValicodeImg\").bind(\"click\", function () {this.src = \"/Valicode/Image\";});});";
        }

        public string ToHtmlString()
        {
            var script = new ArrayList { _click };

            return _input + _image + Javascript(script);
        }

        private static string Javascript(IEnumerable scripts)
        {
            var script = "<script type=\"text/javascript\">";
            if (scripts != null)
            {
                script = scripts.Cast<object>().Aggregate(script, (o, s) => o + s);
            }
            script = script + "</script>";
            return script;
        }
    }
}
