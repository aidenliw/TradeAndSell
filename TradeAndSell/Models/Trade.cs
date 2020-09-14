using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradeAndSell.Models
{
    public class Trade
    {
        public int Id { get; set; }

        [Display(Name = "Item ID")]
        public int ItemId { get; set; }

        [Display(Name = "Seller ID")]
        public string SellerId { get; set; }

        [Display(Name = "Buyer ID")]
        public string BuyerId { get; set; }

        [Display(Name = "Trade Item IDs")]
        public List<int> TradeItemId { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Message")]
        public string Message { get; set; }

        [Display(Name = "Approve Status")]
        public string ApproveStatus { get; set; }
    }
}
