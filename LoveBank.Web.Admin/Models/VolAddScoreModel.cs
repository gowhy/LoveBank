using LoveBank.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoveBank.Web.Admin.Models
{
    public class VolAddScoreModel
    {
        public Vol Vol { get; set; }

        public List<TeamProjectModel> TeamProjectList { get; set; }

    }
}