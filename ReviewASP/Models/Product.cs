﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewASP.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }


        public decimal Price { get; set; }


        public int Stock { get; set; }

        public string Image { get; set; }
        // fk fields - must follow naming convention(Model)Id

        public int BrandId { get; set; }

        public int CategoryId { get; set; }

        //navigatioin properties so we don't have to use joins to connect related records

        public Brand Brand { get; set; }

        public Category Category { get; set; }

        //child ref
        public List<OrderDetail> OrderDetails { get; set; }
        public List<Cart> Carts { get; set; }
        

    }
}
