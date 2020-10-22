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
    public class CartsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public CartsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Cart
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            IQueryable<Item> items = _context.Item.AsQueryable();
            IQueryable<Cart> cartItems = _context.Cart.AsQueryable().Where(c => c.OwnerId == user.Id);
            IQueryable<Item> itemsInCart = items.Where(i => cartItems.Any(c => c.ItemId == i.Id && c.OnWishList == false));

            return View(await itemsInCart.ToListAsync());
        }

        // GET: Wish List
        public async Task<IActionResult> WishList()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            IQueryable<Item> items = _context.Item.AsQueryable();
            IQueryable<Cart> cartItems = _context.Cart.AsQueryable().Where(c => c.OwnerId == user.Id);
            IQueryable<Item> itemsInWishList = items.Where(i => cartItems.Any(c => c.ItemId == i.Id && c.OnWishList == true));

            return View(await itemsInWishList.ToListAsync());
        }

        // POST: Carts/MoveToWishList/5
        public async Task<IActionResult> MoveToWishList(int? id)
        {
            string userId = _userManager.GetUserId(User);
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.ItemId == id && m.OwnerId == userId);
            if (cart == null)
            {
                return NotFound();
            }
            else
            {
                cart.OnWishList = true;
                _context.Entry(cart).Property("OnWishList").IsModified = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // POST: Carts/MoveToWishList/5
        public async Task<IActionResult> MoveToCart(int? id)
        {
            string userId = _userManager.GetUserId(User);
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.ItemId == id && m.OwnerId == userId);
            if (cart == null)
            {
                return NotFound();
            }
            else
            {
                cart.OnWishList = false;
                _context.Entry(cart).Property("OnWishList").IsModified = true;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Account/Manage/MyWishList", new { area = "Identity" });
        }

        // POST: Carts/Delete/5
        public async Task<IActionResult> DeleteFromCart(int? id)
        {
            string userId = _userManager.GetUserId(User);
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.ItemId == id && m.OwnerId == userId);
            if (cart == null)
            {
                return NotFound();
            }
            else
            {
                _context.Cart.Remove(cart);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }

        // POST: Carts/Delete/5
        public async Task<IActionResult> DeleteFromWishList(int? id)
        {
            string userId = _userManager.GetUserId(User);
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Cart
                .FirstOrDefaultAsync(m => m.ItemId == id && m.OwnerId == userId);
            if (cart == null)
            {
                return NotFound();
            }
            else
            {
                _context.Cart.Remove(cart);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Account/Manage/MyWishList", new { area = "Identity" });
        }
    }
}
