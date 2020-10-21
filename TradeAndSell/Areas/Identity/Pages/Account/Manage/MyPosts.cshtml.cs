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
    public class MyPostsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public MyPostsModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public List<Item> ItemList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load posts for user with ID '{_userManager.GetUserId(User)}'.");
            }

            IQueryable<Item> items = _context.Item.AsQueryable();

            // Get the items that posted by current logged in user
            ItemList = items.Where(i => i.SellerId == user.Id).ToList();

            return Page();
        }
    }
}
