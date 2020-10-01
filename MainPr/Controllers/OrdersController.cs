using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MainPr.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace MainPr.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationContext _context;

        public OrdersController(ApplicationContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<string> TakeUserIDAsync()
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
             string UserID = user?.Id;

            return UserID;
        }

        // GET: Orders
        public async Task<IActionResult> Index(string UserID)
        {
            UserID = await TakeUserIDAsync();

            var applicationContext = _context.Orders
                .Include(o => o.Items)
                .Include(o => o.UsersOrders)
                .Where(o => o.UsersOrders.UserId == UserID)
                .Where(o =>o.StatusOrderID == 1);
            return View(await applicationContext.ToListAsync());
        }

        public async Task<IActionResult> AddToOtrderAsync(string UserID, int id)
        {

            UserID = await TakeUserIDAsync();

            if (UserID == null)
            {
                return RedirectToAction("Login", "Account");
            }

            UsersOrder order = new UsersOrder
            {
                UserId = UserID
            };

            _context.Add(order);
            _context.SaveChanges();

            int idOrder = order.UsersOrderID; // Yes it's here

            var item = await _context.Items.FindAsync(id);

            //Orders cart = new Orders
            //{
            //    ItemID = item.ItemID,
            //    CountBuy_item = 1,
            //    Price = item.Price * CountBuy_item,
            //    StatusOrderID = 1,
            //    UsersOrderID = idOrder
            //};

            Orders cart = new Orders();
            cart.ItemID = item.ItemID;
            cart.CountBuy_item = 1;
            cart.Price = item.Price * cart.CountBuy_item;
            cart.StatusOrderID = 1;
            cart.UsersOrderID = idOrder;

            var check = _context.Orders
                .Include(c => c.Items)
                .Include(c => c.UsersOrders)
                .Where(c => c.UsersOrders.UserId == UserID)
                .Where(o => o.StatusOrderID == 1)
                .FirstOrDefault(c => c.ItemID == item.ItemID);

            if (check == null)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else if (cart.ItemID == check.ItemID && cart.StatusOrderID == check.StatusOrderID)
            {
                check.CountBuy_item += 1;
                check.Price += cart.Price;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }


            return RedirectToAction("Index","Items");
        }



        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .Include(o => o.Items)
                .Include(o => o.UsersOrders)
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "ItemID");
            ViewData["UsersOrderID"] = new SelectList(_context.UsersOrders, "UsersOrderID", "UsersOrderID");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemID,UsersOrderID,CartID,CountBuy_item,Price")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orders);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "ItemID", orders.ItemID);
            ViewData["UsersOrderID"] = new SelectList(_context.UsersOrders, "UsersOrderID", "UsersOrderID", orders.UsersOrderID);
            return View(orders);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id, int? userId)
        {
            if (id == null )
            {
                return NotFound();
            }

            var orders = await _context.Orders.FindAsync(id, userId);
            if (orders == null)
            {
                return NotFound();
            }
            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "ItemID", orders.ItemID);
            ViewData["UsersOrderID"] = new SelectList(_context.UsersOrders, "UsersOrderID", "UsersOrderID", orders.UsersOrderID);
            return View(orders);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Orders orders, Item items)
        {
            if (id != orders.ItemID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Item item = await _context.Items.FindAsync(id);
                    orders.Price = item.Price * orders.CountBuy_item;
                    _context.Update(orders);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdersExists(orders.ItemID))
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
            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "ItemID", orders.ItemID);
            ViewData["UsersOrderID"] = new SelectList(_context.UsersOrders, "UsersOrderID", "UsersOrderID", orders.UsersOrderID);
            return View(orders);
        }



        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id, int? userId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .Include(o => o.Items)
                .Include(o => o.UsersOrders)
                .Where(o => o.UsersOrderID == userId)
                .FirstOrDefaultAsync(m => m.ItemID == id);

            _context.Orders.Remove(orders);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Orders/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var orders = await _context.Orders.FindAsync(id);
        //    _context.Orders.Remove(orders);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool OrdersExists(int id)
        {
            return _context.Orders.Any(e => e.ItemID == id);
        }
    }
}
