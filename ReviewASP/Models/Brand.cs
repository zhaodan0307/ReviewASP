using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewASP.Models
{
    public class Brand
    {
        // .NET MVC convention
        public int Id { get; set; }

        public string Name { get; set; }

        public int YearFounded { get; set; }
    }
}
