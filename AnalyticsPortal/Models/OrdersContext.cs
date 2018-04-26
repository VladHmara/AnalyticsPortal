using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnalyticsPortal.Models
{
    public class OrdersContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItemOrder> OrderItemOrders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public OrdersContext(DbContextOptions<OrdersContext> options)
    : base(options)
        {
        }
    }
}
