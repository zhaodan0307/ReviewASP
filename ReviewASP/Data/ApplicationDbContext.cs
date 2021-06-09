﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReviewASP.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReviewASP.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        //declare all the  models so the ab can work with it and so can the rest of our app
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderDetail> OrderDetails{ get; set; }

        public DbSet<Cart> Carts { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
