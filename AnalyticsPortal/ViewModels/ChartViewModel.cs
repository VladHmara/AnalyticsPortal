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
                    Counts = counts.ConvertAll(c=>c.ToString("0."));
                    break;
                case NumType.Money:
                    Counts = counts.ConvertAll(c => c.ToString("$0.##"));
                    break;
            }
        }

        private void SetFillPercentages(List<double> counts)
        {
            List<double> fillPercentages = new List<double>();
            double maxCount = counts.Max();
            foreach (double count in counts)
                fillPercentages.Add((count / maxCount) * 100);
            FillPercentages = fillPercentages.ConvertAll(p=>p.ToString("0.#") + "%");
        }

        private void SetPeriodName(Period period)
        {
            List<string> periodNames = new List<string>();
            DateTime now = DateTime.Now.Date;
            switch (period)
            {
                case Period.Day:
                    for (int i = 6; i >= 0; i--)
                        periodNames.Add((now.AddDays(-i).Day.ToString()));
                    break;
                case Period.Week:
                    DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                    Calendar cal = dfi.Calendar;
                    for (int i = 6; i >= 0; i--)
                    {
                        DateTime date = now.AddDays(-7 * i);
                        int weekNum = cal.GetWeekOfYear(date, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
                        periodNames.Add(weekNum.ToString());
                    }
                    break;
                case Period.Month:
                    for (int i = 6; i >= 0; i--)
                        periodNames.Add((now.AddMonths(-i).ToString("MMM")));
                    break;
            }
            PeriodNames = periodNames;
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
