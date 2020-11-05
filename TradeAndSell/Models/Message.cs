using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TradeAndSell.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Chat ID")]
        public string ChatId { get; set; }

        [Display(Name = "Sender ID")]
        public string SenderId { get; set; }

        [Display(Name = "Receiver ID")]
        public string ReceiverId { get; set; }

        public string Content { get; set; }

        [Display(Name = "Date Time")]
        public DateTime Datetime { get; set; }
    }
}
