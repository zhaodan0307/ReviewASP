using Microsoft.AspNetCore.Mvc;
using ReviewASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewASP.Controllers
{
    public class BrandsControllerOld : Controller
    {
        //这个IActionResult是属于interface
        public IActionResult Index()
        {
            //use the brand model to create a mock list of brand objects to display on the Index view

            var brands = new List<Brand>();

            brands.Add(new Brand { Id = 100, Name = "Canadian Club", YearFounded = 1902 });
            brands.Add(new Brand { Id = 101, Name = "Moison", YearFounded = 1786 });
            brands.Add(new Brand { Id = 102, Name = "Glenfiddich", YearFounded = 1883 });
            brands.Add(new Brand { Id = 103, Name = "JP Wiser", YearFounded = 1853 });
            brands.Add(new Brand { Id = 104, Name = "Jackson Triggs", YearFounded = 1993 });

            //pass the brands list to the view for display
            return View(brands);
        }

        //get: Brands/Details
        public IActionResult Details(string name) {
            //read the name param from the url, and put it in the ViewBag for display on the view
            if (name == null)
            {
                //没有name的话，就返回400,也是一种return的类型，可以return code of http
                return BadRequest();

             }
            ViewBag.name = name;
            return View();
        }

        //get:Brands/create

        public IActionResult Create() {

            //我们可以return 4 种类型，1.默认类型是view，2.也可以return json（如果我们建立一个api），
            //3.也可以return 一个http status code比方404.4种就是redirect，我们可以redirectToAction
            return View();
        }
    }
}
