using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradeAndSell.Models
{
    public class UsageData
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Data Type")]
        public string DataType { get; set; }

        [Display(Name = "Count")]
        public int Count { get; set; }

        [Display(Name = "Date Time")]
        public DateTime DateTime { get; set; }
    }
}
