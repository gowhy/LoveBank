﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using LoveBank.Common;
using LoveBank.MVC.UI;
using System.Web.Routing;

namespace LoveBank.MVC
{
    public static class HtmlExntensions
    {
        
        public static GridButtons<T> AddDelete<T>(this GridButtons<T> buttons,Func<T,object> key) where T:class
        {
            buttons.Add(x => new HtmlString("<a href=\"javascript: del({0})\">删除</a>".FormatWith(key(x))));
            return buttons;
        }

        public static GridButtons<T> ScriptLink<T>(this GridButtons<T> buttons,string text,Func<T,string> js) where T:class
        {
            buttons.Add(x => new HtmlString("<a href=\"javascript: {0}\">{1}</a>".FormatWith(js(x),text)));
            return buttons;
        }

        public static IHtmlString ScriptLink(this HtmlHelper helper,string text,string js)
        {
            return ScriptLink(helper, text, js, new object[] {});
        }

        public static IHtmlString ScriptLink(this HtmlHelper helper,string text,string js,params object[] format)
        {
            return new HtmlString("<a href=\"javascript: {0}\">{1}</a>".FormatWith(js.FormatWith(format), text));
        }
        public static IHtmlString ScriptLink(this HtmlHelper helper, string text, string js, string className, params object[] format)
        {
            return new HtmlString("<a href=\"javascript: {0}\" class=\"{2}\">{1}</a>".FormatWith(js.FormatWith(format), text,className));
        }
        public static IHtmlString ImgLink(this HtmlHelper helper, string src, string width, string height, string title, string className, params object[] format)
        {
            return new HtmlString("<a href=\"{0}\" target =\"_blank\"> \"  <img src=\"{0}\" width=\"{1}\" height=\"{2}\" title=\"{3}\"  class=\"{4}\"/> </a>".FormatWith(src, width, height, title, className));
        }

          //<img src="" width="" height="" title="" />
        public static IHtmlString UploadImage(this HtmlHelper helper, string name)
        {
            return new UploadImage(name, null);
        }

        public static IHtmlString UploadImage(this HtmlHelper helper, string name, string value)
        {
            return new UploadImage(name, value);
        }

        public static IHtmlString ValidateImage(this HtmlHelper helper)
        {
            return new ValidateImage();
        }

        public static string Image(this UrlHelper helper,string name) {
            return helper.Content("~/Content/Default/Images/" + name);
        }

        public static IHtmlString DBItemToSelectList(this HtmlHelper helper, IEnumerable<SelectListItem> listItem, string currentValue, string name, string optionLabel, object htmlAttributes)
        {
            //var selectList = e.ToSelectItem();
            var list = new List<SelectListItem>();
            foreach (SelectListItem current in listItem)
            {
                current.Selected = current.Value != null && current.Value == currentValue;
                list.Add(current);
            }
            listItem = list;
            var stringBuilder = new StringBuilder();
            if (optionLabel != null)
            {
                stringBuilder.AppendLine(ListItemToOption(new SelectListItem
                {
                    Text = optionLabel,
                    Value = string.Empty,
                    Selected = false
                }));
            }
            foreach (SelectListItem current2 in listItem)
            {
                stringBuilder.AppendLine(ListItemToOption(current2));
            }



            var tagBuilder = new TagBuilder("select");
            tagBuilder.InnerHtml = stringBuilder.ToString();
            tagBuilder.GenerateId(name);
            //tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            tagBuilder.MergeAttribute("name", name, true);
            tagBuilder.GenerateId(name);
            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));

        }

        public static IHtmlString EnumToSelectList(this HtmlHelper helper,Enum e,string currentValue,string name,string optionLabel,object htmlAttributes) {
            var selectList = e.ToSelectItem();
            var list = new List<SelectListItem>();
            foreach (SelectListItem current in selectList) {
                current.Selected = current.Value != null && current.Value == currentValue;
                list.Add(current);
            }
            selectList = list;
            var stringBuilder = new StringBuilder();
            if (optionLabel != null)
            {
                stringBuilder.AppendLine(ListItemToOption(new SelectListItem
                {
                    Text = optionLabel,
                    Value = string.Empty,
                    Selected = false
                }));
            }
            foreach (SelectListItem current2 in selectList)
            {
                stringBuilder.AppendLine(ListItemToOption(current2));
            }

          

            var tagBuilder = new TagBuilder("select");
            tagBuilder.InnerHtml = stringBuilder.ToString();
            tagBuilder.GenerateId(name);
            //tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            tagBuilder.MergeAttribute("name",name,true);
            tagBuilder.GenerateId(name);
            return new MvcHtmlString(tagBuilder.ToString(TagRenderMode.Normal));

        }
        internal static string ListItemToOption(SelectListItem item)
        {
            TagBuilder tagBuilder = new TagBuilder("option")
            {
                InnerHtml = HttpUtility.HtmlEncode(item.Text)
            };
            if (item.Value != null)
            {
                tagBuilder.Attributes["value"] = item.Value;
            }
            if (item.Selected)
            {
                tagBuilder.Attributes["selected"] = "selected";
            }
            return tagBuilder.ToString(TagRenderMode.Normal);
        }

    }
}