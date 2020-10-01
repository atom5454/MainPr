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
using Microsoft.AspNetCore.Authorization;

namespace MainPr.Controllers
{
    [Authorize]
    public class CartsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationContext _context;

        public CartsController(ApplicationContext context, UserManager<User> userManager)
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

        //GET: Carts
        public async Task<IActionResult> Index(string UserID)
        {
            UserID = await TakeUserIDAsync();

            var applicationContext = _context.Carts
                .Include(c => c.StatusCarts)
                .Include(c => c.Orders.Items)
                .Include(c => c.Orders)
                .Where(c => c.Orders.UsersOrders.UserId == UserID);
            return View(await applicationContext.ToListAsync());
        }

        [Obsolete]
        public async Task<IActionResult> AddToCartAsync( string UserID, int query)
        {
            UserID = await TakeUserIDAsync();

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
                    .Where(o => o.UsersOrders.UserId == UserID);
                var orders = await applicationContext.ToListAsync();


                (from p in _context.Orders
                 where p.StatusOrderID == 1
                 where p.UsersOrders.UserId == UserID
                 select p).ToList().ForEach(x => x.StatusOrderID = 2);

                _context.SaveChanges();


                foreach (var item in orders)
                {
                    Cart x = new Cart
                    {
                        CartID = query,
                        StatusCartID = 1,
                        UsersOrderID = item.UsersOrderID,
                        ItemID = item.ItemID
                    };
                    _context.Add(x);
                    _context.SaveChanges();
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
        public async Task<IActionResult> Delete(int? idcart, int? iditem, int? iduserorder)
        {

            var cart = await _context.Carts
                .Include(c => c.StatusCarts)
                //.Where(c => c.UsersOrderID == iduserorder)
                //.Where(c => c.ItemID == iditem)
                .FirstOrDefaultAsync(m => m.CartID == idcart);

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
