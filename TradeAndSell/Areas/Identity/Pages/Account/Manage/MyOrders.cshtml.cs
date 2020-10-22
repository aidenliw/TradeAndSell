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
    public class MyOrdersModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public MyOrdersModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public List<Item> ItemList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load orders for user with ID '{_userManager.GetUserId(User)}'.");
            }
            // Get all orders and items from database
            IQueryable<Order> orders = _context.Order.AsQueryable();
            IQueryable<Item> items = _context.Item.AsQueryable();

            // Filter the orders that made by the user and display the items
            IQueryable<Order> OrderList = orders.Where(o => o.BuyerId == user.Id).AsQueryable();
            ItemList = items.Where(i => OrderList.Any(o => o.ItemId == i.Id)).ToList();

            return Page();
        }
    }
}
