using LoveBank.Core.Domain;
using LoveBank.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveBank.Web.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public int AddUserId { get; set; }
        public DateTime AddTime { get; set; }

        public RowState State { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }

        public int CostScore { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string Guid { get; set; }
        public string BarCode { get; set; }
        public string DeptId { get; set; }
        public string DeptIdName { get; set; }
        public string Desc { get; set; }
        public int? MachineId { get; set; }
        public ProductType Type { get; set; }
        public virtual IList<SourceFile> SourceFileList { get; set; }

        public virtual IList<SourceFile> SourceFileListLogo { get; set; }
        public string LogoGuid { get; set; }
        public virtual IList<SourceFile> SourceFileListAd { get; set; }

        public string AdGuid { get; set; }
        public virtual IList<ImgUrlModel> AppSourceFileList { get; set; }

        public virtual IList<ImgUrlModel> AppSourceFileListLogo { get; set; }

        public virtual IList<ImgUrlModel> AppSourceFileListAd { get; set; }
        public bool IsOwn { get; set; }
        public string Sponsors { get; set; }

        //public IQueryable< ImgUrlModel> ProductPic { get; set; }
        public IList<ImgUrlModel> ProductPic { get; set; }
    }
}