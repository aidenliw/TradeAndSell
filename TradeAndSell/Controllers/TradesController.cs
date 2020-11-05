using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TradeAndSell.Data;
using TradeAndSell.Models;

namespace TradeAndSell.Controllers
{
    [Authorize(Roles = "Admin, Member")]
    public class TradesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public TradesController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [Authorize(Roles = "Admin")]
        // GET: Trades
        public async Task<IActionResult> Index()
        {
            return View(await _context.Trade.ToListAsync());
        }

        // GET: Trades/RequestDetails/5
        public async Task<IActionResult> RequestDetails(int? id)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            Trade trade = await _context.Trade
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trade == null)
            {
                return NotFound();
            }
            IQueryable<Item> items = _context.Item.AsQueryable();
            Item targetItem = items.Where(i => i.Id == trade.ItemId).FirstOrDefault();
            List<int> tradeItemIds = trade.TradeItemIds.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            List<Item> tradeItems = items.Where(i => tradeItemIds.Contains(i.Id)).ToList();

            int totalMinTradePrice = tradeItems.Sum(ti => (int)ti.MinTradePrice);
            int totalMaxTradePrice = tradeItems.Sum(ti => (int)ti.MaxTradePrice);

            IQueryable<ApplicationUser> users = _userManager.Users.AsQueryable();

            TradeDetails tradeDetails = new TradeDetails
            {
                TradeId = trade.Id,
                ItemId = targetItem.Id,
                ItemName = targetItem.Title,
                ItemImagePath = targetItem.ImagePath,
                TradeItemIds = trade.TradeItemIds,
                TradeItemIdList = trade.TradeItemIds.Split(',', StringSplitOptions.RemoveEmptyEntries),
                Status = trade.ApproveStatus,
                SellerId = trade.SellerId,
                SellerName = users.Where(u => u.Id == trade.SellerId).Select(u => u.DisplayName).FirstOrDefault(),
                BuyerId = trade.BuyerId,
                BuyerName = users.Where(u => u.Id == trade.BuyerId).Select(u => u.DisplayName).FirstOrDefault(),
                Message = trade.Message,
                Date = trade.Date,
                MyRequest = user.Id == trade.BuyerId ? true : false
            };

            ViewData["TargetItem"] = targetItem;
            ViewData["TradeItems"] = tradeItems;
            ViewData["TradeDetails"] = tradeDetails;
            ViewData["MinTradePrice"] = totalMinTradePrice;
            ViewData["MaxTradePrice"] = totalMaxTradePrice;

            return View(trade);
        }

        // GET: Trades/OfferDetails/5
        public async Task<IActionResult> OfferDetails(int? id)
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (id == null)
            {
                return NotFound();
            }

            Trade trade = await _context.Trade
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trade == null)
            {
                return NotFound();
            }
            IQueryable<Item> items = _context.Item.AsQueryable();
            Item targetItem = items.Where(i => i.Id == trade.ItemId).FirstOrDefault();
            List<int> tradeItemIds = trade.TradeItemIds.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
            List<Item> tradeItems = items.Where(i => tradeItemIds.Contains(i.Id)).ToList();

            int totalMinTradePrice = tradeItems.Sum(ti => (int)ti.MinTradePrice);
            int totalMaxTradePrice = tradeItems.Sum(ti => (int)ti.MaxTradePrice);

            IQueryable<ApplicationUser> users = _userManager.Users.AsQueryable();

            TradeDetails tradeDetails = new TradeDetails
            {
                TradeId = trade.Id,
                ItemId = targetItem.Id,
                ItemName = targetItem.Title,
                ItemImagePath = targetItem.ImagePath,
                TradeItemIds = trade.TradeItemIds,
                TradeItemIdList = trade.TradeItemIds.Split(',', StringSplitOptions.RemoveEmptyEntries),
                Status = trade.ApproveStatus,
                SellerId = trade.SellerId,
                SellerName = users.Where(u => u.Id == trade.SellerId).Select(u => u.DisplayName).FirstOrDefault(),
                BuyerId = trade.BuyerId,
                BuyerName = users.Where(u => u.Id == trade.BuyerId).Select(u => u.DisplayName).FirstOrDefault(),
                ChatId = trade.ChatId,
                Message = trade.Message,
                Date = trade.Date,
                MyRequest = user.Id == trade.BuyerId ? true : false
            };

            ViewData["TargetItem"] = targetItem;
            ViewData["TradeItems"] = tradeItems;
            ViewData["TradeDetails"] = tradeDetails;
            ViewData["MinTradePrice"] = totalMinTradePrice;
            ViewData["MaxTradePrice"] = totalMaxTradePrice;

            return View(trade);
        }

        // POST: Trades/TradeRequest
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TradeRequest([Bind("Id,ItemId,SellerId,TradeItemIds,Message,ApproveStatus")] Trade trade)
        {
            // Get user id, if not logged in, return to login page
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            if (ModelState.IsValid)
            {
                string chatId = Guid.NewGuid().ToString();

                trade.ChatId = chatId;
                trade.BuyerId = userId;
                trade.Date = DateTime.Now;
                _context.Add(trade);

                
                Message newMessage = new Message()
                {
                    ChatId = chatId,
                    SenderId = userId,
                    ReceiverId = trade.SellerId,
                    Content = trade.Message,
                    Datetime = DateTime.Now
                };

                _context.Add(newMessage);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("/Account/Manage/MyTradeRequests", new { area = "Identity" });
        }

        // POST: Trades/TradeAccept/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TradeAccept(int id, [Bind("Id,ChatId,SellerId,BuyerId,Message")] Trade trade)
        {
            if (id != trade.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    IQueryable<Trade> trades = _context.Trade.AsQueryable();
                    Trade theTrade = trades.Where(t => t.Id == trade.Id).FirstOrDefault();
                    theTrade.ApproveStatus = "Approved";
                    _context.Entry(theTrade).Property("ApproveStatus").IsModified = true;
                    //_context.Update(trade);
                    
                    Message newMessage = new Message()
                    {
                        ChatId = trade.ChatId,
                        SenderId = trade.SellerId,
                        ReceiverId = trade.BuyerId,
                        Content = trade.Message,
                        Datetime = DateTime.Now
                    };

                    _context.Add(newMessage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TradeExists(trade.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToPage("/Account/Manage/MyTradeRequests", new { area = "Identity" });
        }

        // POST: Trades/TradeRefuse/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TradeRefuse(int id, [Bind("Id,ChatId,SellerId,BuyerId,Message")] Trade trade)
        {
            if (id != trade.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    IQueryable<Trade> trades = _context.Trade.AsQueryable();
                    Trade theTrade = trades.Where(t => t.Id == trade.Id).FirstOrDefault();
                    theTrade.ApproveStatus = "Refused";
                    _context.Entry(theTrade).Property("ApproveStatus").IsModified = true;
                    //_context.Update(trade);

                    Message newMessage = new Message()
                    {
                        ChatId = trade.ChatId,
                        SenderId = trade.SellerId,
                        ReceiverId = trade.BuyerId,
                        Content = trade.Message,
                        Datetime = DateTime.Now
                    };

                    _context.Add(newMessage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TradeExists(trade.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToPage("/Account/Manage/MyTradeRequests", new { area = "Identity" });
        }


        // GET: Trades/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ItemId,SellerId,BuyerId,TradeItemIds,Date,ChatId,Message,ApproveStatus")] Trade trade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trade);
        }

        [Authorize(Roles = "Admin")]
        // GET: Trades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trade = await _context.Trade.FindAsync(id);
            if (trade == null)
            {
                return NotFound();
            }
            return View(trade);
        }

        // POST: Trades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ItemId,SellerId,BuyerId,TradeItemIds,Date,ChatId,Message,ApproveStatus")] Trade trade)
        {
            if (id != trade.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TradeExists(trade.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(trade);
        }

        [Authorize(Roles = "Admin")]
        // GET: Trades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trade = await _context.Trade
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trade == null)
            {
                return NotFound();
            }

            return View(trade);
        }

        // POST: Trades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trade = await _context.Trade.FindAsync(id);
            _context.Trade.Remove(trade);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TradeExists(int id)
        {
            return _context.Trade.Any(e => e.Id == id);
        }
    }
}
