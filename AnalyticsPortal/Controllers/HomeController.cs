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
            return View();
        }

        public IActionResult Orders()
        {
            double averageCheck = db.Orders.Average(o => o.TotalPrice);
            double amountMoney = db.Orders.Sum(o => o.TotalPrice);
            int numberOfOrders = db.Orders.Count();
            int totalOrderedItems = db.OrderItemOrders.Count();

            OrdersViewModel ovm = new OrdersViewModel()
            {
                AverageCheck = averageCheck.ToString("$0.##"),
                AmountMoney = amountMoney.ToString("$0.##"),
                NumberOfOrders = numberOfOrders,
                TotalOrderedItems = totalOrderedItems
            };

            return PartialView(ovm);
        }

        [HttpPost]
        public IActionResult Chart(NumType type, Period period)
        {
            ChartViewModel cvm;
            switch (period)
            {
                case Period.Day:
                    cvm = GetChartVMByDay(type);
                    break;
                case Period.Week:
                    cvm = GetChartVMByWeek(type);
                    break;
                case Period.Month:
                    cvm = GetChartVMByMonth(type);
                    break;
                default:
                    cvm = new ChartViewModel();
                    break;
            }
            return PartialView(cvm);
        }

        #region GetChartVM
        private ChartViewModel GetChartVMByDay(NumType type)
        {
            List<double> counts = new List<double>();
            DateTime now = DateTime.Now.Date;

            for (int i = 6; i >= 0; i--)
            {
                DateTime date = now.AddDays(-i);
                IQueryable<Order> query = db.Orders.Where(o => o.ClosedDate.Date.Equals(date));
                double count = GetCountFromQuery(query, type);
                counts.Add(count);
            }

            return new ChartViewModel(counts, type, Period.Day);
        }

        private ChartViewModel GetChartVMByWeek(NumType type)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            List<double> counts = new List<double>();
            DateTime now = DateTime.Now.Date;

            for (int i = 6; i >= 0; i--)
            {
                DateTime date = now.AddDays(-7 * i);
                int week = cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek) - 1;
                IQueryable<Order> query = db.Orders
                    .Where(o =>
                        o.ClosedDate.Year.Equals(date.Year) &&
                        ((o.ClosedDate.DayOfYear - 1) / 7).Equals(week));
                double count = GetCountFromQuery(query, type);
                counts.Add(count);
            }

            return new ChartViewModel(counts, type, Period.Week);
        }

        private ChartViewModel GetChartVMByMonth(NumType type)
        {
            List<double> counts = new List<double>();
            DateTime now = DateTime.Now.Date;

            for (int i = 6; i >= 0; i--)
            {
                DateTime date = now.AddMonths(-i);
                IQueryable<Order> query = db.Orders.Where(o =>
                    o.ClosedDate.Year.Equals(date.Year) &&
                    o.ClosedDate.Month.Equals(date.Month));
                double count = GetCountFromQuery(query, type);
                counts.Add(count);
            }

            return new ChartViewModel(counts, type, Period.Month);
        }

        private double GetCountFromQuery(IQueryable<Order> query,NumType type)
        {
            switch (type)
            {
                case NumType.Money:
                    return query.Sum(o => o.TotalPrice);
                case NumType.Orders:
                    return query.Count();
                default:
                    return 0;
            }
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
                    Price = oi.Price.ToString("$0.##"),
                    Media = oi.Media,
                    NumberOfOrders = oio.Count(),
                    Amount = oio.Sum(p => p.Price).ToString("$0.##")
                })
                .OrderByDescending(oio => oio.NumberOfOrders)
                .Take(10)
                .ToList();

            List<OrderItemModel> unordered = db.OrderItems.GroupJoin(
            db.OrderItemOrders,
                oi => oi.Id,
                oio => oio.OrderItemId,
                (oi, oio) => new OrderItemModel
                {
                    Name = oi.Name,
                    Price = oi.Price.ToString("$0.##"),
                    Media = oi.Media,
                    NumberOfOrders = oio.Count(),
                    Amount = oio.Sum(p => p.Price).ToString("$0.##")
                }).
                OrderBy(oio => oio.NumberOfOrders)
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

            double tipsAverage = db.Orders.Sum(o => o.TipsAmount) / db.Orders.Sum(o => o.TotalPrice);
            int tipsNumbers = db.Orders.Count(o => o.TipsAmount != 0.0);
            double tipsSum = db.Orders.Sum(o => o.TipsAmount);

            PaymentsViewModel pvm = new PaymentsViewModel()
            {
                OperationsNumberByCard = operationsNumberByCard,
                OperationsNumberByCash = operationsNumberByCash,
                AmountOfMoneyByCard = amountOfMoneyByCard.ToString("$0.##"),
                AmountOfMoneyByCash = amountOfMoneyByCash.ToString("$0.##"),
                TipsAverage = tipsAverage.ToString("0.#%"),
                TipsNumbers = tipsNumbers,
                TipsSum = tipsSum.ToString("$0.##")
            };
            return PartialView(pvm);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
