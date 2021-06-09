using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReviewASP.Models
{
    public class Order
    {
        public int Id { get; set; }

        public decimal Total { get; set; }

        public String CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Address {get;set;}
        public string City { get; set; }

        public string Province { get; set; }

        public string PostalCode { get; set; }
        //child ref
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
