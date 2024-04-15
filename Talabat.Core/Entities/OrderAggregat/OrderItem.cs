using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.OrderAggregat
{
    public class OrderItem : BaseEntity
    {
        public OrderItem()
        {
            
        }
        public OrderItem(ProductItemOrder product, decimal price, int qunatity)
        {
            Product = product;
            Price = price;
            Qunatity = qunatity;
        }

        public ProductItemOrder Product { get; set; }
        public decimal Price { get; set; }
        public int Qunatity { get; set;}
    }
}
