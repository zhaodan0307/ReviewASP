using Microsoft.AspNetCore.Mvc;
using ReviewASP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewASP.Controllers
{
    public class BrandsController : Controller
    {
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
            ViewBag.name = name;
            return View();
        }
    }
}
