using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewASP.Models
{
    public class OrderDetail
    {
        //junction between Order and Product (many-to-many)
        public int Id { get; set; }

        //foreign key 两个
        public int ProductId { get; set; }

        public int OrderId { get; set; }

        //navigation virtual properties

        public Product Product { get; set; }
        public Order Order { get; set; }

        //剩下的……

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        
    }
}
