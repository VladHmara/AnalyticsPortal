using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AnalyticsPortal.Models
{
    public static class SampleData
    {
        private static Random random = new Random();
        public static void Initialize(OrdersContext context)
        {
            context.Orders.RemoveRange(context.Orders);
            context.OrderItems.RemoveRange(context.OrderItems);
            context.OrderItemOrders.RemoveRange(context.OrderItemOrders);
            context.SaveChanges();

            InitializeOrderItems(context);
            InitializeOrders(context);
        }
        private static void InitializeOrderItems(OrdersContext context)
        {
            foreach (string path in Directory.GetFiles("wwwroot\\images\\OrderItems"))
            {
                context.OrderItems.Add(
                    new OrderItem
                    {
                        Media = path.Replace("wwwroot\\", ""),
                        Name = Path.GetFileNameWithoutExtension(path),
                        Price = random.NextDouble() * 100
                    }
                );
            }
            context.SaveChanges();
        }
        private static void InitializeOrders(OrdersContext context)
        {
            DateTime now = DateTime.Now;
            List<OrderItem> orderItems = context.OrderItems.ToList();
            for (int i = 0; i < 210; i++)
            {
                for (int j = 0; j < random.Next(100); j++)
                {
                    Order order = new Order
                    {
                        ClosedDate = now.AddDays(-i),
                        PaidBy = (PaidBy)random.Next(2),
                        TipsAmount = random.NextDouble() * 10
                    };
                    for (int k = 0; k < random.Next(10); k++)
                    {
                        OrderItemOrder orderItemOrder = new OrderItemOrder
                        {
                            OrderItem = orderItems[random.Next(orderItems.Count)],
                            Order = order
                        };
                        orderItemOrder.Price = orderItemOrder.OrderItem.Price;
                        order.OrderItemOrders.Add(orderItemOrder);
                    }
                    order.TotalPrice = order.OrderItemOrders.Sum(oio => oio.Price);
                    context.Orders.Add(order);
                }
            }
            context.SaveChanges();
        }
    }
}
