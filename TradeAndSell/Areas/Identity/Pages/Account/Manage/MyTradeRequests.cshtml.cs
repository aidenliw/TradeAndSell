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
    public class MyTradeRequestsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public MyTradeRequestsModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public List<TradeDetails> TradeDetails { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load posts for user with ID '{_userManager.GetUserId(User)}'.");
            }

            IQueryable<Item> items = _context.Item.AsQueryable();
            IQueryable<Trade> trades = _context.Trade.AsQueryable();
            IQueryable<ApplicationUser> users = _userManager.Users.AsQueryable();

            // Get trades for current user
            IQueryable<Trade> myTrades = trades.Where(t => t.SellerId == user.Id || t.BuyerId == user.Id);

            IQueryable<TradeDetails> tradeDetails = myTrades.Join(items, trade => trade.ItemId, item => item.Id, (trade, item) => new TradeDetails
            {
                TradeId = trade.Id,
                ItemId = item.Id,
                ItemName = item.Title,
                ItemImagePath = item.ImagePath,
                TradeItemIds = trade.TradeItemIds,
                TradeItemIdList = trade.TradeItemIds.Split(',', StringSplitOptions.RemoveEmptyEntries),
                //TradeItemFirstName = items.AsEnumerable().Where(i => i.Id == int.Parse(trade.TradeItemIds.Split(',', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault())).AsQueryable().Select(i => i.Title).FirstOrDefault(),
                Status = trade.ApproveStatus,
                SellerId = trade.SellerId,
                SellerName = users.Where(u => u.Id == trade.SellerId).Select(u => u.DisplayName).FirstOrDefault(),
                BuyerId = trade.BuyerId,
                BuyerName = users.Where(u => u.Id == trade.BuyerId).Select(u => u.DisplayName).FirstOrDefault(),
                Message = trade.Message,
                Date = trade.Date,
                MyRequest = user.Id == trade.BuyerId ? true : false
            });

            TradeDetails = tradeDetails.ToList();

            return Page();
        }

    }
}
