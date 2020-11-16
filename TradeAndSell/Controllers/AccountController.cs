using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TradeAndSell.Data;
using TradeAndSell.Models;

namespace TradeAndSell.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> RoleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = RoleManager;
            _context = context;
        }

        // GET: Account/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ApplicationUser currentUser = await _userManager.GetUserAsync(User);
            IQueryable<ApplicationUser> users = _userManager.Users;

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            IQueryable<Item> items = _context.Item.Where(i => i.SellerId == user.Id);

            ViewData["CurrentUserID"] = currentUser.Id; ;
            ViewData["UserPosts"] = items;

            return View(user);
        }

        // GET: Account/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _userManager.FindByIdAsync(id);

            if (account == null)
            {
                return NotFound();
            }
            else
            {
                var result = await _userManager.DeleteAsync(account);

                if (result.Succeeded)
                {
                    return RedirectToPage("/Account/Manage/MyAccounts", new { area = "Identity" });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return RedirectToPage("/Account/Manage/MyAccounts", new { area = "Identity" });
            }
        }

        //Seed the database with users, roles and assign users to roles. To call this method, use https://localhost:44387/Account/SeedUserData
        public async Task<IActionResult> SeedUserData()
        {
            //Variable to hold the status of our identity operations
            IdentityResult result;

            //Create 2 new roles (Member, Admin)
            if (await _roleManager.RoleExistsAsync("Member") == false)
            {
                result = await _roleManager.CreateAsync(new IdentityRole("Member"));
                if (!result.Succeeded)
                    return View("Error", new ErrorViewModel { RequestId = "Failed to add Member role" });
            }

            if (await _roleManager.RoleExistsAsync("Admin") == false)
            {
                result = await _roleManager.CreateAsync(new IdentityRole("Admin"));
                if (!result.Succeeded)
                    return View("Error", new ErrorViewModel { RequestId = "Failed to add Admin role" });
            }

            //Create a list of Member
            List<ApplicationUser> MemberList = new List<ApplicationUser>();

            //Sample Member user
            MemberList.Add(new ApplicationUser
            {
                Email = "member@email.com",
                UserName = "member@email.com",
                DisplayName = "Member One",
                FirstName = "Member",
                LastName = "One",
                EmailConfirmed = true
            });

            foreach (ApplicationUser member in MemberList)
            {
                //Create the new user with password "Mohawk1!"
                result = await _userManager.CreateAsync(member, "Mohawk1!");
                if (!result.Succeeded)
                    return View("Error", new ErrorViewModel { RequestId = "Failed to add new member user" });
                //Assign the new user to the Member role
                result = await _userManager.AddToRoleAsync(member, "Member");
                if (!result.Succeeded)
                    return View("Error", new ErrorViewModel { RequestId = "Failed to assign member role" });

            }

            //Create a list of admins
            List<ApplicationUser> AdminsList = new List<ApplicationUser>();

            //Sample admin user
            AdminsList.Add(new ApplicationUser
            {
                Email = "admin@email.com",
                UserName = "admin@email.com",
                DisplayName = "Admin One",
                FirstName = "Admin",
                LastName = "One",
                EmailConfirmed = true
            });

            foreach (ApplicationUser admin in AdminsList)
            {
                //Create the new user with password "Mohawk1!"
                result = await _userManager.CreateAsync(admin, "Mohawk1!");
                if (!result.Succeeded)
                    return View("Error", new ErrorViewModel { RequestId = "Failed to add new admin user" });
                //Assign the new user to the admin role
                result = await _userManager.AddToRoleAsync(admin, "Admin");
                if (!result.Succeeded)
                    return View("Error", new ErrorViewModel { RequestId = "Failed to assign admin role" });

            }



            //If we are here, everything executed according to plan, so we will show a success message
            return Content("Users setup completed.\n\n" +
                "Admin Account:\n" +
                "Username = admin@email.com\n" +
                "Password = Mohawk1!\n\n" +
                "Member Account:\n" +
                "Username = member@email.com\n" +
                "Password = Mohawk1!\n");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
