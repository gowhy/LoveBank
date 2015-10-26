namespace QDT.P2B.Domain.ProjectModule
{
    /// <summary>
    /// 项目的扩展字段值
    /// </summary>
    public class ProjectExtend:Entity
    {
        /// <summary>
        /// 所属Proejct Field ID
        /// </summary>
        public int FeildID { get; set; }

        /// <summary>
        /// 所属Project ID
        /// </summary>
        public int ProjectID { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }
}
