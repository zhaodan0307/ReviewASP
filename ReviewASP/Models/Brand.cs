using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewASP.Models
{
    public class Brand
    {
        // .NET MVC convention
        public int Id { get; set; }

        [Required(ErrorMessage = "name is required ")]
        public string Name { get; set; }


        [Range(1400,2025)]
        public int YearFounded { get; set; }
    }
}
