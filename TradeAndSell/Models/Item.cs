using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradeAndSell.Models
{
    public class Item
    {
        [Display(Name = "Seller ID")]
        public string SellerId { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Condition")]
        public string Condition { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Category")]
        public string Category { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Image Path")]
        public string ImagePath { get; set; }

        [Display(Name = "Price")]
        public float Price { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Allow Trade")]
        public string AllowTrade { get; set; }

        [Display(Name = "Mininum Trade Pirce")]
        public float MinTradePirce { get; set; }

        [Display(Name = "Maximum Trade Price")]
        public float MaxTradePrice { get; set; }
    }
}
