using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain
{
    public class SourceFile 
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Domain { get; set; }
        public string Path { get; set; }
        public string Server { get; set; }
        public System.DateTime AddTime { get; set; }

        public int AddUserId { get; set; }
        public string Extension { get; set; }
        public string FileName { get; set; }
        public string Guid { get; set; }
        ///// <summary>
        ///// 客户端的文件ID，用于在客户标示文件的唯一性
        ///// </summary>
        //public string ClientFileId { get; set; }
    }
}
