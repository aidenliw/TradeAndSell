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
    public class MyAccountsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public MyAccountsModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public List<ApplicationUser> UserList { get; set; }

        [BindProperty]
        public string CurrentUserID { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load posts for user with ID '{_userManager.GetUserId(User)}'.");
            }

            CurrentUserID = user.Id;

            IQueryable<ApplicationUser> users = _userManager.Users;

            // Get the items that posted by current logged in user
            UserList = users.OrderBy(u => u.DisplayName).ToList();

            return Page();
        }
    }
}
