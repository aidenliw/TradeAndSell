using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TradeAndSell.Areas.Identity.Pages.Account.Manage
{
    public class MyCardModel : PageModel
    {
        [TempData]
        public string StatusMessage { get; set; }

        public void OnGet()
        {
        }
    }
}
