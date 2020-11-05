using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradeAndSell.Models
{
    public class TradeDetails
    {
        //id = trade.Id,
        //        itemId = item.Id,
        //        itemName = item.Title,
        //        tradeItemIds = trade.TradeItemIds,
        //        tradeItemIdList = trade.TradeItemIds.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
        //        tradeItemFirstName = items.Where(i => i.Id == int.Parse(trade.TradeItemIds.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()[0])).Select(i => i.Title).FirstOrDefault(),
        //        status = trade.ApproveStatus,
        //        sellerId = trade.SellerId,
        //        sellerName = users.Where(u => u.Id == trade.SellerId).Select(u => u.DisplayName).FirstOrDefault(),
        //        buyerId = trade.BuyerId,
        //        buyerName = users.Where(u => u.Id == trade.BuyerId).Select(u => u.DisplayName).FirstOrDefault(),
        //        message = trade.Message,
        //        date = trade.Date

        [Display(Name = "Trade ID")]
        public int TradeId { get; set; }

        [Display(Name = "Target Item ID")]
        public int ItemId { get; set; }

        [Display(Name = "Target Item Name")]
        public string ItemName { get; set; }

        [Display(Name = "Image Path")]
        public string ItemImagePath { get; set; }

        [Display(Name = "Trade Item ID")]
        public string TradeItemIds { get; set; }

        [Display(Name = "Trade Item List")]
        public string[] TradeItemIdList { get; set; }

        [Display(Name = "Trade Item First Id")]
        public int TradeItemFirstId { get; set; }

        [Display(Name = "Trade Item First Name")]
        public string TradeItemFirstName { get; set; }

        public string Status { get; set; }

        [Display(Name = "Seller ID")]
        public string SellerId { get; set; }

        [Display(Name = "Seller Name")]
        public string SellerName { get; set; }

        [Display(Name = "Buyer ID")]
        public string BuyerId { get; set; }

        [Display(Name = "Buyer Name")]
        public string BuyerName { get; set; }

        [Display(Name = "Chat ID")]
        public string ChatId { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }

        public bool MyRequest { get; set; }
    }
}
