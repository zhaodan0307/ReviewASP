using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewASP.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        //这里让钱前面有一个钱财的标志，会根据server所在地，在中国就显示yuan,这个是microsoft的data format for currency
        [DisplayFormat(DataFormatString="{0:c}")]
        public decimal Price { get; set; }

        [Range(0,9999999)]
        public int Stock { get; set; }

        public string Image { get; set; }
        // fk fields - must follow naming convention(Model)Id

        //两个foreign key
        [Display(Name ="Brand")]
        public int BrandId { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        //navigation properties so we don't have to use joins to connect related records


        public Brand Brand { get; set; }

        public Category Category { get; set; }
        //以上两个部分缺一不可，这样才能被作为是foreign key


        //他是人家的foreign key
        //child ref
        public List<OrderDetail> OrderDetails { get; set; }
        public List<Cart> Carts { get; set; }
        

    }
}
