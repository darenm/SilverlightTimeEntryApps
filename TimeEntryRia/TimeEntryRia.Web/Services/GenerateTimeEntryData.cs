using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;

namespace TimeEntryRia.Web
{
    public class GenerateTimeEntryData : IDisposable
    {
        static Random _random = new Random();
        private TimeEntryEntities _context;

        struct WeekOfYear
        {
            public int Week { get; set; }
            public int Year { get; set; }
        }

        public static void GenerateDataIfRequired()
        {
            var weekOfYear = new WeekOfYear()
            {
                Week = TimeEntry.GetIso8601WeekOfYear(DateTime.Now),
                Year = DateTime.Now.Year
            };

            if (weekOfYear.Week == 1)
            {
                weekOfYear.Week = 52;
                weekOfYear.Year--;
            }

            using (var instance = new GenerateTimeEntryData())
            {
                if (instance.IsThereDataForWeek(weekOfYear))
                {
                    return;
                }

                instance.GenerateData(weekOfYear);
            }
        }

        private bool IsThereDataForWeek(WeekOfYear weekOfYear)
        {
            var mondayDate = TimeEntry.GetIso8601FirstDateOfWeek(weekOfYear.Year, weekOfYear.Week);

            // comparing day, month and year instead of .Date as mondayDate.Date is not supported
            if (_context.TimeEntries.Any(te => te.Date.Day == mondayDate.Day &&
                                               te.Date.Month == mondayDate.Month &&
                                               te.Date.Year == mondayDate.Year))
            {
                return true;
            }

            return false;
        }

        private GenerateTimeEntryData()
        {
            _context = new TimeEntryEntities();
        }

        private void GenerateData(WeekOfYear weekOfYear)
        {
            var projects = _context.Projects.ToList();
            var users = _context.TimeEntryUsers.ToList();

            // otherwise start to generate data
            for (int year = weekOfYear.Year; year > DateTime.Now.Year - 2; year--)
            {
                for (int week = 52; week > 1; week--)
                {
                    var weekDate = TimeEntry.GetIso8601FirstDateOfWeek(year, week).Date;
                    if ( weekDate >= DateTime.Now)
                    {
                        // don't enter future data
                        continue;
                    }

                    for (int day = 0; day < 5; day++)
                    {
                        foreach (var user in users)
                        {
                            var totalTime = 0.0;

                            while (totalTime < 8.0)
                            {
                                var time = Math.Round(_random.NextDouble() * 8.0, 1);
                                totalTime += time;
                                var currentDate = weekDate + TimeSpan.FromDays(day);
                                var project = projects[_random.Next(3)];
                                var timeEntry = new TimeEntry()
                                {
                                    Date = new DateTime(currentDate.Year, currentDate.Month, currentDate.Day),
                                    Hours = time,
                                    Project = project,
                                    UserId = user.Id
                                };
                                _context.TimeEntries.AddObject(timeEntry);
                            }
                        }

                        _context.SaveChanges();
                    }
                }
            }
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }

        // This presumes that weeks start with Monday.
        // Week 1 is the 1st week of the year with a Thursday in it.
        private static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
    }
}