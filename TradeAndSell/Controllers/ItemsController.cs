using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TradeAndSell.Data;
using TradeAndSell.Models;

namespace TradeAndSell.Controllers
{
    public class ItemsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment hostingEnvironment;

        public ItemsController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IWebHostEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _context = context;
            this.hostingEnvironment = hostingEnvironment;
        }

        // GET: Items
        public async Task<IActionResult> Index()
        {
            return View(await _context.Item.ToListAsync());
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        [Authorize(Roles = "Admin, Member")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SellerId,Title,Condition,Description,Category,City,Image,Price,Quantity,AllowTrade,MinTradePirce,MaxTradePrice")] ItemManagement item)
        {
            if (ModelState.IsValid)
            {
                // get current logged in user's id
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                item.SellerId = userId;

                string uniqueFileName = null;
                if (item.Image != null) {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + item.Image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    item.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                Item newItem = new Item
                {
                    Id = item.Id,
                    SellerId = userId,
                    Title = item.Title,
                    Condition = item.Condition,
                    Description = item.Description,
                    Category = item.Category,
                    City = item.City,
                    ImagePath = uniqueFileName,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    AllowTrade = item.AllowTrade,
                    MinTradePirce = item.MinTradePirce,
                    MaxTradePrice = item.MaxTradePrice
                };

                _context.Add(newItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Items/Edit/5
        [Authorize(Roles = "Admin, Member")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ItemManagement newitem = new ItemManagement
            {
                Id = item.Id,
                SellerId = item.SellerId,
                Title = item.Title,
                Condition = item.Condition,
                Description = item.Description,
                Category = item.Category,
                City = item.City,
                Image = null,
                Price = item.Price,
                Quantity = item.Quantity,
                AllowTrade = item.AllowTrade,
                MinTradePirce = item.MinTradePirce,
                MaxTradePrice = item.MaxTradePrice
            };
            return View(newitem);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SellerId,Title,Condition,Description,Category,City,Image,Price,Quantity,AllowTrade,MinTradePirce,MaxTradePrice")] ItemManagement item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string uniqueFileName = null;
                    if (item.Image != null)
                    {
                        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + item.Image.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        item.Image.CopyTo(new FileStream(filePath, FileMode.Create));
                    }

                    Item newItem = new Item
                    {
                        Id = item.Id,
                        SellerId = item.SellerId,
                        Title = item.Title,
                        Condition = item.Condition,
                        Description = item.Description,
                        Category = item.Category,
                        City = item.City,
                        ImagePath = uniqueFileName,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        AllowTrade = item.AllowTrade,
                        MinTradePirce = item.MinTradePirce,
                        MaxTradePrice = item.MaxTradePrice
                    };

                    _context.Update(newItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
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
            return View(item);
        }

        // GET: Items/Delete/5
        [Authorize(Roles = "Admin, Member")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Item.FindAsync(id);
            _context.Item.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.Id == id);
        }
    }
}
