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
    public class MessagesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public MessagesController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Messages/Chat/5
        public async Task<IActionResult> Chat(string chatId)
        {
            if (chatId == null)
            {
                return NotFound();
            }
            ApplicationUser user = await _userManager.GetUserAsync(User);
            IQueryable<ApplicationUser> users = _userManager.Users.AsQueryable();

            IQueryable<MessageDetails> messages = _context.Message.Where(m => m.ChatId == chatId).OrderBy(m => m.Datetime)
                .Select(m => new MessageDetails {
                    Id = m.Id,
                    ChatId = m.ChatId,
                    SenderId = m.SenderId,
                    SenderName = users.Where(u => u.Id == m.SenderId).Select(m => m.DisplayName).FirstOrDefault(),
                    ReceiverId = m.ReceiverId,
                    ReceiverName = users.Where(u => u.Id == m.ReceiverId).Select(m => m.DisplayName).FirstOrDefault(),
                    Content = m.Content,
                    Datetime = m.Datetime
                });
            if (messages == null)
            {
                return NotFound();
            }

            Message senderInfo = _context.Message.Where(m => m.SenderId == user.Id).FirstOrDefault();
            Message receiverInfo = _context.Message.Where(m => m.ReceiverId == user.Id).FirstOrDefault();
            ViewData["SenderInfo"] = senderInfo;
            ViewData["ReceiverInfo"] = receiverInfo;
            ViewData["CurrentChatId"] = chatId;
            // Get the name of the chat user
            ViewData["TargetUser"] = users.Where(u => u.Id == receiverInfo.SenderId).Select(m => m.DisplayName).FirstOrDefault();
            
            ViewData["MessageDetails"] = await messages.ToListAsync();

            return View();
        }

        // POST: Messages/SendChat
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendChat([Bind("Id,ChatId,SenderId,ReceiverId,Content")] Message message)
        {
            if (ModelState.IsValid)
            {
                message.Datetime = DateTime.Now;
                _context.Add(message);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Chat", new { chatId = message.ChatId });
        }

        // POST: Carts/DeleteChat/5
        public async Task<IActionResult> DeleteChat(string chatId)
        {
            string userId = _userManager.GetUserId(User);
            if (chatId == null)
            {
                return NotFound();
            }

            var messages = _context.Message
                .Where(m => m.ChatId == chatId);
            if (messages == null)
            {
                return NotFound();
            }
            else
            {
                _context.Message.RemoveRange(messages);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Account/Manage/MyMessages", new { area = "Identity" });
        }

        [Authorize(Roles = "Admin")]
        // GET: Messages
        public async Task<IActionResult> Index()
        {
            return View(await _context.Message.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        [Authorize(Roles = "Admin")]
        // GET: Messages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ChatId,SenderId,ReceiverId,Content,Datetime")] Message message)
        {
            if (ModelState.IsValid)
            {
                _context.Add(message);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(message);
        }

        [Authorize(Roles = "Admin")]
        // GET: Messages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message.FindAsync(id);
            if (message == null)
            {
                return NotFound();
            }
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ChatId,SenderId,ReceiverId,Content,Datetime")] Message message)
        {
            if (id != message.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(message);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MessageExists(message.Id))
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
            return View(message);
        }

        [Authorize(Roles = "Admin")]
        // GET: Messages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var message = await _context.Message
                .FirstOrDefaultAsync(m => m.Id == id);
            if (message == null)
            {
                return NotFound();
            }

            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var message = await _context.Message.FindAsync(id);
            _context.Message.Remove(message);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MessageExists(int id)
        {
            return _context.Message.Any(e => e.Id == id);
        }
    }
}
