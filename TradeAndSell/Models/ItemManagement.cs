using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradeAndSell.Models
{
    public class ItemManagement
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Seller ID")]
        public string SellerId { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Condition")]
        public string Condition { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string Category { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Display(Name = "Image")]
        public IFormFile Image { get; set; }

        [Required]
        [Display(Name = "Price")]
        public float Price { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Allow Trade")]
        public string AllowTrade { get; set; }

        [Display(Name = "Mininum Trade Pirce")]
        public float? MinTradePrice { get; set; }

        [Display(Name = "Maximum Trade Price")]
        public float? MaxTradePrice { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
