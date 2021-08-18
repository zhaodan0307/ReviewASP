using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReviewASP.Data;
using ReviewASP.Models;
using ReviewASP.Extentions;
using Microsoft.Extensions.Configuration;

// Import necessary packages
using Stripe;
using Stripe.Checkout;



namespace ReviewASP.Controllers
{
    public class StoreController : Controller
    {
        private readonly ApplicationDbContext _context;

        private IConfiguration _configuration;

        public StoreController(ApplicationDbContext context, IConfiguration configuration)
        {
            //注入了一个configuration还有一个database
            _context = context;
            _configuration = configuration;
        }

        // GET: Store
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.OrderBy(c => c.Name).ToListAsync());
        }

        public IActionResult Browse(int id) {

            //use context object to query the database and get a list of products by cateforyId
            //use LINQ
            //vs SQL

            var products = _context.Products
                          .Where(p => p.CategoryId == id)
                          .OrderBy(p => p.Name)
                          .ToList();
            ViewBag.category = _context.Categories.Find(id).Name;
            //ViewBag.category = _context.Categories.Where(c=>c.Id=Id).FirstOrDefault().Name; 

            //在这里，直接return去Browse的index
            return View(products);
        }

        //注意这个id的名字要match那个input field‘是name
        public IActionResult AddToCart(int ProductId, int Quantity)
        {
            // query db to get product price, use LINQ
            var price = _context.Products.Find(ProductId).Price;

            // get or generate a customerid
            string customerId = GetCustomerId();

            // create and save cart object
            var cart = new Cart()
            {
                ProductId = ProductId,
                Quantity = Quantity,
                Price = price,
                DateCreated = DateTime.UtcNow, // returns date time in UTC timezone
                CustomerId = customerId
            };

            _context.Carts.Add(cart);
            _context.SaveChanges();

            // redirect to Cart view          
            return Redirect("Cart");
        }

        public IActionResult Cart() {

            string customerId = GetCustomerId();
            //include这里是包括了相关的那个product的信息
            var cart = _context.Carts
                .Include(c => c.Product)
                .Where(c => c.CustomerId == customerId).ToList();
            //cart现在是个list，所以Sum就是直接把这个list的值加起来
            var total = cart.Sum(c => c.Price);
            ViewBag.TotalAmount = total.ToString("C");

            return View(cart);
        }

        public IActionResult RemoveFromCart(int id) {

            var cartItem = _context.Carts
                            .Where(c => c.Id == id)
                            .FirstOrDefault();

            if (cartItem != null)
            {
                _context.Carts.Remove(cartItem);
                _context.SaveChanges();
            }

            return RedirectToAction("Cart");
        }


        [Authorize]
        public IActionResult Checkout() {


            return View();
        }

        //这里就是建立了一个新的Order的，把这些信息通过post传递进来，通过create（checkout）这个page
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken] // Only a valid view is able to post back to the server
        public IActionResult Checkout([Bind("FirstName,LastName,Address,City,Province,PostalCode")] Models.Order order)
        {
            // populate the 3 automatic order properties
            order.OrderDate = DateTime.UtcNow;
            order.CustomerId = User.Identity.Name;

            // calculate total amount
            var cartCustomerId = GetCustomerId();
            var cartItems = _context.Carts.Where(c => c.CustomerId == cartCustomerId).ToList();
            order.Total = cartItems.Sum(c => c.Price);

            // Store order object in session and
            // Implement a nuget package
            HttpContext.Session.SetObject("Order", order);

            // Redirect to payment
            return RedirectToAction("Payment");
        }

        [Authorize]
        public IActionResult Payment() {
            //TODO Get order object
            var order = HttpContext.Session.GetObject<Models.Order>("Order");

            //calculate total in cents
            ViewBag.Total = order.Total * 100;

            //read key from appsettings;
            ViewBag.Publishablekey = _configuration["Stripe:Publishablekey"];



            return View();
        }
        [Authorize]
        [HttpPost]
        

        public IActionResult Payment(string stripeToken)
        {
            // get order object from session variable
            var order = HttpContext.Session.GetObject<Models.Order>("Order");

            // Import Stripe and Stripe.Checkout in your program
            // Fix any reference to Order class, declare it explicitly: Models.Order
            // get the stripe configuration key
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];

            // Create Stripe session object >> https://stripe.com/docs/checkout/integration-builder
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long?)(order.Total * 100), // amount in cents
                            Currency = "cad",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "DotNetDrinks Purchase"
                            }
                        },
                        Quantity =1
                    }
                },
                Mode = "payment",
                //成功就去save order，失败就去cart
                SuccessUrl = $"https://{Request.Host}/Store/SaveOrder",
                CancelUrl = $"https://{Request.Host}/Store/Cart"
            };

            // redirect to checkout
            var service = new SessionService(); // A service is... a class that produces something

            // now use the service object to create a session object based on the options
            // what this does is calls Stripe's API to create a session on their end
            // we have the ID value which will be used to redirect the user to Stripe
            // With this ID stripe will load the amount information on their end
            Session session = service.Create(options);

            // return json response
            return Json(new { id = session.Id });
        }


        // TODO: Create GET /Store/SaveOrder
        public IActionResult SaveOrder()
        {
            // PAYMENT IS SUCCESSFUL AND ORDER SHOULD BE CREATED IN THE DB
            // get order object from the session store
            var order = HttpContext.Session.GetObject<Models.Order>("Order");
            // save to the db
            _context.Orders.Add(order);
            _context.SaveChanges();
            // Get customerId from session store
            var customerId = GetCustomerId();
            // get all cart items to save them in the order details table > 1 cart item = 1 order details record
            var cartItems = _context.Carts.Where(c => c.CustomerId == customerId);
            // loop through the list
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                };
                _context.OrderDetails.Add(orderDetail);
            }
            _context.SaveChanges();
            // clear up cart
            foreach (var item in cartItems)
            {
                _context.Carts.Remove(item);
            }
            _context.SaveChanges();
           //这里是去 orders/details/orderid
            return RedirectToAction("Details", "Orders", new { @id = order.Id });
        }







        //helper method 
        private string GetCustomerId()
        {
            // Check the session object for a CustomerId value
            // Session will be persisted as long as the user remains on the page
            // Once browser is closed Session might be lost
            if (String.IsNullOrEmpty(HttpContext.Session.GetString("CustomerId")))
            {
                string customerId = "";
                // check if the user is authenticated and use email address as id
                if (User.Identity.IsAuthenticated)
                {
                    customerId = User.Identity.Name; // email address
                }
                else
                {
                    // or for anonymous users, generated a GUID and use that as id
                    customerId = Guid.NewGuid().ToString();
                }
                // Set generated value in my session object
                HttpContext.Session.SetString("CustomerId", customerId);
            }

            // return whatever is in the session object at this point
            return HttpContext.Session.GetString("CustomerId");
        }
    }
}