using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AnalyticsPortal.Models;
using AnalyticsPortal.ViewModels;
using System.Globalization;

namespace AnalyticsPortal.Controllers
{
    public class HomeController : Controller
    {
        private OrdersContext db;

        public HomeController(OrdersContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            double averageCheck = 0; //db.Orders.Average(o => o.TotalPrice);
            return View(averageCheck);
        }

        public IActionResult Orders()
        {
            double averageCheck = db.Orders.Average(o => o.TotalPrice);
            double amountMoney = db.Orders.Sum(o => o.TotalPrice);
            int numberOfOrders = db.Orders.Count();
            int totalOrderedItems = db.OrderItemOrders.Count();

            OrdersViewModel ovm = new OrdersViewModel()
            {
                AverageCheck = averageCheck.ToString(".00"),
                AmountMoney = amountMoney.ToString(".00"),
                NumberOfOrders = numberOfOrders.ToString(),
                TotalOrderedItems = totalOrderedItems.ToString()
            };

            return PartialView(ovm);
        }

        public IActionResult Chart(string type, string period)
        {
            ChartViewModel cvm;
            switch (type + period)
            {
                case "OrdersDay":
                    cvm = GetChartVMByOrdersDay();
                    break;
                case "OrdersWeek":
                    cvm = GetChartVMByOrdersWeek();
                    break;
                case "OrdersMonth":
                    cvm = GetChartVMByOrdersMonth();
                    break;
                case "MoneyDay":
                    cvm = GetChartVMByMoneyDay();
                    break;
                case "MoneyWeek":
                    cvm = GetChartVMByMoneyWeek();
                    break;
                case "MoneyMonth":
                    cvm = GetChartVMByMoneyMonth();
                    break;
                default:
                    cvm = new ChartViewModel();
                    break;
            }
            return PartialView(cvm);
        }

        #region GetChartVM
        private ChartViewModel GetChartVMByOrdersDay()
        {
            List<double> counts = new List<double>();
            DateTime now = DateTime.Now.Date;

            for (int i = 6; i >= 0; i--)
            {
                DateTime date = now.AddDays(-i);
                double count = db.Orders
                    .Count(o => o.ClosedDate.Date.Equals(date));
                counts.Add(count);
            }

            return new ChartViewModel(counts, NumType.Orders, Period.Day);
        }

        private ChartViewModel GetChartVMByOrdersWeek()
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            List<double> counts = new List<double>();
            DateTime now = DateTime.Now.Date;

            for (int i = 6; i >= 0; i--)
            {
                DateTime date = now.AddDays(-7 * i);
                int week = cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) - 1;
                double count = db.Orders
                    .Count(o =>
                        o.ClosedDate.Year.Equals(date.Year) &&
                        ((o.ClosedDate.DayOfYear - 1) / 7).Equals(week));
                counts.Add(count);
            }

            return new ChartViewModel(counts, NumType.Orders, Period.Week);
        }

        private ChartViewModel GetChartVMByOrdersMonth()
        {
            List<double> counts = new List<double>();
            DateTime now = DateTime.Now.Date;

            for (int i = 6; i >= 0; i--)
            {
                DateTime date = now.AddMonths(-i);
                double count = db.Orders
                    .Count(o =>
                        o.ClosedDate.Year.Equals(date.Year) &&
                        o.ClosedDate.Month.Equals(date.Month));
                counts.Add(count);
            }

            return new ChartViewModel(counts, NumType.Orders, Period.Month);
        }

        private ChartViewModel GetChartVMByMoneyDay()
        {
            List<double> counts = new List<double>();
            DateTime now = DateTime.Now.Date;

            for (int i = 6; i >= 0; i--)
            {
                DateTime date = now.AddDays(-i);
                double count = db.Orders
                    .Where(o => o.ClosedDate.Date.Equals(date))
                    .Sum(o => o.TotalPrice);
                counts.Add(count);
            }

            return new ChartViewModel(counts, NumType.Money, Period.Day);
        }

        private ChartViewModel GetChartVMByMoneyWeek()
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            List<double> counts = new List<double>();
            DateTime now = DateTime.Now.Date;

            for (int i = 6; i >= 0; i--)
            {
                DateTime date = now.AddDays(-7 * i);
                int week = cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) - 1;
                double count = db.Orders
                    .Where(o =>
                        o.ClosedDate.Year.Equals(date.Year) &&
                        ((o.ClosedDate.DayOfYear - 1) / 7).Equals(week))
                    .Sum(o => o.TotalPrice);
                counts.Add(count);
            }

            return new ChartViewModel(counts, NumType.Money, Period.Week);
        }

        private ChartViewModel GetChartVMByMoneyMonth()
        {
            List<double> counts = new List<double>();
            DateTime now = DateTime.Now.Date;

            for (int i = 6; i >= 0; i--)
            {
                DateTime date = now.AddMonths(-i);
                double count = db.Orders.Where(o =>
                    o.ClosedDate.Year.Equals(date.Year) &&
                    o.ClosedDate.Month.Equals(date.Month))
                    .Sum(o => o.TotalPrice);
                counts.Add(count);
            }

            return new ChartViewModel(counts, NumType.Money, Period.Month);
        }
        #endregion

        public IActionResult Food()
        {
            List<OrderItemModel> topOrders = db.OrderItems.GroupJoin(
                db.OrderItemOrders,
                oi => oi.Id,
                oio => oio.OrderItemId,
                (oi, oio) => new OrderItemModel
                {
                    Name = oi.Name,
                    Price = oi.Price.ToString(".00"),
                    Media = oi.Media,
                    NumberOfOrders = oio.Count().ToString(),
                    Amount = oio.Sum(p => p.Price).ToString(".00")
                })
                .OrderBy(oio => oio.NumberOfOrders)
                .Take(10)
                .ToList();

            List<OrderItemModel> unordered = db.OrderItems.GroupJoin(
            db.OrderItemOrders,
                oi => oi.Id,
                oio => oio.OrderItemId,
                (oi, oio) => new OrderItemModel
                {
                    Name = oi.Name,
                    Price = oi.Price.ToString(".00"),
                    Media = oi.Media,
                    NumberOfOrders = oio.Count().ToString(),
                    Amount = oio.Sum(p => p.Price).ToString(".00")
                }).
                OrderByDescending(oio => oio.NumberOfOrders)
                .Take(10)
                .ToList();

            FoodViewModel fvm = new FoodViewModel()
            {
                TopOrders = topOrders,
                Unordered = unordered
            };

            return PartialView(fvm);
        }

        public IActionResult Payments()
        {
            int operationsNumberByCard = db.Orders.Count(o => o.PaidBy == PaidBy.Card);
            int operationsNumberByCash = db.Orders.Count(o => o.PaidBy == PaidBy.Cash);

            double amountOfMoneyByCard = db.Orders.Where(o => o.PaidBy == PaidBy.Card).Sum(o => o.TotalPrice);
            double amountOfMoneyByCash = db.Orders.Where(o => o.PaidBy == PaidBy.Cash).Sum(o => o.TotalPrice);

            double tipsAverage = db.Orders.Sum(o => o.TotalPrice / o.TipsAmount);
            int tipsNumbers = db.Orders.Count(o => o.TipsAmount != 0.0);
            double tipsSum = db.Orders.Sum(o => o.TipsAmount);

            PaymentsViewModel pvm = new PaymentsViewModel()
            {
                OperationsNumberByCard = operationsNumberByCard.ToString(),
                OperationsNumberByCash = operationsNumberByCash.ToString(),
                AmountOfMoneyByCard = amountOfMoneyByCard.ToString(".00"),
                AmountOfMoneyByCash = amountOfMoneyByCash.ToString(".00"),
                TipsAverage = tipsAverage.ToString(".0"),
                TipsNumbers = tipsNumbers.ToString(),
                TipsSum = tipsSum.ToString(".00")
            };
            return PartialView(pvm);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
