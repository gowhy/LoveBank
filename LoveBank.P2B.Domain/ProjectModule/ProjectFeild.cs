using QDT.Common;

namespace QDT.P2B.Domain.ProjectModule
{
    /// <summary>
    /// 项目的扩展字段
    /// </summary>
    public class ProjectFeild:Entity {

        private const string FEILD_KEY = "ProjectFeild_";

        public ProjectFeild() {

            IsEffect = true;

            IsRequire = false;

            Tip = string.Empty;

            Sort = 0;
        }

        //public ProjectFeild(ProjectType type,string name,string showName,int inputType):this() {
            
        //    Check.Argument.IsNotNull(type,"type");

        //    Check.Argument.IsNotEmpty(name,"name");

        //    Check.Argument.IsNotEmpty(showName,"showName");

        //    TypeID = type.ID;

        //    Name = name;

        //    ShowName = showName;

        //    InputType = inputType;

        //}
        /// <summary>
        /// 所关联的项目类型 ID，扩展字段归属项目类型
        /// </summary>
        public int TypeID { get; private  set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return InnerName.ReplaceStart(FEILD_KEY, ""); }
            set { InnerName = FEILD_KEY + value; }
        }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string ShowName { get; set; }

        /// <summary>
        /// 属性类型
        /// </summary>
        public int InputType { get; set; }

        /// <summary>
        /// 值范围
        /// </summary>
        public string ValueScope { get; set; }

        /// <summary>
        /// 是否是必须的
        /// </summary>
        public bool IsRequire { get; set; }

        /// <summary>
        /// 是否有效,有效被显示，无效将不显示
        /// </summary>
        public bool IsEffect { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 提示
        /// </summary>
        public string Tip { get; set; }

        /// <summary>
        /// 值的范围
        /// </summary>
        public string[] ValueScopes
        {
            get { return ValueScope.SplitNull(','); }
        }

        /// <summary>
        /// 数据库存的真实的Name（为了避免出现与Project的字段重名）
        /// </summary>
        public string InnerName { get; set; }
    }
}
