using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReviewASP.Data;
using ReviewASP.Models;

namespace ReviewASP.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders > List of orders page
        public async Task<IActionResult> Index()
        {
            // Admins should be able to se everything
            if (User.IsInRole("Administrator"))
            {
                //看到这个所有的
                return View(await _context.Orders.OrderByDescending(o => o.OrderDate).ToListAsync());
            }
            // Customers must only see their own orders
            else
            {
                //这里为我的project做准备，选一个
                var rndGen = new Random();
                int random1 = rndGen.Next(0, _context.Orders.Count()/2);
                int random2 = rndGen.Next(_context.Orders.Count()/2,_context.Orders.Count());
                return View(await _context.Orders
                                            .Where(o => o.CustomerId == User.Identity.Name)
                                            .OrderByDescending(o => o.OrderDate)
                                            .Skip(random1)
                                            .Take(2)
                                            .Skip(random2)
                                            .Take(1)
                                            .ToListAsync()
                                            );
            }
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Include products and order  details records
            //在这里要注意，这一行await _context.Orders，是loading了所有的orders
            var order = await _context.Orders
                                //这一行是说，有相关的orderDetails entities
                                .Include(o => o.OrderDetails)
                                //从orderDetails 在到product
                                .ThenInclude(o => o.Product)
                                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            // add security if not admin and not owner of this order, return unauthorized message
            if (!User.IsInRole("Administrator") && User.Identity.Name != order.CustomerId)
            {
                return Unauthorized();
            }

            return View(order);
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}