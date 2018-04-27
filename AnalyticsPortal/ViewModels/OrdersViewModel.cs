using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnalyticsPortal.ViewModels
{
    public class OrdersViewModel
    {
        public string AverageCheck { get; set; }
        public string AmountMoney { get; set; }
        public int NumberOfOrders { get; set; }
        public int TotalOrderedItems { get; set; }
    }
}
