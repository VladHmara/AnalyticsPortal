using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnalyticsPortal.Models
{
    public class OrderItemOrder
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; }

        public Guid OrderItemId { get; set; }
        public virtual OrderItem OrderItem { get; set; }

        public double Price { get; set; }
    }
}
