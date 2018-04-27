using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnalyticsPortal.ViewModels
{
    public class OrderItemModel
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string Media { get; set; }
        public int NumberOfOrders { get; set; }
        public string Amount { get; set; }
    }
}
