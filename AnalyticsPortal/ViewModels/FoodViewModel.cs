using AnalyticsPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnalyticsPortal.ViewModels
{
    public class FoodViewModel
    {
        public List<OrderItemModel> TopOrders { get; set; }
        public List<OrderItemModel> Unordered { get; set; }

        public FoodViewModel()
        {
            TopOrders = new List<OrderItemModel>();
            Unordered = new List<OrderItemModel>();
        }
    }
}
