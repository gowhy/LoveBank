using LoveBank.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveBank.Core.Domain
{
   public class LoginStatistics
    {
        /// <summary>
        /// Id
        /// </summary>		
        public long Id { get; set; }

        /// <summary>
        /// Type
        /// </summary>		
        public LoginType Type { get; set; }

        /// <summary>
        /// AddTime
        /// </summary>		
        public DateTime AddTime { get; set; }

        /// <summary>
        /// MachineCode
        /// </summary>		
        public string MachineCode { get; set; }

        /// <summary>
        /// UserId
        /// </summary>		
        public int UserId { get; set; }

        /// <summary>
        /// Lon
        /// </summary>		
        public Nullable<double> Lon { get; set; }

        /// <summary>
        /// Lat
        /// </summary>		
        public Nullable<double> Lat { get; set; }

        /// <summary>
        /// Address
        /// </summary>		
        public string Address { get; set; }

        public string Phone { get; set; }

        public int LoginState { get; set; }
    }
}
