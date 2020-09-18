using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MainPr.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MainPr.Controllers
{
    public class CartsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationContext _context;

        public CartsController(ApplicationContext context, UserManager<User> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Carts
        public async Task<IActionResult> Index(string id)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            id = user?.Id;

            var applicationContext = _context.Carts
                .Include(c => c.Items)
                .Include(c => c.Orders)
                .Include(c => c.StatusCarts)
                .Where(c => c.Orders.UserId == id);
            return View(await applicationContext.ToListAsync());
        }

        public async Task<IActionResult> AddToCartAsync(string idUser, int id)
        {

            User user = await _userManager.GetUserAsync(HttpContext.User);
            idUser = user?.Id;

            Order order = new Order();
            order.UserId = idUser;

            _context.Add(order);
            _context.SaveChanges();

            int idOrder = order.OrderID; // Yes it's here

            var item = await _context.Items.FindAsync(id);

            Cart cart = new Cart();
            cart.ItemID = item.ItemID;
            cart.Price = item.Price;
            cart.StatusCartID = 1;
            cart.Count_item = 1;
            cart.OrderID = idOrder;

            var check = _context.Carts
                .Include(c => c.Items)
                .Include(c => c.Orders)
                .Include(c => c.StatusCarts)
                .Where(c => c.Orders.UserId == idUser)
                .FirstOrDefault(c =>c.ItemID == item.ItemID);

            if(check == null)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else if(cart.ItemID == check.ItemID)
            {
                return RedirectToAction(nameof(Index));
            }


            return RedirectToAction(nameof(Index));
        }











        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.Items)
                .Include(c => c.Orders)
                .Include(c => c.StatusCarts)
                .FirstOrDefaultAsync(m => m.ItemID == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "ItemID");
            ViewData["OrderID"] = new SelectList(_context.Orders, "OrderID", "OrderID");
            ViewData["StatusCartID"] = new SelectList(_context.StatusCarts, "StatusCartID", "StatusCartID");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ItemID,OrderID,Count_item,Price,StatusCartID")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "ItemID", cart.ItemID);
            ViewData["OrderID"] = new SelectList(_context.Orders, "OrderID", "OrderID", cart.OrderID);
            ViewData["StatusCartID"] = new SelectList(_context.StatusCarts, "StatusCartID", "StatusCartID", cart.StatusCartID);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id, int order)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id, order);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "ItemID", cart.ItemID);
            ViewData["OrderID"] = new SelectList(_context.Orders, "OrderID", "OrderID", cart.OrderID);
            ViewData["StatusCartID"] = new SelectList(_context.StatusCarts, "StatusCartID", "StatusCartID", cart.StatusCartID);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ItemID,OrderID,Count_item,Price,StatusCartID")] Cart cart)
        {
            if (id != cart.ItemID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.ItemID))
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
            ViewData["ItemID"] = new SelectList(_context.Items, "ItemID", "ItemID", cart.ItemID);
            ViewData["OrderID"] = new SelectList(_context.Orders, "OrderID", "OrderID", cart.OrderID);
            ViewData["StatusCartID"] = new SelectList(_context.StatusCarts, "StatusCartID", "StatusCartID", cart.StatusCartID);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int id, int order)
        {
            var cart = await _context.Carts
                .Include(c => c.Items)
                .Include(c => c.Orders)
                .Include(c => c.StatusCarts)
                .Where(c=> c.OrderID == order)
                .FirstOrDefaultAsync(m => m.ItemID == id);

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int order)
        {
            var cart = await _context.Carts
                .Where(c => c.OrderID == order)
                .FirstOrDefaultAsync(m => m.ItemID == id);

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.ItemID == id);
        }
    }
}
