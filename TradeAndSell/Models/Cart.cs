using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradeAndSell.Models
{
    public class Cart
    {
        public int Id { get; set; }

        [Display(Name = "Item ID")]
        public List<int> ItemId { get; set; }

        [Display(Name = "Owner ID")]
        public string OwnerId { get; set; }

        [Display(Name = "On Wish List")]
        public bool OnWishList { get; set; }
    }
}
