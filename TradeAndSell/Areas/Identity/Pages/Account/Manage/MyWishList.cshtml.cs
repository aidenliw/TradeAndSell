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
    public class MyWishListModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public MyWishListModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public List<Item> ItemsInWishList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load wish list for user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Get the items that added to wish list by current logged in user
            IQueryable<Item> items = _context.Item.AsQueryable();
            IQueryable<Cart> cartItems = _context.Cart.AsQueryable().Where(c => c.OwnerId == user.Id);
            ItemsInWishList = items.Where(i => cartItems.Any(c => c.ItemId == i.Id && c.OnWishList == true)).ToList();

            return Page();
        }
    }
}
