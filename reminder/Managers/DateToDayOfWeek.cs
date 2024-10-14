using System;
using System.Globalization;
using System.Linq;

namespace reminder
{
    public class DateToDayOfWeek
    {
        public string WhatsDayOfWeek(DateTime date)
        { 
            return Today_Tomorrow_Yesterday(date.DayOfWeek);
        }

        private string Today_Tomorrow_Yesterday(DayOfWeek day)
        {
            string dayOfWeek = Convert.ToString(day);
            if (dayOfWeek == Convert.ToString(DateTime.Today.DayOfWeek))
                dayOfWeek = "Today";
            else if (dayOfWeek == Convert.ToString(DateTime.Today.AddDays(+1).DayOfWeek))
                dayOfWeek = "Tomorrow";
            return dayOfWeek;
        }

        public string InputSecondDay(DateTime date1, DateTime date2)
        {
            string dayOfWeek;
            DayOfWeek day = date2.DayOfWeek;
            bool day2InWeek = false;
            var culture = CultureInfo.CurrentCulture;
            var weekOffset = culture.DateTimeFormat.FirstDayOfWeek - date1.DayOfWeek;
            var startOfWeek = date1.AddDays(weekOffset);
            var weekDays = Enumerable.Range(0, 7).Select(i => startOfWeek.AddDays(i));
            foreach (var i in weekDays)
            {
                if (date2 == i)
                    day2InWeek = true;
            }
            if (day2InWeek) 
            {
                dayOfWeek = Today_Tomorrow_Yesterday(day);
            }
            else
            {
                dayOfWeek = Today_Tomorrow_Yesterday(day) + $"({date2.ToString("dd/MM")})";
            }
            return dayOfWeek;
        }
    }
}
