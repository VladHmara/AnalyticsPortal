using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnalyticsPortal.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public double TipsAmount { get; set; }
        public double TotalPrice { get; set; }
        public State State { get; set; }
        public OrderFor OrderFor { get; set; }
        public PaidBy PaidBy { get; set; }
        public DateTime OpendDate { get; set; }
        public DateTime ClosedDate { get; set; }
        public virtual ICollection<OrderItemOrder> OrderItemOrders { get; set; }
        public Order()
        {
            OrderItemOrders = new List<OrderItemOrder>();
        }

    }
    public enum State
    {
        Active,
        Voided,
        Paid
    }
    public enum OrderFor
    {
        Table,
        Seat
    }
    public enum PaidBy
    {
        Card,
        Cash
    }
}
