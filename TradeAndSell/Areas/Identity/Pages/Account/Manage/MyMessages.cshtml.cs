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
    public class MyMessagesModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public MyMessagesModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public List<MessageDetails> MessageList { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load messages for user with ID '{_userManager.GetUserId(User)}'.");
            }

            IQueryable<Message> messages = _context.Message.AsQueryable();
            IQueryable<ApplicationUser> users = _userManager.Users.AsQueryable();

            // Get the messages for current logged in user
            //MessageList = messages.Where(m => m.ReceiverId == user.Id)
            //    .GroupBy(m => m.SenderId)
            //    .Select(m => m.OrderByDescending(o => o.Datetime).FirstOrDefault())
            //    .ToList();
            var test = messages.GroupBy(m => m.ChatId).Select(m => m.OrderByDescending(o => o.Datetime).First());

            MessageList = messages.Where(m => m.ReceiverId == user.Id && m.Content != null).OrderByDescending(m => m.Datetime).Select(m => new MessageDetails { 
                Id = m.Id,
                ChatId = m.ChatId,
                SenderId = m.SenderId,
                SenderName = users.Where(u => u.Id == m.SenderId).Select(m => m.DisplayName).FirstOrDefault(),
                ReceiverId = m.ReceiverId,
                ReceiverName = users.Where(u => u.Id == m.ReceiverId).Select(m => m.DisplayName).FirstOrDefault(),
                Content = m.Content,
                Datetime = m.Datetime
            }).ToList();


            return Page();
        }
    }
}
