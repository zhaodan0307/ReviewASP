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

        //required name, auto validation
        [Required(ErrorMessage = "name is required ")]
        [MaxLength(100)]
        public string Name { get; set; }


        //“https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-5.0” validation 的文档，可以看看有多少种validation的方法
        [Range(1400, 2025)]
        [Display(Name = "Year Founded")]
        public int YearFounded { get; set; }


        

        //navigaton property to child Product objects
        //一个brand对应好多个product
        public List<Product> Products { get; set; }
    }
}
 