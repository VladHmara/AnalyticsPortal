using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnalyticsPortal.ViewModels
{
    public class PaymentsViewModel
    {
        public int OperationsNumberByCard { get; set; }
        public int OperationsNumberByCash { get; set; }
        public string AmountOfMoneyByCard { get; set; }
        public string AmountOfMoneyByCash { get; set; }
        public string TipsAverage { get; set; }
        public int TipsNumbers { get; set; }
        public string TipsSum { get; set; }
    }
}
