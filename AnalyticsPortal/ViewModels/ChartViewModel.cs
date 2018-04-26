using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AnalyticsPortal.ViewModels
{
    public class ChartViewModel
    {
        public List<string> Counts { get; set; }
        public List<string> FillPercentages { get; set; }
        public List<string> PeriodNames { get; set; }


        public ChartViewModel()
        {
            Counts = new List<string>();
            FillPercentages = new List<string>();
            PeriodNames = new List<string>();
        }

        public ChartViewModel(List<double> counts,NumType type, Period period)
        {
            SetCounts(counts,type);
            SetFillPercentages(counts);
            SetPeriodName(period);
        }

        private void SetCounts(List<double> counts, NumType type)
        {
            switch (type) {
                case NumType.Orders:
                    Counts = counts.ConvertAll(c=>c.ToString("."));
                    break;
                case NumType.Money:
                    Counts = counts.ConvertAll(c => c.ToString(".00"));
                    break;
            }
        }

        private void SetFillPercentages(List<double> counts)
        {
            List<double> fillPercentages = new List<double>();
            double maxCount = counts.Max();
            foreach (double count in counts)
                fillPercentages.Add((maxCount / count) * 100);
            FillPercentages = fillPercentages.ConvertAll(p=>p.ToString(".") + "%");
        }

        private void SetPeriodName(Period period)
        {
            DateTime now = DateTime.Now.Date;
            switch (period)
            {
                case Period.Day:
                    for (int i = 6; i >= 0; i--)
                        PeriodNames.Add((now.AddDays(-i).Day.ToString()));
                    break;
                case Period.Week:
                    DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                    Calendar cal = dfi.Calendar;
                    for (int i = 6; i >= 0; i--)
                    {
                        DateTime date = now.AddDays(-7 * i);
                        int weekNum = cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
                        PeriodNames.Add(weekNum.ToString());
                    }
                    break;
                case Period.Month:
                    for (int i = 6; i >= 0; i--)
                        PeriodNames.Add((now.AddMonths(-i).ToString("MMM")));
                    break;
            }
        }
    }
    public enum NumType
    {
        Orders,
        Money
    }
    public enum Period{
        Day,
        Week,
        Month
    }
}
