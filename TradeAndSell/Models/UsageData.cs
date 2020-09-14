using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradeAndSell.Models
{
    public class UsageData
    {
        public int Id { get; set; }
         
        [Display(Name = "Post Count")]
        public int PostCount { get; set; }

        [Display(Name = "Post Count Date")]
        public DateTime PostCountDate { get; set; }

        [Display(Name = "Transaction Count")]
        public int TransactionCount { get; set; }

        [Display(Name = "Transaction Count Date")]
        public DateTime TransactionCountDate { get; set; }

        [Display(Name = "Registration Count")]
        public int RegistrationCount { get; set; }

        [Display(Name = "Registration Count Date")]
        public DateTime RegistrationCountDate { get; set; }
    }
}
