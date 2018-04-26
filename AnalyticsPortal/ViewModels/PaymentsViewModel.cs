using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnalyticsPortal.ViewModels
{
    public class PaymentsViewModel
    {
        public string OperationsNumberByCard { get; set; }
        public string OperationsNumberByCash { get; set; }
        public string AmountOfMoneyByCard { get; set; }
        public string AmountOfMoneyByCash { get; set; }
        public string TipsAverage { get; set; }
        public string TipsNumbers { get; set; }
        public string TipsSum { get; set; }
    }
}
