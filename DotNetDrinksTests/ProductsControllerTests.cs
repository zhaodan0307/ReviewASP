using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReviewASP.Controllers;
using ReviewASP.Data;
using ReviewASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DotNetDrinksTests
{
    [TestClass]
    public class ProductsControllerTests
    {
        // connect to the DB?? create a mock object to simulate our db connection when testing
        // This is an In-Memory db context > Microsoft.EntityFrameworkCore.InMemory
        // connect to the DB?? create a mock object to simulate our db connection when testing
        // This is an In-Memory db context > Microsoft.EntityFrameworkCore.InMemory
        private ApplicationDbContext _context;
        // empty list of products
        List<Product> products = new List<Product>();
        // declare the controller that will be tested
        ProductsController controller;

        // How do I fill _context with data? or when?
        // Create a constructor?? Rather, create an Initialize method
        [TestInitialize]
        public void TestInitialize()
        {

            //内存数据库,其实就是建立一个假的数据库
            // instantiate in-memory db > similar to startup.cs
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            //输入一些假的数据
            //create mock data in this database 
            //建造一个假的category
            var category = new Category
            {
                Id = 100,
                Name = "Test Category"
            };

            _context.Categories.Add(category);

            _context.SaveChanges();

            //创造一个假的brand

            // Create 1 brand
            var brand = new Brand { Id = 100, Name = "No Name" };
            _context.Brands.Add(brand);
            _context.SaveChanges();


            // Create 3 products
            products.Add(new Product { Id = 101, Name = "Product", Price = 11, Category = category, Brand = brand });
            products.Add(new Product { Id = 102, Name = "Another Product", Price = 12, Category = category, Brand = brand });
            products.Add(new Product { Id = 103, Name = "Extra Product", Price = 13, Category = category, Brand = brand });

            //把三个products加入数据库
            foreach (var p in products)
            {
                _context.Products.Add(p);
            }
            _context.SaveChanges();


            //set controller
            // instanciate the controller class with mock db context
            controller = new ProductsController(_context);

        }

        // Tests that I need to write for archieving 100% coverage
        // Create(GET)
        // Create(POST)
        // Delete(GET)
        // DeleteConfirmed(POST)
        // Details
        // Edit(GET)
        // Edit(POST)

        // Index(GET)


        [TestMethod]
        public void IndexViewLoads() {
            //Act
            var result = (ViewResult)controller.Index().Result;
            //assert
            Assert.AreEqual("Index", result.ViewName);


        }

        [TestMethod]
        public void IndexReturnsProductData()
        {
            // Act
            // Call index action method and cast result
            var result = controller.Index();
            var viewResult = (ViewResult)result.Result;
            // Extract list of product generated in the controller
            var model = (List<Product>)viewResult.Model;
            // Match ordering specified in product controller
            var orderedProducts = products.OrderBy(p => p.Name).ToList();
            // Assert both lists are equal
            CollectionAssert.AreEqual(orderedProducts, model);
        }

        [TestMethod]
        public void GetDeleteReturn()
            
        {
            var id = 101;

                
            var result = controller.Delete(id);
            var viewResult = (ViewResult)result.Result;

            var model = (Product)viewResult.Model;

            var product = products.Find(e => e.Id == id);


            Assert.AreEqual(model, product);
           

        }



        [TestMethod]
        public void GetDeleteReturnNoProduct()
        {

            int id = 0;


            var result =(NotFoundResult)controller.Delete(id).Result;

          

            Assert.AreEqual(404,result.StatusCode);
        }

        [TestMethod]
        public void IFGetDeleteReturn()
        {

            var result = (NotFoundResult)controller.Delete(null).Result;


            Assert.AreEqual(404, result.StatusCode);
        }


    } 
}
