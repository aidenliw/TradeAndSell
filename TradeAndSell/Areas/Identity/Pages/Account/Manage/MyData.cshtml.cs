using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TradeAndSell.Data;
using TradeAndSell.Models;

namespace TradeAndSell.Areas.Identity.Pages.Account.Manage
{
    public class MyDataModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public MyDataModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public int RegistrationCount { get; set; }

        [BindProperty]
        public int NewPostsCount { get; set; }

        [BindProperty]
        public int TradeRequestsCount { get; set; }

        [BindProperty]
        public int SucceedTransactionsCount { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load posts for user with ID '{_userManager.GetUserId(User)}'.");
            }

            RegistrationCount = _userManager.Users.Count();
            NewPostsCount = _context.Item.Count();
            TradeRequestsCount = _context.Trade.Count();
            SucceedTransactionsCount = _context.Trade.Where(t => t.ApproveStatus.ToLower() == "approved").Count();

            return Page();
        }
    }
}
