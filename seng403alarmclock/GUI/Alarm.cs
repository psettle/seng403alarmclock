using seng403alarmclock.Model;
using System;
using System.Collections.Generic;

namespace seng403alarmclock.GUI {
    /// <summary>
    /// Internal representation of an alarm, add to it as necessairy
    /// 
    /// alarmTime is the the time the alarm will go off at
    /// 
    /// alarmName is a unique name for the alarm (the time it was created) so that we can id alarms later and remove them
    /// </summary>
    public class Alarm {

        private static TimeFetcher fetcher = new TimeFetcher();

        private DateTime alarmTime { get; set; }
        private String alarmName { get; set; }

        public Boolean IsRinging { get; set; }

        /// <summary>
        /// Indicates if the alarm repeats or not
        /// </summary>
        public bool IsRepeating { get; set; } = false;

        /// <summary>
        /// Indicates if the alarm is running a weekly cycle, instead of a daily cycle
        /// </summary>
        public bool IsWeekly { get; set; } = false;


        /// <summary>
        public Alarm()
        /// Create an alarm 5 minutes from now
        /// </summary>
        {
            IsRinging = false;
            IsWeekly = false;
            IsRepeating = false;
            alarmTime = DateTime.Now.AddMinutes(5);
            alarmName = "Created:" + DateTime.Now.ToString();
        }
 
        public Alarm(int hour, int minute, bool repeat, string audioFile, bool weekly, List<DayOfWeek> days) {
            if(weekly) {
                WeeklyCtor(hour, minute, repeat); 
            } else {
                NonWeeklyCtor(hour, minute, repeat);
            }
        }

        /// <summary>
        /// Contructor for an alarm that is weekly
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="repeat"></param>
        private void WeeklyCtor(int hour, int minute, bool repeat) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Constructor for an alarm that is not weekly
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        private void NonWeeklyCtor(int hour, int minute, bool repeats) {
            IsRepeating = repeats;
            IsWeekly = false;

            DateTime now = fetcher.getCurrentTime();

            int day;
            DateTime d = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0, DateTimeKind.Local);
            if (d > now) {
                day = now.Day;
            } else {  
                day = (now.AddDays(1)).Day;
            }
            alarmTime = new DateTime(now.Year, now.Month, day, hour, minute, 0, DateTimeKind.Local);
        }

        public DateTime GetAlarmTime() {
            //TODO: return a reasonable time
            return alarmTime;
        }


        /// <summary>
        /// Instructs the alarm to calculate its next arrival time
        /// </summary>
        /// <exception cref="NoMoreAlarmsException">
        /// Thrown if there are no more times this alarm should go off
        /// </exception>
        public void CalculateNextAlarmTime() {
            if(!IsWeekly && !IsRepeating) {
                throw new NoMoreAlarmsException();
            } else if(IsRepeating && !IsWeekly) {
                alarmTime = alarmTime.AddDays(1);
            }
        }

       

        /// <summary>
        /// 
        /// </summary>
        /// <returns>If this.IsWeekly, returns a list of days the alarm goes off on, otherwise returns null</returns>
        public List<DayOfWeek> GetWeekdays() {
            return new List<DayOfWeek>() {DayOfWeek.Monday };
        }




    }
}