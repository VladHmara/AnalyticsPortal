using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnalyticsPortal.Models
{
    public class OrderItem
    {
        public Guid Id { get;set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Media { get; set; }
        public virtual ICollection<OrderItemOrder> OrderItemOrders { get; set; }
        public OrderItem()
        {
            OrderItemOrders = new List<OrderItemOrder>();
        }
    }
}
