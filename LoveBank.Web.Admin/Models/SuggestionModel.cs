using LoveBank.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveBank.Web.Admin.Models
{
    public class SuggestionModel
    {
        public int Id { get; set; }
        public string DepId { get; set; }
        public string Content { get; set; }
        public int AddUser { get; set; }
        public string DepName { get; set; }
        public Vol Vol { get; set; }
        public System.DateTime AddTime { get; set; }
    }
}