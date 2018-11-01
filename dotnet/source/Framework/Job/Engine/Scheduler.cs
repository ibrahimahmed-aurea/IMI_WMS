using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using Imi.Framework.Job.Configuration;
using Imi.Framework.Shared.Diagnostics;

namespace Imi.Framework.Job.Engine
{
    internal class ManagedSchedule
    {
        private Schedule schedule;
        public ArrayList listeners = new ArrayList();

        public Schedule Schedule
        {
            get
            {
                return schedule;
            }
            set
            {
                schedule = value;
            }
        }

    }

    internal class ScheduleRule
    {
        private int[] months;
        private int[] daysDate;
        private int[] daysWeek;
        private System.DayOfWeek[] daysWeekDay;
        private int[] hours;
        private int[] minutes;
        private int[] seconds;

        private int[] CreateValueArray(string valueList)
        {
            string[] valueArr = valueList.Split(new char[] { ',' });
            ArrayList values = new ArrayList();

            foreach (string s in valueArr)
            {
                int pos = s.IndexOf('-');
                if (pos >= 0)
                {
                    // Assume from to interval
                    string[] fromto = s.Split(new char[] { '-' });
                    int from = Convert.ToInt32(fromto[0]);
                    int to = Convert.ToInt32(fromto[1]);

                    for (int idx = from; idx <= to; idx++)
                    {
                        values.Add(idx);
                    }
                }
                else
                    values.Add(Convert.ToInt32(s));
            }

            return (values.ToArray(typeof(int)) as int[]);
        }

        /// <summary>
        /// These are the rules:
        /// 
        /// </summary>
        /// <param name="Second">No value specified means every second</param>
        /// <param name="Minute">No value specified mean every minute</param>
        /// <param name="Hour">No value specified means every hour</param>
        /// <param name="DayOfWeek">DayOfWeek and DayOfMonth are not cumulative, i.e. if you specify 23 and Thursday only Thursdays with the date 23 will be selected</param>
        /// <param name="DayOfMonth">not specifying any value equates to every day.</param>
        /// <param name="Month">No month specified means every/any month</param>

        public ScheduleRule(string Second,
          string Minute,
          string Hour,
          string DayOfWeek,
          string DayOfMonth,
          string Month
          )
        {
            if (Month != "")
            {
                string MonthInt;
                MonthInt = Month.Replace("Jan", "1");
                MonthInt = MonthInt.Replace("Feb", "2");
                MonthInt = MonthInt.Replace("Mar", "3");
                MonthInt = MonthInt.Replace("Apr", "4");
                MonthInt = MonthInt.Replace("May", "5");
                MonthInt = MonthInt.Replace("Jun", "6");
                MonthInt = MonthInt.Replace("Jul", "7");
                MonthInt = MonthInt.Replace("Aug", "8");
                MonthInt = MonthInt.Replace("Sep", "9");
                MonthInt = MonthInt.Replace("Oct", "10");
                MonthInt = MonthInt.Replace("Nov", "11");
                MonthInt = MonthInt.Replace("Dec", "12");

                months = CreateValueArray(MonthInt);
            }
            else
            { // No value is every month
                months = CreateValueArray("1-12");
            }

            if (DayOfMonth != "")
            {
                daysDate = CreateValueArray(DayOfMonth);
            }

            // Must do reverse order, i.e. Day before Hour before Minute
            if (DayOfWeek != "")
            {
                string DayOfWeekInt;

                DayOfWeekInt = DayOfWeek.Replace("Sun", "0");
                DayOfWeekInt = DayOfWeekInt.Replace("Mon", "1");
                DayOfWeekInt = DayOfWeekInt.Replace("Tue", "2");
                DayOfWeekInt = DayOfWeekInt.Replace("Wed", "3");
                DayOfWeekInt = DayOfWeekInt.Replace("Thu", "4");
                DayOfWeekInt = DayOfWeekInt.Replace("Fri", "5");
                DayOfWeekInt = DayOfWeekInt.Replace("Sat", "6");

                daysWeek = CreateValueArray(DayOfWeekInt);

                daysWeekDay = new System.DayOfWeek[daysWeek.Length];
                int i = 0;

                foreach (int d in daysWeek)
                {
                    switch (d)
                    {
                        case 0:
                            daysWeekDay[i++] = System.DayOfWeek.Sunday;
                            break;
                        case 1:
                            daysWeekDay[i++] = System.DayOfWeek.Monday;
                            break;
                        case 2:
                            daysWeekDay[i++] = System.DayOfWeek.Tuesday;
                            break;
                        case 3:
                            daysWeekDay[i++] = System.DayOfWeek.Wednesday;
                            break;
                        case 4:
                            daysWeekDay[i++] = System.DayOfWeek.Thursday;
                            break;
                        case 5:
                            daysWeekDay[i++] = System.DayOfWeek.Friday;
                            break;
                        case 6:
                            daysWeekDay[i++] = System.DayOfWeek.Saturday;
                            break;
                    }
                }
            }

            if (Hour != "")
            {
                hours = CreateValueArray(Hour);
            }
            else
            {// no value means every hour
                hours = CreateValueArray("0-23");
            }

            if (Minute != "")
            {
                minutes = CreateValueArray(Minute);
            }
            else
            { // no value means every minute
                minutes = CreateValueArray("0-59");
            }

            if (Second != "")
            {
                seconds = CreateValueArray(Second);
            }
            else
            { // no value means every second
                seconds = CreateValueArray("0-59");
            }
        }

        public DateTime Next(DateTime refTime)
        {
            DateTime searchTime = refTime;

            // need to check the rest of this year and the next year
            // non leap years will loop 1 day extra
            int daysToLoop = 365 + (366 - searchTime.DayOfYear);

            int loopCount = 0;

            for (int i = 0; i < daysToLoop; i++)
            {
                loopCount++;

                // Compare month

                int foundValue = FindGreatEqual(months, searchTime.Month, -1);

                if (foundValue == -1)
                {
                    // goto first valid month next year at 00:00 and start over
                    searchTime = new DateTime(searchTime.Year + 1, months[0], 1); // Midnight ?
                    continue;
                }
                else
                {
                    // if foundvalue is further along, adjust time
                    if (foundValue > searchTime.Month)
                    {
                        searchTime = new DateTime(searchTime.Year, foundValue, 1);
                    }
                }

                // Check day components, both have to match if defined
                bool match = (daysDate != null) ? (new ArrayList(daysDate).Contains(searchTime.Day)) : true;


                if (match)
                    match = (daysWeekDay != null) ? (new ArrayList(daysWeekDay).Contains(searchTime.DayOfWeek)) : true;

                if (!match)
                {
                    // Goto next day, try again
                    searchTime = searchTime.AddTicks((TimeSpan.TicksPerDay - searchTime.TimeOfDay.Ticks + 1));
                    continue;
                }

                // Compare hours
                foundValue = FindGreatEqual(hours, searchTime.Hour, -1);

                if (foundValue == -1)
                {
                    // goto next day
                    searchTime = searchTime.AddTicks((TimeSpan.TicksPerDay - searchTime.TimeOfDay.Ticks + 1));
                    continue;
                }
                else
                {
                    // if foundvalue is further along, adjust time
                    if (foundValue > searchTime.Hour)
                        searchTime = new DateTime(searchTime.Year, searchTime.Month, searchTime.Day, foundValue, 0, 0);
                }


                // Compare minutes
                foundValue = FindGreatEqual(minutes, searchTime.Minute, -1);

                if (foundValue == -1)
                {
                    // goto first value next hour
                    searchTime = new DateTime(searchTime.Year, searchTime.Month, searchTime.Day, searchTime.Hour, minutes[0], 0);
                    searchTime = searchTime.AddHours(1);
                    continue;
                }
                else
                {
                    // if foundvalue is further along, adjust time
                    if (foundValue > searchTime.Minute)
                        searchTime = new DateTime(searchTime.Year, searchTime.Month, searchTime.Day, searchTime.Hour, foundValue, 0);
                }


                // Compare seconds
                foundValue = FindGreatEqual(seconds, searchTime.Second, -1);

                if (foundValue == -1)
                {
                    // goto first value next minute
                    searchTime = new DateTime(searchTime.Year, searchTime.Month, searchTime.Day, searchTime.Hour, searchTime.Minute, seconds[0]);
                    searchTime = searchTime.AddMinutes(1);
                    continue;
                }
                else
                {
                    // if foundvalue is further along, adjust time
                    if (foundValue > searchTime.Second)
                        searchTime = new DateTime(searchTime.Year, searchTime.Month, searchTime.Day, searchTime.Hour, searchTime.Minute, foundValue);
                }

                if (refTime == searchTime)
                {
                    searchTime = searchTime.AddSeconds(1);
                    continue;
                }

                // We have a winner !!
                break;
            }

            return (searchTime);
        }

        private int FindGreatEqual(int[] valueList, int compareValue, int notFoundValue)
        {
            int foundValue = notFoundValue;

            foreach (int intValue in valueList)
            {

                if (intValue >= compareValue)
                {
                    foundValue = intValue;
                    break;
                }
            }

            return foundValue;
        }
    }

    internal class Schedule
    {
        private string name;
        private ArrayList rules;

        public Schedule(string name)
        {
            rules = new ArrayList();
            this.name = name;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public void AddRule(string Second, string Minute, string Hour, string DayOfWeek,
            string DayOfMonth, string Month)
        {

            ScheduleRule rule = new ScheduleRule(Second,
                Minute, Hour, DayOfWeek, DayOfMonth, Month);

            rules.Add(rule);
        }

        public DateTime Next(DateTime refTime)
        {
            DateTime time;
            DateTime minTime = DateTime.MaxValue;
            refTime = refTime.AddMilliseconds(1000 - refTime.Millisecond);

            foreach (ScheduleRule rule in rules)
            {
                time = rule.Next(refTime);

                if (time < minTime)
                    minTime = time;
            }

            return (minTime);
        }
    }

    internal class Scheduler : ManagedJob
    {
        private Dictionary<string, ManagedSchedule> scheduleDictionary;
        private int multiplier;
        private DateTime zeroTime;
        private Thread thread;

        public Scheduler(string name, bool wait, JobArgumentCollection args)
            : base(name, false, args)
        {
            scheduleDictionary = new Dictionary<string, ManagedSchedule>();
            multiplier = 1;
        }

        public void SetMultiplier(int mult)
        {
            zeroTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            multiplier = mult;
        }

        public DateTime Now
        {
            get
            {
                if (multiplier > 1)
                {
                    TimeSpan ts = DateTime.Now.Subtract(zeroTime);
                    return (zeroTime.AddTicks(ts.Ticks * multiplier));
                }
                else
                    return (DateTime.Now);
            }
        }

        public void Sleep(TimeSpan ts)
        {
            if (multiplier > 1)
                ts = new TimeSpan((long)(Math.Floor((double)(ts.Ticks / multiplier))));

            Thread.Sleep(ts);
        }

        public void AddSchedule(ManagedSchedule schedule)
        {
            scheduleDictionary.Add(schedule.Schedule.Name, schedule);
        }

        public override void Execute()
        {
            thread = Thread.CurrentThread;

            Dictionary<string, ManagedSchedule> matureDictionary = new Dictionary<string, ManagedSchedule>();

            zeroTime = DateTime.Now;

            while (JobState != JobState.Stopping)
            {
                DateTime next = DateTime.MaxValue;
                matureDictionary.Clear();

                DateTime now = this.Now;

                Tracing.TraceEvent(TraceEventType.Verbose, 0, "Look for leader at {0}.", now);

                foreach (ManagedSchedule managedSchedule in scheduleDictionary.Values)
                {
                    DateTime time;

                    time = managedSchedule.Schedule.Next(now);

                    if (time <= next)
                    {
                        if (time != next)
                        {
                            matureDictionary.Clear();
                            next = time;

                            Tracing.TraceEvent(TraceEventType.Verbose, 0, "Find new leader {0} next time is {1}.", managedSchedule.Schedule.Name, next);

                        }
                        matureDictionary.Add(managedSchedule.Schedule.Name, managedSchedule);
                    }
                }

                if (matureDictionary.Count > 0)
                {
                    TimeSpan ts = next.Subtract(this.Now);

                    Tracing.TraceEvent(TraceEventType.Verbose, 0, "Sleeping for {0} seconds.", ts.TotalSeconds);

                    if (ts.TotalSeconds > 0)
                    {
                        this.Sleep(ts);
                    }
                }
                else
                    this.Sleep(new TimeSpan(0, 0, 10)); /* Nothing to schedule, sleep 10 seconds */

                if (JobState != JobState.Stopping)
                {
                    foreach (ManagedSchedule schedule in matureDictionary.Values)
                    {
                        foreach (string listener in schedule.listeners)
                        {
                            if (multiplier > 1)
                            {
                                Tracing.TraceEvent(TraceEventType.Verbose, 0, "{0} Scheduler signals {1}.", this.Now, listener);
                            }
                            else
                            {
                                Tracing.TraceEvent(TraceEventType.Verbose, 0, "Scheduler signals {0}.", listener);
                            }

                            OnSignal(listener);
                        }
                    }
                }
            }
        }

        public override void Stop()
        {
            //Abort thread since it might be sleeping
            thread.Abort();
        }

        public override void Init()
        {

        }
    }
}