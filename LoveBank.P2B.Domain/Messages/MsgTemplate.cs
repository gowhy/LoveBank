using System.Collections.Generic;
using LoveBank.Common;
using System.Linq;
using LoveBank.Core;

namespace LoveBank.P2B.Domain.Messages
{
    /// <summary>
    /// 消息模板,消息模版里面涉及到参数的部分，采用{参数名}的格式，参数用大括号包裹起来
    /// </summary>
    public class MsgTemplate:Entity {
        /// <summary>
        /// 标识名
        /// </summary>
        public string Key { set; get; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { set; get; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { set; get; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public MsgType MsgType { set { InnerMsgType = (int)value; } get { return (MsgType) InnerMsgType; } }

        /// <summary>
        /// 是否为html
        /// <remarks>主要用于消息为邮件时</remarks>
        /// </summary>
        public bool IsHtml { set; get; }

        /// <summary>
        /// 提示消息
        /// </summary>
        public string MsgTip { set; get; }

        /// <summary>
        /// 用整数形式表示的消息类型
        /// </summary>
        public int InnerMsgType { private set; get; }

        /// <summary>
        /// 参数字符串
        /// </summary>
        public string ParamString { get; set; }

        private List<string> _params=new List<string>();

        /// <summary>
        /// 参数集合
        /// </summary>
        public IList<string> Params{ 
            get {
                _params = ParamString.SplitNull(',').ToList();
                return _params;
            }
        }

        /// <summary>
        /// 添加参数，如果参数已经存在，将被排除
        /// </summary>
        /// <param name="param"></param>
        public void AddParam(string param) {

            Params.ToList().Add(param);
        }

        /// <summary>
        /// 根据传入参数，输出格式化后的标题字符串
        /// </summary>
        /// <param name="variable">参数集合</param>
        /// <returns></returns>
        public string FormatTitle(params KeyValuePair<string, string>[] variable) {

            var tempTitle = Title;

            foreach (var pair in variable) {
                
                var forPair="{"+pair.Key+"}";

                tempTitle=tempTitle.Replace(forPair, pair.Value);
            }

            return tempTitle;
        }

        /// <summary>
        /// 根据传入参数，输出格式化后的内容字符串
        /// </summary>
        /// <param name="variable">参数集合</param>
        /// <returns></returns>
        public string FormatContent(params KeyValuePair<string,string>[] variable ) {

            var tempContent = Content;

            foreach (var pair in variable ) {

                var forPair = "{" + pair.Key + "}";

                tempContent = tempContent.Replace(forPair, pair.Value);

            }
            return tempContent;

        }
        

    }
}