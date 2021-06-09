﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewASP.Models
{
    public class Category

    {
        public int Id { get; set; }

        public string Name { get; set; }


        //navigaton property to child Product objects

        public List<Product> Products { get; set; }


    }
}
