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
using Stripe;
using Stripe.Checkout;
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
            StripeConfiguration.ApiKey = "sk_test_YXExkbOl2bE14RGjl6k96vIV002kRScUnG";
        }

        // GET: Items
        public async Task<IActionResult> Index(string search, string currentFilter, string catergaryFilter, string cityFilter, string sortBy)
        {
            ViewData["CatergaryFilter"] = catergaryFilter;
            ViewData["CityFilter"] = cityFilter;
            ViewData["SortBy"] = sortBy;

            if (search == null) { search = currentFilter; }
            ViewData["CurrentFilter"] = search;

            IQueryable<Item> items = _context.Item.AsQueryable();
            // Search item by the input
            if (!string.IsNullOrEmpty(search))
            {
                items = items.Where(p => p.Title.ToLower().Contains(search.ToLower())
                || p.Condition.ToLower().Contains(search.ToLower())
                || p.Description.ToLower().Contains(search.ToLower())
                || p.Category.ToLower().Contains(search.ToLower())
                || p.City.ToLower().Contains(search.ToLower()));
            }


            // Filter the item by categary
            switch (catergaryFilter)
            {
                case "All":
                    break;
                case "Books":
                    items = items.Where(i => i.Category == "Book");
                    break;
                case "Furniture":
                    items = items.Where(i => i.Category == "Furniture");
                    break;
                case "Electronics":
                    items = items.Where(i => i.Category == "Electronic");
                    break;
                default:
                    break;
            }

            // Filter the item by city
            switch (cityFilter)
            {
                case "All":
                    break;
                case "Toronto":
                    items = items.Where(i => i.City == "Toronto");
                    break;
                case "Mississauga":
                    items = items.Where(i => i.City == "Mississauga");
                    break;
                case "Hamilton":
                    items = items.Where(i => i.City == "Hamilton");
                    break;
                case "Burlington":
                    items = items.Where(i => i.City == "Burlington");
                    break;
                default:
                    break;
            }

            // Sort the item
            switch (sortBy)
            {
                case "Default":
                    items = items.OrderBy(i => i.CreatedDate);
                    break;
                case "Price: Low to High":
                    items = items.OrderBy(i => i.Price);
                    break;
                case "Price: High to Low":
                    items = items.OrderByDescending(i => i.Price);
                    break;
                case "Newest Arrivals":
                    items = items.OrderByDescending(i => i.CreatedDate);
                    break;
                default:
                    items = items.OrderBy(i => i.CreatedDate);
                    break;
            }
            return View(await items.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        // GET: Trades
        public async Task<IActionResult> IndexTable()
        {
            return View(await _context.Item.ToListAsync());
        }

        [HttpPost("create-checkout-session")]
        public ActionResult CreateCheckoutSession()
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = 100,
                        Currency = "cad",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Payment Demo",
                        },

                    },
                    Quantity = 1,
                    //Images = { hostingEnvironment.WebRootFileProvider.GetFileInfo("images/no-image-avaiable.jpg")?.PhysicalPath },
                    Description = "This payment function is deprecated, this is just a example show case"
                    },
                },
                Mode = "payment",
                SuccessUrl = "https://example.com/success",
                CancelUrl = "https://example.com/cancel",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Json(new { id = session.Id });
        }

        [Authorize(Roles = "Admin, Member")]
        public IActionResult Trade(int id)
        {
            // Get user id, if not logged in, return to login page
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            // Get selected item details
            IQueryable<Item> items = _context.Item.AsQueryable();
            Item selectedItem = items.Where(i => i.Id == id).FirstOrDefault();

            if (selectedItem == null)
            {
                return NotFound();
            }

            // Get the items that posted by current logged in user
            List<Item> myItems = items.Where(i => i.SellerId == userId).OrderBy(i => i.Title).ToList();

            //var tuple = new Tuple<Item, List<Item>, Trade>(selectedItem, myItems, null);

            ViewData["SelectedItem"] = selectedItem;
            ViewData["TradeItems"] = myItems;

            return View();
        }

        [Authorize(Roles = "Admin, Member")]
        public IActionResult AddToCart(int id) 
        {
            // Get user id, if not logged in, return to login page
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            // Get item details
            IQueryable<Item> items = _context.Item.AsQueryable();
            Item selectedItem = items.Where(i => i.Id == id).FirstOrDefault();

            // Add the item to the cart of current logged in user
            Cart newItem = new Cart()
            {
                ItemId = selectedItem.Id,
                OwnerId = userId,
                OnWishList = false
            };
            _context.Add(newItem);
            _context.SaveChanges();

            return RedirectToAction("Index", "Carts");
        }

        [Authorize(Roles = "Admin, Member")]
        public IActionResult AddToWishList(int id)
        {
            // Get user id, if not logged in, return to login page
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            // Get item details
            IQueryable<Item> items = _context.Item.AsQueryable();
            Item selectedItem = items.Where(i => i.Id == id).FirstOrDefault();

            // Add the item to the cart of current logged in user
            Cart newItem = new Cart()
            {
                ItemId = selectedItem.Id,
                OwnerId = userId,
                OnWishList = true
            };
            _context.Add(newItem);
            _context.SaveChanges();

            //return RedirectToAction("WishList", "Carts");
            return RedirectToPage("/Account/Manage/MyWishList", new { area = "Identity" });
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
            ApplicationUser user = await _userManager.GetUserAsync(User);
            ApplicationUser sellerInfo = _userManager.Users.Where(u => u.Id == item.SellerId).FirstOrDefault();
            ViewData["SellerInfo"] = sellerInfo;

            if (user != null && user.Id == item.SellerId)
            {
                ViewData["MyItem"] = "true";
            }
            else 
            {
                ViewData["MyItem"] = "false";
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
        public async Task<IActionResult> Create([Bind("Id,SellerId,Title,Condition,Description,Category,City,Image,Price,Quantity,AllowTrade,MinTradePrice,MaxTradePrice,CreatedDate")] ItemManagement item)
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
                    MinTradePrice = item.MinTradePrice,
                    MaxTradePrice = item.MaxTradePrice,
                    CreatedDate = DateTime.Now
                };

                _context.Add(newItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToPage("/Account/Manage/MyPosts", new { area = "Identity" });
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
                MinTradePrice = item.MinTradePrice,
                MaxTradePrice = item.MaxTradePrice,
                CreatedDate = item.CreatedDate
            };
            return View(newitem);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SellerId,Title,Condition,Description,Category,City,Image,Price,Quantity,AllowTrade,MinTradePrice,MaxTradePrice,CreatedDate")] ItemManagement item)
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
                        MinTradePrice = item.MinTradePrice,
                        MaxTradePrice = item.MaxTradePrice,
                        CreatedDate = item.CreatedDate
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
        [Authorize(Roles = "Admin")]
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

        // POST: Carts/Delete/5
        public async Task<IActionResult> DeleteFromPosts(int? id)
        {
            string userId = _userManager.GetUserId(User);
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Item
                .FirstOrDefaultAsync(m => m.Id == id && m.SellerId == userId);
            if (item == null)
            {
                return NotFound();
            }
            else
            {
                _context.Item.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Account/Manage/MyPosts", new { area = "Identity" });
        }
    }
}
