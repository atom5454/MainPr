using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MainPr.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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

        //GET: Carts
        public async Task<IActionResult> Index(string id)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            id = user?.Id;

            var applicationContext = _context.Carts
                .Include(c => c.StatusCarts)
                .Include(c => c.Orders.Items)
                .Include(c => c.Orders)
                .Where(c => c.Orders.UsersOrders.UserId == id);
            return View(await applicationContext.ToListAsync());
        }

        [Obsolete]
        public async Task<IActionResult> AddToCartAsync( string idUser, int query)
        {
            User user = await _userManager.GetUserAsync(HttpContext.User);
            idUser = user?.Id;


            //var test_orders = _context.Orders
            //    .Include(o => o.Items)
            //    .Include(o => o.UsersOrders)
            //    .Where(o => o.StatusOrderID == 1)
            //    .FirstOrDefault(o => o.UsersOrders.UserId == idUser);

            //var check = _context.Carts
            //    .Include(o => o.Orders)
            //    .Include(o => o.Orders.UsersOrders)
            //    .Where(o => o.ItemID == test_orders.ItemID)
            //    .FirstOrDefault(o => o.Orders.UsersOrders.UserId == idUser);

            //if(check == null)
            //{
                try
                {
                        query = (from e in _context.Carts
                            select e.CartID)
                            .Max();
                }
                catch
                {
                    query = 0;
                }

                query += 1;

                var applicationContext = _context.Orders
                    .Include(o => o.Items)
                    .Include(o => o.UsersOrders)
                    .Where(o => o.StatusOrderID == 1)
                    .Where(o => o.UsersOrders.UserId == idUser);
                var orders = await applicationContext.ToListAsync();


                (from p in _context.Orders
                 where p.StatusOrderID == 1
                 where p.UsersOrders.UserId == idUser
                 select p).ToList().ForEach(x => x.StatusOrderID = 2);

                _context.SaveChanges();


                foreach (var item in orders)
                {
                    Cart x = new Cart();
                    x.CartID = query;
                    x.StatusCartID = 2;
                    x.UsersOrderID = item.UsersOrderID;
                    x.ItemID = item.ItemID;
                    _context.Add(x);
                    _context.SaveChanges();
                }

                return RedirectToAction(nameof(Index));

            //}
            //else if (check.ItemID == test_orders.ItemID && check.Orders.UsersOrders.UserId == idUser && check.StatusCartID == 1)
            //{
            //    return RedirectToAction("Index", "Home");
            //}

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
                .Include(c => c.StatusCarts)
                .FirstOrDefaultAsync(m => m.CartID == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["StatusCartID"] = new SelectList(_context.StatusCarts, "StatusCartID", "StatusCartID");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartID,StatusCartID")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["StatusCartID"] = new SelectList(_context.StatusCarts, "StatusCartID", "StatusCartID", cart.StatusCartID);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }
            ViewData["StatusCartID"] = new SelectList(_context.StatusCarts, "StatusCartID", "StatusCartID", cart.StatusCartID);
            return View(cart);
        }

        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartID,StatusCartID")] Cart cart)
        {
            if (id != cart.CartID)
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
                    if (!CartExists(cart.CartID))
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
            ViewData["StatusCartID"] = new SelectList(_context.StatusCarts, "StatusCartID", "StatusCartID", cart.StatusCartID);
            return View(cart);
        }

        // GET: Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .Include(c => c.StatusCarts)
                .FirstOrDefaultAsync(m => m.CartID == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cart = await _context.Carts.FindAsync(id);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.CartID == id);
        }
    }
}
